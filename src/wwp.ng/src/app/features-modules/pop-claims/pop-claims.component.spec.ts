import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { PopClaimsComponent } from './pop-claims.component';
import { NO_ERRORS_SCHEMA } from '@angular/core';

describe('PopClaimsComponent', () => {
  let component: PopClaimsComponent;
  let fixture: ComponentFixture<PopClaimsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PopClaimsComponent],
      schemas: [NO_ERRORS_SCHEMA]
    });
    fixture = TestBed.createComponent(PopClaimsComponent);
    component = fixture.componentInstance;
  }));

  beforeEach(() => {
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
  it('list component present', () => {
    expect(fixture.nativeElement.querySelector('[data-test="pop-claims-list"]').toBeTruthy());
  });

  //TODO:add test for the button
  //TODO:test for click on the button
});
