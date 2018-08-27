using JetBrains.Annotations;

namespace Liuskva
{
    public interface IConfiguration
    {
        [NotNull] string PathToSecretDecryptionKeyFile { get; }
        [NotNull] string PathToSettingsFile { get; }
    }
}