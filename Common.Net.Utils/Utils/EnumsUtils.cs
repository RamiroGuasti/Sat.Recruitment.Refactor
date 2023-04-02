using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Common.Net.Utils
{
    public static class EnumsUtils
    {
        public static IDictionary<int, string> ToDictionary<T>()
        {
            var dictionary = new Dictionary<int, string>();

            foreach (var item in Enum.GetValues(typeof(T))) { dictionary.Add((int)item, item.ToString()); }

            return dictionary;
        }

        public static string GetDescriptionAttribute(this object value)
        {
            if (value == null) return null; 

            var descriptionAttribute = GetAttributeFromEnum<DescriptionAttribute>(value);

            return descriptionAttribute?.Description;
        }

        public static TEnum? TryParseElementName<TEnum>(string elementName) where TEnum : struct
        {
            TEnum? result = null;
            var enumType = typeof(TEnum);
            try
            {
                result = (TEnum)Enum.Parse(enumType, elementName);
            }
            catch (ArgumentException)
            {
                /*leave returning null values.*/
            }

            return result;
        }

        public static IDictionary<TEnum, string> GetAll<TEnum>() where TEnum : struct
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToDictionary(t => t, t => GetDescriptionAttribute(t));
        }

        public static TAttribute GetAttributeFromEnum<TAttribute>(object value) where TAttribute : Attribute
        {
            var pobjType = value.GetType();
            var pobjFieldInfo = pobjType.GetField(Enum.GetName(pobjType, value));

            return pobjFieldInfo.GetCustomAttributes(typeof(TAttribute), false).OfType<TAttribute>().FirstOrDefault();
        }
    }
}