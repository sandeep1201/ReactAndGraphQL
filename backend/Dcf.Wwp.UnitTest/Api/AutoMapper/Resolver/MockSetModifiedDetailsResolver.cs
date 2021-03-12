using AutoMapper;
using Dcf.Wwp.Api.Library.AutoMapperConfigs.Resolvers;

namespace Dcf.Wwp.UnitTest.Api.AutoMapper.Resolver
{
    public class MockSetWorkerNameFromWorkerIdResolver : SetWorkerNameFromWorkerIdResolver
    {
        public MockSetWorkerNameFromWorkerIdResolver() : base(null)
        {
        }

        public override string Resolve(object source, object destination, string destMember, ResolutionContext context)
        {
            return "Success";
        }
    }
}
