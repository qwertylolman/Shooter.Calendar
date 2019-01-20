using System;
using System.Collections.Generic;
using System.Linq;
using MvvmCross.Base;
using Newtonsoft.Json;
using Shooter.Calendar.Core.Attributes;

namespace Shooter.Calendar.Core.Localization
{
    public class LocalizationFetcher : ILocalizationFetcher
    {
        private readonly IMvxResourceLoader resourceLoader;
        private readonly ILookupDictionaryProvider lookupDictionaryProvider;

        private readonly IList<string> localizationResources;

        private LocalizationConfig localizationConfig;

        public LocalizationFetcher(
            [NotNull] IMvxResourceLoader resourceLoader,
            [NotNull] ILookupDictionaryProvider lookupDictionaryProvider)
        {
            this.resourceLoader = resourceLoader;
            this.lookupDictionaryProvider = lookupDictionaryProvider;

            localizationResources = new List<string>();

            localizationConfig = new LocalizationConfig();
        }

        public LocalizationConfig LocalizationConfig 
        { 
            get { return localizationConfig; }
            set 
            { 
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(LocalizationConfig));
                }

                if (EqualityComparer<LocalizationConfig>.Default.Equals(localizationConfig, value) == true)
                {
                    return;
                }

                localizationConfig = value;
                lookupDictionaryProvider.Drop();
            }
        }

        public void AddLocalizationResource(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) == true)
            {
                return;
            }

            if (localizationResources.Contains(fileName) == true)
            {
                return;
            }

            localizationResources.Add(fileName);
        }

        public void InitLocalizations()
        {
            try
            {
                var resources = localizationResources.Select(GetResourceFilePath).ToList();
                var locale = LocalizationConfig.DefaultLocale;

                foreach (var resource in resources)
                {
                    var str = resourceLoader.GetTextResource(resource);
                    if (string.IsNullOrEmpty(str) == true)
                    {
                        continue;
                    }

                    var dictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(str);

                    foreach (var kvp in dictionary)
                    {
                        lookupDictionaryProvider.AddValue(kvp.Key, locale, kvp.Value);
                    }
                }
            }
            catch(Exception)
            {
                lookupDictionaryProvider.Drop();
                throw;
            }
        }

        private string GetResourceFilePath(string fileName)
        {
            if (fileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase) == false)
            {
                fileName = $"{fileName}.json";
            }

            var config = LocalizationConfig;
            var locale = config.DefaultLocale;
            var folder = config.LocalizationFolder;
            return string.IsNullOrEmpty(locale) == true
                ? $"{folder}/{fileName}"
                : $"{folder}/{locale}/{fileName}";
        }
    }
}
