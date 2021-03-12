import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CtEventsComponent } from './ct-events.component';

describe('CtEventsComponent', () => {
  let component: CtEventsComponent;
  let fixture: ComponentFixture<CtEventsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CtEventsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CtEventsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
