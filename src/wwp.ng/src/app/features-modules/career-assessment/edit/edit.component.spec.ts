import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CareerAssessmentEditComponent } from './edit.component';

describe('EditComponent', () => {
  let component: CareerAssessmentEditComponent;
  let fixture: ComponentFixture<CareerAssessmentEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [CareerAssessmentEditComponent]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CareerAssessmentEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
