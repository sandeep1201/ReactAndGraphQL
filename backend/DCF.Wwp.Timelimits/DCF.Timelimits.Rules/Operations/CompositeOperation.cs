namespace DCF.Timelimits.Rules.Actions
{
    public abstract class CompositeOperation<T> : Operation<T>
    {
        /// <summary>
        /// Gets the first specification.
        /// </summary>
        public IOperation<T> First { get; }

        /// <summary>
        /// Gets the second specification.
        /// </summary>
        public IOperation<T> Second { get; }

        protected CompositeOperation(IOperation<T> first, IOperation<T> second)
        {
            this.First = first;
            this.Second = second;
        }
    }
}