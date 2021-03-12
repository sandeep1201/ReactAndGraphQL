import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BaseW2PlansComponent } from './base-w2-plans.component';

describe('BaseW2PlansComponent', () => {
  let component: BaseW2PlansComponent;
  let fixture: ComponentFixture<BaseW2PlansComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BaseW2PlansComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BaseW2PlansComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
