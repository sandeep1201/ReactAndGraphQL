import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { POPClaimSingleEntryComponent } from './single-entry.component';

describe('SingleEntryComponent', () => {
  let component: POPClaimSingleEntryComponent;
  let fixture: ComponentFixture<POPClaimSingleEntryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [POPClaimSingleEntryComponent]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(POPClaimSingleEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
