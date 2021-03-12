export const enum ValidationCode {
  RequiredInformationMissing = 101,
  RequiredInformationMissing_Details = 102,
  InformationIncorrect = 199,
  ValueOutOfRange_Details = 103,
  ValueInInvalidFormat_Name_FormatType = 104,
  ValueBeforeDOB_Name_Value_DOB = 105,
  DateBeforeDate_Name1_Name2 = 106,
  ValueBefore_X_PlusDOB_Name_Value_DOB = 107,
  InvalidDate = 108,
  DateAfterDate_Name1_Name2 = 109,
  RequiredInformationMissing2 = 118,
  ValuesExceedRange = 123,
  DateAfterCurrent = 110,
  DateBeforeCurrent = 111,
  DateAfterDuration = 112,
  ValueSameValue_Name1_Name2 = 113,
  DateBeforeProgramEnrollmentDate = 150,
  DateBeforeMaxDaysCanBackDate = 151,
  ParticipationPeriodCheck = 152,
  DateBeforeMinSimulatedDate = 160,
  DateAfterMaxSimulatedDate = 161,

  InvalidText = 119,

  InvalidChar = 120,
  DuplicateData = 121,
  DuplicateDataWithNoMessage = 122,

  DateBeforeEnrollmentDate = 114,
  DateAfterDisenrollmentDate = 115,
  DateBeforeDisenrollmentDate = 116,
  DateSameAsEpBeginDate = 117,

  LanguageInfoIncomplete = 2000,
  LanguageEnglishInfoIncomplete = 2001,
  LanguageMustBeUnique = 2002,

  EducationHistoryLastYearAttendedInvalid_ParticipantDob = 2101,
  EducationHistoryLastYearAttendedInFuture = 2102,
  EducationHistoryYearAwardedAfterCurrentYear_GedOrHsed = 2103,
  EducationHistoryYearAwardedPastForGedOrHsed = 2106,
  EducationHistoryLastYearAttendedpriorDobInvalid = 2107,
  EducationHistoryPastLastYear = 2108,
  EducationHistoryPastSchoolAttendanceRequired = 2109,
  EucationHistoryCurrentEnrolledRequired = 2110,

  WorkProgramsPastStatus_Name = 2500,
  WorkProgramsCurrentFutureStartDate = 2501,
  WorkProgramsCurrentPastEndDate = 2502,
  WorkHistoryWorkBarriersRequired = 2503,
  WorkHistoryNoEmployments = 2504,

  ChildCareChildDobInFuture = 2601,
  ChildCareChildDobOver12 = 2602,
  ChildCareChildDobUnder13 = 2603,
  ChildCareChildDobOver18 = 2604,

  DatesOutOfOrder_Date1_Date2 = 900,
  AnswerRequired = 901,

  HistoryTableIncomplete = 2701,
  HistoryTableInvalid = 2702,
  HistoryTableConfirm = 2703,
  HistoryTableDuplicate = 2704,
  ValueInInvalidFormat = 2801,
  SsnValidSubset = 2802,
  InvalidCharAtIndex = 2803,
  DisenrollmentPrecheckActivityError = 3000,
  DisenrollmentPrecheckEnrollmentError = 3001,

  EP = 4000
}

export class ValidationError {
  enum: string;
  code: number;
  title: string;
  message: string;
  formatted: string;
  detailItems: string[] = [];
}
