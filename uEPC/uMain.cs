using System.Data;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Text;
using Microsoft.Data.Sqlite;

namespace uEPC
{
    public partial class uMain : Form
    {
        private readonly string _dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "epc_data.sqlite");

        public uMain()
        {
            InitializeComponent();
            InitializeDataGridView();
            bGenerateAll.Click += bGenerateAll_Click;
            bExport.Click += bExport_Click;
        }

        private void InitializeDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "No", HeaderText = "序号", DataPropertyName = "No" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "UPC", HeaderText = "UPC", DataPropertyName = "UPC" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "CompanyPrefix", HeaderText = "公司前缀", DataPropertyName = "CompanyPrefix" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "Partition", HeaderText = "分区", DataPropertyName = "Partition" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "Filter", HeaderText = "滤值", DataPropertyName = "Filter" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "Header", HeaderText = "标头", DataPropertyName = "Header" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "Serial", HeaderText = "序列号", DataPropertyName = "Serial" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "EPC", HeaderText = "EPC", DataPropertyName = "EPC" });
            dataGridView1.Columns.Add(new DataGridViewTextBoxColumn { Name = "Status", HeaderText = "状态", DataPropertyName = "Status" });
        }

        private string epcGenerate()
        {
            if (TryGenerateEpc(null, out string epc, out string message, out string status))
            {
                tEpcOutput.Text = epc;
                label7.Text = string.IsNullOrWhiteSpace(status) ? "生成成功" : status;
                return epc;
            }

            label7.Text = string.IsNullOrWhiteSpace(status) ? "生成失败" : status;
            if (!string.IsNullOrEmpty(message))
            {
                MessageBox.Show(message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return string.Empty;
        }

        private bool TryGenerateEpc(ulong? serialOverride, out string epc, out string message, out string status)
        {
            bool result = TryGenerateEpcCore(
                tHeader.Text,
                cbxFilter.Text,
                cbxPartition.Text,
                tCompanyPrefix.Text,
                tUPC.Text,
                serialOverride.HasValue ? serialOverride.Value : GetSerialFromUi(),
                textBox1.Text,
                out epc,
                out message,
                out status,
                out string normalizedUpc,
                out bool corrected);

            if (result && corrected)
            {
                tUPC.Text = normalizedUpc;
            }

            return result;
        }

        private bool TryGenerateEpcCore(string headerText, string filterText, string partitionText, string companyPrefixText, string upcText, ulong serial, string tailText, out string epc, out string message, out string status, out string normalizedUpc, out bool corrected)
        {
            epc = string.Empty;
            message = string.Empty;
            status = string.Empty;
            normalizedUpc = upcText;
            corrected = false;

            if (string.IsNullOrWhiteSpace(headerText) || string.IsNullOrWhiteSpace(filterText) || string.IsNullOrWhiteSpace(partitionText) || string.IsNullOrWhiteSpace(companyPrefixText) || string.IsNullOrWhiteSpace(upcText))
            {
                message = "请确保所有输入框均已填写（Header，滤值，分区，公司前缀，UPC）！";
                return false;
            }

            try
            {
                byte header = Convert.ToByte(headerText, 16);
                int filter = Convert.ToInt32(filterText, 2);
                int partition = int.Parse(partitionText);
                string upc = upcText.Trim();
                normalizedUpc = EpcEncoder.NormalizeUpcA(upc);
                corrected = !string.Equals(upc, normalizedUpc, StringComparison.Ordinal);
                string companyPrefix = companyPrefixText.Trim();

                int expectedDigits = EpcEncoder.GetExpectedCompanyPrefixDigits(partition);
                if (companyPrefix.Length != expectedDigits)
                {
                    status = $"GS1 提示：分区 {partition} 对应 {expectedDigits} 位 company prefix，当前为 {companyPrefix.Length} 位。";
                }
                else
                {
                    status = $"GS1 规则：分区 {partition} 对应 {expectedDigits} 位 company prefix。";
                }

                if (corrected)
                {
                    status += $" UPC 校验位已自动修正为 {normalizedUpc.Substring(11)}。";
                }

                epc = EpcEncoder.UpcToSgtin96(normalizedUpc, companyPrefix, partition, filter, serial, header);
                if (!string.IsNullOrWhiteSpace(tailText))
                {
                    epc = ApplyFixedLastHexDigit(epc, tailText);
                    status += $" 自定义尾数已固定为 {tailText.Trim()}。";
                }
                return true;
            }
            catch (Exception ex)
            {
                message = "生成EPC时出错: " + ex.Message;
                return false;
            }
        }

        private ulong GetSerialFromUi()
        {
            if (string.IsNullOrWhiteSpace(tSerialNumber.Text))
            {
                return 0UL;
            }

            ulong.TryParse(tSerialNumber.Text.Trim(), out ulong serial);
            return serial;
        }

        private void tUPC_Leave(object sender, EventArgs e)
        {
            try
            {
                string upc = tUPC.Text?.Trim();
                if (string.IsNullOrWhiteSpace(upc) || upc.Length < 11)
                    return;

                var candidates = EpcEncoder.ExtractCompanyPrefixCandidates(upc);
                if (candidates == null || candidates.Length == 0)
                    return;

                string existing = tCompanyPrefix.Text?.Trim();
                if (!string.IsNullOrWhiteSpace(existing))
                {
                    foreach (var c in candidates)
                    {
                        if (TrimLeadingZeros(c.CompanyPrefix) == TrimLeadingZeros(existing))
                        {
                            tCompanyPrefix.Text = c.CompanyPrefix.Substring(0,7);
                            //cbxPartition.Text = c.Partition.ToString();
                            //label7.Text = $"已匹配 companyPrefix ({c.CompanyPrefix}), partition={c.Partition}";
                            return;
                        }
                    }
                }

                var best = candidates[0];
                foreach (var c in candidates)
                {
                    if (c.CompanyPrefix.Length > best.CompanyPrefix.Length)
                        best = c;
                }
                tCompanyPrefix.Text = best.CompanyPrefix.Substring(0,7);
                //cbxPartition.Text = best.Partition.ToString();
                //label7.Text = $"自动提取 companyPrefix ({best.CompanyPrefix}), partition={best.Partition}";
            }
            catch (Exception ex)
            {
                label7.Text = "自动提取 companyPrefix 失败: " + ex.Message;
            }
        }

        private string TrimLeadingZeros(string s) => string.IsNullOrEmpty(s) ? s : s.TrimStart('0');

        private string ApplyFixedLastHexDigit(string epcHex, string tailText)  // 将 EPC 的最后一位十六进制字符替换为自定义尾数
        {
            if (string.IsNullOrWhiteSpace(tailText))
                return epcHex;

            if (!int.TryParse(tailText.Trim(), out int tailValue) || tailValue < 0 || tailValue > 15)
                throw new ArgumentException("自定义尾数必须是 0-15 的数字");

            if (string.IsNullOrWhiteSpace(epcHex) || epcHex.Length == 0)
                return epcHex;

            string fixedHex = epcHex.Substring(0, epcHex.Length - 1) + tailValue.ToString("X");
            return fixedHex.ToUpperInvariant();
        }

        private void bClear_Click(object sender, EventArgs e)
        {
            ClearStoredRecords();
            dataGridView1.DataSource = null;
            dataGridView1.Rows.Clear();
            tEpcOutput.Clear();
            label7.Text = "已清空结果";
        }

        private void bGenerate_Click(object sender, EventArgs e)
        {
            epcGenerate();
        }

        private async void bGenerateAll_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(tQty.Text.Trim(), out int count) || count <= 0 || tCompanyPrefix.Text.Trim().Length != 7)
            {
                MessageBox.Show("请补充基本资料，UPC-A编码，公司前缀等信息 ", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!ulong.TryParse(tSerialNumber.Text.Trim(), out ulong startSerial))
            {
                MessageBox.Show("序列号起始值必须是数字", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bGenerateAll.Enabled = false;
            bReloadFromDb.Enabled = false;
            bGenerate.Enabled = false;
            label7.Text = "正在后台生成 EPC，请稍候...";

            try
            {
                var records = await Task.Run(() =>
                {
                    var results = new List<(int No, string Upc, string CompanyPrefix, string Partition, string Filter, string Header, ulong Serial, string Epc, string Status)>();
                    for (int i = 0; i < count; i++)
                    {
                        ulong serial = startSerial + (ulong)i;
                        Invoke(new Action(() =>  // 在 UI 线程中调用 TryGenerateEpcCore
                        {
                            if (TryGenerateEpcCore(
                                tHeader.Text,
                                cbxFilter.Text,
                                cbxPartition.Text,
                                tCompanyPrefix.Text,
                                tUPC.Text,
                                serial,
                                textBox1.Text,
                                out string epc,
                                out string message,
                                out string status,
                                out string normalizedUpc,
                                out bool corrected))
                        {
                            results.Add((i + 1, normalizedUpc, tCompanyPrefix.Text.Trim(), cbxPartition.Text, cbxFilter.Text, tHeader.Text, serial, epc, string.IsNullOrWhiteSpace(status) ? "OK" : status));
                        }
                        else
                        {
                            results.Add((i + 1, tUPC.Text.Trim(), tCompanyPrefix.Text.Trim(), cbxPartition.Text, cbxFilter.Text, tHeader.Text, serial, string.Empty, message));
                        }
                        }));
                    }
                    return results;
                });

                SaveRecordsToDatabase(records);
                LoadRecordsFromDatabase();
                label7.Text = $"已批量生成 {count} 条 EPC，起始序列号为 {startSerial}";
            }
            catch (Exception ex)
            {
                label7.Text = "批量生成失败: " + ex.Message;
                MessageBox.Show("批量生成失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                bGenerateAll.Enabled = true;
                bReloadFromDb.Enabled = true;
                bGenerate.Enabled = true;
            }
        }

        private void bReloadFromDb_Click(object sender, EventArgs e)
        {
            LoadRecordsFromDatabase();
            label7.Text = "已从 SQLite 重新载入数据";
        }

        private void bExport_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                MessageBox.Show("当前没有可导出的 EPC 结果", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using var dialog = new SaveFileDialog();
            dialog.Filter = "CSV 文件|*.csv|Excel 文件|*.xlsx|所有文件|*.*";
            dialog.FileName = "epc_export";
            if (dialog.ShowDialog(this) != DialogResult.OK)
                return;

            string path = dialog.FileName;
            string extension = Path.GetExtension(path).ToLowerInvariant();
            try
            {
                if (extension == ".csv")
                {
                    ExportToCsv(path);
                }
                else if (extension == ".xlsx")
                {
                    ExportToExcel(path);
                }
                else
                {
                    MessageBox.Show("仅支持导出为 .csv 或 .xlsx", "不支持的格式", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                MessageBox.Show($"导出成功：{path}", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出失败: " + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void EnsureDatabaseInitialized()
        {
            if (File.Exists(_dbPath))
            {
                return;
            }

            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS EpcRecords (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    No INTEGER NOT NULL,
                    UPC TEXT NOT NULL,
                    CompanyPrefix TEXT NOT NULL,
                    Partition TEXT NOT NULL,
                    Filter TEXT NOT NULL,
                    Header TEXT NOT NULL,
                    Serial INTEGER NOT NULL,
                    EPC TEXT NOT NULL,
                    Status TEXT NOT NULL,
                    CreatedAt TEXT NOT NULL
                );
            ";
            command.ExecuteNonQuery();
        }

        private void SaveRecordsToDatabase(IEnumerable<(int No, string Upc, string CompanyPrefix, string Partition, string Filter, string Header, ulong Serial, string Epc, string Status)> records)
        {
            EnsureDatabaseInitialized();
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();
            using var transaction = connection.BeginTransaction();

            var deleteCommand = connection.CreateCommand();
            deleteCommand.CommandText = "DELETE FROM EpcRecords";
            deleteCommand.ExecuteNonQuery();

            var insertCommand = connection.CreateCommand();
            insertCommand.CommandText = @"
                INSERT INTO EpcRecords (No, UPC, CompanyPrefix, Partition, Filter, Header, Serial, EPC, Status, CreatedAt)
                VALUES (@No, @UPC, @CompanyPrefix, @Partition, @Filter, @Header, @Serial, @EPC, @Status, @CreatedAt);
            ";
            insertCommand.Parameters.Add("@No", SqliteType.Integer);
            insertCommand.Parameters.Add("@UPC", SqliteType.Text);
            insertCommand.Parameters.Add("@CompanyPrefix", SqliteType.Text);
            insertCommand.Parameters.Add("@Partition", SqliteType.Text);
            insertCommand.Parameters.Add("@Filter", SqliteType.Text);
            insertCommand.Parameters.Add("@Header", SqliteType.Text);
            insertCommand.Parameters.Add("@Serial", SqliteType.Integer);
            insertCommand.Parameters.Add("@EPC", SqliteType.Text);
            insertCommand.Parameters.Add("@Status", SqliteType.Text);
            insertCommand.Parameters.Add("@CreatedAt", SqliteType.Text);

            foreach (var record in records)
            {
                insertCommand.Parameters["@No"].Value = record.No;
                insertCommand.Parameters["@UPC"].Value = record.Upc;
                insertCommand.Parameters["@CompanyPrefix"].Value = record.CompanyPrefix;
                insertCommand.Parameters["@Partition"].Value = record.Partition;
                insertCommand.Parameters["@Filter"].Value = record.Filter;
                insertCommand.Parameters["@Header"].Value = record.Header;
                insertCommand.Parameters["@Serial"].Value = (long)record.Serial;
                insertCommand.Parameters["@EPC"].Value = record.Epc;
                insertCommand.Parameters["@Status"].Value = record.Status;
                insertCommand.Parameters["@CreatedAt"].Value = DateTime.UtcNow.ToString("O");
                insertCommand.ExecuteNonQuery();
            }

            transaction.Commit();
        }

        private void LoadRecordsFromDatabase()
        {
            EnsureDatabaseInitialized();
            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "SELECT No, UPC, CompanyPrefix, Partition, Filter, Header, Serial, EPC, Status FROM EpcRecords ORDER BY Id ASC";

            using var reader = command.ExecuteReader();
            var table = new DataTable();
            table.Columns.Add("No", typeof(int));
            table.Columns.Add("UPC", typeof(string));
            table.Columns.Add("CompanyPrefix", typeof(string));
            table.Columns.Add("Partition", typeof(string));
            table.Columns.Add("Filter", typeof(string));
            table.Columns.Add("Header", typeof(string));
            table.Columns.Add("Serial", typeof(ulong));
            table.Columns.Add("EPC", typeof(string));
            table.Columns.Add("Status", typeof(string));

            while (reader.Read())
            {
                table.Rows.Add(
                    reader.GetInt32(0),
                    reader.GetString(1),
                    reader.GetString(2),
                    reader.GetString(3),
                    reader.GetString(4),
                    reader.GetString(5),
                    (ulong)reader.GetInt64(6),
                    reader.GetString(7),
                    reader.GetString(8));
            }

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = table;
            dataGridView1.AutoResizeColumns();
        }

        private void ClearStoredRecords()
        {
            if (!File.Exists(_dbPath))
                return;

            using var connection = new SqliteConnection($"Data Source={_dbPath}");
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = "DELETE FROM EpcRecords";
            command.ExecuteNonQuery();
        }

        private void ExportToCsv(string path)
        {
            var rows = new List<string[]>();
            rows.Add(new[] { "序号", "UPC", "公司前缀", "分区", "滤值", "标头", "序列号", "EPC" });
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow)
                    continue;
                rows.Add(new[]
                {
                    GetCellValue(row, "No"),
                    GetCellValue(row, "UPC"),
                    GetCellValue(row, "CompanyPrefix"),
                    GetCellValue(row, "Partition"),
                    GetCellValue(row, "Filter"),
                    GetCellValue(row, "Header"),
                    GetCellValue(row, "Serial"),
                    GetCellValue(row, "EPC"),
                });
            }

            var sb = new StringBuilder();
            foreach (var row in rows)
            {
                sb.AppendLine(string.Join(",", row.Select(EscapeCsvField)));
            }
            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
        }

        private void ExportToExcel(string path)
        {
            var rows = new List<string[]>();
            rows.Add(new[] { "序号", "UPC", "公司前缀", "分区", "滤值", "标头", "序列号", "EPC" });
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.IsNewRow)
                    continue;
                rows.Add(new[]
                {
                    GetCellValue(row, "No"),
                    GetCellValue(row, "UPC"),
                    GetCellValue(row, "CompanyPrefix"),
                    GetCellValue(row, "Partition"),
                    GetCellValue(row, "Filter"),
                    GetCellValue(row, "Header"),
                    GetCellValue(row, "Serial"),
                    GetCellValue(row, "EPC"),
                });
            }

            using var stream = new MemoryStream();
            using (var archive = new ZipArchive(stream, ZipArchiveMode.Create, true))
            {
                WriteEntry(archive, "[Content_Types].xml", BuildContentTypesXml());
                WriteEntry(archive, "_rels/.rels", BuildRootRelsXml());
                WriteEntry(archive, "xl/workbook.xml", BuildWorkbookXml());
                WriteEntry(archive, "xl/_rels/workbook.xml.rels", BuildWorkbookRelsXml());
                WriteEntry(archive, "xl/styles.xml", BuildStylesXml());
                WriteEntry(archive, "xl/worksheets/sheet1.xml", BuildSheetXml(rows));
            }

            File.WriteAllBytes(path, stream.ToArray());
        }

        private string GetCellValue(DataGridViewRow row, string columnName)
        {
            if (row.Cells[columnName] == null || row.Cells[columnName].Value == null)
                return string.Empty;
            return row.Cells[columnName].Value.ToString() ?? string.Empty;
        }

        private string EscapeCsvField(string value)
        {
            if (value.Contains(',') || value.Contains('"') || value.Contains('\n') || value.Contains('\r'))
            {
                return "\"" + value.Replace("\"", "\"\"") + "\"";
            }
            return value;
        }

        private static void WriteEntry(ZipArchive archive, string entryName, string content)
        {
            var entry = archive.CreateEntry(entryName);
            using var writer = new StreamWriter(entry.Open(), new UTF8Encoding(false));
            writer.Write(content);
        }

        private static string BuildContentTypesXml()
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                   "<Types xmlns=\"http://schemas.openxmlformats.org/package/2006/content-types\">" +
                   "<Default Extension=\"rels\" ContentType=\"application/vnd.openxmlformats-package.relationships+xml\"/>" +
                   "<Default Extension=\"xml\" ContentType=\"application/xml\"/>" +
                   "<Override PartName=\"/xl/workbook.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml\"/>" +
                   "<Override PartName=\"/xl/worksheets/sheet1.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml\"/>" +
                   "<Override PartName=\"/xl/styles.xml\" ContentType=\"application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml\"/>" +
                   "</Types>";
        }

        private static string BuildRootRelsXml()
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                   "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">" +
                   "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument\" Target=\"xl/workbook.xml\"/>" +
                   "</Relationships>";
        }

        private static string BuildWorkbookXml()
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                   "<workbook xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">" +
                   "<sheets><sheet name=\"EPC\" sheetId=\"1\" r:id=\"rId1\"/></sheets>" +
                   "</workbook>";
        }

        private static string BuildWorkbookRelsXml()
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                   "<Relationships xmlns=\"http://schemas.openxmlformats.org/package/2006/relationships\">" +
                   "<Relationship Id=\"rId1\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet\" Target=\"worksheets/sheet1.xml\"/>" +
                   "<Relationship Id=\"rId2\" Type=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships/styles\" Target=\"styles.xml\"/>" +
                   "</Relationships>";
        }

        private static string BuildStylesXml()
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>" +
                   "<styleSheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\">" +
                   "<fonts count=\"1\"><font><sz val=\"11\"/><name val=\"Calibri\"/></font></fonts>" +
                   "<fills count=\"1\"><fill><patternFill patternType=\"none\"/></fill></fills>" +
                   "<borders count=\"1\"><border/></borders>" +
                   "<cellStyleXfs count=\"1\"><xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\"/></cellStyleXfs>" +
                   "<cellXfs count=\"1\"><xf numFmtId=\"0\" fontId=\"0\" fillId=\"0\" borderId=\"0\"/></cellXfs>" +
                   "<cellStyles count=\"1\"><cellStyle name=\"Normal\" xfId=\"0\" builtinId=\"0\"/></cellStyles>" +
                   "</styleSheet>";
        }

        private static string BuildSheetXml(List<string[]> rows)
        {
            var sb = new StringBuilder();
            sb.Append("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?>");
            sb.Append("<worksheet xmlns=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\" xmlns:r=\"http://schemas.openxmlformats.org/officeDocument/2006/relationships\">");
            sb.Append("<sheetData>");
            for (int rowIndex = 0; rowIndex < rows.Count; rowIndex++)
            {
                sb.Append($"<row r=\"{rowIndex + 1}\">");
                for (int colIndex = 0; colIndex < rows[rowIndex].Length; colIndex++)
                {
                    string cellRef = GetExcelColumnName(colIndex + 1) + (rowIndex + 1);
                    string value = rows[rowIndex][colIndex];
                    sb.Append($"<c r=\"{cellRef}\" t=\"inlineStr\"><is><t>{EscapeXml(value)}</t></is></c>");
                }
                sb.Append("</row>");
            }
            sb.Append("</sheetData></worksheet>");
            return sb.ToString();
        }

        private static string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber;
            string columnName = string.Empty;
            while (dividend > 0)
            {
                int modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar('A' + modulo) + columnName;
                dividend = (dividend - modulo - 1) / 26;
            }
            return columnName;
        }

        private static string EscapeXml(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;
            return text.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;");
        }

        private void tDeEPC_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tDeEPC.Text))
            {
                label8.Text = "请输入要解码的 EPC";
                return;
            }
            else
            {
                try
                {
                    var decoded = EpcDecoder.DecodeSgtin96(tDeEPC.Text.Trim());
                    tDeHeader.Text = decoded.Header.ToString("X2");
                    tDeFilter.Text = decoded.Filter.ToString();
                    tDePartition.Text = decoded.Partition.ToString();
                    tDeCompanyPrefix.Text = decoded.CompanyPrefix;
                    tDeIndicator.Text = decoded.Indicator.ToString();
                    tDeItemReference.Text = decoded.ItemReference;
                    tDeGtinNoCheck.Text = decoded.Gtin14.Substring(0, 13);
                    tDeGtin.Text = decoded.Gtin14;
                }
                catch (Exception ex)
                {
                    label8.Text = "解码失败: " + ex.Message;
                }
            }
        }

    }
}
