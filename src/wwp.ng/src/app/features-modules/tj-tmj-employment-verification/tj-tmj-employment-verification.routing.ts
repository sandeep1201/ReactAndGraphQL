import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { TjTmjEmploymentVerificationComponent } from './tj-tmj-employment-verification.component';

const routes: Routes = [
  {
    path: '',
    component: TjTmjEmploymentVerificationComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TjTmjEmploymentVerificationRoutingModule {}
