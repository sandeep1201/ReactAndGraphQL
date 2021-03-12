using AutoMapper;
using Dcf.Wwp.Model.Interface.Core;

namespace Dcf.Wwp.Api.Library.AutoMapperConfigs.Resolvers
{
    public class SetModifiedDetailsResolver : IValueResolver<object, object, string>
    {
        #region Properties

        private readonly IAuthUser _authUser;

        #endregion

        #region Methods

        public SetModifiedDetailsResolver(IAuthUser authUser)
        {
            _authUser = authUser;
        }

        public virtual string Resolve(object source, object destination, string destMember, ResolutionContext context)
        {
            return _authUser.WIUID;
        }

        #endregion
    }
}
