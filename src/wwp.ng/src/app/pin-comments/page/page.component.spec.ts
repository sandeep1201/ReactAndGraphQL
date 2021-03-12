import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PinCommentsPageComponent } from './page.component';

describe('PinCommentsPageComponent', () => {
  let component: PinCommentsPageComponent;
  let fixture: ComponentFixture<PinCommentsPageComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PinCommentsPageComponent]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PinCommentsPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
