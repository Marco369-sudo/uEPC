using System;
using System.Linq;
using System.Numerics;

namespace uEPC
{
    public static class EpcEncoder
    {
        // Partition tables per GS1 SGTIN
        private static readonly int[] CompanyBits = { 40, 37, 34, 30, 27, 24, 20 };
        private static readonly int[] ItemBits =    { 4,  7,  10, 14, 17, 20, 24 };
        private static readonly int[] CompanyDigits = { 12, 11, 10, 9, 8, 7, 6 };
        // Item digits should satisfy: indicator(1) + companyDigits + itemDigits = 13
        private static readonly int[] ItemDigits =    { 0,  1,  2,  3, 4, 5, 6 };

        public static int GetExpectedCompanyPrefixDigits(int partition)
        {
            if (partition < 0 || partition > 6)
                throw new ArgumentOutOfRangeException(nameof(partition), "partition 必须在 0-6 之间");
            return CompanyDigits[partition];
        }

        public static string NormalizeUpcA(string upc12)
        {
            if (string.IsNullOrWhiteSpace(upc12))
                throw new ArgumentException("UPC-A 不能为空", nameof(upc12));

            string digits = new string(upc12.Where(char.IsDigit).ToArray());
            if (digits.Length == 11)
            {
                digits += CalculateUpcCheckDigit(digits);
            }
            else if (digits.Length == 12)
            {
                string body = digits.Substring(0, 11);
                digits = body + CalculateUpcCheckDigit(body);
            }
            else
            {
                throw new ArgumentException("UPC-A 必须是 11 位或 12 位数字", nameof(upc12));
            }

            return digits;
        }

        public static bool IsUpcCheckDigitValid(string upc12)
        {
            if (string.IsNullOrWhiteSpace(upc12))
                return false;

            string digits = new string(upc12.Where(char.IsDigit).ToArray());
            if (digits.Length != 12)
                return false;

            string body = digits.Substring(0, 11);
            int expected = CalculateUpcCheckDigit(body);
            return digits[11] - '0' == expected;
        }

        public static int CalculateUpcCheckDigit(string upcBody)
        {
            if (string.IsNullOrWhiteSpace(upcBody))
                throw new ArgumentException("UPC-A 主体不能为空", nameof(upcBody));

            string digits = new string(upcBody.Where(char.IsDigit).ToArray());
            if (digits.Length != 11)
                throw new ArgumentException("UPC-A 主体必须是 11 位数字", nameof(upcBody));

            int sum = 0;
            for (int i = 0; i < digits.Length; i++)
            {
                int digit = digits[i] - '0';
                sum += (i % 2 == 0) ? digit * 3 : digit;
            }

            int remainder = sum % 10;
            return (10 - remainder) % 10;
        }

        /// <summary>
        /// 将 UPC-A (12 位) 转换为 SGTIN-96 EPC（返回 24 字节的十六进制字符串，96 位）
        /// 参数说明：companyPrefix 必须为 6 到 12 位数字，partition 为 0-6 且需与 companyPrefix 长度匹配。
        /// serial 是实例序列号 (0 .. 2^38-1)。header 默认为 0x30。
        /// </summary>
        public static string UpcToSgtin96(string upc12, string companyPrefix, int partition, int filter = 0, ulong serial = 0UL, byte header = 0x30)
        {
            string normalizedUpc = NormalizeUpcA(upc12);
            if (string.IsNullOrWhiteSpace(companyPrefix)) throw new ArgumentException("companyPrefix 不能为空", nameof(companyPrefix));
            if (partition < 0 || partition > 6) throw new ArgumentOutOfRangeException(nameof(partition));
            if (!ulong.TryParse(serial.ToString(), out _)) throw new ArgumentException("serial 非法", nameof(serial));
            if (serial >= (1UL << 38)) throw new ArgumentOutOfRangeException(nameof(serial), "serial 必须小于 2^38");

            int cpDigits = companyPrefix.Length;
            if (cpDigits < 6 || cpDigits > 12) throw new ArgumentException("companyPrefix 长度必须在 6 到 12 之间", nameof(companyPrefix));

            int expectedPartition = Array.IndexOf(CompanyDigits, cpDigits);
            if (expectedPartition == -1) throw new ArgumentException("companyPrefix 长度不在允许的表中", nameof(companyPrefix));
            if (expectedPartition != partition)
            {
                partition = expectedPartition;
            }

            string gtin14 = normalizedUpc.PadLeft(14, '0');
            if (gtin14.Length < 13) throw new ArgumentException("GTIN 数据长度不足，无法解析");
            string gtinNoCheck = gtin14.Substring(0, 13);

            string indicator = gtinNoCheck.Substring(0, 1);
            if (1 + cpDigits > gtinNoCheck.Length) throw new ArgumentException("GTIN 中无法提取公司前缀，索引超出范围");
            string cpFromGtin = gtinNoCheck.Substring(1, cpDigits);
            string companyPrefixPadded = companyPrefix.PadLeft(cpDigits, '0');
            if (!cpFromGtin.Equals(companyPrefixPadded, StringComparison.Ordinal))
                throw new ArgumentException($"提供的 companyPrefix 与 UPC/GTIN 中的公司前缀不匹配: GTIN中='{cpFromGtin}', 提供='{companyPrefixPadded}'");

            int itemDigits = ItemDigits[partition];
            if (1 + cpDigits + itemDigits > gtinNoCheck.Length) throw new ArgumentException("GTIN 中无法提取 item reference，索引超出范围，请检查 UPC 与 partition 设置");
            string itemRef = gtinNoCheck.Substring(1 + cpDigits, itemDigits);
            string itemRefCombined = indicator + itemRef;

            if (!ulong.TryParse(cpFromGtin, out ulong cpValue)) throw new ArgumentException("companyPrefix 解析为数字失败");
            if (!ulong.TryParse(itemRefCombined, out ulong itemValue)) throw new ArgumentException("item reference 解析为数字失败");

            int cpBits = CompanyBits[partition];
            int itBits = ItemBits[partition];

            if (cpValue >= (1UL << cpBits)) throw new ArgumentOutOfRangeException(nameof(companyPrefix), "companyPrefix 值过大，无法放入指定比特位");
            if (itemValue >= (1UL << itBits)) throw new ArgumentOutOfRangeException(nameof(itemRef), "item reference 值过大，无法放入指定比特位");

            BigInteger value = new BigInteger(0);
            value = (value << 8) | header;
            value = (value << 3) | (filter & 0x7);
            value = (value << 3) | (partition & 0x7);
            value = (value << cpBits) | cpValue;
            value = (value << itBits) | itemValue;
            value = (value << 38) | new BigInteger(serial);

            string hex = value.ToString("X");
            if (hex.Length > 24) throw new InvalidOperationException("生成的 EPC 超过 96 位");
            hex = hex.PadLeft(24, '0');
            return hex.ToUpperInvariant();
        }

        public static (int Partition, string CompanyPrefix)[] ExtractCompanyPrefixCandidates(string upc12)
        {
            string normalizedUpc = NormalizeUpcA(upc12);
            string gtin14 = normalizedUpc.PadLeft(14, '0');
            if (gtin14.Length < 13) throw new ArgumentException("GTIN 数据长度不足，无法解析");
            string gtinNoCheck = gtin14.Substring(0, 13);

            var list = new System.Collections.Generic.List<(int, string)>();
            for (int p = 0; p <= 6; p++)
            {
                int cpDigits = CompanyDigits[p];
                int itemDigits = ItemDigits[p];
                if (1 + cpDigits + itemDigits > gtinNoCheck.Length) continue;
                string cp = gtinNoCheck.Substring(1, cpDigits);
                list.Add((p, cp));
            }
            return list.ToArray();
        }
    }
}
