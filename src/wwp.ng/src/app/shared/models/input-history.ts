

export class InputHistory {
    id: number;
    value: boolean | string | number;
    displayValue: boolean | string;
    changeType: string;
    isDeleted: boolean;

    // TODO: deleteReason.
    deleteReason: any;
    modifiedBy: string;
    modifiedDate: string;
    rowVersion: string;
}
