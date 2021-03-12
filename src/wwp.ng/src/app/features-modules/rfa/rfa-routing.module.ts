import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { RfaPageComponent } from './page/page.component';
import { RfaSingleComponent } from './single/single.component';
import { MasterGuard } from 'src/app/shared/guards/master-guard';
import { Authorization } from 'src/app/shared/models/authorization';

const routes: Routes = [
  {
    path: '',
    component: RfaPageComponent
  },
  {
    path: ':id',
    component: RfaSingleComponent,
    canActivate: [MasterGuard],
    data: { guards: ['ParticipantGuard', 'CoreAccessGuard'], authorizations: [Authorization.canAccessRfa_View] }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class RfaRoutingModule {}
