import { EnumFlagsType, EnumFlagsTest, EnumFlagsTool } from 'ts-enum-tools';
import { EnumEx } from '../../utilities'
export enum ClockStates {
    None = 0,
    Warn = 1,
    Danger = 2,
    CanBeExtended = 4,
    SequenceCanBeDenied = 8,
    WillCauseGapExtension = 16

}

export interface ClockStatesMap {
    None: boolean;
    Warn: boolean;
    Danger: boolean;
    CanBeExtended: boolean;
    SequenceCanBeDenied: boolean;
    WillCauseGapExtension: boolean;
}

export class ClockState implements EnumFlagsTool<ClockStates, ClockStatesMap> {
    eql: (a: ClockStates) => boolean;
    has: (a: ClockStates) => boolean;
    any: (a: ClockStates) => boolean;
    toArray: () => string[];
    toString: () => string;

    private val: ClockStates;

    state: ClockStatesMap;  // Returns map of T/F values

    static eql = (val: ClockStates, flags: ClockStates) => { return EnumFlagsTest.eql(val, flags); };
    static any = (val: ClockStates, flags: ClockStates) => { return EnumFlagsTest.any(val, flags); };
    static has = (val: ClockStates, flags: ClockStates) => { return EnumFlagsTest.has(val, flags); };
    static toString = (val?: ClockStates) => {
        if (val) {
            return ClockState.toArray(val).join('|');
        } else {
            return EnumEx.getNames(ClockStates).join('|');
        }
    }

    static toArray = (val?: ClockStates) => {
        let returnValues: string[] = [];
        if (val) {
            EnumEx.getNamesAndValues(ClockStates).map(x => {
                if (ClockState.any(val, x.value)) {
                    returnValues.push(x.name);
                }
            });
        } else {
            returnValues = EnumEx.getNamesAndValues(ClockStates).map(x => x.name);
        }

        return returnValues;
        // return EnumEx.getNames(val);
    }

    constructor(flags: ClockStates = ClockStates.None) {
        this.val = flags;
        // TODO: Redo with EnumEx
        // let keys = {};
        // const hash = Object.keys(ClockStates).reduce(function (obj, k) {
        //     // Excludes bi-directional numeric keys
        //     if (isNaN(<any>k)) {
        //         obj[k] = +ClockStates[k];
        //     }
        //     // keys[k] = k;
        //     return obj;
        // }, {});

        // const hash2 = new Map<string, number>();

        // EnumEx.getNames(flags).map(v => {
        //     hash2.set(v, +ClockStates[v]);
        // });

        this.eql = (a) => { return ClockState.eql(this.val, a); };
        this.any = (a) => { return ClockState.any(this.val, a); };
        this.has = (a) => { return ClockState.has(this.val, a); };
        this.toString = () => { return ClockState.toString(this.val); };
        this.toArray = () => { return ClockState.toArray(this.val); };

        this.state = {
            None: this.eql(ClockStates.None),
            Warn: this.has(ClockStates.Warn),
            Danger: this.has(ClockStates.Danger),
            CanBeExtended: this.has(ClockStates.CanBeExtended),
            SequenceCanBeDenied: this.has(ClockStates.SequenceCanBeDenied),
            WillCauseGapExtension: this.has(ClockStates.WillCauseGapExtension)
        };
    }
    valueOf() {
        return this.val.valueOf();
    }

}
