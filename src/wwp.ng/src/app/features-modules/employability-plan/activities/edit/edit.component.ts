import { FieldDataTypes } from 'src/app/shared/enums/field-data-types.enum';
import { FamilyBarriersSection } from './../../../../shared/models/family-barriers-section';
// tslint:disable: no-output-on-prefix
import { Observable, forkJoin } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import { Component, OnInit, EventEmitter, Output, Input, OnDestroy } from '@angular/core';
import * as _ from 'lodash';
import { Subscription } from 'rxjs';
import { concatMap, take, tap } from 'rxjs/operators';
import { FieldDataService } from 'src/app/shared/services/field-data.service';
import { GoogleApiService } from 'src/app/shared/services/google-api.service';
import { EmployabilityPlanService } from '../../services/employability-plan.service';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { ActivitySchedule } from '../../models/activity-schedule.model';
import { Activity } from '../../models/activity.model';
import { EmployabilityPlan } from '../../models/employability-plan.model';
import { ValidationManager } from 'src/app/shared/models/validation';
import { ModelErrors } from 'src/app/shared/interfaces/model-errors';
import { EnrolledProgram } from 'src/app/shared/models/enrolled-program.model';
import { WhyReason } from 'src/app/shared/models/why-reasons.model';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { AppService } from 'src/app/core/services/app.service';
import { ActivityLocation } from '../../enums/activity-location.enum';
import { timeValues } from 'src/app/shared/dropdown-values/time-values';
import { Utilities } from 'src/app/shared/utilities';
import { GoogleApi } from 'src/app/shared/models/google-api';
import { NonSelfDirectedActivity } from 'src/app/shared/models/nonSelfDirectedActivity.model';
import { GoogleLocation } from 'src/app/shared/models/google-location';
import { ActivityType } from '../../enums/activity-type.enum';
import { CompletionReasonCode } from 'src/app/shared/enums/completion-reason.enum';
import { FrequencyType } from '../../enums/frequency-type.enum';
import { UpfrontActivityType } from '../../enums/up-front-activity.enum';
import { Status } from 'src/app/shared/models/status.model';
import * as moment from 'moment';
import { EmployabilityPlanStatus } from '../../enums/employability-plan-status.enum';

@Component({
  selector: 'app-activities-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss'],
  providers: [FieldDataService, GoogleApiService, EmployabilityPlanService]
})
export class ActivitiesEditComponent implements OnInit, OnDestroy {
  amPmTab: { id: number; name: string }[];
  hours: DropDownField[] = [];
  minutes: DropDownField[] = [];
  originalModel: ActivitySchedule[] = [];
  originalActivityLocationTypes: DropDownField[];
  frequencyRequired: boolean;
  monthsRequired: boolean;
  daysRequired: boolean;
  frequencyNames: string;
  inConfirmDeleteView: boolean;
  selectedIdForDeleting: any;
  id: number;
  activityTypesID: string;
  value: string;
  @Input() events: any;
  @Input() iseventsLoaded: boolean;
  @Output() onExitEditMode = new EventEmitter<boolean>();
  @Output() onExit = new EventEmitter();

  @Output() nextMonthClickedFromEdit = new EventEmitter();
  @Output() previousMonthClickedFromEdit = new EventEmitter();

  @Input() activityInput: Activity;
  @Input() isReadOnly: boolean;
  @Input() activities: Activity[];
  @Input() inHistory = false;
  public activitiesByPep: Activity[];
  public employabilityPlan: EmployabilityPlan;
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  activityTypes: DropDownField[] = [];
  activityLocationType: DropDownField[] = [];
  public activity: Activity;
  public cachedActivity: Activity;
  public isSectionModified = false;
  public hasTriedSave = false;
  public isSectionValid = true;
  public modelErrors: ModelErrors = {};
  public modelIds: any = {};
  public originalActivity: Activity = new Activity();
  public programDrop: DropDownField[] = [];
  public selectedProgramCode: string;
  public savingStatus: Status;
  public canAddActivity: any;
  public enrolledProgramCode: string;
  public pin: string;
  public employabilityPlanId: string;
  public isLoaded = false;
  public isSaving = false;
  public aSaving = false;
  public disableSave = false;
  public showCalendar = false;
  public frequencyTypes: DropDownField[] = [];
  public days: DropDownField[] = [];
  public monthlyRecurrence: DropDownField[] = [];
  public maxDate: Date;
  public activityScheduler: ActivitySchedule[] = [];
  public frequencyIds: number[] = [];
  public employabilityPlanBeginDate: string;
  public showEndActivityFields = false;
  public completionDropDown: Subscription;
  public completionReasons: DropDownField[] = [];
  public participant: any;
  public currentProgram: EnrolledProgram;
  public subsequentEPId: number;
  public initialEp = false;
  public subsequentEp = false;
  public precheck: WhyReason;
  public checkBoxIsReadOnly = false;
  public activityNameForSingleEntry: string;
  public isEPInProgress: boolean;

  @Input() viewDate = moment()
    .startOf('month')
    .toDate();
  @Input() isBackwardingOfCalendarDisabled = false;
  @Input() isForwardingOfCalendarDisabled = false;

  constructor(
    private route: ActivatedRoute,
    private fdService: FieldDataService,
    private router: Router,
    public appService: AppService,
    private googleApiService: GoogleApiService,
    private employabilityPlanService: EmployabilityPlanService,
    private participantService: ParticipantService
  ) {}

  ngOnInit() {
    this.appService.inHistoryMode.subscribe(obj => {
      this.inHistory = obj.inHistory;
    });
    this.requestDataFromMultipleSources()
      .pipe(tap(res => console.log(res)))
      .subscribe(results => {
        this.pin = results[0].pin;
        this.initEmployabilityPlan(results[1]);
        this.activityLocationType = results[2];
        this.frequencyTypes = results[3];
        this.days = results[4];
        this.monthlyRecurrence = results[5];
        this.originalActivityLocationTypes = this.activityLocationType;
        const onSiteId = Utilities.idByFieldDataName(ActivityLocation.onSite, this.activityLocationType, true);
        this.modelIds.onSiteId = onSiteId;
        const offSiteId = Utilities.idByFieldDataName(ActivityLocation.offSite, this.activityLocationType, true);
        this.modelIds.offSiteId = offSiteId;
        const selfDirectedId = Utilities.idByFieldDataName(ActivityLocation.selfDirected, this.activityLocationType, true);
        this.modelIds.selfDirectedId = selfDirectedId;
      });

    this.hours = timeValues.hours;

    this.minutes = timeValues.minutes;
    this.amPmTab = [
      {
        id: 1,
        name: 'AM'
      },
      {
        id: 2,
        name: 'PM'
      }
    ];
  }

  private requestDataFromMultipleSources(): Observable<any[]> {
    return forkJoin([
      this.route.parent.params.pipe(take(1)),
      this.route.params.pipe(take(1)),
      this.fdService.getFieldDataByField(FieldDataTypes.ActivityLocationTypes).pipe(take(1)),
      this.fdService.getFieldDataByField(FieldDataTypes.FrequencyType).pipe(take(1)),
      //TODO: rename this Weekly Frequency
      this.fdService.getFieldDataByField(FieldDataTypes.Frequency).pipe(take(1)),
      this.fdService.getFieldDataByField(FieldDataTypes.Frequency, 'mr').pipe(take(1))
    ]);
  }

  private initEmployabilityPlan(data) {
    this.employabilityPlan = data;
    this.employabilityPlanId = data.id;
    this.isEPInProgress = this.employabilityPlan.employabilityPlanStatusTypeName === EmployabilityPlanStatus.inProgress;
    this.getProgramsAndEP();
  }

  afterdataLoaded() {
    this.employabilityPlanService
      .getActivitiesByPep(this.pin, this.employabilityPlan.pepId)

      .subscribe(res => {
        this.activitiesByPep = res;
        this.modelIds.UEId = Utilities.fieldDataIdByCode(UpfrontActivityType.UE, this.activityTypes);
        this.modelIds.URId = Utilities.fieldDataIdByCode(UpfrontActivityType.UR, this.activityTypes);
        this.modelIds.UCId = Utilities.fieldDataIdByCode(UpfrontActivityType.UC, this.activityTypes);
        this.activity.anyUpfrontActivities(this.activitiesByPep, this.modelIds, this.enrolledProgramCode);
        this.activity.onlyUpfrontActivitiesCheck(this.activitiesByPep, this.modelIds, this.enrolledProgramCode);
        this.isLoaded = true;
      });
  }
  newActivity() {
    const activity = new Activity();
    activity.id = 0;
    activity.contacts = [];
    this.activity = activity;
    const newActivitySchedule = new ActivitySchedule();
    newActivitySchedule.id = activity.id;
    this.activity.activitySchedules = [new ActivitySchedule()];
    this.displaySelfDirected(this.activity.activityTypeId);
  }
  displayBusinessLocationDetails(e) {
    if (e) {
      if (e === this.modelIds.selfDirectedId) {
        return false;
      } else {
        return true;
      }
    }
  }
  getActivitiesByPep() {}
  isInValid(errors: ModelErrors, parent: string, prop: string): boolean {
    if (errors == null) {
      return false;
    }
    if (errors[parent] == null) {
      return false;
    }
    return errors[parent][prop];
  }
  autoPopulateZipcode(placeId: string, prop: string) {
    this.googleApiService.getZipCodeByPlaceId(placeId).subscribe(data => this.initZipCode(data, prop));
  }

  initZipCode(data: GoogleApi, prop: string) {
    if (prop === 'basic') {
      this.activity.nonSelfDirectedActivity.businessZipAddress = data.address.postalCode;
    }
    if (prop === 'nonSelfDirectedActivity') {
      this.activity.nonSelfDirectedActivity.businessZipAddress = data.address.postalCode;
    }
    this.validate();
  }
  private getProgramsAndEP() {
    this.employabilityPlanService
      .getEpById(this.pin, this.employabilityPlanId)

      .subscribe(res => {
        this.initActivityAndTypes(res);
      });
  }
  callPrecheckforUpFrontActivities() {
    const data = this.activity.precheck(this.activitiesByPep, this.modelIds);
    this.precheck = data;
    if (this.precheck && this.precheck.errors) {
      this.isSectionValid = false;
    } else {
      this.isSectionValid = true;
    }
  }

  private initActivityAndTypes(result) {
    this.employabilityPlan = result;
    this.isEPInProgress = this.employabilityPlan.employabilityPlanStatusTypeName === EmployabilityPlanStatus.inProgress;
    this.enrolledProgramCode = this.employabilityPlan.enrolledProgramCd;
    this.employabilityPlanBeginDate = this.employabilityPlan.beginDate;
    if (this.activityInput) {
      this.employabilityPlanService
        .getActivity(this.pin, this.activityInput.id.toString(), this.employabilityPlanId)
        .pipe(
          concatMap(res => {
            this.activity = res;
            if (this.activity) {
              if (this.employabilityPlan && !this.employabilityPlan.isDeleted && this.employabilityPlan.submitDate === null) {
                this.checkBoxIsReadOnly = false;
                if (this.activity.activityCompletionReasonId != null) {
                  this.activityNameForSingleEntry = this.activity.activityCompletionReasonCode + ' - ' + this.activity.activityCompletionReasonName;
                  this.activity.endActivity = true;
                  this.showEndActivityFields = true;
                  this.activity.endActivity = true;
                }
              } else {
                if (this.activity.activityCompletionReasonId != null) {
                  this.checkBoxIsReadOnly = true;
                  this.activityNameForSingleEntry = this.activity.activityCompletionReasonCode + ' - ' + this.activity.activityCompletionReasonName;
                  this.activity.endActivity = true;
                }
              }
            }
            Activity.clone(this.activity, this.originalActivity);
            return this.fdService.getFieldDataByField('activitytypes', this.enrolledProgramCode);
          })
        )
        .subscribe(res => {
          this.activityTypes = res;
          if (this.activityInput) {
            // The function call below is being made here because we need activity types and to make sure that activityTypes is not null on initial load
            this.onSelectActivity(this.activityInput.activityTypeId);
            this.fetchSubsequentEPId();
            this.afterdataLoaded();
          }
        });
    } else {
      this.newActivity();
      this.activity.nonSelfDirectedActivity = new NonSelfDirectedActivity();
      this.activity.nonSelfDirectedActivity.businessLocation = new GoogleLocation();
      Activity.clone(this.activity, this.originalActivity);
      this.fdService.getFieldDataByField(FieldDataTypes.ActivityTypes, this.enrolledProgramCode).subscribe(res => {
        this.activityTypes = res;
        this.fetchSubsequentEPId();
        this.afterdataLoaded();
      });
    }
    if (this.activityInput != null) {
      for (const item of this.activityInput.activitySchedules) {
        const x = new ActivitySchedule();
        ActivitySchedule.clone(item, x);
        this.originalModel.push(x);
      }
    }
  }

  getCompletionDropDownValues(enrolledProgramCd, activityTypeId) {
    this.fdService.getFieldDataByField(FieldDataTypes.ActivityCompletionReasons, enrolledProgramCd).subscribe(results => {
      //Emptying the dropdown in case the user selects a different activity after selecting the initial one
      this.activity.activityCompletionReasonId = null;
      this.activity.activityTypeName = Utilities.fieldDataNameById(activityTypeId, this.activityTypes);
      // If activtity is one of the options below push the full result else filter VTC code from the result
      if (
        this.activity.activityTypeName === ActivityType.GE ||
        this.activity.activityTypeName === ActivityType.RS ||
        this.activity.activityTypeName === ActivityType.HE ||
        this.activity.activityTypeName === ActivityType.JS ||
        this.activity.activityTypeName === ActivityType.TC
      ) {
        this.completionReasons = results;
      } else {
        this.completionReasons = results.filter(reason => reason.name !== CompletionReasonCode.VTC);
      }
      const actCompletionReasonCode = this.activity.activityCompletionReasonCode;
      const actCompletionReasonId = this.activity.activityCompletionReasonId;
      const actCompletionReasonName = this.activity.activityCompletionReasonName;
      if (this.activity.activityCompletionReasonName != null) {
        this.activity.activityCompletionReasonId = Utilities.idByFieldDataName(actCompletionReasonCode + ' - ' + actCompletionReasonName, this.completionReasons, true);
      }
    });
  }

  private CleanseModelForApi() {
    if (this.activity.nonSelfDirectedActivity.businessPhoneNumber === 0) {
      this.activity.nonSelfDirectedActivity.businessPhoneNumber = null;
    }
    if (this.activity.nonSelfDirectedActivity.businessLocation === null) {
      this.activity.nonSelfDirectedActivity.businessStreetAddress = null;
      this.activity.nonSelfDirectedActivity.businessZipAddress = null;
    }
    if (this.activity.description != null && this.activity.description.trim().length === 0) {
      this.activity.description = null;
    }
    if (this.activity.additionalInformation != null && this.activity.additionalInformation.trim().length === 0) {
      this.activity.additionalInformation = null;
    }
    if (this.activity.activityLocationId === Utilities.idByFieldDataName(ActivityLocation.selfDirected, this.activityLocationType, true)) {
      this.activity.nonSelfDirectedActivity.businessName = null;
      this.activity.nonSelfDirectedActivity.businessLocation = null;
      this.activity.nonSelfDirectedActivity.businessStreetAddress = null;
      this.activity.nonSelfDirectedActivity.businessZipAddress = null;
      this.activity.nonSelfDirectedActivity.businessPhoneNumber = null;
    }
  }

  public validate() {
    this.isSectionModified = true;
    if (this.hasTriedSave === true) {
      this.CleanseModelForApi();
      this.validationManager.resetErrors();
      const result = this.activity.validate(
        this.validationManager,
        this.modelIds,
        this.subsequentEp,
        this.initialEp,
        this.employabilityPlanBeginDate,
        this.showEndActivityFields,
        this.hours,
        this.minutes,
        this.activity
      );

      this.isSectionValid = result.isValid;
      this.modelErrors = result.errors;
      if (this.isSectionValid) this.hasTriedSave = false;
      if (this.modelErrors) {
        this.isSaving = false;
      }
    }
  }

  // The following methods are required for business logic of Activity Scheduler
  onSelectActivity(activityTypeId) {
    this.displaySelfDirected(activityTypeId);
    this.getCompletionDropDownValues(this.enrolledProgramCode, activityTypeId);
  }
  displaySelfDirected(activityTypeId) {
    // Logic to display self directed tab when an activity that can be self directed is selected
    let canSelfDirect: boolean;
    for (const x of this.activityTypes) {
      if (x.id === activityTypeId) {
        canSelfDirect = x.canSelfDirect;
        break;
      }
    }

    // Filter self directed from list of options if the activity selected can be self directed
    if (canSelfDirect) {
      // restore original values if the activity selected cant be self directed
      this.activityLocationType = this.originalActivityLocationTypes;
    } else {
      this.activityLocationType = this.originalActivityLocationTypes.filter(type => type.name !== ActivityLocation.selfDirected);
    }
  }
  fetchSubsequentEPId() {
    let lastEp: any;
    this.participantService
      .getCurrentParticipant()
      .pipe(
        concatMap(participant => {
          this.participant = participant;
          this.currentProgram = this.participant.programs.find(x => +x.enrolledProgramId === +this.employabilityPlan.enrolledProgramId);
          return this.appService.submittedEps;
        })
      )
      .subscribe(results => {
        if (results && results !== null && results.submittedEps.length > 0) {
          lastEp = results.submittedEps
            .filter(item => item.enrolledProgramCd.trim() === this.currentProgram.programCd.trim())
            .sort((first: any, second: any) => {
              const a: Date = new Date(first.beginDate);
              const b: Date = new Date(second.beginDate);
              return a.getTime() - b.getTime();
            });

          if (lastEp && lastEp.length > 0) {
            this.subsequentEp = true;
            this.subsequentEPId = lastEp[lastEp.length - 1].id;
          } else {
            this.initialEp = true;
            this.subsequentEPId = 0;
          }
        } else {
          this.initialEp = true;
          this.subsequentEPId = 0;
        }
      });
  }

  showEndActivityReason() {
    if (!this.showEndActivityFields) {
      if (!Utilities.isNumberEmptyOrNull(this.activity.activityTypeId)) {
        this.showEndActivityFields = true;
        this.fetchSubsequentEPId();
      }
    } else {
      if (this.employabilityPlan.submitDate !== null || this.employabilityPlan.isDeleted) {
        this.checkBoxIsReadOnly = true;
      }
      if (
        this.showEndActivityFields &&
        this.activity.activityCompletionReasonId !== null &&
        this.activity.endDate !== null &&
        this.employabilityPlan.submitDate === null &&
        !this.employabilityPlan.isDeleted
      ) {
        this.activity.activityCompletionReasonId = null;
        this.activity.endDate = null;
        this.showEndActivityFields = false;
      }
    }
  }

  preCheck(activityTypeId?: number) {
    this.callPrecheckforUpFrontActivities();
    if (this.currentProgram.isTj || this.currentProgram.isTmj) {
      this.employabilityPlanService.canAddActivity(this.pin, +this.employabilityPlanId, activityTypeId ? activityTypeId.toString() : '0').subscribe(
        res => {
          this.precheck = _.merge(res, this.precheck);
          return;
        },
        error => console.log(error),
        // This will be called once the above subscription is complete and validate and save are called only when we dont have any errors from precheck
        () => {
          if (this.precheck.status !== true) {
            this.isSaving = false;
            this.isSectionValid = false;
            return;
          }
        }
      );
    } else {
      return;
    }
  }
  save() {
    this.aSaving = true;
    this.hasTriedSave = true;
    this.validate();
    if (this.isSectionValid === true) {
      this.saveActivity();
    } else {
      this.aSaving = false;
    }
  }

  saveActivity() {
    // Save for Activity Scheduler by looping through multiple schedules and procuring respective id's
    this.activity.activitySchedules.forEach(schedule => {
      schedule.activityScheduleFrequencies = [];
      const frequencyArr = [];
      if (schedule.isRecurring) {
        // tslint:disable-next-line: radix
        schedule.frequencyTypeName = Utilities.fieldDataNameById(parseInt(schedule.frequencyTypeId), this.frequencyTypes);
        if (schedule.frequencyTypeName === FrequencyType.bw || schedule.frequencyTypeName === FrequencyType.wk) {
          schedule.wkFrequencyIds.forEach(id => {
            frequencyArr.push({
              id: 0,
              wkFrequencyId: id,
              wkFrequencyName: Utilities.fieldDataNameById(id, this.days),
              activityScheduleId: 0
            });
          });
        } else {
          frequencyArr.push({
            id: 0,
            wkFrequencyId: schedule.wkFrequencyId,
            wkFrequencyName: Utilities.fieldDataNameById(schedule.wkFrequencyId, this.days),
            activityScheduleId: 0,
            mrFrequencyId: schedule.mrFrequencyId,
            mrFrequencyName: Utilities.fieldDataNameById(schedule.mrFrequencyId, this.monthlyRecurrence)
          });
        }
        schedule.activityScheduleFrequencies = frequencyArr;
      } else if (!schedule.isRecurring || schedule.isRecurring === null) {
        schedule.isRecurring = false;
        schedule.frequencyTypeId = null;
        schedule.frequencyTypeName = null;
        schedule.scheduleEndDate = null;
        schedule.wkFrequencyIds = [];
      }
    });

    const isEventsLoaded = this.appService.isEventsLoaded.value;

    this.employabilityPlanService.saveActivity(this.pin, this.employabilityPlan.id, this.activity, this.subsequentEPId).subscribe(
      data => {
        this.appService.cachedEvents.next(null);
        this.appService.isEventsLoaded.next(false);
        this.activity = data;
        Activity.clone(this.activity, this.originalActivity);
        this.aSaving = false;
        this.onExitEditMode.emit();
      },
      error => {
        this.aSaving = false;
        this.appService.isEventsLoaded.next(isEventsLoaded);
      }
    );
  }

  cancel() {
    const inHistory = this.inHistory;
    if (this.isSectionModified) {
      this.appService.isEPUrlChangeBlocked = false;
      this.appService.isDialogPresent = true;
    } else {
      if (this.isReadOnly && !inHistory) {
        this.onExitEditMode.emit();
        this.router.navigateByUrl(`/pin/${this.pin}/employability-plan/overview/${this.employabilityPlanId}`);
      } else {
        this.onExitEditMode.emit();
      }
      if (inHistory) {
        this.router.navigateByUrl(`/pin/${this.pin}/employability-plan/historical-activities`);
        this.appService.inHistoryMode.next({ inHistory: false });
      }
    }
  }
  exit(e) {
    if (e === true) {
      this.onExitEditMode.emit();
    }
  }
  exitActivityEditIgnoreChanges($event) {
    this.onExitEditMode.emit();
  }
  showCalendarView() {
    this.showCalendar = !this.showCalendar;
  }
  closeCalendar(e: boolean) {
    this.showCalendar = e;

    if (!this.isEPInProgress) {
      this.employabilityPlanService.viewDate.next({
        viewDate: moment()
          .startOf('month')
          .toDate()
      });
    } else if (this.isEPInProgress) {
      this.employabilityPlanService.viewDate.next({
        viewDate: moment(this.employabilityPlan.beginDate)
          .startOf('month')
          .toDate()
      });
    }
    this.showCalendar = e;
  }
  nextMonthClicked(e) {
    this.nextMonthClickedFromEdit.emit(e);
  }

  previousMonthClicked(e) {
    this.previousMonthClickedFromEdit.emit(e);
  }
  ngOnDestroy() {}
}
