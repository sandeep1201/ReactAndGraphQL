import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WorkerTaskComponent } from './worker-task.component';

const routes: Routes = [
  {
    path: '',
    component: WorkerTaskComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class WorkerTaskRoutingModule {}
