import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HistoricalGoalsComponent } from './historical-goals.component';

describe('HistoricalGoalsComponent', () => {
  let component: HistoricalGoalsComponent;
  let fixture: ComponentFixture<HistoricalGoalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HistoricalGoalsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HistoricalGoalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
