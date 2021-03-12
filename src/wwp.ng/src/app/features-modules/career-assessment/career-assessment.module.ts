import { CareerAssessmentService } from './services/career-assessment.service';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { CareerAssessmentRoutingModule } from './career-assessment-routing.module';
import { CareerAssessmentComponent } from './career-assessment.component';
import { CareerAssessmentListComponent } from './list/list.component';
import { CareerAssessmentEditComponent } from './edit/edit.component';
import { ContactsModule } from '../contacts/contacts.module';

@NgModule({
  imports: [CommonModule, CareerAssessmentRoutingModule, SharedModule, ContactsModule],
  declarations: [CareerAssessmentComponent, CareerAssessmentListComponent, CareerAssessmentEditComponent],
  providers: [CareerAssessmentService]
})
export class CareerAssessmentModule {}
