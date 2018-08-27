using JetBrains.Annotations;

namespace Liuskva.OedApi
{
    public interface IOedClient
    {
        [NotNull] string FetchDesignation([NotNull] string word);
    }
}