// tslint:disable: import-blacklist
// tslint:disable: no-shadowed-variable
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Subscription, forkJoin } from 'rxjs';
import { Router, ActivatedRoute } from '@angular/router';
import * as moment from 'moment';
import { take } from 'rxjs/operators';
import { FieldDataService } from 'src/app/shared/services/field-data.service';
import { EmployabilityPlanService } from '../../services/employability-plan.service';
import { Activity } from '../../models/activity.model';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { AppService } from 'src/app/core/services/app.service';
import { Utilities } from 'src/app/shared/utilities';
import { CompletionReasonCode } from 'src/app/shared/enums/completion-reason.enum';
import { FieldDataTypes } from 'src/app/shared/enums/field-data-types.enum';

@Component({
  selector: 'app-elapsed-activities',
  templateUrl: './elapsed-activities.component.html',
  styleUrls: ['./elapsed-activities.component.scss'],
  providers: [FieldDataService, EmployabilityPlanService]
})
export class ElapsedActivitiesComponent implements OnInit {
  originalActivities: Activity[];
  activitiesPayload: Activity[];
  completionReasons: DropDownField[];
  tempCompletionReasons: DropDownField[];

  @Input() activities: Activity[];
  @Input() pin;
  @Input() employabilityPlan;
  @Output() exitToEp = new EventEmitter();
  public tempActivities;

  @Output() showunSavedChanges = new EventEmitter();

  public completionDropDown: Subscription;
  public employabilityPlanId: any;
  public elapsedActivtiesToDisplay = false;
  public isLoaded = false;
  public participantPin: any;
  public fromEpOverView = false;
  public isSaving = false;
  constructor(
    private fdService: FieldDataService,
    private employabilityPlanService: EmployabilityPlanService,
    private route: ActivatedRoute,
    private router: Router,
    public appService: AppService
  ) {}

  ngOnInit() {
    //cloning activities and using tempActivties to manipulate the activties for save or cache inner value.
    // For cacheinnervalue the activties displayed on elapsed activties page are different so using tempActivties instead of original activties.
    if (this.activities) {
      this.elapsedActivtiesToDisplay = true;
      this.fromEpOverView = true;
      this.tempActivities = [...this.activities];
      this.getMinStartDate(this.tempActivities);
      this.filterElapsedActivties(this.tempActivities, this.fromEpOverView);
      this.clearSelectionsOnNavigation(this.tempActivities);
      this.getCompletionDropDownValues(this.employabilityPlan.enrolledProgramCd, this.tempActivities);
    } else {
      // If Elapsed activities section is accessed from the driver flow directly on a subsequent EP, we'll go through the below logic.
      // Get EP Id from url
      this.employabilityPlanId = this.route.snapshot.paramMap.get('id');
      this.route.parent.params.pipe(take(1)).subscribe(results => {
        //Get Pin from URL
        this.participantPin = results.pin;
        // Pass PIN & EP Id obtained above to get Activties and Currently enrolled EP
        return forkJoin([
          this.employabilityPlanService.getActivities(this.participantPin, this.employabilityPlanId).pipe(take(1)),
          this.employabilityPlanService.getEmployabilityPlans(this.participantPin).pipe(take(1))
        ])
          .pipe(take(1))
          .subscribe(results => {
            this.isLoaded = true;
            this.elapsedActivtiesToDisplay = true;
            this.tempActivities = results[0];
            //Filter out non-elapsed activties
            this.employabilityPlanService.findMinAndMaxDatesForActivity(this.tempActivities);
            this.filterElapsedActivties(this.tempActivities, this.fromEpOverView, results[1]);
            this.getMinStartDate(this.tempActivities);
            // clear the completion reason drop down selection if user navigates away from page and comes to the page again
            this.clearSelectionsOnNavigation(this.tempActivities);
            const empData = results[1];
            // Get completion reason drop down values with enrolled program code obtained above
            this.getCompletionDropDownValues(empData[0].enrolledProgramCd, this.tempActivities);
          });
      });
    }
  }

  filterElapsedActivties(activities: Activity[], fromEpOverView: boolean, employabilityPlan?) {
    this.tempActivities = activities.filter(activity => {
      // In an In progress EP, in the elapsed activity section driver flow, we check if the activity is carried over and if the ep is in progress
      if (!fromEpOverView && activity.isCarriedOver && employabilityPlan[0].employabilityPlanStatusTypeName === 'In Progress') {
        return moment(activity.maxEndDate, 'MM/DD/YYYY').isBefore(moment(employabilityPlan[0].beginDate));
      } else if (fromEpOverView) {
        return moment(activity.maxEndDate, 'MM/DD/YYYY').isSameOrBefore(moment(Utilities.currentDate));
      }
    });
  }

  getMinStartDate(activities: Activity[]) {
    let scheduleStartDates = [];
    activities.map(activity => {
      scheduleStartDates = [];
      // Push all schedule start dates to an aray
      activity.activitySchedules.forEach(schedule => {
        scheduleStartDates.push(new Date(schedule.scheduleStartDate));
      });
      // Get the min start date from the array
      activity.minStartDate = moment(new Date(Math.min.apply(null, scheduleStartDates)).toString()).format('MM/DD/YYYY');
    });
  }

  clearSelectionsOnNavigation(activities: Activity[]) {
    // clearing selected completion reasons after user navigates away from the page without saving.
    activities.forEach(activity => {
      if (activity.tempCompletionReasonId != null) {
        activity.tempCompletionReasonId = null;
      }
    });
    // Assign modified activties back to tempActivties
    this.tempActivities = activities;
  }

  getCompletionDropDownValues(enrolledProgramCd, tempActivities) {
    this.completionDropDown = this.fdService.getFieldDataByField(FieldDataTypes.ActivityCompletionReasons, enrolledProgramCd).subscribe(results => {
      this.tempCompletionReasons = results;
      tempActivities.completionReasons = new DropDownField();
      tempActivities.forEach(activity => {
        const actCompletionReasonCode = activity.activityCompletionReasonCode;
        const actCompletionReasonId = activity.activityCompletionReasonId;
        const actCompletionReasonName = activity.activityCompletionReasonName;
        if (activity.activityCompletionReasonName != null) {
          activity.activityCompletionReasonId = Utilities.idByFieldDataName(actCompletionReasonCode + ' - ' + actCompletionReasonName, results);
        }
      });
      tempActivities = tempActivities.filter(activity => !activity.activityCompletionReasonId);
      if (tempActivities.length === 0) {
        this.tempActivities = tempActivities;
        this.elapsedActivtiesToDisplay = false;
        this.isLoaded = true;
      } else {
        // If activtity is one of the options below push the full result else filter VTC code from the result
        tempActivities.forEach(item => {
          if (
            item.activityTypeName === CompletionReasonCode.GED ||
            item.activityTypeName === CompletionReasonCode.HSE ||
            item.activityTypeName === CompletionReasonCode.JST ||
            item.activityTypeName === CompletionReasonCode.RS ||
            item.activityTypeName === CompletionReasonCode.TCA
          ) {
            item.completionReasons = results;
          } else {
            item.completionReasons = results.filter(reason => reason.name !== CompletionReasonCode.VTC);
          }
        });
        this.tempActivities = tempActivities;
        // clone activties to keep a copy for displaying modified values.
        this.originalActivities = [...this.tempActivities];
        this.isLoaded = true;
      }
    });

    this.showunSavedChanges.emit(false);
  }
  saveElapsedActivities(exit: boolean) {
    // cloning activities
    //const originalActivities = [...this.activities];
    this.showunSavedChanges.emit(false);
    this.activitiesPayload = this.tempActivities.filter(activity => {
      if (activity.tempCompletionReasonId) {
        activity.activityCompletionReasonId = activity.tempCompletionReasonId;
        // For each activity where completion reason Id is selected, assigning actual end date based on the recurring status
        activity.activitySchedules.forEach(schedule => {
          schedule.actualEndDate = schedule.isRecurring ? schedule.scheduleEndDate : schedule.scheduleStartDate;
        });
        // Get latest activity based on date
        activity.activitySchedules.sort((first: any, second: any) => {
          const a: Date = new Date(first.scheduleStartDate);
          const b: Date = new Date(second.scheduleStartDate);
          return a.getTime() - b.getTime();
        });
        // Assign the latest activties actualEndDate to the endDate property
        activity.endDate = activity.activitySchedules[activity.activitySchedules.length - 1].actualEndDate;
        activity.activityCompletionReasonName = Utilities.fieldDataNameById(activity.activityCompletionReasonId, this.tempCompletionReasons).split(' - ')[1];
        activity.activityCompletionReasonCode = Utilities.fieldDataNameById(activity.activityCompletionReasonId, this.tempCompletionReasons).split(' - ')[0];
        return activity;
      }
    });

    if (this.activitiesPayload.length > 0) {
      this.employabilityPlanId = this.route.snapshot.paramMap.get('id');
      this.route.parent.params.pipe(take(1)).subscribe(results => {
        //Get Pin from URL
        this.participantPin = results.pin;
      });
      this.isSaving = true;
      this.employabilityPlanService
        .saveElapsedActivties(
          this.pin ? this.pin : this.participantPin,
          this.employabilityPlan ? this.employabilityPlan.id : this.employabilityPlanId,
          this.activitiesPayload,
          this.fromEpOverView
        )
        .subscribe(
          data => {
            this.tempActivities = this.tempActivities.filter(activity => !activity.tempCompletionReasonId);

            if (this.tempActivities.length === 0) {
              this.exitToEp.emit();
            }
            if (exit && this.fromEpOverView) {
              this.exitToEp.emit();
            }
            this.isSaving = false;
            //wait for subscribe to complete
            if (exit && !this.fromEpOverView) {
              this.router.navigateByUrl(`/pin/${this.pin ? this.pin : this.participantPin}/employability-plan/activities/${this.employabilityPlanId}`);
            }
            this.appService.componentDataModifiedFromElasped = false;
            this.appService.isEPUrlChangeBlocked = false;
          },
          error => {
            this.isSaving = false;
          }
        );
      //If there is no elapsed activty to save and the user clicks on save+exit, exit to ep
    } else if (exit && this.fromEpOverView) {
      this.exitToEp.emit();
      // If accessing elapsed activity section from driver flow, go to activties section
    } else if (exit && !this.fromEpOverView) {
      this.router.navigateByUrl(`/pin/${this.pin ? this.pin : this.participantPin}/employability-plan/activities/${this.employabilityPlanId}`);
    }
  }
  dropdownValueChanged(value) {
    if (value !== null || value !== undefined) {
      this.showunSavedChanges.emit(true);
      this.appService.componentDataModifiedFromElasped = true;
      this.appService.isEPUrlChangeBlocked = true;
    } else {
      this.showunSavedChanges.emit(false);
      this.appService.componentDataModifiedFromElasped = false;
      this.appService.isEPUrlChangeBlocked = false;
    }
  }
}
