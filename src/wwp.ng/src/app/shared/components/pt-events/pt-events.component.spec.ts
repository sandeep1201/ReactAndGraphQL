import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PtEventsComponent } from './pt-events.component';

describe('PtEventsComponent', () => {
  let component: PtEventsComponent;
  let fixture: ComponentFixture<PtEventsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PtEventsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PtEventsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
