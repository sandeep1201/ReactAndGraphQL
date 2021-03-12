import { HealthComponent } from './sections/health/health.component';
import { EducationAndTrainingComponent } from './sections/education-and-training/education-and-training.component';
import { LegalAssistanceComponent } from './sections/legal-assistance/legal-assistance.component';
import { TransportationComponent } from './sections/transportation/transportation.component';
import { ChildCareComponent } from './sections/child-care/child-care.component';
import { MoneyManagementComponent } from './sections/money-management/money-management.component';
import { EconomicSupportComponent } from './sections/economic-support/economic-support.component';
import { HousingComponent } from './sections/housing/housing.component';
import { EmploymentSupportComponent } from './sections/employment-support/employment-support.component';
import { W2PlanOverviewComponent } from './overview/w-2-plan-overview.component';
import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { W2PlansComponent } from './w-2-plans.component';
import { Authorization } from 'src/app/shared/models/authorization';
import { CoreAccessGuard } from 'src/app/shared/guards/core-access-guard';
import { EditComponent } from './edit/edit.component';
import { EmergencyNeedsComponent } from './sections/emergency-needs/emergency-needs.component';
import { OtherNeedsComponent } from './sections/other-needs/other-needs.component';
import { W2PlanSections } from './enums/w-2-plans-sections.enum';

const routes: Routes = [
  {
    path: '',
    component: W2PlansComponent
  },
  {
    path: 'overview/:id',
    component: W2PlanOverviewComponent
  },
  {
    path: 'edit/:id',
    component: EditComponent,
    children: [
      {
        path: W2PlanSections.EmergencyNeeds,
        component: EmergencyNeedsComponent
      },
      {
        path: W2PlanSections.EmploymentSupport,
        component: EmploymentSupportComponent
      },
      {
        path: W2PlanSections.Housing,
        component: HousingComponent
      },
      {
        path: W2PlanSections.EconomicSupport,
        component: EconomicSupportComponent
      },
      {
        path: W2PlanSections.MoneyManagement,
        component: MoneyManagementComponent
      },
      {
        path: W2PlanSections.ChildCare,
        component: ChildCareComponent
      },
      {
        path: W2PlanSections.Transportation,
        component: TransportationComponent
      },
      {
        path: W2PlanSections.LegalAssistance,
        component: LegalAssistanceComponent
      },
      {
        path: W2PlanSections.EducationAndTraining,
        component: EducationAndTrainingComponent
      },
      {
        path: W2PlanSections.Health,
        component: HealthComponent
      },
      {
        path: W2PlanSections.OtherNeeds,
        component: OtherNeedsComponent
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class W2PlansRoutingModule {}
