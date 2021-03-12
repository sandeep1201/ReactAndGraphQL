import { JobReadinessService } from './services/job-readiness.service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { ActionsNeededModule } from '../actions-needed/actions-needed.module';
import { JobReadinessRoutingModule } from './job-readiness-routing.module';
import { JobReadinessComponent } from './job-readiness.component';

@NgModule({
  imports: [CommonModule, JobReadinessRoutingModule, SharedModule, ActionsNeededModule],
  declarations: [JobReadinessComponent],
  providers: [JobReadinessService]
})
export class JobReadinessModule {}
