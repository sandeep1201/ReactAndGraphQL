import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EpEventsComponent } from './ep-events.component';

describe('EpEventsComponent', () => {
  let component: EpEventsComponent;
  let fixture: ComponentFixture<EpEventsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EpEventsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EpEventsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
