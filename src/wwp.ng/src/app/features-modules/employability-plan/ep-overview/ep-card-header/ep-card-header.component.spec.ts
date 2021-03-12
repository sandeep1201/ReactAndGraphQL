import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EpCardHeaderComponent } from './ep-card-header.component';

describe('EpCardHeaderComponent', () => {
  let component: EpCardHeaderComponent;
  let fixture: ComponentFixture<EpCardHeaderComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EpCardHeaderComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EpCardHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
