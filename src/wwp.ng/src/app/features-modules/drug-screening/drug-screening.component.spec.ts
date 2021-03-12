import { SharedModule } from './../../shared/shared.module';
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DrugScreeningComponent } from './drug-screening.component';
import { DrugScreeningModule } from './drug-screening.module';
import { DrugScreeningService } from './services/drug-screening.service';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { RouterTestingModule } from '@angular/router/testing';
import { AppService } from 'src/app/core/services/app.service';
import { of, pipe, forkJoin } from 'rxjs';
import { CoreModule } from 'src/app/core/core.module';
import { By } from '@angular/platform-browser';

describe('DrugScreeningComponent', () => {
  let component: DrugScreeningComponent;
  let fixture: ComponentFixture<DrugScreeningComponent>;

  beforeEach(async(() => {
    //const drugScreeningServiceSpy = jasmine.createSpyObj('DrugScreeningService', ['getDrugScreeningData', 'saveDrugScreeningData', 'extractDrugScreeningData']);
    // const partServiceSpy = jasmine.createSpyObj('ParticipantService', ['getCachedParticipant']);

    TestBed.configureTestingModule({
      imports: [DrugScreeningModule, CoreModule, SharedModule, RouterTestingModule],
      providers: [
        { provide: DrugScreeningService, useClass: DrugScreeningServiceStub },
        { provide: ParticipantService, useClass: ParticipantServiceStub },
        { provide: AppService, useClass: AppServiceStub }
      ]
    })
      .compileComponents()
      .then(() => {
        fixture = TestBed.createComponent(DrugScreeningComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
      });
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('should contain a heading', () => {
    const heading = fixture.debugElement.query(By.css('u'));
    expect(heading.nativeElement.innerHTML).toEqual('In the past 12 months...');
  });
});

class DrugScreeningServiceStub {
  getDrugScreeningData() {
    return of({});
  }
}

class AppServiceStub {
  isUrlChangeBlocked: boolean;
  isDialogPresent: boolean;
}

class ParticipantServiceStub {
  getCachedParticipant() {
    return of({
      officeTransferId: null,
      officeTransferNumber: null,
      basicInfo: {
        firstName: 'JAMES',
        middleInitialName: '',
        lastName: 'MILIO',
        suffixName: '',
        dateOfBirth: '1990-01-20T00:00:00-06:00',
        age: 30,
        caseNumber: 9003958190.0,
        pinNumber: 9009906230.0,
        refugeeCode: 'No',
        refugeeEntryDate: null,
        genderIndicator: 'M',
        raceCode: 'American Indian / Alaskan',
        isHispanic: false,
        countryOfOrigin: null,
        mfWorkerId: null
      },
      addressInfo: {
        addressLine1: '1819  ABERG AVE',
        addressLine2: '',
        city: 'MADISON',
        state: 'WI',
        zipCode: '53704',
        primaryPhoneNumber: '',
        alternateAddress1: null,
        alternateAddress2: null,
        alternateCity: null,
        alternateState: null,
        alternateZipCode: null,
        alternatePrimaryPhoneNumber: null,
        emailAddress: null,
        livingArrangement: 'INDEPENDENT (HOME/APT/TRLR)'
      },
      officeCountyInfo: { countyNumber: 0, officeNumber: 0, wpGeoArea: 'Balance of State South West' },
      w2EligibilityInfo: {
        placementCode: null,
        daysInPlacement: null,
        stateLifeTimeLimit: null,
        epReviewDueDate: null,
        reviewDueDate: null,
        twoParentStatus: true,
        learnFareStatus: false,
        agStatuseCode: null,
        agSequenceNumber: null,
        eligibilityBeginDate: null,
        eligibilityEndDate: null,
        paymentBeginDate: null,
        paymentEndDate: null,
        agFailureReasonCode1: null,
        agFailureReasonCode2: null,
        agFailureReasonCode3: null,
        fsAgOpen: null,
        maAgOpen: null,
        fpwAgOpen: null,
        ccAgOpen: null,
        fsetStatus: null,
        childSupportStatus: null,
        moreThanSixIndv: null
      },
      enrolledProgramInfo: {
        id: null,
        participantId: null,
        enrolledProgramId: null,
        rfaNumber: null,
        programCode: 'WW',
        programCd: null,
        subProgramCode: null,
        enrollmentDate: null,
        disenrollmentDate: null,
        referralDate: null,
        status: 'Enrolled',
        isTransfer: null,
        statusDate: '2020-03-05T00:00:00-06:00',
        canDisenroll: null,
        officeCounty: null,
        agencyCode: null,
        agencyName: null,
        associatedAgencyCodes: null,
        associatedAgencyNames: null,
        officeId: null,
        officeNumber: null,
        assignedWorker: {
          id: null,
          wamsId: 'tim85851',
          workerId: 'TIMUAT',
          firstName: 'Tim',
          middleInitial: null,
          lastName: 'Nguyen',
          organization: null,
          isActive: null,
          wiuid: '2016101813295248'
        },
        courtOrderedDate: null,
        courtOrderedCounty: null,
        completionReasonId: null,
        completionReasonDetails: null,
        contractorId: null,
        contractorName: null,
        isVoluntary: false,
        learnFareFEP: {
          id: null,
          wamsId: null,
          workerId: null,
          firstName: null,
          middleInitial: null,
          lastName: null,
          organization: null,
          isActive: null,
          wiuid: '2016101813295248'
        },
        caseNumber: null
      },
      relatedPersons: [],
      otherDemographicInformation: {
        isInterpreterNeeded: null,
        hasAlias: false,
        homeLanguageName: null,
        isRufugee: null,
        monthOfEntry: null,
        countryOfOriginName: null,
        isTribalMember: null,
        tribeId: null,
        tribeName: null,
        tribeDetails: null,
        countyOfResidenceName: null,
        householdAddress: { streetAddress: null, aptUnit: null, city: null, state: null, country: null, zipCode: null },
        mailingAddress: { streetAddress: null, aptUnit: null, city: null, state: null, country: null, zipCode: null },
        primaryPhoneNumber: null,
        secondaryPhoneNumber: null,
        emailAddress: null,
        homeLessIndicator: null
      },
      cwwTransferDetails: { newFepId: null, fepOutOfSync: null },
      mostRecentFEPFromDB2_Result: { mostRecentMFFepId: 'XCTE9G', id: 1 }
    });
  }
}
