using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Net.Utils
{
    public static class StringUtils
    {
        public static string GetNumbersFromString(this string input)
        {
            return Regex.Replace(input, @"[^\d]", "");
        }

        public static string RemoveDuplicatedSpaces(this string inputWord)
        {
            if (string.IsNullOrEmpty(inputWord)) return string.Empty;

            RegexOptions options = RegexOptions.None;
            Regex regex = new Regex("[ ]{2,}", options);

            return regex.Replace(inputWord.Trim(), " ");
        }

        public static string ToPascalCase(this string inputWord)
        {
            try
            {
                if (string.IsNullOrEmpty(inputWord)) return string.Empty;

                inputWord = inputWord.RemoveDuplicatedSpaces();

                var itemArray = inputWord.Split(new char[0]);

                StringBuilder result = new StringBuilder();

                foreach (var word in itemArray)
                {
                    result.Append(word.First().ToString().ToUpper()).Append(word.Substring(1).ToLower()).Append(" ");
                }

                return result.Remove(result.Length - 1, 1).ToString();
            }
            catch
            {
                return inputWord;
            }
        }

        public static string RemoveSeparators(this string inputWord)
        {
            inputWord = inputWord.Trim();
            inputWord = inputWord.Replace(" ", string.Empty);
            inputWord = inputWord.Replace("-", string.Empty);
            inputWord = inputWord.Replace("_", string.Empty);
            inputWord = inputWord.Replace("/", string.Empty);
            inputWord = inputWord.Replace(".", string.Empty);
            inputWord = inputWord.Replace(";", string.Empty);
            inputWord = inputWord.Replace("|", string.Empty);
            inputWord = inputWord.Replace(",", string.Empty);
            inputWord = inputWord.Replace("\\", string.Empty);

            return inputWord;
        }

        public static string WingTrim(this string value, char c)
        {
            return value?.TrimStart(c).TrimEnd(c);
        }

        public static string WingCerosTrim(this string value)
        {
            return value.WingTrim('0');
        }

        public static bool StringIsInList(string stringList, string value)
        {
            string[] arrayString = stringList.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

            return arrayString.Any(x => x == value);
        }

        public static string AddCuitSeparator(string value)
        {
            if (string.IsNullOrEmpty(value) && value.Length != 11) return string.Empty;

            return string.Format("{0}-{1}-{2}", value.Substring(0, 2), value.Substring(2, 8), value.Substring(10, 1));         
        }

        public static bool IsDigitsOnly(string str)
        {
            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    return false;
            }

            return true;
        }

        public static string CurrencyFormat(decimal amount, bool withSimbol = false)
        {
            return withSimbol ? string.Format("$ {0}", amount.ToString("n", CultureInfo.CurrentCulture)) : amount.ToString("n", CultureInfo.CurrentCulture);
        }

        public static string DecimalStringFormat(decimal amount, int intPlaces = 10, int decimalPlaces = 2, char separator = '.', bool withSimbol = false)
        {
            var format = string.Empty.PadLeft(intPlaces, '0') + "." + string.Empty.PadRight(decimalPlaces, '0');

            var result = amount.ToString(format).Replace('.', separator).Replace(',', separator);

            if (withSimbol) result = string.Format("$ {0}", result);

            return result;
        }

        public static decimal StringToDecimal(string amount)
        {
            return decimal.Parse(string.Format("{0:0.00}", decimal.Parse(amount) / 100));
        }

        public static string Coalesce(string source, params string[] replacements)
        {
            if (!string.IsNullOrWhiteSpace(source)) return source;

            foreach (var replacement in replacements)
                if (!string.IsNullOrWhiteSpace(replacement)) return replacement;

            return string.Empty;
        }

        public static string FirstCharToUpper(this string str)
        {
            if (string.IsNullOrEmpty(str) || char.IsUpper(str[0])) return str;

            return char.ToUpper(str[0]) + str.Substring(1);
        }

        public static string FirstCharToLowerCase(this string str)
        {
            if (string.IsNullOrEmpty(str) || char.IsLower(str[0]))
                return str;

            return char.ToLower(str[0]) + str.Substring(1);
        }

        public static string ConvertToBase64(string value)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(value));
        }

        public static string RemoveSpecialCharacters(this string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Trunca una cadena según el valor pasado en length.
        /// </summary>
        /// <param name="text">Cadena a truncar.</param>
        /// <param name="length">Longitud a truncar.</param>
        public static string Truncate(this string text, int length)
        {
            return string.IsNullOrEmpty(text) ? text : (text.Length <= length || length < 1) ? text : text.Substring(0, length);
        }

        /// <summary>
        /// Remueve los caracteres ".", " ", "-" de la cadena dada y la pasa a minúsculas. Si se indica también se remueven los saltos de línea.
        /// </summary>
        /// <param name="originalText">Cadena a depurar.</param>
        /// <param name="removeBreakLines">[true] quita los saltos de línea.</param>
        public static string GetCleanTextToLower(string originalText, bool removeBreakLines)
        {
            var response = originalText.Replace(" ", string.Empty).Replace(".", string.Empty).Replace("-", string.Empty).Replace("°", string.Empty);

            if (removeBreakLines) response = response.Replace(Environment.NewLine, "").Replace("\n", string.Empty);

            return response.ToLower();
        }
    }
}