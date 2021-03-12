import { Utilities } from '../utilities';
import { ValidationManager } from '../../shared/models/validation-manager';
import { ValidationResult } from './validation-result';
import { MmDdYyyyValidationContext } from '../interfaces/mmDdYyyy-validation-context';
import * as moment from 'moment';

export class ExamResult {
  id: number;
  subjectId: number;
  name: string;
  nrsId: number;
  nrsRating: string;
  splId: number;
  splRating: string;
  score: number;
  totalScore: number;
  hasPassed: number;
  datePassed: string;
  level: string;
  form: string;
  gradeEquivalency: string;
  casasGradeEquivalency: string;
  version: string;
  rowVersion: string;

  public static clone(input: any, instance: ExamResult) {
    instance.id = input.id;
    instance.name = input.name;
    instance.subjectId = input.subjectId;
    instance.score = input.score;
    instance.nrsId = input.nrsId;
    instance.nrsRating = input.nrsRating;
    instance.splId = input.splId;
    instance.splRating = input.splRating;
    instance.totalScore = input.totalScore;
    instance.hasPassed = input.hasPassed;
    instance.datePassed = input.datePassed;
    instance.level = input.level;
    instance.form = input.form;
    instance.gradeEquivalency = Utilities.convertDecimalToRoundedString(input.gradeEquivalency, 1);
    instance.casasGradeEquivalency = input.casasGradeEquivalency;
    instance.version = input.version;
    instance.rowVersion = input.rowVersion;
  }

  public isEmpty(): boolean {
    // All properties have to be null or blank to make the entire object empty.
    return (
      (this.name == null || this.name.trim() === '') &&
      (this.score == null || this.score.toString() === '') &&
      (this.totalScore == null || this.totalScore.toString() === '') &&
      (this.nrsId == null || this.nrsId.toString() === '') &&
      (this.splId == null || this.splId.toString() === '') &&
      this.hasPassed == null &&
      (this.datePassed == null || this.datePassed.toString() === '') &&
      (this.level == null || this.level.trim() === '') &&
      (this.gradeEquivalency == null || this.gradeEquivalency.trim() === '') &&
      Utilities.stringIsNullOrWhiteSpace(this.casasGradeEquivalency) &&
      Utilities.stringIsNullOrWhiteSpace(this.form) &&
      (this.version == null || this.version.trim() === '')
    );
  }
  deserialize(input: any) {
    ExamResult.clone(input, this);
    return this;
  }
}

export class TestScore {
  id: number;
  name: string;
  typeId: number;
  details: string;
  dateTaken: string;
  examResults: ExamResult[];
  rowVersion: string;
  modifiedBy: string;
  modifiedDate: string;
  modifiedByName: string;

  public static clone(input: any, instance: TestScore) {
    instance.id = input.id;
    instance.name = input.name;
    instance.typeId = input.typeId;
    instance.details = input.details;
    instance.dateTaken = input.dateTaken;

    instance.examResults = Array.from(Utilities.deserilizeChildren(input.examResults, ExamResult));

    instance.rowVersion = input.rowVersion;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.modifiedByName = input.modifiedByName;
  }

  deserialize(input: any) {
    TestScore.clone(input, this);
    return this;
  }

  public isEmpty(): boolean {
    // All properties have to be null or blank to make the entire object empty.
    let isEmpty = true;
    isEmpty = (this.details == null || this.details.trim() === '') && (this.dateTaken == null || this.dateTaken.trim() === '');

    // Only check children if parent is empty.
    if (this.examResults != null && isEmpty === true) {
      for (const x of this.examResults) {
        if (x.isEmpty() === false) {
          isEmpty = false;
        }
      }
    }

    return isEmpty;
  }

  public validate(
    validationManager: ValidationManager,
    participantDob: string,
    existingTestScores: TestScore[],
    isGradeEquivalencyColDisplayed: boolean,
    isCasas: boolean
  ): ValidationResult {
    const result = new ValidationResult();
    const partDob = moment(participantDob).format('MM/DD/YYYY');
    Utilities.validateDropDown(this.typeId, 'typeId', 'Exam Type', result, validationManager);
    const currentDate = Utilities.currentDate.format('MM/DD/YYYY');
    const dateTakenContext: MmDdYyyyValidationContext = {
      date: this.dateTaken,
      prop: 'dateTaken',
      prettyProp: 'Date Taken',
      result: result,
      validationManager: validationManager,
      isRequired: true,
      minDate: partDob,
      minDateAllowSame: false,
      minDateName: null,
      maxDate: currentDate,
      maxDateAllowSame: true,
      maxDateName: 'Current Date',
      participantDOB: partDob
    };

    Utilities.validateMmDdYyyyDate(dateTakenContext);

    const gradeEquivalencyRegEx = new RegExp('^[0-9]{1,2}(.[0-9])$');
    const casasGradeEquivalencyRegEx = new RegExp('[A-Za-z0-9]{1,2}$');
    const formRegEx = new RegExp('^[A-Za-z0-9]{1,4}$');
    const formLevelRegEx = new RegExp('[A-Ea-e](s*|/[A-Ea-e])$');

    if (this.examResults != null) {
      const examResultsErrors = result.createErrorsArray('examResults');
      for (const er of this.examResults) {
        const me = result.createErrorsArrayItem(examResultsErrors);

        if (!isCasas && +er.score > +er.totalScore) {
          result.addErrorForParent(me, 'totalScore');
          result.addErrorForParent(me, 'score');
        }
        // For Tabe 9 10 we need to limit grade equivalency to 12.9.\
        if (isGradeEquivalencyColDisplayed && !Utilities.stringIsNullOrWhiteSpace(er.gradeEquivalency)) {
          if (!gradeEquivalencyRegEx.test(er.gradeEquivalency) || +er.gradeEquivalency > 12.9) result.addErrorForParent(me, 'gradeEquivalency');
        }

        if (isCasas && !Utilities.stringIsNullOrWhiteSpace(er.casasGradeEquivalency) && !casasGradeEquivalencyRegEx.test(er.casasGradeEquivalency))
          result.addErrorForParent(me, 'casasGradeEquivalency');
        if (isCasas && !Utilities.stringIsNullOrWhiteSpace(er.form) && !formRegEx.test(er.form)) result.addErrorForParent(me, 'form');
        if (isCasas && !Utilities.stringIsNullOrWhiteSpace(er.level) && !formLevelRegEx.test(er.level)) result.addErrorForParent(me, 'level');
      }
    }

    if (existingTestScores != null) {
      for (const ts of existingTestScores) {
        if (ts.typeId === Number(this.typeId) && ts.dateTaken === this.dateTaken && ts.id !== this.id) {
          result.addError('typeId');
          result.addError('dateTaken');
        }
      }
    }

    return result;
  }
}
