namespace Kun.Tool
{
    /// <summary>
    /// provider要各自處理速度, 不過限制與deltaTime會交由外部處理
    /// </summary>
    public abstract class InputProvider
    {
        public virtual void Update (float deltaTime)
        {
        }
    }
}
