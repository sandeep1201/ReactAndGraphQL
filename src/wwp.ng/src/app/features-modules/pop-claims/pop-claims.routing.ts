import { AdjudicatorLevelListComponent } from './adjudicator-level/list/list.component';
import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { PopClaimsComponent } from './pop-claims.component';
import { MasterGuard } from 'src/app/shared/guards/master-guard';
import { Authorization } from 'src/app/shared/models/authorization';

const routes: Routes = [
  {
    path: 'adjudicator',
    component: AdjudicatorLevelListComponent,
    canActivate: [MasterGuard],
    data: { guards: ['CoreAccessGuard'], authorizations: [Authorization.canAccessAdjudicatorPOP_View] }
  },
  {
    path: 'worker-level',
    component: PopClaimsComponent,
    canActivate: [MasterGuard],
    data: {
      guards: ['CoreAccessGuard'],
      authorizations: [Authorization.canAccessPOPClaims_View]
    }
  },
  {
    path: 'approver',
    component: PopClaimsComponent,
    canActivate: [MasterGuard],
    data: {
      guards: ['CoreAccessGuard'],
      authorizations: [Authorization.canAccessApproverPOP_View]
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PopClaimsRoutingModule {}
