import { FeatureToggleTypes } from 'src/app/shared/enums/feature-toggle-types.enum';
import { FieldDataTypes } from './../../../shared/enums/field-data-types.enum';
import { Event } from './../../../shared/models/event.model';
import { Utilities } from './../../../shared/utilities';
import { FieldDataService } from 'src/app/shared/services/field-data.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { SubSink } from 'subsink';
import * as moment from 'moment';
import * as _ from 'lodash';
import { forkJoin } from 'rxjs';
import { take } from 'rxjs/operators';
import { ModelErrors } from 'src/app/shared/interfaces/model-errors';
import { Participant } from 'src/app/shared/models/participant';
import { EmployabilityPlan } from '../models/employability-plan.model';
import { Activity } from '../models/activity.model';
import { Goal } from '../models/goal.model';
import { SupportiveService } from 'src/app/features-modules/employability-plan/models/supportive-service.model';
import { ValidationManager } from 'src/app/shared/models/validation';
import { ParticipationStatus } from 'src/app/shared/models/participation-statuses.model';
import { EnrolledProgram } from 'src/app/shared/models/enrolled-program.model';
import { WhyReason } from 'src/app/shared/models/why-reasons.model';
import { EmployabilityPlanService } from '../services/employability-plan.service';
import { SupportiveServiceService } from '../services/supportive-service.service';
import { AppService } from 'src/app/core/services/app.service';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { EmployabilityPlanOverview } from '../models/employability-plan-overview';
import { AccessType } from 'src/app/shared/enums/access-type.enum';
import { EmployabilityPlanStatus } from '../enums/employability-plan-status.enum';
import { ReportService } from 'src/app/shared/services/report.service';
import { PrintEP } from 'src/app/shared/models/print-ep.model';
import { DropDownMultiField } from 'src/app/shared/models/dropdown-multi-field';
import { EnrolledProgramCode } from 'src/app/shared/enums/enrolled-program-code.enum';

@Component({
  selector: 'app-ep-overview',
  templateUrl: './ep-overview.component.html',
  styleUrls: ['./ep-overview.component.scss']
})
export class EpOverviewComponent implements OnInit, OnDestroy {
  public id: number;
  public goBackToOverviewUrl: string;
  public isElapsedActivity: boolean;
  public pin: string;
  public employabilityPlanId: string;
  public goBackUrl: string;
  public isCollapsed = false;
  public inConfirmDeleteView = false;
  public isSectionValid = true;
  public modelErrors: ModelErrors = {};
  public isEpInProgress = true;
  public isEpDeleted = false;
  public isEpDeletedByWorker = false;
  public detailsLoaded = false;
  public participant: Participant;
  public employabilityPlan: EmployabilityPlan;
  public activities: Activity[];
  public goals: Goal[];
  public supportiveServices: SupportiveService[];
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public isElapsedChanges = false;
  private subs = new SubSink();
  public employments: any;
  private employmentInfo: any;
  public allActivities: Activity[];
  elapsedActivityNotEnded: boolean;
  public elapsedNotEndedactivities: Activity[];
  public participationStatuses: ParticipationStatus[] = [];
  public localParticipationStatuses: ParticipationStatus[];
  public isPSLoaded: boolean;
  public currentlyEnrolledPrograms: EnrolledProgram[];
  public canView: boolean;
  public precheck: WhyReason = new WhyReason();
  public activityTypeIds: string;
  public isSubmitting = false;
  public showEndEPButton = false;
  public showPrintEPButton = false;
  public showSubmitSuccess = false;
  public showSubmitFail = false;
  public subjectdetailsLoaded = false;
  public saveSuccess = false;
  currentEp: any;
  public isEpSubmittedRecently = false;
  public empInfo: any;
  public empInfoObj: any;
  public showCalendar = false;
  protected showCCAuthorizations = false;
  protected showCCAuthorizationsButton = false;
  public events: Event[];
  public iseventsLoaded: boolean;
  public pdfLoaded = false;
  public viewDate: any;
  public isBackwardingOfCalendarDisabled = false;
  public isForwardingofCalendarDisabled = false;
  private pullDownDates: DropDownMultiField[] = [];
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private employabilityPlanService: EmployabilityPlanService,
    private supportiveServiceService: SupportiveServiceService,
    public appService: AppService,
    private partService: ParticipantService,
    private reportService: ReportService,
    private fdService: FieldDataService
  ) {}

  ngOnInit() {
    this.appService.isEventsLoaded.next(false);
    // setting the dataModified flag to false on the
    this.appService.componentDataModified.next({ dataModified: false });
    this.showCCAuthorizationsButton = this.appService.getFeatureToggleDate(FeatureToggleTypes.CCAuthorizations);
    this.subs.add(
      forkJoin(this.route.params.pipe(take(1)), this.route.parent.params.pipe(take(1)))
        .pipe(take(1))
        .subscribe(results => {
          this.employabilityPlanId = results[0].id;
          this.pin = results[1].pin;
          this.goBackUrl = '/pin/' + this.pin + '/employability-plan/list';
          this.getDetails();
        })
    );
  }

  sidebarToggle() {
    this.isCollapsed = !this.isCollapsed;
    return this.isCollapsed;
  }
  onSubmit(fromWarning = false) {
    if (!fromWarning) {
      forkJoin(
        this.employabilityPlanService.canAddActivity(this.pin, +this.employabilityPlanId, this.activityTypeIds ? this.activityTypeIds : '0'),
        this.employabilityPlanService.canSaveEP(this.pin, this.participant.id, true, this.employabilityPlan)
      )
        .pipe()
        .subscribe(
          res => {
            this.precheck.errors = [...res[0].errors, ...res[1].errors];
            this.precheck.warnings = [...res[0].warnings, ...res[1].warnings];

            if (
              this.employabilityPlan.enrolledProgramCd &&
              this.employabilityPlan.enrolledProgramCd.trim().toLowerCase() === EnrolledProgramCode.w2 &&
              Utilities.participationPeriodCheck(this.pullDownDates, moment(this.employabilityPlan.beginDate, 'MM/DD/YYYY'), true)
            ) {
              this.precheck.errors.push('When action is taken after Pulldown, the EP Begin Date can be no earlier than first day of the current participation period.');
              res[0].status = false;
            }

            this.precheck.status = res[0].status && res[1].status;

            return;
          },
          () => {},
          // This will be called once the above subscription is complete and validate and save are called only when we dont have any errors from precheck
          () => {
            if (this.precheck.status !== true) {
              this.isSectionValid = false;
            }
            this.validate(this.isSectionValid, this.precheck.warnings.length);
            return;
          }
        );
    } else {
      this.precheck.warnings = [];
      this.validate(this.isSectionValid, this.precheck.warnings.length);
      return;
    }
  }

  closeSubmit() {
    //in navigate url, the url is wrong so navigate will return false and moves on to navigate by url
    this.router
      .navigate([`pin/${this.pin}/employability-plan/overview/`], { skipLocationChange: true })
      .then(() => this.router.navigateByUrl(`pin/${this.pin}/employability-plan/overview/${this.employabilityPlanId}`));
    this.showSubmitSuccess = false;
    this.showSubmitFail = false;
  }

  delete() {
    this.inConfirmDeleteView = true;
  }
  onConfirmDelete() {
    const autoDelete = false;
    this.employabilityPlanService.deleteEP(this.pin, this.employabilityPlanId, autoDelete).subscribe(res => {
      if (res) {
        this.inConfirmDeleteView = false;
        this.router.navigateByUrl(`/pin/${this.pin}/employability-plan/list`);
      }
    });
  }
  onCancelDelete() {
    this.inConfirmDeleteView = false;
  }
  showCalendarView() {
    this.showCalendar = !this.showCalendar;
  }
  closeCalendar(e: boolean) {
    this.setViewDateOnCalendar();
    this.showCalendar = e;
    this.isBackwardingOfCalendarDisabled = false;
    this.isForwardingofCalendarDisabled = false;
  }
  nextMonthClicked(e) {
    this.employabilityPlanService.viewDate.next({ viewDate: e });
    this.getEvents();
  }
  previousMonthClicked(e) {
    this.employabilityPlanService.viewDate.next({ viewDate: e });
    this.getEvents();
  }
  getEvents() {
    let startDate;
    this.employabilityPlanService.viewDate.subscribe(res => {
      startDate = moment(res.viewDate)
        .startOf('month')
        .format('MM-DD-YYYY');
    });

    if (this.employabilityPlan) {
      this.isBackwardingOfCalendarDisabled = moment(startDate).isSameOrBefore(
        moment(this.employabilityPlan.beginDate)
          .startOf('month')
          .subtract(1, 'M')
          .format('MM-DD-YYYY')
      );
      this.isForwardingofCalendarDisabled = moment(startDate).isSame(
        moment(this.employabilityPlan.endDate)
          .startOf('month')
          .format('MM-DD-YYYY')
      );
    }

    const isEventsLoaded = this.appService.isEventsLoaded.value;

    if (!isEventsLoaded) {
      this.employabilityPlanService.getEvents(this.pin, +this.employabilityPlanId, this.employabilityPlan.enrolledProgramId, this.employabilityPlan.beginDate).subscribe(res => {
        if (res) {
          this.events = res;
          this.appService.cachedEvents.next(res);
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

  private setColorOnEvents() {
    this.events.map(e => {
      if (moment(e.start).isBefore(this.employabilityPlan.beginDate)) {
        e.color = 'blue';
      }
    });
  }
  private getDetails() {
    this.subs.add(
      this.requestDataFromMultipleSources()
        .pipe(take(1))
        .subscribe(results => {
          this.initEp(results[0]);
          this.goals = results[1];
          this.activities = results[2];
          this.appService.activitiesFromOverview.next(this.activities);
          this.appService.goalsFromOverview.next(this.goals);
          this.allActivities = results[2];
          this.pullDownDates = results[6];
          this.activities.filter(activity => {
            if (activity && activity !== null) {
              if (!this.activityTypeIds || this.activityTypeIds === null) this.activityTypeIds = `${activity.activityTypeId}`;
              else this.activityTypeIds = `${this.activityTypeIds},${activity.activityTypeId}`;
            }
          });
          this.supportiveServices = results[3];
          this.participant = results[4];
          this.getAllStatusesForPin(results[5]);
          this.getEvents();
          this.showPrint(results[0]);
        })
    );
  }

  public requestDataFromMultipleSources(): Observable<any[]> {
    const response1 = this.employabilityPlanService.getEpById(this.pin, this.employabilityPlanId).pipe(take(1));
    const response2 = this.employabilityPlanService.getGoals(this.pin, this.employabilityPlanId).pipe(take(1));
    const response3 = this.employabilityPlanService.getActivities(this.pin, this.employabilityPlanId).pipe(take(1));
    const response4 = this.supportiveServiceService.getSupportiveServices(this.pin, this.employabilityPlanId).pipe(take(1));
    const response5 = this.partService.getCurrentParticipant();
    const response6 = this.partService.getAllStatusesForPin(this.pin);
    const response7 = this.fdService.getFieldDataByField(FieldDataTypes.PullDownDates).pipe(take(1));
    return forkJoin([response1, response2, response3, response4, response5, response6, response7]);
  }
  private initEp(ep) {
    this.employabilityPlan = ep;
    this.isEpInProgress = ep.employabilityPlanStatusTypeName === EmployabilityPlanStatus.inProgress;
    this.isEpDeleted = ep.isDeleted;
    this.isEpDeletedByWorker = ep.isWorkerDeleted;
    this.setViewDateOnCalendar();
    this.appService.employabilityPlan.next(ep);

    this.getData().subscribe(result => {
      if (result[0] && result[0].results) {
        this.empInfoObj = result[0].results;
      } else if (result[0] && result[0].beginDate !== undefined) {
        this.empInfo = result[0];
      }
      this.isEpSubmittedRecently = result[1].submittedEp;
      this.showHideButton(this.empInfo, this.empInfoObj, ep);
      this.appService.submittedEpGoToOverview.next({ submittedEp: false });
    });
    this.employabilityPlanService.viewDate.subscribe(res => {
      this.viewDate = res.viewDate;
    });

    this.employabilityPlanService
      .getEmploymentForEP(this.pin, +this.employabilityPlanId, this.employabilityPlan.beginDate.split('/').join('-'), this.employabilityPlan.enrolledProgramCd)
      .subscribe(res => {
        this.employments = res.filter(i => i.isSelected === true);
        this.employmentInfo = res;
        this.detailsLoaded = true;
      });
  }

  private setViewDateOnCalendar() {
    if (!this.isEpInProgress) {
      if (this.employabilityPlan.employabilityPlanStatusTypeName === EmployabilityPlanStatus.ended) {
        this.employabilityPlanService.viewDate.next({
          viewDate: moment(this.employabilityPlan.beginDate)
            .startOf('month')
            .toDate()
        });
      } else if (this.employabilityPlan.employabilityPlanStatusTypeName === EmployabilityPlanStatus.submitted) {
        if (this.employabilityPlan.endDate && moment(this.employabilityPlan.endDate).isBefore(moment())) {
          this.employabilityPlanService.viewDate.next({
            viewDate: moment(this.employabilityPlan.beginDate)
              .startOf('month')
              .toDate()
          });
        } else {
          this.employabilityPlanService.viewDate.next({
            viewDate: moment()
              .startOf('month')
              .toDate()
          });
        }
      } else {
        this.employabilityPlanService.viewDate.next({
          viewDate: moment()
            .startOf('month')
            .toDate()
        });
      }
    } else if (this.isEpInProgress) {
      this.employabilityPlanService.viewDate.next({
        viewDate: moment(this.employabilityPlan.beginDate)
          .startOf('month')
          .toDate()
      });
    }
  }

  private showPrint(ep) {
    const progs = this.participant.getMostRecentProgramsUserHasAccessTo(this.appService.user, this.appService);
    const epProgs = [];
    progs.forEach(prog => {
      if (prog.enrolledProgramId === ep.enrolledProgramId) epProgs.push(prog);
    });

    if (ep.employabilityPlanStatusTypeName === 'Submitted' && epProgs.length > 0) this.showPrintEPButton = true;
  }

  public getData() {
    return forkJoin([this.appService.employabilityPlanInfo.pipe(take(1)), this.appService.submittedEpGoToOverview.pipe(take(1))]);
  }
  private showHideButton(data?: any, dataObj?: any, selectedEp?) {
    if ((data && data.length > 0) || (dataObj && dataObj.length > 0)) {
      const enrolledPrograms =
        dataObj.length > 0
          ? dataObj[1].getCurrentEnrolledProgramsUserHasAccessTo(this.appService.user, this.appService)
          : data[1].getCurrentEnrolledProgramsUserHasAccessTo(this.appService.user, this.appService);
      const submittedEps =
        dataObj.length > 0 ? dataObj[0].filter(i => i.employabilityPlanStatusTypeName === 'Submitted') : data[0].filter(i => i.employabilityPlanStatusTypeName === 'Submitted');
      const submittedEPsUserHasAccessTo = submittedEps.filter(ep => this.appService.isUserAuthorizedForProgramByCode(ep.enrolledProgramCd));
      const inProgressEP =
        dataObj.length > 0 ? dataObj[0].filter(i => i.employabilityPlanStatusTypeName === 'In Progress') : data[0].filter(i => i.employabilityPlanStatusTypeName === 'In Progress');
      const inProgressEPsUserHasAccessTo = inProgressEP.filter(ep => this.appService.isUserAuthorizedForProgramByCode(ep.enrolledProgramCd));
      const enrolledProgramsIds = [];
      const submittedEpProgramIds = [];
      const inprogessEpProgramIds = [];
      this.currentEp =
        dataObj.length > 0
          ? dataObj[0].filter(ep => ep.employabilityPlanStatusTypeName === 'Submitted' && ep.enrolledProgramCd === this.employabilityPlan.enrolledProgramCd)
          : data[0].filter(ep => ep.employabilityPlanStatusTypeName === 'Submitted' && ep.enrolledProgramCd === this.employabilityPlan.enrolledProgramCd);
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
        (enrolledProgramsIds &&
          enrolledProgramsIds.length > 0 &&
          submittedEpProgramIds &&
          submittedEpProgramIds.length > 0 &&
          this.currentEp &&
          this.currentEp.length > 0 &&
          !(inprogessEpProgramIds.indexOf(this.currentEp[0].enrolledProgramId) > -1) &&
          inProgressEPsUserHasAccessTo &&
          inProgressEPsUserHasAccessTo.length === 0 &&
          selectedEp &&
          selectedEp.employabilityPlanStatusTypeName !== 'Ended' &&
          selectedEp.employabilityPlanStatusTypeName !== 'In Progress' &&
          this.appService.isUserAuthorizedForProgramByCode(this.employabilityPlan.enrolledProgramCd)) ||
        (this.isEpSubmittedRecently && selectedEp.employabilityPlanStatusTypeName !== 'Ended' && selectedEp.employabilityPlanStatusTypeName !== 'In Progress')
      ) {
        this.showEndEPButton = true;
        this.subjectdetailsLoaded = true;
      } else {
        this.subjectdetailsLoaded = true;
      }
    } else if (this.isEpSubmittedRecently && selectedEp.employabilityPlanStatusTypeName !== 'Ended' && selectedEp.employabilityPlanStatusTypeName !== 'In Progress') {
      this.showEndEPButton = true;
      this.subjectdetailsLoaded = true;
    } else {
      this.showEndEPButton = false;
      this.subjectdetailsLoaded = true;
    }
  }

  public getAllStatusesForPin(res) {
    res.filter(data => {
      if (data.enrolledProgramName === this.employabilityPlan.enrolledProgramName) this.participationStatuses.push(data);
    });
    this.localParticipationStatuses = _.orderBy(this.participationStatuses.slice(), ['isCurrent', 'BeginDate'], ['desc', 'asc']);
  }

  public editSection(epComponent) {
    if (epComponent) {
      this.appService.employabilityPlanInfo.next(this.employabilityPlan);
      const url = `/pin/${this.pin}/employability-plan/${epComponent}/${this.employabilityPlanId}`;
      this.router.navigateByUrl(url);
    } else {
      const url = `/pin/${this.pin}/employability-plan/${this.employabilityPlanId}`;
      this.router.navigateByUrl(url);
    }
  }

  public elapsedSection() {
    this.isElapsedActivity = true;
  }

  checkIfAnyElapsedActivitiesToSave(activities: Activity[]) {
    this.elapsedNotEndedactivities = activities.filter(activity => {
      // In an In progress EP, in the elapsed activity section driver flow, we check if the activity is carried over and if the ep is in progress
      if (activity.isCarriedOver && activity.activityCompletionReasonId === null) {
        return moment(activity.maxEndDate, 'MM/DD/YYYY').isBefore(moment(this.employabilityPlan.beginDate).format('MM/DD/YYYY'));

        // If no activity is carried over
      }
    });
  }

  showEndEmployabilityPlan() {
    const url = `/pin/${this.pin}/employability-plan/end-employability-plan`;
    this.router.navigateByUrl(url, { state: { pullDownDates: this.pullDownDates, employabilityPlanId: this.employabilityPlanId } });
  }

  generatePdf() {
    this.pdfLoaded = true;
    const epModel: PrintEP = new PrintEP();
    epModel.employabilityPlan = this.employabilityPlan;
    epModel.goals = this.goals;
    epModel.employmentInfo = this.employmentInfo.filter(i => i.isSelected);
    epModel.activities = this.activities;
    epModel.supportiveServices = this.supportiveServices;
    epModel.participant = this.participant;
    this.reportService.getEpReport(this.pin, epModel).subscribe(
      data => {
        const blob: any = new Blob([data], { type: 'application/pdf' });
        if (blob) {
          //IE doesn't allow using a blob object directly as link href instead it is necessary to use msSaveOrOpenBlob
          if (window.navigator && window.navigator.msSaveOrOpenBlob) {
            window.navigator.msSaveOrOpenBlob(blob, `EmployabilityPlan__ ${this.pin}.pdf`);
            this.pdfLoaded = false;
            return;
          }

          // For other browsers:
          // Create a link pointing to the ObjectURL containing the blob.
          const blobUrl = URL.createObjectURL(blob);
          window.open(blobUrl, `EmployabilityPlan__ ${this.pin}.pdf`);
          setTimeout(() => {
            //Revoke the blob url after the tab is opened
            window.URL.revokeObjectURL(blobUrl);
          }, 1000);
          this.pdfLoaded = false;
        }
      },
      () => {
        this.pdfLoaded = false;
      }
    );
  }

  validate(isValid: boolean, warning: number) {
    const epOverView = new EmployabilityPlanOverview();
    this.checkIfAnyElapsedActivitiesToSave(this.allActivities);
    const result = epOverView.validate(this.validationManager, this.employabilityPlan, this.allActivities, this.goals, this.elapsedNotEndedactivities);
    this.isSectionValid = result.isValid && (!this.precheck.errors || this.precheck.errors.length === 0);
    this.modelErrors = result.errors;
    if (this.isSectionValid && isValid && warning === 0) {
      this.isSubmitting = true;
      this.employabilityPlanService.submitEp(this.pin, +this.employabilityPlanId).subscribe(
        res => {
          if (res) {
            this.showSubmitSuccess = true;
            this.saveSuccess = true;
            // storing value that tells us if the ep was submitted recently
            this.appService.submittedEpGoToOverview.next({ submittedEp: true });
            this.employabilityPlanService.EditActivitySection.next({ readOnly: true, inEditView: false, showControls: false });
            this.employabilityPlanService.EditGoalSection.next({ readOnly: true, inEditView: false, showControls: false, isHistory: false });
          }
        },
        error => {
          this.isSubmitting = false;
          this.saveSuccess = false;
        }
      );
    } else this.isSubmitting = false;
  }

  exitToEp() {
    this.isElapsedActivity = false;
  }

  elapsedSaveChanges(isSaved: boolean) {
    this.isElapsedChanges = isSaved;
  }

  exitElapsedActivityEditIgnoreChanges($event) {
    this.appService.isDialogPresent = false;
    this.isElapsedActivity = false;
  }
  exitToEpView() {
    this.isElapsedChanges ? (this.appService.isDialogPresent = true) : (this.isElapsedActivity = false);
  }
  hasEditEpAccess(): boolean {
    let canEdit = false;
    if (this.appService.coreAccessContext) {
      if (this.appService.coreAccessContext.evaluate() === AccessType.edit) {
        const programsUserHasAccessto = this.participant.getCurrentEnrolledProgramsUserHasAccessTo(this.appService.user, this.appService);
        if (programsUserHasAccessto.length > 0) {
          programsUserHasAccessto.some(program => {
            if (program.programCode === this.employabilityPlan.enrolledProgramName) {
              canEdit = true;
              return true;
            }
          });
        } else {
          return false;
        }
      } else {
        return false;
      }
    } else {
      return false;
    }
    return canEdit;
  }

  showWarningBtn() {
    return (
      !this.isSubmitting &&
      this.precheck.warnings &&
      this.precheck.warnings.length > 0 &&
      (!this.precheck.errors || this.precheck.errors.length === 0) &&
      (!this.validationManager.errors || this.validationManager.errors.length === 0)
    );
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
    // // avoid memory leaks here by cleaning up after ourselves. If we
    // // don't then we will continue to run our initialiseInvites()
    // // method on every navigationEnd event.
    // if (this.navigationSubscription) {
    //   this.navigationSubscription.unsubscribe();
    // }
  }
}
