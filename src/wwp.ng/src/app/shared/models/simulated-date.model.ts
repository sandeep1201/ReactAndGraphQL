import { Serializable } from '../interfaces/serializable';
import { ValidationManager } from './validation-manager';
import { ValidationResult } from './validation-result';
import * as moment from 'moment';
import { ValidationCode } from './validation-error';

export class SimulatedDate implements Serializable<SimulatedDate> {
    public id: number;
    public wuid: string;
    public startTimeStamp: string;
    public endTimeStamp: string;
    public cdoDate: string;
    public modifiedBy: string;
    public modifiedDate: string;
    public rowVersion: string;

    get cdoDateMoment(): moment.Moment {
      return moment(this.cdoDate, 'MM/DD/YYYY');
    }

  public static create(): SimulatedDate {
    const simulatedDate = new SimulatedDate();
    simulatedDate.id = 0;
    simulatedDate.startTimeStamp = moment().format();
    return simulatedDate;
  }

  public static clone(input: any, instance: SimulatedDate) {
    instance.id = input.id;
    instance.wuid = input.wuid;
    instance.startTimeStamp = input.startTimeStamp;
    instance.endTimeStamp = input.endTimeStamp;
    instance.cdoDate = input.cdoDate;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.rowVersion = input.rowVersion;
  }

  public deserialize(input: any) {
    SimulatedDate.clone(input, this);
    return this;
  }

  validate(validationManager: ValidationManager): ValidationResult {
    const result = new ValidationResult();

    const maxSimulatedDate = moment(moment(), 'MM/DD/YYYY').add(2, 'years').format('MM/DD/YYYY');
    const minSimulatedDate = moment(moment(), 'MM/DD/YYYY').subtract(2, 'years').format('MM/DD/YYYY');

    if (this.cdoDate === null || this.cdoDate === undefined || this.cdoDate === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Simulated Date');
      result.addError('simulatedDate');
    }

    if (this.cdoDate !== '' && this.cdoDate !== null && this.cdoDate !== undefined && this.cdoDate.length !== 10) {
      validationManager.addErrorWithFormat(ValidationCode.ValueInInvalidFormat_Name_FormatType, 'Simulated Date', 'MM/DD/YYYY');
      result.addError('simulatedDate');
    } else if (this.cdoDate !== null && this.cdoDate !== undefined && this.cdoDate !== '' && this.cdoDateMoment.isValid()) {
      if (this.cdoDateMoment.isBefore(minSimulatedDate)) {
        validationManager.addErrorWithFormat(ValidationCode.DateBeforeMinSimulatedDate, 'Simulated Date');
        result.addError('simulatedDate');
      }

      if (this.cdoDateMoment.isAfter(maxSimulatedDate)) {
        validationManager.addErrorWithFormat(ValidationCode.DateAfterMaxSimulatedDate, 'Simulated Date');
        result.addError('simulatedDate');
      }
    } else if (this.cdoDate !== '' && this.cdoDate !== null && this.cdoDate !== undefined && !this.cdoDateMoment.isValid()) {
      validationManager.addErrorWithDetail(ValidationCode.ValueOutOfRange_Details, 'Invalid Simulated Date Entered');
      result.addError('simulatedDate');
    }

    return result;
  }
}
