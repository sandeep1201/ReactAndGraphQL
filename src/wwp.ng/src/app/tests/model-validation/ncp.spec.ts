// tslint:disable:no-unused-variable
// tslint:disable: deprecation
import { LogService } from '../../shared/services/log.service';
import { Router } from '@angular/router';
import * as moment from 'moment';
import * as TypeMoq from 'typemoq';
import {} from 'jasmine';
import { AppService } from './../../core/services/app.service';
import { ActionNeeded } from '../../features-modules/actions-needed/models/action-needed-new';
import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
import { NonCustodialParentsSection, NonCustodialCaretaker, NonCustodialChild } from '../../shared/models/non-custodial-parents-section';
import { ValidationManager } from '../../shared/models/validation-manager';
import { ValidationCode } from '../../shared/models/validation-error';
import { TestingUtilities } from './testing-utilities';
import { empty } from 'rxjs';
import { JwtAuthConfig } from 'src/app/core/jwt-auth-config';
import { Location } from '@angular/common';

describe('NonCustodialParentsEditComponent: Validate', () => {
  const router = TypeMoq.Mock.ofType<Router>();
  const logServiceMoq = TypeMoq.Mock.ofType<LogService>();
  const jwtMoq = TypeMoq.Mock.ofType<JwtAuthConfig>();
  const location = TypeMoq.Mock.ofType<Location>();

  router.setup(x => x.events).returns(() => empty());

  let appService: AppService;
  const authHttpClientMock: TypeMoq.IMock<AuthHttpClient> = TypeMoq.Mock.ofType<AuthHttpClient>();
  let validationManager: ValidationManager;
  const participantDOB: moment.Moment = moment('03/12/1999', 'MM/DD/YYYY');

  const polarDropdown = TestingUtilities.getDropDownByName('polarUnknown');
  const relationshipDropdown = TestingUtilities.getDropDownByName('relationshipNCP');
  const contactIntervalDropdown = TestingUtilities.getDropDownByName('contactInterval');

  let model: NonCustodialParentsSection;

  beforeEach(() => {
    appService = new AppService(authHttpClientMock.object, logServiceMoq.object, router.object, jwtMoq.object, location.object);
    validationManager = new ValidationManager(appService);

    // Start with a fresh model for each test.
    model = new NonCustodialParentsSection();
    const caretaker = new NonCustodialCaretaker();
    model.nonCustodialCaretakers = [];
    model.nonCustodialCaretakers.push(caretaker);
    const child = new NonCustodialChild();
    model.nonCustodialCaretakers[0].nonCustodialChilds = [];
    model.nonCustodialCaretakers[0].nonCustodialChilds.push(child);
    model.actionNeeded = new ActionNeeded();
    model.actionNeeded.tasks = [];
    model.actionNeeded.isNoActionNeeded = true;
  });

  it('US 1084 - NCP: Validation.', () => {
    // Given: Page is blank

    // When: The section is validated.
    const result = model.validate(validationManager, polarDropdown, relationshipDropdown, contactIntervalDropdown, participantDOB.format('MM/DD/YYYY'));
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is not valid.
    expect(errorDetailMsgs.indexOf('Do you have any children under the age of 18 who live with another individual most of the time?')).toBeGreaterThan(-1);

    expect(errorDetailMsgs.length).toEqual(1);
    expect(result.isValid).toEqual(false);
  });

  it('US 1084 - NCP: Validation.', () => {
    // Given: Do you have any children under the age of 18 who live with another individual most of the time? is set to Yes.
    model.hasChildren = true;

    // When: The section is validated.
    const result = model.validate(validationManager, polarDropdown, relationshipDropdown, contactIntervalDropdown, participantDOB.format('MM/DD/YYYY'));
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is not valid.
    expect(errorDetailMsgs.indexOf('First Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Last Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Relationship to Child(ren)')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('How often do you have contact with this person?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Is there anything you would like to change about your relationship with this person?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('DOB')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Child support order?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('How often do you have contact with this child?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('How often do you have contact with this child? - Details')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Are there other adults caring for this child?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Is there anything you would like to change about your relationship with this child?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Is the child in need of any services?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Are you interested in getting a referral for services to help with parenting or visitation?')).toBeGreaterThan(-1);

    expect(errorDetailMsgs.length).toEqual(13);
    expect(result.isValid).toEqual(false);
  });

  it('US 1084 - NCP: Validation.', () => {
    // Given: Do you have any children under the age of 18 who live with another individual most of the time? is set to Yes - Action Needs Selected.
    model.hasChildren = true;
    model.actionNeeded.isNoActionNeeded = true;

    // When: The section is validated.
    const result = model.validate(validationManager, polarDropdown, relationshipDropdown, contactIntervalDropdown, participantDOB.format('MM/DD/YYYY'));
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is not valid.
    expect(errorDetailMsgs.indexOf('First Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Last Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Relationship to Child(ren)')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('How often do you have contact with this person?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Is there anything you would like to change about your relationship with this person?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('DOB')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Child support order?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('How often do you have contact with this child?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('How often do you have contact with this child? - Details')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Are there other adults caring for this child?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Is there anything you would like to change about your relationship with this child?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Is the child in need of any services?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Are you interested in getting a referral for services to help with parenting or visitation?')).toBeGreaterThan(-1);

    expect(errorDetailMsgs.length).toEqual(13);
    expect(result.isValid).toEqual(false);
  });

  it('US 1084 - NCP: Validation.', () => {
    // Given: Do you have any children over the age of 18 who lives with another individual most of the time? is set to Yes - Action Needs Selected.
    model.hasChildren = true;
    model.nonCustodialCaretakers[0].nonCustodialChilds[0].dateOfBirth = moment()
      .subtract(29, 'year')
      .format('MM/DD/YYYY');
    model.actionNeeded.isNoActionNeeded = true;

    // When: The section is validated.
    const result = model.validate(validationManager, polarDropdown, relationshipDropdown, contactIntervalDropdown, participantDOB.format('MM/DD/YYYY'));
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: The section is not valid.
    expect(errorDetailMsgs.indexOf('First Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Last Name')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Relationship to Child(ren)')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('How often do you have contact with this person?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Is there anything you would like to change about your relationship with this person?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Child support order?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('How often do you have contact with this child?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('How often do you have contact with this child? - Details')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Are there other adults caring for this child?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Is there anything you would like to change about your relationship with this child?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Is the child in need of any services?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Are you interested in getting a referral for services to help with parenting or visitation?')).toBeGreaterThan(-1);

    expect(errorDetailMsgs.length).toEqual(12);
    expect(result.isValid).toEqual(false);
  });

  it('US 1084 - NCP: Validation.', () => {
    // Given: Page is Valid.
    model.hasChildren = true;
    model.nonCustodialCaretakers[0].firstName = 'Bob';
    model.nonCustodialCaretakers[0].lastName = 'Ross';
    model.nonCustodialCaretakers[0].nonCustodialParentRelationshipId = 1;
    model.nonCustodialCaretakers[0].contactIntervalId = 1;
    model.nonCustodialCaretakers[0].isRelationshipChangeRequested = false;
    model.nonCustodialCaretakers[0].nonCustodialChilds[0].firstName = 'Rob';
    model.nonCustodialCaretakers[0].nonCustodialChilds[0].lastName = 'Ross';
    model.nonCustodialCaretakers[0].nonCustodialChilds[0].hasChildSupportOrder = false;
    model.nonCustodialCaretakers[0].nonCustodialChilds[0].dateOfBirth = moment()
      .subtract(9, 'year')
      .format('MM/DD/YYYY');
    model.nonCustodialCaretakers[0].nonCustodialChilds[0].contactIntervalId = 1;
    model.nonCustodialCaretakers[0].nonCustodialChilds[0].contactIntervalDetails = 'Contact details';
    model.nonCustodialCaretakers[0].nonCustodialChilds[0].hasOtherAdultsPolarLookupId = 2;
    model.nonCustodialCaretakers[0].nonCustodialChilds[0].isRelationshipChangeRequested = false;
    model.nonCustodialCaretakers[0].nonCustodialChilds[0].isNeedOfServicesPolarLookupId = 2;
    model.isInterestedInReferralServices = false;
    model.actionNeeded.isNoActionNeeded = true;

    // When: The section is validated.
    const result = model.validate(validationManager, polarDropdown, relationshipDropdown, contactIntervalDropdown, participantDOB.format('MM/DD/YYYY'));
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Then section is valid.
    expect(errorDetailMsgs.length).toEqual(0);
    expect(result.isValid).toEqual(true);
  });

  it('US 1084 - NCP: Validation.', () => {
    // Given: Child has support order but pay amount, back child support, interest in child support obligations
    // and interst in referral with visitation questions are not filled.
    model.hasChildren = true;
    model.nonCustodialCaretakers[0].firstName = 'Bob';
    model.nonCustodialCaretakers[0].lastName = 'Ross';
    model.nonCustodialCaretakers[0].nonCustodialParentRelationshipId = 1;
    model.nonCustodialCaretakers[0].contactIntervalId = 1;
    model.nonCustodialCaretakers[0].isRelationshipChangeRequested = false;
    model.nonCustodialCaretakers[0].nonCustodialChilds[0].firstName = 'Rob';
    model.nonCustodialCaretakers[0].nonCustodialChilds[0].lastName = 'Ross';
    model.nonCustodialCaretakers[0].nonCustodialChilds[0].hasChildSupportOrder = true;
    model.nonCustodialCaretakers[0].nonCustodialChilds[0].dateOfBirth = moment()
      .subtract(9, 'year')
      .format('MM/DD/YYYY');
    model.nonCustodialCaretakers[0].nonCustodialChilds[0].contactIntervalId = 1;
    model.nonCustodialCaretakers[0].nonCustodialChilds[0].contactIntervalDetails = 'Contact details';
    model.nonCustodialCaretakers[0].nonCustodialChilds[0].hasOtherAdultsPolarLookupId = 2;
    model.nonCustodialCaretakers[0].nonCustodialChilds[0].isRelationshipChangeRequested = false;
    model.nonCustodialCaretakers[0].nonCustodialChilds[0].isNeedOfServicesPolarLookupId = 2;
    model.actionNeeded.isNoActionNeeded = true;

    // When: The section is validated.
    const result = model.validate(validationManager, polarDropdown, relationshipDropdown, contactIntervalDropdown, participantDOB.format('MM/DD/YYYY'));
    const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

    // Then: Then section is not valid.
    expect(errorDetailMsgs.indexOf('How much child support are you ordered to pay each month?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Do you owe any back child support?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Are you interested in getting a referral for services to help you understand your child support obligations?')).toBeGreaterThan(-1);
    expect(errorDetailMsgs.indexOf('Are you interested in getting a referral for services to help with parenting or visitation?')).toBeGreaterThan(-1);

    expect(errorDetailMsgs.length).toEqual(4);
    expect(result.isValid).toEqual(false);
  });
});
