namespace Shooter.Calendar.Core.Localization
{
    public interface ILocalizationFetcher
    {
        LocalizationConfig LocalizationConfig { get; set; }

        void AddLocalizationResource(string fileName);

        void InitLocalizations();
    }
}
