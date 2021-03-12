using System;
using System.Linq;
using AutoMapper;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.DataAccess.Interfaces;

namespace Dcf.Wwp.Api.Library.AutoMapperConfigs.Resolvers
{
    public class WIUIDToFullNameModifiedByResolver : IValueResolver<object, object, string>
    {
        #region Properties

        private readonly Func<string, string> _convertWIUIdToName;

        #endregion

        #region Methods

        public WIUIDToFullNameModifiedByResolver(IWorkerRepository workerRepo)
        {
            _convertWIUIdToName = (wiuId) =>
                                  {
                                      string wn;
                                      switch (wiuId)
                                      {
                                          case "WWP Conversion":
                                              wn = wiuId;
                                              break;
                                          case "WWP Batch":
                                              wn = wiuId;
                                              break;
                                          case "WWP":
                                              wn = wiuId;
                                              break;
                                          case "CWW":
                                              wn = wiuId;
                                              break;
                                          default:
                                          {
                                              var wo = workerRepo.GetAsQueryable()
                                                                 .Where(i => i.WIUId == wiuId)
                                                                 .Select(i => new { i.FirstName, i.MiddleInitial, i.LastName })
                                                                 .FirstOrDefault();

                                              wn = $"{wo?.FirstName} {wo?.MiddleInitial}. {wo?.LastName}".Replace(" . ", " ");
                                              break;
                                          }
                                      }

                                      return (wn);
                                  };
        }

        public virtual string Resolve(object source, object destination, string destMember, ResolutionContext context)
        {
            return _convertWIUIdToName(source.GetPropertyValue("ModifiedBy").ToString());
        }

        #endregion
    }
}
