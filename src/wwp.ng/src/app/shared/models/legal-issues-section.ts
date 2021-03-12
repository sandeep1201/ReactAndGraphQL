// tslint:disable: no-use-before-declare
import { ActionNeeded } from '../../features-modules/actions-needed/models/action-needed-new';
import { MmDdYyyyValidationContext } from '../interfaces/mmDdYyyy-validation-context';
import { MmYyyyValidationContext } from '../interfaces/mmYyyy-validation-context';
import { ValidationManager } from './validation-manager';
import { ValidationResult } from './validation-result';
import { Utilities } from '../utilities';
import * as moment from 'moment';
import { AppService } from 'src/app/core/services/app.service';

export class LegalIssuesSection {
  isSubmittedViaDriverFlow: boolean;
  isConvictedOfCrime = false;
  convictions: CriminalCharge[];
  deletedConvictions: CriminalCharge[];
  isUnderCommunitySupervision: boolean;
  communitySupervisonDetails: string;
  supervisionContactId: number;
  isPending: boolean;
  pendings: CriminalCharge[];
  hasRestrainingOrders: boolean;
  restrainingOrderNotes: string;
  hasRestrainingOrderToPrevent: boolean;
  restrainingOrderToPreventNotes: string;
  hasFamilyLegalIssues: boolean;
  familyLegalIssueNotes: string;
  hasUpcomingCourtDates: boolean;
  courtDates: CourtDate[];
  actionNeeded: ActionNeeded;
  notes: string;
  rowVersion: string;
  modifiedBy: string;
  modifiedDate: number;

  public static clone(input, instance: LegalIssuesSection) {
    instance.isSubmittedViaDriverFlow = input.isSubmittedViaDriverFlow;
    instance.isConvictedOfCrime = input.isConvictedOfCrime;
    instance.convictions = Utilities.deserilizeChildren(input.convictions, CriminalCharge);
    instance.deletedConvictions = [];
    instance.isUnderCommunitySupervision = input.isUnderCommunitySupervision;
    instance.communitySupervisonDetails = input.communitySupervisonDetails;
    instance.supervisionContactId = input.supervisionContactId;
    instance.isPending = input.isPending;
    instance.hasRestrainingOrders = input.hasRestrainingOrders;
    instance.restrainingOrderNotes = input.restrainingOrderNotes;
    instance.hasRestrainingOrderToPrevent = input.hasRestrainingOrderToPrevent;
    instance.restrainingOrderToPreventNotes = input.restrainingOrderToPreventNotes;
    instance.pendings = Utilities.deserilizeChildren(input.pendings, CriminalCharge);
    instance.hasFamilyLegalIssues = input.hasFamilyLegalIssues;
    instance.familyLegalIssueNotes = input.familyLegalIssueNotes;
    instance.hasUpcomingCourtDates = input.hasUpcomingCourtDates;
    instance.courtDates = Utilities.deserilizeChildren(input.courtDates, CourtDate);
    instance.actionNeeded = Utilities.deserilizeChild(input.actionNeeded, ActionNeeded);
    instance.notes = input.notes;
    instance.rowVersion = input.rowVersion;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
  }

  deserialize(input: any) {
    LegalIssuesSection.clone(input, this);
    return this;
  }

  public validate(validationManager: ValidationManager, participantDOB: string): ValidationResult {
    const result = new ValidationResult();

    Utilities.validateRequiredYesNo(result, validationManager, this.isConvictedOfCrime, 'isConvictedOfCrime', 'Have you ever been convicted of a misdemeanor or felony?');
    if (this.isConvictedOfCrime === true && this.convictions != null) {
      const validationParms: any[] = [];
      validationParms.push(validationManager);
      validationParms.push(participantDOB);
      validationParms.push('convictions');
      Utilities.validateRepeater('convictions', this.convictions, validationParms, result, validationManager);
    }
    if (this.isConvictedOfCrime === true) {
      Utilities.validateRequiredYesNo(result, validationManager, this.isUnderCommunitySupervision, 'isUnderCommunitySupervision', 'Are you currently under community supervision?');
      if (this.isConvictedOfCrime === true && this.isUnderCommunitySupervision === true) {
        Utilities.validateRequiredText(
          this.communitySupervisonDetails,
          'communitySupervisonDetails',
          'Are you currently under community supervision? - Details',
          result,
          validationManager
        );
      }
    }

    Utilities.validateRequiredYesNo(result, validationManager, this.isPending, 'isPending', 'Do you have any pending charges?');
    if (this.isPending === true && this.pendings != null) {
      const validationParms: any[] = [];
      validationParms.push(validationManager);
      validationParms.push(participantDOB);
      validationParms.push('pendings');
      Utilities.validateRepeater('pendings', this.pendings, validationParms, result, validationManager);
    }
    Utilities.validateRequiredYesNo(result, validationManager, this.hasRestrainingOrders, 'hasRestrainingOrders', 'Are there currently any restraining orders against you?');
    if (this.hasRestrainingOrders === true) {
      Utilities.validateRequiredText(
        this.restrainingOrderNotes,
        'restrainingOrderNotes',
        'Are there currently any restraining orders against you? - Details',
        result,
        validationManager
      );
    }
    Utilities.validateRequiredYesNo(
      result,
      validationManager,
      this.hasRestrainingOrderToPrevent,
      'hasRestrainingOrderToPrevent',
      'Do you currently have a restraining order against anyone to prevent that person from contacting you?'
    );
    if (this.hasRestrainingOrderToPrevent === true) {
      Utilities.validateRequiredText(
        this.restrainingOrderToPreventNotes,
        'restrainingOrderToPreventNotes',
        'Do you currently have a restraining order against anyone to prevent that person from contacting you? - Details',
        result,
        validationManager
      );
    }

    Utilities.validateRequiredYesNo(result, validationManager, this.hasFamilyLegalIssues, 'hasFamilyLegalIssues', 'Do you have any immediate family members with legal issues?');

    if (this.hasFamilyLegalIssues === true) {
      Utilities.validateRequiredText(
        this.familyLegalIssueNotes,
        'familyLegalIssueNotes',
        'Do you have any immediate family members with legal issues? - Details',
        result,
        validationManager
      );
    }

    Utilities.validateRequiredYesNo(
      result,
      validationManager,
      this.hasUpcomingCourtDates,
      'hasUpcomingCourtDates',
      'Have you been ordered to appear for any upcoming court dates?'
    );

    if (this.hasUpcomingCourtDates === true && this.courtDates != null) {
      const validationParms: any[] = [];
      validationParms.push(validationManager);
      validationParms.push(participantDOB);
      Utilities.validateRepeater('courtDates', this.courtDates, validationParms, result, validationManager);
    }

    const anResult = this.actionNeeded.validate(validationManager);

    // TODO: wire up anResult better.
    if (anResult.isValid === false) {
      result.addError('actionNeeded');
    }

    return result;
  }
}

export class CriminalCharge {
  id: number;
  type: number;
  typeName: string;
  date: string;
  isDateUnknown: boolean;
  details: string;
  deleteReasonId: number;

  public static create(): CriminalCharge {
    const criminalCharge = new CriminalCharge();
    criminalCharge.id = 0;

    return criminalCharge;
  }

  public isEmpty(): boolean {
    // All properties have to be null or blank to make the entire object empty.
    return (
      (this.type == null || this.type.toString() === '') &&
      (this.date == null || this.date.trim() === '') &&
      (this.isDateUnknown == null || this.isDateUnknown.toString() === '' || this.isDateUnknown === false) &&
      (this.details == null || this.details.trim() === '')
    );
  }

  deserialize(input: any) {
    this.id = input.id;
    this.type = input.type;
    this.typeName = input.typeName;
    this.date = input.date;
    this.isDateUnknown = input.isDateUnknown;
    this.details = input.details;
    this.deleteReasonId = input.deleteReasonId;
    return this;
  }

  public clear(): void {
    this.type = null;
    this.date = null;
    this.isDateUnknown = null;
    this.details = null;
  }

  public validate(validationManager: ValidationManager, participantDob: string, type: string): ValidationResult {
    const result = new ValidationResult();
    Utilities.validateDropDown(this.type, 'type', 'Type', result, validationManager);

    if (this.isDateUnknown !== true) {
      const currentDate = Utilities.currentDate.day(1).format('MM/DD/YYYY');
      let prettyProp = '';
      if (type === 'convictions') {
        prettyProp = 'Month and Year of Conviction';
      } else if (type === 'pendings') {
        prettyProp = 'Month and Year of Charge';
      } else {
        prettyProp = 'Date';
      }
      // Must be after DOB.
      // Cannot be in future.
      const dateContext: MmYyyyValidationContext = {
        date: this.date,
        prop: 'date',
        prettyProp: prettyProp,
        result: result,
        validationManager: validationManager,
        isRequired: true,
        minDate: participantDob,
        minDateAllowSame: false,
        minDateName: "Participant's DOB",
        maxDate: currentDate,
        maxDateAllowSame: true,
        maxDateName: 'Current Date',
        participantDOB: moment(participantDob)
      };
      Utilities.validateMmYyyyDate(dateContext);
    }

    Utilities.validateRequiredText(this.details, 'details', 'Details', result, validationManager);
    return result;
  }
}

export class CourtDate {
  id: number;
  location: string;
  date: string;
  isDateUnknown: boolean;
  details: string;

  public static create(): CourtDate {
    const courtDate = new CourtDate();
    courtDate.id = 0;

    return courtDate;
  }

  public clear(): void {
    this.location = null;
    this.date = null;
    this.isDateUnknown = null;
    this.details = null;
  }

  public isEmpty(): boolean {
    // All properties have to be null or blank to make the entire object empty.
    return (
      (this.location == null || this.location.trim() === '') &&
      (this.date == null || this.date.trim() === '') &&
      (this.isDateUnknown == null || this.isDateUnknown.toString() === '') &&
      (this.details == null || this.details.trim() === '')
    );
  }

  deserialize(input: any) {
    this.id = input.id;
    this.location = input.location;
    this.date = input.date;
    this.isDateUnknown = input.isDateUnknown;
    this.details = input.details;

    return this;
  }

  public validate(validationManager: ValidationManager, participantDob: string): ValidationResult {
    const result = new ValidationResult();

    Utilities.validateRequiredText(this.location, 'location', 'Location', result, validationManager);

    if (this.isDateUnknown !== true) {
      const courtDateValidationContext: MmDdYyyyValidationContext = {
        date: this.date,
        prop: 'date',
        prettyProp: 'Court Date',
        result: result,
        validationManager: validationManager,
        isRequired: true,
        minDate: Utilities.currentDate.format('MM/DD/YYYY'),
        minDateAllowSame: true,
        minDateName: 'Current Date',
        maxDate: null,
        maxDateAllowSame: null,
        maxDateName: null,
        participantDOB: participantDob
      };
      Utilities.validateMmDdYyyyDate(courtDateValidationContext);
    }
    Utilities.validateRequiredText(this.details, 'details', 'Details', result, validationManager);

    return result;
  }
}
