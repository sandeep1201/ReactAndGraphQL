// tslint:disable: deprecation
import { LogService } from '../../shared/services/log.service';
import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
import { Router } from '@angular/router';
import { AppService } from './../../core/services/app.service';
import { PostSecondaryEducationSection, PostSecondaryCollege, PostSecondaryDegree, PostSecondaryLicense } from '../../shared/models/post-secondary-education-section';
import { ValidationManager } from '../../shared/models/validation-manager';
import { ValidationCode } from '../../shared/models/validation-error';
import { empty } from 'rxjs';
import {} from 'jasmine';
import * as TypeMoq from 'typemoq';
import * as moment from 'moment';
import { TestingUtilities } from './testing-utilities';
import { GoogleLocation } from '../../shared/models/google-location';
import { JwtAuthConfig } from 'src/app/core/jwt-auth-config';
import { Location } from '@angular/common';

// Describe the test suite and initialize the testing environment
describe('PostSecondaryEducationSection: Validate', () => {
  const router = TypeMoq.Mock.ofType<Router>();
  const logServiceMoq = TypeMoq.Mock.ofType<LogService>();
  const jwtMoq = TypeMoq.Mock.ofType<JwtAuthConfig>();
  const location = TypeMoq.Mock.ofType<Location>();
  router.setup(x => x.events).returns(() => empty());

  let appService: AppService;
  const authHttpClientMock: TypeMoq.IMock<AuthHttpClient> = TypeMoq.Mock.ofType<AuthHttpClient>();
  let validationManager: ValidationManager;
  const participantDOB: moment.Moment = moment('01/01/1970', 'MM/DD/YYYY');

  let model = new PostSecondaryEducationSection();

  // Initialize a new AppService and ValidationManager before each test
  beforeEach(() => {
    appService = new AppService(authHttpClientMock.object, logServiceMoq.object, router.object, jwtMoq.object, location.object);
    validationManager = new ValidationManager(appService);

    // Start with a fresh model for each test.
    model = new PostSecondaryEducationSection();
  });

  it('PostSecondaryEducation BR 4.1 Colleges & Universities – Yes or No', () => {
    // Given: A PostSecondaryEducation with hasAttendedCollege = null.

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is not valid because hasAttendedCollege must not be true or false.
    expect(errorDetailMsgs.includes('Have you attended, or are you currently attending a college or university?')).toEqual(true);
    expect(result.isValid).toEqual(false);
  });

  it('PostSecondaryEducation BR 4.1.1 Colleges & Universities – Location', () => {
    // Given: A PostSecondaryEducation with hasAttendedCollege = true and a PostSecondaryCollege without a valid GoogleLocation.
    model.hasAttendedCollege = true;
    const college = PostSecondaryCollege.create();
    model.postSecondaryColleges = new Array<PostSecondaryCollege>();
    model.postSecondaryColleges.push(college);

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is not valid because model.postSecondaryColleges must contain a valid GoogleLocation.
    expect(errorDetailMsgs.includes('College/University Location')).toEqual(true);
    expect(result.isValid).toEqual(false);
  });

  it('PostSecondaryEducation 4.1.2 Colleges & Universities – Name', () => {
    // Given: A PostSecondaryEducation with hasAttendedCollege = true and a PostSecondaryCollege with a valid GoogleLocation.
    model.hasAttendedCollege = true;
    const college = PostSecondaryCollege.create();
    college.location = new GoogleLocation();
    college.location.city = 'Madison, WI, United States';
    model.postSecondaryColleges = new Array<PostSecondaryCollege>();
    model.postSecondaryColleges.push(college);

    // When the section is validated.
    const result = model.validate(validationManager, participantDOB);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    //Then: The section is not valid because PostSecondaryCollege must have a valid name.
    expect(errorDetailMsgs.includes('College/University Name')).toEqual(true);
    expect(result.isValid).toEqual(false);
  });

  it('PostSecondaryEducation 4.1.3 Colleges & Universities – Graduated', () => {
    // Given: A PostSecondaryEducation with a PostSecondaryCollege where hasGruaduated = null.
    model.hasAttendedCollege = true;
    const college = PostSecondaryCollege.create();
    college.location = new GoogleLocation();
    college.location.city = 'Madison, WI, United States';
    college.name = 'Madison Area Technical College';
    model.postSecondaryColleges = new Array<PostSecondaryCollege>();
    model.postSecondaryColleges.push(college);

    // When the section is validated.
    const result = model.validate(validationManager, participantDOB);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    //Then: The section is not valid because PostSecondaryCollege hasGraduated must be true or false.
    expect(errorDetailMsgs.includes('College/University Graduated')).toEqual(true);
    expect(result.isValid).toEqual(false);
  });

  it('PostSecondaryEducation 4.2 Degrees – Yes or No', () => {
    // Given: A PostSecondaryEducation with a valid PostSecondaryCollege where hasDegree = null
    model.hasAttendedCollege = true;
    const college = PostSecondaryCollege.create();
    college.location = new GoogleLocation();
    college.location.city = 'Madison, WI, United States';
    college.name = 'Madison Area Technical College';
    college.hasGraduated = true;
    model.postSecondaryColleges = new Array<PostSecondaryCollege>();
    model.postSecondaryColleges.push(college);

    // When the section is validated.
    const result = model.validate(validationManager, participantDOB);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section will not be valid because hasDegrees must be true or false.
    expect(errorDetailMsgs.includes('Do you have any degrees?')).toEqual(true);
    expect(result.isValid).toEqual(false);
  });

  it('PostSecondaryEducation 4.2.1 Degrees – Degree Name', () => {
    // Given: A PostSecondaryEducation with a valid PostSecondaryCollege and hasDegree = true
    model.hasAttendedCollege = true;
    const college = PostSecondaryCollege.create();
    college.location = new GoogleLocation();
    college.location.city = 'Madison, WI, United States';
    college.name = 'Madison Area Technical College';
    college.hasGraduated = true;
    model.postSecondaryColleges = new Array<PostSecondaryCollege>();
    model.postSecondaryColleges.push(college);

    model.hasDegree = true;
    const degree = PostSecondaryDegree.create();
    model.postSecondaryDegrees = new Array<PostSecondaryDegree>();
    model.postSecondaryDegrees.push(degree);

    // When: The section is validated
    const result = model.validate(validationManager, participantDOB);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is not valid because degree.name must have a valid name.
    expect(errorDetailMsgs.includes('Degree Name')).toEqual(true);
    expect(result.isValid).toEqual(false);
  });

  it('PostSecondaryEducation 4.2.2 Degrees – Degree Level', () => {
    // Given: A PostSecondaryEducation with hasAttendedCollege = true and a PostSecondaryCollege with a valid GoogleLocation.
    model.hasAttendedCollege = true;
    const college = PostSecondaryCollege.create();
    college.location = new GoogleLocation();
    college.location.city = 'Madison, WI, United States';
    model.postSecondaryColleges = new Array<PostSecondaryCollege>();
    model.postSecondaryColleges.push(college);

    // When the section is validated.
    const result = model.validate(validationManager, participantDOB);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    //Then: The section is not valid because PostSecondaryCollege must have a valid name.
    expect(errorDetailMsgs.includes('College/University Name')).toEqual(true);
    expect(result.isValid).toEqual(false);
  });

  it('PostSecondaryEducation 4.1.3 Colleges & Universities – Graduated', () => {
    // Given: A PostSecondaryEducation with a PostSecondaryCollege where hasGruaduated = null.
    model.hasAttendedCollege = true;
    const college = PostSecondaryCollege.create();
    college.location = new GoogleLocation();
    college.location.city = 'Madison, WI, United States';
    college.name = 'Madison Area Technical College';
    model.postSecondaryColleges = new Array<PostSecondaryCollege>();
    model.postSecondaryColleges.push(college);

    // When the section is validated.
    const result = model.validate(validationManager, participantDOB);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    //Then: The section is not valid because PostSecondaryCollege hasGraduated must be true or false.
    expect(errorDetailMsgs.includes('College/University Graduated')).toEqual(true);
    expect(result.isValid).toEqual(false);
  });

  it('PostSecondaryEducation 4.2 Degrees – Yes or No', () => {
    // Given: A PostSecondaryEducation with a valid PostSecondaryCollege where hasDegree = null
    model.hasAttendedCollege = true;
    const college = PostSecondaryCollege.create();
    college.location = new GoogleLocation();
    college.location.city = 'Madison, WI, United States';
    college.name = 'Madison Area Technical College';
    college.hasGraduated = true;
    model.postSecondaryColleges = new Array<PostSecondaryCollege>();
    model.postSecondaryColleges.push(college);

    // When the section is validated.
    const result = model.validate(validationManager, participantDOB);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section will not be valid because hasDegrees must be true or false.
    expect(errorDetailMsgs.includes('Do you have any degrees?')).toEqual(true);
    expect(result.isValid).toEqual(false);
  });

  it('PostSecondaryEducation 4.2.1 Degrees – Degree Name', () => {
    // Given: A PostSecondaryEducation with a valid PostSecondaryCollege and hasDegree = true
    model.hasAttendedCollege = true;
    const college = PostSecondaryCollege.create();
    college.location = new GoogleLocation();
    college.location.city = 'Madison, WI, United States';
    college.name = 'Madison Area Technical College';
    college.hasGraduated = true;
    model.postSecondaryColleges = new Array<PostSecondaryCollege>();
    model.postSecondaryColleges.push(college);

    model.hasDegree = true;
    const degree = PostSecondaryDegree.create();
    model.postSecondaryDegrees = new Array<PostSecondaryDegree>();
    model.postSecondaryDegrees.push(degree);

    // When: The section is validated
    const result = model.validate(validationManager, participantDOB);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is not valid because degree.name must have a valid name.
    expect(errorDetailMsgs.includes('Degree Name')).toEqual(true);
    expect(result.isValid).toEqual(false);
  });

  it('PostSecondaryEducation 4.2.2 Degrees – Degree Level', () => {
    // Given: A PostSecondaryEducation with a valid PostSecondaryCollege and hasDegree = true
    model.hasAttendedCollege = true;
    const college = PostSecondaryCollege.create();
    college.location = new GoogleLocation();
    college.location.city = 'Madison, WI, United States';
    college.name = 'Madison Area Technical College';
    college.hasGraduated = true;
    model.postSecondaryColleges = new Array<PostSecondaryCollege>();
    model.postSecondaryColleges.push(college);

    model.hasDegree = true;
    const degree = PostSecondaryDegree.create();
    degree.name = 'Master of Business Administration';
    model.postSecondaryDegrees = new Array<PostSecondaryDegree>();
    model.postSecondaryDegrees.push(degree);

    // When: The section is validated
    const result = model.validate(validationManager, participantDOB);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is not valid because degree.type must be selected.
    expect(errorDetailMsgs.includes('Degree Level')).toEqual(true);
    expect(result.isValid).toEqual(false);
  });

  it('PostSecondaryEducation 4.2.3 Degrees – Degrees College', () => {
    // Given: A PostSecondaryEducation with a valid PostSecondaryCollege and hasDegree = true
    model.hasAttendedCollege = true;
    const college = PostSecondaryCollege.create();
    college.location = new GoogleLocation();
    college.location.city = 'Madison, WI, United States';
    college.name = 'Madison Area Technical College';
    college.hasGraduated = true;
    model.postSecondaryColleges = new Array<PostSecondaryCollege>();
    model.postSecondaryColleges.push(college);

    model.hasDegree = true;
    const degree = PostSecondaryDegree.create();
    degree.name = 'Master of Business Administration';
    degree.type = 1;
    model.postSecondaryDegrees = new Array<PostSecondaryDegree>();
    model.postSecondaryDegrees.push(degree);

    // When: The section is validated
    const result = model.validate(validationManager, participantDOB);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is not valid because degree.college must be selected.
    expect(errorDetailMsgs.includes('Degree College')).toEqual(true);
    expect(result.isValid).toEqual(false);
  });

  it('PostSecondaryEducation BR 4.3 Licenses & Certificates – Yes or No', () => {
    // Given: A PostSecondaryEducation with isWorkingOnLicensesOrCertificates = null.

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is not valid because isWorkingOnLicensesOrCertificates must not be true or false.
    expect(errorDetailMsgs.includes('Do you have or are you working towards any licenses or certificates?')).toEqual(true);
    expect(result.isValid).toEqual(false);
  });

  it('PostSecondaryEducation BR 4.3.1 Licenses & Certificates – Type', () => {
    // Given: A PostSecondaryEducation with isWorkingOnLicensesOrCertificates = true.
    model.isWorkingOnLicensesOrCertificates = true;
    const license = PostSecondaryLicense.create();
    model.postSecondaryLicenses = new Array<PostSecondaryLicense>();
    model.postSecondaryLicenses.push(license);

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is not valid because license.licenseType must be selected.
    expect(errorDetailMsgs.includes('License Type')).toEqual(true);
    expect(result.isValid).toEqual(false);
  });

  it('PostSecondaryEducation BR 4.3.2 Licenses & Certificates – Name', () => {
    // Given: A PostSecondaryEducation with isWorkingOnLicensesOrCertificates = true.
    model.isWorkingOnLicensesOrCertificates = true;
    const license = PostSecondaryLicense.create();
    license.licenseType = 1;
    model.postSecondaryLicenses = new Array<PostSecondaryLicense>();
    model.postSecondaryLicenses.push(license);

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is not valid because license.licenseName must be selected.
    expect(errorDetailMsgs.includes('License Name')).toEqual(true);
    expect(result.isValid).toEqual(false);
  });

  it('PostSecondaryEducation BR 4.3.3 Licenses & Certificates – Licenses & Certificates – Valid in WI', () => {
    // Given: A PostSecondaryEducation with isWorkingOnLicensesOrCertificates = true.
    model.isWorkingOnLicensesOrCertificates = true;
    const license = PostSecondaryLicense.create();
    license.licenseType = 1;
    license.licenseTypeName = 'MCSE';
    model.postSecondaryLicenses = new Array<PostSecondaryLicense>();
    model.postSecondaryLicenses.push(license);

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is not valid because license.isValidInWIValid must have a value.
    expect(errorDetailMsgs.includes('License Valid in WI')).toEqual(true);
    expect(result.isValid).toEqual(false);
  });

  it('PostSecondaryEducation BR 4.3.4  Licenses & Certificates – Licenses & Certificates – Valid in WI', () => {
    // Given: A PostSecondaryEducation with isWorkingOnLicensesOrCertificates = true.
    model.isWorkingOnLicensesOrCertificates = true;
    const license = PostSecondaryLicense.create();
    license.licenseType = 1;
    license.licenseTypeName = 'MCSE';
    license.validInWi = 'Yes';
    model.postSecondaryLicenses = new Array<PostSecondaryLicense>();
    model.postSecondaryLicenses.push(license);

    // When: The section is validated.
    const result = model.validate(validationManager, participantDOB);
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is not valid because license.issuer must have a value.
    expect(errorDetailMsgs.includes('License Issuer')).toEqual(true);
    expect(result.isValid).toEqual(false);
  });
});
