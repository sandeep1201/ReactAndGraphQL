import { Utilities } from '../utilities';
import { ValidationManager } from '../../shared/models/validation-manager';
import { ValidationResult } from './validation-result';
import { MmYyyyValidationContext } from '../interfaces/mmYyyy-validation-context';

import * as moment from 'moment';
import { AppService } from 'src/app/core/services/app.service';

export class MilitarySection {
  isSubmittedViaDriverFlow: boolean;
  hasTraining: boolean;
  branchId: number;
  branchName: string;
  rankId: number;
  rankName: string;
  rate: string;
  isCurrentlyEnlisted = false;
  enlistmentDate: string;
  dischargeDate: string;
  dischargeTypeId: number;
  dischargeTypeName: string;
  skillsAndTraining: string;
  isEligibleForBenefitsYesNoUnknown: number;
  isEligibleForBenefitsYesNoUnknownName: string;
  benefitsDetails: string;
  notes: string;
  rowVersion: string;
  modifiedBy: string;
  modifiedDate: string;

  public static clone(input: any, instance: MilitarySection) {
    instance.isSubmittedViaDriverFlow = input.isSubmittedViaDriverFlow;
    instance.hasTraining = input.hasTraining;
    instance.branchId = input.branchId;
    instance.branchName = input.branchName;
    instance.rankId = input.rankId;
    instance.rankName = input.rankName;
    instance.rate = input.rate;
    instance.isCurrentlyEnlisted = input.isCurrentlyEnlisted;
    instance.enlistmentDate = input.enlistmentDate;
    instance.dischargeDate = input.dischargeDate;
    instance.dischargeTypeId = input.dischargeTypeId;
    instance.dischargeTypeName = input.dischargeTypeName;
    instance.skillsAndTraining = input.skillsAndTraining;
    instance.isEligibleForBenefitsYesNoUnknown = input.isEligibleForBenefitsYesNoUnknown;
    instance.isEligibleForBenefitsYesNoUnknownName = input.isEligibleForBenefitsYesNoUnknownName;
    instance.benefitsDetails = input.benefitsDetails;
    instance.notes = input.notes;
    instance.rowVersion = input.rowVersion;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
  }

  public deserialize(input: any) {
    MilitarySection.clone(input, this);
    return this;
  }

  public validate(validationManager: ValidationManager, participantDob: string): ValidationResult {
    const result = new ValidationResult();
    Utilities.validateRequiredYesNo(result, validationManager, this.hasTraining, 'hasTraining', 'Do you have any military training?');
    const currentDate = Utilities.currentDate.day(1).format('MM/DD/YYYY');

    // A bunch of validations are only checked if the Participant has had training.
    if (this.hasTraining === true) {
      Utilities.validateDropDown(this.branchId, 'branchId', 'Branch', result, validationManager);

      if (this.branchId !== 7) {
        Utilities.validateDropDown(this.rankId, 'rankId', 'Rank', result, validationManager);
      }

      let dischargeDate: string;
      if (this.isCurrentlyEnlisted === true) {
        dischargeDate = null;
      } else {
        dischargeDate = this.dischargeDate;
      }
      // Must be after DOB.
      // Cannot be more than 120 years since DOB.
      const enlistmentDateContext: MmYyyyValidationContext = {
        date: this.enlistmentDate,
        prop: 'enlistmentDate',
        prettyProp: 'Enlistment Date',
        result: result,
        validationManager: validationManager,
        isRequired: false,
        minDate: participantDob,
        minDateAllowSame: true,
        minDateName: "Participant's DOB",
        maxDate: dischargeDate,
        maxDateAllowSame: false,
        maxDateName: 'Discharge Date',
        participantDOB: moment(participantDob)
      };
      Utilities.validateMmYyyyDate(enlistmentDateContext);

      const enlistmentDateContextMaxFuture: MmYyyyValidationContext = {
        date: this.enlistmentDate,
        prop: 'enlistmentDate',
        prettyProp: 'Enlistment Date',
        result: result,
        validationManager: validationManager,
        isRequired: false,
        minDate: participantDob,
        minDateAllowSame: true,
        minDateName: "Participant's DOB",
        maxDate: currentDate,
        maxDateAllowSame: true,
        maxDateName: 'Current Date',
        participantDOB: moment(participantDob)
      };
      Utilities.validateMmYyyyDate(enlistmentDateContextMaxFuture);

      if (this.isDischargeDateDisplayed() === true) {
        // Must be after DOB.
        // Cannot be more than 120 years since DOB.
        const dischargeDateContext: MmYyyyValidationContext = {
          date: this.dischargeDate,
          prop: 'dischargeDate',
          prettyProp: 'Discharge Date',
          result: result,
          validationManager: validationManager,
          isRequired: false,
          minDate: this.dischargeDate,
          minDateAllowSame: true,
          minDateName: 'Discharge Date',
          maxDate: null,
          maxDateAllowSame: false,
          maxDateName: null,
          participantDOB: moment(participantDob)
        };
        Utilities.validateMmYyyyDate(dischargeDateContext);

        if (this.isDischargeDateDisplayed() === true) {
          // Must be after DOB.
          // Second Validation
          const dischargeDateContextMinDob: MmYyyyValidationContext = {
            date: this.dischargeDate,
            prop: 'dischargeDate',
            prettyProp: 'Discharge Date',
            result: result,
            validationManager: validationManager,
            isRequired: false,
            minDate: participantDob,
            minDateAllowSame: true,
            minDateName: "Participant's DOB",
            maxDate: currentDate,
            maxDateAllowSame: true,
            maxDateName: 'Current Date',
            participantDOB: moment(participantDob)
          };

          Utilities.validateMmYyyyDate(dischargeDateContextMinDob);

          if (this.isCurrentlyEnlisted !== true) {
            if (this.branchId !== 7) {
              Utilities.validateDropDown(this.dischargeTypeId, 'dischargeTypeId', 'Discharge Type', result, validationManager);
            }
          }
        }
      }

      Utilities.validateDropDown(
        this.isEligibleForBenefitsYesNoUnknown,
        'isEligibleForBenefitsYesNoUnknown',
        'Are you eligible for any benefits from your military service?',
        result,
        validationManager
      );

      if (this.isEligibleForBenefitsYesNoUnknown === 1) {
        Utilities.validateRequiredText(
          this.benefitsDetails,
          'benefitsDetails',
          'Are you eligible for any benefits from your military service? - Details',
          result,
          validationManager
        );
      }
    }
    return result;
  }

  public isDischargeDateDisplayed() {
    if (this.isCurrentlyEnlisted === true) {
      return false;
    } else {
      return true;
    }
  }
}
