// tslint:disable: deprecation
import { LogService } from '../../shared/services/log.service';
import { Router } from '@angular/router';
import * as moment from 'moment';
import * as TypeMoq from 'typemoq';
import {} from 'jasmine';
import { AppService } from './../../core/services/app.service';
import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
import { DropDownField } from './../../shared/models/dropdown-field';
import { WorkProgramsSection, WorkProgram } from '../../shared/models/work-programs-section';
import { ValidationManager } from '../../shared/models/validation-manager';
import { ValidationCode } from '../../shared/models/validation-error';
import { TestingUtilities } from './testing-utilities';
import { empty } from 'rxjs';
import { JwtAuthConfig } from 'src/app/core/jwt-auth-config';
import { Location } from '@angular/common';

describe('Work-ProgramsEditComponent: Validate', () => {
  const router = TypeMoq.Mock.ofType<Router>();
  const logServiceMoq = TypeMoq.Mock.ofType<LogService>();
  const jwtMoq = TypeMoq.Mock.ofType<JwtAuthConfig>();

  router.setup(x => x.events).returns(() => empty());
  let appService: AppService;
  const authHttpClientMock: TypeMoq.IMock<AuthHttpClient> = TypeMoq.Mock.ofType<AuthHttpClient>();
  let validationManager: ValidationManager;
  const location = TypeMoq.Mock.ofType<Location>();

  const participantDOB = moment('03/29/1991', 'MM/DD/YYYY');

  let model: WorkProgramsSection;

  const dp: DropDownField[] = [];
  const dpi = new DropDownField();
  dpi.name = 'Past';
  dpi.id = 1;
  dp.push(dpi);
  const dpi2 = new DropDownField();
  dpi2.name = 'Current';
  dpi2.id = 2;
  dp.push(dpi2);
  const dpi3 = new DropDownField();
  dpi3.name = 'Waitlist';
  dpi3.id = 3;
  dp.push(dpi3);

  beforeEach(() => {
    appService = new AppService(authHttpClientMock.object, logServiceMoq.object, router.object, jwtMoq.object, location.object);

    validationManager = new ValidationManager(appService);

    // Start with a fresh model for each test.
    model = new WorkProgramsSection();
    model.workPrograms = [];
    const wp = new WorkProgram();
    model.workPrograms.push(wp);
  });

  it('When WorkProgramSection is false', () => {
    // Given: Model to tempCustodialParentUnsubsidizedId.
    model.isInOtherPrograms = false;

    // When: The section is validated.
    const result = model.validate(validationManager, dp, null, participantDOB);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // expect(result.isValid).toBeFalsy();

    // Then: Work Status, Name, Location are required.
    expect(result.isValid).toBe(true);

    // expect(errorDetailMsgs.indexOf('Job Type')).toBe(-1);

    // expect(errorDetailMsgs.indexOf('Work Status')).toBeGreaterThan(-1);
    // expect(errorDetailMsgs.indexOf('Name')).toBeGreaterThan(-1);
    // expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);

    expect(errorDetailMsgs.length).toEqual(0);
  });

  it('When WorkProgramSection is true', () => {
    // Given: Model to tempCustodialParentUnsubsidizedId.
    model.isInOtherPrograms = true;

    // When: The section is validated.
    const result = model.validate(validationManager, dp, null, participantDOB);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // expect(result.isValid).toBeFalsy();

    // Then: Work Status, Name, Location are required.
    expect(result.isValid).toBe(false);

    // expect(errorDetailMsgs.indexOf('Job Type')).toBe(-1);

    expect(errorDetailMsgs.indexOf('Work Status')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);

    expect(errorDetailMsgs.length).toEqual(3);
  });

  it('When WorkProgramSection is true, status current and dates are not set.', () => {
    // Given: Model to isInOtherPrograms True, status current and dates are not set.
    model.isInOtherPrograms = true;
    model.workPrograms[0].workStatus = 2; // Current.

    // When: The section is validated.
    const result = model.validate(validationManager, dp, null, participantDOB);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Name, Location are required.
    expect(result.isValid).toBe(false);
    expect(errorDetailMsgs.indexOf('Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Start Date')).toBeGreaterThan(-1);

    expect(errorDetailMsgs.length).toEqual(3);
  });

  it('When WorkProgramSection is true', () => {
    // Given: Model to isInOtherPrograms True, status past and dates are not set.
    model.isInOtherPrograms = true;
    model.workPrograms[0].workStatus = 3; // Waitlist.

    // When: The section is validated.
    const result = model.validate(validationManager, dp, null, participantDOB);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Name, Location are required.
    expect(result.isValid).toBe(false);
    expect(errorDetailMsgs.indexOf('Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);

    expect(errorDetailMsgs.length).toEqual(2);
  });

  it('When WorkProgramSection is true - isInOtherPrograms True, status past and dates are not set.', () => {
    // Given: Model to isInOtherPrograms True, status past and dates are not set.
    model.isInOtherPrograms = true;
    model.workPrograms[0].workStatus = 1; // Past.

    // When: The section is validated.
    const result = model.validate(validationManager, dp, null, participantDOB);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Name, Location are required.
    expect(result.isValid).toBe(false);
    expect(errorDetailMsgs.indexOf('Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Start Date')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('End Date')).toBeGreaterThan(-1);

    expect(errorDetailMsgs.length).toEqual(4);
  });

  it('When WorkProgramSection is true - isInOtherPrograms True, status past and dates set.', () => {
    // Given: Model to isInOtherPrograms True, status past and dates set.
    model.isInOtherPrograms = true;
    model.workPrograms[0].workStatus = 1; // Past.
    model.workPrograms[0].startDate = '03/2015';
    model.workPrograms[0].endDate = '05/2015';

    // When: The section is validated.
    const result = model.validate(validationManager, dp, null, participantDOB);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Name, Location are required.
    expect(result.isValid).toBe(false);
    expect(errorDetailMsgs.indexOf('Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Location')).toBeGreaterThan(-1);

    expect(errorDetailMsgs.length).toEqual(2);
  });
});
