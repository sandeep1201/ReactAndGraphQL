using System;
using System.Linq;
using AutoMapper;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.DataAccess.Interfaces;

namespace Dcf.Wwp.Api.Library.AutoMapperConfigs.Resolvers
{
    public class SetWorkerNameFromWorkerIdResolver : IValueResolver<object, object, string>
    {
        #region Properties

        private readonly Func<object, string> _convertIdToName;

        #endregion

        #region Methods

        public SetWorkerNameFromWorkerIdResolver(IWorkerRepository workerRepo)
        {
            _convertIdToName = (id) =>
                               {
                                   int.TryParse(id?.ToString(), out var intId);
                                   var wo = workerRepo.GetAsQueryable()
                                                      .Where(i => i.Id == intId)
                                                      .Select(i => new { i.FirstName, i.MiddleInitial, i.LastName })
                                                      .FirstOrDefault();

                                   return $"{wo?.FirstName} {wo?.MiddleInitial}. {wo?.LastName}".Replace(" . ", " ");
                               };
        }

        public virtual string Resolve(object source, object destination, string destMember, ResolutionContext context)
        {
            return _convertIdToName(source.GetPropertyValue("WorkerId"));
        }

        #endregion
    }
}
