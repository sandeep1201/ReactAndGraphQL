using System.Threading.Tasks;
using DCF.Core.Dependency;

namespace DCF.Core.Runtime.Pipline
{
    public class TransistionPipeline<T, K> : Pipeline<T>, ITransistionPipeline<T,K> where T : class where K : class
    {
        private K childSubject;
        // for reference only, the builder will use this
        internal Pipeline<T> Parent { get; set; }
        public Pipeline<K> Child { get; set; }

        public TransistionPipeline(Pipeline<K> childPipeline, K subject) 
            : this(IocManager.Instance, childPipeline, subject)
        {
            
        }
        public TransistionPipeline(IIocManager iocManager, Pipeline<K> childPipeline, K subject) : base(iocManager)
        {
            this.Child = childPipeline;
            this.childSubject = subject;
        }

        public override async Task Execute(T subject)
        {
            await base.Execute(subject);
            // execute child pipelines
            if (this.Child != null)
                await this.Child.Execute(this.childSubject);
        }
    }
}