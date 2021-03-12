import { Serializable } from 'src/app/shared/interfaces/serializable';
import * as moment from 'moment';
import { ValidationManager, ValidationResult, ValidationCode } from 'src/app/shared/models/validation';
import { Utilities } from 'src/app/shared/utilities';
import { MmDdYyyyValidationContext } from 'src/app/shared/interfaces/mmDdYyyy-validation-context';
import { EAIndividualType } from './ea-request-sections.enum';
import { DropDownField } from 'src/app/shared/models/dropdown-field';

export class EAGroupMembers implements Serializable<EAGroupMembers> {
  eaGroupMembers: EARequestParticipant[];
  isPreviousMemberClicked: boolean;
  requestId: number;
  modifiedBy: string;
  modifiedDate: string;
  isSubmittedViaDriverFlow: boolean;

  public static create() {
    const eaGroupMembers = new EAGroupMembers();
    eaGroupMembers.requestId = 0;
    eaGroupMembers.eaGroupMembers = [];
    return eaGroupMembers;
  }

  public static clone(input: any, instance: EAGroupMembers) {
    instance.eaGroupMembers = Utilities.deserilizeChildren(input.eaGroupMembers, EARequestParticipant, 0);
    instance.isPreviousMemberClicked = input.isPreviousMemberClicked;
    instance.requestId = input.requestId;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.isSubmittedViaDriverFlow = input.isSubmittedViaDriverFlow;
  }

  public deserialize(input: any) {
    EAGroupMembers.clone(input, this);
    return this;
  }
}

export class EARequestParticipant implements Serializable<EARequestParticipant> {
  id: number;
  participantId: number;
  pinNumber: number;
  firstName: string;
  middleInitial: string;
  lastName: string;
  suffixName: string;
  participantDOB: string;
  participantGender: string;
  eaRequestId: number;
  eaIndividualTypeId: number;
  eaIndividualTypeCode: EAIndividualType;
  eaIndividualTypeName: string;
  eaRelationTypeId: number;
  eaRelationTypeName: string;
  isDeleted: boolean;
  isIncluded: boolean;
  ssn: string;
  ssnAppliedDate: string;
  ssnExemptTypeId: number;
  ssnExemptTypeName: string;
  modifiedBy: string;
  modifiedDate: string;

  //non-api
  add: boolean;
  alreadyAdded: boolean;
  participantName: string;

  public static validate(
    validationManager: ValidationManager,
    model: EARequestParticipant[],
    eaIndividualTypeDrop: DropDownField[],
    unIncludableParticipants?: EARequestParticipant[]
  ): ValidationResult {
    const result = new ValidationResult();
    model.forEach((x, index) => {
      if (x.isIncluded && (!x.eaIndividualTypeId || x.eaIndividualTypeId === null)) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Individual Type.');
        result.addError(`individualTypeId${index}`);
      } else if (
        x.isIncluded &&
        Utilities.fieldDataCodeById(x.eaIndividualTypeId, eaIndividualTypeDrop) === EAIndividualType.DependentChild &&
        Utilities.currentDate.clone().diff(x.participantDOB, 'years') > 18
      ) {
        validationManager.addErrorWithDetail(ValidationCode.DuplicateDataWithNoMessage, 'Dependent Child is not a minor.');
        result.addError(`individualTypeId${index}`);
      }
      if (x.isIncluded && (!x.eaRelationTypeId || x.eaRelationTypeId === null)) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Relationship.');
        result.addError(`relationshipTypeId${index}`);
      }
      if (x.isIncluded === undefined) {
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Include in EA Group.');
        result.addError(`isIncluded${index}`);
      }
      if (!x.ssn && x.isIncluded) {
        if (!x.ssnAppliedDate && !x.ssnExemptTypeId) {
          validationManager.addErrorWithDetail(ValidationCode.DuplicateDataWithNoMessage, 'When SSN is unknown either SSN Application Date or Exempt is required.');
          result.addError(`ssnAppliedDate${index}`);
          result.addError(`ssnExemptTypeId${index}`);
        } else if (x.ssnAppliedDate && !x.ssnExemptTypeId) {
          const currentDate = moment(Utilities.currentDate).format('MM/DD/YYYY');
          const applicatinDateContext: MmDdYyyyValidationContext = {
            date: x.ssnAppliedDate,
            prop: `ssnAppliedDate${index}`,
            prettyProp: 'SSN Application Date',
            result: result,
            validationManager: validationManager,
            isRequired: true,
            maxDate: currentDate,
            maxDateAllowSame: true,
            maxDateName: currentDate,
            participantDOB: null,
            minDateAllowSame: false,
            minDate: null,
            minDateName: null
          };
          Utilities.validateMmDdYyyyDate(applicatinDateContext);
          if (!!result.errors[`ssnAppliedDate${index}`]) {
            result.addError('disableSave');
          }
          if (!result.errors[`ssnAppliedDate${index}`] && moment(x.ssnAppliedDate) <= moment().subtract(6, 'months')) {
            validationManager.addErrorWithDetail(
              ValidationCode.DuplicateDataWithNoMessage,
              'SSN Application Date is older than 6 months for a group member. Please enter/request individual’s SSN.'
            );
            result.addError(`ssnAppliedDate${index}`);
          }
        }
      }
      if (unIncludableParticipants && unIncludableParticipants.find(y => x.participantId === y.participantId)) {
        validationManager.addErrorWithDetail(
          ValidationCode.DuplicateDataWithNoMessage,
          'Group member has income and/or assets that must be deleted before Include in EA group can be set to No.'
        );
        result.addError(`isIncluded${index}`);
        result.addError(`disableSave`);
      }
    });
    return result;
  }

  public static create(participantId: number, pin: number, eaIndividualTypeId?: number, eaRelationTypeId?: number): EARequestParticipant {
    const eARequestParticipant = new EARequestParticipant();
    eARequestParticipant.id = 0;
    eARequestParticipant.participantId = participantId;
    eARequestParticipant.pinNumber = pin;
    eARequestParticipant.eaIndividualTypeId = eaIndividualTypeId;
    eARequestParticipant.eaRelationTypeId = eaRelationTypeId;
    eARequestParticipant.isIncluded = true;
    return eARequestParticipant;
  }

  public static clone(input: any, instance: EARequestParticipant) {
    instance.id = input.id;
    instance.participantId = input.participantId;
    instance.pinNumber = input.pinNumber;
    instance.firstName = input.firstName;
    instance.middleInitial = input.middleInitial;
    instance.lastName = input.lastName;
    instance.suffixName = input.suffixName;
    instance.participantName = Utilities.formatDisplayPersonName(input.firstName, input.middleInitial, input.lastName, input.suffixName, true);
    instance.participantDOB = input.participantDOB ? moment(input.participantDOB).format('MM/DD/YYYY') : input.participantDOB;
    instance.eaRequestId = input.eaRequestId;
    instance.eaIndividualTypeId = input.eaIndividualTypeId;
    instance.eaIndividualTypeCode = input.eaIndividualTypeCode;
    instance.eaIndividualTypeName = input.eaIndividualTypeName;
    instance.eaRelationTypeId = input.eaRelationTypeId;
    instance.eaRelationTypeName = input.eaRelationTypeName;
    instance.isDeleted = input.isDeleted;
    instance.isIncluded = input.isIncluded;
    instance.ssn = input.ssn;
    instance.ssnAppliedDate = input.ssnAppliedDate ? moment(input.ssnAppliedDate).format('MM/DD/YYYY') : input.ssnAppliedDate;
    instance.ssnExemptTypeId = input.ssnExemptTypeId;
    instance.ssnExemptTypeName = input.ssnExemptTypeName;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
  }

  public deserialize(input: any) {
    EARequestParticipant.clone(input, this);
    return this;
  }
}

export class EAPreviousGroupMembers implements Serializable<EAPreviousGroupMembers> {
  caseNumber: string;
  pinNumber: number;
  otherPersonPinNumber: number;
  otherPersonFirstName: string;
  otherPersonLastName: string;
  otherPersonDOB: string;
  relationship: string;
  otherPersonAge: number;
  otherPersonGender: string;
  ssn: string;
  ids: number;

  public static clone(input: any, instance: EAPreviousGroupMembers) {
    instance.caseNumber = input.caseNumber;
    instance.pinNumber = input.pinNumber;
    instance.otherPersonFirstName = input.otherPersonFirstName;
    instance.otherPersonLastName = input.otherPersonLastName;
    instance.otherPersonPinNumber = input.otherPersonPinNumber;
    instance.otherPersonDOB = input.otherPersonDOB ? moment(input.otherPersonDOB).format('MM/DD/YYYY') : input.otherPersonDOB;
    instance.relationship = input.relationship;
    instance.otherPersonAge = input.otherPersonAge;
    instance.otherPersonGender = input.otherPersonGender;
    instance.ssn = input.ssn;
    instance.ids = input.ids;
  }

  public static createRequestParticipantModel(input: EAPreviousGroupMembers): EARequestParticipant {
    const earp = new EARequestParticipant();
    earp.id = 0;
    earp.pinNumber = input.otherPersonPinNumber;
    earp.participantName = `${input.otherPersonFirstName} ${input.otherPersonLastName}`;
    earp.participantDOB = input.otherPersonDOB;
    earp.participantGender = input.otherPersonGender;
    earp.ssn = input.ssn;
    earp.add = false;
    return earp;
  }

  public deserialize(input: any) {
    EAPreviousGroupMembers.clone(input, this);
    return this;
  }
}
