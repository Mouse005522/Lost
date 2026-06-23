using Cysharp.Threading.Tasks;

namespace Kun.Tool
{
    public interface IPrepareable
    {
        UniTask PrepareFlowAsync ();
    }
}
