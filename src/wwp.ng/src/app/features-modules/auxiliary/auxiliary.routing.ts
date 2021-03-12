import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuxiliaryComponent } from './auxiliary.component';

const routes: Routes = [
  {
    path: '',
    component: AuxiliaryComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuxiliaryRoutingModule {}
