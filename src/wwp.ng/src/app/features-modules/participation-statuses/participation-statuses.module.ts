import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { ParticipationStatusEditComponent } from './edit/edit.component';
import { ParticipationStatusListComponent } from './list/list.component';
import { ParticipationStatusesListPageComponent } from './list-page/list-page.component';

@NgModule({
  imports: [CommonModule, SharedModule],
  declarations: [ParticipationStatusEditComponent, ParticipationStatusListComponent, ParticipationStatusesListPageComponent],
  exports: [ParticipationStatusEditComponent, ParticipationStatusListComponent, ParticipationStatusesListPageComponent],
  entryComponents: [ParticipationStatusEditComponent]
})
export class ParticipationStatusModule {}
