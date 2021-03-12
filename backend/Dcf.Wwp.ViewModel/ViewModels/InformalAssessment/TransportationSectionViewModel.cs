using System;
using System.Collections.Generic;
using System.Linq;
using Dcf.Wwp.Api.Library.Contracts.InformalAssessment;
using Dcf.Wwp.Model.Interface;
using Dcf.Wwp.Model.Interface.Constants;
using Dcf.Wwp.Model.Interface.Core;
using Dcf.Wwp.Model.Interface.Repository;

namespace Dcf.Wwp.Api.Library.ViewModels.InformalAssessment
{
    public class TransportationSectionViewModel : BaseInformalAssessmentViewModel
    {
        public TransportationSectionViewModel(IRepository repo, IAuthUser authUser) : base(repo, authUser)
        {
        }

        public TransportationSectionContract GetData()
        {
            return GetContract(InformalAssessment, Participant, Repo);
        }

        public bool PostData(TransportationSectionContract contract, string userId)
        {
            var ia          = InformalAssessment;
            var participant = Participant;

            if (participant == null)
            {
                throw new InvalidOperationException("PIN not valid.");
            }

            if (contract == null)
            {
                throw new InvalidOperationException("TransportationSectionContract is missing.");
            }

            if (ia == null)
            {
                throw new InvalidOperationException("InformalAssessment record is missing.");
            }

            var updateTime = DateTime.Now;

            try
            {
                var ts = participant.TransportationSection ?? Repo.NewTransportationSection(participant.Id, userId);
                ts.ModifiedBy   = AuthUser.Username;
                ts.ModifiedDate = updateTime;
                Repo.StartChangeTracking(ts);

                var tas = ia.TransportationAssessmentSection ?? Repo.NewTransportationAssessmentSection(ia, userId);
                tas.ModifiedBy   = AuthUser.Username;
                tas.ModifiedDate = updateTime;
                Repo.StartChangeTracking(tas);

                // To force a new assessment section to be modified so that it
                // is registered as changed, we update the ReviewCompleted.
                tas.ReviewCompleted = true;

                var userRowVersion       = contract.RowVersion;
                var assessmentRowVersion = contract.AssessmentRowVersion;

                // First go through all the existing bridge records and soft delete them...
                ts.TransportationSectionMethodBridges?.ForEach(bridgeRow => bridgeRow.IsDeleted = true);

                // Now go and restore or create new records for
                // the ones that are sent via the contract.
                foreach (var methodId in contract.Methods)
                {
                    var restore = ts.AllTransportationSectionMethodBridges?.FirstOrDefault(z => z.TransporationTypeId == methodId);

                    if (restore != null)
                    {
                        restore.IsDeleted = false;
                    }
                    else
                    {
                        var newBridgeRow = Repo.NewTransportationSectionMethodBridge(ts, userId);
                        newBridgeRow.TransporationTypeId = methodId;
                    }
                }

                ts.TransporationDetails = contract.MethodDetails;

                // get only the transportation types requiring insurance or veh. registrations
                var transTypes = Repo.GetTransportationTypesWhere(i => i.RequiresInsurance == true && i.RequiresCurrentRegistration == true && !i.IsDeleted);

                // does participant have a personal or borrowed vehicle in contract transportation methods collection?
                var mustHaveInsOrReg = transTypes?.Any(i => contract.Methods.Contains(i.Id));

                mustHaveInsOrReg = mustHaveInsOrReg ?? false;

                if (mustHaveInsOrReg == true)
                {
                    // yes - assign and save the values to those two questions
                    ts.IsVehicleInsuredId                = contract.IsVehicleInsuredId;
                    ts.VehicleInsuredDetails             = contract.VehicleInsuredDetails;
                    ts.IsVehicleRegistrationCurrentId    = contract.IsVehicleRegistrationCurrentId;
                    ts.VehicleRegistrationCurrentDetails = contract.VehicleRegistrationCurrentDetails;
                }
                else
                {
                    // no, only other types of methods - clear out the following (so they don't save)
                    ts.IsVehicleInsuredId                = null;
                    ts.VehicleInsuredDetails             = null;
                    ts.IsVehicleRegistrationCurrentId    = null;
                    ts.VehicleRegistrationCurrentDetails = null;
                }

                ts.HasValidDrivingLicense = contract.HasValidDriversLicense;

                if (ts.HasValidDrivingLicense == true)
                {
                    ts.DriversLicenseStateId         = contract.DriversLicenseStateId;
                    ts.DriversLicenseExpirationDate  = contract.DriversLicenseExpirationDate;
                    ts.DriversLicenseDetails         = contract.DriversLicenseDetails;
                    ts.DriversLicenseInvalidReasonId = null;
                    ts.DriversLicenseInvalidDetails  = null;
                }
                else
                {
                    if (ts.HasValidDrivingLicense == false)
                    {
                        ts.DriversLicenseInvalidReasonId = contract.DriversLicenseInvalidReasonId;
                        ts.DriversLicenseInvalidDetails  = contract.DriversLicenseInvalidDetails;
                        ts.DriversLicenseStateId         = null;
                        ts.DriversLicenseExpirationDate  = null;
                        ts.DriversLicenseDetails         = null;
                    }
                }

                ts.HadCommercialDriversLicense = contract.HadCommercialDriversLicense;

                // HadCommercialDriversLicense only shows when user answers yes to having a valid license.
                if (ts.DriversLicenseInvalidReasonId == TransportationSectionLookUps.NeverAppliedforaLicense && ts.HasValidDrivingLicense == false)
                {
                    ts.HadCommercialDriversLicense      = null;
                    ts.IsCommercialDriversLicenseActive = null;
                    ts.CommercialDriversLicenseDetails  = null;
                }
                else
                {
                    if (ts.HadCommercialDriversLicense == true)
                    {
                        ts.IsCommercialDriversLicenseActive = contract.IsCommercialDriversLicenseActive;
                        ts.CommercialDriversLicenseDetails  = contract.CommercialDriversLicenseDetails;
                    }
                    else
                    {
                        ts.IsCommercialDriversLicenseActive = null;
                        ts.CommercialDriversLicenseDetails  = null;
                    }
                }

                ts.Notes = contract.Notes;

                // If for any weird reason this was deleted we are going to un-delete it.
                ts.IsDeleted = false;

                // Do a concurrency check.
                if (!Repo.IsRowVersionStillCurrent(ts, userRowVersion))
                {
                    return false;
                }

                if (!Repo.IsRowVersionStillCurrent(tas, assessmentRowVersion))
                {
                    return false;
                }

                var currentIA = Repo.GetMostRecentAssessment(participant);
                currentIA.ModifiedBy   = AuthUser.Username;
                currentIA.ModifiedDate = updateTime;

                // If the first save completes, it actually has already saved the Assessment Section
                // object as well since they are on the save repository context.  But if the Section
                // didn't need saving, we still need to SaveIfChangd on the Assessment Section.
                if (!Repo.SaveIfChanged(ts, userId))
                {
                    Repo.SaveIfChanged(tas, userId);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                throw;
            }

            return true;
        }

        public static TransportationSectionContract GetContract(IInformalAssessment ia, IParticipant participant, IRepository repository)
        {
            var contract = new TransportationSectionContract();

            if (ia != null && participant != null)
            {
                // Since the action needed is not dependent upon any section
                // or assessment section data, we'll add it right away.
                contract.ActionNeeded = ActionNeededViewModel.GetActionNeededContract(participant, repository, ActionNeededPage.Transportation);

                if (participant.TransportationSection != null)
                {
                    var section = participant.TransportationSection;

                    if (section.TransportationSectionMethodBridges != null)
                    {
                        foreach (var bridgeRow in section.TransportationSectionMethodBridges)
                        {
                            if (bridgeRow.TransporationTypeId.HasValue)
                            {
                                contract.Methods.Add(bridgeRow.TransporationTypeId.Value);
                                contract.TransportationMethods.Add(bridgeRow.TransportationType?.Name);
                            }
                        }
                    }

                    contract.MethodDetails                     = section.TransporationDetails;
                    contract.IsVehicleInsuredId                = section.IsVehicleInsuredId;
                    contract.IsVehicleInsuredName              = section.IsVehicleInsuredYesNoUnknownLookup?.Name;
                    contract.VehicleInsuredDetails             = section.VehicleInsuredDetails;
                    contract.IsVehicleRegistrationCurrentId    = section.IsVehicleRegistrationCurrentId;
                    contract.IsVehicleRegistrationCurrentName  = section.IsVehicleRegistrationCurrentYesNoUnknownLookup?.Name;
                    contract.VehicleRegistrationCurrentDetails = section.VehicleRegistrationCurrentDetails;
                    contract.HasValidDriversLicense            = section.HasValidDrivingLicense;
                    contract.DriversLicenseStateId             = section.DriversLicenseStateId;
                    contract.DriversLicenseStateName           = section.State?.Name;
                    contract.DriversLicenseExpirationDate      = section.DriversLicenseExpirationDate;
                    contract.DriversLicenseDetails             = section.DriversLicenseDetails;
                    contract.DriversLicenseInvalidReasonId     = section.DriversLicenseInvalidReasonId;
                    contract.DriversLicenseInvalidReasonName   = section.DriversLicenseInvalidReasonType?.Name;
                    contract.DriversLicenseInvalidDetails      = section.DriversLicenseInvalidDetails;
                    contract.HadCommercialDriversLicense       = section.HadCommercialDriversLicense;
                    contract.IsCommercialDriversLicenseActive  = section.IsCommercialDriversLicenseActive;
                    contract.CommercialDriversLicenseDetails   = section.CommercialDriversLicenseDetails;

                    contract.Notes = section.Notes;

                    // Standard modified by/rowversion stuff
                    contract.RowVersion   = section.RowVersion;
                    contract.ModifiedBy   = section.ModifiedBy;
                    contract.ModifiedDate = section.ModifiedDate;
                }

                if (ia.TransportationAssessmentSection != null)
                {
                    contract.IsSubmittedViaDriverFlow = true;
                    contract.AssessmentRowVersion     = ia.TransportationAssessmentSection.RowVersion;
                }
            }

            return (contract);
        }
    }
}
