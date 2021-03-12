import * as moment from 'moment';

export interface ValidationContext {
  prop: string;
  prettyProp: string;
  isRequired: boolean;
}


export interface MmYyyyValidationContext {
  prop: string;
  prettyProp: string;
  isRequired: boolean;
  minDate: string;
  minDateAllowSame: boolean;
  minDateName: string;
  minDateUseParticipantDob: boolean;
  maxDate: string;
  maxDateAllowSame: boolean;
  maxDateName: string;
  maxDateUseParticipantDobPlus120: boolean;
  participantDob: moment.Moment;
}


export interface YyyyValidationContext {
  yyyy: number;
  prop: string;
  prettyProp: string;
  isRequired: boolean;
  minDateParticipantDobYear: number;
  maxDateNotInFuture: boolean;
}
