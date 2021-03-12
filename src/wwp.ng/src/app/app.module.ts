import { ContactsModule } from './features-modules/contacts/contacts.module';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA, ErrorHandler } from '@angular/core';
import { LoggerModule, NgxLoggerLevel } from 'ngx-logger';
import { DatePipe } from '@angular/common';
import { AppErrorHandler } from './error.handler';
import { environment } from '../environments/environment';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { CoreModule } from './core/core.module';
import { AppComponent } from './app.component';
import { routing, appRoutingProviders } from './app.routing';
import { SimpleNotificationsModule } from 'angular2-notifications';
import { Between13And18ChildrenAgeRepeaterComponent } from './participant/informal-assessment/edit/between13-and18-children-age-repeater/between13-and18-children-age-repeater.component';
import { CaretakingRepeaterComponent } from './participant/informal-assessment/edit/caretaking-repeater/caretaking-repeater.component';
import { ChildYouthSupportsEditComponent } from './participant/informal-assessment/edit/child-youth-supports.component';
import { ChildYouthSupportsOverviewComponent } from './participant/informal-assessment/overview/child-youth-supports.component';
import { CollegeRepeaterComponent } from './participant/informal-assessment/edit/college-repeater/college-repeater.component';
import { ConvictionRepeaterComponent } from './participant/informal-assessment/edit/conviction-repeater/conviction-repeater.component';
import { CourtDatesRepeaterComponent } from './participant/informal-assessment/edit/court-dates-repeater/court-dates-repeater.component';
import { HomePageComponent } from './home-page/home-page.component';
import { DegreeRepeaterComponent } from './participant/informal-assessment/edit/degree-repeater/degree-repeater.component';
import { EditComponent as InformalAssessmentEditComponent } from './participant/informal-assessment/edit/edit.component';
import { EducationHistoryEditComponent } from './participant/informal-assessment/edit/education-history.component';
import { EducationHistoryOverviewComponent } from './participant/informal-assessment/overview/education-history.component';
import { EducationTabComponent } from './participant/informal-assessment/edit/education-tab/education-tab.component';
import { FamilyBarriersEditComponent } from './participant/informal-assessment/edit/family-barriers.component';
import { FamilyBarriersOverviewComponent } from './participant/informal-assessment/overview/family-barriers.component';
import { HeaderComponent } from './header/header.component';
import { HousingEditComponent } from './participant/informal-assessment/edit/housing.component';
import { HousingOverviewComponent } from './participant/informal-assessment/overview/housing.component';
import { HousingRepeaterComponent } from './participant/informal-assessment/edit/housing-repeater/housing-repeater.component';
import { LanguageRepeaterComponent } from './participant/informal-assessment/edit/language-repeater/language-repeater.component';
import { LanguagesEditComponent } from './participant/informal-assessment/edit/languages.component';
import { LanguagesOverviewComponent } from './participant/informal-assessment/overview/languages.component';
import { LegalIssuesEditComponent } from './participant/informal-assessment/edit/legal-issues.component';
import { LegalIssuesOverviewComponent } from './participant/informal-assessment/overview/legal-issues.component';
import { LicenseRepeaterComponent } from './participant/informal-assessment/edit/license-repeater/license-repeater.component';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { MilitaryOverviewComponent } from './participant/informal-assessment/overview/military.component';
import { MilitaryTrainingEditComponent } from './participant/informal-assessment/edit/military-training.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { PostSecondaryEducationEditComponent } from './participant/informal-assessment/edit/post-secondary-education.component';
import { PostSecondaryEducationOverviewComponent } from './participant/informal-assessment/overview/post-secondary-education.component';
import { TransportationOverviewComponent } from './participant/informal-assessment/overview/transportation.component';
import { StartComponent } from './start/start.component';
import { SummaryComponent as InformalAssessmentSummaryComponent } from './participant/informal-assessment/summary/summary.component';
import { SummaryComponent as ParticipantSummaryComponent } from './participant/summary/summary.component';
import { Under12ChildAgeRepeaterComponent } from './participant/informal-assessment/edit/under12-child-age-repeater/under12-child-age-repeater.component';
import { WorkHistoryGatepostEditComponent } from './participant/informal-assessment/edit/work-history.component';
import { WorkHistoryOverviewComponent } from './participant/informal-assessment/overview/work-history.component';
import { WorkProgramRepeaterComponent } from './participant/informal-assessment/edit/work-program-repeater/work-program-repeater.component';
import { WorkProgramsEditComponent } from './participant/informal-assessment/edit/work-programs.component';
import { WorkProgramsOverviewComponent } from './participant/informal-assessment/overview/work-programs.component';
import { ParticipantBarriersEditComponent } from './participant/informal-assessment/edit/participant-barriers.component';
import { SummaryComponent as TimeLimitsSummaryComponent } from './time-limits/summary/summary.component';
import { ClockSummaryComponent } from './time-limits/summary/clock-summary.component';
import { TimelimitsTimelineComponent } from './time-limits/timeline/limits-timeline.component';
import { EditExtensionComponent } from './time-limits/extensions/edit/edit-extension.component';
import { MonthDetailsComponent } from './time-limits/month-details/month-details.component';
import { ClockSummaryDetailsComponent } from './time-limits/clock-summary-details/clock-summary-details.component';
import { MonthBoxComponent } from './time-limits/timeline/month-box/month-box.component';
import { EditMonthComponent } from './time-limits/edit/edit-month.component';
import { ValidationManager } from './shared/models/validation';
import { ExtensionListComponent } from './time-limits/extensions/list/list.component';
import { ExtensionListPageComponent } from './time-limits/extensions/list-page/list-page.component';
import { ExtensionDetailComponent } from './time-limits/extensions/detail/detail.component';
import { ClockTypeNamePipe } from './time-limits/pipes/clock-type-name.pipe';
import { SearchComponent } from './time-limits/search/search.component';
import { NcpEditComponent } from './participant/informal-assessment/edit/ncp.component';
import { NcpCaretakerRepeaterComponent } from './participant/informal-assessment/edit/ncp-caretaker-repeater/ncp-caretaker-repeater.component';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';
import { NcpChildRepeaterComponent } from './participant/informal-assessment/edit/ncp-child-repeater/ncp-child-repeater.component';
import { NcpOverviewComponent } from './participant/informal-assessment/overview/ncp.component';
import { DeleteWarningComponent } from './time-limits/extensions/delete-warning/delete-warning.component';
import { NcprChildRepeaterComponent } from './participant/informal-assessment/edit/ncpr-child-repeater/ncpr-child-repeater.component';
import { NcprOtherParentRepeaterComponent } from './participant/informal-assessment/edit/ncpr-other-parent-repeater/ncpr-other-parent-repeater.component';
import { NcprEditComponent } from './participant/informal-assessment/edit/ncpr.component';
import { NcprOverviewComponent } from './participant/informal-assessment/overview/ncpr.component';
import { PageHelpComponent } from './help/help.component';
import { HelpNavComponent } from './help/help-nav/help-nav.component';
import { TransportationEditComponent } from './participant/informal-assessment/edit/transportation.component';
import { TimeLimitExtensionsHelpComponent } from './help/help-pages/tl-ext-decisions.component';
import { TimeLimitOverviewHelpComponent } from './help/help-pages/tl-overview.component';
import { ParticipantBarriersOverviewComponent } from './participant/informal-assessment/overview/participant-barriers.component';
import { ParticipantCardComponent } from './home-page/participant-card/participant-card.component';
import { EnrollmentComponent } from './enrollment/enrollment/enrollment.component';
import { DisenrollmentComponent } from './enrollment/disenrollment/disenrollment.component';
import { EnrollmentHeaderComponent } from './enrollment/header/header.component';
import { ReassignComponent } from './enrollment/reassign/reassign.component';
import { ClearanceComponent } from './clearance/clearance.component';
import { TransferComponent } from './enrollment/transfer/transfer.component';
import { RegistrationComponent } from './enrollment/registration/registration.component';
import { AliasSsnRepeaterComponent } from './enrollment/registration/alias-ssn-repeater/alias-ssn-repeater.component';
import { CwwChildrenComponent } from './participant/cww/cww-children/cww-children.component';
import { CardHeaderComponent } from './participant/informal-assessment/overview/card-header/card-header.component';
import { PepCardComponent } from './participant/summary/pep-card/pep-card.component';
import { ReportsComponent } from './webi/reports/reports.component';
import { AliasRepeaterComponent } from './clearance/alias-repeater/alias-repeater.component';
//import { OrganizationInformationComponent } from './organization-information/organization-information.component';
// import { LocationRepeaterComponent } from './organization-information/location-repeater/location-repeater.component';
//import { FinalistLocationListComponent } from './organization-information/list/finalist-location-list.component';
//import { FinalistLocationEditComponent } from './organization-information/edit/finalist-location-edit.component';
import { AppService } from './core/services/app.service';
import { SharedModule } from './shared/shared.module';
import { LoginDialogComponent } from './shared/components/login-dialog/login-dialog.component';
import { ActionsNeededModule } from './features-modules/actions-needed/actions-needed.module';
import { ParticipationStatusModule } from './features-modules/participation-statuses/participation-statuses.module';
import { TestScoresModule } from './features-modules/test-scores/test-scores.module';
import { WorkHistoryModule } from './features-modules/work-history/work-history.module';
import { ParticipantBarriersModule } from './features-modules/participant-barriers/participant-barriers.module';
import { AlertWarningComponent } from './src/app/shared/components/alert-warning/alert-warning.component';
import { ListComponent } from './features-modules/agency-level-list/list/list.component';

@NgModule({
  declarations: [
    AppComponent,
    Between13And18ChildrenAgeRepeaterComponent,
    CaretakingRepeaterComponent,
    ChildYouthSupportsEditComponent,
    ChildYouthSupportsOverviewComponent,
    CollegeRepeaterComponent,
    ConvictionRepeaterComponent,
    CourtDatesRepeaterComponent,
    HomePageComponent,
    DegreeRepeaterComponent,
    EducationHistoryEditComponent,
    EducationHistoryOverviewComponent,
    EducationTabComponent,
    FamilyBarriersEditComponent,
    FamilyBarriersOverviewComponent,
    HeaderComponent,
    HousingEditComponent,
    HousingOverviewComponent,
    HousingRepeaterComponent,
    InformalAssessmentEditComponent,
    InformalAssessmentSummaryComponent,
    TimeLimitsSummaryComponent,
    LanguageRepeaterComponent,
    LanguagesEditComponent,
    LanguagesOverviewComponent,
    LegalIssuesEditComponent,
    LegalIssuesOverviewComponent,
    LicenseRepeaterComponent,
    LoginComponent,
    LogoutComponent,
    LoginDialogComponent,
    MilitaryOverviewComponent,
    MilitaryTrainingEditComponent,
    PageNotFoundComponent,
    ParticipantSummaryComponent,
    PostSecondaryEducationEditComponent,
    PostSecondaryEducationOverviewComponent,
    StartComponent,
    Under12ChildAgeRepeaterComponent,
    WorkHistoryGatepostEditComponent,
    WorkHistoryOverviewComponent,
    WorkProgramRepeaterComponent,
    WorkProgramsEditComponent,
    WorkProgramsOverviewComponent,
    ParticipantBarriersEditComponent,
    TimeLimitsSummaryComponent,
    ClockSummaryComponent,
    TimelimitsTimelineComponent,
    MonthDetailsComponent,
    ClockSummaryDetailsComponent,
    EditExtensionComponent,
    EditMonthComponent,
    MonthBoxComponent,
    ExtensionListComponent,
    SearchComponent,
    ExtensionListPageComponent,
    ExtensionDetailComponent,
    ClockTypeNamePipe,
    NcpEditComponent,
    NcpCaretakerRepeaterComponent,
    NcpOverviewComponent,
    UnauthorizedComponent,
    NcpChildRepeaterComponent,
    DeleteWarningComponent,
    NcprChildRepeaterComponent,
    NcprOtherParentRepeaterComponent,
    NcprEditComponent,
    NcprOverviewComponent,
    PageHelpComponent,
    HelpNavComponent,
    TransportationEditComponent,
    TransportationOverviewComponent,
    TimeLimitExtensionsHelpComponent,
    TimeLimitOverviewHelpComponent,
    ParticipantBarriersOverviewComponent,
    ParticipantCardComponent,
    EnrollmentComponent,
    DisenrollmentComponent,
    EnrollmentHeaderComponent,
    ReassignComponent,
    ClearanceComponent,
    TransferComponent,
    RegistrationComponent,
    AliasSsnRepeaterComponent,
    CwwChildrenComponent,
    CardHeaderComponent,
    PepCardComponent,
    ReportsComponent,
    AliasRepeaterComponent,
    //OrganizationInformationComponent,
    //FinalistLocationListComponent,
    //FinalistLocationEditComponent,
    AlertWarningComponent,
    ListComponent
  ],
  imports: [
    BrowserModule,
    SharedModule.forRoot(),
    CoreModule,
    routing,
    BrowserAnimationsModule,
    SimpleNotificationsModule.forRoot(),
    ActionsNeededModule.forRoot(),
    ParticipationStatusModule,
    TestScoresModule.forRoot(),
    ContactsModule.forRoot(),
    WorkHistoryModule.forRoot(),
    ParticipantBarriersModule.forRoot(),
    LoggerModule.forRoot({
      serverLoggingUrl: environment.apiServer + 'api/log',
      level: NgxLoggerLevel.DEBUG,
      serverLogLevel: NgxLoggerLevel.ERROR
    })
  ],
  providers: [appRoutingProviders, [{ provide: ValidationManager, deps: [AppService] }], { provide: ErrorHandler, useClass: AppErrorHandler }, DatePipe],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  entryComponents: [
    LoginDialogComponent,
    DisenrollmentComponent,
    EnrollmentComponent,
    TimeLimitExtensionsHelpComponent,
    TimeLimitOverviewHelpComponent,
    ReassignComponent,
    TransferComponent // so that Angular will create a component Factory for MdDialog to use
  ]
})
export class AppModule {}
