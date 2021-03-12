import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import * as moment from 'moment';
import { Activity } from '../../models/activity.model';
import { EmployabilityPlanService } from '../../services/employability-plan.service';
import { EmployabilityPlan } from '../../models/employability-plan.model';

@Component({
  selector: 'app-activities-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ActivitiesListComponent implements OnInit {
  @Input() employabilityPlan: EmployabilityPlan;
  @Input() activities: Activity[];
  @Input() isReadOnly = false;
  @Input() showControls = true;
  @Input() latestActivityDate: string;
  @Output() deleteActivity = new EventEmitter();
  @Output() goToSingleEntry = new EventEmitter();
  @Input() epBeginDate: string;
  public deleteActivityId;
  public activitiesLoaded = false;
  public showCalendar = false;
  public events: any;

  constructor(private employabilityPlanService: EmployabilityPlanService, private route: ActivatedRoute) {}

  ngOnInit() {
    this.filterElapsedActivitiesAfterEpBeginDate(this.activities);
  }

  filterElapsedActivitiesAfterEpBeginDate(activities) {
    this.employabilityPlanService.findMinAndMaxDatesForActivity(this.activities);
    this.activities = activities.filter(activity => {
      // In an In progress EP, in the elapsed activity section driver flow, we check if the activity is carried over and if the ep is in progress
      if (activity.isCarriedOver) {
        return moment(activity.maxEndDate, 'MM/DD/YYYY').isSameOrAfter(moment(this.employabilityPlan.beginDate).format('MM/DD/YYYY'));
        // If no activity is carried over
      } else return true;
    });
    this.activitiesLoaded = true;
  }

  delete(a) {
    this.deleteActivity.emit(a);
  }

  singleEntry(a: Activity) {
    this.goToSingleEntry.emit(a);
  }
}
