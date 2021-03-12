import {ClockType, ClockTypes} from './clocktypes';


export class Tick {
    Id: number;
    public clockTypes: ClockType;
    public state: number;
    constructor(clockTypes: ClockTypes = ClockTypes.None, public notes: string = '') {
        this.clockTypes = new ClockType(clockTypes);
        return this;
    }

    clone(): Tick {
        const cloneObj = new Tick(this.clockTypes.valueOf(), this.notes);
        cloneObj.Id = this.Id;
        cloneObj.state = this.state;
        return cloneObj;
    }

    get tickType(): ClockTypes {
        return this.clockTypes.filterCombos();
    }
}
