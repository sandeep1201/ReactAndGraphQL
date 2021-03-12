import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { PopClaimsRoutingModule } from './pop-claims.routing';
import { PopClaimsComponent } from './pop-claims.component';
import { WorkerLevelPopClaimsListComponent } from './worker-level/list/list.component';
import { PopClaimsEditComponent } from './edit/edit.component';
import { AgencyLevelPOPClaimsListComponent } from './agency-level/list/list.component';
import { AdjudicatorLevelListComponent } from './adjudicator-level/list/list.component';
import { POPClaimSingleEntryComponent } from './single-entry/single-entry.component';

@NgModule({
  declarations: [
    PopClaimsComponent,
    WorkerLevelPopClaimsListComponent,
    PopClaimsEditComponent,
    AgencyLevelPOPClaimsListComponent,
    POPClaimSingleEntryComponent,
    AdjudicatorLevelListComponent
  ],
  imports: [CommonModule, PopClaimsRoutingModule, SharedModule],
  entryComponents: [WorkerLevelPopClaimsListComponent, POPClaimSingleEntryComponent],
  providers: []
})
export class PopClaimsModule {}
