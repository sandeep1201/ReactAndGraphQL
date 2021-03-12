import { Serializable } from '../../../shared/interfaces/serializable';
import * as moment from 'moment';
import { ValidationResult, ValidationManager, ValidationCode } from '../../../shared/models/validation';
import { Utilities } from '../../../shared/utilities';

export class CareerAssessment implements Serializable<CareerAssessment> {
  id: number;

  participantId: number;

  elementId: number;
  completionDate: string;
  assessmentProvider: string;
  assessmentToolUsed: string;
  assessmentResults: string;
  careerAssessmentContactId: number;
  relatedOccupation: string;
  assessmentResultAppliedToEP: string;
  isDeleted: boolean;
  createdDate: string;
  elementIds: number[];
  elementNames: string[];

  rowVersion: string;
  modifiedBy: string;
  modifiedDate: string;

  public static clone(input: any, instance: CareerAssessment) {
    instance.id = input.id;
    instance.participantId = input.participantId;
    instance.completionDate = input.completionDate;
    instance.assessmentProvider = input.assessmentProvider;
    instance.assessmentToolUsed = input.assessmentToolUsed;
    instance.assessmentResults = input.assessmentResults;
    instance.careerAssessmentContactId = input.careerAssessmentContactId;
    instance.relatedOccupation = input.relatedOccupation;
    instance.assessmentResultAppliedToEP = input.assessmentResultAppliedToEP;
    instance.elementIds = Utilities.deserilizeArray(input.elementIds);
    instance.elementNames = input.elementName || [];
    instance.isDeleted = input.isDeleted;
    instance.rowVersion = input.rowVersion;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
  }

  public deserialize(input: any) {
    CareerAssessment.clone(input, this);
    return this;
  }
  // tslint:disable-next-line: member-ordering

  public validate(validationManager: ValidationManager, participantDOB: string): ValidationResult {
    const result = new ValidationResult();
    if (!this.elementIds || (this.elementIds && this.elementIds.length === 0)) {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Element(s)');
      result.addError('elementIds');
    }
    if (this.completionDate == null || this.completionDate === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Completion Date');
      result.addError('completionDate');
    } else if (this.completionDate !== null || this.completionDate !== '') {
      const inputDate = moment(this.completionDate, 'MM/DD/YYYY', true);
      const DOB = moment(participantDOB).format('MM/DD/YYYY');
      Utilities.dateFormatValidation(validationManager, result, this.completionDate, 'Completion Date', 'completionDate');
      if (inputDate.isValid()) {
        if (inputDate.isBefore(moment(DOB))) {
          validationManager.addErrorWithFormat(ValidationCode.ValueBeforeDOB_Name_Value_DOB, 'Completion Date', this.completionDate, DOB);
          result.addError('completionDate');
        }
        if (moment(inputDate, 'MM/DD/YYYY', true).isAfter(Utilities.currentDate.format('MM/DD/YYYY'))) {
          validationManager.addErrorWithFormat(ValidationCode.DateAfterCurrent, 'Completion Date', this.completionDate, Utilities.currentDate.format('MM/DD/YYYY'));
          result.addError('CompletionDate');
        }
      }
    }
    if (this.assessmentProvider == null || this.assessmentProvider === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Assessment Provider');
      result.addError('assessmentProvider');
    }
    if (this.assessmentToolUsed == null || this.assessmentToolUsed === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Assessment Tool Used');
      result.addError('assessmentToolUsed');
    }
    if (this.assessmentResults == null || this.assessmentResults === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Assessment Results');
      result.addError('assessmentResults');
    }
    if (this.relatedOccupation == null || this.relatedOccupation === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Related Occupation(s)');
      result.addError('relatedOccupation');
    }
    if (this.assessmentResultAppliedToEP == null || this.assessmentResultAppliedToEP === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Assessment Results Applied to EP');
      result.addError('assessmentResultAppliedToEP');
    }
    return result;
  }
}
