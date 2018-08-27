using JetBrains.Annotations;

namespace Liuskva.Utilities
{
    public interface IBootstrap
    {
        void Run([NotNull] string[] args);
    }
}
