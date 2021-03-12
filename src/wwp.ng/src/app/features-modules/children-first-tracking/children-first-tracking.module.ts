import { ChildrenFirstTrackingRoutingModule } from './children-first-tracking-routing';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChildrenFirstTrackingComponent } from './children-first-tracking.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { ChildrenFirstTrackingService } from './services/children-first-tracking.service';
import { EmployabilityPlanService } from '../employability-plan/services/employability-plan.service';
import { ChildrenFirstTrackingEntryComponent } from './children-first-tracking-entry/children-first-tracking-entry.component';

@NgModule({
  declarations: [ChildrenFirstTrackingComponent, ChildrenFirstTrackingEntryComponent],
  providers: [ChildrenFirstTrackingService, EmployabilityPlanService],
  imports: [CommonModule, ChildrenFirstTrackingRoutingModule, SharedModule]
})
export class ChildrenFirstTrackingModule {}
