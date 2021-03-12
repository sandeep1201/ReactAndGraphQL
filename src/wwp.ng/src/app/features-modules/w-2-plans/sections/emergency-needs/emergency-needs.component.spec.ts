import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmergencyNeedsComponent } from './emergency-needs.component';

describe('EmergencyNeedsComponent', () => {
  let component: EmergencyNeedsComponent;
  let fixture: ComponentFixture<EmergencyNeedsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmergencyNeedsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmergencyNeedsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
