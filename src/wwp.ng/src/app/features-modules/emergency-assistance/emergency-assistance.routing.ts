import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { MasterGuard } from 'src/app/shared/guards/master-guard';
import { Authorization } from 'src/app/shared/models/authorization';
import { EARequestHistoryComponent } from './ea-request-history/ea-request-history.component';
import { EARequestDetailsComponent } from './ea-request-history/single/single.component';
import { EARequestEditComponent } from './ea-request-history/edit/edit.component';
import { EARequestSections } from './models/ea-request-sections.enum';
import { EARequestDemographicsEditComponent } from './ea-request-history/edit/demographics.component';
import { EARequestEmergencyTypeEditComponent } from './ea-request-history/edit/emergency-type.component';
import { EARequestHouseholdMembersEditComponent } from './ea-request-history/edit/household-members.component';
import { EARequestHouseholdFinancialsEditComponent } from './ea-request-history/edit/household-financials.component';
import { EARequestAgencySummaryEditComponent } from './ea-request-history/edit/agency-summary.component';
import { InformalAssessmentGuard } from 'src/app/shared/guards/informal-assessment-guard';
import { EmergencyAssistanceApplicationGuard } from './guards/ea-application.guard';

const routes: Routes = [
  {
    path: 'ea-application-history',
    canActivate: [MasterGuard],
    data: { guards: ['ParticipantGuard', 'CoreAccessGuard', 'EmergenyAssistanceRequestHistoryGuard'], authorizations: [Authorization.canAccessEA_View] },
    children: [
      {
        path: '',
        component: EARequestHistoryComponent
      },
      {
        path: ':id',
        component: EARequestDetailsComponent,
        canActivate: [EmergencyAssistanceApplicationGuard]
      },
      {
        path: ':id/:mode',
        canActivate: [MasterGuard],
        data: { guards: ['EmergencyAssistanceGuard'] },
        canDeactivate: [InformalAssessmentGuard],
        component: EARequestEditComponent,
        children: [
          {
            path: EARequestSections.Demographics,
            component: EARequestDemographicsEditComponent
          },
          {
            path: EARequestSections.Emergency,
            component: EARequestEmergencyTypeEditComponent
          },
          {
            path: EARequestSections.Members,
            component: EARequestHouseholdMembersEditComponent
          },
          {
            path: EARequestSections.Financials,
            component: EARequestHouseholdFinancialsEditComponent
          },
          {
            path: EARequestSections.AgencySummary,
            component: EARequestAgencySummaryEditComponent
          },
          {
            path: '',
            component: EARequestDemographicsEditComponent
          }
        ]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: [EmergencyAssistanceApplicationGuard]
})
export class EmergencyAssistanceRoutingModule {}
