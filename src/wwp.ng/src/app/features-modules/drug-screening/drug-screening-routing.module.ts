import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DrugScreeningComponent } from './drug-screening.component';

const routes: Routes = [
  {
    path: '',
    component: DrugScreeningComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DrugScreeningRoutingModule {}
