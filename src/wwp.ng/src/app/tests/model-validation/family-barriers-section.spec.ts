// tslint:disable:no-unused-variable
// tslint:disable: deprecation
import { LogService } from '../../shared/services/log.service';
import { Router } from '@angular/router';
import { ActionNeeded } from '../../features-modules/actions-needed/models/action-needed-new';
import { AppService } from './../../core/services/app.service';
import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
import { DropDownField } from './../../shared/models/dropdown-field';
import { FamilyBarriersSection } from '../../shared/models/family-barriers-section';
import { TestingUtilities } from './testing-utilities';
import { ValidationCode } from '../../shared/models/validation-error';
import { ValidationManager } from '../../shared/models/validation-manager';
import { empty } from 'rxjs';
import * as moment from 'moment';
import * as TypeMoq from 'typemoq';
import { JwtAuthConfig } from 'src/app/core/jwt-auth-config';
import { Location } from '@angular/common';

describe('FamilyBarriersSection: Validate', () => {
  const router = TypeMoq.Mock.ofType<Router>();
  const logServiceMoq = TypeMoq.Mock.ofType<LogService>();
  const jwtMoq = TypeMoq.Mock.ofType<JwtAuthConfig>();
  const location = TypeMoq.Mock.ofType<Location>();

  router.setup(x => x.events).returns(() => empty());

  let appService: AppService;
  const authHttpClientMock: TypeMoq.IMock<AuthHttpClient> = TypeMoq.Mock.ofType<AuthHttpClient>();
  let validationManager: ValidationManager;
  const participantDOB: moment.Moment = moment('03/29/1991', 'MM/DD/YYYY');
  let model: FamilyBarriersSection;

  const dp: DropDownField[] = [];
  const dpi = new DropDownField();
  dpi.id = 1;
  dp.push(dpi);
  const dpi2 = new DropDownField();
  dpi2.id = 2;
  dp.push(dpi);
  const dpi3 = new DropDownField();
  dpi3.id = 3;
  dp.push(dpi3);

  beforeEach(() => {
    // Start with a fresh model for each test.
    model = new FamilyBarriersSection();
    model.actionNeeded = new ActionNeeded();
    model.actionNeeded.tasks = [];
    model.actionNeeded.isNoActionNeeded = true;

    appService = new AppService(authHttpClientMock.object, logServiceMoq.object, router.object, jwtMoq.object, location.object);
    validationManager = new ValidationManager(appService);
  });

  it('Empty section is not valid.', () => {
    // Given: An empty Family Barriers section.

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, dp);

    // Then: The section is not valid.
    expect(result.isValid).toEqual(false);
  });

  it('Have you ever applied for SSI or SSDI is required based on the canAccessFamilyBarriersSsi.', () => {
    // Given: An empty Family Barriers section.
    const canAccessFamilyBarriersSsi = true;
    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, dp, canAccessFamilyBarriersSsi);

    // Then: The section is not valid and an error is shown that 'Have you ever applied for SSI or SSDI?' is required.
    expect(result.isValid).toEqual(false);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    expect(errorDetailMsgs.indexOf('Have you ever applied for SSI or SSDI?')).toBeGreaterThan(-1);
  });

  it('Have you ever applied for SSI or SSDI is required based on canAccessFamilyBarriersSsi.', () => {
    // Given: An empty Family Barriers section with it set Yes for 'Have you ever applied for SSI or SSDI?'
    model.hasEverAppliedSsi = true;
    const canAccessFamilyBarriersSsi = false;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, dp, canAccessFamilyBarriersSsi);

    // Then: The section does not show an error for 'Have you ever applied for SSI or SSDI?'.
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    expect(errorDetailMsgs.indexOf('Have you ever applied for SSI or SSDI?')).toBe(-1);
  });

  it('Have you ever applied for SSI or SSDI is required based on canAccessFamilyBarriersSsi.', () => {
    // Given: An empty Family Barriers section with it set to No for 'Have you ever applied for SSI or SSDI?'
    model.hasEverAppliedSsi = false;
    const canAccessFamilyBarriersSsi = false;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, dp);

    // Then: The section does not show an error for 'Have you ever applied for SSI or SSDI?'.
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    expect(errorDetailMsgs.indexOf('Have you ever applied for SSI or SSDI?')).toBe(-1);
  });

  it('Are you currently in the process of applying for SSI or SSDI is required sometimes.', () => {
    // Given: A Family Barriers section with 'Have you ever applied for SSI or SSDI?' set to Yes
    model.hasEverAppliedSsi = true;
    const canAccessFamilyBarriersSsi = true;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, dp, canAccessFamilyBarriersSsi);

    // Then: The section does show an error for 'Are you currently in the process of applying for SSI or SSDI'?.
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    expect(errorDetailMsgs.indexOf('Are you currently in the process of applying for SSI or SSDI?')).toBeGreaterThan(-1);
  });

  it('Are you currently in the process of applying for SSI or SSDI is not required sometimes.', () => {
    // Given: A Family Barriers section with 'Have you ever applied for SSI or SSDI?' set to No
    model.hasEverAppliedSsi = false;
    const canAccessFamilyBarriersSsi = false;

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, dp, canAccessFamilyBarriersSsi);

    // Then: The section does not show an error for 'Are you currently in the process of applying for SSI or SSDI?'.
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    expect(errorDetailMsgs.indexOf('Are you currently in the process of applying for SSI or SSDI?')).toBe(-1);
  });
});
