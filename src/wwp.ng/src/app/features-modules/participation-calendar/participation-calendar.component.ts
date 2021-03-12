import { FieldDataTypes } from 'src/app/shared/enums/field-data-types.enum';
import { FieldDataService } from './../../shared/services/field-data.service';
import { Participant } from './../../shared/models/participant';
import { Component, OnInit, OnDestroy } from '@angular/core';

import * as moment from 'moment';
import { ActivatedRoute } from '@angular/router';
import { take, concatMap } from 'rxjs/operators';
import { ParticipationTracking } from '../../shared/models/participation-tracking.model';
import { AppService } from 'src/app/core/services/app.service';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { ParticipationTrackingService } from './services/participation-tracking.service';
import { Utilities } from 'src/app/shared/utilities';
import { forkJoin } from 'rxjs';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-participation-calendar',
  templateUrl: './participation-calendar.component.html',
  styleUrls: ['./participation-calendar.component.scss']
})
export class ParticipationCalendarComponent implements OnInit, OnDestroy {
  public showCalendar = true;
  public clonedCurrentDate = Utilities.currentDate.clone().toDate();
  public currentDate: moment.Moment;
  public momentDate: Date;
  public viewDate: any;
  public events: any;
  public PTDetails: ParticipationTracking[];
  public participantId: number;
  public goBackUrl: string;
  public pin;
  public iseventsLoaded = false;
  public isPEEnabled = false;
  public singlePTEvent: any;
  public participant: Participant;
  public isBackwardingDisabled = false;
  public PTFeatureDate;
  public startDate;
  public endDate;
  public canEdit;
  public isLoaded = false;
  public isForwardingCalendarDisabled = false;
  public pullDownDates: DropDownField[];
  public isAdjustDateCalled = false;
  private eventsSub = new SubSink();

  constructor(
    private route: ActivatedRoute,
    private appService: AppService,
    private partService: ParticipantService,
    private ptService: ParticipationTrackingService,
    private fdService: FieldDataService
  ) {}

  ngOnInit() {
    this.pin = this.route.snapshot.params.pin;
    this.goBackUrl = '/pin/' + this.pin;
    this.clonedCurrentDate.setHours(0, 0, 0, 0);
    this.currentDate = moment(this.clonedCurrentDate);
    this.momentDate = this.clonedCurrentDate;

    forkJoin(this.partService.getCachedParticipant(this.pin).pipe(take(1)), this.fdService.getFieldDataByField(FieldDataTypes.PullDownDates).pipe(take(1)))
      .pipe(
        concatMap(results => {
          if (results[0]) {
            this.participant = results[0];
            this.participantId = results[0].id;
            this.pullDownDates = results[1];
            this.canEdit = this.ptService.canEdit;
            return this.ptService.viewDate;
          }
        })
      )
      .subscribe(res => {
        this.viewDate = res.viewDate;
        this.startDate = moment(this.viewDate).format('MMDDYYYY');
        this.PTFeatureDate = moment(new Date(this.appService.getFeatureToggleValue('ParticipationTracking'))).toDate();
        this.endDate = moment(this.viewDate)
          .add(1, 'M')
          .subtract(1, 'day')
          .format('MMDDYYYY');
        this.setForwardingAndBackwardingForCalendar();
        this.adjustDate(this.latestPullDownDate(this.pullDownDates));
      });
  }

  setForwardingAndBackwardingForCalendar() {
    if (
      moment(this.viewDate)
        .add(1, 'M')
        .isBefore(this.currentDate, 'day')
    ) {
      this.isForwardingCalendarDisabled = false;
    } else if (
      moment(this.viewDate)
        .add(1, 'M')
        .isSame(this.currentDate, 'month') &&
      moment(this.currentDate).date() > 15
    ) {
      this.isForwardingCalendarDisabled = false;
    } else {
      this.isForwardingCalendarDisabled = true;
    }
    if (
      moment(this.viewDate)
        .add(1, 'M')
        .isSameOrBefore(this.PTFeatureDate, 'day')
    ) {
      this.isBackwardingDisabled = true;
    } else {
      this.isBackwardingDisabled = false;
    }
  }

  private initEvents() {
    this.eventsSub.add(
      this.ptService.modeForParticipationEntry.subscribe(res => {
        this.isPEEnabled = res.inEditView;
        if (!this.isPEEnabled) {
          this.getEvents();
        }
      })
    );
  }

  showCalendarView() {
    this.showCalendar = !this.showCalendar;
  }

  nextMonthClicked(e) {
    this.ptService.viewDate.next({ viewDate: e });
    this.getEvents();
  }

  previousMonthClicked(e) {
    this.ptService.viewDate.next({ viewDate: e });
    this.getEvents();
  }

  getEvents() {
    this.iseventsLoaded = false;
    this.ptService
      .getParticipationTrackingDetails(this.pin, this.participantId, this.startDate, this.endDate, false)
      .pipe(take(1))
      .subscribe(res => {
        this.events = res;
        this.iseventsLoaded = true;
        this.isLoaded = true;
      });
  }

  loadPE(e) {
    this.singlePTEvent = e;
    this.ptService.modeForParticipationEntry.next({ readOnly: false, inEditView: true });
  }
  latestPullDownDate(pullDownDates) {
    return moment(moment(Utilities.getPullDownDate(pullDownDates, moment(this.currentDate, 'MM/DD/YYYY'))).format('MM/DD/YYYY'));
  }

  makeFullOrNoParticipation(e) {
    this.iseventsLoaded = false;
    this.ptService
      .makeFullOrNoParticipation(
        this.pin,
        this.participantId,
        e.innerText
          .split(' ')
          .join('')
          .toLowerCase(),
        this.startDate,
        this.endDate,
        e.details.allEventsForaWeek
      )
      .subscribe(res => {
        this.events = res;
        this.iseventsLoaded = true;
      });
  }

  public adjustDate(pullDownDate) {
    let canCallInitEvents = true;
    if (!this.isAdjustDateCalled && this.viewDate && moment(this.viewDate).isSame(this.currentDate, 'month')) {
      this.isAdjustDateCalled = true;

      if (
        !this.iseventsLoaded &&
        !(moment(this.currentDate, 'MM/DD/YYYY').isAfter(`${pullDownDate.month() + 1}/16/${pullDownDate.year()}`) && moment(this.currentDate, 'MM/DD/YYYY').isAfter(pullDownDate))
      ) {
        canCallInitEvents = false;
        this.ptService.viewDate.next({
          viewDate: moment(
            new Date(
              moment(this.momentDate)
                .subtract(1, 'M')
                .toDate()
                .getFullYear(),
              moment(this.momentDate)
                .subtract(1, 'M')
                .toDate()
                .getMonth(),
              16
            )
          ).toDate()
        });
      }
    }

    if (canCallInitEvents && !this.iseventsLoaded) this.initEvents();
  }

  ngOnDestroy() {
    if (this.eventsSub) this.eventsSub.unsubscribe();
    this.ptService.viewDate.next({ viewDate: moment(new Date(this.momentDate.getFullYear(), this.momentDate.getMonth(), 16)).toDate() });
  }
}
