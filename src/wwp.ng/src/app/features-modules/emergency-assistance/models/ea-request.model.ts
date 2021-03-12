import { EAHouseholdFinancialsSection } from './ea-request-financials.model';
import { EARequestEmergencyTypeSection } from './ea-request-emergency-type.model';
import { EARequestDemographicsSection } from './ea-request-demographics.model';
import { Serializable } from 'src/app/shared/interfaces/serializable';
import { Utilities } from 'src/app/shared/utilities';
import { EAGroupMembers } from './ea-request-participant.model';
import { EAAgencySummarySection } from './ea-request-agency-summary.model';
import { CommentModel } from 'src/app/shared/components/comment/comment.model';
import { EAStatusCodes } from './ea-request-sections.enum';
import { EAPayment } from './ea-request-payment.model';

export class EARequest implements Serializable<EARequest> {
  id: number;
  requestNumber: number;
  caresCaseNumber: number;
  statusId: number;
  statusName: string;
  statusCode: EAStatusCodes;
  statusReasonIds: number[];
  statusReasonNames: string[];
  statusLastUpdated: string;
  statusDeadLine: string;
  approvedPaymentAmount: string;
  organizationId: number;
  organizationCode: string;
  organizationName: string;
  eaDemographics: EARequestDemographicsSection;
  eaEmergencyType: EARequestEmergencyTypeSection;
  eaGroupMembers: EAGroupMembers;
  eaHouseHoldFinancials: EAHouseholdFinancialsSection;
  eaAgencySummary: EAAgencySummarySection;
  eaComments: CommentModel[];
  eaPayments: EAPayment[];

  public static create() {
    const ea = new EARequest();
    ea.id = 0;
    ea.eaDemographics = EARequestDemographicsSection.create();
    ea.eaEmergencyType = EARequestEmergencyTypeSection.create();
    ea.eaGroupMembers = EAGroupMembers.create();
    ea.eaHouseHoldFinancials = EAHouseholdFinancialsSection.create();
    ea.eaAgencySummary = EAAgencySummarySection.create();
    ea.eaComments = [];
    return ea;
  }

  public static clone(input: any, instance: EARequest) {
    instance.id = input.id;
    instance.requestNumber = input.requestNumber;
    instance.caresCaseNumber = input.caresCaseNumber;
    instance.statusId = input.statusId;
    instance.statusName = input.statusName;
    instance.statusCode = input.statusCode;
    instance.statusReasonIds = Utilities.deserilizeArray(input.statusReasonIds);
    instance.statusReasonNames = Utilities.deserilizeArray(input.statusReasonNames);
    instance.statusLastUpdated = input.statusLastUpdated;
    instance.statusDeadLine = input.statusDeadLine;
    instance.approvedPaymentAmount = input.approvedPaymentAmount;
    instance.organizationId = input.organizationId;
    instance.organizationCode = input.organizationCode;
    instance.organizationName = input.organizationName;
    instance.eaDemographics = Utilities.deserilizeChild(input.eaDemographics, EARequestDemographicsSection);
    instance.eaEmergencyType = Utilities.deserilizeChild(input.eaEmergencyType, EARequestEmergencyTypeSection);
    instance.eaGroupMembers = Utilities.deserilizeChild(input.eaGroupMembers, EAGroupMembers);
    instance.eaHouseHoldFinancials = Utilities.deserilizeChild(input.eaHouseHoldFinancials, EAHouseholdFinancialsSection);
    instance.eaAgencySummary = Utilities.deserilizeChild(input.eaAgencySummary, EAAgencySummarySection);
    instance.eaComments = Utilities.deserilizeChildren(input.eaComments, CommentModel, 0);
    instance.eaPayments = Utilities.deserilizeChildren(input.eaPayments, EAPayment, 0);
  }

  public deserialize(input: any) {
    EARequest.clone(input, this);
    return this;
  }
}
