// import { Participant } from './../../../shared/models/participant';
// import { ValidationManager } from './../../../shared/models/validation-manager';
// import { JwtAuthConfig } from './../../../core/jwt-auth-config';
// import { LogService } from './../../../shared/services/log.service';
// import * as moment from 'moment';
// import { Auxiliary } from './auxiliary.model';
// import { Router } from '@angular/router';
// import * as TypeMoq from 'typemoq';
// import { empty } from 'rxjs';
// import { Location } from '@angular/common';
// import { AppService } from '../../../core/services/app.service';
// import { AuthHttpClient } from '../../../core/interceptors/AuthHttpClient';

// describe('Auxiliary: isOpen', () => {
//   const router = TypeMoq.Mock.ofType<Router>();
//   const logServiceMoq = TypeMoq.Mock.ofType<LogService>();
//   const jwtMoq = TypeMoq.Mock.ofType<JwtAuthConfig>();
//   const location = TypeMoq.Mock.ofType<Location>();
//   let validationManager: ValidationManager;
//   router.setup(x => x.events).returns(() => empty());
//   let model: Auxiliary;
//   let appService: AppService;
//   const authHttpClientMock: TypeMoq.IMock<AuthHttpClient> = TypeMoq.Mock.ofType<AuthHttpClient>();
//   const pullDownDates = [
//     { id: 1, benefitMonth: 1, benefitYear: 2020, pullDownDate: '2020-01-27T00:00:00-06:00' },
//     { id: 2, benefitMonth: 2, benefitYear: 2020, pullDownDate: '2020-02-25T00:00:00-06:00' },
//     { id: 3, benefitMonth: 3, benefitYear: 2020, pullDownDate: '2020-03-25T00:00:00-05:00' },
//     { id: 4, benefitMonth: 4, benefitYear: 2020, pullDownDate: '2020-04-24T00:00:00-05:00' },
//     { id: 5, benefitMonth: 5, benefitYear: 2020, pullDownDate: '2020-05-26T00:00:00-05:00' },
//     { id: 6, benefitMonth: 6, benefitYear: 2020, pullDownDate: '2020-06-24T00:00:00-05:00' },
//     { id: 7, benefitMonth: 7, benefitYear: 2020, pullDownDate: '2020-07-27T00:00:00-05:00' },
//     { id: 8, benefitMonth: 8, benefitYear: 2020, pullDownDate: '2020-08-25T00:00:00-05:00' },
//     { id: 9, benefitMonth: 9, benefitYear: 2020, pullDownDate: '2020-09-24T00:00:00-05:00' },
//     { id: 10, benefitMonth: 10, benefitYear: 2020, pullDownDate: '2020-10-27T00:00:00-05:00' },
//     { id: 11, benefitMonth: 11, benefitYear: 2020, pullDownDate: '2020-11-23T00:00:00-06:00' },
//     { id: 12, benefitMonth: 12, benefitYear: 2020, pullDownDate: '2020-12-23T00:00:00-06:00' },
//     { id: 13, benefitMonth: 1, benefitYear: 2021, pullDownDate: '2021-01-26T00:00:00-06:00' },
//     { id: 14, benefitMonth: 2, benefitYear: 2022, pullDownDate: '2021-02-23T00:00:00-06:00' }
//   ];
//   let allAuxs = [
//     {
//       id: 38,
//       participantId: 13606,
//       pinNumber: 9786538.0,
//       participantName: 'MARIA CREEK',
//       caseNumber: 3886409.0,
//       countyNumber: 13,
//       countyName: 'DANE                ',
//       officeNumber: 811,
//       officeName: 'FORWARD SERVICE CORP W-2 PROGRAM',
//       agencyCode: 'FSC',
//       participationPeriodId: 4,
//       participationPeriodName: 'March 16 - April 15',
//       participationPeriodYear: 2020,
//       originalPayment: 653.0,
//       recoupmentAmount: null,
//       requestedAmount: 50.0,
//       auxiliaryReasonId: 1,
//       auxiliaryReasonName: 'Agency Misapplied Program Policy',
//       auxiliaryStatusTypeId: 5,
//       auxiliaryStatusTypeCode: 'SB',
//       auxiliaryStatusTypeName: 'Submitted',
//       auxiliaryStatusTypeDisplayName: 'Submitted',
//       auxiliaryStatusDate: '2020-05-21T00:00:00-05:00',
//       details: 'asdsad',
//       isSubmit: false,
//       isWithdraw: false,
//       isAllowed: null,
//       overPayAmount: null,
//       modifiedBy: 'Sandeep Reddy Alalla',
//       modifiedDate: '2020-05-21T15:28:06.657-05:00',
//       auxiliaryStatuses: [
//         {
//           id: 80,
//           auxiliaryStatusTypeId: 5,
//           auxiliaryStatusName: 'Submitted',
//           auxiliaryStatusDisplayName: 'Submitted',
//           auxiliaryStatusDate: '2020-05-21T00:00:00-05:00',
//           details: 'asdsad',
//           modifiedBy: 'Sandeep Reddy Alalla',
//           modifiedDate: '2020-05-21T15:28:06.657-05:00'
//         }
//       ]
//     },
//     {
//       id: 33,
//       participantId: 13606,
//       pinNumber: 9786538.0,
//       participantName: 'MARIA CREEK',
//       caseNumber: 3886409.0,
//       countyNumber: 13,
//       countyName: 'DANE                ',
//       officeNumber: 811,
//       officeName: 'FORWARD SERVICE CORP W-2 PROGRAM',
//       agencyCode: 'FSC',
//       participationPeriodId: 4,
//       participationPeriodName: 'March 16 - April 15',
//       participationPeriodYear: 2020,
//       originalPayment: 653.0,
//       recoupmentAmount: null,
//       requestedAmount: 100.0,
//       auxiliaryReasonId: 1,
//       auxiliaryReasonName: 'Agency Misapplied Program Policy',
//       auxiliaryStatusTypeId: 5,
//       auxiliaryStatusTypeCode: 'SB',
//       auxiliaryStatusTypeName: 'Submitted',
//       auxiliaryStatusTypeDisplayName: 'Submitted',
//       auxiliaryStatusDate: '2020-05-19T00:00:00-05:00',
//       details: 'test',
//       isSubmit: false,
//       isWithdraw: false,
//       isAllowed: null,
//       overPayAmount: null,
//       modifiedBy: 'Sandeep Reddy Alalla',
//       modifiedDate: '2020-05-19T14:08:15.563-05:00',
//       auxiliaryStatuses: [
//         {
//           id: 71,
//           auxiliaryStatusTypeId: 5,
//           auxiliaryStatusName: 'Submitted',
//           auxiliaryStatusDisplayName: 'Submitted',
//           auxiliaryStatusDate: '2020-05-19T00:00:00-05:00',
//           details: 'test',
//           modifiedBy: 'Sandeep Reddy Alalla',
//           modifiedDate: '2020-05-19T14:08:15.563-05:00'
//         }
//       ]
//     },
//     {
//       id: 29,
//       participantId: 13606,
//       pinNumber: 9786538.0,
//       participantName: 'MARIA CREEK',
//       caseNumber: 3886409.0,
//       countyNumber: 13,
//       countyName: 'DANE                ',
//       officeNumber: 811,
//       officeName: 'FORWARD SERVICE CORP W-2 PROGRAM',
//       agencyCode: 'FSC',
//       participationPeriodId: 4,
//       participationPeriodName: 'March 16 - April 15',
//       participationPeriodYear: 2020,
//       originalPayment: 653.0,
//       recoupmentAmount: null,
//       requestedAmount: 100.0,
//       auxiliaryReasonId: 1,
//       auxiliaryReasonName: 'Agency Misapplied Program Policy',
//       auxiliaryStatusTypeId: 5,
//       auxiliaryStatusTypeCode: 'SB',
//       auxiliaryStatusTypeName: 'Submitted',
//       auxiliaryStatusTypeDisplayName: 'Submitted',
//       auxiliaryStatusDate: '2020-05-13T00:00:00-05:00',
//       details: 'test',
//       isSubmit: false,
//       isWithdraw: false,
//       isAllowed: null,
//       overPayAmount: null,
//       modifiedBy: 'Sandeep Reddy Alalla',
//       modifiedDate: '2020-05-13T13:55:16.883-05:00',
//       auxiliaryStatuses: [
//         {
//           id: 67,
//           auxiliaryStatusTypeId: 5,
//           auxiliaryStatusName: 'Submitted',
//           auxiliaryStatusDisplayName: 'Submitted',
//           auxiliaryStatusDate: '2020-05-13T00:00:00-05:00',
//           details: 'test',
//           modifiedBy: 'Sandeep Reddy Alalla',
//           modifiedDate: '2020-05-13T13:55:16.883-05:00'
//         }
//       ]
//     },
//     {
//       id: 28,
//       participantId: 13606,
//       pinNumber: 9786538.0,
//       participantName: 'MARIA CREEK',
//       caseNumber: 3886409.0,
//       countyNumber: null,
//       countyName: null,
//       officeNumber: null,
//       officeName: null,
//       agencyCode: 'FSC',
//       participationPeriodId: 2,
//       participationPeriodName: 'January 16 - February 15',
//       participationPeriodYear: 2020,
//       originalPayment: 0.0,
//       recoupmentAmount: null,
//       requestedAmount: 674.0,
//       auxiliaryReasonId: 2,
//       auxiliaryReasonName: 'At Risk Pregnancy',
//       auxiliaryStatusTypeId: 5,
//       auxiliaryStatusTypeCode: 'SB',
//       auxiliaryStatusTypeName: 'Submitted',
//       auxiliaryStatusTypeDisplayName: 'Submitted',
//       auxiliaryStatusDate: '2020-05-13T00:00:00-05:00',
//       details: 'test',
//       isSubmit: false,
//       isWithdraw: false,
//       isAllowed: null,
//       overPayAmount: null,
//       modifiedBy: 'Sandeep Reddy Alalla',
//       modifiedDate: '2020-05-13T13:26:39.94-05:00',
//       auxiliaryStatuses: [
//         {
//           id: 66,
//           auxiliaryStatusTypeId: 5,
//           auxiliaryStatusName: 'Submitted',
//           auxiliaryStatusDisplayName: 'Submitted',
//           auxiliaryStatusDate: '2020-05-13T00:00:00-05:00',
//           details: 'test',
//           modifiedBy: 'Sandeep Reddy Alalla',
//           modifiedDate: '2020-05-13T13:26:39.94-05:00'
//         }
//       ]
//     },
//     {
//       id: 27,
//       participantId: 13606,
//       pinNumber: 9786538.0,
//       participantName: 'MARIA CREEK',
//       caseNumber: 3886409.0,
//       countyNumber: 13,
//       countyName: 'DANE                ',
//       officeNumber: 811,
//       officeName: 'FORWARD SERVICE CORP W-2 PROGRAM',
//       agencyCode: 'FSC',
//       participationPeriodId: 4,
//       participationPeriodName: 'March 16 - April 15',
//       participationPeriodYear: 2020,
//       originalPayment: 653.0,
//       recoupmentAmount: null,
//       requestedAmount: 0.0,
//       auxiliaryReasonId: 2,
//       auxiliaryReasonName: 'At Risk Pregnancy',
//       auxiliaryStatusTypeId: 5,
//       auxiliaryStatusTypeCode: 'SB',
//       auxiliaryStatusTypeName: 'Submitted',
//       auxiliaryStatusTypeDisplayName: 'Submitted',
//       auxiliaryStatusDate: '2020-05-13T00:00:00-05:00',
//       details: 'test',
//       isSubmit: false,
//       isWithdraw: false,
//       isAllowed: null,
//       overPayAmount: null,
//       modifiedBy: 'Sandeep Reddy Alalla',
//       modifiedDate: '2020-05-13T12:21:36.083-05:00',
//       auxiliaryStatuses: [
//         {
//           id: 65,
//           auxiliaryStatusTypeId: 5,
//           auxiliaryStatusName: 'Submitted',
//           auxiliaryStatusDisplayName: 'Submitted',
//           auxiliaryStatusDate: '2020-05-13T00:00:00-05:00',
//           details: 'test',
//           modifiedBy: 'Sandeep Reddy Alalla',
//           modifiedDate: '2020-05-13T12:21:36.083-05:00'
//         }
//       ]
//     },
//     {
//       id: 26,
//       participantId: 13606,
//       pinNumber: 9786538.0,
//       participantName: 'MARIA CREEK',
//       caseNumber: null,
//       countyNumber: null,
//       countyName: null,
//       officeNumber: null,
//       officeName: null,
//       agencyCode: 'FSC',
//       participationPeriodId: 4,
//       participationPeriodName: 'March 16 - April 15',
//       participationPeriodYear: 2019,
//       originalPayment: 0.0,
//       recoupmentAmount: null,
//       requestedAmount: 0.0,
//       auxiliaryReasonId: 3,
//       auxiliaryReasonName: 'Backdated Placement Beyond 10 Days',
//       auxiliaryStatusTypeId: 5,
//       auxiliaryStatusTypeCode: 'SB',
//       auxiliaryStatusTypeName: 'Submitted',
//       auxiliaryStatusTypeDisplayName: 'Submitted',
//       auxiliaryStatusDate: '2020-05-13T00:00:00-05:00',
//       details: 'adsadsad',
//       isSubmit: false,
//       isWithdraw: false,
//       isAllowed: null,
//       overPayAmount: null,
//       modifiedBy: 'Sandeep Reddy Alalla',
//       modifiedDate: '2020-05-13T12:19:03.263-05:00',
//       auxiliaryStatuses: [
//         {
//           id: 64,
//           auxiliaryStatusTypeId: 5,
//           auxiliaryStatusName: 'Submitted',
//           auxiliaryStatusDisplayName: 'Submitted',
//           auxiliaryStatusDate: '2020-05-13T00:00:00-05:00',
//           details: 'adsadsad',
//           modifiedBy: 'Sandeep Reddy Alalla',
//           modifiedDate: '2020-05-13T12:19:03.263-05:00'
//         }
//       ]
//     },
//     {
//       id: 21,
//       participantId: 13606,
//       pinNumber: 9786538.0,
//       participantName: 'MARIA CREEK',
//       caseNumber: null,
//       countyNumber: null,
//       countyName: null,
//       officeNumber: null,
//       officeName: null,
//       agencyCode: 'FSC',
//       participationPeriodId: 3,
//       participationPeriodName: 'February 16 - March 15',
//       participationPeriodYear: 2017,
//       originalPayment: 0.0,
//       recoupmentAmount: null,
//       requestedAmount: 0.0,
//       auxiliaryReasonId: 1,
//       auxiliaryReasonName: 'Agency Misapplied Program Policy',
//       auxiliaryStatusTypeId: 5,
//       auxiliaryStatusTypeCode: 'SB',
//       auxiliaryStatusTypeName: 'Submitted',
//       auxiliaryStatusTypeDisplayName: 'Submitted',
//       auxiliaryStatusDate: '2020-05-13T00:00:00-05:00',
//       details: 'dasdsd',
//       isSubmit: false,
//       isWithdraw: false,
//       isAllowed: null,
//       overPayAmount: null,
//       modifiedBy: 'Sandeep Reddy Alalla',
//       modifiedDate: '2020-05-13T09:35:05.88-05:00',
//       auxiliaryStatuses: [
//         {
//           id: 59,
//           auxiliaryStatusTypeId: 5,
//           auxiliaryStatusName: 'Submitted',
//           auxiliaryStatusDisplayName: 'Submitted',
//           auxiliaryStatusDate: '2020-05-13T00:00:00-05:00',
//           details: 'dasdsd',
//           modifiedBy: 'Sandeep Reddy Alalla',
//           modifiedDate: '2020-05-13T09:35:05.88-05:00'
//         }
//       ]
//     },
//     {
//       id: 20,
//       participantId: 13606,
//       pinNumber: 9786538.0,
//       participantName: 'MARIA CREEK',
//       caseNumber: 3886409.0,
//       countyNumber: null,
//       countyName: null,
//       officeNumber: null,
//       officeName: null,
//       agencyCode: 'FSC',
//       participationPeriodId: 3,
//       participationPeriodName: 'February 16 - March 15',
//       participationPeriodYear: 2020,
//       originalPayment: 0.0,
//       recoupmentAmount: null,
//       requestedAmount: 0.0,
//       auxiliaryReasonId: 1,
//       auxiliaryReasonName: 'Agency Misapplied Program Policy',
//       auxiliaryStatusTypeId: 5,
//       auxiliaryStatusTypeCode: 'SB',
//       auxiliaryStatusTypeName: 'Submitted',
//       auxiliaryStatusTypeDisplayName: 'Submitted',
//       auxiliaryStatusDate: '2020-05-13T00:00:00-05:00',
//       details: 'asdasd',
//       isSubmit: false,
//       isWithdraw: false,
//       isAllowed: null,
//       overPayAmount: null,
//       modifiedBy: 'Sandeep Reddy Alalla',
//       modifiedDate: '2020-05-13T09:34:02.773-05:00',
//       auxiliaryStatuses: [
//         {
//           id: 58,
//           auxiliaryStatusTypeId: 5,
//           auxiliaryStatusName: 'Submitted',
//           auxiliaryStatusDisplayName: 'Submitted',
//           auxiliaryStatusDate: '2020-05-13T00:00:00-05:00',
//           details: 'asdasd',
//           modifiedBy: 'Sandeep Reddy Alalla',
//           modifiedDate: '2020-05-13T09:34:02.773-05:00'
//         }
//       ]
//     },
//     {
//       id: 19,
//       participantId: 13606,
//       pinNumber: 9786538.0,
//       participantName: 'MARIA CREEK',
//       caseNumber: null,
//       countyNumber: null,
//       countyName: null,
//       officeNumber: null,
//       officeName: null,
//       agencyCode: 'FSC',
//       participationPeriodId: 1,
//       participationPeriodName: 'December 16 - January 15',
//       participationPeriodYear: 2019,
//       originalPayment: 0.0,
//       recoupmentAmount: null,
//       requestedAmount: 0.0,
//       auxiliaryReasonId: 2,
//       auxiliaryReasonName: 'At Risk Pregnancy',
//       auxiliaryStatusTypeId: 5,
//       auxiliaryStatusTypeCode: 'SB',
//       auxiliaryStatusTypeName: 'Submitted',
//       auxiliaryStatusTypeDisplayName: 'Submitted',
//       auxiliaryStatusDate: '2020-05-13T00:00:00-05:00',
//       details: 'sadsad',
//       isSubmit: false,
//       isWithdraw: false,
//       isAllowed: null,
//       overPayAmount: null,
//       modifiedBy: 'Sandeep Reddy Alalla',
//       modifiedDate: '2020-05-13T09:21:09.797-05:00',
//       auxiliaryStatuses: [
//         {
//           id: 57,
//           auxiliaryStatusTypeId: 5,
//           auxiliaryStatusName: 'Submitted',
//           auxiliaryStatusDisplayName: 'Submitted',
//           auxiliaryStatusDate: '2020-05-13T00:00:00-05:00',
//           details: 'sadsad',
//           modifiedBy: 'Sandeep Reddy Alalla',
//           modifiedDate: '2020-05-13T09:21:09.797-05:00'
//         }
//       ]
//     },
//     {
//       id: 17,
//       participantId: 13606,
//       pinNumber: 9786538.0,
//       participantName: 'MARIA CREEK',
//       caseNumber: 3886409.0,
//       countyNumber: 13,
//       countyName: 'DANE                ',
//       officeNumber: 811,
//       officeName: 'FORWARD SERVICE CORP W-2 PROGRAM',
//       agencyCode: 'FSC',
//       participationPeriodId: 12,
//       participationPeriodName: 'November 16 - December 15',
//       participationPeriodYear: 2019,
//       originalPayment: 0.0,
//       recoupmentAmount: null,
//       requestedAmount: 100,
//       auxiliaryReasonId: 1,
//       auxiliaryReasonName: 'Agency Misapplied Program Policy',
//       auxiliaryStatusTypeId: 1,
//       auxiliaryStatusTypeCode: 'AP',
//       auxiliaryStatusTypeName: 'Approve',
//       auxiliaryStatusTypeDisplayName: 'Approved',
//       auxiliaryStatusDate: '2020-05-08T00:00:00-05:00',
//       details: 'Test',
//       isSubmit: false,
//       isWithdraw: false,
//       isAllowed: null,
//       overPayAmount: null,
//       modifiedBy: 'Silambholi Tholkappian',
//       modifiedDate: '2020-05-08T16:38:29.543-05:00',
//       auxiliaryStatuses: [
//         {
//           id: 56,
//           auxiliaryStatusTypeId: 1,
//           auxiliaryStatusName: 'Approve',
//           auxiliaryStatusDisplayName: 'Approved',
//           auxiliaryStatusDate: '2020-05-08T00:00:00-05:00',
//           details: 'Test',
//           modifiedBy: 'Silambholi Tholkappian',
//           modifiedDate: '2020-05-08T16:38:29.543-05:00'
//         },
//         {
//           id: 54,
//           auxiliaryStatusTypeId: 5,
//           auxiliaryStatusName: 'Submitted',
//           auxiliaryStatusDisplayName: 'Submitted',
//           auxiliaryStatusDate: '2020-05-05T00:00:00-05:00',
//           details: 'test',
//           modifiedBy: 'Sandeep Reddy Alalla',
//           modifiedDate: '2020-05-05T13:38:43.783-05:00'
//         }
//       ]
//     },
//     {
//       id: 18,
//       participantId: 13606,
//       pinNumber: 9786538.0,
//       participantName: 'MARIA CREEK',
//       caseNumber: 3886409.0,
//       countyNumber: null,
//       countyName: null,
//       officeNumber: null,
//       officeName: null,
//       agencyCode: 'FSC',
//       participationPeriodId: 1,
//       participationPeriodName: 'December 16 - January 15',
//       participationPeriodYear: 2020,
//       originalPayment: 0.0,
//       recoupmentAmount: null,
//       requestedAmount: 0.0,
//       auxiliaryReasonId: 3,
//       auxiliaryReasonName: 'Backdated Placement Beyond 10 Days',
//       auxiliaryStatusTypeId: 5,
//       auxiliaryStatusTypeCode: 'SB',
//       auxiliaryStatusTypeName: 'Submitted',
//       auxiliaryStatusTypeDisplayName: 'Submitted',
//       auxiliaryStatusDate: '2020-05-07T00:00:00-05:00',
//       details: 'testing',
//       isSubmit: false,
//       isWithdraw: false,
//       isAllowed: null,
//       overPayAmount: null,
//       modifiedBy: 'Sandeep Reddy Alalla',
//       modifiedDate: '2020-05-07T17:13:47.567-05:00',
//       auxiliaryStatuses: [
//         {
//           id: 55,
//           auxiliaryStatusTypeId: 5,
//           auxiliaryStatusName: 'Submitted',
//           auxiliaryStatusDisplayName: 'Submitted',
//           auxiliaryStatusDate: '2020-05-07T00:00:00-05:00',
//           details: 'testing',
//           modifiedBy: 'Sandeep Reddy Alalla',
//           modifiedDate: '2020-05-07T17:13:47.567-05:00'
//         }
//       ]
//     },
//     {
//       id: 12,
//       participantId: 13606,
//       pinNumber: 9786538.0,
//       participantName: 'MARIA CREEK',
//       caseNumber: 3886409.0,
//       countyNumber: 13,
//       countyName: 'DANE                ',
//       officeNumber: 811,
//       officeName: 'FORWARD SERVICE CORP W-2 PROGRAM',
//       agencyCode: 'FSC',
//       participationPeriodId: 4,
//       participationPeriodName: 'March 16 - April 15',
//       participationPeriodYear: 2020,
//       originalPayment: 100.0,
//       recoupmentAmount: null,
//       requestedAmount: 50.0,
//       auxiliaryReasonId: 1,
//       auxiliaryReasonName: 'Agency Misapplied Program Policy',
//       auxiliaryStatusTypeId: 1,
//       auxiliaryStatusTypeCode: 'AP',
//       auxiliaryStatusTypeName: 'Approve',
//       auxiliaryStatusTypeDisplayName: 'Approved',
//       auxiliaryStatusDate: '2020-04-21T00:00:00-05:00',
//       details: 'Test',
//       isSubmit: false,
//       isWithdraw: false,
//       isAllowed: null,
//       overPayAmount: null,
//       modifiedBy: 'Silambholi Tholkappian',
//       modifiedDate: '2020-04-21T19:41:48.2-05:00',
//       auxiliaryStatuses: [
//         {
//           id: 51,
//           auxiliaryStatusTypeId: 1,
//           auxiliaryStatusName: 'Approve',
//           auxiliaryStatusDisplayName: 'Approved',
//           auxiliaryStatusDate: '2020-04-21T00:00:00-05:00',
//           details: 'Test',
//           modifiedBy: 'Silambholi Tholkappian',
//           modifiedDate: '2020-04-21T19:41:48.2-05:00'
//         },
//         {
//           id: 41,
//           auxiliaryStatusTypeId: 3,
//           auxiliaryStatusName: 'Review',
//           auxiliaryStatusDisplayName: 'Review in Progress',
//           auxiliaryStatusDate: '2020-04-16T00:00:00-05:00',
//           details: 'Test',
//           modifiedBy: 'Sandeep Reddy Alalla',
//           modifiedDate: '2020-04-16T15:13:58.18-05:00'
//         },
//         {
//           id: 34,
//           auxiliaryStatusTypeId: 5,
//           auxiliaryStatusName: 'Submitted',
//           auxiliaryStatusDisplayName: 'Submitted',
//           auxiliaryStatusDate: '2020-04-16T00:00:00-05:00',
//           details: 'Test',
//           modifiedBy: 'Silambholi Tholkappian',
//           modifiedDate: '2020-04-16T14:07:46.85-05:00'
//         }
//       ]
//     },
//     {
//       id: 13,
//       participantId: 13606,
//       pinNumber: 9786538.0,
//       participantName: 'MARIA CREEK',
//       caseNumber: 3886409.0,
//       countyNumber: 13,
//       countyName: 'DANE                ',
//       officeNumber: 811,
//       officeName: 'FORWARD SERVICE CORP W-2 PROGRAM',
//       agencyCode: 'FSC',
//       participationPeriodId: 4,
//       participationPeriodName: 'March 16 - April 15',
//       participationPeriodYear: 2020,
//       originalPayment: 100.0,
//       recoupmentAmount: null,
//       requestedAmount: 50.0,
//       auxiliaryReasonId: 1,
//       auxiliaryReasonName: 'Agency Misapplied Program Policy',
//       auxiliaryStatusTypeId: 1,
//       auxiliaryStatusTypeCode: 'AP',
//       auxiliaryStatusTypeName: 'Approve',
//       auxiliaryStatusTypeDisplayName: 'Approved',
//       auxiliaryStatusDate: '2020-04-16T00:00:00-05:00',
//       details: 'Test',
//       isSubmit: false,
//       isWithdraw: false,
//       isAllowed: null,
//       overPayAmount: null,
//       modifiedBy: 'Silambholi Tholkappian',
//       modifiedDate: '2020-04-16T14:59:09.5-05:00',
//       auxiliaryStatuses: [
//         {
//           id: 40,
//           auxiliaryStatusTypeId: 1,
//           auxiliaryStatusName: 'Approve',
//           auxiliaryStatusDisplayName: 'Approved',
//           auxiliaryStatusDate: '2020-04-16T00:00:00-05:00',
//           details: 'Test',
//           modifiedBy: 'Silambholi Tholkappian',
//           modifiedDate: '2020-04-16T14:59:09.5-05:00'
//         },
//         {
//           id: 39,
//           auxiliaryStatusTypeId: 5,
//           auxiliaryStatusName: 'Submitted',
//           auxiliaryStatusDisplayName: 'Submitted',
//           auxiliaryStatusDate: '2020-04-16T00:00:00-05:00',
//           details: 'Test',
//           modifiedBy: 'Silambholi Tholkappian',
//           modifiedDate: '2020-04-16T14:55:02.577-05:00'
//         },
//         {
//           id: 38,
//           auxiliaryStatusTypeId: 5,
//           auxiliaryStatusName: 'Submitted',
//           auxiliaryStatusDisplayName: 'Submitted',
//           auxiliaryStatusDate: '2020-04-16T00:00:00-05:00',
//           details: 'Test',
//           modifiedBy: 'Silambholi Tholkappian',
//           modifiedDate: '2020-04-16T14:54:13.12-05:00'
//         },
//         {
//           id: 37,
//           auxiliaryStatusTypeId: 5,
//           auxiliaryStatusName: 'Submitted',
//           auxiliaryStatusDisplayName: 'Submitted',
//           auxiliaryStatusDate: '2020-04-16T00:00:00-05:00',
//           details: 'Test',
//           modifiedBy: 'Silambholi Tholkappian',
//           modifiedDate: '2020-04-16T14:53:09.62-05:00'
//         },
//         {
//           id: 36,
//           auxiliaryStatusTypeId: 5,
//           auxiliaryStatusName: 'Submitted',
//           auxiliaryStatusDisplayName: 'Submitted',
//           auxiliaryStatusDate: '2020-04-16T00:00:00-05:00',
//           details: 'Test',
//           modifiedBy: 'Silambholi Tholkappian',
//           modifiedDate: '2020-04-16T14:49:27.873-05:00'
//         },
//         {
//           id: 35,
//           auxiliaryStatusTypeId: 5,
//           auxiliaryStatusName: 'Submitted',
//           auxiliaryStatusDisplayName: 'Submitted',
//           auxiliaryStatusDate: '2020-04-16T00:00:00-05:00',
//           details: 'Test',
//           modifiedBy: 'Silambholi Tholkappian',
//           modifiedDate: '2020-04-16T14:08:26.473-05:00'
//         }
//       ]
//     }
//   ];
//   beforeEach(() => {
//     // Start with a fresh model for each test.
//     model = new Auxiliary();
//     appService = new AppService(authHttpClientMock.object, logServiceMoq.object, router.object, jwtMoq.object, location.object);
//     validationManager = new ValidationManager(appService);
//   });

//   it('LatestpullDownDate', () => {
//     // this gives the latest passed pulldown date based on the current date
//     const currentDate = moment('04/05/2020').format('MM/DD/YYYY');
//     const latestPullDownDate = model.latestPassedPullDownDate(pullDownDates, moment(currentDate));
//     expect(latestPullDownDate).toEqual('03/25/2020');
//   });
//   it('datesToConsider', () => {
//     // this gives the latest passed pulldown date based on the current date
//     const participationPeriod = 'December 16 - January 15';
//     const latestPullDownDate = model.datestoConsider(2020, participationPeriod);
//     expect(latestPullDownDate).toEqual(['12/16/2019', '01/15/2020']);
//   });
//   it('datesToConsider', () => {
//     // this gives the latest passed pulldown date based on the current date
//     const participationPeriod = 'January 16 - February 15';
//     const latestPullDownDate = model.datestoConsider(2020, participationPeriod);
//     expect(latestPullDownDate).toEqual(['01/16/2020', '02/15/2020']);
//   });
//   it('splitToMonthAndDate', () => {
//     const participationPeriod = 'December 16 - January 15';
//     const months = model.splitToMonthAndDate(participationPeriod);
//     expect(months).toEqual(['December/16', 'January/15']);
//   });
//   it('monthsAndDatesForValidation', () => {
//     const participationPeriod = 'December 16 - January 15';
//     const currentDate = moment('04/05/2020').format('MM/DD/YYYY');
//     const data = model.monthsAndDatesForValidation(2020, pullDownDates, participationPeriod);
//     expect(data).toEqual({ pullDownDate: '03/25/2020', datesToConsider: ['12/16/2019', '01/15/2020'] });
//   });
//   it('validationMethod for PPID and PPYear null values', () => {
//     const participationPeriod = 'December 16 - January 15';
//     const currentDate = moment('04/05/2020').format('MM/DD/YYYY');
//     model.isAllowed = true;
//     model.overPayAmount = '0';
//     const validationResult = model.validate(validationManager, true, '03/25/2020', allAuxs, participationPeriod, pullDownDates, moment(currentDate));
//     expect(validationResult.errors.participationPeriodId).toBeTruthy();
//     expect(validationResult.errors.participationPeriodYear).toBeTruthy();
//     expect(validationResult.errors.auxiliaryReasonId).toBeFalsy();
//     expect(validationResult.errors.details).toBeFalsy();
//     expect(validationResult.errors.originalPayment).toBeFalsy();
//     expect(validationResult.errors.requestedAmount).toBeFalsy();
//     expect(validationResult.isValid).toBeFalsy();
//   });
//   it('validationMethod for all null values except PPID and PPYear', () => {
//     const participationPeriod = 'December 16 - January 15';
//     const currentDate = moment('04/05/2020').format('MM/DD/YYYY');
//     model.isAllowed = true;
//     model.overPayAmount = '0';
//     model.participationPeriodId = 1;
//     model.participationPeriodYear = 2020;
//     const validationResult = model.validate(validationManager, true, '03/25/2020', allAuxs, participationPeriod, pullDownDates, moment(currentDate));
//     expect(validationResult.errors.participationPeriodId).toBeFalsy();
//     expect(validationResult.errors.participationPeriodYear).toBeFalsy();
//     expect(validationResult.errors.auxiliaryReasonId).toBeTruthy();
//     expect(validationResult.errors.details).toBeTruthy();
//     expect(validationResult.errors.originalPayment).toBeTruthy();
//     expect(validationResult.errors.requestedAmount).toBeTruthy();
//     expect(validationResult.isValid).toBeFalsy();
//   });
//   it('validationMethod for pull down date not passed', () => {
//     const participationPeriod = 'March 16 - April 15';
//     const currentDate = moment('04/05/2020').format('MM/DD/YYYY');
//     model.isAllowed = true;
//     model.overPayAmount = '0';
//     model.participantId = 12345;
//     // participant.cutOverDate = '03/25/2020';
//     model.participationPeriodId = 1;
//     model.auxiliaryReasonId = 2;
//     model.details = 'test';
//     model.auxiliaryReasonName = 'test';
//     model.requestedAmount = 100;
//     model.originalPayment = 300;
//     model.participationPeriodYear = 2020;
//     const validationResult = model.validate(validationManager, true, '03/25/2020', allAuxs, participationPeriod, pullDownDates, moment(currentDate));
//     expect(validationResult.errors.participationPeriodId).toBeTruthy();
//     expect(validationResult.isValid).toBeFalsy();
//   });
//   it('validationMethod for pull down date not passed', () => {
//     const participationPeriod = 'February 16 - March 15';
//     const currentDate = moment('04/05/2020').format('MM/DD/YYYY');
//     model.isAllowed = true;
//     model.overPayAmount = '0';
//     model.participantId = 12345;
//     // participant.cutOverDate = '03/25/2020';
//     model.participationPeriodId = 1;
//     model.auxiliaryReasonId = 2;
//     model.details = 'test';
//     model.auxiliaryReasonName = 'test';
//     model.requestedAmount = 674;
//     model.originalPayment = 300;
//     model.participationPeriodYear = 2020;
//     const validationResult = model.validate(validationManager, true, null, allAuxs, participationPeriod, pullDownDates, moment(currentDate));
//     expect(validationResult.errors.requestedAmount).toBeTruthy();
//     expect(validationResult.isValid).toBeFalsy();
//   });
//   it('validationMethod for requested amount > maxAmount and dates are before cutoverDate', () => {
//     const participationPeriod = 'February 16 - March 15';
//     const currentDate = moment('04/05/2020').format('MM/DD/YYYY');
//     model.isAllowed = true;
//     model.overPayAmount = '0';
//     model.participantId = 12345;
//     // participant.cutOverDate = '03/25/2020';
//     model.participationPeriodId = 1;
//     model.auxiliaryReasonId = 2;
//     model.details = 'test';
//     model.auxiliaryReasonName = 'test';
//     model.requestedAmount = 674;
//     model.originalPayment = 300;
//     model.participationPeriodYear = 2020;
//     const validationResult = model.validate(validationManager, true, '03/25/2020', allAuxs, participationPeriod, pullDownDates, moment(currentDate));
//     expect(validationResult.errors.requestedAmount).toBeTruthy();
//     expect(validationResult.isValid).toBeFalsy();
//   });
//   it('validationMethod for requested amount > maxAmount and dates are after cutoverDate', () => {
//     const participationPeriod = 'April 16 - May 15';
//     const currentDate = moment('06/05/2020').format('MM/DD/YYYY');
//     model.isAllowed = true;
//     model.overPayAmount = '0';
//     model.participantId = 12345;
//     // participant.cutOverDate = '03/25/2020';
//     model.participationPeriodId = 1;
//     model.auxiliaryReasonId = 2;
//     model.details = 'test';
//     model.auxiliaryReasonName = 'test';
//     model.requestedAmount = 674;
//     model.originalPayment = 300;
//     model.participationPeriodYear = 2020;
//     const validationResult = model.validate(validationManager, true, '03/25/2020', allAuxs, participationPeriod, pullDownDates, moment(currentDate));
//     expect(validationResult.errors.requestedAmount).toBeTruthy();
//     expect(validationResult.isValid).toBeFalsy();
//   });
//   it('validate calculateTotalW2Payment method', () => {
//     const participationPeriod = 'November 16 - December 15';
//     const participationPeriodYear = 2019;
//     const totoalW2Payment = model.calculateTotalW2Payment(allAuxs, participationPeriod, participationPeriodYear);
//     expect(totoalW2Payment).toEqual(100);
//   });
//   it('validate calculateTotalW2Payment method for empty auxs', () => {
//     const allEmptyAuxs = [];
//     const participationPeriod = 'November 16 - December 15';
//     const participationPeriodYear = 2019;
//     const totoalW2Payment = model.calculateTotalW2Payment(allEmptyAuxs, participationPeriod, participationPeriodYear);
//     expect(totoalW2Payment).toBeUndefined();
//   });
//   it('isStatus required', () => {
//     const res = model.isStatusRequired('SB', true);
//     expect(res).toBeTruthy();
//   });
// });
