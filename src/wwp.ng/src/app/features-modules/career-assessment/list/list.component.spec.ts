import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CareerAssessmentListComponent } from './list.component';

describe('ListComponent', () => {
  let component: CareerAssessmentListComponent;
  let fixture: ComponentFixture<CareerAssessmentListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [CareerAssessmentListComponent]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CareerAssessmentListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
