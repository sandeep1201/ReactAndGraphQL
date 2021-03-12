import { EmployabilityPlan } from 'src/app/features-modules/employability-plan/models/employability-plan.model';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import * as moment from 'moment';
import { ActivitySchedule } from '../../models/activity-schedule.model';
import { Activity } from '../../models/activity.model';
import { EmployabilityPlanService } from '../../services/employability-plan.service';
import { AppService } from 'src/app/core/services/app.service';
import { Utilities } from 'src/app/shared/utilities';

@Component({
  selector: 'app-activity-overview',
  templateUrl: './activity-overview.component.html',
  styleUrls: ['./activity-overview.component.scss']
})
export class ActivityOverviewComponent implements OnInit {
  showElapsedActivityBt = false;
  latestactivityObject: ActivitySchedule[][];
  @Input() pin: number;
  @Input() epId: number;
  @Input() activities: Activity[];
  @Input() showEdit = true;
  @Input() employabilityPlan: EmployabilityPlan;
  @Output() edit = new EventEmitter<string>();
  @Output() endElapsedActivities = new EventEmitter<string>();
  @Output() showElapsedActivity = new EventEmitter();
  @Input() canEdit = true;
  @Output() elapsedActivityFlag: boolean;

  public isLoaded = false;

  setMaxDateFlag = false;
  currentEp: any;
  constructor(private router: Router, private employabilityPlanService: EmployabilityPlanService, private appService: AppService) {}

  ngOnInit() {
    this.appService.employabilityPlanInfo.subscribe(data => {
      if (data && data.results) {
        this.showHideElapsedActivityButton(data.results);
      }
    });
    this.employabilityPlanService.findMinAndMaxDatesForActivity(this.activities);
    // If EP is in submitted state
    if (this.employabilityPlan.employabilityPlanStatusTypeName === 'Submitted') {
      this.elapsedActivityFlag = this.activities.some((activity: Activity) => {
        // If atleast one activity is not ended
        if (!activity.activityCompletionReasonId) {
          return moment(activity.maxEndDate, 'MM/DD/YYYY').isSameOrBefore(Utilities.currentDate);
        }
      });
    }
    if (this.elapsedActivityFlag && this.showElapsedActivityBt && this.canEdit) {
      this.elapsedActivityFlag = true;
      this.isLoaded = true;
    } else {
      this.elapsedActivityFlag = false;
      this.isLoaded = true;
    }
  }

  private showHideElapsedActivityButton(data: any) {
    const enrolledPrograms = data[1].getCurrentEnrolledProgramsUserHasAccessTo(this.appService.user, this.appService);
    const submittedEps = data[0].filter(i => i.employabilityPlanStatusTypeName === 'Submitted');
    const submittedEPsUserHasAccessTo = submittedEps.filter(ep => this.appService.isUserAuthorizedForProgramByCode(ep.enrolledProgramCd));
    const inProgressEP = data[0].filter(i => i.employabilityPlanStatusTypeName === 'In Progress');
    const inProgressEPsUserHasAccessTo = inProgressEP.filter(ep => this.appService.isUserAuthorizedForProgramByCode(ep.enrolledProgramCd));
    const enrolledProgramsIds = [];
    const submittedEpProgramIds = [];
    const inprogessEpProgramIds = [];
    this.currentEp = data[0].filter(ep => ep.employabilityPlanStatusTypeName === 'Submitted' && ep.enrolledProgramCd === this.employabilityPlan.enrolledProgramCd);

    enrolledPrograms.forEach(element => {
      enrolledProgramsIds.push(element.enrolledProgramId);
    });
    submittedEPsUserHasAccessTo.forEach(element => {
      submittedEpProgramIds.push(element.enrolledProgramId);
    });
    inProgressEPsUserHasAccessTo.forEach(element => {
      inprogessEpProgramIds.push(element.enrolledProgramId);
    });
    // using index of as IE 11 doesn't support includes
    if (
      enrolledProgramsIds &&
      enrolledProgramsIds.length > 0 &&
      submittedEpProgramIds &&
      submittedEpProgramIds.length > 0 &&
      this.currentEp &&
      this.currentEp.length > 0 &&
      !(inprogessEpProgramIds.indexOf(this.currentEp[0].enrolledProgramId) > -1)
    ) {
      this.showElapsedActivityBt = true;
    }
  }
  editSection() {
    this.edit.emit('activities');
  }
  elapsedSection() {
    this.endElapsedActivities.emit('activities');
  }

  showElapedActivity(event) {
    this.showElapsedActivity.emit();
  }
  singleEntry(a) {
    this.employabilityPlanService.EditActivitySection.next({ readOnly: true, inEditView: true, showControls: false, activity: a });
    this.router.navigateByUrl(`pin/${this.pin}/employability-plan/activities/${a.employabilityPlanId}`);
  }
}
