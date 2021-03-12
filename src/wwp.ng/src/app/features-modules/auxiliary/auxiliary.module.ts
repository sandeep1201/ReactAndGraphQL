import { SharedModule } from 'src/app/shared/shared.module';
import { NgModule } from '@angular/core';
import { AuxiliaryApproverListComponent } from './approver-list/approver-list.component';
import { CommonModule } from '@angular/common';
import { AuxiliaryWorkerListComponent } from './worker-list/worker-list.component';
import { AuxiliaryComponent } from './auxiliary.component';
import { AuxiliaryRoutingModule } from './auxiliary.routing';
import { AuxiliaryEditComponent } from './edit/edit.component';
import { AuxiliaryService } from './services/auxiliary.service';
import { W2AuxiliaryApproversComponent } from './w2-auxiliary-approvers/w2-auxiliary-approvers.component';

@NgModule({
  declarations: [AuxiliaryComponent, AuxiliaryApproverListComponent, AuxiliaryWorkerListComponent, AuxiliaryEditComponent, W2AuxiliaryApproversComponent],
  imports: [CommonModule, AuxiliaryRoutingModule, SharedModule],
  providers: [AuxiliaryService]
})
export class AuxiliaryModule {}
