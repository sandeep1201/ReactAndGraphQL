import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { OrganizationInformationComponent } from './organization-information.component';

const routes: Routes = [
  {
    path: '',
    component: OrganizationInformationComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class OrganizationInformationRoutingModule {}
