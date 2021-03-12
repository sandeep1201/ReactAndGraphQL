import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChildCareAuthorizationsComponent } from './childcare-authorizations.component';

describe('ChildcareAuthorizationsComponent', () => {
  let component: ChildCareAuthorizationsComponent;
  let fixture: ComponentFixture<ChildCareAuthorizationsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ChildCareAuthorizationsComponent]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChildCareAuthorizationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
