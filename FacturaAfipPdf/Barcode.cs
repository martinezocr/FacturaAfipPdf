using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;

namespace Afip
{
    public static class Barcode
    {
        private const byte CHAR_ZERO = 48;

        /// <summary>
        /// Calcula el dígito verificador del código de barras
        /// </summary>
        /// <param name="cod">número del código de barras</param>
        /// <returns>dígito verificador del código de barras</returns>
        public static byte CalculateVerificationDigit(string cod)
        {
            int num1 = 0;
            for (int index = 0; index < cod.Length; index += 2)
                num1 += (int)cod[index] - 48;
            int num2 = num1 * 3;
            for (int index = 1; index < cod.Length; index += 2)
                num2 += (int)cod[index] - 48;
            byte num3 = 0;
            while (num2 / 10 * 10 != num2)
            {
                ++num2;
                ++num3;
            }
            return num3;
        }
        /// <summary>
        /// Genera el codigo de barras
        /// </summary>
        /// <param name="data">codigo</param>
        /// <param name="width">largo</param>
        /// <param name="height">alto</param>
        /// <param name="scaleFactor">escala</param>
        /// <param name="addDV"> false por defecto</param>
        /// <param name="backgroundColor">color fondo, null por defecto</param>
        /// <param name="foregroundColor">color fuente, null por defecto</param>
        /// <returns></returns>
        public static Image GenerateITFImage(
          string data,
          int width,
          int height,
          int scaleFactor,
          bool addDV = false,
          Brush backgroundColor = null,
          Brush foregroundColor = null)
        {
            if (addDV)
                data += CalculateVerificationDigit(data).ToString();
            IDictionary<int, string> dictionary1 = (IDictionary<int, string>)new Dictionary<int, string>();
            dictionary1.Add(0, "00110");
            dictionary1.Add(1, "10001");
            dictionary1.Add(2, "01001");
            dictionary1.Add(3, "11000");
            dictionary1.Add(4, "00101");
            dictionary1.Add(5, "10100");
            dictionary1.Add(6, "01100");
            dictionary1.Add(7, "00011");
            dictionary1.Add(8, "10010");
            dictionary1.Add(9, "01010");
            if (string.IsNullOrEmpty(data))
                throw new ArgumentNullException("Data");
            if (!data.All<char>(new Func<char, bool>(char.IsDigit)))
                throw new ArgumentOutOfRangeException("Data", "Los datos sólo pueden ser números");
            if (data.Length % 2 != 0)
                throw new ArgumentException("Data", "La cantidad de dígitos debe ser par");
            IList<KeyValuePair<int, string>> keyValuePairList1 = (IList<KeyValuePair<int, string>>)new List<KeyValuePair<int, string>>();
            char ch;
            for (int index = 0; index < data.Length; ++index)
            {
                ch = data[index];
                Convert.ToInt32(ch.ToString());
                IDictionary<int, string> dictionary2 = dictionary1;
                ch = data[index];
                int int32_1 = Convert.ToInt32(ch.ToString());
                string str1 = dictionary2[int32_1];
                IList<KeyValuePair<int, string>> keyValuePairList2 = keyValuePairList1;
                ch = data[index];
                int int32_2 = Convert.ToInt32(ch.ToString());
                IDictionary<int, string> dictionary3 = dictionary1;
                ch = data[index];
                int int32_3 = Convert.ToInt32(ch.ToString());
                string str2 = dictionary3[int32_3];
                KeyValuePair<int, string> keyValuePair = new KeyValuePair<int, string>(int32_2, str2);
                keyValuePairList2.Add(keyValuePair);
            }
            string str3 = string.Empty;
            for (int index1 = 0; index1 < keyValuePairList1.Count; index1 += 2)
            {
                string str1 = keyValuePairList1[index1].Value;
                string str2 = keyValuePairList1[index1 + 1].Value;
                for (int index2 = 0; index2 < 5; ++index2)
                {
                    string str4 = str3;
                    ch = str1[index2];
                    string str5 = ch.ToString() == "0" ? "X" : "Y";
                    ch = str2[index2];
                    string str6 = ch.ToString() == "0" ? "A" : "B";
                    str3 = str4 + str5 + str6;
                }
            }
            string str7 = "XAXA" + str3 + "YAX";
            if (backgroundColor == null)
                backgroundColor = Brushes.White;
            if (foregroundColor == null)
                foregroundColor = Brushes.Black;
            int x = 20;
            int width1 = scaleFactor;
            int width2 = 3 * scaleFactor;
            Image image = (Image)new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(image))
            {
                graphics.FillRectangle(backgroundColor, 0, 0, width, height);
                for (int index = 0; index < str7.Length; ++index)
                {
                    ch = str7[index];
                    string str1 = ch.ToString();
                    if (!(str1 == "A"))
                    {
                        if (!(str1 == "B"))
                        {
                            if (!(str1 == "X"))
                            {
                                if (str1 == "Y")
                                {
                                    graphics.FillRectangle(foregroundColor, x, 0, width2, height);
                                    x += width2;
                                }
                            }
                            else
                            {
                                graphics.FillRectangle(foregroundColor, x, 0, width1, height);
                                x += width1;
                            }
                        }
                        else
                        {
                            graphics.FillRectangle(backgroundColor, x, 0, width2, height);
                            x += width2;
                        }
                    }
                    else
                    {
                        graphics.FillRectangle(backgroundColor, x, 0, width1, height);
                        x += width1;
                    }
                }
                return image;
            }
        }

        public static string GenerateITFString(string data, bool addDV = false)
        {
            if (addDV)
                data += Barcode.CalculateVerificationDigit(data).ToString();
            string empty = string.Empty;
            for (int startIndex = 0; startIndex < data.Length; startIndex += 2)
            {
                byte num = byte.Parse(data.Substring(startIndex, 2));
                empty += ((char)((int)num + (num < (byte)94 ? 33 : 101))).ToString();
            }
            return "É" + empty + "Ê";
        }
    }
}
