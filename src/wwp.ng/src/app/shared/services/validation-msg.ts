export const ValidationMsg = [
  { enum: 'RequiredInformationMissing', code: 101, title: 'Required information not provided', message: 'The following information is needed to complete this section:' },
  { enum: 'RequiredInformationMissing_Details', code: 102, title: 'Required information not provided', message: 'The following information is needed to complete this section:' },
  {
    enum: 'ValueOutOfRange_Details',
    code: 103,
    title: 'Value out of range',
    message: "The information you've provided is not within an acceptable range.  Correct the following information:"
  },
  {
    enum: 'ValueInInvalidFormat_Name_FormatType',
    code: 104,
    title: 'Incorrect Format',
    message: 'You have provided {0} in the wrong format. The field requires inputs following the {1} pattern.'
  },
  {
    enum: 'ValueBeforeDOB_Name_Value_DOB',
    code: 105,
    title: 'Invalid Data',
    message: "For the {0} field, you have entered a date of {1}, which is before the participant's date of birth {2}. "
  },
  { enum: 'ValueSameValue_Name1_Name2', code: 113, title: 'Invalid Date Provided', message: 'The {0} field cannot be the same as {1}.' },
  { enum: 'DateAfterDate_Name1_Name2', code: 109, title: 'Invalid Date Provided', message: 'The {0} field cannot be after {1}.' },
  { enum: 'DateBeforeDate_Name1_Name2', code: 106, title: 'Invalid Date Provided', message: 'The {0} field cannot be before {1}.' },
  {
    enum: 'ValueBefore_X_PlusDOB_Name_Value_DOB',
    code: 107,
    title: 'Invalid Date Provided',
    message: "For the {0} field, you have entered a date of {1} which is more than {3} years after the participant's date of birth {2}."
  },
  { enum: 'InvalidDate', code: 108, title: 'Invalid Date Provided', message: 'The date entered is not a valid date.' },
  { enum: 'RequiredInformationMissing', code: 118, title: 'Required information not provided' },
  { enum: 'ValuesExceedRange', code: 123, title: '' },
  { enum: 'InformationIncorrect', code: 199, title: 'Incorrect Format', message: 'Correct the following information:' },

  { enum: 'DateAfterCurrent', code: 110, title: 'Invalid Date Provided', message: 'For the {0} field, you have entered a date of {1} which is after the current date. ' },
  { enum: 'DateBeforeCurrent', code: 111, title: 'Invalid Date Provided', message: 'For the {0} field, you have entered a date of {1} which is before the current date. ' },
  {
    enum: 'DateAfterDuration',
    code: 112,
    title: 'Invalid Date Provided',
    message: 'For the {0} field, you have entered a date of {1} which is more than {2} after the current date. '
  },
  { enum: 'DateBeforeEnrollmentDate', code: 114, title: 'Invalid Date Provided', message: '{0} Job Begin Date, must be on or after the {1} enrollment date  {2}. ' },
  { enum: 'DateAfterDisenrollmentDate', code: 115, title: 'Invalid Date Provided', message: '{0} Job End Date must be on or before the {1} disenrollment date {2}. ' },
  { enum: 'DateBeforeDisenrollmentDate', code: 116, title: 'Invalid Date Provided', message: '{0} Job Begin Date must be on or before the {1} disenrollment date {2}. ' },

  { enum: 'DateSameAsEpBeginDate', code: 117, title: 'Invalid Date Provided', message: 'EP End Date must be after the begin date. ' },
  { enum: 'DateBeforeProgramEnrollmentDate', code: 150, title: 'Invalid Date Provided', message: 'The {0} field cannot be before the enrollment date {1}.' },
  { enum: 'DateBeforeMaxDaysCanBackDate', code: 151, title: 'Value out of range', message: 'The {0} field cannot be more than {1} days before the current date.' },
  {
    enum: 'ParticipationPeriodCheck',
    code: 152,
    title: 'Value out of range',
    message: 'When action is taken after Pulldown, the {0} Date can be no earlier than {1} day of the {2} participation period.'
  },

  { enum: 'DateBeforeMinSimulatedDate', code: 160, title: 'Invalid Date Provided', message: 'WWP does not allow simulation more than 2 years prior to current date.' },
  { enum: 'DateAfterMaxSimulatedDate', code: 161, title: 'Invalid Date Provided', message: 'WWP does not allow simulation more than 2 years after the current date.' },

  { enum: 'InvalidText', code: 119, title: 'Invalid Text ', message: 'For the {0} field, you may only {1}.' },
  { enum: 'InvalidChar', code: 120, title: 'Invalid Character added.', message: 'For the {0} field, you have entered {1} which is not allowed. ' },
  { enum: 'DuplicateData', code: 121, title: 'Invalid Data', message: 'Please make sure there are no duplicates.' },
  { enum: 'DuplicateData', code: 122, title: 'Invalid Data' },

  { enum: 'LanguageInfoIncomplete', code: 2000, title: 'Language information incomplete', message: 'You have to read, write, or speak a language in order to know it.' },
  { enum: 'LanguageEnglishInfoIncomplete', code: 2001, title: 'English information incomplete', message: 'Indicate whether the participant can read, write and speak English.' },
  { enum: 'LanguageMustBeUnique', code: 2002, title: 'Language may only be selected once', message: 'Each language may only be selected once.' },

  {
    enum: 'EducationHistoryLastYearAttendedInvalid_ParticipantDob',
    code: 2101,
    title: 'Last Year Attended Invalid',
    message: "Participant was born in '{0}'. Correct the year he/she last attended school."
  },
  {
    enum: 'EducationHistoryLastYearAttendedInFuture',
    code: 2102,
    title: 'Last Year Attended Invalid',
    message: 'Correct year he/she last attended school. It cannot be in the future.'
  },
  {
    enum: 'EducationHistoryYearAwardedAfterCurrentYear_GedOrHsed',
    code: 2103,
    title: 'Year Awarded is invalid',
    message: 'Correct the year the {0} was awarded. It cannot be in the future.'
  },
  {
    enum: 'EducationHistoryYearAwardedPastForGedOrHsed',
    code: 2106,
    title: 'Year Awarded is invalid',
    message: "Year Awarded for GED or HSED must be in the past. If the participant is currently working on their GED or HSED, select 'None' for high school graduation status."
  },
  {
    enum: 'EducationHistoryLastYearAttendedpriorDobInvalid',
    code: 2107,
    title: 'Last Year Attended is invalid',
    message:
      "You have indicated that the last year that the participant attended elementary, middle, or high school was prior to the participant's date of birth 'dobYYYY'. Please enter the correct year."
  },
  {
    enum: 'EducationHistoryPastLastYear',
    code: 2108,
    title: 'Last Year Attended is invalid',
    message: 'Last Year Attended must be a past or current year. Enter the correct year.'
  },
  {
    enum: 'EducationHistoryPastSchoolAttendanceRequired',
    code: 2109,
    title: 'Past school attendance status is required',
    message: "In order to submit the informal assessment, you will need to select 'Yes' or 'No' to indicate whether the participant has ever attendedschool."
  },
  {
    enum: 'EucationHistoryCurrentEnrolledRequired',
    code: 2110,
    title: 'Currently Enrolled is required',
    message: "In order to submit the informal assessment,you will need to select 'Yes'or 'No' indicate whether the participant is currently enrolled in school."
  },

  { enum: 'WorkProgramsPastStatus_Name', code: 2500, title: 'Invalid Date for Work Program Status Type', message: "A Past work program's {0} field must be in the past." },
  {
    enum: 'WorkProgramsCurrentFutureStartDate',
    code: 2501,
    title: 'Invalid Date for Work Program Status Type',
    message: "A Current work program's start date can not be in the future."
  },
  { enum: 'WorkProgramsCurrentPastEndDate', code: 2502, title: 'Invalid Date for Work Program Status Type', message: "A Current work program's end date can not be in the past." },
  { enum: 'WorkHistoryWorkBarriersRequired', code: 2503, title: 'Required information not provided', message: 'Are there any work barriers for this participant?' },
  { enum: 'WorkHistoryNoEmployments', code: 2504, title: 'Required information not provided', message: 'Currently have no jobs listed in work history.' },

  { enum: 'ChildCareChildDobInFuture', code: 2601, title: 'Invalid Date for Child', message: "A child's date of birth cannot be in the future." },
  { enum: 'ChildCareChildDobOver12', code: 2602, title: 'Invalid Date for Child', message: "The child's date of birth makes them over 12." },
  { enum: 'ChildCareChildDobUnder13', code: 2603, title: 'Invalid Date for Child', message: "The child's date of birth makes them under 13." },
  { enum: 'ChildCareChildDobOver18', code: 2604, title: 'Invalid Date for Child', message: "The child's date of birth makes them over 18." },

  {
    enum: 'DatesOutOfOrder_Date1_Date2',
    code: 900,
    title: 'Dates Out of Order',
    message: 'You have provided a beginning date, {0}, that is after the ending date, {1}. Please enter a starting date that is before the ending date.'
  },
  { enum: 'AnswerRequired', code: 901, title: 'Answer Required', message: 'You have not indicated {0}. Please select an answer to this question.' },

  {
    enum: 'HistoryTableInvalid',
    code: 2702,
    title: 'Invalid History Record',
    message: 'A record in the history table is invalid. Please make sure all dates are in proper formats and in a valid date range. Invalid records will not be saved.'
  },
  {
    enum: 'HistoryTableConfirm',
    code: 2703,
    title: 'Open Record in History Table',
    message:
      'A record in the history table has been left open. Please confirm any changes made to this record before continuing. Uncommitted changes to a history record will not be saved.'
  },
  { enum: 'HistoryTableDuplicate', code: 2704, title: 'Invalid History Record', message: 'Please make sure there are no duplicate {0}.' },

  {
    enum: 'ValueInInvalidFormat',
    code: 2801,
    title: 'Incorrect Format',
    message: 'You have provided {0} in the wrong format. The field requires inputs following the {1} pattern.'
  },
  { enum: 'SsnValidSubset', code: 2802, title: 'Incorrect Format', message: 'You have provided {0} in the wrong format. The {1} portion of the SSN cannot be {2}.' },
  { enum: 'InvalidCharAtIndex', code: 2803, title: 'Incorrect Format', message: "You have provided {0} in the wrong format. The '{1}' character cannot be {2}." },
  {
    enum: 'DisenrollmentPrecheckActivityError',
    code: 3000,
    title: 'Invalid Date Provided',
    message: 'Disenrollment Date cannot be prior to the end date of an assigned activity({0}). Please review activity end dates or enter a different Disenrollment Date.'
  },
  { enum: 'DisenrollmentPrecheckEnrollmentError', code: 3001, title: 'Invalid Date Provided', message: 'Disenrollment Date must be on or after the enrollment date.' },
  { enum: 'Ep', code: 4000, title: 'Unable to Save', message: '{0}' }
];
