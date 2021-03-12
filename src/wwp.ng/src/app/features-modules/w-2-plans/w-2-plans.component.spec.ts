import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { W2PlansComponent } from './w-2-plans.component';

describe('W2PlansComponent', () => {
  let component: W2PlansComponent;
  let fixture: ComponentFixture<W2PlansComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [W2PlansComponent]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(W2PlansComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
