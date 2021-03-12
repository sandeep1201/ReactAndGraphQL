using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.Model.Interface;

namespace Dcf.Wwp.Api.Library.Contracts
{
    public abstract class BaseContract : IHasId, IIsNew
    {
        public int Id { get; set; }

        public bool IsNew()
        {
            return Id == 0;
        }
    }
}