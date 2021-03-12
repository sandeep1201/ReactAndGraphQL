import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MakeUpHoursRepeaterComponent } from './make-up-hours-repeater.component';

describe('MakeUpHoursRepeaterComponent', () => {
  let component: MakeUpHoursRepeaterComponent;
  let fixture: ComponentFixture<MakeUpHoursRepeaterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MakeUpHoursRepeaterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MakeUpHoursRepeaterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
