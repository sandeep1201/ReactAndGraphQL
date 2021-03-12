import { Utilities } from './../../utilities';
import { BaseModelContract, CommonDelCreatedModel } from '../model-base-contract';
import { deepClone } from '../../lib/deep-clone';
import { ReasonForChangeContract, TimelineMonthContract } from '../../services/contracts/timelimits/service.contract';
import { merge } from '../../lib/merge';
import { AppService } from 'src/app/core/services/app.service';
import { Timeline } from './';
import { Dictionary } from '../../dictionary';
import { EnumFlagsType, EnumFlagsTest, EnumFlagsTool } from 'ts-enum-tools';
import * as moment from 'moment';
import { range, DateRange, rangeInterval, within } from '../../moment-range';
import { ValidationError, ValidationManager, ValidationResult, Validator } from '../../models/validation';
import * as validation from 'lakmus';

import { Participant } from '../participant';
import { ClockType, ClockTypes } from './clocktypes';
import { Extension } from './extension';
import { Tick } from './tick';
import { coerce } from '../../decorators/';
import { coerceNumber } from '../../decorators/number-property';
import { LogService } from '../../services/log.service';

export class ReasonForChange extends CommonDelCreatedModel {
  public static deserialize(contract: ReasonForChangeContract) {
    let reason = new ReasonForChange();
    CommonDelCreatedModel.setCommonProperties(reason, contract);
    reason.name = contract.name;
    reason.isRequired = contract.isRequired;
    reason.code = contract.code;
    return reason;
  }

  public name: string;
  public isRequired: boolean;
  public code: string;
}

export class TimelineMonth extends CommonDelCreatedModel {
  isEdited: boolean;
  get isPaid(): boolean {
    return false; // this.paymentIssued > 0;
  }

  // activeExtensions() {
  //     return this.extensions.filter((ex, i, arr) => {
  //         return ex.isActive;
  //     });
  // }

  hasExtension(clockType?: ClockTypes): boolean {
    if (clockType == null) {
      return this.extensions.length > 0;
    } else {
      return this.extensions.find(ex => ex.clockType.any(clockType)) != null;
    }
  }

  extensions: Extension[] = [];
  // {
  //     //TODO: Maybe we should watch the timeline.extensions and cache these? Subscribe (replaySubject?) with RxJS?
  //     if (!this.timeline) {
  //         return [];
  //     }
  //     // return true if the extension overlaps the month in case the date the extension starts if funky
  //     return this.timeline.extensions.filter(ex => {
  //         const overlaps = ex.dateRange.overlaps(rangeInterval(this.monthRange.start, 'month'));
  //         return overlaps;
  //     });
  // }

  readonly isJanuary: boolean;

  private _tick: Tick;
  get tick(): Tick {
    if (!this._tick) {
      this._tick = new Tick();
    }
    return this._tick;
  }
  set tick(val: Tick) {
    this._tick = val;
  }

  paymentIssued: number = null;
  get isCurrentMonth() {
    return Utilities.currentDate.isSame(this.date, 'month');
  }
  get isFuture() {
    if (this.date.isSameOrAfter(Utilities.currentDate, 'month') === true) {
      let x = 10;
    }
    return this.date.isSameOrAfter(Utilities.currentDate, 'month');
  }

  cachedOriginalModel: TimelineMonth;
  date: moment.Moment;

  @coerceNumber() reasonForChange: number;
  reasonForChangeDetails;
  modifiedBy: string;
  modifiedDate: moment.Moment;
  createdDate: moment.Moment;

  readonly monthRange: DateRange;

  constructor(date: moment.MomentInput) {
    super();
    if (date != null) {
      this.monthRange = range(moment(date).startOf('month'), moment(date).endOf('month'));
      this.date = this.monthRange.start.startOf('month');
      this.isJanuary = this.date.month() === 0;

      // Note: the current month may not be 'future' if it has a tick because of the batch run date,
      // but we are ignoring this for now
    }
  }

  tostring() {
    return this.monthRange.start.month() + '-' + this.monthRange.start.year();
  }

  changeHistory: TimelineMonth[] = [];

  public validate(validationManager: ValidationManager): ValidationResult {
    const result = new ValidationResult();
    const validator = new TimelineMonthValidator(this);
    const validatorResult = validator.validate();
    if (!validatorResult.isValid) {
      for (const error of validatorResult.errors) {
        result.addError(error.errorMessage);
      }
    }
    return result;
  }

  clone(isRecursive = false) {
    const newMonth = new TimelineMonth(this.date);
    // let clonedMonth = merge(newMonth, this);
    // merge non-plainObjects because MERGE doesn't know how to create these...
    // clonedMonth.tick = merge(new Tick(), this.tick);
    // clonedMonth.reasonForChange = deepClone(this.reasonForChange);
    // clonedMonth.tick = merge(new Tick(), this.tick);
    // return clonedMonth;

    newMonth.id = this.id;
    newMonth.isEdited = this.isEdited;
    newMonth.extensions = this.extensions.slice(); // Copy
    newMonth.paymentIssued = this.paymentIssued;
    newMonth.reasonForChange = this.reasonForChange;
    newMonth.reasonForChangeDetails = this.reasonForChangeDetails;
    newMonth.modifiedBy = this.modifiedBy;

    const modifiedDate = this.modifiedDate ? moment(this.modifiedDate) : null;
    newMonth.modifiedDate = modifiedDate && modifiedDate.isValid() ? modifiedDate : null;

    const createdDate = this.createdDate ? moment(this.createdDate) : null;
    newMonth.createdDate = createdDate && createdDate.isValid() ? moment(this.createdDate) : null;

    newMonth.isDeleted = this.isDeleted;
    newMonth.rowVersion = this.rowVersion;
    newMonth.tick = this.tick.clone();
    newMonth.changeHistory = this.changeHistory.slice();
    newMonth.cachedOriginalModel = isRecursive || this.cachedOriginalModel ? this.cachedOriginalModel : newMonth.clone(true);
    return newMonth;
  }

  public static deserialize(json: TimelineMonthContract, logger: LogService) {
    try {
      let month: TimelineMonth;
      const date = CommonDelCreatedModel.getDateFromString(json.effectiveMonth);
      if (!date.isValid()) {
        throw new Error(`invalid 'effectiveMonth' : ${json.effectiveMonth}`);
      }

      if (json.isDeleted) {
        logger.info('skipping parsing of deleted month: ID:' + json.id);
      }

      month = new TimelineMonth(date);

      if (month.date.isAfter(Utilities.currentDate, 'month')) {
        logger.warn('skipping parsing of deleted month: ID:' + json.id);
      }

      // set BaseModel Props
      CommonDelCreatedModel.setCommonProperties(month, json);

      let clockTypes = <ClockTypes>json.timelimitTypeId;
      //Add Federal flag if needed
      if (json.isFederal) {
        clockTypes |= ClockTypes.Federal;
      }

      //Add State flag if needed
      if (json.isState) {
        clockTypes |= ClockTypes.State;
      }

      //Add NOPlacementLimit flag if needed
      if (ClockType.any(clockTypes, ClockTypes.PlacementLimit) && json.isPlacement === false) {
        clockTypes |= ClockTypes.NoPlacementLimit;
      }

      month.tick.clockTypes = new ClockType(clockTypes);
      month.tick.notes = json.notes;
      month.isEdited = json.isEdited;
      month.paymentIssued = json.paymentIssued;
      month.reasonForChange = json.reasonForChangeId;
      month.reasonForChangeDetails = json.changeReasonDetails;

      month.cachedOriginalModel = month.clone();

      return month;
    } catch (error) {
      throw new Error('Could not deserialize server response. ' + error.message);
    }
  }

  public serialize() {
    const tmContract = new TimelineMonthContract();
    // tmContract.id = this.Id;
    // tmContract.createdDate = this.createdDate ? this.createdDate.format() : null;
    // tmContract.modifiedBy = this.modifiedBy;
    // tmContract.modifiedDate = this.modifiedDate ? this.modifiedDate.format() : null;
    // tmContract.isDeleted = this.isDeleted;
    // tmContract.rowVersion = this.rowVersion;

    BaseModelContract.setCommonProperties(tmContract, this);

    tmContract.timelimitTypeId = this.tick.tickType;
    tmContract.effectiveMonth = this.date.format();
    tmContract.reasonForChangeId = this.reasonForChange;
    tmContract.changeReasonDetails = this.reasonForChangeDetails;
    tmContract.isFederal = this.tick.clockTypes.state.Federal;
    tmContract.isState = this.tick.clockTypes.state.State;
    tmContract.isPlacement = this.tick.clockTypes.state.PlacementLimit && !this.tick.clockTypes.state.NOPlacementLimit;
    tmContract.notes = this.tick.notes;
    tmContract.stateId = this.tick.state;
    return tmContract;
  }
}

export class TimelineMonthValidator extends Validator<TimelineMonth> {
  constructor(instance: TimelineMonth) {
    super(instance);
  }

  public validate(): validation.ValidationResult {
    this.validateDate();
    this.validateReasonForChange();
    this.validateReasonForChangeDetails();
    this.validateStateId();
    return this.validationResult;
  }

  private validateDate() {
    let context = new validation.ValidationContext();
    context.instance = this.instance;
    context.propertyDisplayName = 'date';
    context.propertyName = 'date';
    context.propertyValue = this.instance.date;

    let validator = new validation.NotEmptyValidator('Timelimit month is empty');
    if (!validator.isValid(context)) {
      this.validationResult.errors.push(new validation.ValidationError(validator.getErrorMessage(context)));
    }
  }

  private validateReasonForChange() {
    let context = new validation.ValidationContext();
    context.instance = this.instance;
    context.propertyDisplayName = 'reasonForChange';
    context.propertyName = 'reasonForChange';
    context.propertyValue = this.instance.reasonForChange;

    let notEmptyValidator = new validation.NotEmptyValidator('Reason for change is Required.');
    let greaterThanValidtor = new validation.GreaterThanValidator(0, 'Reason for change is Required.');

    if (!notEmptyValidator.isValid(context)) {
      this.validationResult.errors.push(new validation.ValidationError(notEmptyValidator.getErrorMessage(context)));
      return;
    }

    let shouldCheckGreaterThen =
      (this.instance.id < 1 && !this.instance.tick.clockTypes.state.None) ||
      (this.instance.cachedOriginalModel && !this.instance.tick.clockTypes.eql(this.instance.cachedOriginalModel.tick.clockTypes.valueOf()));

    if (shouldCheckGreaterThen && !greaterThanValidtor.isValid(context)) {
      this.validationResult.errors.push(new validation.ValidationError(greaterThanValidtor.getErrorMessage(context)));
      return;
    }
  }

  private validateReasonForChangeDetails() {
    let context = new validation.ValidationContext();
    context.instance = this.instance;
    context.propertyDisplayName = 'ReasonForChange Details';
    context.propertyName = 'reasonForChangeDetails';
    context.propertyValue = this.instance.reasonForChangeDetails;

    let notEmptyValidator = new validation.NotEmptyValidator('Reason for change details are required.');
    let shouldCheckEmpty =
      (this.instance.id < 1 && !this.instance.tick.clockTypes.state.None) ||
      (this.instance.cachedOriginalModel && !this.instance.tick.clockTypes.eql(this.instance.cachedOriginalModel.tick.clockTypes.valueOf()));

    if (shouldCheckEmpty && !notEmptyValidator.isValid(context)) {
      this.validationResult.errors.push(new validation.ValidationError(notEmptyValidator.getErrorMessage(context)));
      return;
    }
  }

  private validateStateId() {
    let context = new validation.ValidationContext();
    context.instance = this.instance;
    context.propertyDisplayName = 'State';
    context.propertyName = 'state';
    context.propertyValue = this.instance.tick.state;

    let notEmptyValidator = new validation.NotEmptyValidator('State is required for change when using "Benefits from Another State" reason for change.');
    let shouldCheckEmpty = this.instance.tick.clockTypes.state.OTF && this.instance.reasonForChange === 1;
    if (shouldCheckEmpty && !notEmptyValidator.isValid(context)) {
      this.validationResult.errors.push(new validation.ValidationError(notEmptyValidator.getErrorMessage(context)));
    }
  }
}

// export class TimelineMonthValidator2 extends validation.Validator<TimelineMonth> {
//     constructor() {
//         super();

//         this.ruleFor(x => x.date)
//             .notEmpty()
//             .withMessage('');

//         this.ruleFor(x => x.date)
//             .must(x => x.isSameOrBefore(moment(Utilities.currentDate), 'month'))
//             .when(x => x.tick.clockTypes.valueOf() !== ClockTypes.None)
//             .withMessage("Editing Timelimit Type not allowed for month. Only current or previous months may be edited.");

//         this.ruleFor(x => x.reasonForChange)
//             .notEmpty()
//             .greaterThan(0)
//             .withMessage("Reason for change is Required.")
//             .when(x =>
//                     (x.id < 1 && !x.tick.clockTypes.state.None) ||
//                     (x.cachedOriginalModel && !x.tick.clockTypes.eql(x.cachedOriginalModel.tick.clockTypes.valueOf())));

//         this.ruleFor(x => x.reasonForChangeDetails)
//             .notEmpty()
//             .withMessage("Reason for change details is Required.")
//             .when(x =>
//                 (x.id < 1 && !x.tick.clockTypes.state.None) ||
//                 (x.cachedOriginalModel && !x.tick.clockTypes.eql(x.cachedOriginalModel.tick.clockTypes.valueOf())));
//     }

// }
