import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ParticipationCalendarComponent } from './participation-calendar.component';
import { ParticipationCalendarRoutingModule } from './participation-calendar.routing';
import { SharedModule } from 'src/app/shared/shared.module';
import { ParticipationEntryComponent } from './participation-entry/participation-entry.component';
import { MakeUpHoursRepeaterComponent } from './make-up-hours-repeater/make-up-hours-repeater.component';

@NgModule({
  declarations: [ParticipationCalendarComponent, ParticipationEntryComponent, MakeUpHoursRepeaterComponent],
  imports: [CommonModule, ParticipationCalendarRoutingModule, SharedModule]
})
export class ParticipationCalendarModule {}
