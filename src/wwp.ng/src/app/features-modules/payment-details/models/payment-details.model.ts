import { Utilities } from './../../../shared/utilities';
import { Serializable } from '../../../shared/interfaces/serializable';
import * as moment from 'moment';

export class ParticipantPaymentHistory implements Serializable<ParticipantPaymentHistory> {
  effectiveMonth: number;
  issuanceMonth: string;

  participationBeginDate: Date;
  participationEndDate: Date;
  baseW2Payment: string;
  drugFelonPenalty: string;
  recoupment: string;
  learnfarePenalty: string;
  adjustedBasePayment: string;
  nonParticipationReduction: string;
  finalPayment: string;
  vendorPayment: string;
  participantPayment: string;
  createdDate: string;
  modifiedBy: string;
  modifiedDate: Date;
  isDelayed: boolean;
  isOriginal: boolean;
  header: string;
  switchCaseForHeader: string;
  pmtLbl: string;
  baseW2PaymentDelayedCycle: string;
  drugFelonPenaltyDelayedCycle: string;

  public static clone(input: any, instance: ParticipantPaymentHistory) {
    instance.effectiveMonth = input.effectiveMonth;
    instance.issuanceMonth = moment(input.participationEndDate).format('MM/YYYY');
    instance.participationBeginDate = input.participationBeginDate;
    instance.participationEndDate = input.participationEndDate;
    instance.baseW2Payment = (Math.round(input.baseW2Payment * 100) / 100).toFixed(2);
    instance.drugFelonPenalty = (Math.round(input.drugFelonPenalty * 100) / 100).toFixed(2);
    instance.recoupment = (Math.round(input.recoupment * 100) / 100).toFixed(2);
    instance.learnfarePenalty = (Math.round(input.learnfarePenalty * 100) / 100).toFixed(2);
    instance.adjustedBasePayment = (Math.round(input.adjustedBasePayment * 100) / 100).toFixed(2);
    instance.nonParticipationReduction = (Math.round(input.nonParticipationReduction * 100) / 100).toFixed(2);
    instance.finalPayment = (Math.round(input.finalPayment * 100) / 100).toFixed(2);
    instance.vendorPayment = (Math.round(input.vendorPayment * 100) / 100).toFixed(2);
    instance.participantPayment = (Math.round(input.participantPayment * 100) / 100).toFixed(2);
    instance.createdDate = moment(input.createdDate).format('MM/DD/YYYY');
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.isDelayed = input.isDelayed;
    instance.isOriginal = input.isOriginal;
  }

  public deserialize(input: any) {
    ParticipantPaymentHistory.clone(input, this);
    return this;
  }
}

export class ManualAuxiliary implements Serializable<ManualAuxiliary> {
  reason: string;
  amount: string;
  approvalDate: string;

  public static clone(input: any, instance: ManualAuxiliary) {
    instance.reason = input.reason;
    instance.amount = input.amount;
    instance.approvalDate = input.approvalDate;
  }

  public deserialize(input: any) {
    ManualAuxiliary.clone(input, this);
    return this;
  }
}
export class UnAppliedSanctionableHours implements Serializable<UnAppliedSanctionableHours> {
  unAppliedHours: string;
  lastUpdated: string;

  public static clone(input: any, instance: UnAppliedSanctionableHours) {
    instance.unAppliedHours = input.unAppliedHours ? (Math.round(input.unAppliedHours * 10) / 10).toFixed(1) : input.unAppliedHours;
    instance.lastUpdated = input.lastUpdated;
  }

  public deserialize(input: any) {
    UnAppliedSanctionableHours.clone(input, this);
    return this;
  }
}

export class PaymentDetails implements Serializable<PaymentDetails> {
  participantPaymentHistories: ParticipantPaymentHistory[];
  manualAuxiliaries: ManualAuxiliary[];
  UnAppliedSanctionableHours: UnAppliedSanctionableHours;

  public static clone(input: any, instance: PaymentDetails) {
    instance.participantPaymentHistories = Utilities.deserilizeChildren(input.participantPaymentHistories, ParticipantPaymentHistory, 0);
    instance.manualAuxiliaries = Utilities.deserilizeChildren(input.manualAuxiliaries, ManualAuxiliary, 0);
    instance.UnAppliedSanctionableHours = input.unAppliedSanctionableHours
      ? new UnAppliedSanctionableHours().deserialize(input.unAppliedSanctionableHours)
      : input.unAppliedSanctionableHours;
  }

  public deserialize(input: any) {
    PaymentDetails.clone(input, this);
    return this;
  }
}
