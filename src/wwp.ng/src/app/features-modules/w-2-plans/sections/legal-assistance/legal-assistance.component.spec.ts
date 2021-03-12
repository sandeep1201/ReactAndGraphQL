import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LegalAssistanceComponent } from './legal-assistance.component';

describe('LegalAssistanceComponent', () => {
  let component: LegalAssistanceComponent;
  let fixture: ComponentFixture<LegalAssistanceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LegalAssistanceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LegalAssistanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
