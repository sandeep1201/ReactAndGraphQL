import { FinalistAddress } from 'src/app/shared/models/finalist-address.model';
import { Serializable } from 'src/app/shared/interfaces/serializable';
import * as moment from 'moment';
import { Utilities } from 'src/app/shared/utilities';
import { ValidationManager, ValidationResult, ValidationCode } from 'src/app/shared/models/validation';
import { MmDdYyyyValidationContext } from 'src/app/shared/interfaces/mmDdYyyy-validation-context';
import { EARequest } from './ea-request.model';

export class EAPayment implements Serializable<EAPayment> {
  id: number;
  requestId: number;
  voucherOrCheckNumber: string;
  voucherOrCheckDate: string;
  voucherOrCheckAmount: string;
  payeeName: string;
  mailingAddress: FinalistAddress;
  notes: string;
  modifiedBy: string;
  modifiedDate: string;
  isDeleted: string;

  public static create(requestId: number): EAPayment {
    const ea = new EAPayment();
    ea.id = 0;
    ea.requestId = requestId;
    ea.mailingAddress = new FinalistAddress();
    return ea;
  }

  public static clone(input: any, instance: EAPayment) {
    instance.id = input.id;
    instance.requestId = input.requestId;
    instance.voucherOrCheckNumber = input.voucherOrCheckNumber;
    instance.voucherOrCheckDate = input.voucherOrCheckDate ? moment(input.voucherOrCheckDate).format('MM/DD/YYYY') : input.voucherOrCheckDate;
    instance.voucherOrCheckAmount = input.voucherOrCheckAmount;
    instance.payeeName = input.payeeName;
    instance.mailingAddress = input.mailingAddress ? Utilities.deserilizeChild(input.mailingAddress, FinalistAddress) : new FinalistAddress();
    instance.notes = input.notes;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate;
    instance.isDeleted = input.isDeleted;
  }

  public deserialize(input: any) {
    EAPayment.clone(input, this);
    return this;
  }

  public validate(validationManager: ValidationManager, eaRequestModel: EARequest, totalPayment: string): ValidationResult {
    const result = new ValidationResult();

    const currentDate = Utilities.currentDate.clone();
    const maxAllowedDate = currentDate.format('MM/DD/YYYY');
    const voucherDateContext: MmDdYyyyValidationContext = {
      date: this.voucherOrCheckDate,
      prop: 'voucherOrCheckDate',
      prettyProp: 'Voucher/Check Date',
      result: result,
      validationManager: validationManager,
      isRequired: false,
      maxDate: maxAllowedDate,
      maxDateAllowSame: true,
      maxDateName: maxAllowedDate,
      participantDOB: null,
      minDateAllowSame: true,
      minDate: eaRequestModel.eaDemographics.applicationDate,
      minDateName: eaRequestModel.eaDemographics.applicationDate
    };
    Utilities.validateMmDdYyyyDate(voucherDateContext);

    const maxCheckAmount =
      Utilities.currencyToNumber(eaRequestModel.approvedPaymentAmount) -
      (this.id > 0
        ? Utilities.currencyToNumber(totalPayment) - Utilities.currencyToNumber(eaRequestModel.eaPayments.find(x => x.id === this.id).voucherOrCheckAmount)
        : Utilities.currencyToNumber(totalPayment));
    if (this.voucherOrCheckAmount == null || this.voucherOrCheckAmount.toString() === '') {
      result.addError('voucherOrCheckAmount');
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Voucher/Check Amount');
    } else if (this.voucherOrCheckAmount && Utilities.currencyToNumber(this.voucherOrCheckAmount) > maxCheckAmount) {
      result.addError('voucherOrCheckAmount');
      validationManager.addErrorWithDetail(ValidationCode.DuplicateDataWithNoMessage, 'Sum of voucher/check amounts is greater than approved payment amount.');
    }

    if (this.payeeName == null || this.payeeName.trim() === '') {
      validationManager.addErrorWithDetail(ValidationCode.RequiredInformationMissing_Details, 'Payee Name');
      result.addError('payeeName');
    }

    let mailingResult = new ValidationResult();
    mailingResult = this.mailingAddress.validateSave(validationManager);
    Object.keys(mailingResult.errors).forEach(item => {
      result.addError(item);
    });

    return result;
  }
}
