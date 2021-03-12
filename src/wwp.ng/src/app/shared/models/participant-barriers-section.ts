import { Serializable } from '../interfaces/serializable';
import { Utilities } from '../utilities';
import { ValidationManager } from './validation-manager';
import { ValidationResult } from './validation-result';
import { YesNoStatus } from './primitives';

export class ParticipantBarriersSection implements Serializable<ParticipantBarriersSection> {
  isSubmittedViaDriverFlow: boolean;
  isPhysicalHealthHardToManage: YesNoStatus;
  isPhysicalHealthHardToParticipate: YesNoStatus;
  isPhysicalHealthTakeMedication: YesNoStatus;

  isMentalHealthDiagnosed: YesNoStatus;
  isMentalHealthHardToManage: YesNoStatus;
  isMentalHealthHardToParticipate: YesNoStatus;
  isMentalHealthTakeMedication: YesNoStatus;

  isAodaHardToManage: YesNoStatus;
  isAodaHardToParticipate: YesNoStatus;
  isAodaTakeTreatment: YesNoStatus;

  isSafeAppropriateToAsk: boolean;
  isDomesticViolenceHurtingYouOrOthers: YesNoStatus;
  isDomesticViolenceEverHarmedPhysicallyOrSexually: YesNoStatus;
  isDomesticViolencePartnerControlledMoney: YesNoStatus;
  isDomesticViolenceReceivedServicesOrLivedInShelter: YesNoStatus;
  isDomesticViolenceEmotionallyOrVerballyAbusing: YesNoStatus;
  isDomesticViolenceCallingHarassingStalkingAtWork: YesNoStatus;
  isDomesticViolenceMakingItDifficultToWork: YesNoStatus;
  isDomesticViolenceOverwhelmedByRapeOrSexualAssault: YesNoStatus;
  isDomesticViolenceInvolvedInCourts: YesNoStatus;
  isLearningDisabilityDiagnosed: YesNoStatus;
  isLearningDisabilityHardToManage: YesNoStatus;
  isLearningDisabilityHardToParticipate: YesNoStatus;
  notes: string;
  assessmentRowVersion: boolean;
  rowVersion: string;
  modifiedBy: string;
  modifiedDate: string;

  public static clone(input: any, instance: ParticipantBarriersSection) {
    instance.isSubmittedViaDriverFlow = input.isSubmittedViaDriverFlow;

    instance.isPhysicalHealthHardToManage = Utilities.deserilizeChild(input.isPhysicalHealthHardToManage, YesNoStatus);
    instance.isPhysicalHealthHardToParticipate = Utilities.deserilizeChild(input.isPhysicalHealthHardToParticipate, YesNoStatus);
    instance.isPhysicalHealthTakeMedication = Utilities.deserilizeChild(input.isPhysicalHealthTakeMedication, YesNoStatus);

    instance.isMentalHealthDiagnosed = Utilities.deserilizeChild(input.isMentalHealthDiagnosed, YesNoStatus);
    instance.isMentalHealthHardToManage = Utilities.deserilizeChild(input.isMentalHealthHardToManage, YesNoStatus);
    instance.isMentalHealthHardToParticipate = Utilities.deserilizeChild(input.isMentalHealthHardToParticipate, YesNoStatus);
    instance.isMentalHealthTakeMedication = Utilities.deserilizeChild(input.isMentalHealthTakeMedication, YesNoStatus);

    instance.isAodaHardToManage = Utilities.deserilizeChild(input.isAodaHardToManage, YesNoStatus);
    instance.isAodaHardToParticipate = Utilities.deserilizeChild(input.isAodaHardToParticipate, YesNoStatus);
    instance.isAodaTakeTreatment = Utilities.deserilizeChild(input.isAodaTakeTreatment, YesNoStatus);

    instance.isSafeAppropriateToAsk = input.isSafeAppropriateToAsk;
    instance.isDomesticViolenceHurtingYouOrOthers = Utilities.deserilizeChild(input.isDomesticViolenceHurtingYouOrOthers, YesNoStatus);
    instance.isDomesticViolenceEverHarmedPhysicallyOrSexually = Utilities.deserilizeChild(input.isDomesticViolenceEverHarmedPhysicallyOrSexually, YesNoStatus);
    instance.isDomesticViolencePartnerControlledMoney = Utilities.deserilizeChild(input.isDomesticViolencePartnerControlledMoney, YesNoStatus);
    instance.isDomesticViolenceReceivedServicesOrLivedInShelter = Utilities.deserilizeChild(input.isDomesticViolenceReceivedServicesOrLivedInShelter, YesNoStatus);
    instance.isDomesticViolenceEmotionallyOrVerballyAbusing = Utilities.deserilizeChild(input.isDomesticViolenceEmotionallyOrVerballyAbusing, YesNoStatus);
    instance.isDomesticViolenceCallingHarassingStalkingAtWork = Utilities.deserilizeChild(input.isDomesticViolenceCallingHarassingStalkingAtWork, YesNoStatus);
    instance.isDomesticViolenceMakingItDifficultToWork = Utilities.deserilizeChild(input.isDomesticViolenceMakingItDifficultToWork, YesNoStatus);
    instance.isDomesticViolenceOverwhelmedByRapeOrSexualAssault = Utilities.deserilizeChild(input.isDomesticViolenceOverwhelmedByRapeOrSexualAssault, YesNoStatus);
    instance.isDomesticViolenceInvolvedInCourts = Utilities.deserilizeChild(input.isDomesticViolenceInvolvedInCourts, YesNoStatus);

    instance.isLearningDisabilityDiagnosed = Utilities.deserilizeChild(input.isLearningDisabilityDiagnosed, YesNoStatus);
    instance.isLearningDisabilityHardToManage = Utilities.deserilizeChild(input.isLearningDisabilityHardToManage, YesNoStatus);
    instance.isLearningDisabilityHardToParticipate = Utilities.deserilizeChild(input.isLearningDisabilityHardToParticipate, YesNoStatus);

    instance.notes = input.notes;
    instance.assessmentRowVersion = input.assessmentRowVersion;
    instance.rowVersion = input.rowVersion;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
  }

  public deserialize(input: any) {
    ParticipantBarriersSection.clone(input, this);

    return this;
  }

  public isDetailsRequired(yesNoRefused: YesNoStatus, yesId: number) {
    // We need to use the == instead of the triple-equals below beacuse the status value from the history mechanism comes across
    // in string form.
    //
    // tslint:disable-next-line:triple-equals
    if (yesNoRefused != null && yesNoRefused.status != null && yesNoRefused.status == yesId) {
      return true;
    } else {
      return false;
    }
  }

  isPhysicalHealthTakeMedicationDisplayed(yesId: number): boolean {
    if (this.isPhysicalHealthHardToManage == null || this.isPhysicalHealthHardToParticipate == null) {
      return false;
    }

    // We need to use the == instead of the triple-equals below beacuse the status value from the history mechanism comes across
    // in string form.
    //
    // tslint:disable-next-line:triple-equals
    if (this.isPhysicalHealthHardToManage.status == yesId || this.isPhysicalHealthHardToParticipate.status == yesId) {
      return true;
    }
    return false;
  }

  isMentalHealthTakeMedicationDisplayed(yesId: number): boolean {
    if (this.isMentalHealthDiagnosed == null || this.isMentalHealthHardToManage == null || this.isMentalHealthHardToParticipate == null) {
      return false;
    }

    // We need to use the == instead of the triple-equals below beacuse the status value from the history mechanism comes across
    // in string form.
    //
    // tslint:disable-next-line:triple-equals
    if (this.isMentalHealthDiagnosed.status == yesId || this.isMentalHealthHardToManage.status == yesId || this.isMentalHealthHardToParticipate.status == yesId) {
      return true;
    }
    return false;
  }

  isAodaTakeTreatmentDisplayed(yesId: number): boolean {
    if (this.isAodaHardToManage == null || this.isAodaHardToParticipate == null) {
      return false;
    }

    // We need to use the == instead of the triple-equals below beacuse the status value from the history mechanism comes across
    // in string form.
    //
    // tslint:disable-next-line:triple-equals
    if (this.isAodaHardToManage.status == yesId || this.isAodaHardToParticipate.status == yesId) {
      return true;
    }
    return false;
  }

  public cleanse(): void {
    console.warn('Cleanse Needs to added');
  }

  public validate(validationManager: ValidationManager, yesId: number, hasPBAccessBol: boolean, canRequestPBAccess: boolean, requestedElevatedAccess: boolean) {
    const result = new ValidationResult();
    if (hasPBAccessBol || (canRequestPBAccess && requestedElevatedAccess)) {
      Utilities.validateYesNoRefused(
        this.isPhysicalHealthHardToManage,
        yesId,
        result,
        validationManager,
        'isPhysicalHealthHardToManage',
        'Do you have any health problems that make it hard to manage your daily life?'
      );

      Utilities.validateYesNoRefused(
        this.isPhysicalHealthHardToParticipate,
        yesId,
        result,
        validationManager,
        'isPhysicalHealthHardToParticipate',
        'Do you have concerns that problems with your health will make it hard to participate in work activities?'
      );

      if (this.isPhysicalHealthTakeMedicationDisplayed(yesId)) {
        Utilities.validateYesNoRefused(
          this.isPhysicalHealthTakeMedication,
          yesId,
          result,
          validationManager,
          'isPhysicalHealthTakeMedication',
          'Do you currently see a health care provider or take medications for health problem(s)?'
        );
      }

      Utilities.validateYesNoRefused(
        this.isMentalHealthDiagnosed,
        yesId,
        result,
        validationManager,
        'isMentalHealthDiagnosed',
        'Have you ever met with a counselor or psychiatrist for mental health services or been diagnosed with a mental health condition?'
      );

      Utilities.validateYesNoRefused(
        this.isMentalHealthHardToManage,
        yesId,
        result,
        validationManager,
        'isMentalHealthHardToManage',
        'Do you have any mental health conditions that make it hard for you to manage your daily life?'
      );

      Utilities.validateYesNoRefused(
        this.isMentalHealthHardToParticipate,
        yesId,
        result,
        validationManager,
        'isMentalHealthHardToParticipate',
        'Do you have concerns that a mental health condition will make it hard for you to participate in work activities?'
      );

      if (this.isMentalHealthTakeMedicationDisplayed(yesId)) {
        Utilities.validateYesNoRefused(
          this.isMentalHealthTakeMedication,
          yesId,
          result,
          validationManager,
          'isMentalHealthTakeMedication',
          'Do you currently see a counselor or psychiatrist for mental health services or take medication for a mental health condition?'
        );
      }

      Utilities.validateYesNoRefused(
        this.isAodaHardToManage,
        yesId,
        result,
        validationManager,
        'isAodaHardToManage',
        'Does alcohol or drug use make it hard for you to manage your daily life?'
      );

      Utilities.validateYesNoRefused(
        this.isAodaHardToParticipate,
        yesId,
        result,
        validationManager,
        'isAodaHardToParticipate',
        'Do you have concerns that alcohol or drug use will make it hard for you to participate in work activities?'
      );

      if (this.isAodaTakeTreatmentDisplayed(yesId)) {
        Utilities.validateYesNoRefused(
          this.isAodaTakeTreatment,
          yesId,
          result,
          validationManager,
          'isAodaTakeTreatment',
          'Are you currently in any alcohol or drug treatment services?'
        );
      }

      Utilities.validateYesNoRefused(
        this.isLearningDisabilityDiagnosed,
        yesId,
        result,
        validationManager,
        'isLearningDisabilityDiagnosed',
        'Did you ever have problems learning in school or have you ever been diagnosed with a learning disability?'
      );

      Utilities.validateYesNoRefused(
        this.isLearningDisabilityHardToManage,
        yesId,
        result,
        validationManager,
        'isLearningDisabilityHardToManage',
        'Do you have learning problems that make it hard to manage your daily life?'
      );

      Utilities.validateYesNoRefused(
        this.isLearningDisabilityHardToParticipate,
        yesId,
        result,
        validationManager,
        'isLearningDisabilityHardToParticipate',
        'Do you have concerns that learning problems will make it hard to participate in work activities?'
      );
    }
    return result;
  }
}
