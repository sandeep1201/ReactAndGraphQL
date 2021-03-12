import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NonParticipationDetailsComponent } from './non-participation-details.component';
import { NonParticipationDetailsRoutingModule } from './non-participation-details.routing';
import { SharedModule } from 'src/app/shared/shared.module';

@NgModule({
  declarations: [NonParticipationDetailsComponent],
  imports: [CommonModule, NonParticipationDetailsRoutingModule, SharedModule]
})
export class NonParticipationDetailsModule {}
