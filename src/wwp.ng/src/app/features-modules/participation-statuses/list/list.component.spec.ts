import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ParticipationStatusListComponent } from './list.component';

import { NO_ERRORS_SCHEMA } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { DateMmDdYyyyPipe } from 'src/app/shared/pipes/date-mm-dd-yyyy.pipe';
import { AppService } from 'src/app/core/services/app.service';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { Participant } from 'src/app/shared/models/participant';
export class TypeOfB {
  readOnly: boolean;
  inEditView: boolean;
}
const b = new BehaviorSubject<TypeOfB>({ readOnly: false, inEditView: false });
const mockParticipantService = {
  getAllStatusesForPin() {
    return of([
      {
        id: 33,
        participantId: 10944,
        pin: 2008983170.0,
        statusId: 8,
        statusName: 'Child Support Payments',
        statusCode: 'CP',
        details: 'this is a current PS',
        beginDate: '2019-04-23T00:00:00-05:00',
        endDate: null,
        isCurrent: true,
        enrolledProgramId: 11,
        enrolledProgramName: 'W-2',
        enrolledProgramCode: 'WW '
      }
    ]);
  },
  modeForParticipationStatuses: of(b)
};

describe('ParticipationStatusListComponent', () => {
  let comp: ParticipationStatusListComponent;
  let fixture: ComponentFixture<ParticipationStatusListComponent>;
  let mockAppService;

  beforeEach(() => {
    // mockParticipantService = jasmine.createSpyObj(['getAllStatusesForPin', 'modeForParticipationStatuses']);
    mockAppService = jasmine.createSpyObj(['user', 'isUserAuthorized', 'isMostRecentProgramInSisterOrg', 'getMostRecentProgramsByAgency']);
    TestBed.configureTestingModule({
      declarations: [ParticipationStatusListComponent, DateMmDdYyyyPipe],
      providers: [
        { provide: AppService, useValue: mockAppService },
        { provide: ParticipantService, useValue: mockParticipantService }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    });
    fixture = TestBed.createComponent(ParticipationStatusListComponent);
    comp = fixture.componentInstance;
    // mockParticipantService.modeForParticipationStatuses.and.returnValue(b.asObservable());
    // mockParticipantService.getAllStatusesForPin.and.returnValue(Observable.of([{ "id": 33, "participantId": 10944, "pin": 2008983170.0, "statusId": 8, "statusName": "Child Support Payments", "statusCode": "CP", "details": "this is a current PS", "beginDate": "2019-04-23T00:00:00-05:00", "endDate": null, "isCurrent": true, "enrolledProgramId": 11, "enrolledProgramName": "W-2", "enrolledProgramCode": "WW " }]))
  });
  it('can load instance', () => {
    expect(comp).toBeTruthy();
  });
  it('should get all the PS', () => {
    comp.pin = '1234567';
    comp.isReadOnly = false;
    comp.participant = new Participant();
    const model = {
      id: 10944,
      pin: '2008983170',
      firstName: 'Lisa',
      lastName: 'Fseworker2',
      middleInitialName: '',
      nameSuffix: '',
      dateOfBirth: '1972-01-01T00:00:00-06:00',
      enrolledDate: '2018-11-12T00:00:00-06:00',
      disenrolledDate: null,
      sortOrder: null,
      groupOrder: null,
      hasConfidentialAccess: true,
      isConfidentialCase: false,
      countyOfResidenceId: null,
      address: '',
      displayName: '',
      programs: [
        {
          id: 11713,
          agencyCode: 'FSC',
          agencyName: 'Forward Service Corp',
          officeId: null,
          officeCounty: 'ROCK',
          officeNumber: 721,
          rfaNumber: null,
          referralDate: '2017-11-22T00:00:00-06:00',
          enrolledProgramId: 11,
          programCd: 'WW',
          programCode: 'W-2',
          enrollmentDate: '2018-11-12T00:00:00-06:00',
          disenrollmentDate: null,
          completionReasonId: null,
          completionReasonDetails: null,
          status: 'Enrolled',
          statusDate: '2018-11-12T00:00:00-06:00',
          subProgramCode: null,
          isTransfer: false,
          cfCourtOrderedDate: null,
          cfCourtOrderedCounty: null,
          contractorName: null,
          learnFareFEP: {},
          assignedWorker: { id: 211, firstName: 'Sandeep Reddy', middleInitial: '', lastName: 'Alalla', wamsId: 'alallsxgrl', workerId: 'XCT555' }
        }
      ],
      mciId: 4118060647,

      hasBeenThroughClientReg: null
    };
    const cachedModel = [];
    comp.participant.deserialize(model);
    mockAppService.user.agencyCode = 'FSC';
    fixture.detectChanges();
    mockParticipantService.modeForParticipationStatuses.subscribe(res => {
      if (res) {
        comp.isInEditMode = res.value.inEditView;
        expect(comp.participationStatuses[0].id).toEqual(33);
      }
    });
  });
});
