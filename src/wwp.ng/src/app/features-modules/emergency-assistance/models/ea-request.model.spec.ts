// import { DropDownField } from 'src/app/shared/models/dropdown-field';
// import { Router } from '@angular/router';
// import { LogService } from 'src/app/shared/services/log.service';
// import { JwtAuthConfig } from 'src/app/core/jwt-auth-config';
// import { ValidationManager } from 'src/app/shared/models/validation';
// import { empty } from 'rxjs';
// import { AppService } from 'src/app/core/services/app.service';
// import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
// import { Location } from '@angular/common';
// import * as TypeMoq from 'typemoq';
// import { EARequest } from './ea-request.model';

// describe('EmergencyAssistance', () => {
//   const router = TypeMoq.Mock.ofType<Router>();
//   const logServiceMoq = TypeMoq.Mock.ofType<LogService>();
//   const jwtMoq = TypeMoq.Mock.ofType<JwtAuthConfig>();
//   const location = TypeMoq.Mock.ofType<Location>();
//   let validationManager: ValidationManager;
//   router.setup(x => x.events).returns(() => empty());
//   let model: EARequest;
//   let appService: AppService;
//   const authHttpClientMock: TypeMoq.IMock<AuthHttpClient> = TypeMoq.Mock.ofType<AuthHttpClient>();
//   const eaStatusDrop: DropDownField[] = [
//     { id: 1, name: 'In Progress', code: 'IP', optionCd: null },
//     { id: 2, name: 'Approved', code: 'AP', optionCd: null },
//     { id: 3, name: 'Denied', code: 'DN', optionCd: null },
//     { id: 4, name: 'Pending', code: 'PN', optionCd: null },
//     { id: 5, name: 'Withdrawn', code: 'WD', optionCd: null }
//   ];

//   beforeEach(() => {
//     model = new EARequest();
//     appService = new AppService(authHttpClientMock.object, logServiceMoq.object, router.object, jwtMoq.object, location.object);
//     validationManager = new ValidationManager(appService);
//   });

//   it('create new method', () => {
//     const pariticipatId = 1000000;
//     const pinNumber = 193623265;
//     const model = EARequest.create(pariticipatId, pinNumber);
//     expect(model.id).toBe(0);
//     expect(model.eaRequestParticipants[0].id).toBe(0);
//     expect(model.eaRequestParticipants[0].participantId).toEqual(pariticipatId);
//     expect(model.eaRequestParticipants[0].pinNumber).toEqual(pinNumber);
//     expect(model.eaRequestParticipants[0].eaIndividualTypeId).toBeUndefined();
//     expect(model.eaRequestParticipants[0].eaRelationTypeId).toBeUndefined();
//     expect(model.eaRequestParticipants[0].isIncluded).toBeTruthy();
//     expect(model.eaComments).toBeDefined();
//   });

//   it('validationMethod for all null values', () => {
//     const validationResult = model.validate(validationManager, eaStatusDrop);
//     expect(validationResult.errors.applicationDate).toBeTruthy();
//     expect(validationResult.errors.emergencyTypeId).toBeTruthy();
//     expect(validationResult.errors.statusId).toBeTruthy();
//     expect(validationResult.errors.statusReasonId).toBeUndefined();
//     expect(validationResult.errors.comments).toBeUndefined();
//   });

//   it('validationMethod for invalid max date amount and other values are empty srings', () => {
//     model.maxPaymentAmount = '-100';
//     model.applicationDate = '';
//     model.emergencyTypeIds = null;
//     model.statusId = 0;
//     const validationResult = model.validate(validationManager, eaStatusDrop);
//     expect(validationResult.errors.applicationDate).toBeTruthy();
//     expect(validationResult.errors.emergencyTypeId).toBeTruthy();
//     expect(validationResult.errors.statusId).toBeTruthy();
//     expect(validationResult.errors.statusReasonId).toBeUndefined();
//     expect(validationResult.errors.comments).toBeUndefined();
//   });

//   it('validationMethod for invalid date and max payment amount', () => {
//     model.maxPaymentAmount = '. 0';
//     model.applicationDate = '120203256'; // Invalid or future date
//     model.emergencyTypeIds = [3];
//     model.statusId = 1;
//     const validationResult = model.validate(validationManager, eaStatusDrop);
//     expect(validationResult.errors.applicationDate).toBeTruthy();
//     expect(validationResult.errors.emergencyTypeId).toBeFalsy();
//     expect(validationResult.errors.statusId).toBeFalsy();
//     expect(validationResult.errors.statusReasonId).toBeUndefined();
//     expect(validationResult.errors.comments).toBeUndefined();
//   });

//   it('validationMethod for status has no reason', () => {
//     model.statusId = 1;
//     model.statusReasonId = null;
//     const validationResult = model.validate(validationManager, eaStatusDrop);
//     expect(validationResult.errors.applicationDate).toBeTruthy();
//     expect(validationResult.errors.emergencyTypeId).toBeTruthy();
//     expect(validationResult.errors.statusId).toBeFalsy();
//     expect(validationResult.errors.statusReasonId).toBeUndefined();
//     expect(validationResult.errors.comments).toBeUndefined();
//   });

//   it('validationMethod for status Reason value null', () => {
//     model.statusId = 4;
//     model.statusReasonId = null;
//     const validationResult = model.validate(validationManager, eaStatusDrop);
//     expect(validationResult.errors.applicationDate).toBeTruthy();
//     expect(validationResult.errors.emergencyTypeId).toBeTruthy();
//     expect(validationResult.errors.statusId).toBeFalsy();
//     expect(validationResult.errors.statusReasonId).toBeTruthy();
//     expect(validationResult.errors.comments).toBeUndefined();
//   });

//   it('validationMethod for status Reason has value', () => {
//     model.statusId = 4;
//     model.statusReasonId = 2;
//     const validationResult = model.validate(validationManager, eaStatusDrop);
//     expect(validationResult.errors.applicationDate).toBeTruthy();
//     expect(validationResult.errors.emergencyTypeId).toBeTruthy();
//     expect(validationResult.errors.statusId).toBeFalsy();
//     expect(validationResult.errors.statusReasonId).toBeFalsy();
//     expect(validationResult.errors.comments).toBeUndefined();
//   });
// });
