// tslint:disable: deprecation
// tslint:disable: no-unused-expression
import { SystemClockService } from '../../shared/services/system-clock.service';
import { LogService } from '../../shared/services/log.service';
import { Router, NavigationEnd } from '@angular/router';
import * as TypeMoq from 'typemoq';
import * as moment from 'moment';
import { AppService } from './../../core/services/app.service';
import { Employment, WageHour, Absence, WorkHistoryIdentities } from '../../shared/models/work-history-app';
import { GoogleLocation } from '../../shared/models/google-location';
import { JobActionType } from '../../shared/models/job-actions';
import { ValidationManager } from '../../shared/models/validation-manager';
import { ValidationCode } from '../../shared/models/validation-error';
import { TestingUtilities } from './testing-utilities';
import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
import { Observable, empty } from 'rxjs';
import { JwtAuthConfig } from 'src/app/core/jwt-auth-config';
import { Location } from '@angular/common';

describe('WorkHistoryEditComponent: Validate', () => {
  const tempCustodialParentUnsubsidizedId = 1;
  const tempNonCustodialParentUnsubsidizedId = 2;
  const tmjUnsubsidizedId = 3;
  const tjUnsubsidizedId = 4;
  const volunteerId = 5;
  const tjSubsidizedId = 6;
  const tmjSunsidizedId = 7;
  const tempCustodialParentsubsidizedId = 8;
  const tempNonCustodialParentsubsidizedId = 9;

  const router = TypeMoq.Mock.ofType<Router>();
  const logServiceMoq = TypeMoq.Mock.ofType<LogService>();
  const jwtMoq = TypeMoq.Mock.ofType<JwtAuthConfig>();
  const location = TypeMoq.Mock.ofType<Location>();

  router.setup(x => x.events).returns(() => empty());

  let appService: AppService;
  const authHttpClientMock: TypeMoq.IMock<AuthHttpClient> = TypeMoq.Mock.ofType<AuthHttpClient>();
  let validationManager: ValidationManager;
  const wageHour = new WageHour();
  const model: Employment[] = [];
  const employment = new Employment();

  model.push(employment);
  model[0].wageHour = wageHour;

  beforeEach(() => {
    appService = new AppService(authHttpClientMock.object, logServiceMoq.object, router.object, jwtMoq.object, location.object);

    validationManager = new ValidationManager(appService);
  });

  it('Basic - should create an instance', () => {
    expect(model).toBeTruthy();
  });

  /**
   * Tests for Is Street Address Required based on Job Type.
   */
  // it('US944 - Is Street Address required when Custodial Parent (Unsubsidized) Job Type is selected. - True', () => {
  //   model[0].jobTypeId = tempCustodialParentUnsubsidizedId;
  //   let result = model[0].isAddressRequired(tempCustodialParentUnsubsidizedId, tempNonCustodialParentUnsubsidizedId, tmjUnsubsidizedId, tjUnsubsidizedId);
  //   expect(result).toBe(true);
  // });

  // it('US944 - Is Street Address required when Non-Custodial Parent (Unsubsidized) Job Type is selected. - True', () => {
  //   model[0].jobTypeId = tempNonCustodialParentUnsubsidizedId;
  //   let result = model[0].isAddressRequired(tempCustodialParentUnsubsidizedId, tempNonCustodialParentUnsubsidizedId, tmjUnsubsidizedId, tjUnsubsidizedId);
  //   expect(result).toBe(true);
  // });

  // // Negative Test.
  // it('US944 - Is Street Address required when tmjUnsubsidizedId Job Type is selected. - false', () => {
  //   model[0].jobTypeId = volunteerId;
  //   let result = model[0].isAddressRequired(tempCustodialParentUnsubsidizedId, tempNonCustodialParentUnsubsidizedId, tmjUnsubsidizedId, tjUnsubsidizedId);
  //   expect(result).toEqual(false);
  // });

  // // Negative Test.
  // it('US944 - Is Street Address required when no(null) Job Type is selected. - false', () => {
  //   model[0].jobTypeId = null;
  //   let result = model[0].isAddressRequired(tempCustodialParentUnsubsidizedId, tempNonCustodialParentUnsubsidizedId, tmjUnsubsidizedId, tjUnsubsidizedId);
  //   expect(result).toBe(false);
  // });

  /**
   * Tests for Is Zip Code Required based on Job Type.
   */
  it('US944 - Is Zip Code required when TEMP Custodial Parent (Unsubsidized) Job Type is selected. - true', () => {
    model[0].jobTypeId = tempCustodialParentUnsubsidizedId;
    const result = model[0].isZipCodeRequired(
      tempCustodialParentUnsubsidizedId,
      tempNonCustodialParentUnsubsidizedId,
      tmjUnsubsidizedId,
      tjUnsubsidizedId,
      tempCustodialParentsubsidizedId,
      tempNonCustodialParentsubsidizedId,
      tmjSunsidizedId,
      tjSubsidizedId
    );
    expect(result).toBe(true);
  });

  it('US944 - Is Zip Code required when TEMP Non-Custodial Parent (Unsubsidized) Job Type is selected. - true', () => {
    model[0].jobTypeId = tempNonCustodialParentUnsubsidizedId;
    const result = model[0].isZipCodeRequired(
      tempCustodialParentUnsubsidizedId,
      tempNonCustodialParentUnsubsidizedId,
      tmjUnsubsidizedId,
      tjUnsubsidizedId,
      tempCustodialParentsubsidizedId,
      tempNonCustodialParentsubsidizedId,
      tmjSunsidizedId,
      tjSubsidizedId
    );
    expect(result).toBe(true);
  });

  it('US944 - Is Zip Code required when TMJ (Unsubsidized) Job Type is selected. - true', () => {
    model[0].jobTypeId = tempCustodialParentUnsubsidizedId;
    const result = model[0].isZipCodeRequired(
      tempCustodialParentUnsubsidizedId,
      tempNonCustodialParentUnsubsidizedId,
      tmjUnsubsidizedId,
      tjUnsubsidizedId,
      tempCustodialParentsubsidizedId,
      tempNonCustodialParentsubsidizedId,
      tmjSunsidizedId,
      tjSubsidizedId
    );
    expect(result).toBe(true);
  });

  it('US944 - Is Zip Code required when TJ (Unsubsidized) Job Type is selected. - true', () => {
    model[0].jobTypeId = tempCustodialParentUnsubsidizedId;
    const result = model[0].isZipCodeRequired(
      tempCustodialParentUnsubsidizedId,
      tempNonCustodialParentUnsubsidizedId,
      tmjUnsubsidizedId,
      tjUnsubsidizedId,
      tempCustodialParentsubsidizedId,
      tempNonCustodialParentsubsidizedId,
      tmjSunsidizedId,
      tjSubsidizedId
    );
    expect(result).toBe(true);
  });

  // Negative Test.
  it('US944 - Is Zip Code required when no Job Type is selected. - false', () => {
    model[0].jobTypeId = null;
    const result = model[0].isZipCodeRequired(
      tempCustodialParentUnsubsidizedId,
      tempNonCustodialParentUnsubsidizedId,
      tmjUnsubsidizedId,
      tjUnsubsidizedId,
      tempCustodialParentsubsidizedId,
      tempNonCustodialParentsubsidizedId,
      tmjSunsidizedId,
      tjSubsidizedId
    );
    expect(result).toBe(false);
  });

  /**
   * Tests for determining job category.
   * Assumptions: Enrollment Date is always supplied.
   */
  it('US944 - What is Job Category when begin date null and end date null  - null', () => {
    model[0].jobBeginDate = null;
    model[0].isCurrentlyEmployed = null;
    model[0].jobEndDate = null;
    const result = model[0].whichJobCategory();
    expect(result).toBeNull;
  });

  it('US944 - What is Job Category when begin date is before enrollment date and end date is before enrollment date. - pastJob', () => {
    model[0].jobBeginDate = '12/10/2015';
    model[0].isCurrentlyEmployed = null;
    model[0].jobEndDate = '06/10/2016';
    const result = model[0].whichJobCategory();
    expect(result).toBe('pastJob');
  });

  it('US944 - What is Job Category when is currently Employed box is checked - currentJob', () => {
    model[0].isCurrentlyEmployed = true;
    const result = model[0].whichJobCategory();
    expect(result).toBe('currentJob');
  });

  it('US944 - What is Job Category when before date is after end date. - null', () => {
    model[0].jobBeginDate = '12/10/2017';
    model[0].isCurrentlyEmployed = null;
    model[0].jobEndDate = '06/10/2016';
    const result = model[0].whichJobCategory();
    expect(result).toBeNull;
  });

  it('US944 - What is Job Category when begin date is before the enroll date and end date is before the enroll date . - pastJob', () => {
    model[0].jobBeginDate = '01/01/2017';
    model[0].isCurrentlyEmployed = null;
    model[0].jobEndDate = '01/31/2017';
    const result = model[0].whichJobCategory();
    expect(result).toBe('pastJob');
  });

  it('US944 - What is Job Category when begin date and end date are the same and both are before the enroll date . - pastJob', () => {
    model[0].jobBeginDate = '01/15/2017';
    model[0].isCurrentlyEmployed = null;
    model[0].jobEndDate = '01/15/2017';
    const result = model[0].whichJobCategory();
    expect(result).toBe('pastJob');
  });

  it('US944 - What is Job Category when begin date and end date  are null. - null', () => {
    model[0].jobBeginDate = null;
    model[0].isCurrentlyEmployed = null;
    model[0].jobEndDate = null;
    const result = model[0].whichJobCategory();
    expect(result).toBeNull;
  });

  it('US944 - What is Job Category when begin date has invalid month. - null', () => {
    model[0].jobBeginDate = '13/12/2016';
    model[0].isCurrentlyEmployed = null;
    model[0].jobEndDate = '12/31/2016';
    const result = model[0].whichJobCategory();
    expect(result).toBeNull;
  });

  it('US944 - What is Job Category when begin date has invalid day. - null', () => {
    model[0].jobBeginDate = '06/31/2016';
    model[0].isCurrentlyEmployed = null;
    model[0].jobEndDate = '12/31/2016';
    const result = model[0].whichJobCategory();
    expect(result).toBeNull;
  });

  it('US944 - What is Job Category when end date has invalid month. - null', () => {
    model[0].jobBeginDate = '06/12/2016';
    model[0].isCurrentlyEmployed = null;
    model[0].jobEndDate = '13/31/2016';
    const result = model[0].whichJobCategory();
    expect(result).toBeNull;
  });

  it('US944 - What is Job Category when end date has invalid day. - null', () => {
    model[0].jobBeginDate = '06/15/2016';
    model[0].isCurrentlyEmployed = null;
    model[0].jobEndDate = '11/31/2016';
    const result = model[0].whichJobCategory();
    expect(result).toBeNull;
  });

  it('US979 - What is Hourly Wage when pay rate is 8 and Pay rate name is day. - 8.00/day', () => {
    model[0].wageHour.currentPayRate = '8';
    model[0].wageHour.currentPayRateIntervalName = 'Day';
    const result = model[0].wageHour.calculateHourlyWage();
    expect(result.value + '/' + result.units).toEqual('8.00/Day');
  });

  it('US979 - What is Hourly Wage when pay rate is 200 and Pay rate name is day. - 200.00/day', () => {
    model[0].wageHour.currentPayRate = '200';
    model[0].wageHour.currentPayRateIntervalName = 'Day';
    const result = model[0].wageHour.calculateHourlyWage();

    expect(result.value + '/' + result.units).toEqual('200.00/Day');
  });

  it('US979 - What is Hourly Wage when pay rate is 500 and Pay rate name is week. - 12.50/Hour', () => {
    model[0].wageHour.currentPayRate = '500';
    model[0].wageHour.currentPayRateIntervalName = 'Week';
    model[0].wageHour.currentAverageWeeklyHours = '40';
    const result = model[0].wageHour.calculateHourlyWage();
    expect(result.value + '/' + result.units).toEqual('12.50/Hour');
  });

  it('US979 - What is Hourly Wage when pay rate is 1500 and Pay rate name is week. - 37.50/Hour', () => {
    model[0].wageHour.currentPayRate = '1500';
    model[0].wageHour.currentPayRateIntervalName = 'Week';
    model[0].wageHour.currentAverageWeeklyHours = '40';
    const result = model[0].wageHour.calculateHourlyWage();
    expect(result.value + '/' + result.units).toEqual('37.50/Hour');
  });

  it('US979 - What is Hourly Wage when pay rate is 1000 and Pay rate name is semi-monthly. - 12.50/Hour', () => {
    model[0].wageHour.currentPayRate = '1000';
    model[0].wageHour.currentPayRateIntervalName = 'semi-monthly';
    model[0].wageHour.currentAverageWeeklyHours = '40';
    const result = model[0].wageHour.calculateHourlyWage();
    expect(result.value + '/' + result.units).toEqual('11.63/Hour');
  });

  it('US979 - What is Hourly Wage when pay rate is 1500 and Pay rate name is semi-monthly. - 17.44/Hour', () => {
    model[0].wageHour.currentPayRate = '1500';
    model[0].wageHour.currentPayRateIntervalName = 'semi-monthly';
    model[0].wageHour.currentAverageWeeklyHours = '40';
    const result = model[0].wageHour.calculateHourlyWage();
    expect(result.value + '/' + result.units).toEqual('17.44/Hour');
  });

  it('US979 - What is Hourly Wage when pay rate is 1075 and Pay rate name is bi-weekly. - 13.44/Hour', () => {
    model[0].wageHour.currentPayRate = '1075';
    model[0].wageHour.currentPayRateIntervalName = 'bi-weekly';
    model[0].wageHour.currentAverageWeeklyHours = '40';
    const result = model[0].wageHour.calculateHourlyWage();
    expect(result.value + '/' + result.units).toEqual('13.44/Hour');
  });

  it('US979 - What is Hourly Wage when pay rate is 1500 and Pay rate name is bi-weekly. - 18.75/Hour', () => {
    model[0].wageHour.currentPayRate = '1500';
    model[0].wageHour.currentPayRateIntervalName = 'bi-weekly';
    model[0].wageHour.currentAverageWeeklyHours = '40';
    const result = model[0].wageHour.calculateHourlyWage();
    expect(result.value + '/' + result.units).toEqual('18.75/Hour');
  });

  it('US979 - What is Hourly Wage when pay rate is 2150 and Pay rate name is monthly. - 12.50/Hour', () => {
    model[0].wageHour.currentPayRate = '2150';
    model[0].wageHour.currentPayRateIntervalName = 'monthly';
    model[0].wageHour.currentAverageWeeklyHours = '40';
    const result = model[0].wageHour.calculateHourlyWage();
    expect(result.value + '/' + result.units).toEqual('12.50/Hour');
  });

  it('US979 - What is Hourly Wage when pay rate is 1500 and Pay rate name is monthly. - 8.72/Hour', () => {
    model[0].wageHour.currentPayRate = '1500';
    model[0].wageHour.currentPayRateIntervalName = 'monthly';
    model[0].wageHour.currentAverageWeeklyHours = '40';
    const result = model[0].wageHour.calculateHourlyWage();
    expect(result.value + '/' + result.units).toEqual('8.72/Hour');
  });

  it('US979 - What is Hourly Wage when pay rate is 2150 and Pay rate name is irregular. - 2150.00/irregular', () => {
    model[0].wageHour.currentPayRate = '2150';
    model[0].wageHour.currentPayRateIntervalName = 'irregular';
    const result = model[0].wageHour.calculateHourlyWage();
    expect(result.value + '/' + result.units).toEqual('2150.00/irregular');
  });

  it('US979 - What is Hourly Wage when pay rate is 0 and Pay rate name is irregular. - 0.00/irregular', () => {
    model[0].wageHour.currentPayRate = '0';
    model[0].wageHour.currentPayRateIntervalName = 'irregular';
    const result = model[0].wageHour.calculateHourlyWage();
    expect(result.value + '/' + result.units).toEqual('0.00/irregular');
  });

  it('US979 - What is Hourly Wage when pay rate is null and Pay rate name is monthly. - null', () => {
    model[0].wageHour.currentPayRate = '';
    model[0].wageHour.currentPayRateIntervalName = 'monthly';
    const result = model[0].wageHour.calculateHourlyWage();
    expect(result).toEqual(null);
  });

  it('US979 - What is Hourly Wage when pay rate is 1000 and Pay rate name is null. - null', () => {
    model[0].wageHour.currentPayRate = '1000';
    model[0].wageHour.currentPayRateIntervalName = ' ';
    const result = model[0].wageHour.calculateHourlyWage();
    expect(result).toEqual(null);
  });

  // The following tests were requested by palani.
  // Colar Shuntae.

  it('2', () => {
    model[0].jobBeginDate = '01/01/2016';
    model[0].isCurrentlyEmployed = true;
    model[0].jobEndDate = null;
    const result = model[0].whichJobCategory();
    expect(result).toBe('currentJob');
  });
});

/**
 * Tests for WageHour object.
 */

describe('WorkHistory-WageHour: Requires', () => {
  const noPayTypeId = 5;
  const otherPayTypeId = 6;

  const ne = new NavigationEnd(0, 'http://localhost:4200/login', 'http://localhost:4200/login');
  const events = new Observable(observer => {
    observer.next(this.ne);
    observer.complete();
  });

  const router = TypeMoq.Mock.ofType<Router>();

  router.setup(x => x.events).returns(() => empty());
  const logServiceMoq = TypeMoq.Mock.ofType<LogService>();
  const jwtMoq = TypeMoq.Mock.ofType<JwtAuthConfig>();
  const location = TypeMoq.Mock.ofType<Location>();

  let appService: AppService;
  const authHttpClientMock: TypeMoq.IMock<AuthHttpClient> = TypeMoq.Mock.ofType<AuthHttpClient>();
  let validationManager: ValidationManager;
  let model: WageHour;

  beforeEach(() => {
    appService = new AppService(authHttpClientMock.object, logServiceMoq.object, router.object, jwtMoq.object, location.object);

    validationManager = new ValidationManager(appService);
    const wh = new WageHour();
    const jct = new JobActionType();
    wh.currentPayType = jct;
    wh.currentPayType.jobActionTypes = [];
    wh.wageHourHistories = [];
    model = wh;
  });

  it('Basic - should create an instance', () => {
    expect(model).toBeTruthy();
  });

  it('US944 - Is currentPayTypeDetails required when "Other" pay type is selected. - true', () => {
    model.currentPayType.jobActionTypes.push(otherPayTypeId);
    const result = model.isCurrentPayTypeDetailsRequired(otherPayTypeId);
    expect(result).toEqual(true);
    expect(model.currentPayType.jobActionTypes.length).toEqual(1);
  });

  it('US944 - Is currentPayTypeDetails required when "Null" pay type is selected. - false', () => {
    model.currentPayType.jobActionTypes.push(null);
    const result = model.isCurrentPayTypeDetailsRequired(otherPayTypeId);
    expect(result).toEqual(false);
    expect(model.currentPayType.jobActionTypes.length).toEqual(1);
  });

  it('US944 - Is currentPayTypeDetails required when "No Pay" pay type is selected. - false', () => {
    model.currentPayType.jobActionTypes.push(noPayTypeId);
    const result = model.isCurrentPayTypeDetailsRequired(otherPayTypeId);
    expect(result).toEqual(false);
    expect(model.currentPayType.jobActionTypes.length).toEqual(1);
  });

  it('US944 - Is currentPayTypeDetails required when "No Pay" pay type is selected. - false', () => {
    model.currentPayType.jobActionTypes.push(noPayTypeId);
    const result = model.isCurrentPayTypeDetailsRequired(otherPayTypeId);
    expect(result).toEqual(false);
    expect(model.currentPayType.jobActionTypes.length).toEqual(1);
  });

  // it('US945 - When Other Pay Type is selected test for required fields', () => {
  //     // Arrange: Setting Pay Type to include Other.
  //     model.currentPayType.jobActionTypes.push(otherPayTypeId);

  //     let result = model.validate(validationManager, participantDOB, jobBeginDate, jobEndDate, programStatus, otherPayTypeId, isCurrentlyEmployed);

  //     expect(result.isValid).toBeFalsy();
  //
  //     let errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

  //     // Assert: Effective Date, Pay Rate, Pay Rate Interval, Average Weekly Hours and Pay Type Details are required.
  //     expect(result.isValid).toBeFalsy();

  //     expect(errorDetailMsgs.indexOf('Effective Date')).toBeGreaterThan(-1);
  //     expect(errorDetailMsgs.indexOf('Pay Rate')).toBeGreaterThan(-1);
  //     expect(errorDetailMsgs.indexOf('Pay Rate Interval')).toBeGreaterThan(-1);
  //     expect(errorDetailMsgs.indexOf('Average Weekly Hours')).toBeGreaterThan(-1);
  //     expect(errorDetailMsgs.indexOf('Pay Type Details')).toBeGreaterThan(-1);
  //     expect(errorDetailMsgs.length).toEqual(5);
  // });
});

describe('WorkHistory-Absence: Requires', () => {
  const router = TypeMoq.Mock.ofType<Router>();

  router.setup(x => x.events).returns(() => empty());
  const logServiceMoq = TypeMoq.Mock.ofType<LogService>();
  const jwtMoq = TypeMoq.Mock.ofType<JwtAuthConfig>();
  const location = TypeMoq.Mock.ofType<Location>();

  let appService: AppService;
  const authHttpClientMock: TypeMoq.IMock<AuthHttpClient> = TypeMoq.Mock.ofType<AuthHttpClient>();
  let validationManager: ValidationManager;
  let model: Absence;
  const participantDOB: moment.Moment = moment('03/29/1991', 'MM/DD/YYYY');
  const jobBeginDate: moment.Moment = moment('03/29/1992', 'MM/DD/YYYY');
  const jobEndDate: moment.Moment = moment('04/05/1993', 'MM/DD/YYYY');
  const isCurrentlyEmployed = null;
  const otherAbsenceTypeId = null;
  const absences = null;

  beforeEach(() => {
    appService = new AppService(authHttpClientMock.object, logServiceMoq.object, router.object, jwtMoq.object, location.object);
    validationManager = new ValidationManager(appService);
    const ab = new Absence();
    model = ab;
  });

  it('Basic - should create an instance', () => {
    expect(model).toBeTruthy();
  });

  it('US951 - When Start Date, End Date or Reason are blank - object is invalid', () => {
    model.beginDate = null;
    model.endDate = null;
    model.absenceReasonId = null;
    const result = model.validate(validationManager, participantDOB, jobBeginDate, jobEndDate, isCurrentlyEmployed, otherAbsenceTypeId, absences);
    expect(result.isValid).toBeFalsy();
  });

  it('US951 - When Start Date, End Date or Reason are set correctly - object is valid', () => {
    model.beginDate = '04/01/1993';
    model.endDate = '04/05/1993';
    model.absenceReasonId = 2;
    const result = model.validate(validationManager, participantDOB, jobBeginDate, jobEndDate, isCurrentlyEmployed, otherAbsenceTypeId, absences);
    expect(result.isValid).toBeTruthy();
  });

  it('US951 - When Start Date is before Job Start Date, End Date and Reason are set correctly - object is invalid', () => {
    model.beginDate = '03/28/1992';
    model.endDate = '04/05/1993';
    model.absenceReasonId = 2;
    const result = model.validate(validationManager, participantDOB, jobBeginDate, jobEndDate, isCurrentlyEmployed, otherAbsenceTypeId, absences);
    expect(result.isValid).toBeFalsy();
  });

  it('US951 - When Start Date is the same as Job Start Date, End Date and Reason are set correctly - object is valid', () => {
    model.beginDate = '03/29/1992';
    model.endDate = '04/05/1993';
    model.absenceReasonId = 2;
    const result = model.validate(validationManager, participantDOB, jobBeginDate, jobEndDate, isCurrentlyEmployed, otherAbsenceTypeId, absences);
    expect(result.isValid).toBeTruthy();
  });

  it('US951 - When Start Date is after End Date but Reason is set correctly - object is invalid', () => {
    model.beginDate = '04/02/1993';
    model.endDate = '04/01/1993';
    model.absenceReasonId = 2;
    const result = model.validate(validationManager, participantDOB, jobBeginDate, jobEndDate, isCurrentlyEmployed, otherAbsenceTypeId, absences);
    expect(result.isValid).toBeFalsy();
  });

  it('US951 - When Start Date is after End Date but Reason is set correctly - object is invalid', () => {
    model.beginDate = '02/15/2016';
    model.endDate = '04/12/2016';
    model.absenceReasonId = 2;
    const result = model.validate(validationManager, participantDOB, jobBeginDate, jobEndDate, isCurrentlyEmployed, otherAbsenceTypeId, absences);
    expect(result.isValid).toBeFalsy();
  });

  it('US951 - When Start Date is after End Date but Reason is set correctly - object is invalid', () => {
    model.beginDate = '02/15/2016';
    model.endDate = '02/12/2016';
    model.absenceReasonId = 2;
    const jobBeginDateTest: moment.Moment = moment('02/10/2016', 'MM/DD/YYYY');
    const result = model.validate(validationManager, participantDOB, jobBeginDateTest, jobEndDate, true, otherAbsenceTypeId, absences);
    expect(result.isValid).toEqual(false);
  });
});

describe('WorkHistory-Employment: Validate', () => {
  const ne = new NavigationEnd(0, 'http://localhost:4200/login', 'http://localhost:4200/login');
  const events = new Observable(observer => {
    observer.next(this.ne);
    observer.complete();
  });

  const router = TypeMoq.Mock.ofType<Router>();

  router.setup(x => x.events).returns(() => empty());
  const logServiceMoq = TypeMoq.Mock.ofType<LogService>();
  const jwtMoq = TypeMoq.Mock.ofType<JwtAuthConfig>();
  const location = TypeMoq.Mock.ofType<Location>();

  let appService: AppService;
  const authHttpClientMock: TypeMoq.IMock<AuthHttpClient> = TypeMoq.Mock.ofType<AuthHttpClient>();
  let validationManager: ValidationManager;
  let model: Employment;
  const participantDOB: moment.Moment = moment('03/29/1991', 'MM/DD/YYYY');
  const enrollmentDate = '03/29/1992';
  const disenrollmentDate = '04/05/1993';
  const enrolledPrograms = [
    {
      id: 11596,
      enrolledProgramId: 1,
      rfaNumber: null,
      programCode: 'TMJ',
      programCd: 'TMJ',
      subProgramCode: 'C',
      enrollmentDate: '2014-08-06T00:00:00-05:00',
      disenrollmentDate: null,
      referralDate: '2014-04-20T00:00:00-05:00',
      status: 'Enrolled',
      isTransfer: null,
      statusDate: null,
      canDisenroll: null,
      officeCounty: 'MILWAUKEE           ',
      agencyCode: 'RS',
      agencyName: 'Ross',
      officeId: 1,
      officeNumber: 1581,
      assignedWorker: null,
      cfCourtOrderedDate: null,
      cfCourtOrderedCounty: null,
      completionReasonId: null,
      contractorId: 1,
      contractorName: 'Ross',
      learnFareFEP: null
    }
  ];

  const subsidizedId = 11;
  const unSubsidizedId = 12;
  const volunteerId = 13;
  const workExperienceId = 14;
  const internshipId = 15;
  const externshipId = 16;
  const selfEmployedId = 17;

  const modelIds = new WorkHistoryIdentities();
  modelIds.jobFoundOtherWorkProgramId = 6;
  modelIds.jobFoundWorkerAssistedId = 5;
  modelIds.tempNonCustodialParentUnsubsidizedId = 2;
  modelIds.tempCustodialParentUnsubsidizedId = 1;
  modelIds.tmjUnsubsidizedId = 3;
  modelIds.tjUnsubsidizedId = 4;
  modelIds.volunteerJobTypeId = volunteerId;
  modelIds.otherPayTypeId = 7;
  modelIds.leavingReasonFiredId = 8;
  modelIds.leavingReasonPermanentlyLaidOffId = 9;
  modelIds.leavingReasonQuitId = 10;
  modelIds.leavingReasonOtherId = 111;

  beforeEach(() => {
    appService = new AppService(authHttpClientMock.object, logServiceMoq.object, router.object, jwtMoq.object, location.object);

    validationManager = new ValidationManager(appService);
    const ab = new Employment();
    const l = new GoogleLocation();
    ab.location = l;
    model = ab;
    const wh = new WageHour();
    const act = new JobActionType();
    ab.jobDuties = [];
    wh.currentPayType = act;
    model.wageHour = wh;
    model.wageHour.wageHourHistories = [];
    model.absences = [];
  });

  it('Basic - should create an instance', () => {
    expect(model).toBeTruthy();
  });

  it('US943 - When tempCustodialParentUnsubsidizedId is selected test for required fields', () => {
    // Given: Model to tempCustodialParentUnsubsidizedId.
    model.jobTypeId = 1;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, enrollmentDate, disenrollmentDate, modelIds, this.enrolledPrograms);

    expect(result.isValid).toBeFalsy();

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Begin Date, End Date, Job Position, Company/Organization Name, Location, Street Address, Zip, Private/Public, Reason for Leaving are required.
    expect(result.isValid).toBeFalsy();

    expect(errorDetailMsgs.indexOf('Job Type')).toBe(-1);

    expect(errorDetailMsgs.indexOf('Begin Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('End Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Position')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Company/Organization Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Street Address')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Zip')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Duties')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Private/Public')).toBeGreaterThan(-1);

    expect(errorDetailMsgs.length).toEqual(9);
  });

  it('US943 - When tempCustodialParentUnsubsidizedId and currently employed is selected test for required fields', () => {
    // Given: Model to tempCustodialParentUnsubsidizedId.
    model.jobTypeId = 1;
    model.isCurrentlyEmployed = true;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, enrollmentDate, disenrollmentDate, modelIds, this.enrolledPrograms);

    expect(result.isValid).toBeFalsy();

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Begin Date, Job Position, Company/Organization Name, Location, Street Address, Zip, Private/Public, Reason for Leaving are required.
    expect(result.isValid).toBeFalsy();

    expect(errorDetailMsgs.indexOf('Job Type')).toBe(-1);
    expect(errorDetailMsgs.indexOf('End Date')).toBe(-1);

    expect(errorDetailMsgs.indexOf('Begin Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Position')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Company/Organization Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Street Address')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Zip')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Private/Public')).toBeGreaterThan(-1);
    // expect(errorDetailMsgs.indexOf('Reason for Leaving')).toBeGreaterThan(-1);
    // expect(errorDetailMsgs.length).toEqual(15);
  });

  it('US943 - When tempCustodialParentUnsubsidizedId is selected test for required fields', () => {
    // Given: Model to tempCustodialParentUnsubsidizedId.
    model.jobTypeId = modelIds.tempCustodialParentUnsubsidizedId;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, enrollmentDate, disenrollmentDate, modelIds, this.enrolledPrograms);

    expect(result.isValid).toBeFalsy();

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Begin Date, End Date, Job Position, Company/Organization Name, Location, Street Address, Zip, Private/Public, Reason for Leaving are required.
    expect(result.isValid).toBeFalsy();

    expect(errorDetailMsgs.indexOf('Job Type')).toBe(-1);

    expect(errorDetailMsgs.indexOf('Begin Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('End Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Position')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Company/Organization Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Street Address')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Zip')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Private/Public')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Duties')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.length).toEqual(9);
  });

  it('US943 - When tmjUnsubsidizedId is selected test for required fields', () => {
    // Given: Model to tmjUnsubsidizedId.
    model.jobTypeId = modelIds.tmjUnsubsidizedId;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, enrollmentDate, disenrollmentDate, modelIds, this.enrolledPrograms);

    expect(result.isValid).toBeFalsy();

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Begin Date, End Date, Job Position, Company/Organization Name, Location, Zip, Private/Public, Reason, Located in TMI Area? for Leaving are required.
    expect(result.isValid).toBeFalsy();

    expect(errorDetailMsgs.indexOf('Job Type')).toBe(-1);

    expect(errorDetailMsgs.indexOf('Begin Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('End Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Position')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Company/Organization Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Street Address')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Zip')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Duties')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Private/Public')).toBeGreaterThan(-1);
    // Reason for leaving is only present for past jobs.

    expect(errorDetailMsgs.indexOf('Located in TMI Area?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.length).toEqual(10);
  });

  it('US943 - When tmjUnsubsidizedId is selected and job staus is "isCurrentJob" test for required fields', () => {
    // Given: Model to tmjUnsubsidizedId and dates set for isCurrentJob.
    model.jobTypeId = modelIds.tmjUnsubsidizedId;
    model.isCurrentlyEmployed = true;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, enrollmentDate, disenrollmentDate, modelIds, this.enrolledPrograms);

    expect(result.isValid).toBeFalsy();

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Job Position, Company/Organization Name, Location, Zip, Private/Public, Reason, Located in TMI Area?, Benefits Offered', How was job found?
    // Reason for Leaving, Effective Date, Average Weekly Hours, Pay Types and Pay Rate for Leaving are required.
    expect(result.isValid).toBeFalsy();

    expect(errorDetailMsgs.indexOf('Begin Date')).toBeGreaterThan(-1);

    expect(errorDetailMsgs.indexOf('Job Position')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Company/Organization Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Street Address')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Zip')).toBeGreaterThan(-1);

    expect(errorDetailMsgs.indexOf('Job Duties')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Private/Public')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Located in TMI Area?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Benefits Offered')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('How was job found?')).toBeGreaterThan(-1);

    expect(errorDetailMsgs.indexOf('Effective Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Average Weekly Hours')).toBeGreaterThan(-1);

    expect(errorDetailMsgs.indexOf('Pay Types')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Pay Rate')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Pay Rate Interval')).toBeGreaterThan(-1);

    expect(errorDetailMsgs.length).toEqual(16);
  });

  it('US943 - When tmjUnsubsidizedId is selected test for required fields', () => {
    // Given: An empty Work History section with job type set to TMJ (Unsubsidized).
    model.jobTypeId = modelIds.tmjUnsubsidizedId;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, enrollmentDate, disenrollmentDate, modelIds, this.enrolledPrograms);

    expect(result.isValid).toBeFalsy();

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Begin Date, End Date, Job Position, Company/Organization Name, Location, Street Address, Zip, Job Duties, Private/Public, Located in TMI Area? are required.
    expect(result.isValid).toBeFalsy();

    expect(errorDetailMsgs.indexOf('Job Type')).toBe(-1);

    expect(errorDetailMsgs.indexOf('Begin Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('End Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Position')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Company/Organization Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Street Address')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Zip')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Duties')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Private/Public')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Located in TMI Area?')).toBeGreaterThan(-1);

    expect(errorDetailMsgs.length).toEqual(10);
  });

  it('US943 - When subsidizedId is selected test for required fields', () => {
    // Given: An empty Work History section with job type set to Subsidized.
    model.jobTypeId = subsidizedId;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, enrollmentDate, disenrollmentDate, modelIds, this.enrolledPrograms);

    expect(result.isValid).toBeFalsy();

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Begin Date, End Date, Job Position, Company/Organization Name, Location, Job Duties, are required.
    expect(result.isValid).toBeFalsy();

    expect(errorDetailMsgs.indexOf('Job Type')).toBe(-1);

    expect(errorDetailMsgs.indexOf('Begin Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('End Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Position')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Company/Organization Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Duties')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.length).toEqual(6);
  });

  it('US943 - When subsidizedId and currently employed is selected test for required fields', () => {
    // Given: An empty Work History section with job type set to Subsidized and Currently Employed is checked.
    model.jobTypeId = subsidizedId;
    model.isCurrentlyEmployed = true;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, enrollmentDate, disenrollmentDate, modelIds, this.enrolledPrograms);

    expect(result.isValid).toBeFalsy();

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Begin Date, Job Position, Company/Organization Name, Location, Job Duties, Benefits Offered, How was job found?, Effective Date,
    //   Pay Types, Pay Rate, Pay Rate Interval and Average Weekly Hours are required.
    expect(result.isValid).toBeFalsy();

    expect(errorDetailMsgs.indexOf('Job Type')).toBe(-1);

    expect(errorDetailMsgs.indexOf('Begin Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Position')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Company/Organization Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Duties')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Benefits Offered')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('How was job found?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Effective Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Pay Types')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Pay Rate')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Pay Rate Interval')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Average Weekly Hours')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.length).toEqual(12);
  });

  it('US943 - When unSubsidizedId is selected test for required fields', () => {
    // Given: An empty Work History section with job type set to Unsubsidized.
    model.jobTypeId = unSubsidizedId;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, enrollmentDate, disenrollmentDate, modelIds, this.enrolledPrograms);

    expect(result.isValid).toBeFalsy();

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Begin Date, End Date, Job Position, Company/Organization Name, Location, Job Duties, are required.
    expect(result.isValid).toBeFalsy();

    expect(errorDetailMsgs.indexOf('Job Type')).toBe(-1);

    expect(errorDetailMsgs.indexOf('Begin Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('End Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Position')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Company/Organization Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Duties')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.length).toEqual(6);
  });

  it('US943 - When unSubsidizedId and currently employed is selected test for required fields', () => {
    // Given: An empty Work History section with job type set to Unsubsidized and Currently Employed is checked.
    model.jobTypeId = unSubsidizedId;
    model.isCurrentlyEmployed = true;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, enrollmentDate, disenrollmentDate, modelIds, this.enrolledPrograms);

    expect(result.isValid).toBeFalsy();

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Begin Date, End Date, Job Position, Company/Organization Name, Location, Job Duties, Benefits Offered, How was job found?, Effective Date,
    //  Pay Types, Pay Rate, Pay Rate Interval and Average Weekly Hours are required.
    expect(result.isValid).toBeFalsy();

    expect(errorDetailMsgs.indexOf('Job Type')).toBe(-1);

    expect(errorDetailMsgs.indexOf('Begin Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Position')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Company/Organization Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Duties')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Benefits Offered')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('How was job found?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Effective Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Pay Types')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Pay Rate')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Pay Rate Interval')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Average Weekly Hours')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.length).toEqual(12);
  });

  it('US943 - When volunteerId is selected test for required fields', () => {
    // Given: An empty Work History section with job type set to Volunteer..
    model.jobTypeId = volunteerId;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, enrollmentDate, disenrollmentDate, modelIds, this.enrolledPrograms);

    expect(result.isValid).toBeFalsy();

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Begin Date, End Date, Job Position, Company/Organization Name, Location, Job Duties, are required.
    expect(result.isValid).toBeFalsy();

    expect(errorDetailMsgs.indexOf('Job Type')).toBe(-1);

    expect(errorDetailMsgs.indexOf('Begin Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('End Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Position')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Company/Organization Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Duties')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.length).toEqual(6);
  });

  it('US943 - When volunteerId and currently employed is selected test for required fields', () => {
    // Given: An empty Work History section with job type set to Volunteer and Currently Employed is checked.
    model.jobTypeId = volunteerId;
    model.isCurrentlyEmployed = true;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, enrollmentDate, disenrollmentDate, modelIds, this.enrolledPrograms);

    expect(result.isValid).toBeFalsy();

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Begin Date, Job Position, Company/Organization Name, Location, Job Duties, Benefits Offered, How was job found?, Effective Date,
    //  and Average Weekly Hours are required.
    expect(result.isValid).toBeFalsy();

    expect(errorDetailMsgs.indexOf('Job Type')).toBe(-1);

    expect(errorDetailMsgs.indexOf('Begin Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Position')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Company/Organization Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Duties')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Benefits Offered')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('How was job found?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Effective Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Average Weekly Hours')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.length).toEqual(9);
  });

  it('US943 - When workExperienceId is selected test for required fields', () => {
    // Given: An empty Work History section with jobtype set to Work Experience.
    model.jobTypeId = workExperienceId;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, enrollmentDate, disenrollmentDate, modelIds, this.enrolledPrograms);

    expect(result.isValid).toBeFalsy();

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Begin Date, End Date, Job Position, Company/Organization Name, Location, Job Duties, are required.
    expect(result.isValid).toBeFalsy();

    expect(errorDetailMsgs.indexOf('Job Type')).toBe(-1);

    expect(errorDetailMsgs.indexOf('Begin Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('End Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Position')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Company/Organization Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Duties')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.length).toEqual(6);
  });

  it('US943 - When workExperienceId is selected and currently employed test for required fields', () => {
    // Given: An empty Work History section with jobtype set to Work Experience and Currently Employed is checked.
    model.jobTypeId = workExperienceId;
    model.isCurrentlyEmployed = true;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, enrollmentDate, disenrollmentDate, modelIds, this.enrolledPrograms);

    expect(result.isValid).toBeFalsy();

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Begin Date, Job Position, Company/Organization Name, Location, Job Duties, Benefits Offered, How was job found?, Effective Date, Pay Types,
    //  Pay Rate, Pay Rate Interval and Average Weekly Hours are required.
    expect(result.isValid).toBeFalsy();

    expect(errorDetailMsgs.indexOf('Job Type')).toBe(-1);

    expect(errorDetailMsgs.indexOf('Begin Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Position')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Company/Organization Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Duties')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Benefits Offered')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('How was job found?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Effective Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Pay Types')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Pay Rate')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Pay Rate Interval')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Average Weekly Hours')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.length).toEqual(12);
  });

  it('US943 - When internshipId is selected test for required fields', () => {
    // Given: An empty Work History section with jobtype set to Internship.
    model.jobTypeId = internshipId;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, enrollmentDate, disenrollmentDate, modelIds, this.enrolledPrograms);

    expect(result.isValid).toBeFalsy();

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Begin Date, End Date, Job Position, Company/Organization Name, Location, Job Duties, are required.
    expect(result.isValid).toBeFalsy();

    expect(errorDetailMsgs.indexOf('Job Type')).toBe(-1);

    expect(errorDetailMsgs.indexOf('Begin Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('End Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Position')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Company/Organization Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Duties')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.length).toEqual(6);
  });

  it('US943 - When internshipId is selected and currently employed test for required fields', () => {
    // Given: An empty Work History section with jobtype set to Internship and Currently Employed is checked.
    model.jobTypeId = internshipId;
    model.isCurrentlyEmployed = true;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, enrollmentDate, disenrollmentDate, modelIds, this.enrolledPrograms);

    expect(result.isValid).toBeFalsy();

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Begin Date, Job Position, Company/Organization Name, Location, Job Duties, Benefits Offered, How was job found?, Effective Date, Pay Types, Pay Rate,
    //  Pay Rate Interval, Average Weekly Hours are required.
    expect(result.isValid).toBeFalsy();

    expect(errorDetailMsgs.indexOf('Job Type')).toBe(-1);

    expect(errorDetailMsgs.indexOf('Begin Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Position')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Company/Organization Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Duties')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Benefits Offered')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('How was job found?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Effective Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Pay Types')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Pay Rate')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Pay Rate Interval')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Average Weekly Hours')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.length).toEqual(12);
  });

  it('US943 - When externshipId is selected test for required fields', () => {
    // Given: An empty Work History section with jobtype set to Externship.
    model.jobTypeId = externshipId;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, enrollmentDate, disenrollmentDate, modelIds, this.enrolledPrograms);

    expect(result.isValid).toBeFalsy();

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Begin Date, End Date, Job Position, Company/Organization Name, Location, Job Duties, are required.
    expect(result.isValid).toBeFalsy();

    expect(errorDetailMsgs.indexOf('Job Type')).toBe(-1);

    expect(errorDetailMsgs.indexOf('Begin Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('End Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Position')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Company/Organization Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Duties')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.length).toEqual(6);
  });

  it('US943 - When externshipId and currently employed is selected test for required fields', () => {
    // Given: An empty Work History section with jobtype set to Externship and Currently Employed is checked.
    model.jobTypeId = externshipId;
    model.isCurrentlyEmployed = true;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, enrollmentDate, disenrollmentDate, modelIds, this.enrolledPrograms);

    expect(result.isValid).toBeFalsy();

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Begin Date, Job Position, Company/Organization Name, Location, Job Duties, Benefits Offered, How was job found?, Effective Date
    //  Pay Types, Pay Rate, Pay Rate Interval and Average Weekly Hours are required.
    expect(result.isValid).toBeFalsy();

    expect(errorDetailMsgs.indexOf('Job Type')).toBe(-1);

    expect(errorDetailMsgs.indexOf('Begin Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Position')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Company/Organization Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Duties')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Benefits Offered')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('How was job found?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Effective Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Pay Types')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Pay Rate')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Pay Rate Interval')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Average Weekly Hours')).toBeGreaterThan(-1);

    expect(errorDetailMsgs.length).toEqual(12);
  });

  it('US943 - When selfEmployedId is selected test for required fields', () => {
    // Given: An empty Work History section with jobtype set to Self-Employed.
    model.jobTypeId = selfEmployedId;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, enrollmentDate, disenrollmentDate, modelIds, this.enrolledPrograms);

    expect(result.isValid).toBeFalsy();

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Begin Date, End Date, Job Position, Company/Organization Name, Location, Job Duties, are required.
    expect(result.isValid).toBeFalsy();

    expect(errorDetailMsgs.indexOf('Job Type')).toBe(-1);

    expect(errorDetailMsgs.indexOf('Begin Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('End Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Position')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Company/Organization Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Duties')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.length).toEqual(6);
  });

  it('US943 - When selfEmployedId and currently employed is selected test for required fields', () => {
    // Given: An empty Work History section with jobtype set to work Self-Employed and Currently Employed is checked.
    model.jobTypeId = selfEmployedId;
    model.isCurrentlyEmployed = true;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, enrollmentDate, disenrollmentDate, modelIds, this.enrolledPrograms);

    expect(result.isValid).toBeFalsy();

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Begin Date, Job Position, Company/Organization Name, Location, Job Duties, Benefits Offered, How was job found?, Effective Date, Pay Types
    //   Pay Rate, Pay Rate Interval and Average Weekly Hours are required.
    expect(result.isValid).toBeFalsy();

    expect(errorDetailMsgs.indexOf('Job Type')).toBe(-1);

    expect(errorDetailMsgs.indexOf('Begin Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Position')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Company/Organization Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Duties')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Benefits Offered')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('How was job found?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Effective Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Pay Types')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Pay Rate')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Pay Rate Interval')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Average Weekly Hours')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.length).toEqual(12);
  });

  it('US943 - When tmjUnsubsidizedId and currently employed is selected test for required fields', () => {
    // Given: An empty Work History section with jobtype set to TMJ (Unsubsidized) and Currently Employed is checked.
    model.jobTypeId = modelIds.tmjUnsubsidizedId;
    model.isCurrentlyEmployed = true;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, enrollmentDate, disenrollmentDate, modelIds, this.enrolledPrograms);

    expect(result.isValid).toBeFalsy();

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Begin Date, Job Position, Company/Organization Name, Location, Street Address, Zip, Job Duties, Private/Public, Reason, Located in TMI Area? for Leaving
    //  Benefits Offered, How was job found?, Effective Date, Pay Types, Pay Rate, Pay Rate Interval and Average Weekly Hours are required.
    expect(result.isValid).toBeFalsy();

    expect(errorDetailMsgs.indexOf('Job Type')).toBe(-1);

    expect(errorDetailMsgs.indexOf('Begin Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Position')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Company/Organization Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Street Address')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Zip')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Duties')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Private/Public')).toBeGreaterThan(-1);
    // Reason for leaving is only present for past jobs.

    expect(errorDetailMsgs.indexOf('Located in TMI Area?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Benefits Offered')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('How was job found?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Effective Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Pay Types')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Pay Rate')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Pay Rate Interval')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Average Weekly Hours')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.length).toEqual(16);
  });

  it('US943 - When tjUnsubsidizedId is selected test for required fields', () => {
    // Given: An empty Work History section with jobtype set to TJ (Unsubsidized).
    model.jobTypeId = modelIds.tjUnsubsidizedId;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, enrollmentDate, disenrollmentDate, modelIds, this.enrolledPrograms);

    expect(result.isValid).toBeFalsy();

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Begin Date, End Date, Job Position, Company/Organization Name, Location, Street Address, Zip, Job Duties, and Private/Public are required.
    expect(result.isValid).toBeFalsy();

    expect(errorDetailMsgs.indexOf('Job Type')).toBe(-1);

    expect(errorDetailMsgs.indexOf('Begin Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('End Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Position')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Company/Organization Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Street Address')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Zip')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Duties')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Private/Public')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.length).toEqual(9);
  });

  it('US943 - When tjUnsubsidizedId and currently employed is selected test for required fields', () => {
    // Given: An empty Work History section with jobtype set to TJ (Unsubsidized) and Currently Employed is checked.
    model.jobTypeId = modelIds.tjUnsubsidizedId;
    model.isCurrentlyEmployed = true;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, enrollmentDate, disenrollmentDate, modelIds, this.enrolledPrograms);

    expect(result.isValid).toBeFalsy();

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Begin Date, Job Position, Company/Organization Name, Location, Street Address, Zip, Job Duties, Private/Public, Benefits Offered,
    // How was job found?, Effective Date, Pay Types, Pay Rate, Pay Rate Interval and Average Weekly Hours are required.
    expect(result.isValid).toBeFalsy();

    expect(errorDetailMsgs.indexOf('Job Type')).toBe(-1);

    expect(errorDetailMsgs.indexOf('Begin Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Position')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Company/Organization Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Street Address')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Zip')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Job Duties')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Private/Public')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Benefits Offered')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('How was job found?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Effective Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Pay Types')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Pay Rate')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Pay Rate Interval')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Average Weekly Hours')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.length).toEqual(15);
  });
});

describe('WorkHistory-WageHour: Requires', () => {
  const noPayTypeId = 5;
  const otherPayTypeId = 6;
  const isCurrentlyEmployed = true;

  const ne = new NavigationEnd(0, 'http://localhost:4200/login', 'http://localhost:4200/login');
  const events = new Observable(observer => {
    observer.next(this.ne);
    observer.complete();
  });

  const router = TypeMoq.Mock.ofType<Router>();

  router.setup(x => x.events).returns(() => empty());

  let appService: AppService;
  const authHttpClientMock: TypeMoq.IMock<AuthHttpClient> = TypeMoq.Mock.ofType<AuthHttpClient>();
  const logServiceMoq = TypeMoq.Mock.ofType<LogService>();
  const jwtMoq = TypeMoq.Mock.ofType<JwtAuthConfig>();
  const systemClockServiceMoq = TypeMoq.Mock.ofType<SystemClockService>();
  const location = TypeMoq.Mock.ofType<Location>();

  let validationManager: ValidationManager;
  let model: Absence;

  const participantDOB: moment.Moment = moment('03/29/1991', 'MM/DD/YYYY');
  const jobBeginDate: moment.Moment = moment('06/30/2006', 'MM/DD/YYYY');
  const jobEndDate: moment.Moment = moment('12/31/9999', 'MM/DD/YYYY');
  const programStatus = 'currentJob';
  const otherAbsenceTypeId = 1;
  const absences = null;

  beforeEach(() => {
    appService = new AppService(authHttpClientMock.object, logServiceMoq.object, router.object, jwtMoq.object, location.object);
    validationManager = new ValidationManager(appService);
    const wh = new WageHour();
    const ab = new Absence();
    const jct = new JobActionType();
    wh.currentPayType = jct;
    wh.currentPayType.jobActionTypes = [];
    wh.wageHourHistories = [];
    model = ab;
  });
  it('Basic - should create an instance', () => {
    expect(model).toBeTruthy();
  });

  it('US943 - When job begin date and the begin date are the same is selected test for required fields', () => {
    // Given: An empty Work History section with job type set to Volunteer and Currently Employed is checked.
    model.beginDate = '06/30/2006';
    model.endDate = '06/30/2006';

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, jobBeginDate, jobEndDate, false, otherAbsenceTypeId, absences);

    expect(result.isValid).toBeFalsy();

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Begin Date, End Date and  Reason are required.
    expect(result.isValid).toBeFalsy();

    expect(errorDetailMsgs.indexOf('Begin Date')).toBe(-1);

    expect(errorDetailMsgs.indexOf('Reason')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.length).toEqual(1);
  });

  //it('US943 - When job begin date is greater than end date is selected test for required fields', () => {
  // Given: An empty Work History section with job type set to Volunteer and Currently Employed is checked.
  //model.beginDate = '07/30/2006';
  //model.endDate = '06/30/2006';

  // When: The section is validated.
  //let result = model.validate(validationManager, participantDOB, jobBeginDate, jobEndDate);

  //expect(result.isValid).toBeFalsy();
  //
  //let errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

  // Then: Begin Date, End Date and  Reason are required.
  //expect(result.isValid).toBeFalsy();

  //expect(errorDetailMsgs.indexOf('Begin Date')).toBeGreaterThan(-1);
  //expect(errorDetailMsgs.indexOf('End Date')).toBeGreaterThan(-1);

  //expect(errorDetailMsgs.indexOf('Reason')).toBe(-1);
  //expect(errorDetailMsgs.length).toEqual(1);
  //});
});
