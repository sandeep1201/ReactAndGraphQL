// import { EARequestHistoryComponent } from './ea-request-history.component';
// import * as TypeMoq from 'typemoq';
// import { EmergencyAssistanceService } from '../services/emergancy-assistance.service';
// import { Router, ActivatedRoute } from '@angular/router';
// import { ParticipantService } from 'src/app/shared/services/participant.service';

// describe('EARequestHistoryComponent', () => {
//   const routerMoq = TypeMoq.Mock.ofType<Router>();
//   const partServiceMoq = TypeMoq.Mock.ofType<ParticipantService>();
//   const route = TypeMoq.Mock.ofType<ActivatedRoute>();
//   const eaServiceMoq = TypeMoq.Mock.ofType<EmergencyAssistanceService>();
//   let component: EARequestHistoryComponent;

//   let model = [];

//   model = [
//     {
//       id: 13,
//       requestNumber: 0.0,
//       applicationDate: '2020-05-15T00:00:00-05:00',
//       emergencyTypeIds: [2, 1],
//       emergencyTypeNames: ['Impending Homelessness (Renters-Foreclosure)', 'Impending Homelessness (Financial Crisis and Notice to Terminate Tenancy)'],
//       statusId: 4,
//       statusName: 'Pending',
//       statusReasonId: 18,
//       statusReasonName: 'Payment Delay (No New Housing Additional 30 Days)',
//       maxPaymentAmount: '100.00',
//       modifiedBy: 'Dinesh Reddy Anumula',
//       modifiedDate: '2020-06-10T18:41:08.157-05:00',
//       eaRequestParticipants: [
//         {
//           id: 3,
//           pinNumber: 2009285468,
//           participantId: 11196,
//           participantName: 'JIM WALTON',
//           participantDOB: '1970-02-20T00:00:00-06:00',
//           eaRequestId: 13,
//           eaIndividualTypeId: 1,
//           eaIndividualTypeName: 'Caretaker Relative',
//           eaRelationTypeId: 1,
//           eaRelationTypeName: 'Self',
//           isIncluded: false,
//           isDeleted: false,
//           modifiedBy: 'Dinesh Reddy Anumula',
//           modifiedDate: '2020-06-10T18:41:08.157-05:00',
//           add: false,
//           alreadyAdded: false
//         },
//         {
//           id: 26,
//           pinNumber: 2009285247,
//           participantId: 13614,
//           participantName: 'JILL WALTON',
//           participantDOB: '1970-01-20T00:00:00-06:00',
//           eaRequestId: 13,
//           eaIndividualTypeId: 1,
//           eaIndividualTypeName: 'Other Caretaker Relative',
//           eaRelationTypeId: 2,
//           eaRelationTypeName: 'Daughter',
//           isIncluded: false,
//           isDeleted: false,
//           modifiedBy: 'Dinesh Reddy Anumula',
//           modifiedDate: '2020-06-10T18:41:08.157-05:00',
//           add: false,
//           alreadyAdded: false
//         }
//       ],
//       eaComments: []
//     }
//   ];

//   beforeEach(() => {
//     component = new EARequestHistoryComponent(route.object, partServiceMoq.object, eaServiceMoq.object, routerMoq.object);
//   });

//   it('should create', () => {
//     expect(component).toBeTruthy();
//   });

//   // it('getIndividualType', () => {
//   //   expect(component.getIndividualType(model[0])).toBe('Caretaker Relative');
//   // });
// });
