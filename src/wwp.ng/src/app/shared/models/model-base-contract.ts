import * as moment from 'moment';
export abstract class CommonDelCreatedModel {
    public id: number = 0;
    public createdDate: moment.Moment;
    public modifiedBy: string;
    public modifiedDate: moment.Moment;
    public isDeleted: boolean;
    public rowVersion: string;

    public static getDateFromString(dateString: string, format: moment.MomentFormatSpecification = moment.ISO_8601) {
        const dateObj = dateString ? moment(dateString, format) : null;
        return dateObj && dateObj.isValid() ? dateObj : null;
    }

    public static setCommonProperties(instance: CommonDelCreatedModel, contract: BaseModelContract): void {
        instance.id = contract.id || 0;
        instance.modifiedBy = contract.modifiedBy;
        instance.modifiedDate = CommonDelCreatedModel.getDateFromString(contract.modifiedDate);
        instance.isDeleted = contract.isDeleted;
        instance.rowVersion = contract.rowVersion;
        instance.createdDate = CommonDelCreatedModel.getDateFromString(contract.createdDate);
    }
}

export class BaseModelContract {
    public id: number;
    public createdDate: string;
    public modifiedBy: string;
    public modifiedDate: string;
    public isDeleted: boolean;
    public rowVersion: string;

    public static setCommonProperties(instance: BaseModelContract, model: CommonDelCreatedModel): void {
        instance.id = model.id;
        instance.modifiedBy = model.modifiedBy;
        instance.modifiedDate = model.modifiedDate && model.modifiedDate.isValid() ? model.modifiedDate.format() : null;
        instance.isDeleted = model.isDeleted;
        instance.rowVersion = model.rowVersion;
        instance.createdDate = model.createdDate && model.createdDate.isValid() ? model.createdDate.format() : null;
    }
}
