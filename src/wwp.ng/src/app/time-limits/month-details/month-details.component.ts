import { Component, OnInit, Input, Output, EventEmitter, ChangeDetectionStrategy } from '@angular/core';

import * as moment from 'moment';
import { empty } from 'rxjs';
import { catchError, take } from 'rxjs/operators';

import { AppService } from './../../core/services/app.service';
import { LogService } from '../../shared/services/log.service';
import { Participant } from '../../shared/models/participant';
import { TextTransformer, Utilities } from '../../shared/utilities';
import { TimelineMonth, Timeline, ClockTypes } from '../../shared/models/time-limits';
import { TimeLimitsService } from '../../shared/services/timelimits.service';
import { Authorization } from '../../shared/models/authorization';

@Component({
  selector: 'app-month-details',
  templateUrl: './month-details.component.html',
  styleUrls: ['./month-details.component.css'],
  changeDetection: ChangeDetectionStrategy.Default
})
export class MonthDetailsComponent implements OnInit {
  ReasonsForChange = new Map<number, string>();
  ClockTypes: typeof ClockTypes = ClockTypes;

  @Input()
  month: TimelineMonth;

  @Input()
  timeline: Timeline;

  @Input()
  pin: number;
  @Input()
  participant: Participant;
  @Output()
  save = new EventEmitter<TimelineMonth[]>();

  editMonthOpen = false;
  get editIsAuthorized() {
    return this.appService.isUserAuthorizedToEditTimeLimits(this.participant);
  }

  get canEditMonthBasedOnSecurity(): boolean {
    let canEdit = false;

    // W-2 HelpDesk.
    if (this.appService.isUserAuthorized(Authorization.timeLimitsEditAll)) {
      canEdit = true;
    } else if (
      this.appService.isUserAuthorized(Authorization.timeLimitsEditInAgency) &&
      (this.participant.isParticipantServedByAgency(this.appService.user.agencyCode) || this.appService.user.agencyCode === 'DCF')
    ) {
      // QC and case management can only edit own agency pins.
      canEdit = true;
    } else if (this.appService.isUserAuthorized(Authorization.timeLimitsEdit) && this.participant.isParticipantServedByWorker(this.appService.user.wiuid)) {
      // FEP can only edit own pin.
      canEdit = true;
    }
    return canEdit;
  }
  get canEditMonth(): boolean {
    if (this.month) {
      return (
        this.canEditMonthBasedOnSecurity &&
        this.editIsAuthorized &&
        this.month.date.isSameOrAfter(this.participant.dateOfBirth, 'month') &&
        (this.month.isEdited || !this.month.tick.clockTypes.state.None || this.month.date.isSameOrBefore(Utilities.currentDate, 'month'))
      );
    }
    return false;
  }

  isPlacement(month: TimelineMonth) {
    if (month) {
      let isPlacement = month.tick.clockTypes.state.PlacementLimit;
      let isNo24 = month.tick.clockTypes.state.NOPlacementLimit;
      return isPlacement && !isNo24;
    }
    return false;
  }

  selectedEditMonth: TimelineMonth;

  isLoadingChangeHistory = false;

  get notes() {
    if (this.month) {
      return TextTransformer.wpautop(this.month.tick.notes);
    }
  }

  constructor(private timeLimitsService: TimeLimitsService, private appService: AppService, private logService: LogService) {}

  ngOnInit() {
    this.initReasonsForChange();
  }

  editMonthClicked() {
    this.selectedEditMonth = this.month.clone();
    this.editMonthOpen = true;
    this.logService.event('tl-month-edit');
  }

  closeEditMonth(refresh: boolean) {
    this.selectedEditMonth = null;
  }

  monthSaved(months: TimelineMonth[]) {
    this.save.emit(months);
  }

  private initReasonsForChange() {
    //this.ReasonsForChange = this.timeLimitsService.getReasonsForChange();
    this.timeLimitsService.getReasonsForChange().subscribe(reasons => {
      reasons.map(x => {
        this.ReasonsForChange.set(x.id, x.name);
      });
    });
  }

  loadHistory() {
    this.logService.event('tl-load-history');

    this.isLoadingChangeHistory = true;
    this.timeLimitsService
      .getMonthHistory(this.pin.toString(), this.month.date)
      .pipe(take(1))
      .pipe(
        catchError(() => {
          this.isLoadingChangeHistory = false;
          return empty();
        })
      )
      .subscribe(x => {
        this.month.changeHistory = x.sort((a, b) => {
          let x = moment.isMoment(a.modifiedDate) ? a.modifiedDate : a.createdDate;
          let y = moment.isMoment(b.modifiedDate) ? b.modifiedDate : b.createdDate;

          try {
            return x.diff(y);
          } catch (e) {
            return -1;
          }
        });
        this.isLoadingChangeHistory = false;
        // Trigger change detection
        //this.month = this.month.clone();
      });
  }
}
