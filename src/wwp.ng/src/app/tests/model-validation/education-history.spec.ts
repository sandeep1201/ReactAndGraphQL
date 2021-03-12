// tslint:disable:no-unused-variable
// tslint:disable: deprecation
import { Router } from '@angular/router';
import * as moment from 'moment';
import * as TypeMoq from 'typemoq';
import { AppService } from './../../core/services/app.service';
import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
import { EducationHistorySection } from '../../shared/models/education-history-section';
import { GoogleLocation } from '../../shared/models/google-location';
import { ValidationManager } from '../../shared/models/validation-manager';
import { ValidationCode } from '../../shared/models/validation-error';
import { TestingUtilities } from './testing-utilities';
import { LogService } from '../../shared/services/log.service';
import { empty } from 'rxjs';
import { JwtAuthConfig } from 'src/app/core/jwt-auth-config';
import { Location } from '@angular/common';

describe('EducationHistoryEditComponent: Validate', () => {
  const router = TypeMoq.Mock.ofType<Router>();
  const logServiceMoq = TypeMoq.Mock.ofType<LogService>();
  const jwtMoq = TypeMoq.Mock.ofType<JwtAuthConfig>();
  const location = TypeMoq.Mock.ofType<Location>();
  router.setup(x => x.events).returns(() => empty());

  let appService: AppService;
  const authHttpClientMock: TypeMoq.IMock<AuthHttpClient> = TypeMoq.Mock.ofType<AuthHttpClient>();
  let validationManager: ValidationManager;
  const participantDOB: moment.Moment = moment('03/12/1999', 'MM/DD/YYYY');
  const diplomaId = 1;
  const gedId = 2;
  const hsedId = 3;
  const noneId = 4;

  let model: EducationHistorySection;

  beforeEach(() => {
    appService = new AppService(authHttpClientMock.object, logServiceMoq.object, router.object, jwtMoq.object, location.object);
    validationManager = new ValidationManager(appService);

    // Start with a fresh model for each test.
    model = new EducationHistorySection();
    model.location = new GoogleLocation();
  });

  it('Empty Educational History section is not valid.', () => {
    // Given: Graduation status is Diploma and the rest is null for Education History section.
    model.diploma = diplomaId;
    model.location = null;
    model.schoolName = null;
    model.lastYearAttended = null;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, diplomaId, gedId, hsedId, noneId);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is not valid.
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('School Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Last Year Attended')).toBeGreaterThan(-1);

    expect(errorDetailMsgs.length).toEqual(3);
    expect(result.isValid).toEqual(false);
  });

  it('Empty Educational History section is not valid.', () => {
    // Given: Graduation Status is GED and the rest is null in Education History section.
    model.diploma = gedId;
    model.location = null;
    model.schoolName = null;
    model.lastYearAttended = null;
    model.lastGradeCompleted = null;
    model.issuingAuthorityName = null;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, diplomaId, gedId, hsedId, noneId);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is not valid.
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('School Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Last Year Attended')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Last Grade Completed')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('State Issued')).toBeGreaterThan(-1);

    expect(errorDetailMsgs.length).toEqual(5);
    expect(result.isValid).toEqual(false);
  });

  it('Empty Educational History section is not valid.', () => {
    // Given: Graduation Status is HSED and the rest is null in Education History section.
    model.diploma = hsedId;
    model.location = null;
    model.schoolName = null;
    model.lastYearAttended = null;
    model.lastGradeCompleted = null;
    model.issuingAuthorityName = null;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, diplomaId, gedId, hsedId, noneId);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is not valid.
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('School Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Last Year Attended')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Last Grade Completed')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('State Issued')).toBeGreaterThan(-1);

    expect(errorDetailMsgs.length).toEqual(5);
    expect(result.isValid).toEqual(false);
  });

  it('Diploma entry for Educational History section is valid.', () => {
    // Given: Graduation status of none is entered and the rest is null on Education History section.
    model.diploma = noneId;
    model.hasEverGoneToSchool = null;
    model.gedHsedStatus = null;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, diplomaId, gedId, hsedId, noneId);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is valid.
    expect(errorDetailMsgs.indexOf('Have you ever attended school?')).toBeGreaterThan(-1);

    expect(errorDetailMsgs.length).toEqual(1);
    expect(result.isValid).toEqual(false);
  });

  it('Diploma entry for Educational History section is valid.', () => {
    // Given: Graduation status of Diploma is entered with all fields entered on Education History section.
    model.diploma = diplomaId;
    model.location.city = 'Madison, WI, United States';
    model.schoolName = 'East High School';
    model.lastYearAttended = 2000;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, diplomaId, gedId, hsedId, noneId);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is valid.

    expect(errorDetailMsgs.length).toEqual(0);
    expect(result.isValid).toEqual(true);
  });

  it('Diploma entry for Educational History section is valid.', () => {
    // Given: Graduation status of GED is entered without GED fields entered on Education History section.
    model.diploma = gedId;
    model.location.city = 'Madison, WI, United States';
    model.schoolName = 'East High School';
    model.lastYearAttended = 2000;
    model.lastGradeCompleted = 11;
    // model.issuingAuthorityName = 'Wisconsin';
    // model.certificateYearAwarded = 2014;

    // When: The section is validated.

    const result = model.validate(validationManager, participantDOB, diplomaId, gedId, hsedId, noneId);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is valid.
    expect(errorDetailMsgs.indexOf('State Issued')).toBeGreaterThan(-1);

    expect(errorDetailMsgs.length).toEqual(1);
    expect(result.isValid).toEqual(false);
  });

  it('Diploma entry for Educational History section is valid.', () => {
    // Given: Graduation status of GED is entered with GED fields entered on Education History section.
    model.diploma = gedId;
    model.location.city = 'Madison, WI, United States';
    model.schoolName = 'East High School';
    model.lastYearAttended = 2000;
    model.lastGradeCompleted = 11;
    model.issuingAuthorityCode = 1;
    model.certificateYearAwarded = 2014;

    // When: The section is validated.

    const result = model.validate(validationManager, participantDOB, diplomaId, gedId, hsedId, noneId);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is valid.

    expect(errorDetailMsgs.length).toEqual(0);
    expect(result.isValid).toEqual(true);
  });

  it('Diploma entry for Educational History section is valid.', () => {
    // Given: Graduation status of HSED is entered with HSED fields entered on Education History section.
    model.diploma = hsedId;
    model.location.city = 'Madison, WI, United States';
    model.schoolName = 'LaFollette High School';
    model.lastYearAttended = 2012;
    model.lastGradeCompleted = 11;
    model.issuingAuthorityCode = 1;
    model.certificateYearAwarded = 2014;

    // When: The section is validated.

    const result = model.validate(validationManager, participantDOB, diplomaId, gedId, hsedId, noneId);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is valid.

    expect(errorDetailMsgs.length).toEqual(0);
    expect(result.isValid).toEqual(true);
  });

  it('Diploma entry for Educational History section is valid.', () => {
    // Given: Graduation status of none is entered and enrolled in school is true on Education History section.
    model.diploma = noneId;
    model.hasEverGoneToSchool = true;
    model.location.city = 'Madison, WI, United States';
    model.schoolName = 'LaFollette High School';
    model.lastYearAttended = 2012;
    model.lastGradeCompleted = 11;
    model.isCurrentlyEnrolled = true;
    model.hasEducationPlan = false;

    // When: The section is validated.

    const result = model.validate(validationManager, participantDOB, diplomaId, gedId, hsedId, noneId);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is valid.

    expect(errorDetailMsgs.length).toEqual(0);
    expect(result.isValid).toEqual(true);
  });

  it('Diploma entry for Educational History section is valid.', () => {
    // Given: Graduation status of none is entered and enrolled in school is false on Education History section.
    model.diploma = noneId;
    model.hasEverGoneToSchool = true;
    model.location.city = 'Madison, WI, United States';
    model.schoolName = 'LaFollette High School';
    model.lastYearAttended = 2012;
    model.lastGradeCompleted = 11;
    model.isCurrentlyEnrolled = false;

    // When: The section is validated.

    const result = model.validate(validationManager, participantDOB, diplomaId, gedId, hsedId, noneId);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is valid.

    expect(errorDetailMsgs.length).toEqual(0);
    expect(result.isValid).toEqual(true);
  });

  it('Diploma entry for Educational History section is valid.', () => {
    // Given: Graduation status of none is entered and attended school is false on Education History section.
    model.diploma = noneId;
    model.hasEverGoneToSchool = false;

    // When: The section is validated.

    const result = model.validate(validationManager, participantDOB, diplomaId, gedId, hsedId, noneId);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is valid.

    expect(errorDetailMsgs.length).toEqual(0);
    expect(result.isValid).toEqual(true);
  });
});
