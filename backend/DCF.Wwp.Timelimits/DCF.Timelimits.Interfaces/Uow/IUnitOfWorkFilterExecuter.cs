using System;

namespace DCF.Core.Domain.Uow
{
    public interface IUnitOfWorkFilterExecuter
    {
        void ApplyDisableFilter(IUnitOfWork unitOfWork, String filterName);
        void ApplyEnableFilter(IUnitOfWork unitOfWork, String filterName);
        void ApplyFilterParameterValue(IUnitOfWork unitOfWork, String filterName, String parameterName, Object value);
    }
}
