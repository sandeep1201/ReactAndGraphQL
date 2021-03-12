import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ParticipationEntryComponent } from './participation-entry.component';

describe('ParticipationEntryComponent', () => {
  let component: ParticipationEntryComponent;
  let fixture: ComponentFixture<ParticipationEntryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ParticipationEntryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ParticipationEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
