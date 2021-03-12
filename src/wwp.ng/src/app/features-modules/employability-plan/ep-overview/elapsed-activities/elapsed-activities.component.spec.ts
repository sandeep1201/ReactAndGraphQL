import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ElapsedActivitiesComponent } from './elapsed-activities.component';

describe('ElapsedActivitiesComponent', () => {
  let component: ElapsedActivitiesComponent;
  let fixture: ComponentFixture<ElapsedActivitiesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ElapsedActivitiesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ElapsedActivitiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
