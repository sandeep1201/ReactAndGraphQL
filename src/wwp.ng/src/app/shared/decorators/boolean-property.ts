
export function coerceBoolean() {
  return function (target: Object, propertyKey: string | symbol) {
    // note "this" is undefined for some reason, and Target is the prototype.
    // we don't want to scope the variable to the function on the prototype because
    // all instances will share the same property. Therefore we are using (hopefully)
    // unique key in "this" in the setter/getter

    // Setter
    let setter = function (newVal: any) {
      this[getSymbolKey(propertyKey)] = coerceBooleanProperty(newVal);
    };

    // Getter
    let getter = function () {
      return this[getSymbolKey(propertyKey)];
    };

    if (delete target[propertyKey]) {
      Object.defineProperty(target, propertyKey, {
        get: getter,
        set: setter,
        enumerable: true,
        configurable: true
      });
    }
  }
}

function getSymbolKey(propertyKey: string | symbol) {
  let prefix = "__cb_";
  if (typeof propertyKey === 'string') {
    return prefix + propertyKey;
  } else {
    return prefix + Symbol.keyFor(propertyKey);
  }
}

/** Coerces a data-bound value (typically a string) to a boolean. */
export function coerceBooleanProperty(value: any): boolean {
  return value != null && `${value}` !== 'false';
}
