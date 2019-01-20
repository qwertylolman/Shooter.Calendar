using MvvmCross;

namespace Shooter.Calendar.Core.Localization
{
    public static class LocalizationExtensions
    {
        public static string DefaultFallbackValue = "";

        private static ILookupDictionaryProvider dictionaryProvider;
        private static ILocalizationFetcher localizationFetcher;

        public static ILookupDictionaryProvider DictionaryProvider
            => dictionaryProvider ?? (dictionaryProvider = Mvx.IoCProvider.Resolve<ILookupDictionaryProvider>());

        public static ILocalizationFetcher LocalizationFetcher
            => localizationFetcher ?? (localizationFetcher = Mvx.IoCProvider.Resolve<ILocalizationFetcher>());

        public static string Locale
            => LocalizationFetcher.LocalizationConfig.DefaultLocale;

        public static string Get(string key)
            => DictionaryProvider.GetStringOrDefault(key, Locale, DefaultFallbackValue);

        public static string Format(string key, params object[] args)
            => string.Format(DictionaryProvider.GetStringOrDefault(key, Locale, DefaultFallbackValue), args);
    }
}
