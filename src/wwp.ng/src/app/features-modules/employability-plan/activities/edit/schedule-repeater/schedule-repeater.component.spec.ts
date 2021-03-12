import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ScheduleRepeaterComponent } from './schedule-repeater.component';

describe('ScheduleRepeaterComponent', () => {
  let component: ScheduleRepeaterComponent;
  let fixture: ComponentFixture<ScheduleRepeaterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ScheduleRepeaterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ScheduleRepeaterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
