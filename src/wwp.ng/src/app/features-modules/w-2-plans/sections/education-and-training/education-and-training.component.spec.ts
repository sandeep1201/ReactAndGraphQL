import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EducationAndTrainingComponent } from './education-and-training.component';

describe('EducationAndTrainingComponent', () => {
  let component: EducationAndTrainingComponent;
  let fixture: ComponentFixture<EducationAndTrainingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EducationAndTrainingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EducationAndTrainingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
