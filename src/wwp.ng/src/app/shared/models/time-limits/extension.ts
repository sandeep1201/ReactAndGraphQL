import { Utilities } from './../../utilities';
import { ExtensionContract, ExtensionSequenceContract, ExtensionReasonContract } from '../../services/contracts/timelimits/service.contract';
import { BaseModelContract, CommonDelCreatedModel } from '../model-base-contract';
import { coerceNumber, coerceNumberProperty } from '../../decorators/number-property';
import { coerceBoolean, coerceBooleanProperty } from '../../decorators/boolean-property';
import { AppService } from 'src/app/core/services/app.service';
import { Timeline } from './';
import * as moment from 'moment';
import { ClockType, ClockTypes } from './clocktypes';
import { DateRange, range } from '../../moment-range';
import { ValidationCode, ValidationError, ValidationManager, ValidationResult, Validator } from '../../models/validation';
import * as validation from 'lakmus';
import * as arrayUtils from '../../arrays';

export enum ExtensionDecision {
  Deny = 2,
  Approve = 1
}

export class ExtensionReason extends CommonDelCreatedModel {
  name: string;
  validFor: ClockTypes = ClockTypes.ExtensableTypes;

  public static deserialize(contract: ExtensionReasonContract) {
    let extReason = new ExtensionReason();
    ExtensionReason.setCommonProperties(extReason, contract);
    extReason.name = contract.name;
    if (contract.validClockTypes != null) {
      extReason.validFor = contract.validClockTypes;
    }
    return extReason;
  }
}

export class ExtensionSequence {
  public sequenceId: number;
  public get extensions(): Extension[] {
    return this._extensions;
  }

  public get isDeleted() {
    return !!(this.currentExtension && this.currentExtension.isDeleted);
    //_extensions.findIndex(x => x.isDeleted) !== -1;
  }
  private _extensions: Extension[] = [];

  constructor(public clockType: ClockTypes = ClockTypes.None) {}

  public get currentExtension(): Extension {
    let filteredExtensions = this._extensions.filter(x => !x.isDeleted);
    let extensions: Extension[] = filteredExtensions.length ? filteredExtensions : this._extensions;
    if (!extensions.length) {
      return null; // Note: reduce throws a TypeError is the array is empty( with no inital value)
    }

    // Note: reduce will return the first value without executing the callback if only 1 item is present( with no inital value)
    return extensions.reduce((a, b) => {
      return a.decisionDate.isAfter(b.decisionDate) ? a : b;
    });
  }

  public static deserialize(json: ExtensionSequenceContract): ExtensionSequence {
    let clockTypes = <ClockTypes>json.timelimitTypeId;
    let extensionSequence = new ExtensionSequence(clockTypes);
    extensionSequence.sequenceId = json.sequenceId;

    if (json.extensions) {
      for (let extData of json.extensions) {
        let ext = Extension.deserialize(extData);
        ext.sequenceId = json.sequenceId;
        extensionSequence.extensions.push(ext);
      }
    }
    return extensionSequence;
  }
}

export class Extension extends CommonDelCreatedModel {
  public sequenceId: number = 0;
  public cachedOriginalModel: Extension;
  private _clockType: ClockTypes;
  get clockType(): ClockType {
    return new ClockType(this._clockType & ClockTypes.ExtensableTypes); //make sure we only have an extensible type(s);
  }

  set clockType(val: ClockType) {
    let cType = new ClockType(+val);
    // convert TNP/TMP to TEMP ext
    if (cType.any(ClockTypes.TNP | ClockTypes.TMP)) {
      cType = new ClockType(ClockTypes.TEMP);
    }

    this._clockType = cType.valueOf();
  }

  private _dateRange: DateRange;
  get dateRange(): DateRange {
    if (this.decision === ExtensionDecision.Approve) {
      return this._dateRange;
    }
    return null;
  }

  set dateRange(val: DateRange) {
    this._dateRange = val;
  }

  decision: ExtensionDecision;
  discussionDate: moment.Moment;
  decisionDate: moment.Moment;
  approvalReasonId: number;
  denialReasonId: number;
  reasonDetails: string;
  dvrReferralPending: boolean;
  recieivingDvrServices: boolean;
  pendingSSAppOrAppeal: boolean;
  notes: string = '';
  isBackdated: boolean = false;

  get isActive(): boolean {
    // return !this.isDeleted && this.hasStarted && !this.hasElapsed;
    return !this.isDeleted && this.dateRange && this.dateRange.contains(Utilities.currentDate);
  }

  get hasStarted(): boolean {
    return this.dateRange && this.dateRange.start.isBefore(Utilities.currentDate, 'month');
  }

  get hasElapsed(): boolean {
    return this.dateRange && this.dateRange.end.isBefore(Utilities.currentDate, 'month');
  }

  isDeleted: boolean = false;

  constructor(startDate?: moment.MomentInput, endDate?: moment.MomentInput, decision?: ExtensionDecision) {
    super();
    if (startDate != null && endDate != null) {
      this.dateRange = range(moment(startDate).startOf('month'), moment(endDate).endOf('month'));
    }
    this.decision = decision;
    this.createdDate = moment(Utilities.currentDate);
  }

  validate(validationManager: ValidationManager): ValidationResult {
    const result = new ValidationResult();
    const validator = new ExtensionValidator(this);
    const validatorResult = validator.validate();
    if (!validatorResult.isValid) {
      for (const error of validatorResult.errors) {
        result.addError(error.errorMessage);
        validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing, error.errorMessage);
      }
    }
    return result;
  }

  clone(isRecursive = false) {
    let modelCopy: Extension = new Extension();
    modelCopy.dateRange = this.dateRange ? this.dateRange.clone() : null;
    modelCopy.decision = this.decision;
    modelCopy.clockType = this.clockType;
    modelCopy.discussionDate = this.discussionDate ? this.discussionDate.clone() : null;
    modelCopy.createdDate = this.createdDate ? this.createdDate.clone() : null;
    modelCopy.decisionDate = this.decisionDate ? this.decisionDate.clone() : null;
    modelCopy.id = this.id;
    modelCopy.sequenceId = this.sequenceId;
    modelCopy.isDeleted = this.isDeleted;
    modelCopy.pendingSSAppOrAppeal = this.pendingSSAppOrAppeal;
    // modelCopy.reason = this.reason ? {id:this.id, description: this.reason.description, validFor: this.reason.validFor } : null;
    modelCopy.approvalReasonId = this.approvalReasonId;
    modelCopy.denialReasonId = this.denialReasonId;
    modelCopy.reasonDetails = this.reasonDetails;
    modelCopy.dvrReferralPending = this.dvrReferralPending;
    modelCopy.recieivingDvrServices = this.recieivingDvrServices;
    modelCopy.pendingSSAppOrAppeal = this.pendingSSAppOrAppeal;
    modelCopy.isBackdated = this.isBackdated;
    modelCopy.cachedOriginalModel = isRecursive || this.cachedOriginalModel ? this.cachedOriginalModel : modelCopy.clone(true);
    return modelCopy;
  }

  toString() {
    let clockType = this.clockType.valueOf() || ClockTypes.None;
    return ClockType.toString(ClockType.filterCombos(clockType));
  }

  public static deserialize(json: ExtensionContract) {
    let extension = new Extension();
    // set BaseModel Props
    // extension.id = json.id;
    // extension.createdDate = moment(json.createdDate);
    // extension.modifiedBy = json.modifiedBy;
    // extension.modifiedDate = moment(json.modifiedDate);
    // extension.isDeleted = json.isDeleted;
    // extension.rowVersion = json.rowVersion;
    CommonDelCreatedModel.setCommonProperties(extension, json);

    let clockTypes = <ClockTypes>json.timelimitTypeId;
    extension.clockType = new ClockType(clockTypes);
    extension.decision = json.extensionDecisionId;
    extension.approvalReasonId = json.approvalReasonId;
    extension.denialReasonId = json.denialReasonId;
    extension.reasonDetails = json.reasonDetails;
    extension.dvrReferralPending = json.dvrReferralPending;
    extension.recieivingDvrServices = json.recieivingDvrServices;
    extension.pendingSSAppOrAppeal = json.pendingSSAppOrAppeal;
    extension.isBackdated = json.isBackdated;

    let startDate = CommonDelCreatedModel.getDateFromString(json.startDate);
    let endDate = CommonDelCreatedModel.getDateFromString(json.endDate);

    let dateRange = startDate && startDate.isValid() && endDate && endDate.isValid() ? range(startDate.startOf('month'), endDate.endOf('month')) : null;
    extension.dateRange = dateRange && dateRange.start.isValid() && dateRange.end.isValid() ? dateRange : null;
    extension.discussionDate = CommonDelCreatedModel.getDateFromString(json.initialDiscussionDate);
    extension.decisionDate = CommonDelCreatedModel.getDateFromString(json.decisionDate);

    extension.cachedOriginalModel = extension.clone();

    return extension;
  }

  public serialize() {
    const extContract = new ExtensionContract();
    BaseModelContract.setCommonProperties(extContract, this);
    extContract.approvalReasonId = this.approvalReasonId;
    extContract.decisionDate = this.decisionDate ? this.decisionDate.format() : null;
    extContract.denialReasonId = this.denialReasonId;
    extContract.dvrReferralPending = this.dvrReferralPending;
    extContract.endDate = this.dateRange && this.dateRange.end ? this.dateRange.end.format() : null;
    extContract.startDate = this.dateRange && this.dateRange.start ? this.dateRange.start.format() : null;
    extContract.extensionDecisionId = this.decision;
    extContract.initialDiscussionDate = this.discussionDate ? this.discussionDate.format() : null;
    extContract.notes = this.notes;
    extContract.pendingSSAppOrAppeal = this.pendingSSAppOrAppeal;
    extContract.reasonDetails = this.reasonDetails;
    extContract.recieivingDvrServices = this.recieivingDvrServices;
    extContract.sequenceId = coerceNumberProperty(this.sequenceId);
    extContract.timelimitTypeId = coerceNumberProperty(this.clockType.valueOf());
    extContract.isBackdated = coerceBooleanProperty(this.isBackdated);
    return extContract;
  }
}

export class ExtensionValidator extends Validator<Extension> {
  constructor(instance: Extension) {
    super(instance);
  }

  public validate() {
    this.validateClockType();
    this.validateDenailReasonId();
    this.validateApprovalReasonId();
    this.validateExtensionDateRange();
    return this.validationResult;
  }
  private validateClockType() {
    let context = new validation.ValidationContext();
    context.instance = this.instance;
    context.propertyDisplayName = 'Timelimit Type';
    context.propertyName = 'clockType';
    context.propertyValue = this.instance.clockType.valueOf();
    let notEmptyValidator = new validation.NotEmptyValidator('Invalid Time limit type.');

    let shouldCheckNotEmptyValidator = this.instance.clockType.any(ClockTypes.ExtensableTypes) || this.instance.denialReasonId || this.instance.approvalReasonId;
    if (shouldCheckNotEmptyValidator && !notEmptyValidator.isValid(context)) {
      this.validationResult.errors.push(new validation.ValidationError(notEmptyValidator.getErrorMessage(context)));
      return;
    }
  }

  private validateDenailReasonId() {
    let context = new validation.ValidationContext();
    context.instance = this.instance;
    context.propertyDisplayName = 'Denail Reason';
    context.propertyName = 'denialReasonId';
    context.propertyValue = this.instance.denialReasonId;
    let notEmptyValidator = new validation.NotEmptyValidator('Denial reason is required.');

    let shouldCheckNotEmptyValidator = this.instance.decision === ExtensionDecision.Deny;
    if (shouldCheckNotEmptyValidator && !notEmptyValidator.isValid(context)) {
      this.validationResult.errors.push(new validation.ValidationError(notEmptyValidator.getErrorMessage(context)));
      return;
    }
  }

  private validateApprovalReasonId() {
    let context = new validation.ValidationContext();
    context.instance = this.instance;
    context.propertyDisplayName = 'Approval ReasonId';
    context.propertyName = 'approvalReasonId';
    context.propertyValue = this.instance.approvalReasonId;
    let notEmptyValidator = new validation.NotEmptyValidator('Approval reason is required.');

    let shouldCheckNotEmptyValidator = this.instance.decision === ExtensionDecision.Approve;
    if (shouldCheckNotEmptyValidator && !notEmptyValidator.isValid(context)) {
      this.validationResult.errors.push(new validation.ValidationError(notEmptyValidator.getErrorMessage(context)));
      return;
    }
  }

  private validateExtensionDateRange() {
    let context = new validation.ValidationContext();
    context.instance = this.instance;
    context.propertyDisplayName = 'Extension Date Range';
    context.propertyName = 'dateRange';
    context.propertyValue = this.instance.dateRange;
    let notEmptyValidator = new validation.NotEmptyValidator('Extension length is required.');

    let shouldCheckNotEmptyValidator = this.instance.decision === ExtensionDecision.Approve;
    if (shouldCheckNotEmptyValidator && !notEmptyValidator.isValid(context)) {
      this.validationResult.errors.push(new validation.ValidationError(notEmptyValidator.getErrorMessage(context)));
    }
  }
}
// export class ExtensionValidator2 extends validation.Validator<Extension> {

//     constructor() {
//         super();

//         this.ruleFor(x => x.clockType)
//             .notEmpty()
//             .must(x => { return x.any(ClockTypes.ExtensableTypes) })
//             .withMessage('Invalid Time limit type');

//         this.ruleFor(x => x.denialReasonId)
//             .notEmpty()
//             .when(x => x.decision === ExtensionDecision.Deny);

//         this.ruleFor(x => x.approvalReasonId)
//             .notEmpty()
//             .when(x => x.decision === ExtensionDecision.Approve);

//         // this.ruleFor(x => x.reasonDetails)
//         //     .notEmpty()
//         //     .withMessage('Details are required.');
//     }
// }
