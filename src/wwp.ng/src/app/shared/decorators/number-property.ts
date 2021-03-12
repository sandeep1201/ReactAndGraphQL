
export function coerceNumber(){
  return function (target: Object, propertyKey: string | symbol){
    // note "this" is undefined for some reason, and Target is the prototype.
    // we don't want to scope the variable to the function on the prototype because
    // all instances will share the same property. Therefore we are using (hopefully)
    // unique key in "this" in the setter/getter


    // Setter
    let setter = function(newVal:any){
      this[getSymbolKey(propertyKey)] = coerceNumberProperty(newVal);
    }

    // Getter
    let getter = function(){
      return this[getSymbolKey(propertyKey)];
    };

    if(delete target[propertyKey]){
      Object.defineProperty(target,propertyKey,{
        get: getter,
        set:setter,
        enumerable:true,
        configurable:true
      });
    }
  }
}

function getSymbolKey(propertyKey: string | symbol){
  let prefix = "__cn_";
  if(typeof propertyKey === 'string'){
    return prefix + propertyKey;
  }else{
    return prefix +  Symbol.keyFor(propertyKey);
  }
}

/** Coerces a data-bound value (typically a string) to a number. */
export function coerceNumberProperty(value: any, fallbackValue = 0) {
  // parseFloat(value) handles most of the cases we're interested in (it treats null, empty string,
  // and other non-number values as NaN, where Number just uses 0) but it considers the string
  // '123hello' to be a valid number. Therefore we also check if Number(value) is NaN.
  return isNaN(parseFloat(value as any)) || isNaN(Number(value)) ? fallbackValue : Number(value);
}
