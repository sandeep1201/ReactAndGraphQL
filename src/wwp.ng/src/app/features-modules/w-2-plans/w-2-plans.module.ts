import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { W2PlansRoutingModule } from './w-2-plans.routing';
import { W2PlansComponent } from './w-2-plans.component';
import { W2PlansService } from './services/w-2-plans.service';
import { W2PlanOverviewComponent } from './overview/w-2-plan-overview.component';
import { EditComponent } from './edit/edit.component';
import { EmergencyNeedsComponent } from './sections/emergency-needs/emergency-needs.component';
import { EmploymentSupportComponent } from './sections/employment-support/employment-support.component';
import { HousingComponent } from './sections/housing/housing.component';
import { EconomicSupportComponent } from './sections/economic-support/economic-support.component';
import { ChildCareComponent } from './sections/child-care/child-care.component';
import { TransportationComponent } from './sections/transportation/transportation.component';
import { LegalAssistanceComponent } from './sections/legal-assistance/legal-assistance.component';
import { EducationAndTrainingComponent } from './sections/education-and-training/education-and-training.component';
import { HealthComponent } from './sections/health/health.component';
import { OtherNeedsComponent } from './sections/other-needs/other-needs.component';
import { MoneyManagementComponent } from './sections/money-management/money-management.component';
import { BaseW2PlansComponent } from './base-w2-plans/base-w2-plans.component';
import { ResourcesRepeaterComponent } from './resources-repeater/resources-repeater.component';

@NgModule({
  declarations: [
    W2PlansComponent,
    W2PlanOverviewComponent,
    EditComponent,
    EmergencyNeedsComponent,
    EmploymentSupportComponent,
    HousingComponent,
    EconomicSupportComponent,
    ChildCareComponent,
    TransportationComponent,
    LegalAssistanceComponent,
    EducationAndTrainingComponent,
    HealthComponent,
    OtherNeedsComponent,
    MoneyManagementComponent,
    BaseW2PlansComponent,
    ResourcesRepeaterComponent
  ],
  imports: [CommonModule, SharedModule, W2PlansRoutingModule],
  providers: [W2PlansService],
  entryComponents: [BaseW2PlansComponent]
})
export class W2PlansModule {}
