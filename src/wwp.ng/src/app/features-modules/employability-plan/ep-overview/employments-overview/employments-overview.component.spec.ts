import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EpEmploymentsOverviewComponent } from './employments-overview.component';

describe('EpEmploymentsOverviewComponent', () => {
  let component: EpEmploymentsOverviewComponent;
  let fixture: ComponentFixture<EpEmploymentsOverviewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [EpEmploymentsOverviewComponent]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EpEmploymentsOverviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
