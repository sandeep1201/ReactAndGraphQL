import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { W2AuxiliaryApproversComponent } from './w2-auxiliary-approvers.component';

describe('W2AuxiliaryApproversComponent', () => {
  let component: W2AuxiliaryApproversComponent;
  let fixture: ComponentFixture<W2AuxiliaryApproversComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [W2AuxiliaryApproversComponent]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(W2AuxiliaryApproversComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
