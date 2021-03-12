import * as moment from 'moment';

export class TransactionModel {
  public id: number;
  public participantId: number;
  public workerId: number;
  public officeId: number;
  public transactionTypeId: number;
  public description: string;
  public createdDate: string;
  public effectiveDate: string;
  public modifiedBy: string;
  public modifiedDate: string;
  public workerName: string;
  public agencyId: number;
  public agencyName: string;
  public countyName: string;
  public transactionTypeName: string;
  public transactionTypeCode: string;
  public transactionTypeDescription: string;

  public static clone(input: any, instance: TransactionModel) {
    instance.id = input.id;
    instance.participantId = input.participantId;
    instance.workerId = input.workerId;
    instance.officeId = input.officeId;
    instance.transactionTypeId = input.transactionTypeId;
    instance.description = input.description;
    instance.createdDate = input.createdDate ? moment(input.createdDate).format('MM/DD/YYYY') : input.createdDate;
    instance.effectiveDate = input.effectiveDate ? moment(input.effectiveDate).format('MM/DD/YYYY') : input.effectiveDate;
    instance.modifiedBy = input.modifiedBy;
    instance.modifiedDate = input.modifiedDate ? moment(input.modifiedDate).format('MM/DD/YYYY') : input.modifiedDate;
    instance.workerName = input.workerName;
    instance.agencyId = input.agencyId;
    instance.agencyName = input.agencyName;
    instance.countyName = input.countyName;
    instance.transactionTypeName = input.transactionTypeName;
    instance.transactionTypeCode = input.transactionTypeCode;
    instance.transactionTypeDescription = input.transactionTypeDescription;
  }

  public deserialize(input: any) {
    TransactionModel.clone(input, this);
    return this;
  }
}
