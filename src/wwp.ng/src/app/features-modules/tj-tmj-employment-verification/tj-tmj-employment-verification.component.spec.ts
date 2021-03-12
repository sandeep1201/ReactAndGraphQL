import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TjTmjEmploymentVerificationComponent } from './tj-tmj-employment-verification.component';

describe('TjTmjEmploymentVerificationComponent', () => {
  let component: TjTmjEmploymentVerificationComponent;
  let fixture: ComponentFixture<TjTmjEmploymentVerificationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TjTmjEmploymentVerificationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TjTmjEmploymentVerificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
