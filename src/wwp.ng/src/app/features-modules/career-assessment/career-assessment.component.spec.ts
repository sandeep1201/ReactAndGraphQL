import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CareerAssessmentComponent } from './career-assessment.component';

describe('CareerAssessmentComponent', () => {
  let component: CareerAssessmentComponent;
  let fixture: ComponentFixture<CareerAssessmentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CareerAssessmentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CareerAssessmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
