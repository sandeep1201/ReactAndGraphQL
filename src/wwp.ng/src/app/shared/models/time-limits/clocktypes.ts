import { EnumFlagsType, EnumFlagsTest, EnumFlagsTool } from 'ts-enum-tools';
import { EnumEx } from '../../utilities'
// These are ^2 so we can use the enum values as Flags (bitwise operators)
// tslint:disable:no-bitwise

export enum ClockTypes {
    None = 0,
    Federal = 1,            // 1 << 0  =  00000000000001 = 1
    State = 2,              // 1 << 1  =  00000000000010 = 2
    CSJ = 4,                // 1 << 2  =  00000000000100 = 4
    W2T = 8,                // 1 << 3  =  00000000001000 = 8
    TNP = 16,               // 1 << 4  =  00000000010000 = 16
    TMP = 32,               // 1 << 5  =  00000000100000 = 32
    CMC = 64,               // 1 << 6  =  00000001000000 = 64
    OPC = 128,              // 1 << 7  =  00000010000000 = 128
    OTF = 256,              // 1 << 8  =  00000100000000 = 256
    TRIBAL = 512,           // 1 << 9  =  00001000000000 = 512
    TJB = 1024,             // 1 << 10 =  00010000000000 = 1024
    JOBS = 2048,            // 1 << 11 =  00100000000000 = 2048
    NoPlacementLimit = 4096,             // 1 << 12 =  01000000000000 = 4096
    //FEO = 8192,// 1 << 13 =  10000000000000 = 8192
    TEMP = TNP | TMP,
    OTHER = OTF | TRIBAL | TJB | JOBS | NoPlacementLimit,
    PlacementLimit = CSJ | W2T | TEMP, // refactor to PlacementLimit
    ExtensableTypes = PlacementLimit | State,
    CreateableTypes = OTF | TRIBAL | TJB | JOBS | OPC | CMC | PlacementLimit
}

export interface ClockTypeMap {
    None: boolean;
    Federal: boolean;
    State: boolean;
    CSJ: boolean;
    W2T: boolean;
    TNP: boolean;
    TMP: boolean;
    CMC: boolean;
    OPC: boolean;
    OTF: boolean;
    TRIBAL: boolean;
    TJB: boolean;
    JOBS: boolean;
    TEMP: boolean;
    NOPlacementLimit:boolean; // TODO: rename this to something less specific the business can agree on
    OTHER: boolean;
    PlacementLimit: boolean;
    ExtensibleTypes: boolean;
}

export class ClockType implements EnumFlagsTool<ClockTypes, ClockTypeMap> {
    static eql = (val:ClockTypes,flags:ClockTypes) => { return EnumFlagsTest.eql(val,flags); };
    static any = (val:ClockTypes,flags:ClockTypes) => { return EnumFlagsTest.any(val,flags); };
    static has = (val:ClockTypes,flags:ClockTypes) => { return EnumFlagsTest.has(val,flags); };
    static toString = (val?:ClockTypes) => {
      if(val) {
        return ClockType.toArray(val).join('|');
      }else{
        return EnumEx.getNames(ClockTypes).join('|');
      }
    };
    static toArray = (val?:ClockTypes) => {
      let returnValues: string[] = [];
      if(val) {
        EnumEx.getNamesAndValues(ClockTypes).map(x => {
          if (ClockType.any(val, x.value)) {
            returnValues.push(x.name);
          }
        });
      }else{
        returnValues =  EnumEx.getNamesAndValues(ClockTypes).map(x=>x.name);
      }

      return returnValues;
      //return EnumEx.getNames(val);
    };

    public val: ClockTypes;
    state: ClockTypeMap;  // Returns map of T/F values
    eql: (a: ClockTypes) => boolean;
    has: (a: ClockTypes) => boolean;
    any: (a: ClockTypes) => boolean;
    toArray: () => string[];
    toString: () => string;


    // private _clockTypeProp: EnumFlagsTool<ClockTypes, ClockTypeMap>;
    // get clockTypeProp(): EnumFlagsTool<ClockTypes, ClockTypeMap> {
    //     if (this._clockTypeProp == null) {
    //         const flagFunc = EnumFlagsType<ClockTypes, ClockTypeMap>(ClockTypes);
    //         this._clockTypeProp = {
    //             state: flagFunc(this.val).state,  // Returns map of T/F values
    //             eql: flagFunc(this.val).eql,
    //             has: flagFunc(this.val).has,
    //             any: flagFunc(this.val).any,
    //             toArray: flagFunc(this.val).toArray,
    //             toString: flagFunc(this.val).toString
    //         };
    //     }
    //     return this._clockTypeProp;
    // }

    constructor(clockType: ClockTypes) {

        this.val = +clockType;

        // TODO: Redo with EnumEx
        // let keys = {};
        let hash = Object.keys(ClockTypes).reduce(function (obj, k) {
            // Excludes bi-directional numeric keys
            if (isNaN(<any>k)) {
                obj[k] = +ClockTypes[k];
            }
            // keys[k] = k;
            return obj;
        }, {});

        let hash2 = new Map<string, number>();

        EnumEx.getNames(clockType).map(v => {
            hash2.set(v, +ClockTypes[v])
        });

        this.eql = (a) => { return ClockType.eql(this.val, a); };
        this.any = (a) => { return ClockType.any(this.val, a); };
        this.has = (a) => { return ClockType.has(this.val, a); };
        this.toString = () => { return ClockType.toString(this.val); }
        this.toArray = () => { return ClockType.toArray(this.val); };

        this.state = {
            None: this.eql(ClockTypes.None),
            Federal: this.has(ClockTypes.Federal),
            State: this.has(ClockTypes.State),
            CSJ: this.has(ClockTypes.CSJ),
            W2T: this.has(ClockTypes.W2T),
            TNP: this.has(ClockTypes.TNP),
            TMP: this.has(ClockTypes.TMP),
            CMC: this.has(ClockTypes.CMC),
            OPC: this.has(ClockTypes.OPC),
            OTF: this.has(ClockTypes.OTF),
            TRIBAL: this.has(ClockTypes.TRIBAL),
            TJB: this.has(ClockTypes.TJB),
            JOBS: this.has(ClockTypes.JOBS),
            TEMP: this.any(ClockTypes.TEMP),
            OTHER: this.any(ClockTypes.OTHER),
            PlacementLimit: this.any(ClockTypes.PlacementLimit),
            NOPlacementLimit: this.has(ClockTypes.NoPlacementLimit),
            ExtensibleTypes: this.any(ClockTypes.ExtensableTypes)
        }

    }

    public filterCombos(){
        return ClockType.filterCombos(this.valueOf())
    }

    public static filterCombos(clockType: ClockTypes){
        // These Types should exists on there own
        if(!ClockType.eql(clockType,ClockTypes.Federal) && !ClockType.eql(clockType,ClockTypes.State) && !ClockType.eql(clockType,ClockTypes.NoPlacementLimit) && !ClockType.eql(clockType,ClockTypes.TEMP)) {
          clockType &= ~ClockTypes.Federal;
          clockType &= ~ClockTypes.State;
          clockType &= ~ClockTypes.NoPlacementLimit;
          //clockType &= ~ClockTypes.TEMP;
        }
        return clockType;
    }

    public static IsSingleFlag(val: ClockType | ClockTypes){
      let n:number;
      if(val instanceof ClockType) {
        n = val.valueOf();
      }else{
        n = +val;
      }
      return n === 0 || (n & (n-1)) ===0
    }

    valueOf() {
        return this.val.valueOf();
    }

}
