namespace DCF.Core.Runtime.Pipline
{
    public interface ITransistionPipeline<T, K> : IPipeline<T> where T : class where K : class
    {
        Pipeline<K> Child { get; }
    }
}