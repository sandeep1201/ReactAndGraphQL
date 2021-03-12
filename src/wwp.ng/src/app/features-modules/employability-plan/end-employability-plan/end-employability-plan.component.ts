// tslint:disable: no-output-on-prefix
// tslint:disable: import-blacklist
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { Observable, forkJoin } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import * as moment from 'moment';
import * as _ from 'lodash';
import { take, concatMap } from 'rxjs/operators';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { EmployabilityPlan } from '../models/employability-plan.model';
import { ValidationManager } from 'src/app/shared/models/validation';
import { Goal } from '../models/goal.model';
import { Activity } from '../models/activity.model';
import { EndEP } from '../models/end-ep.model';
import { ModelErrors } from 'src/app/shared/interfaces/model-errors';
import { FieldDataService } from 'src/app/shared/services/field-data.service';
import { EmployabilityPlanService } from '../services/employability-plan.service';
import { AppService } from 'src/app/core/services/app.service';
import { CompletionReasonCode } from 'src/app/shared/enums/completion-reason.enum';
import { Utilities } from 'src/app/shared/utilities';
import { DropDownMultiField } from 'src/app/shared/models/dropdown-multi-field';
import { FieldDataTypes } from 'src/app/shared/enums/field-data-types.enum';

@Component({
  selector: 'app-end-employability-plan',
  templateUrl: './end-employability-plan.component.html',
  styleUrls: ['./end-employability-plan.component.scss']
})
export class EndEmployabilityPlanComponent implements OnInit {
  public goals: any;
  public isLoaded = false;
  public isSectionModified = false;
  public pin: string;
  public endGoalTypesDrop: any[];
  public activities: any;
  ep: any;
  public completionReasons: DropDownField[];
  public employabilityPlan: EmployabilityPlan;
  @Output() onExitEditMode = new EventEmitter<boolean>();
  public employabilityPlanId: string;
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public hasTriedSave = false;
  public originalGoal: Goal = new Goal();
  public originalActivity: Activity = new Activity();
  public endEP: EndEP = new EndEP();
  public isSectionValid = true;
  private maxDaysCanBackDate: string;
  public modelErrors: ModelErrors[] = [];
  public isSaving = false;
  inConfirmDeleteView = false;
  selectedIdForDeleting: number;
  pullDownDates: DropDownMultiField[] = [];

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private fdService: FieldDataService,
    private employabilityPlanService: EmployabilityPlanService,
    public appService: AppService
  ) {}

  ngOnInit() {
    // Get PIN, Goal End reasons, Activities, Goals
    this.pullDownDates = history.state.pullDownDates;
    this.employabilityPlanId = history.state.employabilityPlanId;
    this.requestDataFromMultipleSources().subscribe(result => {
      this.pin = result[0].pin;
      this.endGoalTypesDrop = result[1];
      this.activities = result[2];
      this.goals = result[3];
      if (this.goals.length > 0) {
        this.goalEndDate(this.goals);
      }
      // Filter Not Ended Activities
      if (this.activities.length > 0) {
        this.activities = this.activities.filter(activity => !activity.activityCompletionReasonId);
        this.findMinAndMaxDatesForActivity(this.activities);
      }
    });

    this.appService.employabilityPlan
      .pipe(
        concatMap(res => {
          this.employabilityPlan = res;
          return forkJoin([
            this.employabilityPlanService.getDateForBackDating(this.employabilityPlan.enrolledProgramCd),
            this.fdService.getFieldDataByField(FieldDataTypes.ActivityCompletionReasons, this.employabilityPlan.enrolledProgramCd)
          ]);
        })
      )
      .subscribe(results => {
        if (results[0] && results[0].length > 0) this.maxDaysCanBackDate = results[0][0].maxDaysCanBackDate;
        this.completionReasons = results[1];
        if (this.completionReasons.length > 0 && this.activities) {
          // If activtity is one of the options below push the full result else filter VTC code from the result
          this.activities.forEach(item => {
            if (
              item.activityTypeName === CompletionReasonCode.GED ||
              item.activityTypeName === CompletionReasonCode.HSE ||
              item.activityTypeName === CompletionReasonCode.JST ||
              item.activityTypeName === CompletionReasonCode.RS ||
              item.activityTypeName === CompletionReasonCode.TCA
            ) {
              item.completionReasons = results[1];
            } else {
              item.completionReasons = results[1].filter(reason => reason.name !== CompletionReasonCode.VTC);
            }
          });
          Goal.clone(this.goals, this.originalGoal);
          Activity.clone(this.activities, this.originalActivity);

          this.isLoaded = true;
        }
      });
  }

  public requestDataFromMultipleSources(): Observable<any[]> {
    const response1 = this.route.params.pipe(take(1));
    const response2 = this.fdService.getFieldDataByField('goal-end-reasons');
    const response3 = this.appService.activitiesFromOverview.pipe(take(1));
    const response4 = this.appService.goalsFromOverview.pipe(take(1));
    return forkJoin([response1, response2, response3, response4]);
  }

  public findMinAndMaxDatesForActivity(activities: Activity[]) {
    ////**************////////////
    if (activities)
      // tslint:disable-next-line: no-shadowed-variable
      activities.map(activities => {
        let setMaxDateFlag: Boolean;
        let scheduleStartDates = [];
        // Push all schedule start dates to an aray
        activities.activitySchedules.forEach(activitySchedule => {
          scheduleStartDates.push(new Date(activitySchedule.scheduleStartDate));
        });
        // Get the max start date from the array
        // activities.minStartDate = moment(new Date(Math.min.apply(null, scheduleStartDates)).toString()).format('MM/DD/YYYY');
        // If the activity has not ended (Activity doesn't have a completion reason)
        let scheduleEndDates = [];
        if (!activities.activityCompletionReasonId) {
          // If every schedule in the activity is recurring
          if (activities.activitySchedules.every(schedule => schedule.isRecurring === true)) {
            // Push all the schedule end date to an array
            activities.activitySchedules.forEach(schedule => {
              scheduleEndDates.push(new Date(schedule.scheduleEndDate));
            });
          }
          // If all the schedules are non-recurring in an activity
          else if (activities.activitySchedules.every(schedule => schedule.isRecurring === false)) {
            // Push all schedule start dates to an array
            activities.activitySchedules.forEach(schedule => {
              scheduleEndDates.push(new Date(schedule.scheduleStartDate));
            });
          }
          // If the activity has a combination fo recurring and non-recurring schedules
          else {
            scheduleEndDates = [];
            scheduleStartDates = [];
            // Set a flag
            setMaxDateFlag = true;
            activities.activitySchedules.forEach(schedule => {
              scheduleStartDates.push(new Date(schedule.scheduleStartDate));
              // Push all schedule end dates to array
              if (schedule.scheduleEndDate !== null) scheduleEndDates.push(new Date(schedule.scheduleEndDate));
            });
          }
          // If flag set go into this logic to calculate max date from all the start dates and end dates of all the schedules in an activity
          if (setMaxDateFlag) {
            activities.maxEndDate = moment(new Date(Math.max.apply(null, scheduleEndDates)).toString()).format('MM/DD/YYYY');
            const maxStartDate = moment(new Date(Math.max.apply(null, scheduleStartDates)).toString()).format('MM/DD/YYYY');
            activities.maxEndDate = moment.max(moment(maxStartDate), moment(activities.maxEndDate)).format('MM/DD/YYYY');
          }
          // Else just calculate the max end date of the schedules
          else {
            activities.maxEndDate = moment(new Date(Math.max.apply(null, scheduleEndDates)).toString()).format('MM/DD/YYYY');
          }
        }
        ////**************////////////
      });
  }

  public goalEndDate(goals: Goal[]) {
    if (goals)
      goals.forEach(goal => {
        goal.endDate = moment(Utilities.currentDate.toString()).format('MM/DD/YYYY');
      });
  }

  isChildModelErrorsItemInvalid(i: number, childRepeaterName: string, property: string): boolean {
    return Utilities.isChildModelErrorsItemInvalid(this.modelErrors[0], childRepeaterName, i, property);
  }

  cancel() {
    if (this.isSectionModified) {
      this.appService.isEPUrlChangeBlocked = false;
      this.appService.isDialogPresent = true;
    } else {
      this.onExitEditMode.emit();
      this.router.navigateByUrl(`/pin/${this.pin}/employability-plan/overview/${this.employabilityPlan.id}`);
    }
  }

  getInitialGoalModelValue(i: number, property: string) {
    return Utilities.getPropertybyIdAndName(this.goals, i, property);
  }

  getInitialActivityModelValue(i: number, property: string) {
    return Utilities.getPropertybyIdAndName(this.activities, i, property);
  }

  exitEndEPEditIgnoreChanges($event) {
    this.router.navigateByUrl(`/pin/${this.pin}/employability-plan/overview/${this.employabilityPlan.id}`);
  }

  validate() {
    this.isSectionModified = true;
    if (this.hasTriedSave === true) {
      //this.CleanseModelForApi();
      this.validationManager.resetErrors();
      const result = this.endEP.validate(
        this.validationManager,
        this.goals,
        this.activities,
        this.employabilityPlan.beginDate,
        this.maxDaysCanBackDate,
        this.pullDownDates,
        this.employabilityPlan.enrolledProgramCd
      );
      this.modelErrors = [];
      this.modelErrors.push(result.errors);
      this.isSectionValid = result.isValid;
      if (this.isSectionValid) {
        this.hasTriedSave = false;
      }
    }
  }

  save() {
    this.isSaving = true;
    this.hasTriedSave = true;
    this.validate();
    if (this.isSectionValid === true) {
      this.saveEndEP();
    } else this.isSaving = false;
  }

  saveEndEP() {
    const goalsArr = [];
    if (this.goals && this.goals.length > 0) {
      this.goals.forEach(assignedGoal => {
        goalsArr.push({
          id: assignedGoal.id,
          endDate: assignedGoal.endDate,
          endReasonId: assignedGoal.endReasonId
        });
      });
    }
    this.endEP.goals = goalsArr;

    const activitiesArr = [];
    if (this.activities && this.activities.length > 0) {
      this.activities.forEach(assignedActivity => {
        activitiesArr.push({
          id: assignedActivity.id,
          activityCompletionReasonId: assignedActivity.activityCompletionReasonId,
          endDate: assignedActivity.endDate
        });
      });
    }

    this.endEP.activities = activitiesArr;
    this.employabilityPlanService.saveEndEP(this.pin, this.employabilityPlan.id, this.endEP).subscribe(
      data => {
        this.endEP[0] = data;
        this.isSaving = false;
        this.onExitEditMode.emit();
        this.router.navigateByUrl(`/pin/${this.pin}/employability-plan/list`);
      },
      error => {
        this.isSaving = false;
      }
    );
  }

  isFutureActivity(startDate: string) {
    const momentDate = moment(startDate, 'MM/DD/YYYY');
    const currentDate = moment(moment().format('MM/DD/YYYY'), 'MM/DD/YYYY');
    if (startDate && startDate.trim() !== '' && startDate.length === 10 && momentDate.isValid() && momentDate.isAfter(currentDate)) return true;
    else return false;
  }

  deleteActivity(activity: Activity) {
    this.selectedIdForDeleting = activity.id;
    this.inConfirmDeleteView = true;
  }

  onConfirmDelete() {
    const id = this.selectedIdForDeleting;
    this.employabilityPlanService.deleteActivity(this.pin, id, this.employabilityPlanId, true).subscribe(res => {
      this.activities = _.remove(this.activities, function(activity: Activity) {
        return activity.id !== id;
      });
      this.appService.activitiesFromOverview.next(this.activities);
    });
    this.inConfirmDeleteView = false;
  }

  onCancelDelete() {
    this.inConfirmDeleteView = false;
  }
}
