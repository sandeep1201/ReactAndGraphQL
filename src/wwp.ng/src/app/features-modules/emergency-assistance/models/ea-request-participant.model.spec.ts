// import { Router } from '@angular/router';
// import { LogService } from 'src/app/shared/services/log.service';
// import { JwtAuthConfig } from 'src/app/core/jwt-auth-config';
// import { ValidationManager } from 'src/app/shared/models/validation';
// import { empty } from 'rxjs';
// import { AppService } from 'src/app/core/services/app.service';
// import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
// import { Location } from '@angular/common';
// import * as TypeMoq from 'typemoq';
// import { EARequestParticipant } from './ea-request-participant.model';

// describe('Emergency Request Participant : isEdit', () => {
//   const router = TypeMoq.Mock.ofType<Router>();
//   const logServiceMoq = TypeMoq.Mock.ofType<LogService>();
//   const jwtMoq = TypeMoq.Mock.ofType<JwtAuthConfig>();
//   const location = TypeMoq.Mock.ofType<Location>();
//   let validationManager: ValidationManager;
//   router.setup(x => x.events).returns(() => empty());
//   let appService: AppService;
//   const authHttpClientMock: TypeMoq.IMock<AuthHttpClient> = TypeMoq.Mock.ofType<AuthHttpClient>();
//   let model = [];
//   const samplemodel = [
//     {
//       id: 8,
//       pinNumber: 1009969382.0,
//       participantId: 13660,
//       participantName: 'NKFZP W. PJSOX',
//       participantDOB: '1985-04-27T00:00:00-05:00',
//       eaRequestId: 18,
//       isDeleted: false,
//       isIncluded: false,
//       modifiedBy: 'Dinesh Reddy Anumula',
//       modifiedDate: '2020-05-22T15:08:15.703-05:00'
//     },
//     {
//       id: 9,
//       pinNumber: 1009969391.0,
//       participantId: 13662,
//       participantName: 'QKXQZ COAYY',
//       participantDOB: '1993-04-27T00:00:00-05:00',
//       eaRequestId: 18,
//       isDeleted: false,
//       isIncluded: true,
//       modifiedBy: 'Dinesh Reddy Anumula',
//       modifiedDate: '2020-05-22T15:08:15.703-05:00'
//     }
//   ] as EARequestParticipant[];

//   beforeEach(() => {
//     model = [];
//     const item = new EARequestParticipant();
//     samplemodel.map(x => (EARequestParticipant.clone(x, item), model.push(item)));
//     appService = new AppService(authHttpClientMock.object, logServiceMoq.object, router.object, jwtMoq.object, location.object);
//     validationManager = new ValidationManager(appService);
//   });

//   it('Add field is required but passed null', () => {
//     const validationResult = EARequestParticipant.validate(validationManager, model);
//     model.forEach((x, index) => {
//       expect(validationResult.errors[`add${index}`]).toBeTruthy();
//       expect(validationResult.errors[`eaIndividualTypeId${index}`]).toBeFalsy();
//       expect(validationResult.errors[`eaRelationTypeId${index}`]).toBeFalsy();
//     });
//   });

//   it('Add fields passed but individual and relation type null', () => {
//     model.map(x => (x.add = true));
//     const validationResult = EARequestParticipant.validate(validationManager, model);
//     model.forEach((x, index) => {
//       expect(validationResult.errors[`add${index}`]).toBeFalsy();
//       expect(validationResult.errors[`individualTypeId${index}`]).toBeTruthy();
//       expect(validationResult.errors[`relationshipTypeId${index}`]).toBeTruthy();
//     });
//   });

//   it('All required fields are provided', () => {
//     model.map(x => ((x.add = true), (x.eaIndividualTypeId = 1), (x.eaRelationTypeId = 1)));
//     const validationResult = EARequestParticipant.validate(validationManager, model);
//     model.forEach((x, index) => {
//       expect(validationResult.errors[`add${index}`]).toBeFalsy();
//       expect(validationResult.errors[`individualTypeId${index}`]).toBeFalsy();
//       expect(validationResult.errors[`relationshipTypeId${index}`]).toBeFalsy();
//     });
//   });
// });
