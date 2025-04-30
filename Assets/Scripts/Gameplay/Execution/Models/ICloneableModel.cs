namespace Gameplay.Execution.Models
{
    /// <summary>
    /// Interface for models that can be cloned.
    /// </summary>
    public interface ICloneableModel<T> where T : ICloneableModel<T>
    {
        T Clone();
    }
}