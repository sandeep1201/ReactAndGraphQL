import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmploymentSupportComponent } from './employment-support.component';

describe('EmploymentSupportComponent', () => {
  let component: EmploymentSupportComponent;
  let fixture: ComponentFixture<EmploymentSupportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmploymentSupportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmploymentSupportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
