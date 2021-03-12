import {forIn} from "./forIn";
let _rKind = /^\[object (.*)\]$/;
let _toString = Object.prototype.toString;
let undef;

export function kindOf(val:any):string{
  if(val === null) {
    return 'Null'
  }else if(val ===  undef){
    return 'Undefined'
  }
  else {
    return _rKind.exec(_toString.call(val))[1] ;
  }
}

export function isKind(val,type:string){
  return kindOf(val) === type;
}

export function isPlainObject(val:any){
  return (!!val && typeof val === 'object' && val.constructor === Object);
}
export function isObject(val){
  return isKind(val, "Object")
}

export function hasOwn(obj, prop){
  return Object.prototype.hasOwnProperty.call(obj, prop);
}

/**
 * Similar to Array/forEach but works over object properties and fixes Don't
 * Enum bug on IE.
 * based on: http://whattheheadsaid.com/2010/10/a-safer-object-keys-compatibility-implementation
 */
export function forOwn(obj, fn, thisObj) {
  forIn(obj, function (val, key) {
    if (hasOwn(obj, key)) {
      return fn.call(thisObj, obj[key], key, obj);
    }
  });
}
