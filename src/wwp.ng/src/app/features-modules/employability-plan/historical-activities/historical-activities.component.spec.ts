import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HistoricalActivitiesComponent } from './historical-activities.component';

describe('HistoricalActivitiesComponent', () => {
  let component: HistoricalActivitiesComponent;
  let fixture: ComponentFixture<HistoricalActivitiesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HistoricalActivitiesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HistoricalActivitiesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
