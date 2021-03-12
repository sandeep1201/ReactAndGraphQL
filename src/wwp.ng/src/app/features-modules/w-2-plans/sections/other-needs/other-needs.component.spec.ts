import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OtherNeedsComponent } from './other-needs.component';

describe('OtherNeedsComponent', () => {
  let component: OtherNeedsComponent;
  let fixture: ComponentFixture<OtherNeedsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OtherNeedsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OtherNeedsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
