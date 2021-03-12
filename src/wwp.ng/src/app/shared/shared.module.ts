import { DynamicModule } from 'ng-dynamic-component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { Ng2PaginationModule } from 'ng2-pagination';

import { Ng2DatetimePickerModule } from 'ng2-datetime-picker';

// services
import { SystemClockService } from './services/system-clock.service';
import { NgModule, ModuleWithProviders, NgZone } from '@angular/core';
import { ActionNeededService } from '../features-modules/actions-needed/services/action-needed.service';
import { AgencyService } from './services/agency.service';
import { CdoLogService } from './services/cdo-log.service';
import { ClientRegistrationService } from './services/client-registration.service';
import { ContentService } from './services/content.service';
import { FeatureToggleService } from './services/feature-toggle.service';
import { FieldDataService } from './services/field-data.service';
import { GoogleApiService } from './services/google-api.service';
import { HelpService } from './services/help.service';
import { InformalAssessmentEditService } from './services/informal-assessment-edit.service';
import { InformalAssessmentService } from './services/informal-assessment.service';
import { LogService, LogServiceConfig } from './services/log.service';
import { MasterCustomerIdentifierService } from './services/master-customer-identifier.service';
import { OfficeService } from './services/office.service';
import { OrganizationInformationService } from '../features-modules/organization-info/services/organization-information.service';
import { ParticipantBarrierAppService } from './services/participant-barrier-app.service';
import { ParticipantService } from './services/participant.service';
import { PinCommentsService } from './services/pin-comments.service';
import { ReportService } from './services/report.service';
import { RfaService } from './services/rfa.service';
import { TableauService } from './services/tableau.service';
import { TestScoresService } from './services/test-scores.service';
import { TimeLimitsService } from './services/timelimits.service';
import { UserPreferencesService } from './services/user-preferences.service';
import { WorkHistoryAppService } from './services/work-history-app.service';
import { WorkerService } from './services/worker.service';
import { FinalistAddressService } from './services/finalist-address.service';
import { ContactInfoService } from './services/contactInfo.service';
// Components
import { ActionNeededComponent } from './components/action-needed/action-needed.component';
import { DateChangerComponent } from './components/admin/date-changer/date-changer.component';
import { AppHistoryAlertComponent } from './components/app-history-alert/app-history-alert.component';
import { AppHistoryHeaderComponent } from './components/app-history-header/app-histoy-header.component';
import { AppModifiedStampComponent } from './components/app-modified-stamp/app-modified-stamp.component';
import { AutoCompleteComponent } from './components/auto-complete/auto-complete.component';
import { FinalistAddressComponent } from './components/finalist-address/finalist-address.component';
// import { CalendarComponent } from './components/calendar/calendar.component';
// import { CKButtonDirective, CkEditableDirective, CKEditorComponent, CKGroupDirective } from './components/ck-editor';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';
import { ContentContainerModuleComponent } from './components/content/content-container/container.component';
import { ContentEditBarComponent } from './components/content/content-edit-bar/content-edit-bar.component';
import { TextContentModuleComponent } from './components/content/text-content-module/text-content-module.component';
import { CurrencyInputComponent } from './components/currency-input/currency-input.component';
import { DateInputComponent } from './components/date-input/date-input.component';
import { DateMmDdYyyyComponent } from './components/date-mm-dd-yyyy/date-mm-dd-yyyy.component';
import { CalendarComponent } from './components/calendar/calendar.component';
import { DateMmDdYyyyNewComponent } from './components/date-mm-dd-yyyy-new/date-mm-dd-yyyy-new.component';
import { DateMmYyyyComponent } from './components/date-mm-yyyy/date-mm-yyyy.component';
import { DateMmYyyyInputComponent } from './components/date-mm-yyyy-input/date-mm-yyyy-input.component';
import { DateYyyyComponent } from './components/date-yyyy/date-yyyy.component';
import { DatetimePickerComponent } from './components/datetime-picker/datetime-picker.component';
import { DateAdapter } from './lib/date-adapters/date-adapter';
import { adapterFactory } from 'calendar-utils/date-adapters/date-fns';
import { DeleteReasonButtonComponent } from './components/delete-reason-button/delete-reason-button.component';
import { DialogBoxComponent } from './components/dialog-box/dialog-box.component';
import { DropdownComponent } from './components/dropdown/dropdown.component';
import { ErrorModalComponent } from './components/error-modal/error-modal.component';
import { FilterComponent } from './components/filter/filter.component';
import { GenericTextInputComponent } from './components/generic-text-input/generic-text-input.component';
import { HelpButtonComponent } from './components/help-button/help-button.component';
import { InputHistoryComponent } from './components/input-history/input-history.component';
import { ListingComponent } from './components/listing/listing.component';
import { LoginSimulationDialogComponent } from './components/login-simulation-dialog/login-simulation-dialog.component';
import { ContactInfoDialogComponent } from './components/contact-info-dialog/contact-info-dialog.component';
import { ModalPlaceholderComponent } from './components/modal-placeholder/modal-placeholder.component';
import { ModifiedStampComponent } from './components/modified-stamp/modified-stamp.component';
import { MonthSelectorComponent } from './components/month-selector/month-selector.component';
import { MultiSelectDropdownComponent } from './components/multi-select-dropdown/multi-select-dropdown.component';
import { NumericalInputComponent } from './components/numerical-input/numerical-input.component';
import { PhoneNumberInputComponent } from './components/phone-number-input/phone-number-input.component';
import { RadioButtonComponent } from './components/radio-button/radio-button.component';
import { RemoteAutoCompleteComponent } from './components/remote-auto-complete/remote-auto-complete.component';
import { RemoteAutoSuggestComponent } from './components/remote-auto-suggest/remote-auto-suggest.component';
import { RequestRestrictedAccessDialogComponent } from './components/request-restricted-access-dialog/request-restricted-access-dialog.component';
import { RestrictedAccessDialogComponent } from './components/restricted-access-dialog/restricted-access-dialog.component';
import { SelectComponent } from './components/select/select.component';
import { TabSelectorComponent } from './components/tab-selector/tab-selector.component';
import { TextAreaComponent } from './components/text-area/text-area.component';
import { TextInputComponent } from './components/text-input/text-input.component';
import { TimeoutDialogComponent } from './components/timeout-dialog/timeout-dialog.component';
import { ValidationSummaryComponent } from './components/validation-summary/validation-summary.component';
import { WarningModalComponent } from './components/warning-modal/warning-modal.component';
import { YesNoComponent } from './components/yes-no/yes-no.component';
import { YesNoBasicComponent } from './components/yes-no-basic/yes-no-basic.component';
import { YesNoRefuseComponent } from './components/yes-no-refuse/yes-no-refuse.component';
import { VirtualScrollModule } from './components/virtual-scroll';
//pipes
import { ArraySeparatorByCharPipe } from './pipes/array-separator-by-char.pipe';
import { DateMmDdYyyyPipe } from './pipes/date-mm-dd-yyyy.pipe';
import { DateMmYyyyPipe } from './pipes/date-mm-yyyy.pipe';
import { DefaultPipe } from './pipes/default.pipe';
import { EmptyStringIfNullPipe } from './pipes/empty-string-if-null.pipe';
import { HighDatePipe } from './pipes/high-date.pipe';
import { PhoneNumberPipe } from './pipes/phone-number.pipe';
import { SsnPipe } from './pipes/ssn.pipe';
import { YesNoPipe } from './pipes/yes-no.pipe';
import { PadPipe } from './pipes/pad.pipe';
import { AutoResizeDirective } from './directives/auto-resize.directive';
import { DatetimePickerDirective } from './directives/datetime-picker.directive';
import { HighlightHoverDirective } from './directives/highlight-hover';
import { InputFocusDirective } from './directives/input-focus.directive';
import { MoneyMaskDirective } from './directives/money-mask.directive';
import { OnlyNumbersDirective } from './directives/numbers-only.directive';
import { ScrollDirective } from './directives/scroll.directive';

import { CalendarUtils } from './models/calendar-utils.provider';
import { MomentModule } from 'angular2-moment';
import { NgxMaskModule } from 'ngx-mask';
import { SubHeaderComponent } from './components/sub-header/sub-header.component';
import { SubHeaderNavComponent } from './components/sub-header-nav/sub-header-nav.component';
import { EpEventsComponent } from './components/ep-events/ep-events.component';
import { PtEventsComponent } from './components/pt-events/pt-events.component';
import { CtEventsComponent } from './components/ct-events/ct-events.component';
import { CancelSaveFooterComponent } from './components/cancel-save-footer/cancel-save-footer.component';
import { LoadingComponent } from './components/app-loading/app-loading.component';
import { PinCommentsListComponent } from '../pin-comments/list/list.component';
import { PinCommentsPageComponent } from '../pin-comments/page/page.component';
import { CommentsComponent } from './components/comment/comments.component';
import { CommentsEditComponent } from './components/comment/edit/edit.component';
import { CommentsService } from './components/comment/comments.service';
import { WorkerTaskComponent } from './components/worker-task/worker-task.component';
import { WorkerTaskService } from './components/worker-task/worker-task.service';
import { WorkerTaskEditComponent } from './components/worker-task/edit/edit.component';
import { AppRadioButton2Component } from './components/app-radio-button2/app-radio-button2.component';

@NgModule({
  declarations: [
    ActionNeededComponent,
    DateChangerComponent,
    AppHistoryAlertComponent,
    AppHistoryHeaderComponent,
    AppModifiedStampComponent,
    AutoCompleteComponent,
    CalendarComponent,
    // CKButtonDirective,
    // CkEditableDirective,
    // CKEditorComponent,
    // CKGroupDirective,
    ConfirmDialogComponent,
    ContentContainerModuleComponent,
    ContentEditBarComponent,
    TextContentModuleComponent,
    CurrencyInputComponent,
    DateInputComponent,
    DateMmDdYyyyComponent,
    DateMmDdYyyyNewComponent,
    DateMmYyyyComponent,
    DateMmYyyyInputComponent,
    DateYyyyComponent,
    DatetimePickerComponent,
    DeleteReasonButtonComponent,
    DialogBoxComponent,
    DropdownComponent,
    ErrorModalComponent,
    FilterComponent,
    GenericTextInputComponent,
    HelpButtonComponent,
    InputHistoryComponent,
    ListingComponent,
    LoginSimulationDialogComponent,
    ContactInfoDialogComponent,
    ModalPlaceholderComponent,
    ModifiedStampComponent,
    MonthSelectorComponent,
    MultiSelectDropdownComponent,
    NumericalInputComponent,
    PhoneNumberInputComponent,
    RadioButtonComponent,
    RemoteAutoCompleteComponent,
    RemoteAutoSuggestComponent,
    RequestRestrictedAccessDialogComponent,
    RestrictedAccessDialogComponent,
    SelectComponent,
    SubHeaderComponent,
    FinalistAddressComponent,
    SubHeaderNavComponent,
    TabSelectorComponent,
    TextAreaComponent,
    TextInputComponent,
    TimeoutDialogComponent,
    ValidationSummaryComponent,
    WarningModalComponent,
    YesNoComponent,
    YesNoBasicComponent,
    YesNoRefuseComponent,
    ArraySeparatorByCharPipe,
    DateMmDdYyyyPipe,
    DateMmYyyyPipe,
    DefaultPipe,
    EmptyStringIfNullPipe,
    HighDatePipe,
    PadPipe,
    PhoneNumberPipe,
    SsnPipe,
    YesNoPipe,
    AutoResizeDirective,
    DatetimePickerDirective,
    HighlightHoverDirective,
    InputFocusDirective,
    MoneyMaskDirective,
    OnlyNumbersDirective,
    ScrollDirective,
    EpEventsComponent,
    PtEventsComponent,
    CtEventsComponent,
    CancelSaveFooterComponent,
    LoadingComponent,
    CommentsComponent,
    PinCommentsListComponent,
    PinCommentsPageComponent,
    CommentsEditComponent,
    WorkerTaskComponent,
    WorkerTaskEditComponent,
    AppRadioButton2Component
  ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule,
    MomentModule,
    Ng2PaginationModule,
    Ng2DatetimePickerModule,
    NgxMaskModule.forRoot(),
    VirtualScrollModule,
    DynamicModule.withComponents([ContentContainerModuleComponent, TextContentModuleComponent])
  ],
  entryComponents: [
    // ClockSummaryDetailsComponent,
    DateChangerComponent,
    InputHistoryComponent,
    // ErrorModalComponent,
    LoginSimulationDialogComponent,
    ContactInfoDialogComponent,
    TimeoutDialogComponent,
    RestrictedAccessDialogComponent,
    RequestRestrictedAccessDialogComponent,
    WarningModalComponent,
    DatetimePickerComponent
  ],
  providers: [PadPipe],
  exports: [
    FormsModule,
    RouterModule,
    MomentModule,
    Ng2PaginationModule,
    Ng2DatetimePickerModule,
    ActionNeededComponent,
    DateChangerComponent,
    AppHistoryAlertComponent,
    AppHistoryHeaderComponent,
    AppModifiedStampComponent,
    FinalistAddressComponent,
    AutoCompleteComponent,
    CalendarComponent,
    // CKButtonDirective,
    // CkEditableDirective,
    // CKEditorComponent,
    // CKGroupDirective,
    ConfirmDialogComponent,
    ContentContainerModuleComponent,
    ContentEditBarComponent,
    TextContentModuleComponent,
    CurrencyInputComponent,
    DateInputComponent,
    DateMmDdYyyyComponent,
    DateMmDdYyyyNewComponent,
    DateMmYyyyComponent,
    DateMmYyyyInputComponent,
    DateYyyyComponent,
    DatetimePickerComponent,
    DeleteReasonButtonComponent,
    DialogBoxComponent,
    DropdownComponent,
    ErrorModalComponent,
    FilterComponent,
    GenericTextInputComponent,
    HelpButtonComponent,
    InputHistoryComponent,
    ListingComponent,
    LoginSimulationDialogComponent,
    ContactInfoDialogComponent,
    ModalPlaceholderComponent,
    ModifiedStampComponent,
    MonthSelectorComponent,
    MultiSelectDropdownComponent,
    NgxMaskModule,
    NumericalInputComponent,
    PhoneNumberInputComponent,
    RadioButtonComponent,
    RemoteAutoCompleteComponent,
    RemoteAutoSuggestComponent,
    RequestRestrictedAccessDialogComponent,
    RestrictedAccessDialogComponent,
    SelectComponent,
    SubHeaderComponent,
    SubHeaderNavComponent,
    TabSelectorComponent,
    TextAreaComponent,
    TextInputComponent,
    TimeoutDialogComponent,
    ValidationSummaryComponent,
    WarningModalComponent,
    YesNoComponent,
    YesNoBasicComponent,
    YesNoRefuseComponent,
    ArraySeparatorByCharPipe,
    DateMmDdYyyyPipe,
    DateMmYyyyPipe,
    DefaultPipe,
    EmptyStringIfNullPipe,
    HighDatePipe,
    PadPipe,
    PhoneNumberPipe,
    SsnPipe,
    YesNoPipe,
    AutoResizeDirective,
    DatetimePickerDirective,
    HighlightHoverDirective,
    InputFocusDirective,
    MoneyMaskDirective,
    OnlyNumbersDirective,
    ScrollDirective,
    Ng2PaginationModule,
    VirtualScrollModule,
    CancelSaveFooterComponent,
    LoadingComponent,
    CommentsComponent,
    PinCommentsListComponent,
    PinCommentsPageComponent,
    CommentsEditComponent,
    WorkerTaskComponent,
    WorkerTaskEditComponent,
    AppRadioButton2Component
  ]
})
export class SharedModule {
  static forRoot(): ModuleWithProviders {
    return {
      ngModule: SharedModule,
      providers: [
        ActionNeededService,
        AgencyService,
        CdoLogService,
        ClientRegistrationService,
        ContentService,
        FeatureToggleService,
        FieldDataService,
        GoogleApiService,
        HelpService,
        InformalAssessmentEditService,
        InformalAssessmentService,
        LogService,
        MasterCustomerIdentifierService,
        ClientRegistrationService,
        OfficeService,
        OrganizationInformationService,
        ParticipantBarrierAppService,
        ParticipantService,
        PinCommentsService,
        CommentsService,
        WorkerTaskService,
        ReportService,
        RfaService,
        SystemClockService,
        TableauService,
        TestScoresService,
        TimeLimitsService,
        UserPreferencesService,
        WorkHistoryAppService,
        WorkerService,
        FinalistAddressService,
        ContactInfoService,
        LogServiceConfig,
        CalendarUtils,
        [
          {
            provide: LogService,
            useClass: LogService,
            deps: [NgZone, LogServiceConfig]
          }
        ],
        {
          provide: DateAdapter,
          useFactory: adapterFactory
        }
      ]
    };
  }
}
