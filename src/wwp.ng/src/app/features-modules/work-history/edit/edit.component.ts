import { JobTypeName } from '../../../shared/enums/job-type-name.enum';
// tslint:disable: no-output-on-prefix
// tslint:disable: no-shadowed-variable
import { ActivatedRoute, Router } from '@angular/router';
import { Component, EventEmitter, Output, Input, OnInit, OnDestroy, DoCheck } from '@angular/core';
import { Subscription, of, Observable, forkJoin } from 'rxjs';

import * as moment from 'moment';
import * as _ from 'lodash';
import { JobActionType } from '../../../shared/models/job-actions';
import { EmployerOfRecordDetails } from '../models/employerofRecordDetails.model';
import { AppService } from './../../../core/services/app.service';
import { BaseParticipantComponent } from '../../../shared/components/base-participant-component';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { GoogleApiService } from '../../../shared/services/google-api.service';
import { GoogleApi } from '../../../shared/models/google-api';
import { GoogleLocation } from '../../../shared/models/google-location';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { Employment, WageHour, WageHourHistory, WorkHistoryIdentities } from '../../../shared/models/work-history-app';
import { TextDetail, CalculatedString } from '../../../shared/models/primitives';
import { WorkHistoryAppService } from '../../../shared/services/work-history-app.service';
import { ParticipantService } from '../../../shared/services/participant.service';
import { Utilities } from '../../../shared/utilities';
import * as util from '../../../shared/utilities';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { EnrolledProgram } from '../../../shared/models/enrolled-program.model';
import { WhyReason } from '../../../shared/models/why-reasons.model';
import { ValidationResult } from '../../../shared/models/validation';
import { LeavingReasonName } from '../enums/leaving-reason-name.enum';

declare var $: any;

@Component({
  selector: 'app-work-history-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css'],
  providers: [GoogleApiService]
})
export class WorkHistoryEditComponent extends BaseParticipantComponent implements OnInit, OnDestroy, DoCheck {
  @Output()
  onSaveAndExit = new EventEmitter();
  @Input()
  recordId = 0;
  @Input() hasOnlyFcdp = false;
  @Input() isHD = false;

  @Input() totalLifeTimeTMJTJSubsidizedHours;

  private boSub: Subscription;
  private jfSub: Subscription;
  private wpSub: Subscription;
  private jtSub: Subscription;
  private lrSub: Subscription;
  private jsSub: Subscription;
  private epSub: Subscription;
  private ptSub: Subscription;
  private itSub: Subscription;
  private ggZipSub: Subscription;
  private erSub: Subscription;
  private programsSub: Subscription;

  public isCollapsed = false;
  public isLoaded = false;
  public isNewRecord = false;
  public hadSaveError = false;
  public isSaving = false;
  private hasTriedSave = false;
  private hasTriedMovingToHistory = false;
  private isSectionModified = false;
  public isSectionValid = true;
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public currentWageValidationManager: ValidationManager = new ValidationManager(this.appService);
  public modelErrors: ModelErrors = {};

  // If these parameters are not set correctly, the console will contain warnings.
  private tempCustodialParentUnsubsidizedId: number;
  private tempNonCustodialParentUnsubsidizedId: number;
  private tmjUnsubsidizedId: number;
  private tjUnsubsidizedId: number;
  private tempCustodialParentSubsidizedId: number;
  private tempNonCustodialParentSubsidizedId: number;
  private tmjSubsidizedId: number;
  private tjSubsidizedId: number;
  private volunteerId: number;
  private unSubsidizedId: number;
  private staffingAgencyId: number;
  private selfEmployedId: number;
  private leavingReasonFiredId: number;
  private leavingReasonPermanentlyLaidOffId: number;
  private leavingReasonQuitId: number;
  private leavingReasonOtherId: number;
  private jobFoundWorkerAssistedId: number;
  private jobFoundOtherWorkProgramId: number;
  private otherPayTypeId: number;
  private noPayPayTypeId: number;

  public jobTypesDrop: DropDownField[];
  public jobFoundMethods: DropDownField[];
  public workPrograms: DropDownField[];
  public jobLeavingReasonsDrop: DropDownField[];

  public jobLeavingReasonsDropBasedOnJobType: DropDownField[];
  public jobSectorsDrop: DropDownField[];

  public benefitsOffered: DropDownField[];

  public date10DaysAfterCurrent: moment.Moment;
  public currentDate: moment.Moment;

  public model: Employment;
  public originalModel: Employment = new Employment();
  public modelIds: WorkHistoryIdentities = new WorkHistoryIdentities();
  public currentJobCategory: string;
  public employerOfRecordDrop: DropDownField[];
  public displayEmployerofRecord = false;
  public employerOfRecordSelectedValue: string;
  public isHourlySubsidyDisabled = true;
  public employerOfRecordTypes: DropDownField[];
  public allPrograms: EnrolledProgram[];
  public intervalTypes: DropDownField[];
  public precheck: WhyReason = new WhyReason();
  public skipModifingSection = false;
  public otherWorkProgramId: number;
  public displayEpMessage = false;
  public showEPFeature = false;
  public isHourlyEntryClicked = false;
  public canSaveWithWarningAfterLeavingReasonCheck = false;
  filteredIntervalTypes: DropDownField[];
  constructor(
    private googleApiService: GoogleApiService,
    public appService: AppService,
    private fdService: FieldDataService,
    private workHistoryAppService: WorkHistoryAppService,
    route: ActivatedRoute,
    router: Router,
    partService: ParticipantService
  ) {
    super(route, router, partService);
  }

  onPinInit() {
    // this.goBackUrl = '/pin/' + this.pin;
  }

  onParticipantInit() {
    // TODO: Add some Rxjs goodness to make sure both observables are complete (base class and here)
    // before setting isLoaded.
    this.isLoaded = true;
  }

  ngOnInit() {
    super.onInit();
    this.showEPFeature = this.appService.getFeatureToggleDate('EPGoLive');
    this.boSub = this.fdService.getJobBenefitsOffered().subscribe(data => this.initJobBenefitsOffered(data));
    this.jfSub = this.fdService.getJobFoundMethods().subscribe(data => this.initJobFoundMethods(data));
    this.wpSub = this.fdService.getWorkPrograms().subscribe(data => this.initWorkPrograms(data));
    this.jtSub = this.fdService.getJobTypes().subscribe(data => this.initJobTypes(data));
    this.lrSub = this.fdService.getJobLeavingReasons().subscribe(data => this.initJobLeavingReasons(data));
    this.ptSub = this.fdService.getWageTypes().subscribe(data => this.initPayTypes(data));
    this.itSub = this.fdService.getIntervalTypes().subscribe(data => this.initIntervalTypes(data));
    this.jsSub = this.fdService.getJobSectors().subscribe(data => this.initJobSectors(data));
    this.erSub = this.fdService.getEmployerOfRecordTypes().subscribe(data => this.initEmployerOfRecordTypes(data));
    this.programsSub = this.partService.getCachedParticipant(this.pin).subscribe(p => {
      this.allPrograms = p.programs;
    });
    if (this.recordId !== 0) {
      this.epSub = this.workHistoryAppService.getEmployment(this.recordId).subscribe(data => this.initEmployment(data));
    } else {
      this.model = new Employment();
      this.model.id = 0;
      this.model.location = new GoogleLocation();
      this.model.jobAction = JobActionType.create();
      this.model.jobDuties = [];
      const td = TextDetail.create();
      this.model.jobDuties.push(td);
      this.model.wageHour = new WageHour();
      this.model.wageHour.currentPayType = JobActionType.create();
      this.model.wageHour.wageHourHistories = [];
      this.model.absences = [];
      this.isNewRecord = true;
      this.model.employerOfRecordDetails = new EmployerOfRecordDetails();
      this.model.employerOfRecordDetails.location = new GoogleLocation();
      this.model.employerOfRecordId = null;
      Employment.clone(this.model, this.originalModel);
    }

    // TODO: Find the Angular way!
    const body = document.getElementsByTagName('body')[0];
    body.classList.add('noscroll');

    this.date10DaysAfterCurrent = Utilities.currentDate.add(10, 'days');
    this.currentDate = Utilities.currentDate;

    // jQuery for sidebar
    $('app-work-history-edit .app-content').on('scroll', function() {
      const t = $(this),
        o = $('app-work-history-edit aside');
      if (1.2 * o.outerHeight() < t.height()) {
        const e = t.scrollTop(),
          s = e - $('app-work-history-edit .nav-header').outerHeight() + 50;
        s > 0 ? o.css('top', s + 'px') : o.css('top', '0');
        // console.log(e);
      } else {
        o.css('top', '0');
      }
    });

    // jQuery for scrolling to sections
    $(document).on('click', '#sidebar li.menu-item', function() {
      const sec = $(this).data('goto'),
        sh = $('#' + sec).position().top;
      $('app-work-history-edit .app-content').stop();
      if (sec) {
        $('app-work-history-edit .app-content').animate(
          {
            scrollTop: sh
          },
          1000
        );
      }
    });
  }

  ngDoCheck() {
    if (this.model && this.model.employerOfRecordId) {
      this.employerOfRecordValue(this.model.employerOfRecordId);
    }
  }
  initIntervalTypes(data) {
    this.intervalTypes = data;
  }

  initJobFoundMethods(data) {
    this.jobFoundOtherWorkProgramId = Utilities.idByFieldDataName('Other Work Program', data);
    this.jobFoundWorkerAssistedId = Utilities.idByFieldDataName('Worker Assisted', data);
    this.modelIds.jobFoundWorkerAssistedId = this.jobFoundWorkerAssistedId;
    this.jobFoundMethods = data;
  }
  initWorkPrograms(data) {
    this.otherWorkProgramId = Utilities.idByFieldDataName('Other', data);
    this.modelIds.otherWorkProgramId = this.otherWorkProgramId;
    this.workPrograms = data;
  }

  sidebarToggle() {
    this.isCollapsed = !this.isCollapsed;
    return this.isCollapsed;
  }

  initJobBenefitsOffered(data) {
    this.benefitsOffered = data;
  }

  initJobTypes(data) {
    this.tempCustodialParentUnsubsidizedId = Utilities.idByFieldDataName('TEMP Custodial Parent (Unsubsidized)', data);
    this.tempCustodialParentSubsidizedId = Utilities.idByFieldDataName('TEMP Custodial Parent (Subsidized)', data);
    this.modelIds.tempCustodialParentSubsidizedId = this.tempCustodialParentSubsidizedId;
    this.tempNonCustodialParentSubsidizedId = Utilities.idByFieldDataName('TEMP Non-Custodial Parent (Subsidized)', data);
    this.modelIds.tempNonCustodialParentSubsidizedId = this.tempNonCustodialParentSubsidizedId;
    this.tmjSubsidizedId = Utilities.idByFieldDataName('TMJ (Subsidized)', data);
    this.modelIds.tmjSubsidizedId = this.tmjSubsidizedId;
    this.tjSubsidizedId = Utilities.idByFieldDataName('TJ (Subsidized)', data);
    this.modelIds.tjSubsidizedId = this.tjSubsidizedId;
    this.modelIds.tempCustodialParentUnsubsidizedId = this.tempCustodialParentUnsubsidizedId;

    this.tempNonCustodialParentUnsubsidizedId = Utilities.idByFieldDataName('TEMP Non-Custodial Parent (Unsubsidized)', data);
    this.modelIds.tempNonCustodialParentUnsubsidizedId = this.tempNonCustodialParentUnsubsidizedId;

    this.tmjUnsubsidizedId = Utilities.idByFieldDataName('TMJ (Unsubsidized)', data);
    this.modelIds.tmjUnsubsidizedId = this.tmjUnsubsidizedId;

    this.tjUnsubsidizedId = Utilities.idByFieldDataName('TJ (Unsubsidized)', data);
    this.modelIds.tjUnsubsidizedId = this.tjUnsubsidizedId;

    this.volunteerId = Utilities.idByFieldDataName('Volunteer', data);
    this.modelIds.volunteerJobTypeId = this.volunteerId;
    this.unSubsidizedId = Utilities.idByFieldDataName('Unsubsidized', data);
    this.modelIds.unSubsidizedId = this.unSubsidizedId;
    this.staffingAgencyId = Utilities.idByFieldDataName('Staffing Agency', data);
    this.modelIds.staffingAgencyId = this.staffingAgencyId;
    this.selfEmployedId = Utilities.idByFieldDataName('Self-Employed', data);
    this.modelIds.selfEmployedId = this.selfEmployedId;
    this.jobTypesDrop = data;
  }

  initJobLeavingReasons(data) {
    this.leavingReasonFiredId = Utilities.idByFieldDataName('Fired', data);
    this.modelIds.leavingReasonFiredId = this.leavingReasonFiredId;

    this.leavingReasonPermanentlyLaidOffId = Utilities.idByFieldDataName('Permanently Laid Off', data);
    this.modelIds.leavingReasonPermanentlyLaidOffId = this.leavingReasonPermanentlyLaidOffId;

    this.leavingReasonQuitId = Utilities.idByFieldDataName('Quit', data);
    this.modelIds.leavingReasonQuitId = this.leavingReasonQuitId;

    this.leavingReasonOtherId = Utilities.idByFieldDataName('Other', data);
    this.modelIds.leavingReasonOtherId = this.leavingReasonOtherId;
    this.jobLeavingReasonsDrop = data;
    this.jobLeavingReasonsDropBasedOnJobType = data;
  }

  filterJobLeavingReasonsDropBasedOnJobType() {
    if (this.jobLeavingReasonsDrop && this.model && this.model.jobTypeId) {
      this.jobLeavingReasonsDropBasedOnJobType = this.jobLeavingReasonsDrop.filter(i => i.optionId === this.model.jobTypeId);
    } else {
      this.jobLeavingReasonsDropBasedOnJobType = this.jobLeavingReasonsDrop;
    }
  }

  initJobSectors(data) {
    this.jobSectorsDrop = data;
  }
  initEmployment(data) {
    this.model = data;
    this.filterJobLeavingReasonsDropBasedOnJobType();
    Employment.clone(this.model, this.originalModel);
    if (this.model.jobTypeId) {
      this.employerofRecord(this.model.jobTypeId);
    }
  }

  initPayTypes(data) {
    this.otherPayTypeId = Utilities.idByFieldDataName('Other', data);
    this.modelIds.otherPayTypeId = this.otherPayTypeId;
    this.noPayPayTypeId = Utilities.idByFieldDataName('No Pay', data);
    this.modelIds.noPayPayTypeId = this.noPayPayTypeId;
  }
  initEmployerOfRecordTypes(data) {
    this.employerOfRecordTypes = data;
    this.modelIds.employerOfRecordTypes = this.employerOfRecordTypes;
  }

  isAddressRequired(): boolean {
    return this.model.isAddressRequired(
      this.tempCustodialParentUnsubsidizedId,
      this.tempNonCustodialParentUnsubsidizedId,
      this.tmjUnsubsidizedId,
      this.tjUnsubsidizedId,
      this.tempCustodialParentSubsidizedId,
      this.tempNonCustodialParentSubsidizedId,
      this.tmjSubsidizedId,
      this.tjSubsidizedId
    );
  }

  isZipCodeRequired(): boolean {
    return this.model.isZipCodeRequired(
      this.tempCustodialParentUnsubsidizedId,
      this.tempNonCustodialParentUnsubsidizedId,
      this.tmjUnsubsidizedId,
      this.tjUnsubsidizedId,
      this.tempCustodialParentSubsidizedId,
      this.tempNonCustodialParentSubsidizedId,
      this.tmjSubsidizedId,
      this.tjSubsidizedId
    );
  }
  isFEINRequired(): boolean {
    return this.model.isFEINRequired(this.tjSubsidizedId, this.tmjSubsidizedId);
  }
  employerofRecord(e): boolean {
    if (this.model.isEmployerOfRecordRequired(this.tempCustodialParentSubsidizedId, this.tempNonCustodialParentSubsidizedId, this.tmjSubsidizedId, this.tjSubsidizedId)) {
      this.displayEmployerofRecord = true;
      this.isHourlySubsidyDisabled = false;
      return true;
    } else {
      this.displayEmployerofRecord = false;
      this.isHourlySubsidyDisabled = true;
      return false;
    }
  }
  difference(object, base) {
    function changes(object, base) {
      return _.transform(object, function(result, value, key) {
        if (!_.isEqual(value, base[key]) && value) {
          result[key] = _.isObject(value) && _.isObject(base[key]) ? changes(value, base[key]) : value;
        }
      });
    }
    return changes(object, base);
  }
  isInValid(errors: ModelErrors, parent: string, prop: string): boolean {
    if (errors == null) {
      return false;
    }
    if (errors[parent] == null) {
      return false;
    }
    return errors[parent][prop];
  }
  employerOfRecordValue(e) {
    const otherId = Utilities.idByFieldDataName('Other', this.employerOfRecordTypes);
    const workSiteId = Utilities.idByFieldDataName('Work Site', this.employerOfRecordTypes);
    const contractorId = Utilities.idByFieldDataName('Contractor', this.employerOfRecordTypes);
    switch (+e) {
      case otherId:
        this.employerOfRecordSelectedValue = 'Other';
        break;
      case workSiteId:
        this.employerOfRecordSelectedValue = 'Work Site';
        break;
      case contractorId:
        this.employerOfRecordSelectedValue = 'Contractor';
        break;
      default:
        this.employerOfRecordSelectedValue = '';
        break;
    }
    if (e === otherId) {
      return true;
    } else {
      return false;
    }
  }

  isPrivatePublicRequired(): boolean {
    return this.model.isPrivatePublicRequired(
      this.tempCustodialParentUnsubsidizedId,
      this.tempNonCustodialParentUnsubsidizedId,
      this.tmjUnsubsidizedId,
      this.tjUnsubsidizedId,
      this.tempCustodialParentSubsidizedId,
      this.tempNonCustodialParentSubsidizedId,
      this.tmjSubsidizedId,
      this.tjSubsidizedId
    );
  }

  // isLocatedInTMIAreaRequired(): boolean {
  //   return this.model.isLocatedInTMIAreaRequired(this.tmjUnsubsidizedId, this.tmjSubsidizedId);
  // }
  displayEPMessage() {
    if (
      (Number(this.model.jobTypeId) === this.unSubsidizedId ||
        Number(this.model.jobTypeId) === this.staffingAgencyId ||
        Number(this.model.jobTypeId) === this.selfEmployedId ||
        Number(this.model.jobTypeId) === this.tempNonCustodialParentSubsidizedId ||
        Number(this.model.jobTypeId) === this.tempCustodialParentSubsidizedId ||
        Number(this.model.jobTypeId) === this.tempNonCustodialParentUnsubsidizedId ||
        Number(this.model.jobTypeId) === this.tempCustodialParentUnsubsidizedId ||
        Number(this.model.jobTypeId) === this.tmjSubsidizedId ||
        Number(this.model.jobTypeId) === this.tmjUnsubsidizedId ||
        Number(this.model.jobTypeId) === this.tjSubsidizedId ||
        Number(this.model.jobTypeId) === this.tjUnsubsidizedId) &&
      this.showEPFeature
    ) {
      this.displayEpMessage = true;
    } else {
      this.displayEpMessage = false;
    }
  }
  isWorkerIdRequired(): boolean {
    return this.model.isWorkerIdRequired(this.jobFoundWorkerAssistedId, this.whichJobCategory());
  }

  isJobFoundDetailsRequired(): boolean {
    return this.model.isJobFoundDetailsRequired(this.otherWorkProgramId, this.jobFoundOtherWorkProgramId);
  }

  canAddAndShowTMJTJSubsidizedHours(employment: Employment) {
    // checkReadOnlyAccess will check for enrolled program and also the most recent enrolled program user has access to.
    if (!!this.participant) {
      return Employment.canAddAndShowTMJTJSubsidizedHours(employment, this.participant, this.appService, true);
    }
  }

  goToHourlyEntry(e: Employment) {
    this.isHourlyEntryClicked = true;
    if (this.isSectionModified) {
      this.appService.isDialogPresent = true;
    } else {
      this.router.navigate([`/pin/${this.pin}/work-history/${e.id}/hourly-entry`], {
        state: {
          employment: e,
          pin: this.pin,
          participant: this.participant,
          totalLifeTimeTMJTJSubsidizedHours: this.totalLifeTimeTMJTJSubsidizedHours,
          isStateStaff: this.appService.isStateStaff
        }
      });
    }
  }

  isPayRateDisabledAndNotRequired(): boolean {
    return this.model.isJobFoundDetailsRequired(this.otherWorkProgramId, this.jobFoundOtherWorkProgramId);
  }

  isLeavingReasonsRequired(): boolean {
    return this.model.isLeavingReasonsRequired(this.currentJobCategory);
  }
  showCurrentlyEmployed() {
    return this.model.isCurrentJobAtCreation !== false && Utilities.isStringEmptyOrNull(this.model.jobEndDate);
  }

  isLeavingReasonsDetailsRequired(): boolean {
    return this.model.isLeavingReasonsDetailsRequired(this.leavingReasonFiredId, this.leavingReasonPermanentlyLaidOffId, this.leavingReasonQuitId, this.leavingReasonOtherId);
  }

  autoPopulateZipcode(placeId: string, prop: string) {
    this.ggZipSub = this.googleApiService.getZipCodeByPlaceId(placeId).subscribe(data => this.initZipCode(data, prop));
  }

  initZipCode(data: GoogleApi, prop: string) {
    if (prop === 'basic') {
      this.model.location.zipAddress = data.address.postalCode;
    }
    if (prop === 'employerOfRecord') {
      this.model.employerOfRecordDetails.location.zipAddress = data.address.postalCode;
    }
    this.validate();
  }

  initDefaultWorkerAssisted() {
    if (this.recordId === 0) {
      switch (Number(this.model.jobTypeId)) {
        case this.modelIds.tempCustodialParentUnsubsidizedId: {
          this.model.jobFoundMethodId = this.modelIds.jobFoundWorkerAssistedId;
          break;
        }
        case this.modelIds.tempNonCustodialParentUnsubsidizedId: {
          this.model.jobFoundMethodId = this.modelIds.jobFoundWorkerAssistedId;
          break;
        }
        case this.modelIds.tmjUnsubsidizedId: {
          this.model.jobFoundMethodId = this.modelIds.jobFoundWorkerAssistedId;
          break;
        }
        case this.modelIds.tjUnsubsidizedId: {
          this.model.jobFoundMethodId = this.modelIds.jobFoundWorkerAssistedId;
          break;
        }
        case this.modelIds.tempCustodialParentSubsidizedId: {
          this.model.jobFoundMethodId = this.modelIds.jobFoundWorkerAssistedId;
          break;
        }
        case this.modelIds.tempNonCustodialParentSubsidizedId: {
          this.model.jobFoundMethodId = this.modelIds.jobFoundWorkerAssistedId;
          break;
        }
        case this.modelIds.tmjSubsidizedId: {
          this.model.jobFoundMethodId = this.modelIds.jobFoundWorkerAssistedId;
          break;
        }
        case this.modelIds.tjSubsidizedId: {
          this.model.jobFoundMethodId = this.modelIds.jobFoundWorkerAssistedId;
          break;
        }
        default: {
          // this.model.jobFoundMethodId = null;
          break;
        }
      }
    }
  }

  isBeginningPayDisabled() {
    return this.model.isBeginningPayDisabled(this.volunteerId);
  }

  isEndingPayDisabled() {
    return this.model.isEndingPayDisabled(this.volunteerId);
  }

  whichJobCategory(): string {
    if (this.isParticipantInfoLoaded === true) {
      this.currentJobCategory = this.model.whichJobCategory();
      return this.currentJobCategory;
    }
  }
  isJobFoundMethodBlockDisplayed() {
    if (this.employerOfRecordSelectedValue !== 'Other' && (this.whichJobCategory() === 'currentJob' || this.model.isCurrentJobAtCreation)) {
      return true;
    } else {
      return false;
    }
  }
  isWorkProgramBlockDisplayed() {
    if (this.model.jobFoundMethodId === this.jobFoundOtherWorkProgramId) {
      return true;
    } else {
      return false;
    }
  }
  processInput() {
    if (this.model.wageHour.currentAverageWeeklyHours) {
      const value = this.model.wageHour.currentAverageWeeklyHours;
      const numbers = value.replace(/[^0-9]/g, '');
      this.model.wageHour.currentAverageWeeklyHours = numbers;
    }
  }

  moveCurrentWageToHistory() {
    this.currentWageValidationManager.resetErrors();
    this.hasTriedMovingToHistory = true;
    if (this.model.wageHour != null) {
      const result = this.model.wageHour.validate(
        this.currentWageValidationManager,
        this.participantDOB,
        this.model.jobBeginDate,
        this.model.jobEndDate,
        this.whichJobCategory(),
        this.model.isCurrentlyEmployed,
        this.model.jobTypeId,
        this.model.allEffectiveDates,
        this.modelIds,
        this.model.isCurrentJobAtCreation
      );
      this.modelErrors = result.errors;

      if (result.isValid) {
        const wh = new WageHourHistory();
        this.getAndSetIntervalNames();
        wh.effectiveDate = this.model.wageHour.currentEffectiveDate;
        wh.historyPayType = new JobActionType();
        wh.historyPayType.jobActionTypes = this.model.wageHour.currentPayType.jobActionTypes;
        wh.payTypeDetails = this.model.wageHour.currentPayTypeDetails;
        wh.payRate = this.model.wageHour.currentPayRate;
        wh.payRateIntervalId = this.model.wageHour.currentPayRateIntervalId;
        wh.hourlySubsidyRate = this.model.wageHour.currentHourlySubsidyRate || '';
        wh.workSiteContribution = this.model.wageHour.workSiteContribution || '';
        wh.averageWeeklyHours = this.model.wageHour.currentAverageWeeklyHours;
        wh.isMovedFromCurrent = true;
        this.model.wageHour.wageHourHistories.push(wh);
        const cs = this.model.wageHour.calculateHourlyWage();
        if (cs && cs.isCalculated) {
          wh.computedWageRateUnit = cs.units;
          wh.computedWageRateValue = cs.value;
        } else {
          wh.payRateIntervalName = Utilities.fieldDataNameById(wh.payRateIntervalId, this.intervalTypes);
          wh.computedWageRateUnit = wh.payRateIntervalName;
          wh.computedWageRateValue = wh.payRate;
        }

        this.model.wageHour.eraseCurrentWage();
        this.currentWageValidationManager.resetErrors();
        this.hasTriedMovingToHistory = false;
      }
    }
  }

  validateCurrent() {
    if (this.hasTriedMovingToHistory === true) {
      this.currentWageValidationManager.resetErrors();
      const result = this.model.wageHour.validate(
        this.currentWageValidationManager,
        this.participantDOB,
        this.model.jobBeginDate,
        this.model.jobEndDate,
        this.whichJobCategory(),
        this.model.isCurrentlyEmployed,
        this.model.jobTypeId,
        this.model.allEffectiveDates,
        this.modelIds,
        this.model.isCurrentJobAtCreation
      );
      this.modelErrors = result.errors;
      if (result.isValid) {
        // If everything is valid, lets reset it.
        this.currentWageValidationManager.resetErrors();
        this.hasTriedMovingToHistory = false;
      }
    }
  }
  checkForLeavingReason() {
    console.log(
      this.model.leavingReasonId,
      this.jobLeavingReasonsDropBasedOnJobType,
      LeavingReasonName.completed1040HoursofWorkInTMJTJ,
      Utilities.fieldDataNameById(this.model.leavingReasonId, this.jobLeavingReasonsDropBasedOnJobType) === LeavingReasonName.completed1040HoursofWorkInTMJTJ
    );
    if (
      this.hasTriedSave &&
      Utilities.fieldDataNameById(this.model.leavingReasonId, this.jobLeavingReasonsDropBasedOnJobType) === LeavingReasonName.completed1040HoursofWorkInTMJTJ
    ) {
      if (!this.canSaveWithWarningAfterLeavingReasonCheck) {
        if (this.precheck.warnings) {
          this.precheck.warnings = [...this.precheck.warnings, 'Total TJ/TMJ Subsidized Hours entered may not equal 1040 hours. Please verify.'];
        } else {
          this.precheck.warnings = ['Total TJ/TMJ Subsidized Hours entered may not equal 1040 hours. Please verify.'];
        }
        this.canSaveWithWarningAfterLeavingReasonCheck = true;
      }
    } else {
      if (this.precheck.warnings && this.precheck.warnings.indexOf('Total TJ/TMJ Subsidized Hours entered may not equal 1040 hours. Please verify.') > -1)
        this.precheck.warnings.splice(this.precheck.warnings.indexOf('Total TJ/TMJ Subsidized Hours entered may not equal 1040 hours. Please verify.'), 1);
      this.canSaveWithWarningAfterLeavingReasonCheck = false;
    }
  }
  cleanseModel() {
    if (this.employerOfRecordSelectedValue === 'Other') {
      this.model.fein = null;
    }
    if (!this.displayEmployerofRecord && (this.model.employerOfRecordId || this.model.employerOfRecordDetails)) {
      this.model.employerOfRecordId = null;
      this.model.employerOfRecordDetails = null;
    }
    if (!this.isJobFoundMethodBlockDisplayed()) {
      this.model.jobFoundMethodId = null;
      this.model.jobFoundMethodName = null;
      this.model.jobFoundMethodDetails = null;
    }
    if (!this.isWorkProgramBlockDisplayed()) {
      this.model.workProgramId = null;
    }

    // For converted records, once the work history is editted the isConverted flag
    // needs to be cleared.
    // US2982 we should ignore the validation to the converted record if the user is just adding the end date without touching anyother fields.
    if (this.model.isConverted) {
      const diff = Object.keys(this.difference(this.model, this.originalModel));
      if (diff.length === 1 && diff.indexOf('jobEndDate') !== -1) {
        this.skipModifingSection = true;
      }
    }
    this.model.isConverted = false;
  }

  validate(e?: string) {
    this.isSectionModified = true;
    if (this.skipModifingSection) {
      this.validationManager.resetErrors();
      // we are passing the new result into the validate method here because for converted record we are calling only validate on End Date and that menthod do not have ValidateMethod
      const resultToPass = new ValidationResult();
      const result = this.model.validateJustEndDate(resultToPass, this.validationManager);
      this.isSectionValid = result.isValid;
      this.modelErrors = result.errors;
      if (this.isSectionValid === true) {
        this.hasTriedSave = false;
      }
    } else {
      if (this.hasTriedSave === true) {
        this.validationManager.resetErrors();
        this.currentWageValidationManager.resetErrors();
        const maxHourlySubsidy = this.appService.getFeatureToggleValue('HourlySubsidy');

        // Call the model's validate method.
        const result = this.model.validate(
          this.validationManager,
          this.participantDOB,
          this.enrolledDate.format('MM/DD/YYYY'),
          this.disenrolledDate.format('MM/DD/YYYY'),
          this.modelIds,
          this.allPrograms,
          this.originalModel,
          this.employerOfRecordSelectedValue,
          this.jobFoundOtherWorkProgramId,
          maxHourlySubsidy
        );

        this.isSectionValid = result.isValid;
        if (!_.isEmpty(this.precheck) && !_.isEmpty(this.precheck.errors)) {
          this.isSectionValid = false;
        }
        this.modelErrors = result.errors;

        if (this.isSectionValid === true && _.isEmpty(this.precheck.warnings)) {
          this.hasTriedSave = false;
        }
      }
    }
  }
  getAndSetIntervalNames() {
    this.model.wageHour.pastBeginPayRateIntervalName = Utilities.fieldDataNameById(this.model.wageHour.pastBeginPayRateIntervalId, this.intervalTypes);
    this.model.wageHour.currentPayRateIntervalName = Utilities.fieldDataNameById(this.model.wageHour.currentPayRateIntervalId, this.intervalTypes);
    this.model.wageHour.pastEndPayRateIntervalName = Utilities.fieldDataNameById(this.model.wageHour.pastEndPayRateIntervalId, this.intervalTypes);
  }
  calculateAndSetComputerHourlyWage() {
    this.getAndSetIntervalNames();
    const cs = this.model.wageHour.calculateHourlyWage();
    const db2Cs = this.model.wageHour.calculateHourlyWageForDB2(this.intervalTypes);
    if (cs) {
      if (this.whichJobCategory() === 'currentJob' || this.originalModel.isCurrentJobAtCreation) {
        this.model.wageHour.computedCurrentWageRateUnit = cs.units;
        this.model.wageHour.computedCurrentWageRateValue = cs.value;
      } else {
        this.model.wageHour.computedPastEndWageRateUnit = cs.units;
        this.model.wageHour.computedPastEndWageRateValue = cs.value;
      }
    }
    if (db2Cs) {
      this.model.wageHour.computedDB2WageRateUnit = db2Cs.units;
      this.model.wageHour.computedDB2WageRateValue = db2Cs.value;
    }
  }

  calculateComputedHourlyWage(): Observable<WhyReason | null> {
    this.getAndSetIntervalNames();
    const cs: CalculatedString = this.model.wageHour.calculateHourlyWage();
    const res = new WhyReason();
    if (cs && cs.units === 'Hour') {
      if (+cs.value > 999.99) {
        res.errors = ['Calculated hourly wage cannot exceed $999.99 per hour.'];
      } else if (+cs.value > 50 && +cs.value < 1000) {
        res.canSaveWithWarnings = true;
        res.warnings = ['You have entered a calculated hourly wage that exceeds $50 per hour.  Please make sure the information is correct before saving.'];
      }
    }
    return of(res);
  }

  preCheckToAddWH(isCalledFromSaveWithWarningButton?: boolean) {
    this.workHistoryAppService.upsertPreCheck(+this.pin, this.model, this.isHD).subscribe(
      res => {
        if (res && this.precheck) {
          this.precheck.canAddWorkHistory = res.canAddWorkHistory;
          res.warnings.forEach(w => {
            this.precheck.warnings.push(w);
          });
          if (this.precheck.errors) {
            this.precheck.errors = [...this.precheck.errors, ...res.errors];
          } else {
            this.precheck.errors = [...res.errors];
          }
        } else {
          this.precheck = res;
          return;
        }
      },
      error => {
        console.log(error);
        this.isSaving = false;
        return;
      },
      // This will be called once the above subscription is complete and validate and save are called only when we dont have any errors from precheck
      () => {
        if (this.precheck.canAddWorkHistory === true && (isCalledFromSaveWithWarningButton || _.isEmpty(this.precheck.warnings))) {
          this.validate();
          this.save();
        } else if (!_.isEmpty(this.precheck.errors)) {
          this.isSaving = false;
          this.isSectionValid = false;
          return;
        } else {
          this.validate();
        }
      }
    );
  }

  save() {
    if (this.isSectionValid === true) {
      this.isSaving = true;
      this.calculateAndSetComputerHourlyWage();
      if (this.whichJobCategory() === 'currentJob' || this.model.isCurrentJobAtCreation) {
        this.model.isCurrentJobAtCreation = true;
      } else {
        this.model.isCurrentJobAtCreation = false;
      }
      if (this.appService && this.appService.wageHistory && this.appService.wageHistory.value) this.model.wageHour.wageHourHistories.push(this.appService.wageHistory.value);
      this.workHistoryAppService.postEmployment(this.model).subscribe(
        resp => {
          this.onSaveAndExit.emit({ isHourlyEntryClicked: this.isHourlyEntryClicked });
          this.isSaving = false;
          this.appService.wageHistory.next(null);
        },
        error => {
          this.isSaving = false;
          this.hadSaveError = true;
          this.appService.wageHistory.next(null);
          throw error;
        }
      );
    }
  }

  checkforHourlyWageWarning() {
    if (this.hasTriedSave || (!_.isEmpty(this.precheck) && !util.isUndefined(this.precheck.canSaveWithWarnings) && this.precheck.canSaveWithWarnings)) {
      this.calculateComputedHourlyWage().subscribe(res => {
        if (res && this.precheck && this.precheck.warnings && this.precheck.warnings.length > 0 && !this.precheck.canSaveWithWarnings) {
          this.precheck.canSaveWithWarnings = res.canSaveWithWarnings ? res.canSaveWithWarnings : this.precheck.canSaveWithWarnings;
          if (res.warnings && res.warnings.length > 0) {
            res.warnings.forEach(w => {
              this.precheck.warnings.push(w);
            });
          }
        } else {
          //TODO: i think we should look at how we are setting the warnings, instead of this being an array it would be better to have an array of objects with a key and a warning message.
          if (
            this.precheck &&
            this.precheck.canSaveWithWarnings &&
            this.precheck.warnings &&
            this.precheck.warnings.indexOf('You have entered a calculated hourly wage that exceeds $50 per hour.  Please make sure the information is correct before saving.') > -1
          ) {
            this.precheck.warnings.splice(
              this.precheck.warnings.indexOf('You have entered a calculated hourly wage that exceeds $50 per hour.  Please make sure the information is correct before saving.'),
              1
            );
            this.precheck.canSaveWithWarnings = false;
          } else {
            this.precheck = res;
          }
        }
      });
    }
  }

  saveAndExitWorkHistory(e?: boolean) {
    // Clear all previous errors.
    this.cleanseModel();
    this.hasTriedSave = true;
    if (this.model.isCurrentlyEmployed) {
      this.model.jobEndDate = null;
      this.model.leavingReasonId = null;
      this.model.leavingReasonDetails = null;
    }
    if (!this.model.wageHour.isCurrentPayTypeDisabled(this.model.jobTypeId, this.modelIds.volunteerJobTypeId)) {
      this.checkforHourlyWageWarning();
    }
    this.checkForLeavingReason();
    if (
      this.isSectionModified === true &&
      (this.model.jobTypeId === this.modelIds.tmjSubsidizedId ||
        this.model.jobTypeId === this.modelIds.tjSubsidizedId ||
        this.model.jobTypeId === this.modelIds.tmjUnsubsidizedId ||
        this.model.jobTypeId === this.modelIds.tjUnsubsidizedId ||
        this.isHD)
    ) {
      this.preCheckToAddWH(e);
    } else if (!_.isEmpty(this.precheck)) {
      if (e) {
        this.validate();
        this.save();
      } else {
        this.validate();
      }
    } else {
      this.validate();
      this.save();
    }
  }

  exitWorkHistory() {
    if (this.isSectionModified === true) {
      this.appService.isDialogPresent = true;
    } else {
      this.onSaveAndExit.emit({ isHourlyEntryClicked: this.isHourlyEntryClicked });
    }
  }

  exitWorkHistoryIgnoreChanges(e: Event) {
    this.onSaveAndExit.emit({ employment: this.model, isHourlyEntryClicked: this.isHourlyEntryClicked });
  }

  ngOnDestroy() {
    if (this.jtSub != null) {
      this.jtSub.unsubscribe();
      this.jtSub = null;
    }
    if (this.lrSub != null) {
      this.lrSub.unsubscribe();
      this.lrSub = null;
    }
    if (this.boSub != null) {
      this.boSub.unsubscribe();
      this.boSub = null;
    }
    if (this.jfSub != null) {
      this.jfSub.unsubscribe();
      this.jfSub = null;
    }
    if (this.epSub != null) {
      this.epSub.unsubscribe();
      this.epSub = null;
    }
    if (this.jsSub != null) {
      this.jsSub.unsubscribe();
      this.jsSub = null;
    }
    if (this.ptSub != null) {
      this.ptSub.unsubscribe();
      this.ptSub = null;
    }
    if (this.ggZipSub != null) {
      this.ggZipSub.unsubscribe();
      this.ggZipSub = null;
    }
    if (this.programsSub != null) {
      this.programsSub.unsubscribe();
      this.programsSub = null;
    }
    super.onDestroy();

    // TODO: Find the Angular way!
    const body = document.getElementsByTagName('body')[0];
    body.classList.remove('noscroll');
  }
}
