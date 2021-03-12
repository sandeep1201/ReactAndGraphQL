import { AppService } from 'src/app/core/services/app.service';
import { Serializable } from '../interfaces/serializable';
import { RfaChild } from './rfa-child.model';
import { PadPipe } from './../pipes/pad.pipe';
import { Utilities } from '../utilities';
import { MmDdYyyyValidationContext } from '../interfaces/mmDdYyyy-validation-context';
import { ValidationManager } from './validation-manager';
import { ValidationResult } from './validation-result';
import { Validate } from '../validate';
import { ValidationCode } from './validation-error';
import * as moment from 'moment';
import { EnrolledProgramCode } from '../enums/enrolled-program-code.enum';

export class RFAProgram implements Serializable<RFAProgram> {
  id: number;
  isOldRfa? = false;
  rfaNumber: number;
  programId: number;
  programName: string;
  programCode: string;
  statusId: number;
  statusName: string;
  statusDate: string;
  agencyName: string;
  canEditRfa = false;

  agencyCountyName: string;
  countyOfResidenceId: number;
  countyOfResidenceName: string;
  county: string;
  officeCountyId: string;
  applicationDate: string;
  applicationDueDate: string;
  disenrollmentDate: string;
  courtOrderCountyTribeId: number;
  courtOrderCountyTribeName: string;
  courtOrderCountyTribeNumber: number;
  workProgramOfficeId: number;
  workProgramOfficeName: string;
  workProgramOfficeNumber: number;
  workProgramOfficeCountyName: string;
  courtOrderEffectiveDate: string;
  contractorId: number;
  contractorName: string;
  contractorCode: string;

  completionReasonDetails: string;
  completionReasonId: number;
  annualHouseholdIncome: string;
  householdSize: number;
  lastDateOfEmployment: string;
  hasWorked16HoursLess: boolean;
  tjTmjIsEligibleForUnemployment: boolean;
  tjTmjIsReceivingW2Benefits: boolean;
  isUSCitizen: boolean;
  tjTmjHasWorked1040Hours: boolean;
  tjTmjIsAppCompleteAndSigned: boolean;
  tjTmjHasEligibilityBeenVerified: boolean;
  hasChildren: boolean;
  children: RfaChild[];
  tjTmjIsBenefitFromSubsidizedJob: boolean;
  tjTmjBenefitFromSubsidizedJobDetails: string;
  populationTypesIds: number[];
  populationTypesNames: string[];
  populationTypeDetails: string;
  rowVersion: string;
  modifiedBy: string;
  modifiedDate: string;
  isEligible: boolean;
  eligibilityCodes: string[];
  disenrolledDate: string;
  enrolledDate: string;
  tjTmjHasNeverEmployed = false;

  // These are Ids we will use for validation.
  tjId: number;
  tmjId: number;
  childrenFirstId: number;
  fcdpId: number;

  isVoluntary: boolean;
  kidsPin: number;
  referralSource: string;
  doesNotMeetCriteriaPopTypeId: number;
  fcdpTabId: number;
  public static clone(input: any, instance: RFAProgram) {
    instance.id = input.id;
    instance.rfaNumber = input.rfaNumber;
    instance.programId = input.programId;
    instance.programName = input.programName;
    instance.programCode = input.programCode;
    instance.statusId = input.statusId;
    instance.statusName = input.statusName;
    instance.statusDate = input.statusDate;
    instance.agencyName = input.agencyName;
    instance.agencyCountyName = input.agencyCountyName;
    instance.countyOfResidenceId = input.countyOfResidenceId;
    instance.countyOfResidenceName = input.countyOfResidenceName;
    instance.county = input.county;
    instance.applicationDate = input.applicationDate;
    instance.applicationDueDate = input.applicationDueDate;
    instance.disenrollmentDate = input.disenrollmentDate;
    instance.courtOrderEffectiveDate = input.courtOrderEffectiveDate;
    instance.workProgramOfficeId = input.workProgramOfficeId;
    instance.workProgramOfficeName = input.workProgramOfficeName;
    instance.workProgramOfficeNumber = input.workProgramOfficeNumber;
    instance.workProgramOfficeCountyName = input.workProgramOfficeCountyName;
    instance.courtOrderCountyTribeId = input.courtOrderCountyTribeId;
    instance.courtOrderCountyTribeName = input.courtOrderCountyTribeName;
    instance.courtOrderCountyTribeNumber = input.courtOrderCountyTribeNumber;
    instance.isVoluntary = input.isVoluntary;
    instance.kidsPin = input.kidsPin;
    instance.referralSource = input.referralSource;
    instance.hasChildren = input.hasChildren;
    instance.children = Utilities.deserilizeChildren(input.children, RfaChild, 0);
    instance.applicationDate = input.applicationDate;
    instance.applicationDueDate = input.applicationDueDate;
    instance.annualHouseholdIncome = input.annualHouseholdIncome;
    instance.householdSize = input.householdSize;
    instance.tjTmjHasNeverEmployed = input.tjTmjHasNeverEmployed;
    instance.lastDateOfEmployment = input.lastDateOfEmployment;
    instance.contractorId = input.contractorId;
    instance.contractorName = input.contractorName;
    instance.completionReasonDetails = input.completionReasonDetails;
    instance.completionReasonId = input.completionReasonId;
    instance.contractorCode = input.contractorCode;
    instance.hasWorked16HoursLess = input.hasWorked16HoursLess;
    instance.tjTmjIsEligibleForUnemployment = input.tjTmjIsEligibleForUnemployment;
    instance.tjTmjIsReceivingW2Benefits = input.tjTmjIsReceivingW2Benefits;
    instance.isUSCitizen = input.isUSCitizen;
    instance.tjTmjHasWorked1040Hours = input.tjTmjHasWorked1040Hours;
    instance.tjTmjIsAppCompleteAndSigned = input.tjTmjIsAppCompleteAndSigned;
    instance.tjTmjHasEligibilityBeenVerified = input.tjTmjHasEligibilityBeenVerified;
    instance.tjTmjIsBenefitFromSubsidizedJob = input.tjTmjIsBenefitFromSubsidizedJob;
    instance.tjTmjBenefitFromSubsidizedJobDetails = input.tjTmjBenefitFromSubsidizedJobDetails;
    instance.populationTypesIds = Utilities.deserilizeArray(input.populationTypesIds);
    instance.populationTypesNames = input.populationTypesNames;
    instance.populationTypeDetails = input.populationTypeDetails;
    instance.eligibilityCodes = input.eligibilityCodes;
    instance.enrolledDate = input.enrolledDate;
    instance.disenrolledDate = input.disenrolledDate;
    instance.isEligible = input.isEligible;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.rowVersion = input.rowVersion;
  }

  public static create(): RFAProgram {
    const rfa = new RFAProgram();
    rfa.id = 0;
    return rfa;
  }

  public deserialize(input: any) {
    RFAProgram.clone(input, this);
    return this;
  }

  set courtOrderEffectiveDateMmDdYyyy(date) {
    if (date != null && date.length === 10 && moment(date, 'MM/DD/YYYY', true).isValid()) {
      this.courtOrderEffectiveDate = moment(date).toISOString();
    } else {
      this.courtOrderEffectiveDate = date;
    }
  }
  get courtOrderEffectiveDateMmDdYyyy() {
    if (this.courtOrderEffectiveDate != null && moment(this.courtOrderEffectiveDate, moment.ISO_8601, true).isValid()) {
      return moment(this.courtOrderEffectiveDate, moment.ISO_8601, true).format('MM/DD/YYYY');
    } else {
      return this.courtOrderEffectiveDate;
    }
  }

  set applicationDateMmDdYyyy(date) {
    if (date != null && date.length === 10 && moment(date, 'MM/DD/YYYY', true).isValid()) {
      this.applicationDate = moment(date).toISOString();
    } else {
      this.applicationDate = date;
    }
  }
  get applicationDateMmDdYyyy() {
    if (this.applicationDate != null && moment(this.applicationDate, moment.ISO_8601, true).isValid()) {
      return moment(this.applicationDate, moment.ISO_8601, true).format('MM/DD/YYYY');
    } else {
      return this.applicationDate;
    }
  }

  set lastDateOfEmploymentMmDdYyyy(date) {
    if (date != null && date.length === 10 && moment(date, 'MM/DD/YYYY', true).isValid()) {
      this.lastDateOfEmployment = moment(date).toISOString();
    } else {
      this.lastDateOfEmployment = date;
    }
  }

  get lastDateOfEmploymentMmDdYyyy() {
    if (this.lastDateOfEmployment != null && moment(this.lastDateOfEmployment, moment.ISO_8601, true).isValid()) {
      return moment(this.lastDateOfEmployment, moment.ISO_8601, true).format('MM/DD/YYYY');
    } else {
      return this.lastDateOfEmployment;
    }
  }

  get populationTypesNamesCsv(): string {
    if (this.populationTypesNames != null) {
      return this.populationTypesNames.join(', ');
    } else {
      return null;
    }
  }

  get displayedCountyWPOffice(): string {
    const pad = new PadPipe();
    return this.agencyCountyName + ' - ' + pad.transform(this.workProgramOfficeNumber, 4);
  }

  // US 1319: Program Type is a REQUIRED FIELD.
  get isProgramRequired(): boolean {
    return true;
  }

  // US 1320: County/Tribe Where Court Ordered is a REQUIRED FIELD.
  get isCourtOrderCountyTribeIdRequired(): boolean {
    if (this.programId === this.childrenFirstId) {
      return true;
    } else {
      return false;
    }
  }

  // US 1320: Effective date of Court Order is a REQUIRED FIELD.
  get iscourtOrderEffectiveDateRequired(): boolean {
    if (this.programId === this.childrenFirstId) {
      return true;
    } else {
      return false;
    }
  }

  get isFcdpCourtOrderEffectiveDateRequired(): boolean {
    if (this.programId === this.fcdpId && this.fcdpTabId) {
      return true;
    } else {
      return false;
    }
  }

  // US 1320: Effective date of Court Order is a REQUIRED FIELD.
  get isWorkProgramOfficeRequired(): boolean {
    if (this.programId === this.childrenFirstId) {
      return true;
    } else {
      return false;
    }
  }

  // US 1321: Contractor is a REQUIRED FIELD.
  get isContractorRequired(): boolean {
    return true;
  }

  // US 1321: County of Residence is a REQUIRED FIELD.
  get isCountyOfResidenceRequired(): boolean {
    return true;
  }

  // US 1321: Application Date is a REQUIRED FIELD.
  get isApplicationDateRequired(): boolean {
    return true;
  }

  // US 1321: Is the applicant the biological/adoptive parent - or relative and primary caregiver -
  // of child(ren) under the age of 18 is a REQUIRED FIELD.
  get isHasChildrenRequired(): boolean {
    return true;
  }

  // US 1321: HouseHold income is a REQUIRED FIELD.
  get isAnnualHouseHoldIncomeRequired(): boolean {
    return true;
  }

  // US 1321: HouseHold size is a REQUIRED FIELD.
  get isHouseholdSizeRequired(): boolean {
    return true;
  }

  // US 1321: Last date of employment is a REQUIRED FIELD.
  get isLastDateOfEmploymentRequired(): boolean {
    if (this.tjTmjHasNeverEmployed === true) {
      return false;
    } else {
      return true;
    }
  }

  // US 1321: Eligible for Unemployment Insurance Benefits is a REQUIRED FIELD.
  get tjTmjIsEligibleForUnemploymentRequired(): boolean {
    return true;
  }

  // US 1321: Is applicant receiving W-2 benefits or services is a REQUIRED FIELD.
  get isApplicantReceivingW2BenefitsRequired(): boolean {
    return true;
  }

  // US 1321: U.S Citizen is a REQUIRED FIELD.
  get isUSCitizenRequired(): boolean {
    return true;
  }

  // US 1321: Has applicant worked a total of 1,040 hours in TMJ/TJ in the past? is a REQUIRED FIELD.
  get isHasApplicant1040TotalHoursRequired(): boolean {
    return true;
  }

  // US 1321: In the last four weeks, has the applicant worked an average of 16 or less hours per week? is a REQUIRED FIELD.
  get ishasWorked16HoursLessRequired(): boolean {
    if (this.ishasWorked16HoursLessDisplayed) {
      return true;
    } else {
      return false;
    }
  }

  // TODO: add US.
  get ishasWorked16HoursLessDisplayed(): boolean {
    if (this.tjTmjHasNeverEmployed === true) {
      return false;
    }

    if (
      moment(this.lastDateOfEmployment, moment.ISO_8601, true) <= moment(Utilities.currentDate) &&
      moment(this.lastDateOfEmployment, moment.ISO_8601, true) >=
        moment(Utilities.currentDate)
          .startOf('day')
          .subtract(4, 'week')
    ) {
      return true;
    } else {
      return false;
    }
  }

  // US 1321: Has application been completed and signed? is a REQUIRED FIELD.
  get tjTmjIsAppCompleteAndSignedRequired(): boolean {
    return true;
  }

  // US 1321: Has eligibility information been verified? is a REQUIRED FIELD.
  get tjTmjHasEligibilityBeenVerifiedRequired(): boolean {
    return true;
  }

  // US 1321: Is applicant able to obtain and benefit from a subsidized job? is a REQUIRED FIELD.
  get isCanAbleApplicantBenefitFromSubsidizedJobRequired(): boolean {
    return true;
  }

  get isPopulationTypesRequired(): boolean {
    return true;
  }

  // US 1321: TODO.
  get isCanAbleApplicantBenefitFromSubsidizedJobDetailsRequired(): boolean {
    return true;
  }

  // US 1321: Is applicant able to obtain and benefit from a subsidized job - Details a DISPLAYED FIELD.
  get isCanAbleApplicantBenefitFromSubsidizedJobDetailsDisplayed(): boolean {
    if (this.tjTmjIsBenefitFromSubsidizedJob === false) {
      return true;
    } else {
      return false;
    }
  }

  // US 1349: Population details is displayed when "Does not meet criteria for target population" is selected.
  get isPopulationDetailsDisplayed(): boolean {
    if (this.populationTypesIds != null && this.populationTypesIds.indexOf(this.doesNotMeetCriteriaPopTypeId) > -1) {
      return true;
    } else {
      return false;
    }
  }

  // US 1349: Population details is required when "Does not meet criteria for target population" is selected.
  get isPopulationDetailsRequired(): boolean {
    if (this.populationTypesIds != null && this.populationTypesIds.indexOf(this.doesNotMeetCriteriaPopTypeId) > -1) {
      return true;
    } else {
      return false;
    }
  }

  get isTmjProgram(): boolean {
    if (this.programId === this.tmjId || Utilities.lowerCaseTrimAsNotNull(this.programCode) === EnrolledProgramCode.tmj) {
      return true;
    } else {
      return false;
    }
  }

  get isTJProgram(): boolean {
    if (this.programId === this.tjId || Utilities.lowerCaseTrimAsNotNull(this.programCode) === EnrolledProgramCode.tj) {
      return true;
    } else {
      return false;
    }
  }

  get isTJTmjProgram(): boolean {
    if (
      this.programId === this.tjId ||
      this.programId === this.tmjId ||
      Utilities.lowerCaseTrimAsNotNull(this.programCode) === EnrolledProgramCode.tj ||
      Utilities.lowerCaseTrimAsNotNull(this.programCode) === EnrolledProgramCode.tmj
    ) {
      return true;
    } else {
      return false;
    }
  }

  get isCFProgram(): boolean {
    if (this.programId === this.childrenFirstId || Utilities.lowerCaseTrimAsNotNull(this.programCode) === EnrolledProgramCode.cf) {
      return true;
    } else {
      return false;
    }
  }

  get isFCDPProgram(): boolean {
    if (this.programId === this.fcdpId || Utilities.lowerCaseTrimAsNotNull(this.programCode) === EnrolledProgramCode.fcdp) {
      return true;
    } else {
      return false;
    }
  }

  get isEnrolledStatus(): boolean {
    if (this.statusName === 'Enrolled') {
      return true;
    } else {
      return false;
    }
  }

  get isReferredStatus(): boolean {
    if (this.statusName === 'Referred') {
      return true;
    } else {
      return false;
    }
  }

  get isInProgressStatus(): boolean {
    if (this.statusName === 'In Progress') {
      return true;
    } else {
      return false;
    }
  }

  get canDetermineEligibility(): boolean {
    let canDetermine = false;
    if (this.isCFProgram) {
      if (
        this.programId > 0 &&
        this.countyOfResidenceId > 0 &&
        this.courtOrderCountyTribeId > 0 &&
        !Utilities.isStringEmptyOrNull(this.courtOrderEffectiveDateMmDdYyyy) &&
        this.workProgramOfficeId > 0
      ) {
        canDetermine = true;
      }
    }
    if (this.isFCDPProgram) {
      if (this.programId > 0 && (this.kidsPin && this.kidsPin.toString().length) > 0 && this.workProgramOfficeId > 0) {
        canDetermine = true;
      }
    } else if (this.isTJTmjProgram) {
      if (!this.isTmjTjEmpty()) {
        canDetermine = true;
      }
    }

    return canDetermine;
  }

  private isRepeaterFilledWithData() {
    let isFilled = false;
    for (let i = 0; i < this.children.length; i++) {
      if (
        Utilities.isRepeaterRowRequired(this.children, i) === true &&
        !Utilities.isStringEmptyOrNull(this.children[i].firstName) &&
        !Utilities.isStringEmptyOrNull(this.children[i].lastName) &&
        !Utilities.isStringEmptyOrNull(this.children[i].dateOfBirth) &&
        this.children[i].genderId !== null
      ) {
        isFilled = true;
      } else if (Utilities.isRepeaterRowRequired(this.children, i) === true && !this.children[i].isEmpty()) {
        isFilled = false;
        break;
      }
    }
    return isFilled;
  }

  public validate(validationManager: ValidationManager, participantDOB: string, fcdpCourtOrderId: number, isFromRefer?: boolean): ValidationResult {
    const result = new ValidationResult();
    if (this.programId === this.childrenFirstId) {
      //If program is CF, set isValid to true so that the refer button is enabled
      //if all the fields are filled.
      result.isValid = true;
      //Run validation if the Refer button is clicked
      if (isFromRefer) {
        //Formatting date as there is an IE specific issue. IE doesn't process invalid dates correctly.
        const formattedDt = moment(this.courtOrderEffectiveDateMmDdYyyy).format('MM/DD/YYYY');
        const effectiveCourtOrderContext: MmDdYyyyValidationContext = {
          date: formattedDt,
          prop: 'courtOrderEffectiveDateMmDdYyyy',
          prettyProp: 'Effective date of Court Order',
          result: result,
          validationManager: validationManager,
          isRequired: true,
          minDate: moment(Utilities.currentDate)
            .subtract(1, 'years')
            .format('MM/DD/YYYY'),
          minDateAllowSame: true,
          minDateName:
            'date (' +
            moment(Utilities.currentDate)
              .subtract(1, 'years')
              .format('MM/DD/YYYY') +
            ')',
          maxDate: moment(Utilities.currentDate)
            .add(1, 'years')
            .format('MM/DD/YYYY'),
          maxDateAllowSame: true,
          maxDateName:
            'date (' +
            moment(Utilities.currentDate)
              .add(1, 'years')
              .format('MM/DD/YYYY') +
            ')',
          participantDOB: null
        };
        Utilities.validateMmDdYyyyDate(effectiveCourtOrderContext);
      }
    } else if (this.programId === this.fcdpId) {
      if (isFromRefer) {
        //Kids Pin and Referral Source are validated only when called from refer button
        if (this.kidsPin === null || (this.kidsPin && this.kidsPin.toString() === '')) {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'KIDS PIN');
          result.addError('kidsPin');
        }
        if (this.kidsPin <= 0) {
          validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'KIDS PIN should be greater than 0');
          result.addError('kidsPin');
        }
        if (!this.referralSource || (this.referralSource && this.referralSource.trim() === '')) {
          validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Referral Source');
          result.addError('referralSource');
        }
      }

      if (fcdpCourtOrderId === 1 && this.courtOrderEffectiveDate) {
        //Formatting date as there is an IE specific issue. IE doesn't process invalid dates correctly.
        const formattedDt = moment(this.courtOrderEffectiveDateMmDdYyyy).format('MM/DD/YYYY');
        const effectiveCourtOrderContext: MmDdYyyyValidationContext = {
          date: formattedDt,
          prop: 'courtOrderEffectiveDateMmDdYyyy',
          prettyProp: 'Effective date of Court Order',
          result: result,
          validationManager: validationManager,
          isRequired: true,
          minDate: moment(Utilities.currentDate)
            .subtract(1, 'years')
            .format('MM/DD/YYYY'),
          minDateAllowSame: true,
          minDateName:
            'date (' +
            moment(Utilities.currentDate)
              .subtract(1, 'years')
              .format('MM/DD/YYYY') +
            ')',
          maxDate: moment(Utilities.currentDate)
            .add(1, 'years')
            .format('MM/DD/YYYY'),
          maxDateAllowSame: true,
          maxDateName:
            'date (' +
            moment(Utilities.currentDate)
              .add(1, 'years')
              .format('MM/DD/YYYY') +
            ')',
          participantDOB: null
        };
        Utilities.validateMmDdYyyyDate(effectiveCourtOrderContext);
      }
    } else if (this.programId === this.tjId || this.programId === this.tmjId) {
      Utilities.validateDropDown(this.contractorId, 'contractorId', 'Contractor', result, validationManager);

      const applicationDateContext: MmDdYyyyValidationContext = {
        date: this.applicationDateMmDdYyyy,
        prop: 'applicationDateMmDdYyyy',
        prettyProp: 'Application Date',
        result: result,
        validationManager: validationManager,
        isRequired: true,
        minDate: '02/01/2014',
        minDateAllowSame: true,
        minDateName: '02/01/2014',
        maxDate: moment(Utilities.currentDate).format('MM/DD/YYYY'),
        maxDateAllowSame: true,
        maxDateName: 'Current Date',
        participantDOB: null
      };
      Utilities.validateMmDdYyyyDate(applicationDateContext);

      Utilities.validateRequiredYesNo(
        result,
        validationManager,
        this.hasChildren,
        'hasChildren',
        'Is the applicant the biological/adoptive parent - or relative and primary caregiver - of child(ren) under the age of 18?'
      );

      if (this.hasChildren != null && this.hasChildren === true) {
        const errArr = result.createErrorsArray('children');

        let allItemsAreEmpty = true;

        // Check to see if all are empty.
        for (const wp of this.children) {
          if (!wp.isEmpty()) {
            allItemsAreEmpty = false;
            break;
          }
        }

        if (allItemsAreEmpty) {
          // If all are empty validate the first one.
          if (this.children[0] != null || this.children.length === 0) {
            if (this.children.length === 0) {
              const x = RfaChild.create();
              this.children.push(x);
            }
            errArr.push(this.children[0].validate(validationManager, result).errors);
          }
        } else {
          for (const c of this.children) {
            if (!c.isEmpty()) {
              const v = c.validate(validationManager);
              errArr.push(v.errors);
              if (v.isValid === false) {
                result.isValid = false;
              }
            } else {
              // Push empty when item is blank in order to keep correct index.
              const resultEmpty = new ValidationResult();
              errArr.push(resultEmpty.errors);
            }
          }
        }
      }

      if (!Utilities.validateRequiredNumber(result, validationManager, this.householdSize, 'householdSize', 'Household Size')) {
      } else if (+this.householdSize < 1 || +this.householdSize > 18) {
        validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Household size must be greater than 0 and and less than 19');
        result.addError('householdSize');
      }

      // TODO: Server sometimes sends decimal as server or string.
      let annualHouseholdIncomeString = '';
      if (this.annualHouseholdIncome != null) {
        annualHouseholdIncomeString = this.annualHouseholdIncome.toString();
      } else {
        annualHouseholdIncomeString = null;
      }
      Utilities.validateRequiredCurrency(result, validationManager, annualHouseholdIncomeString, 'annualHouseholdIncome', 'Annual Household Income');

      const lastDateOfEmploymentContext: MmDdYyyyValidationContext = {
        date: this.lastDateOfEmploymentMmDdYyyy,
        prop: 'lastDateOfEmploymentMmDdYyyy',
        prettyProp: 'Last Date of Employment',
        result: result,
        validationManager: validationManager,
        isRequired: true,
        minDate: participantDOB,
        minDateAllowSame: true,
        minDateName: 'DOB',
        maxDate: moment(Utilities.currentDate).format('MM/DD/YYYY'),
        maxDateAllowSame: true,
        maxDateName: 'Current Date',
        participantDOB: null
      };
      if (this.tjTmjHasNeverEmployed !== true) {
        Utilities.validateMmDdYyyyDate(lastDateOfEmploymentContext);
      }

      if (this.ishasWorked16HoursLessDisplayed && this.ishasWorked16HoursLessRequired) {
        Utilities.validateRequiredYesNo(
          result,
          validationManager,
          this.hasWorked16HoursLess,
          'hasWorked16HoursLess',
          'In the last four weeks, has the applicant worked an average of 16 or less hours per week?'
        );
      }

      Utilities.validateRequiredYesNo(
        result,
        validationManager,
        this.tjTmjIsEligibleForUnemployment,
        'tjTmjIsEligibleForUnemployment',
        'Eligible for Unemployment Insurance Benefits?'
      );

      Utilities.validateRequiredYesNo(result, validationManager, this.tjTmjIsReceivingW2Benefits, 'tjTmjIsReceivingW2Benefits', 'Is applicant receiving W-2 benefits or services?');

      Utilities.validateRequiredYesNo(result, validationManager, this.isUSCitizen, 'isUSCitizen', 'Is applicant a U.S citizen or qualified non-citizen?');

      Utilities.validateRequiredYesNo(
        result,
        validationManager,
        this.tjTmjHasWorked1040Hours,
        'tjTmjHasWorked1040Hours',
        'Has applicant worked a total of 1,040 hours in TMJ/TJ in the past?'
      );

      Utilities.validateRequiredYesNo(result, validationManager, this.tjTmjIsAppCompleteAndSigned, 'tjTmjIsAppCompleteAndSigned', 'Has application been completed and signed?');

      Utilities.validateRequiredYesNo(
        result,
        validationManager,
        this.tjTmjHasEligibilityBeenVerified,
        'tjTmjHasEligibilityBeenVerified',
        'Has eligibility information been verified?'
      );

      Utilities.validateRequiredYesNo(
        result,
        validationManager,
        this.tjTmjIsBenefitFromSubsidizedJob,
        'tjTmjIsBenefitFromSubsidizedJob',
        'Is applicant able to obtain and benefit from a subsidized job?'
      );

      if (this.isCanAbleApplicantBenefitFromSubsidizedJobDetailsRequired && this.isCanAbleApplicantBenefitFromSubsidizedJobDetailsDisplayed) {
        Utilities.validateRequiredText(
          this.tjTmjBenefitFromSubsidizedJobDetails,
          'tjTmjBenefitFromSubsidizedJobDetails',
          'Is applicant able to obtain and benefit from a subsidized job? - Details',
          result,
          validationManager
        );
      }
    }
    return result;
  }

  private isTmjTjEmpty() {
    if (this.programId == null || this.programId === 0) {
      return true;
    } else if (this.countyOfResidenceId == null || this.countyOfResidenceId === 0) {
      return true;
    } else if (this.contractorId == null || this.contractorId === 0) {
      return true;
    } else if (Utilities.isStringEmptyOrNull(this.applicationDateMmDdYyyy)) {
      return true;
    } else if (this.hasChildren == null) {
      return true;
    } else if (this.hasChildren === true && !this.isRepeaterFilledWithData()) {
      return true;
    } else if (+this.householdSize < 0 || this.householdSize == null || this.householdSize.toString() === '') {
      return true;
    } else if (this.annualHouseholdIncome == null || Utilities.isStringEmptyOrNull(this.annualHouseholdIncome.toString())) {
      return true;
    } else if (this.tjTmjHasNeverEmployed === false && Utilities.isStringEmptyOrNull(this.lastDateOfEmploymentMmDdYyyy)) {
      return true;
    } else if (this.ishasWorked16HoursLessDisplayed && this.hasWorked16HoursLess == null) {
      return true;
    } else if (this.tjTmjIsEligibleForUnemployment == null) {
      return true;
    } else if (this.tjTmjIsReceivingW2Benefits == null) {
      return true;
    } else if (this.isUSCitizen == null) {
      return true;
    } else if (this.tjTmjHasWorked1040Hours == null) {
      return true;
    } else if (this.tjTmjIsAppCompleteAndSigned == null) {
      return true;
    } else if (this.tjTmjHasEligibilityBeenVerified == null) {
      return true;
    } else if (this.tjTmjIsBenefitFromSubsidizedJob == null) {
      return true;
    } else if (this.tjTmjIsBenefitFromSubsidizedJob == null) {
      return true;
    } else if (this.tjTmjIsBenefitFromSubsidizedJob === false && Utilities.isStringEmptyOrNull(this.tjTmjBenefitFromSubsidizedJobDetails)) {
      return true;
    } else {
      return false;
    }
  }
}

export class OldRfaProgram {
  programName: string;
  rfaNumber: number;
  statusName: string;
  applicationDate: string;
  countyName;
  string;
  countyNumber: number;
  isOldRfa = true;

  public static clone(input: any, instance: OldRfaProgram) {
    instance.programName = input.programName;
    instance.rfaNumber = input.rfaNumber;
    instance.statusName = input.statusName;
    instance.applicationDate = input.applicationDate;
    instance.countyName = input.countyName;
    instance.countyNumber = input.countyNumber;
  }

  public deserialize(input: any) {
    OldRfaProgram.clone(input, this);
    return this;
  }
}
