import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EconomicSupportComponent } from './economic-support.component';

describe('EconomicSupportComponent', () => {
  let component: EconomicSupportComponent;
  let fixture: ComponentFixture<EconomicSupportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EconomicSupportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EconomicSupportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
