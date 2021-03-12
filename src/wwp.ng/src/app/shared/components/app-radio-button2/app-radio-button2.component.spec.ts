import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AppRadioButton2Component } from './app-radio-button2.component';

describe('AppRadioButton2Component', () => {
  let component: AppRadioButton2Component;
  let fixture: ComponentFixture<AppRadioButton2Component>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppRadioButton2Component ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppRadioButton2Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
