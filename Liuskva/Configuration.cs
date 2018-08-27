namespace Liuskva
{
    public class Configuration : IConfiguration
    {
        public string PathToSecretDecryptionKeyFile { get; } = ".secret";
        public string PathToSettingsFile { get; } = "Settings.json";
    }
}