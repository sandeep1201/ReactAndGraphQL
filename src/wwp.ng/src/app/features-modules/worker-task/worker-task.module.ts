import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { WorkerTaskRoutingModule } from './worker-task-routing.module';
import { WorkerTaskComponent } from './worker-task.component';

@NgModule({
  imports: [CommonModule, SharedModule, WorkerTaskRoutingModule],
  declarations: [WorkerTaskComponent]
})
export class WorkerTaskModule {}
