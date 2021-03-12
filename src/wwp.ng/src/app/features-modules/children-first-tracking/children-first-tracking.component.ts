import { ChildrenFirstTracking } from './models/children-first-tracking.model';
import { EmployabilityPlanService } from './../employability-plan/services/employability-plan.service';
import { ChildrenFirstTrackingService } from './services/children-first-tracking.service';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { concatMap } from 'rxjs/operators';
import * as moment from 'moment';
import { EnrolledProgramCode } from 'src/app/shared/enums/enrolled-program-code.enum';
import { EmployabilityPlanStatus } from '../employability-plan/enums/employability-plan-status.enum';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { Participant } from 'src/app/shared/models/participant';
import { AppService } from 'src/app/core/services/app.service';
import { of } from 'rxjs';
import { AccessType } from 'src/app/shared/enums/access-type.enum';
import { Utilities } from 'src/app/shared/utilities';
import * as _ from 'lodash';
@Component({
  selector: 'app-children-first-tracking',
  templateUrl: './children-first-tracking.component.html',
  styleUrls: ['./children-first-tracking.component.scss']
})
export class ChildrenFirstTrackingComponent implements OnInit, OnDestroy {
  public goBackUrl: string;
  public pin: string;
  public showCalendar = false;
  public viewDate: any;
  public employabilityPlans: any;
  public isBackwardingOfDisabled = false;
  public isForwardingOfDisabled = false;
  public participant: Participant;
  public participantId: number;
  public iseventsLoaded = false;
  public events: any;
  public singleCTEvent: any;
  public isCFEnabled = false;
  public singleCFEntry: ChildrenFirstTracking;
  //This property is to make the form fields disable on the CF tracking entry if the participant is dosenrolled from CF program.
  public canEdit: boolean;
  public latestEPEndDate: any;
  public viewDate$: any;
  public cftMode$: any;
  public startDate;
  public endDate;
  public currentDate = Utilities.currentDate.format('MM/DD/YYYY');

  constructor(
    private route: ActivatedRoute,
    private partService: ParticipantService,
    private cftService: ChildrenFirstTrackingService,
    private employabilityPlanService: EmployabilityPlanService,
    public appService: AppService
  ) {}

  ngOnInit() {
    this.cftService.viewDate.subscribe(res => {
      this.viewDate = res.viewDate;
    });
    this.route.params
      .pipe(
        concatMap(result => {
          this.pin = result.pin;
          this.goBackUrl = '/pin/' + this.pin;
          return this.partService.getCurrentParticipant();
        })
      )
      .subscribe(res => {
        this.participant = res;
        this.participantId = res.id;
        this.initEPAndEvents();
      });
  }
  private initEPAndEvents() {
    this.employabilityPlanService
      .getEmployabilityPlans(this.pin)
      .pipe(
        concatMap(result => {
          const cfEp = result.filter(
            item =>
              item.enrolledProgramCd.trim().toLowerCase() === EnrolledProgramCode.cf &&
              (item.employabilityPlanStatusTypeName.trim() === EmployabilityPlanStatus.submitted || item.employabilityPlanStatusTypeName.trim() === EmployabilityPlanStatus.ended)
          );
          this.employabilityPlans = cfEp;
          let datestoConsider = [this.currentDate];
          const latestSubmittedEp = this.employabilityPlans.filter(ep => ep.employabilityPlanStatusTypeName === 'Submitted');
          if (!_.isEmpty(latestSubmittedEp)) {
            datestoConsider.push(latestSubmittedEp[0].endDate);
            if (this.participant.programs && this.participant.programs.length > 0) {
              this.participant.programs.forEach(p => {
                if (p.isCF) {
                  datestoConsider.push(p.disenrollmentDate);
                }
              });
            }
          }
          datestoConsider = datestoConsider.filter(i => {
            return i !== null;
          });
          this.latestEPEndDate = new Date(
            Math.min.apply(
              null,
              datestoConsider.map(function(e) {
                return new Date(e);
              })
            )
          );
          return of(this.employabilityPlans);
          // Here if EP exists and the current date is after the EP enddate we are setting the viewDate to  the EP endate else to the current month.
        })
      )
      .subscribe(res => {
        if (
          res.length > 0 &&
          moment(this.viewDate).isSameOrAfter(
            moment(this.latestEPEndDate || this.currentDate)
              .startOf('month')
              .add(1, 'M')
              .format('MM-DD-YYYY')
          )
        ) {
          this.viewDate = moment(this.latestEPEndDate || this.currentDate)
            .startOf('month')
            .toDate();
        } else {
          this.viewDate = moment(this.viewDate)
            .startOf('month')
            .toDate();
        }
        this.getEvents();
        // enrolledCFProgram exists only if the isEnrolled is true and that will only be true if enrolledStatus is 'enrolled'
        this.canEdit =
          this.appService.coreAccessContext.evaluate() === AccessType.edit && this.participant.enrolledCFProgram && this.participant.enrolledCFProgram.disenrollmentDate === null;
        this.showCalendar = true;
      });
  }
  showCalendarView() {
    this.showCalendar = !this.showCalendar;
  }
  closeCalendar(e: boolean) {
    this.showCalendar = e;
  }
  nextMonthClicked(e) {
    this.cftService.viewDate.next({ viewDate: e });
    this.getEvents();
  }
  previousMonthClicked(e) {
    this.cftService.viewDate.next({ viewDate: e });
    this.getEvents();
  }

  getEvents() {
    // let startDate;
    // let endDate;
    this.iseventsLoaded = false;
    this.startDate = moment(this.viewDate)
      .startOf('month')
      .format('MM-DD-YYYY');
    this.endDate = moment(this.viewDate)
      .startOf('month')
      .endOf('month')
      .format('MM-DD-YYYY');
    if (this.participant.programs && this.participant.programs.some(p => p.isCF) && this.latestEPEndDate) {
      let enrollmentDate = '';
      this.participant.programs.forEach(p => {
        if (p.isCF) {
          enrollmentDate = p.enrollmentDateMmDdYyyy;
        }
      });
      // The user can navigate back to the month of enrollment date of the program and navigate forward up to the month of the EP endDate.
      this.isBackwardingOfDisabled = moment(this.startDate).isSameOrBefore(
        moment(enrollmentDate)
          .startOf('month')
          .format('MM-DD-YYYY')
      );
      this.isForwardingOfDisabled = moment(this.startDate).isSameOrAfter(
        moment(this.latestEPEndDate)
          .startOf('month')
          .format('MM-DD-YYYY')
      );
      //sice this is only called for children first program i am hard coding the the program code
      this.cftService.getChildrenTrackingDetails(this.pin, this.participantId, this.startDate, this.endDate, 'cf', false).subscribe(res => {
        this.iseventsLoaded = true;
        this.events = res;
      });
    } else {
      this.iseventsLoaded = true;
    }
  }
  makeFullOrNoParticipation(e) {
    this.iseventsLoaded = false;
    this.cftService
      .makeFullOrNoParticipation(
        this.pin,
        this.participantId,
        e.innerText
          .split(' ')
          .join('')
          .toLowerCase(),
        this.startDate,
        this.endDate,
        'cf',
        e.details.allEventsForaWeek
      )
      .subscribe(res => {
        this.events = res;
        this.iseventsLoaded = true;
      });
  }
  loadCTE(e) {
    this.singleCFEntry = e;
    this.cftService.modeForCFParticipationEntry.next({ readOnly: false, inEditView: true });
    this.cftService.modeForCFParticipationEntry.subscribe(res => {
      this.isCFEnabled = res.inEditView;
      if (!this.isCFEnabled) {
        this.getEvents();
      }
    });
  }
  ngOnDestroy() {
    this.cftService.viewDate.next({ viewDate: moment().toDate() });
  }
}
