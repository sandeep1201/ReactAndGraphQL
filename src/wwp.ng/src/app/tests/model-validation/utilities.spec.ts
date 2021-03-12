// tslint:disable: deprecation
import { LogService } from '../../shared/services/log.service';
import { AppService } from './../../core/services/app.service';
import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
import { Router } from '@angular/router';
import { Utilities } from '../../shared/utilities';
import * as TypeMoq from 'typemoq';
import {} from 'jasmine';
import { ValidationResult } from '../../shared/models/validation-result';
import { ValidationManager } from '../../shared/models/validation-manager';
import { empty } from 'rxjs';
import { JwtAuthConfig } from 'src/app/core/jwt-auth-config';
import { Location } from '@angular/common';

describe('Utilities: validateMmDdYyyyDate', () => {
  const router = TypeMoq.Mock.ofType<Router>();

  router.setup(x => x.events).returns(() => empty());
  const logServiceMoq = TypeMoq.Mock.ofType<LogService>();
  const jwtMoq = TypeMoq.Mock.ofType<JwtAuthConfig>();
  const location = TypeMoq.Mock.ofType<Location>();

  let appService: AppService;
  const authHttpClientMock: TypeMoq.IMock<AuthHttpClient> = TypeMoq.Mock.ofType<AuthHttpClient>();
  let validationManager: ValidationManager;
  let result: ValidationResult;

  beforeEach(() => {
    result = new ValidationResult();
    appService = new AppService(authHttpClientMock.object, logServiceMoq.object, router.object, jwtMoq.object, location.object);

    validationManager = new ValidationManager(appService);
    this.referralDateWithValidationRules = null;
  });

  it('Should be Valid', () => {
    this.referralDateWithValidationRules = {
      date: '03/29/1991',
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: false,
      minDate: null,
      minDateAllowSame: null,
      minDateName: null,
      maxDate: null,
      maxDateAllowSame: null,
      maxDateName: null,
      participantDOB: null
    };
    const isDateValid = Utilities.validateMmDdYyyyDate(this.referralDateWithValidationRules);
    expect(isDateValid).toBe(true);
  });

  it('Should be invalid - Date Format Wrong', () => {
    this.referralDateWithValidationRules = {
      date: '03/29/191',
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: false,
      minDate: null,
      minDateAllowSame: null,
      minDateName: null,
      maxDate: null,
      maxDateAllowSame: null,
      maxDateName: null,
      participantDOB: null
    };
    const isDateValid = Utilities.validateMmDdYyyyDate(this.referralDateWithValidationRules);
    expect(isDateValid).toBe(false);
  });

  it('Should be Valid - Date is before max date', () => {
    this.referralDateWithValidationRules = {
      date: '03/29/1991',
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: false,
      minDate: null,
      minDateAllowSame: null,
      minDateName: null,
      maxDate: '03/29/1995',
      maxDateAllowSame: false,
      maxDateName: null,
      participantDOB: null
    };
    const isDateValid = Utilities.validateMmDdYyyyDate(this.referralDateWithValidationRules);
    expect(isDateValid).toBe(true);
  });

  it('Should be Valid - Date is after min date', () => {
    this.referralDateWithValidationRules = {
      date: '03/29/1991',
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: false,
      minDate: '03/29/1990',
      minDateAllowSame: false,
      minDateName: null,
      maxDate: null,
      maxDateAllowSame: null,
      maxDateName: null,
      participantDOB: null
    };
    const isDateValid = Utilities.validateMmDdYyyyDate(this.referralDateWithValidationRules);
    expect(isDateValid).toBe(true);
  });

  it('Should be Valid - Date is same min date', () => {
    this.referralDateWithValidationRules = {
      date: '03/29/1990',
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: false,
      minDate: '03/29/1990',
      minDateAllowSame: true,
      minDateName: null,
      maxDate: null,
      maxDateAllowSame: null,
      maxDateName: null,
      participantDOB: null
    };
    const isDateValid = Utilities.validateMmDdYyyyDate(this.referralDateWithValidationRules);
    expect(isDateValid).toBe(true);
  });

  it('Should be Invalid - Date is same min date', () => {
    this.referralDateWithValidationRules = {
      date: '03/29/1990',
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: false,
      minDate: '03/29/1990',
      minDateAllowSame: false,
      minDateName: null,
      maxDate: null,
      maxDateAllowSame: null,
      maxDateName: null,
      participantDOB: null
    };
    const isDateValid = Utilities.validateMmDdYyyyDate(this.referralDateWithValidationRules);
    expect(isDateValid).toBe(false);
  });

  it('Should be Valid - Date is same as min date and allowed to be same', () => {
    this.referralDateWithValidationRules = {
      date: '03/29/1991',
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: false,
      minDate: '03/29/1991',
      minDateAllowSame: true,
      minDateName: null,
      maxDate: null,
      maxDateAllowSame: null,
      maxDateName: null,
      participantDOB: null
    };
    const isDateValid = Utilities.validateMmDdYyyyDate(this.referralDateWithValidationRules);
    expect(isDateValid).toBe(true);
  });

  it('Should be Valid - Date is same as max date and allowed to be same', () => {
    this.referralDateWithValidationRules = {
      date: '03/29/1991',
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: false,
      minDate: null,
      minDateAllowSame: null,
      minDateName: null,
      maxDate: '03/29/1991',
      maxDateAllowSame: true,
      maxDateName: null,
      participantDOB: null
    };
    const isDateValid = Utilities.validateMmDdYyyyDate(this.referralDateWithValidationRules);
    expect(isDateValid).toBe(true);
  });

  it('Should be Invalid - Date is more than 150 years since participantDOB', () => {
    this.referralDateWithValidationRules = {
      date: '03/29/2150',
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: false,
      minDate: null,
      minDateAllowSame: null,
      minDateName: null,
      maxDate: null,
      maxDateAllowSame: null,
      maxDateName: null,
      participantDOB: '03/28/2000'
    };
    const isDateValid = Utilities.validateMmDdYyyyDate(this.referralDateWithValidationRules);
    expect(isDateValid).toBe(false);
  });

  it('Should be Valid - Date is 150 years since participantDOB', () => {
    this.referralDateWithValidationRules = {
      date: '03/29/2150',
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: false,
      minDate: null,
      minDateAllowSame: null,
      minDateName: null,
      maxDate: null,
      maxDateAllowSame: null,
      maxDateName: null,
      participantDOB: '03/29/2000'
    };
    const isDateValid = Utilities.validateMmDdYyyyDate(this.referralDateWithValidationRules);
    expect(isDateValid).toBe(true);
  });

  it('Should be Valid - Date is after minDate', () => {
    this.referralDateWithValidationRules = {
      date: '03/29/1999',
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: false,
      minDate: '03/29/1998',
      minDateAllowSame: false,
      minDateName: null,
      maxDate: null,
      maxDateAllowSame: null,
      maxDateName: null,
      participantDOB: '03/29/1991'
    };
    const isDateValid = Utilities.validateMmDdYyyyDate(this.referralDateWithValidationRules);
    expect(isDateValid).toBe(true);
  });

  it('Should be Invalid - Date is before minDate', () => {
    this.referralDateWithValidationRules = {
      date: '03/29/1998',
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: false,
      minDate: '03/29/1999',
      minDateAllowSame: false,
      minDateName: null,
      maxDate: null,
      maxDateAllowSame: null,
      maxDateName: null,
      participantDOB: '03/29/1991'
    };
    const isDateValid = Utilities.validateMmDdYyyyDate(this.referralDateWithValidationRules);
    expect(isDateValid).toBe(false);
  });

  it('Should be Valid - Date is same as minDate', () => {
    this.referralDateWithValidationRules = {
      date: '03/29/1999',
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: false,
      minDate: '03/29/1999',
      minDateAllowSame: true,
      minDateName: null,
      maxDate: null,
      maxDateAllowSame: null,
      maxDateName: null,
      participantDOB: null
    };

    const isDateValid = Utilities.validateMmDdYyyyDate(this.referralDateWithValidationRules);
    expect(isDateValid).toBe(true);
  });

  it('Should be invalid - Month and day is 55', () => {
    this.referralDateWithValidationRules = {
      date: '55/55/5555',
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: false,
      minDate: null,
      minDateAllowSame: null,
      minDateName: null,
      maxDate: null,
      maxDateAllowSame: null,
      maxDateName: null,
      participantDOB: null
    };
    const isDateValid = Utilities.validateMmDdYyyyDate(this.referralDateWithValidationRules);
    expect(isDateValid).toBe(false);
  });
});

describe('Utilities: validateMmYyyyDate', () => {
  const router = TypeMoq.Mock.ofType<Router>();

  router.setup(x => x.events).returns(() => empty());
  const logServiceMoq = TypeMoq.Mock.ofType<LogService>();
  const jwtMoq = TypeMoq.Mock.ofType<JwtAuthConfig>();
  const location = TypeMoq.Mock.ofType<Location>();

  let appService: AppService;
  const authHttpClientMock: TypeMoq.IMock<AuthHttpClient> = TypeMoq.Mock.ofType<AuthHttpClient>();
  let validationManager: ValidationManager;
  let result: ValidationResult;

  beforeEach(() => {
    result = new ValidationResult();
    appService = new AppService(authHttpClientMock.object, logServiceMoq.object, router.object, jwtMoq.object, location.object);
    validationManager = new ValidationManager(appService);
  });

  it('Should be Valid for Required', () => {
    this.referralDateWithValidationRules = {
      date: '03/2000',
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: true,
      minDate: null,
      minDateAllowSame: null,
      minDateName: null,
      maxDate: null,
      maxDateAllowSame: null,
      maxDateName: null,
      participantDOB: null
    };
    const isDateValid = Utilities.validateMmYyyyDate(this.referralDateWithValidationRules);
    expect(isDateValid).toBe(true);
  });

  it('Should be Invalid for Required', () => {
    this.referralDateWithValidationRules = {
      date: '',
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: true,
      minDate: null,
      minDateAllowSame: null,
      minDateName: null,
      maxDate: null,
      maxDateAllowSame: null,
      maxDateName: null,
      participantDOB: null
    };
    const isDateValid = Utilities.validateMmYyyyDate(this.referralDateWithValidationRules);
    expect(isDateValid).toBe(false);
  });

  it('Should be Invalid for Required', () => {
    this.referralDateWithValidationRules = {
      date: null,
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: true,
      minDate: null,
      minDateAllowSame: null,
      minDateName: null,
      maxDate: null,
      maxDateAllowSame: null,
      maxDateName: null,
      participantDOB: null
    };
    const isDateValid = Utilities.validateMmYyyyDate(this.referralDateWithValidationRules);
    expect(isDateValid).toBe(false);
  });

  it('Should be Invalid for Required', () => {
    this.referralDateWithValidationRules = {
      date: '03/2000',
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: true,
      minDate: '03/2001',
      minDateAllowSame: false,
      minDateName: 'minDate',
      maxDate: null,
      maxDateAllowSame: null,
      maxDateName: null,
      participantDOB: null
    };
    const isDateValid = Utilities.validateMmYyyyDate(this.referralDateWithValidationRules);
    expect(isDateValid).toBe(false);
  });

  it('Should be Invalid for Required', () => {
    this.referralDateWithValidationRules = {
      date: '03/2000',
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: true,
      minDate: '03/2000',
      minDateAllowSame: true,
      minDateName: 'minDate',
      maxDate: null,
      maxDateAllowSame: null,
      maxDateName: null,
      participantDOB: null
    };
    const isDateValid = Utilities.validateMmYyyyDate(this.referralDateWithValidationRules);
    expect(isDateValid).toBe(true);
  });

  it('Should be Valid for Required', () => {
    this.referralDateWithValidationRules = {
      date: '03/2000',
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: true,
      minDate: '03/2000',
      minDateAllowSame: true,
      minDateName: 'minDate',
      maxDate: '03/2000',
      maxDateAllowSame: true,
      maxDateName: 'maxDate',
      participantDOB: null
    };
    const isDateValid = Utilities.validateMmYyyyDate(this.referralDateWithValidationRules);
    expect(isDateValid).toBe(true);
  });

  it('Should be Invalid for Required', () => {
    this.referralDateWithValidationRules = {
      date: '03/2000',
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: true,
      minDate: '03/2000',
      minDateAllowSame: true,
      minDateName: 'minDate',
      maxDate: '03/2000',
      maxDateAllowSame: false,
      maxDateName: 'maxDate',
      participantDOB: null
    };
    const isDateValid = Utilities.validateMmYyyyDate(this.referralDateWithValidationRules);
    expect(isDateValid).toBe(false);
  });

  it('Should be Invalid for Required', () => {
    this.referralDateWithValidationRules = {
      date: '03/2000',
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: true,
      minDate: '03/2000',
      minDateAllowSame: false,
      minDateName: 'minDate',
      maxDate: '03/2000',
      maxDateAllowSame: false,
      maxDateName: 'maxDate',
      participantDOB: null
    };
    const isDateValid = Utilities.validateMmYyyyDate(this.referralDateWithValidationRules);
    expect(isDateValid).toBe(false);
  });

  it('Should be Invalid for Required', () => {
    this.referralDateWithValidationRules = {
      date: '03/2000',
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: true,
      minDate: '03/2000',
      minDateAllowSame: false,
      minDateName: 'minDate',
      maxDate: '03/2000',
      maxDateAllowSame: false,
      maxDateName: 'maxDate',
      participantDOB: '04/10/2000'
    };
    const isDateValid = Utilities.validateMmYyyyDate(this.referralDateWithValidationRules);
    expect(isDateValid).toBe(false);
  });

  it('Should be Invalid for 14 as month', () => {
    this.referralDateWithValidationRules = {
      date: '14/2000',
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: false,
      minDate: null,
      minDateAllowSame: false,
      minDateName: null,
      maxDate: null,
      maxDateAllowSame: false,
      maxDateName: null,
      participantDOB: null
    };
    const isDateValid = Utilities.validateMmYyyyDate(this.referralDateWithValidationRules);
    expect(isDateValid).toBe(false);
  });

  it('Should be Invalid for 13 as month', () => {
    this.referralDateWithValidationRules = {
      date: '13/2000',
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: false,
      minDate: null,
      minDateAllowSame: false,
      minDateName: null,
      maxDate: null,
      maxDateAllowSame: false,
      maxDateName: null,
      participantDOB: null
    };
    const isDateValid = Utilities.validateMmYyyyDate(this.referralDateWithValidationRules);
    expect(isDateValid).toBe(false);
  });

  it('Should be Invalid for 00 as month', () => {
    this.referralDateWithValidationRules = {
      date: '00/2000',
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: false,
      minDate: null,
      minDateAllowSame: false,
      minDateName: null,
      maxDate: null,
      maxDateAllowSame: false,
      maxDateName: null,
      participantDOB: null
    };
    const isDateValid = Utilities.validateMmYyyyDate(this.referralDateWithValidationRules);
    expect(isDateValid).toBe(false);
  });

  it('Should be valid for 01 as month', () => {
    this.referralDateWithValidationRules = {
      date: '01/2000',
      prop: 'referralDate',
      prettyProp: 'Referral Date',
      result: result,
      validationManager: validationManager,
      isRequired: false,
      minDate: null,
      minDateAllowSame: false,
      minDateName: null,
      maxDate: null,
      maxDateAllowSame: false,
      maxDateName: null,
      participantDOB: null
    };
    const isDateValid = Utilities.validateMmYyyyDate(this.referralDateWithValidationRules);
    expect(isDateValid).toBe(true);
  });
});
