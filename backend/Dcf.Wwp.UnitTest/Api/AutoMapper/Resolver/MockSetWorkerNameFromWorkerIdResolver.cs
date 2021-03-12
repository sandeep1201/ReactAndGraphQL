using AutoMapper;
using Dcf.Wwp.Api.Library.AutoMapperConfigs.Resolvers;

namespace Dcf.Wwp.UnitTest.Api.AutoMapper.Resolver
{
    public class MockSetModifiedDetailsResolver : SetModifiedDetailsResolver
    {
        public MockSetModifiedDetailsResolver() : base(null)
        {
        }

        public override string Resolve(object source, object destination, string destMember, ResolutionContext context)
        {
            return "Success";
        }
    }
}
