import { DrugScreeningModule } from './features-modules/drug-screening/drug-screening.module';
import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
// Please do not remove these modules we have to include these here so that angular would know about these module during complining lazy loading still works as intended even we import these modules here.
import { RfaModule } from './features-modules/rfa/rfa.module';
import { OrganizationInformationModule } from './features-modules/organization-info/organization-information.module';
import { JobReadinessModule } from './features-modules/job-readiness/job-readiness.module';
import { ActionsNeededModule } from './features-modules/actions-needed/actions-needed.module';
import { EmployabilityPlanModule } from './features-modules/employability-plan/employability-plan.module';
import { ContactsModule } from './features-modules/contacts/contacts.module';
import { CareerAssessmentModule } from './features-modules/career-assessment/career-assessment.module';
import { WorkHistoryModule } from './features-modules/work-history/work-history.module';
import { ParticipantBarriersModule } from './features-modules/participant-barriers/participant-barriers.module';
import { ParticipationCalendarModule } from './features-modules/participation-calendar/participation-calendar.module';
import { NonParticipationDetailsModule } from './features-modules/non-participation-details/non-participation-details.module';
import { ChildrenFirstTrackingModule } from './features-modules/children-first-tracking/children-first-tracking.module';

import { Authorization } from './shared/models/authorization';
import { ChildYouthSupportsEditComponent } from './participant/informal-assessment/edit/child-youth-supports.component';
import { HomePageComponent } from './home-page/home-page.component';
import { EditComponent as EditInformalAssessmentComponent } from './participant/informal-assessment/edit/edit.component';
import { EducationHistoryEditComponent } from './participant/informal-assessment/edit/education-history.component';
import { FamilyBarriersEditComponent } from './participant/informal-assessment/edit/family-barriers.component';
import { HousingEditComponent } from './participant/informal-assessment/edit/housing.component';
import { LanguagesEditComponent } from './participant/informal-assessment/edit/languages.component';
import { LegalIssuesEditComponent } from './participant/informal-assessment/edit/legal-issues.component';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { MilitaryTrainingEditComponent } from './participant/informal-assessment/edit/military-training.component';
import { ParticipantBarriersEditComponent } from './participant/informal-assessment/edit/participant-barriers.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { PostSecondaryEducationEditComponent } from './participant/informal-assessment/edit/post-secondary-education.component';
import { PageHelpComponent } from './help/help.component';
import { StartComponent } from './start/start.component';
import { SummaryComponent as InformalAssessmentSummaryComponent } from './participant/informal-assessment/summary/summary.component';
import { SummaryComponent as ParticipantSummaryComponent } from './participant/summary/summary.component';
import { TestScoresListPageComponent } from './features-modules/test-scores/list-page/list-page.component';
import { TransportationEditComponent } from './participant/informal-assessment/edit/transportation.component';
import { WorkHistoryGatepostEditComponent } from './participant/informal-assessment/edit/work-history.component';
import { WorkProgramsEditComponent } from './participant/informal-assessment/edit/work-programs.component';
import { SummaryComponent as TimeLimitsSummaryComponent } from './time-limits/summary/summary.component';
import { ExtensionListPageComponent } from './time-limits/extensions/list-page/list-page.component';
import { ExtensionDetailComponent } from './time-limits/extensions/detail/detail.component';
import { NcpEditComponent } from './participant/informal-assessment/edit/ncp.component';
import { NcprEditComponent } from './participant/informal-assessment/edit/ncpr.component';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';
import { ClearanceComponent } from './clearance/clearance.component';
import { ParticipantService } from './shared/services/participant.service';
import { ReportsComponent } from './webi/reports/reports.component';
import { TimeLimitsAppGuard } from './shared/guards/time-limits-app.guard';
import { ParticipantGuard } from './shared/guards/participant-guard';
import { CoreAccessGuard } from './shared/guards/core-access-guard';
import { ClientRegistrationGuard } from './shared/guards/client-registration-guard';
import { InformalAssessmentGuard } from './shared/guards/informal-assessment-guard';
import { ParticipantBarriersAppGuard } from './features-modules/participant-barriers/guards/participant-barriers-app-guard';
import { AuthGuard } from './shared/guards/auth-guard';
import { RegistrationComponent } from './enrollment/registration/registration.component';
import { ParticipationStatusesListPageComponent } from './features-modules/participation-statuses/list-page/list-page.component';
import { OrganizationInformationComponent } from './features-modules/organization-info/organization-information.component';
import { OrgInfoGuard } from './shared/guards/org-info-guard';
import { MasterGuard } from './shared/guards/master-guard';
import { PinCommentsPageComponent } from './pin-comments/page/page.component';
import { ActionsNeededComponent } from './features-modules/actions-needed/actions-needed.component';
import { ContactsListPageComponent } from './features-modules/contacts/list-page/list-page.component';
import { ContactDetailComponent } from './features-modules/contacts/detail/detail.component';
import { WorkHistoryListPageComponent } from './features-modules/work-history/list-page/list-page.component';
import { WorkHistorySingleComponent } from './features-modules/work-history/single/single.component';
import { ParticipantBarriersListPageComponent } from './features-modules/participant-barriers/list-page/list-page.component';
import { ParticipantBarriersSingleComponent } from './features-modules/participant-barriers/single/single.component';
import { NonParticipationDetailsGuard } from './features-modules/non-participation-details/guards/non-participation-details-guard';
import { ParticipationCalendarGuard } from './features-modules/participation-calendar/guards/participation-calendar.guard';
import { HourlyEntryListComponent } from './features-modules/work-history/weekly-hours-worked/list/list.component';

const appRoutes: Routes = [
  { path: '', redirectTo: '/start', pathMatch: 'full' },
  { path: 'start', component: StartComponent },
  { path: 'login', component: LoginComponent },
  { path: 'logout', component: LogoutComponent },
  { path: 'unauthorized', component: UnauthorizedComponent },
  { path: 'home', component: HomePageComponent, canActivate: [AuthGuard] },
  { path: 'help', component: PageHelpComponent },
  {
    path: 'organization-information',
    loadChildren: () => import('./features-modules/organization-info/organization-information.module').then(m => m.OrganizationInformationModule),
    data: { guards: ['CoreAccessGuard'], authorizations: [Authorization.canAccessOrganizationInformation] },
    canActivate: [MasterGuard, OrgInfoGuard]
  },
  {
    path: 'worker-task',
    loadChildren: () => import('./features-modules/worker-task/worker-task.module').then(m => m.WorkerTaskModule),
    data: { guards: ['CoreAccessGuard'], authorizations: [Authorization.canAccessWorkerTask_View] },
    canActivate: [MasterGuard]
  },
  {
    path: 'reports',
    component: ReportsComponent,
    data: { guards: ['CoreAccessGuard'], authorizations: [Authorization.canAccessReports] },
    canActivate: [MasterGuard]
  },
  {
    path: 'clearance',
    data: { guards: ['CoreAccessGuard'], authorizations: [Authorization.canAccessClearance] },
    component: ClearanceComponent,
    canActivate: [MasterGuard]
  },
  {
    path: 'pin/:pin/job-readiness',
    loadChildren: () => import('./features-modules/job-readiness/job-readiness.module').then(m => m.JobReadinessModule),
    data: { guards: ['ParticipantGuard', 'CoreAccessGuard'], authorizations: [Authorization.canAccessJobReadiness_View] },
    canActivate: [MasterGuard],
    canDeactivate: [InformalAssessmentGuard]
  },
  {
    path: 'pin/:pin/participation-calendar',
    loadChildren: () => import('./features-modules/participation-calendar/participation-calendar.module').then(m => m.ParticipationCalendarModule),
    data: {
      guards: ['ParticipantGuard', 'ParticipationCalendarGuard']
    },
    canActivate: [MasterGuard]
  },
  {
    path: 'pin/:pin/children-first-tracking',
    loadChildren: () => import('./features-modules/children-first-tracking/children-first-tracking.module').then(m => m.ChildrenFirstTrackingModule),
    data: {
      guards: ['ParticipantGuard', 'CoreAccessGuard'],
      authorizations: [Authorization.canAccessContactsApp_View, Authorization.canAccessChildrenFirstTracking_Edit]
    },
    canActivate: [MasterGuard]
  },
  {
    path: 'pin/:pin/non-participation-details',
    loadChildren: () => import('./features-modules/non-participation-details/non-participation-details.module').then(m => m.NonParticipationDetailsModule),
    data: { guards: ['ParticipantGuard', 'NonParticipationDetailsGuard'], authorizations: [Authorization.canAccessNonParticipationDetails_View] },
    canActivate: [MasterGuard]
  },
  {
    path: 'pin/:pin/drug-screening',
    loadChildren: () => import('./features-modules/drug-screening/drug-screening.module').then(m => m.DrugScreeningModule),
    data: { guards: ['ParticipantGuard', 'CoreAccessGuard'], authorizations: [Authorization.canAccessDrugScreening_View] },
    canActivate: [MasterGuard],
    canDeactivate: [InformalAssessmentGuard]
  },
  {
    path: 'pin/:pin/contacts',
    component: ContactsListPageComponent,
    data: { guards: ['ParticipantGuard', 'CoreAccessGuard'], authorizations: [Authorization.canAccessContactsApp_View] },
    canActivate: [MasterGuard]
  },
  {
    path: 'pin/:pin/contacts/:id',
    component: ContactDetailComponent,
    canActivate: [MasterGuard],
    data: { guards: ['ParticipantGuard', 'CoreAccessGuard'], authorizations: [Authorization.canAccessContactsApp_Edit] }
  },
  {
    path: 'pin/:pin/employability-plan',
    loadChildren: () => import('./features-modules/employability-plan/employability-plan.module').then(m => m.EmployabilityPlanModule),
    data: {
      guards: ['ParticipantGuard', 'CoreAccessGuard'],
      authorizations: [Authorization.canAccessEmployabilityPlanApp_View, Authorization.canAccessEmployabilityPlanApp_Edit]
    },
    canActivate: [MasterGuard],
    canDeactivate: [InformalAssessmentGuard]
  },
  {
    path: 'pin/:pin/rfa',
    loadChildren: () => import('./features-modules/rfa/rfa.module').then(m => m.RfaModule),
    data: { guards: ['ParticipantGuard', 'CoreAccessGuard'], authorizations: [Authorization.canAccessRfa_View] },
    canActivate: [MasterGuard]
  },
  {
    path: 'pin/:pin/client-registration',
    data: { guards: ['CoreAccessGuard', 'ClientRegistrationGuard'], authorizations: [Authorization.canAccessClientReg_View] },
    component: RegistrationComponent,
    canActivate: [MasterGuard]
  },
  {
    path: 'pin/:pin/assessment/edit',
    // sec_page Assessment Edit
    // sec_usage Retricts access to any of the edit assessement functionality
    data: { guards: ['ParticipantGuard', 'CoreAccessGuard'], authorizations: [Authorization.canAccessInformalAssessment_Edit] },
    component: EditInformalAssessmentComponent,
    canActivate: [MasterGuard],
    canDeactivate: [InformalAssessmentGuard],
    children: [
      {
        path: 'languages',
        component: LanguagesEditComponent
      },
      {
        path: 'education',
        component: EducationHistoryEditComponent
      },
      {
        path: 'post-secondary',
        component: PostSecondaryEducationEditComponent
      },
      {
        path: 'military',
        component: MilitaryTrainingEditComponent
      },
      {
        path: 'work-programs',
        component: WorkProgramsEditComponent
      },
      {
        path: 'child-youth-supports',
        component: ChildYouthSupportsEditComponent
      },
      {
        path: 'housing',
        component: HousingEditComponent
      },
      {
        path: 'legal-issues',
        component: LegalIssuesEditComponent
      },
      {
        path: 'work-history',
        component: WorkHistoryGatepostEditComponent
      },
      {
        path: 'family-barriers',
        component: FamilyBarriersEditComponent
      },
      {
        path: 'participant-barriers',
        component: ParticipantBarriersEditComponent
      },
      {
        path: 'non-custodial-parents',
        component: NcpEditComponent
      },
      {
        path: 'non-custodial-parents-referral',
        component: NcprEditComponent
      },
      {
        path: 'transportation',
        component: TransportationEditComponent
      },
      {
        path: '',
        component: LanguagesEditComponent
      }
    ]
  },
  {
    path: 'pin/:pin/assessment',
    component: InformalAssessmentSummaryComponent,
    canActivate: [MasterGuard],
    data: { guards: ['ParticipantGuard', 'CoreAccessGuard'], authorizations: [Authorization.canAccessInformalAssessment_View] }
  },
  {
    path: 'pin/:pin/test-scores',
    component: TestScoresListPageComponent,
    canActivate: [MasterGuard],
    data: { guards: ['ParticipantGuard', 'CoreAccessGuard'], authorizations: [Authorization.canAccessTestScoresApp_View] }
  },
  {
    path: 'pin/:pin/action-needed',
    component: ActionsNeededComponent,
    canActivate: [MasterGuard],
    data: { guards: ['ParticipantGuard', 'CoreAccessGuard'], authorizations: [Authorization.canAccessActionNeededApp_View] }
  },
  {
    path: 'pin/:pin/participant-barriers/:id',
    component: ParticipantBarriersSingleComponent,
    canActivate: [MasterGuard],
    data: { guards: ['ParticipantGuard'] }
  },
  {
    path: 'pin/:pin/participant-barriers',
    component: ParticipantBarriersListPageComponent,
    canActivate: [MasterGuard],
    data: { guards: ['ParticipantGuard', 'ParticipantBarriersAppGuard'] }
  },
  {
    path: 'pin/:pin/time-limits',
    component: TimeLimitsSummaryComponent,
    canActivate: [MasterGuard, TimeLimitsAppGuard],
    data: { guards: ['ParticipantGuard'] }
  },
  {
    path: 'pin/:pin/time-limits/extensions',
    component: ExtensionListPageComponent,
    canActivate: [MasterGuard, TimeLimitsAppGuard],
    data: { guards: ['ParticipantGuard'] }
  },
  {
    path: 'pin/:pin/time-limits/extensions/:id',
    component: ExtensionDetailComponent,
    canActivate: [MasterGuard, TimeLimitsAppGuard],
    data: { guards: ['ParticipantGuard'] }
  },
  {
    path: 'pin/:pin/payment-details',
    loadChildren: () => import('./features-modules/payment-details/payment-details.module').then(m => m.PaymentDetailsModule),
    canActivate: [MasterGuard],
    data: { guards: ['ParticipantGuard', 'CoreAccessGuard'], authorizations: [Authorization.canAccessPaymentDetails_View] }
  },
  {
    path: 'pop-claims',
    loadChildren: () => import('./features-modules/pop-claims/pop-claims.module').then(m => m.PopClaimsModule)
  },
  {
    path: 'pin/:pin/pop-claims',
    loadChildren: () => import('./features-modules/pop-claims/pop-claims.module').then(m => m.PopClaimsModule),
    canActivate: [MasterGuard],
    data: { guards: ['ParticipantGuard', 'CoreAccessGuard'], authorizations: [Authorization.canAccessPOPClaims_View, Authorization.canAccessPOPClaims_Edit] }
  },
  {
    path: 'pin/:pin/transactions',
    loadChildren: () => import('./features-modules/transactions/transactions.module').then(m => m.TransactionsModule),
    canActivate: [MasterGuard],
    data: { guards: ['ParticipantGuard', 'CoreAccessGuard'], authorizations: [Authorization.canAccessTransactions_View] }
  },
  {
    path: 'pin/:pin/tj-tmj-employment-verification',
    loadChildren: () => import('./features-modules/tj-tmj-employment-verification/tj-tmj-employment-verification.module').then(m => m.TjTmjEmploymentVerificationModule),
    canActivate: [MasterGuard],
    data: { guards: ['ParticipantGuard', 'TJTMJEmploymentVerificationGuard'] }
  },
  {
    path: 'pin/:pin/w-2-plans',
    loadChildren: () => import('./features-modules/w-2-plans/w-2-plans.module').then(m => m.W2PlansModule),
    canActivate: [MasterGuard],
    data: { guards: ['ParticipantGuard', 'CoreAccessGuard'], authorizations: [Authorization.canAccessW2Plans_View] }
  },
  {
    path: 'pin/:pin/emergency-assistance',
    loadChildren: () => import('./features-modules/emergency-assistance/emergency-assistance.module').then(m => m.EmergencyAssistanceModule)
  },
  {
    path: 'w2-auxiliary-approvers',
    loadChildren: () => import('./features-modules/auxiliary/auxiliary.module').then(m => m.AuxiliaryModule),
    canActivate: [MasterGuard],
    data: { guards: ['CoreAccessGuard'], authorizations: [Authorization.canAccessW2AuxApprovers_View] }
  },
  {
    path: 'auxiliary-request',
    loadChildren: () => import('./features-modules/auxiliary/auxiliary.module').then(m => m.AuxiliaryModule),
    canActivate: [MasterGuard],
    data: { guards: ['CoreAccessGuard'], authorizations: [Authorization.canAccessApproverAux_View, Authorization.canAccessApproverAux_Edit] }
  },
  {
    path: 'pin/:pin/auxiliary',
    loadChildren: () => import('./features-modules/auxiliary/auxiliary.module').then(m => m.AuxiliaryModule),
    canActivate: [MasterGuard],
    data: { guards: ['ParticipantGuard', 'CoreAccessGuard'], authorizations: [Authorization.canAccessAuxiliary_View] }
  },
  {
    path: 'pin/:pin/pin-comments',
    component: PinCommentsPageComponent,
    canActivate: [MasterGuard],
    data: { guards: ['CoreAccessGuard'], authorizations: [Authorization.canAccessPinComments_View] }
  },

  {
    path: 'pin/:pin/work-history/:id',
    component: WorkHistorySingleComponent,
    canActivate: [MasterGuard],
    data: { guards: ['ParticipantGuard', 'CoreAccessGuard'], authorizations: [Authorization.canAccessWorkHistoryApp_View] }
  },
  {
    path: 'pin/:pin/work-history',
    component: WorkHistoryListPageComponent,
    canActivate: [MasterGuard],
    data: { guards: ['ParticipantGuard', 'CoreAccessGuard'], authorizations: [Authorization.canAccessWorkHistoryApp_View] }
  },
  {
    path: 'pin/:pin/work-history/:id/hourly-entry',
    component: HourlyEntryListComponent,
    canActivate: [MasterGuard],
    data: { guards: ['ParticipantGuard', 'CoreAccessGuard'], authorizations: [Authorization.canAccessWorkHistoryApp_View] }
  },

  {
    path: 'pin/:pin/career-assessment',
    loadChildren: './features-modules/career-assessment/career-assessment.module#CareerAssessmentModule',
    data: { guards: ['CoreAccessGuard'], authorizations: [Authorization.canAccessCareerAssessment_View] },
    canActivate: [MasterGuard]
  },
  {
    path: 'pin/:pin',
    component: ParticipantSummaryComponent,
    canActivate: [MasterGuard],
    data: { guards: ['ParticipantGuard', 'CoreAccessGuard'], authorizations: [Authorization.canAccessParticipantSummary], isFromPartSumGuard: true }
  },
  {
    path: 'pin/:pin/participation-statuses',
    component: ParticipationStatusesListPageComponent,
    canActivate: [MasterGuard],
    data: { guards: ['CoreAccessGuard'], authorizations: [Authorization.canAccessParticipationStatus_View] }
  },
  { path: '**', component: PageNotFoundComponent }
];

export const appRoutingProviders: any[] = [
  AuthGuard,
  ParticipantGuard,
  ClientRegistrationGuard,
  CoreAccessGuard,
  InformalAssessmentGuard,
  ParticipantBarriersAppGuard,
  ParticipantService,
  TimeLimitsAppGuard,
  MasterGuard,
  OrgInfoGuard,
  NonParticipationDetailsGuard
];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);
