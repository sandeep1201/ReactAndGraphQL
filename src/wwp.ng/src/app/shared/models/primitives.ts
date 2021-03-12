import { DropDownField } from './dropdown-field';
import { Serializable } from '../interfaces/serializable';

export class YesNo {
    YesOrNo: DropDownField[];

    constructor() {
        this.YesOrNo = [];

        const y = new DropDownField();
        y.id = true;
        y.name = 'Yes';
        this.YesOrNo.push(y);

        const n = new DropDownField();
        n.id = false;
        n.name = 'No';
        this.YesOrNo.push(n);
    }
}

export class YesNoStatus implements Serializable<YesNoStatus> {
    status: number;
    statusName: string;
    details: string;

    public static create(): YesNoStatus {
        const x = new YesNoStatus();
        x.status = null;
        x.statusName = null;
        x.details = null;
        return x;
    }

    deserialize(input: any) {
        this.status = input.status;
        this.statusName = input.statusName;
        this.details = input.details;

        return this;
    }

}

export class TextDetail implements Serializable<TextDetail> {
    id: number;
    details: string;
    isDeleted: boolean;

    public static create(): TextDetail {
        const td = new TextDetail();
        td.id = 0;
        td.details = '';
        td.isDeleted = false;
        return td;
    }

    deserialize(input: any) {
        this.id = input.id;
        this.details = input.details;
        this.isDeleted = input.isDeleted;
        return this;
    }
}

export class Payload {
    public trivalue: Trivalue[] = [];
    public stringArray: string[] = [];

}
export class Trivalue {
    first: any;
    second: any;
    third: any[];
}

export class CalculatedString {
    value: string;
    units: string;
    isCalculated: boolean;
}

export class GenericItem {
    value: any;
}


