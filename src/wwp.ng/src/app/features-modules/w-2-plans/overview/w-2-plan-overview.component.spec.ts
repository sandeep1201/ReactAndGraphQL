import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { W2PlanOverviewComponent } from './w-2-plan-overview.component';

describe('W2PlanOverviewComponent', () => {
  let component: W2PlanOverviewComponent;
  let fixture: ComponentFixture<W2PlanOverviewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [W2PlanOverviewComponent]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(W2PlanOverviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
