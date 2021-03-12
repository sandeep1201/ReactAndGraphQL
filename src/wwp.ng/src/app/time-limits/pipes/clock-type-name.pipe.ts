import { Pipe, PipeTransform } from '@angular/core';
import {ClockTypes, ClockType} from "../../shared/models/time-limits/clocktypes";

@Pipe({
  name: 'clockTypeName'
})
export class ClockTypeNamePipe implements PipeTransform {

  transform(value: ClockTypes, showNone = false ): string {
    if(value == null || value.toString() === ''){
      return null;
    }

    if(value === ClockTypes.NoPlacementLimit){
      return "NO24";
    }else if(value === ClockTypes.W2T){
      return "W-2 T";
    }else if(!showNone && value === ClockTypes.None){
      return null;
    }

      return ClockTypes[value];
  }

}
