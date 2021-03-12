/**
 * Provides validity information for section objects.  This type is a dictionary
 * where a string value should match the name of the section.
 *
 * @export
 * @interface SectionValid
 */
export interface SectionValid {
  // Each property will be a string with a boolean value where true indicates
  // the section has data and has passed the validate method.  A false indicates
  // the section has data and has failed the validate method.  A null or missing
  // value indicates the data has never been saved for that assessment.
  [property: string]: boolean;
}
