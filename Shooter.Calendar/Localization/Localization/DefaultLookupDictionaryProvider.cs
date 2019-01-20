using System;
using System.Collections.Generic;

namespace Localization
{
    public class DefaultLookupDictionaryProvider : ILookupDictionaryProvider
    {
        private static readonly string Salt;

        static DefaultLookupDictionaryProvider()
        {
            Salt = Guid.NewGuid().GetHashCode().ToString();
        }

        private readonly IDictionary<string, string> dictionary;

        public DefaultLookupDictionaryProvider()
        {
            dictionary = new Dictionary<string, string>();
        }

        public void AddValue(string key, string locale, string value)
        {
            dictionary[CombineKeyAndLocale(key, locale)] = value;
        }

        public string GetStringOrDefault(string key, string locale, string defaultValue)
        {
            var combinedKey = CombineKeyAndLocale(key, locale);
            string value;
            if (dictionary.TryGetValue(combinedKey, out value) == true)
            {
                return value;
            }

            if (dictionary.TryGetValue(key, out value) == true)
            {
                return value;
            }


            return defaultValue;
        }

        private string CombineKeyAndLocale(string key, string locale)
        {
            if (string.IsNullOrEmpty(locale) == true)
            {
                return key;
            }

            return $"{key}.{Salt}.{locale}";
        }

        public void Drop()
        {
            dictionary.Clear();
        }
    }
}
