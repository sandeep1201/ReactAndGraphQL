import {ClockTypes} from '../../../models/time-limits/clocktypes';
import {BaseModelContract} from '../../../models/model-base-contract';
export class TimelineMonthContract extends BaseModelContract {
    public timelimitTypeId: number;
    public effectiveMonth: string;
    public isEdited: boolean;
    public paymentIssued: number;
    public reasonForChangeId: number;
    public isPlacement: boolean;
    public isState: boolean;
    public isFederal: boolean;
    public changeReasonDetails: string;
    public notes: string;
    public stateId: number;
  }
  
  export class ExtensionReasonContract extends BaseModelContract {
    public name: string;
    public validClockTypes: ClockTypes;
  }
  
  export class ExtensionContract extends BaseModelContract {
    public timelimitTypeId: number;
    public sequenceId: number;
    public extensionDecisionId: number;
    public startDate: string;
    public endDate: string;
    public decisionDate: string;
    public initialDiscussionDate: string;
    public approvalReasonId: number;
    public denialReasonId: number;
    public reasonDetails: string;
    public dvrReferralPending: boolean;
    public recieivingDvrServices: boolean;
    public pendingSSAppOrAppeal: boolean;
    public notes: string;
    public isBackdated: boolean;
  }
  
  
  export class ExtensionSequenceContract {
    public sequenceId: number;
    public timelimitTypeId: number;
    public extensions: ExtensionContract[];
  }
  
  export class ReasonForChangeContract extends BaseModelContract {
    public name: string;
    public isRequired: boolean;
    public code: string;
  
  }
  
  export class AssistanceGroupContract {
    public parents: AssistanceGroupMemberContract[];
    public children: AssistanceGroupMemberContract[];
  
  }
  
  export class AssistanceGroupMemberContract {
    public pin: number;
    public isSelectable: boolean;
    public relationship: string;
    public isPlaced: boolean;
  }