using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Dcf.Wwp.Api.Library.Contracts;
using Dcf.Wwp.Api.Library.Extensions;
using Dcf.Wwp.Api.Library.Interfaces;
using Dcf.Wwp.DataAccess.Interfaces;
using Dcf.Wwp.DataAccess.Models;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Core;

namespace Dcf.Wwp.Api.Library.Domains
{
    public class WeeklyHoursWorkedDomain : IWeeklyHoursWorkedDomain
    {
        #region Properties

        private readonly IUnitOfWork                      _unitOfWork;
        private readonly IAuthUser                        _authUser;
        private readonly IMapper                          _mapper;
        private readonly IWeeklyHoursWorkedRepository     _weeklyHoursWorkedRepository;
        private readonly IEmploymentInformationRepository _employmentInformationRepository;
        private const    int                              MaxHours = 1040;

        #endregion

        public WeeklyHoursWorkedDomain(IUnitOfWork                      unitOfWork,
                                       IAuthUser                        authUser,
                                       IMapper                          mapper,
                                       IWeeklyHoursWorkedRepository     weeklyHoursWorkedRepository,
                                       IEmploymentInformationRepository employmentInformationRepository)
        {
            _unitOfWork                      = unitOfWork;
            _authUser                        = authUser;
            _mapper                          = mapper;
            _weeklyHoursWorkedRepository     = weeklyHoursWorkedRepository;
            _employmentInformationRepository = employmentInformationRepository;
        }

        public IEnumerable<WeeklyHoursWorked> GetWeeklyHoursWorkedEntries(int employmentInformationId)
        {
            return _weeklyHoursWorkedRepository.GetMany(i => i.EmploymentInformationId == employmentInformationId);
        }

        public List<WeeklyHoursWorkedContract> GetWeeklyHoursWorkedContracts(int employmentInformationId)
        {
            return _mapper.Map<List<WeeklyHoursWorkedContract>>(GetWeeklyHoursWorkedEntries(employmentInformationId));
        }

        public WeeklyHoursWorkedContract GetWeeklyHoursWorkedEntry(int id)
        {
            return _mapper.Map<WeeklyHoursWorkedContract>(_weeklyHoursWorkedRepository.Get(i => i.Id == id));
        }

        public List<WeeklyHoursWorkedContract> UpsertWeeklyHoursWorkedEntry(WeeklyHoursWorkedContract contract)
        {
            if (contract == null)
            {
                throw new ArgumentNullException(nameof(contract));
            }

            var modifiedBy   = _authUser.WIUID;
            var modifiedDate = DateTime.Now;
            var updatedHours = 0.0m;
            var totalSubsidizedHours = _weeklyHoursWorkedRepository.GetMany(i => i.EmploymentInformationId == contract.EmploymentInformationId)
                                                                   .Where(i => i.Id                        != contract.Id)
                                                                   .Sum(i => i.Hours) + contract.Hours;
            if (contract.Id > 0)
            {
                var weeklyHoursWorkedFromDb =  _weeklyHoursWorkedRepository.Get(i => i.Id == contract.Id);
                updatedHours = contract.Hours - weeklyHoursWorkedFromDb.Hours;
            }

            var weeklyHoursWorked = _mapper.Map<WeeklyHoursWorked>(contract);

            weeklyHoursWorked.EmploymentInformation                      = _employmentInformationRepository.Get(i => i.Id == contract.EmploymentInformationId);
            weeklyHoursWorked.EmploymentInformation.TotalSubsidizedHours = totalSubsidizedHours;
            weeklyHoursWorked.EmploymentInformation.ModifiedBy           = _authUser.Username;
            weeklyHoursWorked.EmploymentInformation.ModifiedDate         = modifiedDate;
            weeklyHoursWorked.ModifiedBy                                 = modifiedBy;
            weeklyHoursWorked.ModifiedDate                               = modifiedDate;

            UpdateOrInsertTotalLifetimeHoursDate(modifiedDate, weeklyHoursWorked, updatedHours);
            UpdateTotalSubsidyAmount(modifiedDate, weeklyHoursWorked, employmentInfoModel: weeklyHoursWorked.EmploymentInformation);

            _weeklyHoursWorkedRepository.AddOrUpdate(weeklyHoursWorked);
            _unitOfWork.Commit();

            return GetWeeklyHoursWorkedContracts(contract.EmploymentInformationId);
        }


        private void UpdateOrInsertTotalLifetimeHoursDate(DateTime modifiedDate, WeeklyHoursWorked weeklyHoursWorked, decimal updatedHours = 0)
        {
            var participant = weeklyHoursWorked?.EmploymentInformation.Participant;
            if (participant?.TotalLifetimeHoursDate == null &&
                participant?.EmploymentInformations.Where(i => i.DeleteReasonId == null).Sum(i => i.TotalSubsidizedHours) + (weeklyHoursWorked?.Id == 0
                                                                                                                                 ? 0
                                                                                                                                 : updatedHours) >= MaxHours)
            {
                var weeklyHoursWorkedEntries = _weeklyHoursWorkedRepository.GetMany(i => i.EmploymentInformationId == weeklyHoursWorked.EmploymentInformationId ).ToList();

                weeklyHoursWorkedEntries.Add(weeklyHoursWorked);
                weeklyHoursWorked.EmploymentInformation.Participant.TotalLifetimeHoursDate = weeklyHoursWorkedEntries.OrderByDescending(i => i.StartDate).FirstOrDefault()?.StartDate;
                weeklyHoursWorked.EmploymentInformation.Participant.ModifiedBy             = _authUser.Username;
                weeklyHoursWorked.EmploymentInformation.Participant.ModifiedDate           = modifiedDate;
            }
            else
                if (participant?.TotalLifetimeHoursDate != null &&
                    participant?.EmploymentInformations.Where(i => i.DeleteReasonId == null).Sum(i => i.TotalSubsidizedHours) + (weeklyHoursWorked.Id == 0
                                                                                                                                     ? 0
                                                                                                                                     : updatedHours) < MaxHours)
                {
                    weeklyHoursWorked.EmploymentInformation.Participant.TotalLifetimeHoursDate = null;
                    weeklyHoursWorked.EmploymentInformation.Participant.ModifiedBy             = _authUser.Username;
                    weeklyHoursWorked.EmploymentInformation.Participant.ModifiedDate           = modifiedDate;
                }
        }

        public List<WeeklyHoursWorkedContract> DeleteWeeklyHoursWorkedEntry(int id)
        {
            var weeklyHoursWorked = _weeklyHoursWorkedRepository.Get(i => i.Id == id);

            weeklyHoursWorked.EmploymentInformation.TotalSubsidizedHours -= weeklyHoursWorked.Hours;
            var modifiedDate = DateTime.Now;

            weeklyHoursWorked.EmploymentInformation.ModifiedBy   = _authUser.Username;
            weeklyHoursWorked.EmploymentInformation.ModifiedDate = modifiedDate;

            UpdateOrInsertTotalLifetimeHoursDate(modifiedDate, weeklyHoursWorked);

            _weeklyHoursWorkedRepository.Delete(weeklyHoursWorked);
            _unitOfWork.Commit();

            return GetWeeklyHoursWorkedContracts(weeklyHoursWorked.EmploymentInformationId);
        }

        public dynamic UpdateTotalSubsidyAmount(DateTime               modifiedDate,                   WeeklyHoursWorked     weeklyHoursWorked   = null,
                                                IEmploymentInformation employmentInfoInterface = null, EmploymentInformation employmentInfoModel = null)
        {
            var wageHours = new List <(DateTime? EffectiveDate, decimal? HourlySubsidyRate, decimal? WorkSiteContribution)>
                            {
                                employmentInfoInterface != null
                                    ? (!employmentInfoInterface.IsDeleted ? (employmentInfoInterface.WageHour.CurrentEffectiveDate, employmentInfoInterface.WageHour?.CurrentHourlySubsidyRate, employmentInfoInterface.WageHour?.WorkSiteContribution) : (null, null, null))
                                    : (employmentInfoModel != null && !employmentInfoModel.WageHour.IsDeleted ? (employmentInfoModel.WageHour.CurrentEffectiveDate, employmentInfoModel.WageHour?.CurrentHourlySubsidyRate, employmentInfoModel.WageHour?.WorkSiteContribution) : (null, null, null))
                            };

            var wageHourHistories = employmentInfoInterface != null
                                        ? employmentInfoInterface.WageHour?.WageHourHistories?.Where(i => !i.IsDeleted).Select(i => (i.EffectiveDate, i.HourlySubsidyRate, i.WorkSiteContribution))
                                        : employmentInfoModel?.WageHour?.WageHourHistories?.Where(i => !i.IsDeleted).Select(i => (i.EffectiveDate, i.HourlySubsidyRate, i.WorkSiteContribution));

            decimal calculatedTotalSubsidyAmount;
            decimal calculatedWorkSiteContributionAmount;
            dynamic weeklyWorkedEntries = null;

            if (wageHourHistories != null)
                wageHours.AddRange(wageHourHistories);

            if (employmentInfoInterface != null)
            {
                employmentInfoInterface.WeeklyHoursWorkedEntries.ForEach(i =>
                                                                         {
                                                                             calculatedTotalSubsidyAmount         = CalculateTotalSubsidyAmount(wageHours, i);
                                                                             calculatedWorkSiteContributionAmount = CalculateWorkSiteContributionAmount(wageHours, i);

                                                                             if (i.TotalSubsidyAmount == calculatedTotalSubsidyAmount && i.TotalWorkSiteAmount == calculatedWorkSiteContributionAmount) return;
                                                                             i.TotalSubsidyAmount  = calculatedTotalSubsidyAmount;
                                                                             i.TotalWorkSiteAmount = calculatedWorkSiteContributionAmount;
                                                                             i.ModifiedBy          = _authUser.WIUID;
                                                                             i.ModifiedDate        = modifiedDate;
                                                                         });

                weeklyWorkedEntries = employmentInfoInterface.WeeklyHoursWorkedEntries;
            }

            if (weeklyHoursWorked == null) return weeklyWorkedEntries;

            calculatedTotalSubsidyAmount         = CalculateTotalSubsidyAmount(wageHours, weeklyHoursWorked);
            calculatedWorkSiteContributionAmount = CalculateWorkSiteContributionAmount(wageHours, weeklyHoursWorked);

            if (weeklyHoursWorked.TotalSubsidyAmount != calculatedTotalSubsidyAmount)
            {
                weeklyHoursWorked.TotalSubsidyAmount = calculatedTotalSubsidyAmount;
                weeklyHoursWorked.ModifiedBy         = _authUser.WIUID;
                weeklyHoursWorked.ModifiedDate       = modifiedDate;
            }

            if (weeklyHoursWorked.TotalWorkSiteAmount != calculatedWorkSiteContributionAmount)
            {
                weeklyHoursWorked.TotalWorkSiteAmount = calculatedWorkSiteContributionAmount;
                weeklyHoursWorked.ModifiedBy          = _authUser.WIUID;
                weeklyHoursWorked.ModifiedDate        = modifiedDate;
            }

            weeklyWorkedEntries = weeklyHoursWorked;

            return weeklyWorkedEntries;
        }

        private decimal CalculateTotalSubsidyAmount(IEnumerable<(DateTime? EffectiveDate, decimal? HourlySubsidyRate, decimal? WorkSiteContribution)> wageHours, dynamic weeklyHoursWorked)
        {
            //if there is no record in the past make sure the records are for that week.
            wageHours = wageHours.OrderByDescending(i => i.EffectiveDate).ToList();
            var closestHourlySubsidyRate = wageHours.FirstOrDefault(j => j.EffectiveDate <= weeklyHoursWorked.StartDate).HourlySubsidyRate
                                           ?? wageHours.FirstOrDefault(j => j.EffectiveDate    >= ToDateTimeMonthYearExtensions.StartOfWeek(weeklyHoursWorked.StartDate)
                                                                            && j.EffectiveDate <= ToDateTimeMonthYearExtensions.LastDayOfWeek(weeklyHoursWorked.StartDate)).HourlySubsidyRate ?? 0;
            var calculatedTotalSubsidyAmount = Math.Round(weeklyHoursWorked.Hours * closestHourlySubsidyRate, 2);

            return Convert.ToDecimal(calculatedTotalSubsidyAmount);
        }

        private decimal CalculateWorkSiteContributionAmount(IEnumerable<(DateTime? EffectiveDate, decimal? HourlySubsidyRate, decimal? WorkSiteContribution)> wageHours, dynamic weeklyHoursWorked)
        {
            //if there is no record in the past make sure the records are for that week.
            wageHours = wageHours.OrderByDescending(j => j.EffectiveDate).ToList();
            var closestWorkSiteContribution = wageHours.FirstOrDefault(j => j.EffectiveDate <= weeklyHoursWorked.StartDate).WorkSiteContribution
                                              ?? wageHours.FirstOrDefault(j => j.EffectiveDate    >= ToDateTimeMonthYearExtensions.StartOfWeek(weeklyHoursWorked.StartDate)
                                                                               && j.EffectiveDate <= ToDateTimeMonthYearExtensions.LastDayOfWeek(weeklyHoursWorked.StartDate)).WorkSiteContribution ?? 0;
            var calculatedWorkSiteContributionAmount = Math.Round(weeklyHoursWorked.Hours * closestWorkSiteContribution, 2);

            return Convert.ToDecimal(calculatedWorkSiteContributionAmount);
        }
    }
}
