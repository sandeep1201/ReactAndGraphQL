import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EAIPVEditComponent } from './edit.component';

describe('EditComponent', () => {
  let component: EAIPVEditComponent;
  let fixture: ComponentFixture<EAIPVEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [EAIPVEditComponent]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EAIPVEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
