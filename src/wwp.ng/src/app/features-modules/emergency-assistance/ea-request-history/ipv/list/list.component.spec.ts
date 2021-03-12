import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EAIPVListComponent } from './list.component';

describe('ListComponent', () => {
  let component: EAIPVListComponent;
  let fixture: ComponentFixture<EAIPVListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [EAIPVListComponent]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EAIPVListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
