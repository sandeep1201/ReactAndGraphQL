// tslint:disable: deprecation
import { LogService } from '../../shared/services/log.service';
import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
import { Router } from '@angular/router';
import { AppService } from './../../core/services/app.service';
import { ActionNeeded } from '../../features-modules/actions-needed/models/action-needed-new';
import { DropDownField } from './../../shared/models/dropdown-field';
import { TransportationSection } from '../../shared/models/transportation-section';
import { TestingUtilities } from './testing-utilities';
import { ValidationManager } from '../../shared/models/validation-manager';
import { ValidationCode } from '../../shared/models/validation-error';
import { empty } from 'rxjs';
import * as TypeMoq from 'typemoq';
import * as moment from 'moment';
import { JwtAuthConfig } from 'src/app/core/jwt-auth-config';
import { Utilities } from 'src/app/shared/utilities';
import { Location } from '@angular/common';

describe('TransportationSection: Validate', () => {
  const participantDOB: string = moment('03/29/1991', 'MM/DD/YYYY').format('MM/DD/YYYY');
  const currentDate: string = moment(Utilities.currentDate).format('MM/DD/YYYY');

  const statuses = ['Suspended/Revoked', 'Expired', 'Cancelled', 'Denied', 'Never Applied for a License', 'Other'];
  const transTypes = ['Personal Vehicle', 'Borrowed Vehicle', 'Public Transit', 'Taxi/Rental', 'Bike/Rental', 'Family/Friend', 'Other', 'None Available'];

  const statusesDrop = statuses.map(function(name, index) {
    const dpi = new DropDownField();
    dpi.id = index;
    dpi.name = name;
    return dpi;
  });

  const transTypeDrop = transTypes.map(function(name, index) {
    const dpi = new DropDownField();
    dpi.id = index + 1;
    dpi.name = name;
    return dpi;
  });

  const router = TypeMoq.Mock.ofType<Router>();
  const logServiceMoq = TypeMoq.Mock.ofType<LogService>();
  const jwtMoq = TypeMoq.Mock.ofType<JwtAuthConfig>();
  const location = TypeMoq.Mock.ofType<Location>();

  router.setup(x => x.events).returns(() => empty());
  let appService: AppService;
  const authHttpClientMock: TypeMoq.IMock<AuthHttpClient> = TypeMoq.Mock.ofType<AuthHttpClient>();
  let validationManager: ValidationManager;

  let model = new TransportationSection();

  beforeEach(() => {
    appService = new AppService(authHttpClientMock.object, logServiceMoq.object, router.object, jwtMoq.object, location.object);

    validationManager = new ValidationManager(appService);
    model = new TransportationSection();
    const actionNeeded = new ActionNeeded();
    model.actionNeeded = actionNeeded;
    model.actionNeeded.isNoActionNeeded = true;
  });

  it('Empty section is not valid.', () => {
    // Given: An empty Transportation section.
    validationManager.resetErrors();
    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, transTypeDrop, statusesDrop);

    // Then: The section is not valid.
    expect(result.isValid).toEqual(false);

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    expect(errorDetailMsgs.length).toEqual(3);
  });

  it('Empty section is not valid but action needed is set.', () => {
    // Given: An empty Transportation section.
    validationManager.resetErrors();

    const actionNeeded = new ActionNeeded();
    actionNeeded.isNoActionNeeded = true;
    model.actionNeeded = actionNeeded;
    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, transTypeDrop, statusesDrop);

    // Then: The section is not valid.
    expect(result.isValid).toEqual(false);

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    expect(errorDetailMsgs.length).toEqual(3);
  });

  it('Validation test for cdl false.', () => {
    // Given: An empty Transportation section.
    validationManager.resetErrors();

    const actionNeeded = new ActionNeeded();
    actionNeeded.isNoActionNeeded = true;
    model.actionNeeded = actionNeeded;
    model.methods = [];
    model.methods.push(1);
    model.isVehicleInsuredId = 2;
    model.isVehicleRegistrationCurrentId = 2;
    model.hasValidDriversLicense = true;
    model.hadCommercialDriversLicense = false;
    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, transTypeDrop, statusesDrop);

    // Then: The section is not valid.
    expect(result.isValid).toEqual(false);

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    expect(errorDetailMsgs.length).toEqual(2);

    expect(errorDetailMsgs.indexOf('State Issued')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Expiration Date Month and Year')).toBeGreaterThan(-1);
  });

  it('Validation test for cdl true.', () => {
    // Given: An empty Transportation section.
    validationManager.resetErrors();

    const actionNeeded = new ActionNeeded();
    actionNeeded.isNoActionNeeded = true;
    model.actionNeeded = actionNeeded;
    model.methods = [];
    model.methods.push(1);
    model.isVehicleInsuredId = 2;
    model.isVehicleRegistrationCurrentId = 2;
    model.hasValidDriversLicense = true;
    model.hadCommercialDriversLicense = true;
    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, transTypeDrop, statusesDrop);

    // Then: The section is not valid.
    expect(result.isValid).toEqual(false);

    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    expect(errorDetailMsgs.length).toEqual(4);

    expect(errorDetailMsgs.indexOf('State Issued')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Expiration Date Month and Year')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Is your CDL still active?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('What kind of vehicles have you operated in the past?')).toBeGreaterThan(-1);
  });

  it('Validation all required fields.', () => {
    // Given: An empty Transportation section.
    validationManager.resetErrors();

    const actionNeeded = new ActionNeeded();
    actionNeeded.isNoActionNeeded = true;
    model.actionNeeded = actionNeeded;
    model.methods = [];
    model.methods.push(1);
    model.isVehicleInsuredId = 1;
    model.isVehicleRegistrationCurrentId = 1;
    model.hasValidDriversLicense = true;
    model.isCommercialDriversLicenseActive = true;
    model.driversLicenseStateId = 4;
    model.driversLicenseExpirationDateMmDdYyyy = currentDate;
    model.hadCommercialDriversLicense = true;
    model.commercialDriversLicenseDetails = 'no';

    // When: The section is validated.
    const result = model.validate(validationManager, currentDate, transTypeDrop, statusesDrop);

    // Then: The section is not valid.
    expect(result.isValid).toEqual(true);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    expect(errorDetailMsgs.length).toEqual(0);
  });

  it('Expiration Date Month and Year validation.', () => {
    // Given: An empty Transportation section.
    validationManager.resetErrors();

    const actionNeeded = new ActionNeeded();
    actionNeeded.isNoActionNeeded = true;
    model.actionNeeded = actionNeeded;
    model.methods = [];
    model.methods.push(1);
    model.isVehicleInsuredId = 1;
    model.isVehicleRegistrationCurrentId = 1;
    model.hasValidDriversLicense = true;
    model.isCommercialDriversLicenseActive = true;
    model.driversLicenseStateId = 4;
    model.driversLicenseExpirationDateMmDdYyyy = '01/01/1800';
    model.hadCommercialDriversLicense = true;
    model.commercialDriversLicenseDetails = 'no';

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB, transTypeDrop, statusesDrop);

    // Then: The section is not valid.
    expect(result.isValid).toEqual(false);
  });
});
