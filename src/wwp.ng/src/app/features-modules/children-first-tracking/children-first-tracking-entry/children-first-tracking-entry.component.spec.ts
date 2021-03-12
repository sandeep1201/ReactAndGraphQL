import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChildrenFirstTrackingEntryComponent } from './children-first-tracking-entry.component';

describe('ChildrenFirstTrackingEntryComponent', () => {
  let component: ChildrenFirstTrackingEntryComponent;
  let fixture: ComponentFixture<ChildrenFirstTrackingEntryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChildrenFirstTrackingEntryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChildrenFirstTrackingEntryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
