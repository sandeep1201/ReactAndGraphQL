import { ComponentFixture, TestBed } from '@angular/core/testing';
import { NO_ERRORS_SCHEMA } from '@angular/core';
import { EpEmploymentsPageComponent } from './page.component';
import { ActivatedRoute, Router } from '@angular/router';
import { EmployabilityPlanService } from '../../services/employability-plan.service';

describe('EpEmploymentsPageComponent', () => {
  let comp: EpEmploymentsPageComponent;
  let fixture: ComponentFixture<EpEmploymentsPageComponent>;
  let employabilityPlanService, mockActivatedRoute, mockRouter;

  beforeEach(() => {
    mockRouter = jasmine.createSpyObj(['navigateByUrl']);
    mockActivatedRoute = {
      parent: {
        params: {
          pin: '2008983170',
          id: '789'
        }
      }
    };
    TestBed.configureTestingModule({
      declarations: [EpEmploymentsPageComponent],
      providers: [
        { provide: EmployabilityPlanService, useValue: employabilityPlanService },
        { provide: ActivatedRoute, useValue: mockActivatedRoute },
        { provide: Router, useValue: mockRouter }
      ],
      schemas: [NO_ERRORS_SCHEMA]
    });
    fixture = TestBed.createComponent(EpEmploymentsPageComponent);
    comp = fixture.componentInstance;
  });
  it('can load instance', () => {
    expect(comp).toBeTruthy();
  });
  it('length of filteredEmployments should be 2', () => {
    comp.pin = '1234567';
    comp.filteredEmployments = JSON.parse(
      '[{"id":0,"employmentInformationId":53440,"jobTypeId":2,"jobTypeName":"Unsubsidized","jobBeginDate":"07/15/2019","jobEndDate":null,"jobPosition":"asdas","companyName":"asd","location":{"description":"Madison, WI","longitude":null,"latitude":null,"city":"Madison","state":"WI","country":"United States","fullAddress":"1210 West Dayton Street","zipAddress":null,"googlePlaceId":"ChIJ_xkgOm1TBogRmEFIurX8DE4","addressPlaceId":null,"aptUnit":null},"wageHour":40.0,"isSelected":false},{"id":27,"employmentInformationId":53441,"jobTypeId":12,"jobTypeName":"Staffing Agency","jobBeginDate":"07/15/2019","jobEndDate":null,"jobPosition":"test2","companyName":"asd","location":{"description":"Madison, WI","longitude":null,"latitude":null,"city":"Madison","state":"WI","country":"United States","fullAddress":"1614 Fordem Avenue","zipAddress":null,"googlePlaceId":"ChIJ_xkgOm1TBogRmEFIurX8DE4","addressPlaceId":null,"aptUnit":null},"wageHour":30.0,"isSelected":true}]'
    );
    expect(comp.filteredEmployments.length).toEqual(2);
  });
  it('filteredEmployments 2 entry should be selected', () => {
    comp.pin = '1234567';
    comp.filteredEmployments = JSON.parse(
      '[{"id":0,"employmentInformationId":53440,"jobTypeId":2,"jobTypeName":"Unsubsidized","jobBeginDate":"07/15/2019","jobEndDate":null,"jobPosition":"asdas","companyName":"asd","location":{"description":"Madison, WI","longitude":null,"latitude":null,"city":"Madison","state":"WI","country":"United States","fullAddress":"1210 West Dayton Street","zipAddress":null,"googlePlaceId":"ChIJ_xkgOm1TBogRmEFIurX8DE4","addressPlaceId":null,"aptUnit":null},"wageHour":40.0,"isSelected":false},{"id":27,"employmentInformationId":53441,"jobTypeId":12,"jobTypeName":"Staffing Agency","jobBeginDate":"07/15/2019","jobEndDate":null,"jobPosition":"test2","companyName":"asd","location":{"description":"Madison, WI","longitude":null,"latitude":null,"city":"Madison","state":"WI","country":"United States","fullAddress":"1614 Fordem Avenue","zipAddress":null,"googlePlaceId":"ChIJ_xkgOm1TBogRmEFIurX8DE4","addressPlaceId":null,"aptUnit":null},"wageHour":30.0,"isSelected":true}]'
    );
    expect(comp.filteredEmployments[1].isSelected).toBeTruthy();
  });
});
