/**
 * Tracks errors for model objects.  This type is a dictionary where a string
 * value should match the property name from the model.
 *
 * @export
 * @interface ModelErrors
 */
export interface ModelErrors {
  // Each property will be a string with a boolean value where true indicates
  // the property has an error.  To support nesting of complex objects and
  // arrays, a value could also be another ModelErrors object or array of them.
  [property: string]: boolean | ModelErrors | ModelErrors[];
}
