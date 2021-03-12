import { EPGuard } from './guards/ep-guard';
import { ContactsModule } from './../contacts/contacts.module';
//modules
import { CommonModule } from '@angular/common';
import { SharedModule } from 'src/app/shared/shared.module';
import { NgModule } from '@angular/core';

//services
import { SupportiveServiceService } from './services/supportive-service.service';
import { EmployabilityPlanService } from './services/employability-plan.service';

// Components
import { ActivitiesPageComponent } from './activities/page/page.component';
import { ActivitySingleComponent } from './activities/single/single.component';
import { ActivitiesEditComponent } from './activities/edit/edit.component';
import { ActivitiesListComponent } from './activities/list/list.component';
import { ActivityOverviewComponent } from './ep-overview/activity-overview/activity-overview.component';
import { ElapsedActivitiesComponent } from './ep-overview/elapsed-activities/elapsed-activities.component';
import { EmployabilityListComponent } from './list/list.component';
import { EmploymentPlanComponent } from './employment-plan/employment-plan.component';
import { EmployabilityPlanDriverFlowComponent } from './driver-flow/driver-flow.component';
import { EmployabilityPlanOverviewComponent } from './ep-overview/employability-plan-overview/employability-plan-overview.component';
import { EmployabilityPlanPageComponent } from './page/page.component';
import { EpCardHeaderComponent } from './ep-overview/ep-card-header/ep-card-header.component';
import { EpEmploymentsListComponent } from './employments/list/list.component';
import { EpEmploymentsPageComponent } from './employments/page/page.component';
import { EpEmploymentsOverviewComponent } from './ep-overview/employments-overview/employments-overview.component';
import { EpOverviewComponent } from './ep-overview/ep-overview.component';
import { EndEmployabilityPlanComponent } from './end-employability-plan/end-employability-plan.component';
import { GoalsEditComponent } from './goals/edit/edit.component';
import { GoalsListComponent } from './goals/list/list.component';
import { GoalsOverviewComponent } from './ep-overview/goals-overview/goals-overview.component';
import { GoalsPageComponent } from './goals/page/page.component';
import { GoalStepRepeaterComponent } from './goals/goal-step-repeater/goal-step-repeater.component';
import { HistoricalActivitiesComponent } from './historical-activities/historical-activities.component';
import { HistoricalGoalsComponent } from './historical-goals/historical-goals.component';
import { SupportiveServiceComponent } from './supportive-service/supportive-service.component';
import { SupportiveServicesOverviewComponent } from './ep-overview/supportive-services-overview/supportive-services-overview.component';
import { ScheduleRepeaterComponent } from './activities/edit/schedule-repeater/schedule-repeater.component';
import { ServiceRepeaterComponent } from './supportive-service/service-repeater/service-repeater.component';
import { EmployabilityPlanRoutingModule } from './employability-plan-routing.module';
import { EndGoalComponent } from './goals/end-goal/end-goal.component';
import { ChildCareAuthorizationsComponent } from './childcare-authorizations/childcare-authorizations.component';

@NgModule({
  imports: [CommonModule, SharedModule, ContactsModule, EmployabilityPlanRoutingModule],
  declarations: [
    ActivitiesPageComponent,
    ActivitySingleComponent,
    ActivitiesEditComponent,
    ActivitiesListComponent,
    ActivityOverviewComponent,
    ElapsedActivitiesComponent,
    EmployabilityListComponent,
    EmploymentPlanComponent,
    EmployabilityPlanDriverFlowComponent,
    EmployabilityPlanOverviewComponent,
    EmployabilityPlanPageComponent,
    EndGoalComponent,
    EpCardHeaderComponent,
    EpEmploymentsListComponent,
    EpEmploymentsPageComponent,
    EpEmploymentsOverviewComponent,
    EpOverviewComponent,
    EndEmployabilityPlanComponent,
    GoalsEditComponent,
    GoalsListComponent,
    GoalsOverviewComponent,
    GoalsPageComponent,
    GoalStepRepeaterComponent,
    HistoricalActivitiesComponent,
    HistoricalGoalsComponent,
    SupportiveServiceComponent,
    SupportiveServicesOverviewComponent,
    ScheduleRepeaterComponent,
    ServiceRepeaterComponent,
    ChildCareAuthorizationsComponent
  ],
  providers: [EmployabilityPlanService, SupportiveServiceService, EPGuard],
  exports: [
    ActivitiesPageComponent,
    ActivitySingleComponent,
    ActivitiesEditComponent,
    ActivitiesListComponent,
    ActivityOverviewComponent,
    ElapsedActivitiesComponent,
    EmployabilityListComponent,
    EmploymentPlanComponent,
    EmployabilityPlanDriverFlowComponent,
    EmployabilityPlanOverviewComponent,
    EmployabilityPlanPageComponent,
    EpCardHeaderComponent,
    EpEmploymentsListComponent,
    EpEmploymentsPageComponent,
    EpEmploymentsOverviewComponent,
    EpOverviewComponent,
    EndEmployabilityPlanComponent,
    GoalsEditComponent,
    GoalsListComponent,
    GoalsOverviewComponent,
    GoalsPageComponent,
    GoalStepRepeaterComponent,
    HistoricalActivitiesComponent,
    HistoricalGoalsComponent,
    SupportiveServiceComponent,
    SupportiveServicesOverviewComponent,
    ScheduleRepeaterComponent,
    ServiceRepeaterComponent
  ]
})
export class EmployabilityPlanModule {}
