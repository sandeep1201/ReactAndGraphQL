import { Serializable } from 'src/app/shared/interfaces/serializable';
import { Utilities } from 'src/app/shared/utilities';
import { ValidationManager, ValidationResult, ValidationCode } from 'src/app/shared/models/validation';
import { FinalistAddress } from 'src/app/shared/models/finalist-address.model';
import { EAIPVStatus } from './ea-request-sections.enum';
import { MmDdYyyyValidationContext } from 'src/app/shared/interfaces/mmDdYyyy-validation-context';
import * as moment from 'moment';

export class EAIPV implements Serializable<EAIPV> {
  id: number;
  participantId: number;
  organizationCode: string;
  determinationDate: string;
  iPVNumber: number;
  reasonIds: number[];
  reasonCodes: string[];
  reasonNames: string[];
  occurrenceId: number;
  occurrenceCode: string;
  occurrenceName: string;
  mailingAddress: FinalistAddress;
  statusId: number;
  statusCode: EAIPVStatus;
  statusName: string;
  penaltyStartDate: string;
  penaltyEndDate: string;
  displayPenaltyEndDate: string;
  description: string;
  notes: string;
  countyId: number;
  countyName: string;
  modifiedBy: string;
  modifiedDate: string;
  isOverTurned = false;
  overTurnedDate: string;
  canView = false;
  canEdit = false;

  public static create() {
    const eaIPV = new EAIPV();
    eaIPV.id = 0;
    eaIPV.determinationDate = Utilities.currentDate.clone().format('MM/DD/YYYY');
    eaIPV.mailingAddress = new FinalistAddress();
    return eaIPV;
  }

  public static clone(input: any, instance: EAIPV) {
    instance.id = input.id;
    instance.participantId = input.participantId;
    instance.organizationCode = input.organizationCode;
    instance.determinationDate = input.determinationDate ? moment(input.determinationDate).format('MM/DD/YYYY') : input.determinationDate;
    instance.iPVNumber = input.ipvNumber;
    instance.reasonIds = Utilities.deserilizeArray(input.reasonIds);
    instance.reasonCodes = Utilities.deserilizeArray(input.reasonCodes);
    instance.reasonNames = Utilities.deserilizeArray(input.reasonNames);
    instance.occurrenceId = input.occurrenceId;
    instance.occurrenceCode = input.occurrenceCode;
    instance.occurrenceName = input.occurrenceName;
    instance.mailingAddress = input.mailingAddress ? Utilities.deserilizeChild(input.mailingAddress, FinalistAddress) : new FinalistAddress();
    instance.statusId = input.statusId;
    instance.statusCode = input.statusCode;
    instance.statusName = input.statusName;
    instance.penaltyStartDate = input.penaltyStartDate ? moment(input.penaltyStartDate).format('MM/DD/YYYY') : input.penaltyStartDate;
    instance.penaltyEndDate = input.penaltyEndDate;
    instance.displayPenaltyEndDate = input.penaltyEndDate ? moment(input.penaltyEndDate).format('MM/DD/YYYY') : 'Permanent';
    instance.description = input.description;
    instance.notes = input.notes;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.countyId = input.countyId;
    instance.countyName = input.countyName;
  }

  public deserialize(input: any) {
    EAIPV.clone(input, this);
    return this;
  }

  public validate(validationManager: ValidationManager, activeStatusId: number): ValidationResult {
    const result = new ValidationResult();

    this.validateDeterminationDate(validationManager, result);
    this.validateReasonDropDown(validationManager, result);
    this.validateOccurrenceDropDown(validationManager, result);
    this.validateMailingAddress(validationManager, result);
    this.validateOverTurnedDate(validationManager, result, activeStatusId);
    this.validateDescription(validationManager, result);
    return result;
  }

  private validateDeterminationDate(validationManager: ValidationManager, result: ValidationResult) {
    const currentDate = Utilities.currentDate.clone();
    const maxDeterminationDate = currentDate.format('MM/DD/YYYY');
    const minDeterminationDate = currentDate.subtract(30, 'days').format('MM/DD/YYYY');
    const applicatinDateContext: MmDdYyyyValidationContext = {
      date: this.determinationDate,
      prop: 'determinationDate',
      prettyProp: 'Determination Date',
      result: result,
      validationManager: validationManager,
      isRequired: true,
      maxDate: maxDeterminationDate,
      maxDateAllowSame: true,
      maxDateName: maxDeterminationDate,
      participantDOB: null,
      minDateAllowSame: false,
      minDate: minDeterminationDate,
      minDateName: minDeterminationDate
    };

    Utilities.validateMmDdYyyyDate(applicatinDateContext);
  }

  private validateReasonDropDown(validationManager: ValidationManager, result: ValidationResult) {
    if (!this.reasonIds || this.reasonIds.length === 0) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Reason(s)');
      result.addError('reasonIds');
    }
  }

  private validateOccurrenceDropDown(validationManager: ValidationManager, result: ValidationResult) {
    if (!this.occurrenceId) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Occurrence');
      result.addError('occurrenceId');
    }
  }

  private validateMailingAddress(validationManager: ValidationManager, result: ValidationResult) {
    let mailingAddressValidationResult = new ValidationResult();
    mailingAddressValidationResult = this.mailingAddress.validateSave(validationManager);
    Object.keys(mailingAddressValidationResult.errors).forEach(i => {
      result.addError(i);
    });
  }

  private validateOverTurnedDate(validationManager: ValidationManager, result: ValidationResult, activeStatusId: number) {
    if (this.isOverTurned && this.statusId === activeStatusId) {
      const maxOverTurnedDate = Utilities.currentDate.clone().format('MM/DD/YYYY');
      const minDateCanBackDate = moment(this.penaltyStartDate).format('MM/DD/YYYY');
      const overTurnedDateContext: MmDdYyyyValidationContext = {
        date: this.overTurnedDate,
        prop: 'overTurnedDate',
        prettyProp: 'Overturned Date',
        result: result,
        validationManager: validationManager,
        isRequired: true,
        maxDate: maxOverTurnedDate,
        maxDateAllowSame: true,
        maxDateName: maxOverTurnedDate,
        participantDOB: null,
        minDateAllowSame: false,
        minDate: minDateCanBackDate,
        minDateName: minDateCanBackDate
      };

      Utilities.validateMmDdYyyyDate(overTurnedDateContext);
    }
  }

  private validateDescription(validationManager: ValidationManager, result: ValidationResult) {
    if (Utilities.isStringEmptyOrNull(this.description)) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Description');
      result.addError('description');
    }
  }
}
