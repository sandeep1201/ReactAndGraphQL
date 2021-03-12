import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { WorkerTaskComponent } from 'src/app/features-modules/worker-task/worker-task.component';

describe('WorkerTaskComponent', () => {
  let component: WorkerTaskComponent;
  let fixture: ComponentFixture<WorkerTaskComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [WorkerTaskComponent]
    }).compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(WorkerTaskComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
