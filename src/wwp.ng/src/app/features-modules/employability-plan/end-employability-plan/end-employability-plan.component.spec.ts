import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EndEmployabilityPlanComponent } from './end-employability-plan.component';

describe('EndEmployabilityPlanComponent', () => {
  let component: EndEmployabilityPlanComponent;
  let fixture: ComponentFixture<EndEmployabilityPlanComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EndEmployabilityPlanComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EndEmployabilityPlanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
