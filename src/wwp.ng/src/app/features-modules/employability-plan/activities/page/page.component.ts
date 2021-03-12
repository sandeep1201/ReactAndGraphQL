import { Event } from './../../../../shared/models/event.model';
import { Component, OnInit, Input } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import * as moment from 'moment';
import { take, concatMap } from 'rxjs/operators';
import { EmployabilityPlan } from '../../models/employability-plan.model';
import { Activity } from '../../models/activity.model';
import { EmployabilityPlanService } from '../../services/employability-plan.service';
import { EmployabilityPlanStatus } from '../../enums/employability-plan-status.enum';
import { AppService } from 'src/app/core/services/app.service';

@Component({
  selector: 'app-activities-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.scss']
})
export class ActivitiesPageComponent implements OnInit {
  @Input() inHistory = false;

  latestactivityDate: any;
  latestactivityObject: any;
  activityStartDate: any;
  showControls: boolean;
  inEditView = false;
  pin: string;
  employabilityPlanId: string;
  public employabilityPlan: EmployabilityPlan;
  public isReadOnly = false;
  activities: Activity[];
  activity: Activity;
  allactivities: Activity[];
  activitiesLoaded = false;
  inConfirmDeleteView = false;
  selectedIdForDeleting: number;
  public epBeginDate: string;
  public events: Event[];
  public iseventsLoaded: boolean;
  public showCalendar = false;
  public showControlsInProgress: boolean;

  public viewDate: any;

  public isEPInProgress: boolean;
  public isBackwardingOfCalendarDisabled = false;
  public isForwardingOfCalendarDisabled = false;

  constructor(private appService: AppService, private employabilityPlanService: EmployabilityPlanService, private router: Router, private route: ActivatedRoute) {}

  ngOnInit() {
    this.pin = this.route.parent.snapshot.paramMap.get('pin');
    this.employabilityPlanService.EditActivitySection.subscribe(res => {
      this.setActivityAccess(res);
    });

    this.employabilityPlanService.viewDate.subscribe(res => {
      this.viewDate = res.viewDate;
    });

    this.route.params
      .pipe(
        take(1),
        concatMap(res => {
          this.employabilityPlanId = res.id;
          return this.employabilityPlanService.getEpById(this.pin, this.employabilityPlanId);
        })
      )
      .subscribe(res => {
        this.employabilityPlan = res;
        this.getActivities();
        this.getEvents();
      });
  }

  getActivities() {
    if (+this.employabilityPlanId > 0) {
      this.employabilityPlanService.getActivities(this.pin, this.employabilityPlanId).subscribe(data => {
        this.activities = data;
        this.filterElapsedActivitiesAfterEpBeginDate(this.activities);
        this.employabilityPlanService.findMinAndMaxDatesForActivity(this.activities);
      });
    } else {
      this.router.navigateByUrl(`/pin/${this.pin}/employability-plan/list`);
    }
  }

  getEvents() {
    const startDate = moment(this.viewDate)
      .startOf('month')
      .format('MM-DD-YYYY');

    this.isBackwardingOfCalendarDisabled = moment(startDate).isSameOrBefore(
      moment(this.employabilityPlan.beginDate)
        .startOf('month')
        .subtract(1, 'M')
        .format('MM-DD-YYYY')
    );
    this.isForwardingOfCalendarDisabled = moment(startDate).isSame(
      moment(this.employabilityPlan.endDate)
        .startOf('month')
        .format('MM-DD-YYYY')
    );

    const isEventsLoaded = this.appService.isEventsLoaded.value;

    if (!isEventsLoaded) {
      this.employabilityPlanService.getEvents(this.pin, +this.employabilityPlanId, this.employabilityPlan.enrolledProgramId, this.employabilityPlan.beginDate).subscribe(res => {
        if (res) {
          this.events = res;
          this.appService.cachedEvents.next(this.events);
          this.appService.isEventsLoaded.next(true);
          this.setColorOnEvents();
          this.iseventsLoaded = true;
        }
      });
    } else {
      this.events = this.appService.cachedEvents.value;
      this.setColorOnEvents();
      this.iseventsLoaded = true;
    }
  }
  setColorOnEvents() {
    this.events.map(e => {
      if (moment(e.start).isBefore(this.employabilityPlan.beginDate)) {
        e.color = 'blue';
      }
    });
  }

  setActivityAccess(res) {
    if (res.activity) {
      this.activity = res.activity;
    }

    this.inEditView = res.inEditView;
    this.isReadOnly = res.readOnly;
    this.showControls = res.showControls;

    if (this.employabilityPlan) {
      if (this.employabilityPlan && !this.employabilityPlan.isDeleted && this.employabilityPlan.submitDate === null) {
        this.isReadOnly = false;
        this.showControls = true;
      }
    }
  }

  nextMonthClicked(e) {
    this.employabilityPlanService.viewDate.next({ viewDate: moment(e).toDate() });
    this.getEvents();
  }

  previousMonthClicked(e) {
    this.employabilityPlanService.viewDate.next({ viewDate: moment(e).toDate() });
    this.getEvents();
  }

  filterElapsedActivitiesAfterEpBeginDate(activities) {
    this.employabilityPlanService.findMinAndMaxDatesForActivity(this.activities);
    this.isEPInProgress = this.employabilityPlan.employabilityPlanStatusTypeName === EmployabilityPlanStatus.inProgress;
    this.activities = activities.filter(activity => {
      // In an In progress EP, in the elapsed activity section driver flow, we check if the activity is carried over and if the ep is in progress
      if (activity.isCarriedOver) {
        return moment(activity.maxEndDate, 'MM/DD/YYYY').isSameOrAfter(moment(this.employabilityPlan.beginDate).format('MM/DD/YYYY'));
        // If no activity is carried over
      } else return true;
    });
    this.activitiesLoaded = true;
  }

  add() {
    if (this.activity) {
      this.activity = null;
    }
    this.employabilityPlanService.EditActivitySection.next({ readOnly: false, inEditView: true, showControls: true });
  }

  deleteActivity(e) {
    this.selectedIdForDeleting = e.id;
    this.inConfirmDeleteView = true;
  }

  singleEntry(a) {
    if (this.employabilityPlan && !this.employabilityPlan.isDeleted && this.employabilityPlan.submitDate === null) {
      this.employabilityPlanService.EditActivitySection.next({ readOnly: false, inEditView: true, showControls: true });
    } else {
      this.employabilityPlanService.EditActivitySection.next({ readOnly: false, inEditView: true, showControls: false });
    }
    this.activity = a;
  }

  onConfirmDelete() {
    this.employabilityPlanService.deleteActivity(this.pin, this.selectedIdForDeleting, this.employabilityPlanId, false).subscribe(res => {
      this.getActivities();
    });
    this.inConfirmDeleteView = false;
  }

  onCancelDelete() {
    this.inConfirmDeleteView = false;
  }

  exitEditView() {
    this.employabilityPlanService.EditActivitySection.next({ readOnly: false, inEditView: false });
    this.getActivities();
    this.getEvents();
  }
  saveAndContinue() {
    this.router.navigateByUrl(`/pin/${this.pin}/employability-plan/supportive-service/${this.employabilityPlanId}`);
  }
  showCalendarView() {
    this.showCalendar = !this.showCalendar;
  }
  closeCalendar(e: boolean) {
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
}
