namespace Shooter.Calendar.Core.Localization
{
    public interface ILookupDictionaryProvider
    {
        string GetStringOrDefault(string key, string locale, string defaultValue);

        void AddValue(string key, string locale, string value);

        void Drop();
    }
}
