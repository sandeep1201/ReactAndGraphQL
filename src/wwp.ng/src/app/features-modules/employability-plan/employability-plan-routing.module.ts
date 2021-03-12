import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { EmploymentPlanComponent } from './employment-plan/employment-plan.component';
import { EPGuard } from 'src/app/features-modules/employability-plan/guards/ep-guard';
import { EmployabilityPlanDriverFlowComponent } from './driver-flow/driver-flow.component';
import { HistoricalGoalsComponent } from './historical-goals/historical-goals.component';
import { Authorization } from 'src/app/shared/models/authorization';
import { HistoricalActivitiesComponent } from './historical-activities/historical-activities.component';
import { EndEmployabilityPlanComponent } from './end-employability-plan/end-employability-plan.component';
import { GoalsPageComponent } from './goals/page/page.component';
import { EpEmploymentsPageComponent } from './employments/page/page.component';
import { ElapsedActivitiesComponent } from './ep-overview/elapsed-activities/elapsed-activities.component';
import { ActivitiesPageComponent } from './activities/page/page.component';
import { SupportiveServiceComponent } from './supportive-service/supportive-service.component';
import { EmployabilityPlanPageComponent } from './page/page.component';
import { ActivitySingleComponent } from './activities/single/single.component';
import { EpOverviewComponent } from './ep-overview/ep-overview.component';
import { InformalAssessmentGuard } from 'src/app/shared/guards/informal-assessment-guard';
import { MasterGuard } from 'src/app/shared/guards/master-guard';

const routes: Routes = [
  {
    path: 'list',
    component: EmployabilityPlanPageComponent
  },
  {
    path: 'historical-goals',
    component: HistoricalGoalsComponent,
    canActivate: [MasterGuard, EPGuard],
    data: { guards: ['ParticipantGuard', 'CoreAccessGuard'], authorizations: [Authorization.canAccessEmployabilityPlanApp_View] }
  },
  {
    path: 'historical-activities',
    component: HistoricalActivitiesComponent,
    canActivate: [MasterGuard, EPGuard],
    data: { guards: ['ParticipantGuard', 'CoreAccessGuard'], authorizations: [Authorization.canAccessEmployabilityPlanApp_View] }
  },
  {
    path: 'end-employability-plan',
    component: EndEmployabilityPlanComponent,
    canActivate: [MasterGuard, EPGuard],
    data: { guards: ['ParticipantGuard', 'CoreAccessGuard'], authorizations: [Authorization.canAccessEmployabilityPlanApp_View] }
  },
  {
    path: '',
    component: EmployabilityPlanDriverFlowComponent,
    canActivate: [MasterGuard],
    canDeactivate: [InformalAssessmentGuard],
    data: {
      guards: ['ParticipantGuard', 'CoreAccessGuard'],
      authorizations: [Authorization.canAccessEmployabilityPlanApp_View, Authorization.canAccessEmployabilityPlanApp_Edit]
    },
    children: [
      {
        path: ':id',
        component: EmploymentPlanComponent,
        canActivate: [EPGuard]
      },
      {
        path: 'goals/:id',
        component: GoalsPageComponent,
        canActivate: [EPGuard]
      },
      {
        path: 'employments/:id',
        component: EpEmploymentsPageComponent,
        canActivate: [EPGuard]
      },
      {
        path: 'elapsed-activities/:id',
        component: ElapsedActivitiesComponent,
        canActivate: [EPGuard]
        // canDeactivate: [SupportiveServiceGuard]
      },
      {
        path: 'activities/:id',
        component: ActivitiesPageComponent,
        canActivate: [EPGuard]
      },

      {
        path: 'supportive-service/:id',
        component: SupportiveServiceComponent,
        canActivate: [EPGuard]
        // canDeactivate: [SupportiveServiceGuard]
      }
    ]
  },
  {
    path: 'overview/:id',
    component: EpOverviewComponent,
    canActivate: [EPGuard],
    data: { authorizations: [Authorization.canAccessEmployabilityPlanApp_View] }
  },
  {
    path: 'single/activity/:id',
    component: ActivitySingleComponent,
    data: { authorizations: [Authorization.canAccessEmployabilityPlanApp_View] }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class EmployabilityPlanRoutingModule {}
