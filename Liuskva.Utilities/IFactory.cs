using JetBrains.Annotations;

namespace Liuskva.Utilities
{
    public interface IFactory
    {
        [NotNull] TInstance GetInstance<TInstance>() where TInstance : class;
    }
}
