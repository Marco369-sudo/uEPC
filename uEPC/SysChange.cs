using DryIoc.ImTools;
using System;
using System.Collections.Generic;
using System.Text;

namespace uEPC
{
    public class SysChange
    {
        private string BitStrToHextStr(string binaryString)  // 二进制字符串转换16进制字符串
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < binaryString.Length; i += 4)
            {
                string fourBits = binaryString.Substring(i, 4);
                int decimalValue = Convert.ToInt32(fourBits, 2);
                result.Append(decimalValue.ToString("X")); // 转换为16进制并追加到结果中
            }
            return result.ToString();
        }

        private string HexStrToBinStr(string hexString)  // 16进制字符串转换二进制字符串
        {
            StringBuilder result = new StringBuilder();
            foreach (char hexChar in hexString)
            {
                int decimalValue = Convert.ToInt32(hexChar.ToString(), 16);
                result.Append(Convert.ToString(decimalValue, 2).PadLeft(4, '0')); // 转换为二进制并补齐4位
            }
            return result.ToString();
        }

        private string HexStrToDecStr(string hexString)  // 16进制字符串转换10进制字符串
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hexString.Length; i += 2)
            {
                string twoHexChars = hexString.Substring(i, 2);
                int decimalValue = Convert.ToInt32(twoHexChars, 16);
                result.Append(decimalValue.ToString()); // 转换为10进制并追加到结果中
            }
            return result.ToString();
        }

        private string DecStrToHexStr(string decimalString)  // 10进制字符串转换16进制字符串
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < decimalString.Length; i += 2)
            {
                string twoDecimalChars = decimalString.Substring(i, 2);
                int decimalValue = Convert.ToInt32(twoDecimalChars);
                result.Append(decimalValue.ToString("X")); // 转换为16进制并追加到结果中
            }
            return result.ToString();
        }

        private string DecStrToBitStr(string decimalString)  // 10进制字符串转换二进制字符串
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < decimalString.Length; i += 2)
            {
                string twoDecimalChars = decimalString.Substring(i, 2);
                int decimalValue = Convert.ToInt32(twoDecimalChars);
                result.Append(Convert.ToString(decimalValue, 2).PadLeft(8, '0')); // 转换为二进制并补齐8位
            }
            return result.ToString();
        }

        private string BitStrToDecStr(string binaryString)  // 二进制字符串转换10进制字符串
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < binaryString.Length; i += 8)
            {
                string eightBits = binaryString.Substring(i, 8);
                int decimalValue = Convert.ToInt32(eightBits, 2);
                result.Append(decimalValue.ToString()); // 转换为10进制并追加到结果中
            }
            return result.ToString();
        }

        private string StrToHexStr(string inputString)  // 字符串转换16进制字符串
        {
            StringBuilder result = new StringBuilder();
            foreach (char c in inputString)
            {
                result.Append(((int)c).ToString("X2")); // 转换为16进制并追加到结果中
            }
            return result.ToString();
        }

        private string HexStrToStr(string hexString)  // 16进制字符串转换字符串
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hexString.Length; i += 2)
            {
                string twoHexChars = hexString.Substring(i, 2);
                int decimalValue = Convert.ToInt32(twoHexChars, 16);
                result.Append((char)decimalValue); // 转换为字符并追加到结果中
            }
            return result.ToString();
        }

        private string StrToBitStr(string inputString)  // 字符串转换二进制字符串
        {
            StringBuilder result = new StringBuilder();
            foreach (char c in inputString)
            {
                result.Append(Convert.ToString((int)c, 2).PadLeft(8, '0')); // 转换为二进制并补齐8位
            }
            return result.ToString();
        }

        private string BitStrToStr(string binaryString)  // 二进制字符串转换字符串
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < binaryString.Length; i += 8)
            {
                string eightBits = binaryString.Substring(i, 8);
                int decimalValue = Convert.ToInt32(eightBits, 2);
                result.Append((char)decimalValue); // 转换为字符并追加到结果中
            }
            return result.ToString();
        }

        private string StrToDecStr(string inputString)  // 字符串转换10进制字符串
        {
            StringBuilder result = new StringBuilder();
            foreach (char c in inputString)
            {
                result.Append(((int)c).ToString()); // 转换为10进制并追加到结果中
            }
            return result.ToString();
        }

        private string DecStrToStr(string decimalString)  // 10进制字符串转换字符串
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < decimalString.Length; i += 2)
            {
                string twoDecimalChars = decimalString.Substring(i, 2);
                int decimalValue = Convert.ToInt32(twoDecimalChars);
                result.Append((char)decimalValue); // 转换为字符并追加到结果中
            }
            return result.ToString();
        }


        private string DecStrToAnyBaseStr(string decimalString, int targetBase) //10进制转换任意进制
        {
            StringBuilder result = new StringBuilder();
            foreach (char c in decimalString)
            {
                int decimalValue = Convert.ToInt32(c.ToString());
                result.Append(Convert.ToString(decimalValue, targetBase).PadLeft(4, '0')); // 转换为目标进制并补齐4位
            }
            return result.ToString();
        }

        private string AnyBaseStrToDecStr(string anyBaseString, int sourceBase) //任意进制转换10进制
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < anyBaseString.Length; i += 4)
            {
                string fourChars = anyBaseString.Substring(i, 4);
                int decimalValue = Convert.ToInt32(fourChars, sourceBase);
                result.Append(decimalValue.ToString()); // 转换为10进制并追加到结果中
            }
            return result.ToString();
        }

        private string OtoHexStr(string octalString)  // 八进制字符串转换16进制字符串
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < octalString.Length; i += 3)
            {
                string threeOctalChars = octalString.Substring(i, 3);
                int decimalValue = Convert.ToInt32(threeOctalChars, 8);
                result.Append(decimalValue.ToString("X")); // 转换为16进制并追加到结果中
            }
            return result.ToString();
        }

        private string HtoOctalStr(string hexString)  // 16进制字符串转换八进制字符串
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < hexString.Length; i += 2)
            {
                string twoHexChars = hexString.Substring(i, 2);
                int decimalValue = Convert.ToInt32(twoHexChars, 16);
                result.Append(Convert.ToString(decimalValue, 8).PadLeft(3, '0')); // 转换为八进制并补齐3位
            }
            return result.ToString();
        }
        private string OtoDecStr(string octalString)  // 八进制字符串转换10进制字符串
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < octalString.Length; i += 3)
            {
                string threeOctalChars = octalString.Substring(i, 3);
                int decimalValue = Convert.ToInt32(threeOctalChars, 8);
                result.Append(decimalValue.ToString()); // 转换为10进制并追加到结果中
            }
            return result.ToString();
        }

        private string DtoOctalStr(string decimalString)  // 10进制字符串转换八进制字符串
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < decimalString.Length; i += 2)
            {
                string twoDecimalChars = decimalString.Substring(i, 2);
                int decimalValue = Convert.ToInt32(twoDecimalChars);
                result.Append(Convert.ToString(decimalValue, 8).PadLeft(3, '0')); // 转换为八进制并补齐3位
            }
            return result.ToString();
        }

        private float HexStrToFloat(string hexString)  // 16进制字符串转换浮点数
        {
            uint intValue = Convert.ToUInt32(hexString, 16);
            byte[] bytes = BitConverter.GetBytes(intValue);
            return BitConverter.ToSingle(bytes, 0);
        }

        private string FloatToHexStr(float floatValue)  // 浮点数转换16进制字符串
        {
            byte[] bytes = BitConverter.GetBytes(floatValue);
            uint intValue = BitConverter.ToUInt32(bytes, 0);
            return intValue.ToString("X");
        }

    }
}
