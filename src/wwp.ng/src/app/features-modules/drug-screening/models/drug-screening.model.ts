import { Serializable } from 'src/app/shared/interfaces/serializable';
import { ValidationManager, ValidationResult } from 'src/app/shared/models/validation';
import { Utilities } from 'src/app/shared/utilities';
import { DrugScreeningStatus } from './drug-screening-status.model';

export class DrugScreening implements Serializable<DrugScreening> {
  id: number;
  usedNonRequiredDrugs: boolean;
  abusedMoreDrugs: boolean;
  cannotStopAbusingDrugs: boolean;
  hadBlackoutsFromDrugs: boolean;
  feelGuiltyAboutDrugs: boolean;
  spouseComplaintOnDrugs: boolean;
  neglectedFamilyForDrugs: boolean;
  illegalActivitiesForDrugs: boolean;
  sickFromStoppingDrugs: boolean;
  medicalProblemsFromDrugs: boolean;
  drugScreeningStatusTypeId: number;

  details: string;
  drugScreeningStatuses: DrugScreeningStatus[];
  isDeleted: boolean;

  modifiedBy: string;
  modifiedDate: string;
  public static clone(input: any, instance: DrugScreening) {
    instance.id = input.id;
    instance.abusedMoreDrugs = input.abusedMoreDrugs;
    instance.cannotStopAbusingDrugs = input.cannotStopAbusingDrugs;
    instance.feelGuiltyAboutDrugs = input.feelGuiltyAboutDrugs;
    instance.hadBlackoutsFromDrugs = input.hadBlackoutsFromDrugs;
    instance.illegalActivitiesForDrugs = input.illegalActivitiesForDrugs;
    instance.isDeleted = input.isDeleted;
    instance.medicalProblemsFromDrugs = input.medicalProblemsFromDrugs;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.neglectedFamilyForDrugs = input.neglectedFamilyForDrugs;
    instance.sickFromStoppingDrugs = input.sickFromStoppingDrugs;
    instance.spouseComplaintOnDrugs = input.spouseComplaintOnDrugs;
    instance.usedNonRequiredDrugs = input.usedNonRequiredDrugs;
    instance.drugScreeningStatusTypeId = input.drugScreeningStatusTypeId;
    instance.drugScreeningStatuses = Utilities.deserilizeChildren(input.drugScreeningStatuses, DrugScreeningStatus);
    instance.details = input.details;
  }
  public deserialize(input: any) {
    DrugScreening.clone(input, this);
    return this;
  }
  public validate(validationManager: ValidationManager) {
    const result = new ValidationResult();

    Utilities.validatePropForNullAndEmptyValues(result, validationManager, this.abusedMoreDrugs, 'abusedMoreDrugs', 'Abused Drugs');
    Utilities.validatePropForNullAndEmptyValues(result, validationManager, this.cannotStopAbusingDrugs, 'cannotStopAbusingDrugs', 'Cannot Stop Abusing Drugs');
    Utilities.validatePropForNullAndEmptyValues(result, validationManager, this.feelGuiltyAboutDrugs, 'feelGuiltyAboutDrugs', 'Feel Fuilty About Drugs');
    Utilities.validatePropForNullAndEmptyValues(result, validationManager, this.hadBlackoutsFromDrugs, 'hadBlackoutsFromDrugs', 'Had Blackouts From Drugs');
    Utilities.validatePropForNullAndEmptyValues(result, validationManager, this.illegalActivitiesForDrugs, 'illegalActivitiesForDrugs', 'Illegal Activities for Drugs');
    Utilities.validatePropForNullAndEmptyValues(result, validationManager, this.medicalProblemsFromDrugs, 'medicalProblemsFromDrugs', 'Medical Problems From Drugs');
    Utilities.validatePropForNullAndEmptyValues(result, validationManager, this.neglectedFamilyForDrugs, 'neglectedFamilyForDrugs', 'Neglected Family For Drugs');
    Utilities.validatePropForNullAndEmptyValues(result, validationManager, this.sickFromStoppingDrugs, 'sickFromStoppingDrugs', 'Sick From Stopping Drugs');
    Utilities.validatePropForNullAndEmptyValues(result, validationManager, this.spouseComplaintOnDrugs, 'spouseComplaintOnDrugs', 'Spouse Complaint On Drugs');
    Utilities.validatePropForNullAndEmptyValues(result, validationManager, this.usedNonRequiredDrugs, 'usedNonRequiredDrugs', 'Used Non Required Drugs');
    Utilities.validatePropForNullAndEmptyValues(result, validationManager, this.drugScreeningStatusTypeId, 'drugScreeningStatusTypeId', 'Status');
    return result;
  }
}
