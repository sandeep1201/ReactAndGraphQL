import {isPlainObject, kindOf, forOwn} from "./object-utils";
import {clone} from "./clone";
import * as moment from 'moment';
import {DateRange} from "../moment-range";
/**
 * Created by terry.fraser on 3/29/2017.
 */
export function deepClone(val,instanceClone:any = null){
  switch ( kindOf(val) ) {
    case 'Object':
      if(moment.isMoment(val)){
        return val.clone();
      }
      //use defined clone property if available
      // if(val && typeof(val.clone) === 'function'){
      //   try{
      //     return val.clone();
      //   }catch(error){
      //     console.log("Error using object's defined clone property. Cloning manually.");
      //   }
      // }

      return cloneObject(val, instanceClone);


    case 'Array':
      return cloneArray(val, instanceClone);
    default:
      return clone(val);
  }
}

function cloneObject(source, instanceClone) {
  if (isPlainObject(source)) {
    let out:any = {};
    forOwn(source, function(val, key) {
      this[key] = deepClone(val, instanceClone);
    }, out);
    return out;
  } else if (instanceClone) {
    return instanceClone(source);
  } else {
    return source;
  }
}

function cloneArray(arr, instanceClone) {
  let out = [],
    i = -1,
    n = arr.length,
    val;
  while (++i < n) {
    out[i] = deepClone(arr[i], instanceClone);
  }
  return out;
}

