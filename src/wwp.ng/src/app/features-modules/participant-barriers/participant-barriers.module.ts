import { ModuleWithProviders, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { ContactsModule } from '../contacts/contacts.module';
import { BarriersAccommodationsRepeaterComponent } from './accommodations/accommodations.component';
import { ParticipantBarriersEditComponent } from './edit/edit.component';
import { ParticipantBarriersEmbedComponent } from './embed/embed.component';
import { BarriersFormalAssessmentRepeaterComponent } from './formal-assessments/formal-assessments.component';
import { ParticipantBarriersListComponent } from './list/list.component';
import { ParticipantBarriersListPageComponent } from './list-page/list-page.component';
import { ParticipantBarriersSingleComponent } from './single/single.component';
import { ParticipantBarrierAppService } from '../../shared/services/participant-barrier-app.service';
import { ParticipantBarriersAppGuard } from './guards/participant-barriers-app-guard';

@NgModule({
  imports: [CommonModule, SharedModule, ContactsModule],
  declarations: [
    BarriersAccommodationsRepeaterComponent,
    ParticipantBarriersEditComponent,
    ParticipantBarriersEmbedComponent,
    BarriersFormalAssessmentRepeaterComponent,
    ParticipantBarriersListComponent,
    ParticipantBarriersListPageComponent,
    ParticipantBarriersSingleComponent
  ],
  providers: [ParticipantBarriersAppGuard],
  exports: [
    BarriersAccommodationsRepeaterComponent,
    ParticipantBarriersEditComponent,
    ParticipantBarriersEmbedComponent,
    BarriersFormalAssessmentRepeaterComponent,
    ParticipantBarriersListComponent,
    ParticipantBarriersListPageComponent,
    ParticipantBarriersSingleComponent
  ]
})
export class ParticipantBarriersModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: ParticipantBarriersModule,
      providers: [ParticipantBarrierAppService]
    };
  }
}
