using Shooter.Calendar.Core.Common.Extensions;
using MvvmCross;

namespace Shooter.Calendar.Core.Localization
{
    public static class InitializationExtensions
    {
        public static void Initialize()
        {
            IoCExtensions.RegisterSingleton<ILookupDictionaryProvider, DefaultLookupDictionaryProvider>();
            IoCExtensions.RegisterSingleton<ILocalizationFetcher, LocalizationFetcher>();
        }

        public static void AddLocalizationResources(params string[] fileNames)
        {
            var fetcher = Mvx.IoCProvider.Resolve<ILocalizationFetcher>();

            foreach(var fileName in fileNames)
            {
                fetcher.AddLocalizationResource(fileName);
            }
        }
    }
}
