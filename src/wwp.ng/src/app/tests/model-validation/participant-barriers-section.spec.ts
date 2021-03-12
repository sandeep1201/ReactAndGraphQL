// /* tslint:disable:no-unused-variable */

// import { BaseRequestOptions, Http } from '@angular/http';
// import { Router } from '@angular/router';
// import { RouterTestingModule } from '@angular/router/testing';
// import { TestBed, async, inject } from '@angular/core/testing';
// import { MockBackend, MockConnection } from '@angular/http/testing';

// import * as moment from 'moment';
// import * as TypeMoq from 'typemoq';

// import { AppService } from './../../core/services/app.service';
//import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
// import { ParticipantBarriersSection } from '../../shared/models/participant-barriers-section';
// import { GoogleLocation } from '../../shared/models/google-location';
// import { JobActionType } from '../../shared/models/job-actions';

// import { ValidationManager } from '../../shared/models/validation-manager';
// import { ValidationError, ValidationCode } from '../../shared/models/validation-error';
// import { TestingUtilities } from './testing-utilities';
// import { YesNoRefused } from '../../shared/models/primitives';

// const mockHttpProvider = {
//     deps: [MockBackend, BaseRequestOptions],
//     useFactory: (backend: MockBackend, defaultOptions: BaseRequestOptions) => {
//         return new Http(backend, defaultOptions);
//     }
// };

// describe('Participant-barriersEditComponent: Validate', () => {
//     let router: Router;
//     let appService: AppService;
//     const authHttpClientMock: TypeMoq.IMock<AuthHttpClient> = TypeMoq.Mock.ofType<AuthHttpClient>();
//     let validationManager: ValidationManager;

//     let model: ParticipantBarriersSection;

//     beforeEach(() => {
//         router = jasmine.createSpyObj('Router', ['navigate']);
//         appService = new AppService(authHttpClientMock.object, router, null);
//         validationManager = new ValidationManager(appService);

//         // Start with a fresh model for each test.
//         model = new ParticipantBarriersSection();
//         const srd = new YesNoRefused();
//         model.isPhysicalHealthHardToManage = srd;

//         const srd2 = new YesNoRefused();
//         model.isPhysicalHealthHardToParticipate = srd2;

//         const srd3 = new YesNoRefused();
//         model.isMentalHealthDiagnosed = srd3;

//         const srd4 = new YesNoRefused();
//         model.isMentalHealthHardToManage = srd4;

//         const srd5 = new YesNoRefused();
//         model.isMentalHealthHardToParticipate = srd5;

//         const srd6 = new YesNoRefused();
//         model.isAodaHardToManage = srd6;

//         const srd7 = new YesNoRefused();
//         model.isAodaHardToParticipate = srd7;

//         const srd8 = new YesNoRefused();
//         model.isLearningDisabilityDiagnosed = srd8;

//         const srd9 = new YesNoRefused();
//         model.isLearningDisabilityHardToManage = srd9;

//         const srd10 = new YesNoRefused();
//         model.isLearningDisabilityHardToParticipate = srd10;

//         const srd11 = new YesNoRefused();
//         model.isDomesticViolenceFamilySafety = srd11;

//         const srd12 = new YesNoRefused();
//         model.isDomesticViolenceHouseholdSupportive = srd12;

//         const srd13 = new YesNoRefused();
//         model.isDomesticViolenceHouseholdPreventive = srd13;

//         // it('Empty section is not valid.', () => {
//         //     // Given: An empty Participant Barriers section.

//         //     // When: The section is validated.
//         //     const result = model.validate(validationManager);

//         //     // Then: The section is not valid.
//         //     console.log('This code works');
//         //     expect(result.isValid).toEqual(false);
//         // });

//         // it('status is true and details are null.', () => {
//         //     // Given: An empty details when status is yes.
//         //     let srd = new YesNoRefused();
//         //     model.isPhysicalHealthHardToManage = srd;
//         //     model.isPhysicalHealthHardToManage.status = true ;
//         //     model.isPhysicalHealthHardToManage.refused = false;
//         //     model.isPhysicalHealthHardToManage.details = null;

//         //     // When: The section is validated.
//         //     const result = model.validate(validationManager);

//         //     // Then: The section is not valid.
//         //     console.log('Code is working!');
//         //     expect(result.isValid).toEqual(false);
//         // });

//         it('Empty section is not valid.', () => {
//             // Given: An empty Participant Barriers section.
//             model.isPhysicalHealthHardToManage.status = null;
//             model.isPhysicalHealthHardToManage.refused = null;
//             model.isPhysicalHealthHardToManage.details = null;

//             model.isPhysicalHealthHardToParticipate.status = null;
//             model.isPhysicalHealthHardToParticipate.refused = null;
//             model.isPhysicalHealthHardToParticipate.details = null;

//             model.isMentalHealthDiagnosed.status = null;
//             model.isMentalHealthDiagnosed.refused = null;
//             model.isMentalHealthDiagnosed.details = null;

//             model.isMentalHealthHardToManage.status = null;
//             model.isMentalHealthHardToManage.refused = null;
//             model.isMentalHealthHardToManage.details = null;

//             model.isMentalHealthHardToParticipate.status = null;
//             model.isMentalHealthHardToParticipate.refused = null;
//             model.isMentalHealthHardToParticipate.details = null;

//             model.isAodaHardToManage.status = null;
//             model.isAodaHardToManage.refused = null;
//             model.isAodaHardToManage.details = null;

//             model.isAodaHardToParticipate.status = null;
//             model.isAodaHardToParticipate.refused = null;
//             model.isAodaHardToParticipate.details = null;

//             model.isLearningDisabilityDiagnosed.status = null;
//             model.isLearningDisabilityDiagnosed.refused = null;
//             model.isLearningDisabilityDiagnosed.details = null;

//             model.isLearningDisabilityHardToManage.status = null;
//             model.isLearningDisabilityHardToManage.refused = null;
//             model.isLearningDisabilityHardToManage.details = null;

//             model.isLearningDisabilityHardToParticipate.status = null;
//             model.isLearningDisabilityHardToParticipate.refused = null;
//             model.isLearningDisabilityHardToParticipate.details = null;

//             // When: The section is validated.
//             const result = model.validate(validationManager);
//             const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

//             // Then: The section is not valid.
//             expect(errorDetailMsgs.indexOf('Do you have any health problems that make it hard to manage your daily life?')).toBeGreaterThan(-1);
//             expect(errorDetailMsgs.indexOf('Do you have concerns that problems with your health will make it hard to participate in work activities?')).toBeGreaterThan(-1);
//             expect(errorDetailMsgs.indexOf('Have you ever met with a counselor or psychiatrist for mental health services or been diagnosed with a mental health condition?')).toBeGreaterThan(-1);
//             expect(errorDetailMsgs.indexOf('Do you have any mental health conditions that make it hard for you to manage your daily life?')).toBeGreaterThan(-1);
//             expect(errorDetailMsgs.indexOf('Do you have concerns that a mental health condition will make it hard for you to participate in work activities?')).toBeGreaterThan(-1);
//             expect(errorDetailMsgs.indexOf('Does alcohol or drug use make it hard for you to manage your daily life?')).toBeGreaterThan(-1);
//             expect(errorDetailMsgs.indexOf('Do you have concerns that alcohol or drug use will make it hard for you to participate in work activities?')).toBeGreaterThan(-1);
//             expect(errorDetailMsgs.indexOf('Did you ever have problems learning in school or have you ever been diagnosed with a learning disability?')).toBeGreaterThan(-1);
//             expect(errorDetailMsgs.indexOf('Do you have learning problems that make it hard to manage your daily life?')).toBeGreaterThan(-1);
//             expect(errorDetailMsgs.indexOf('Do you have concerns that learning problems will make it hard to participate in work activities?')).toBeGreaterThan(-1);
//             expect(errorDetailMsgs.indexOf('Is it safe and appropriate to ask questions about domestic violence?')).toBeGreaterThan(-1);

//             expect(errorDetailMsgs.length).toEqual(11);
//             expect(result.isValid).toEqual(false);
//         });

//         it('True status value in section is valid.', () => {
//             // Given: All checked Yes with details in Participant Barriers section.
//             model.isPhysicalHealthHardToManage.status = true;
//             model.isPhysicalHealthHardToManage.refused = null;
//             model.isPhysicalHealthHardToManage.details = 'Back issues';

//             model.isPhysicalHealthHardToParticipate.status = true;
//             model.isPhysicalHealthHardToParticipate.refused = null;
//             model.isPhysicalHealthHardToParticipate.details = 'Unable to lift more than 5 lbs';

//             model.isPhysicalHealthTakeMedication.status = true;
//             model.isPhysicalHealthTakeMedication.refused = null;
//             model.isPhysicalHealthTakeMedication.details = 'Medication for pain';

//             model.isMentalHealthDiagnosed.status = true;
//             model.isMentalHealthDiagnosed.refused = null;
//             model.isMentalHealthDiagnosed.details = 'diagnosed with depression';

//             model.isMentalHealthHardToManage.status = true;
//             model.isMentalHealthHardToManage.refused = null;
//             model.isMentalHealthHardToManage.details = 'depression';

//             model.isMentalHealthHardToParticipate.status = true;
//             model.isMentalHealthHardToParticipate.refused = null;
//             model.isMentalHealthHardToParticipate.details = 'Unable to concentrate';

//             model.isMentalHealthHardToParticipate.status = true;
//             model.isMentalHealthHardToParticipate.refused = null;
//             model.isMentalHealthHardToParticipate.details = 'medication for depression';

//             model.isAodaHardToManage.status = true;
//             model.isAodaHardToManage.refused = null;
//             model.isAodaHardToManage.details = 'Alcohol';

//             model.isAodaHardToParticipate.status = true;
//             model.isAodaHardToParticipate.refused = null;
//             model.isAodaHardToParticipate.details = 'unable to work';

//             model.isAodaHardToParticipate.status = true;
//             model.isAodaHardToParticipate.refused = null;
//             model.isAodaHardToParticipate.details = 'treatment for alcohol';

//             model.isLearningDisabilityDiagnosed.status = true;
//             model.isLearningDisabilityDiagnosed.refused = null;
//             model.isLearningDisabilityDiagnosed.details = 'Trouble learning in school';

//             model.isLearningDisabilityHardToManage.status = true;
//             model.isLearningDisabilityHardToManage.refused = null;
//             model.isLearningDisabilityHardToManage.details = 'Reading and Writing';

//             model.isLearningDisabilityHardToParticipate.status = true;
//             model.isLearningDisabilityHardToParticipate.refused = null;
//             model.isLearningDisabilityHardToParticipate.details = 'Unable to read and write';

//             model.isSafeAppropriateToAsk = true;
//             model.isDomesticViolenceFamilySafety.status = true;
//             model.isDomesticViolenceFamilySafety.details = 'yes';
//             model.isDomesticViolenceHouseholdSupportive.status = true;
//             model.isDomesticViolenceHouseholdSupportive.details = 'Husband';
//             model.isDomesticViolenceHouseholdPreventive.status = true;
//             model.isDomesticViolenceHouseholdPreventive.details = 'Husband';

//             // When: The section is validated.

//             const result = model.validate(validationManager);
//             const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

//             // Then: The section is valid.

//             expect(errorDetailMsgs.length).toEqual(0);
//             expect(result.isValid).toEqual(true);
//         });

//         it('false status value in section is valid.', () => {
//             // Given: All checked No in Participant Barriers section.
//             model.isPhysicalHealthHardToManage.status = false;
//             model.isPhysicalHealthHardToManage.refused = null;
//             model.isPhysicalHealthHardToManage.details = null;

//             model.isPhysicalHealthHardToParticipate.status = false;
//             model.isPhysicalHealthHardToParticipate.refused = null;
//             model.isPhysicalHealthHardToParticipate.details = null;

//             model.isMentalHealthDiagnosed.status = false;
//             model.isMentalHealthDiagnosed.refused = null;
//             model.isMentalHealthDiagnosed.details = null;

//             model.isMentalHealthHardToManage.status = false;
//             model.isMentalHealthHardToManage.refused = null;
//             model.isMentalHealthHardToManage.details = null;

//             model.isMentalHealthHardToParticipate.status = false;
//             model.isMentalHealthHardToParticipate.refused = null;
//             model.isMentalHealthHardToParticipate.details = null;

//             model.isAodaHardToManage.status = false;
//             model.isAodaHardToManage.refused = null;
//             model.isAodaHardToManage.details = null;

//             model.isAodaHardToParticipate.status = false;
//             model.isAodaHardToParticipate.refused = null;
//             model.isAodaHardToParticipate.details = null;

//             model.isLearningDisabilityDiagnosed.status = false;
//             model.isLearningDisabilityDiagnosed.refused = null;
//             model.isLearningDisabilityDiagnosed.details = null;

//             model.isLearningDisabilityHardToManage.status = false;
//             model.isLearningDisabilityHardToManage.refused = null;
//             model.isLearningDisabilityHardToManage.details = null;

//             model.isLearningDisabilityHardToParticipate.status = false;
//             model.isLearningDisabilityHardToParticipate.refused = null;
//             model.isLearningDisabilityHardToParticipate.details = null;

//             model.isSafeAppropriateToAsk = false;

//             // When: The section is validated.
//             const result = model.validate(validationManager);
//             const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

//             // Then: The section is valid.
//             expect(errorDetailMsgs.length).toEqual(0);
//             expect(result.isValid).toEqual(true);
//         });

//         it('Refused status in section is valid.', () => {
//             // Given: All checked Refused in Participant Barriers section.
//             model.isPhysicalHealthHardToManage.status = null;
//             model.isPhysicalHealthHardToManage.refused = true;
//             model.isPhysicalHealthHardToManage.details = null;

//             model.isPhysicalHealthHardToParticipate.status = null;
//             model.isPhysicalHealthHardToParticipate.refused = true;
//             model.isPhysicalHealthHardToParticipate.details = null;

//             model.isMentalHealthDiagnosed.status = null;
//             model.isMentalHealthDiagnosed.refused = true;
//             model.isMentalHealthDiagnosed.details = null;

//             model.isMentalHealthHardToManage.status = null;
//             model.isMentalHealthHardToManage.refused = true;
//             model.isMentalHealthHardToManage.details = null;

//             model.isMentalHealthHardToParticipate.status = null;
//             model.isMentalHealthHardToParticipate.refused = true;
//             model.isMentalHealthHardToParticipate.details = null;

//             model.isAodaHardToManage.status = null;
//             model.isAodaHardToManage.refused = true;
//             model.isAodaHardToManage.details = null;

//             model.isAodaHardToParticipate.status = null;
//             model.isAodaHardToParticipate.refused = true;
//             model.isAodaHardToParticipate.details = null;

//             model.isLearningDisabilityDiagnosed.status = null;
//             model.isLearningDisabilityDiagnosed.refused = true;
//             model.isLearningDisabilityDiagnosed.details = null;

//             model.isLearningDisabilityHardToManage.status = null;
//             model.isLearningDisabilityHardToManage.refused = true;
//             model.isLearningDisabilityHardToManage.details = null;

//             model.isLearningDisabilityHardToParticipate.status = null;
//             model.isLearningDisabilityHardToParticipate.refused = true;
//             model.isLearningDisabilityHardToParticipate.details = null;

//             model.isSafeAppropriateToAsk = false;

//             // When: The section is validated.
//             const result = model.validate(validationManager);
//             const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

//             // Then: The section is valid.

//             expect(errorDetailMsgs.length).toEqual(0);
//             expect(result.isValid).toEqual(true);
//         });

//         it('All checked Yes with no details in section is invalid.', () => {
//             // Given: All checked yes with no details in Participant Barriers section.
//             model.isPhysicalHealthHardToManage.status = true;
//             model.isPhysicalHealthHardToManage.refused = null;
//             model.isPhysicalHealthHardToManage.details = null;

//             model.isPhysicalHealthHardToParticipate.status = true;
//             model.isPhysicalHealthHardToParticipate.refused = null;
//             model.isPhysicalHealthHardToParticipate.details = null;

//             model.isPhysicalHealthTakeMedication.status = true;
//             model.isPhysicalHealthTakeMedication.refused = null;
//             model.isPhysicalHealthTakeMedication.details = null;

//             model.isMentalHealthDiagnosed.status = true;
//             model.isMentalHealthDiagnosed.refused = null;
//             model.isMentalHealthDiagnosed.details = null;

//             model.isMentalHealthHardToManage.status = true;
//             model.isMentalHealthHardToManage.refused = null;
//             model.isMentalHealthHardToManage.details = null;

//             model.isMentalHealthHardToParticipate.status = true;
//             model.isMentalHealthHardToParticipate.refused = null;
//             model.isMentalHealthHardToParticipate.details = null;

//             model.isAodaHardToManage.status = true;
//             model.isAodaHardToManage.refused = null;
//             model.isAodaHardToManage.details = null;

//             model.isAodaHardToParticipate.status = true;
//             model.isAodaHardToParticipate.refused = null;
//             model.isAodaHardToParticipate.details = null;

//             model.isLearningDisabilityDiagnosed.status = true;
//             model.isLearningDisabilityDiagnosed.refused = null;
//             model.isLearningDisabilityDiagnosed.details = null;

//             model.isLearningDisabilityHardToManage.status = true;
//             model.isLearningDisabilityHardToManage.refused = null;
//             model.isLearningDisabilityHardToManage.details = null;

//             model.isLearningDisabilityHardToParticipate.status = true;
//             model.isLearningDisabilityHardToParticipate.refused = null;
//             model.isLearningDisabilityHardToParticipate.details = null;

//             model.isSafeAppropriateToAsk = true;
//             model.isDomesticViolenceFamilySafety.status = true;
//             model.isDomesticViolenceFamilySafety.details = null;
//             model.isDomesticViolenceHouseholdSupportive.status = true;
//             model.isDomesticViolenceHouseholdSupportive.details = null;
//             model.isDomesticViolenceHouseholdPreventive.status = true;
//             model.isDomesticViolenceHouseholdPreventive.details = null;

//             // When: The section is validated.
//             const result = model.validate(validationManager);
//             const errorDetailMsgs = TestingUtilities.getErrorMessagesByCode(ValidationCode.RequiredInformationMissing_Details, validationManager.errors);

//             // Then: The section is invalid.
//             expect(errorDetailMsgs.indexOf('Do you have any health problems that make it hard to manage your daily life?')).toBeGreaterThan(-1);
//             expect(errorDetailMsgs.indexOf('Do you have concerns that problems with your health will make it hard to participate in work activities?')).toBeGreaterThan(-1);
//             expect(errorDetailMsgs.indexOf('Do you currently see a health care provider or take medications for health problem(s)?')).toBeGreaterThan(-1);
//             expect(errorDetailMsgs.indexOf('Have you ever met with a counselor or psychiatrist for mental health services or been diagnosed with a mental health condition?')).toBeGreaterThan(-1);
//             expect(errorDetailMsgs.indexOf('Do you have any mental health conditions that make it hard for you to manage your daily life?')).toBeGreaterThan(-1);
//             expect(errorDetailMsgs.indexOf('Do you have concerns that a mental health condition will make it hard for you to participate in work activities?')).toBeGreaterThan(-1);
//             expect(errorDetailMsgs.indexOf('Do you currently see a counselor or psychiatrist for mental health services or take medication for a mental health condition?')).toBeGreaterThan(-1);
//             expect(errorDetailMsgs.indexOf('Does alcohol or drug use make it hard for you to manage your daily life?')).toBeGreaterThan(-1);
//             expect(errorDetailMsgs.indexOf('Do you have concerns that alcohol or drug use will make it hard for you to participate in work activities?')).toBeGreaterThan(-1);
//             expect(errorDetailMsgs.indexOf('Are you currently in any alcohol or drug treatment services?')).toBeGreaterThan(-1);
//             expect(errorDetailMsgs.indexOf('Did you ever have problems learning in school or have you ever been diagnosed with a learning disability?')).toBeGreaterThan(-1);
//             expect(errorDetailMsgs.indexOf('Do you have learning problems that make it hard to manage your daily life?')).toBeGreaterThan(-1);
//             expect(errorDetailMsgs.indexOf('Do you have concerns that learning problems will make it hard to participate in work activities?')).toBeGreaterThan(-1);
//             expect(errorDetailMsgs.indexOf('Do you have any concerns about your safety or the safety of your family?')).toBeGreaterThan(-1);
//             expect(errorDetailMsgs.indexOf('Is there anyone in your household who is not supportive of you working?')).toBeGreaterThan(-1);
//             expect(errorDetailMsgs.indexOf('Does anyone in your household cause problems that prevent you from going to work?')).toBeGreaterThan(-1);

//             expect(errorDetailMsgs.length).toEqual(16);
//             expect(result.isValid).toEqual(false);
//         });
//     });
// });
