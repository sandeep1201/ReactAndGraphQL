import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CancelSaveFooterComponent } from './cancel-save-footer.component';

describe('CancelSaveFooterComponent', () => {
  let component: CancelSaveFooterComponent;
  let fixture: ComponentFixture<CancelSaveFooterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CancelSaveFooterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CancelSaveFooterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
