using JetBrains.Annotations;

namespace Liuskva.Utilities
{
    public interface ISettings
    {
        [NotNull] string OedApiKey { get; }
        [NotNull] string OedApiUrl { get; }
        [NotNull] string OedAppId { get; }
    }
}