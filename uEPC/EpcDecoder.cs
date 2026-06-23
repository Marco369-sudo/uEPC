using System;
using System.Globalization;
using System.Numerics;

namespace uEPC
{
    public sealed class DecodedSgtin96
    {
        public byte Header { get; init; }
        public int Filter { get; init; }
        public int Partition { get; init; }
        public string CompanyPrefix { get; init; } = string.Empty;
        public string Indicator { get; init; } = string.Empty;
        public string ItemReference { get; init; } = string.Empty;
        public ulong Serial { get; init; }
        public string GtinNoCheck { get; init; } = string.Empty;
        public string Gtin14 { get; init; } = string.Empty;
    }

    public static class EpcDecoder
    {
        private static readonly int[] CompanyBits = { 40, 37, 34, 30, 27, 24, 20 };
        private static readonly int[] ItemBits = { 4, 7, 10, 14, 17, 20, 24 };
        private static readonly int[] CompanyDigits = { 12, 11, 10, 9, 8, 7, 6 };
        private static readonly int[] ItemDigits = { 0, 1, 2, 3, 4, 5, 6 };

        /// <summary>
        /// 将 SGTIN-96 EPC（24 位十六进制字符串）解码为结构化信息。
        /// </summary>
        public static DecodedSgtin96 DecodeSgtin96(string epcHex)
        {
            if (string.IsNullOrWhiteSpace(epcHex))
                throw new ArgumentException("EPC 不能为空", nameof(epcHex));

            string normalized = NormalizeHex(epcHex);
            if (normalized.Length != 24)
                throw new ArgumentException("SGTIN-96 EPC 必须是 24 个十六进制字符", nameof(epcHex));

            BigInteger value;
            try
            {
                value = BigInteger.Parse(normalized, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture);
            }
            catch (FormatException ex)
            {
                throw new ArgumentException("EPC 包含非法的十六进制字符", nameof(epcHex), ex);
            }

            if (value.Sign < 0)
                throw new ArgumentException("EPC 值非法", nameof(epcHex));

            int remainingBits = 96;
            int headerValue = (int)(value >> (remainingBits - 8));
            remainingBits -= 8;
            value &= GetMask(remainingBits);

            int filterValue = (int)(value >> (remainingBits - 3));
            remainingBits -= 3;
            value &= GetMask(remainingBits);

            int partition = (int)(value >> (remainingBits - 3));
            remainingBits -= 3;
            value &= GetMask(remainingBits);

            if (partition < 0 || partition > 6)
                throw new ArgumentException("EPC 中的分区值非法", nameof(epcHex));

            int cpBits = CompanyBits[partition];
            int itBits = ItemBits[partition];

            BigInteger companyValue = value >> (remainingBits - cpBits);
            remainingBits -= cpBits;
            value &= GetMask(remainingBits);

            BigInteger itemValue = value >> (remainingBits - itBits);
            remainingBits -= itBits;
            value &= GetMask(remainingBits);

            if (remainingBits != 38)
                throw new InvalidOperationException("解码位长度与 SGTIN-96 结构不匹配");

            BigInteger serialValue = value;
            if (serialValue > ulong.MaxValue)
                throw new InvalidOperationException("序列号超出 64 位范围");

            ulong serial = (ulong)serialValue;

            string companyPrefix = companyValue.ToString(CultureInfo.InvariantCulture).PadLeft(CompanyDigits[partition], '0');
            string itemCombined = itemValue.ToString(CultureInfo.InvariantCulture).PadLeft(ItemDigits[partition] + 1, '0');
            string indicator = itemCombined.Substring(0, 1);
            string itemReference = itemCombined.Length > 1 ? itemCombined.Substring(1) : string.Empty;
            string gtinNoCheck = indicator + companyPrefix + itemReference;
            string gtin14 = gtinNoCheck + CalculateGtinCheckDigit(gtinNoCheck);

            return new DecodedSgtin96
            {
                Header = (byte)headerValue,
                Filter = filterValue,
                Partition = partition,
                CompanyPrefix = companyPrefix,
                Indicator = indicator,
                ItemReference = itemReference,
                Serial = serial,
                GtinNoCheck = gtinNoCheck,
                Gtin14 = gtin14,
            };
        }

        private static BigInteger GetMask(int bits)
        {
            if (bits <= 0)
                return BigInteger.Zero;
            return (BigInteger.One << bits) - 1;
        }

        private static string NormalizeHex(string epcHex)
        {
            string normalized = epcHex.Trim();
            if (normalized.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                normalized = normalized.Substring(2);

            normalized = normalized.Replace("-", string.Empty, StringComparison.Ordinal)
                                   .Replace(" ", string.Empty, StringComparison.Ordinal)
                                   .Replace("\t", string.Empty, StringComparison.Ordinal)
                                   .Replace("\r", string.Empty, StringComparison.Ordinal)
                                   .Replace("\n", string.Empty, StringComparison.Ordinal);

            return normalized.ToUpperInvariant();
        }

        private static int CalculateGtinCheckDigit(string gtinNoCheck)
        {
            if (gtinNoCheck.Length < 11)
                throw new ArgumentException("GTIN/UPC 的数据位数不足，无法计算校验位", nameof(gtinNoCheck));

            int sum = 0;
            for (int i = 0; i < gtinNoCheck.Length; i++)
            {
                int digit = gtinNoCheck[i] - '0';
                // GTIN/UPC 校验位：从左到右，奇数位乘 3，偶数位乘 1
                sum += (i % 2 == 0) ? digit * 3 : digit;
            }

            int remainder = sum % 10;
            return (10 - remainder) % 10;
        }
    }
}
