import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { WorkerTaskEditComponent } from './edit.component';

describe('WorkerTaskEditComponent', () => {
  let component: WorkerTaskEditComponent;
  let fixture: ComponentFixture<WorkerTaskEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [WorkerTaskEditComponent]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkerTaskEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
