namespace Localization
{
    public class LocalizationConfig
    {
        public static string LocalizationFolderDefault = "LocalizationResources";

        public static string DefaultLocaleDefault = "";

        public string LocalizationFolder { get; set; } = LocalizationFolderDefault;

        public string DefaultLocale { get; set; } = DefaultLocaleDefault;
    }
}
