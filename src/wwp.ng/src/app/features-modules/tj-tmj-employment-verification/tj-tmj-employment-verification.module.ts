import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { TjTmjEmploymentVerificationRoutingModule } from './tj-tmj-employment-verification.routing';
import { TjTmjEmploymentVerificationComponent } from './tj-tmj-employment-verification.component';
import { TJTMJEmploymentVerificationService } from './services/tj-tmj-employment-verification.service';

@NgModule({
  declarations: [TjTmjEmploymentVerificationComponent],
  imports: [CommonModule, TjTmjEmploymentVerificationRoutingModule, SharedModule],
  providers: [TJTMJEmploymentVerificationService]
})
export class TjTmjEmploymentVerificationModule {}
