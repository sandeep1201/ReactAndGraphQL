import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChildrenFirstTrackingComponent } from './children-first-tracking.component';

describe('ChildrenFirstTrackingComponent', () => {
  let component: ChildrenFirstTrackingComponent;
  let fixture: ComponentFixture<ChildrenFirstTrackingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChildrenFirstTrackingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChildrenFirstTrackingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
