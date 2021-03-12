import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PopClaimsEditComponent } from './edit.component';

describe('EditComponent', () => {
  let component: PopClaimsEditComponent;
  let fixture: ComponentFixture<PopClaimsEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PopClaimsEditComponent]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PopClaimsEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
