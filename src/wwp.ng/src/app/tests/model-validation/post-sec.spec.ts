// tslint:disable: deprecation
import { LogService } from '../../shared/services/log.service';
import { Router } from '@angular/router';
import * as moment from 'moment';
import * as TypeMoq from 'typemoq';
import {} from 'jasmine';
import { AppService } from './../../core/services/app.service';
import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
import { PostSecondaryEducationSection } from '../../shared/models/post-secondary-education-section';
import { ValidationManager } from '../../shared/models/validation-manager';
import { ValidationCode } from '../../shared/models/validation-error';
import { TestingUtilities } from './testing-utilities';
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

  let model: PostSecondaryEducationSection;

  beforeEach(() => {
    appService = new AppService(authHttpClientMock.object, logServiceMoq.object, router.object, jwtMoq.object, location.object);
    validationManager = new ValidationManager(appService);

    // Start with a fresh model for each test.
    model = new PostSecondaryEducationSection();
  });

  it('Empty Post Sec section is not valid.', () => {
    // Given: Post-Secondary Section is empty.

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is not valid.
    expect(errorDetailMsgs.indexOf('Have you attended, or are you currently attending a college or university?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Do you have or are you working towards any licenses or certificates?')).toBeGreaterThan(-1);

    expect(errorDetailMsgs.length).toEqual(2);
    expect(result.isValid).toEqual(false);
  });

  it('Post Sec section is not valid.', () => {
    // Given: Post-Secondary Section is set to yes for isWorkingOnLicensesOrCertificates.
    model.isWorkingOnLicensesOrCertificates = true;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is not valid.
    expect(errorDetailMsgs.indexOf('Have you attended, or are you currently attending a college or university?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('License Type')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('License Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('License Valid in WI')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('License Issuer')).toBeGreaterThan(-1);

    expect(errorDetailMsgs.length).toEqual(5);
    expect(result.isValid).toEqual(false);
  });
});
