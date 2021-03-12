/**
 * Created by terry.fraser on 3/29/2017.
 */
import {forOwn} from "./object-utils";
/**
 * Combine properties from all the objects into first one.
 * - This method affects target object in place, if you want to create a new Object pass an empty object as first param.
 * @param {object} target    Target Object
 * @param {...object} objects    Objects to be combined (0...n objects).
 * @return {object} Target Object.
 */
export function mixIn(target, objects){
  let i = 0,
    n = arguments.length,
    obj;
  while(++i < n){
    obj = arguments[i];
    if (obj != null) {
      forOwn(obj, copyProp, target);
    }
  }
  return target;
}

function copyProp(val, key){
  this[key] = val;
}