// tslint:disable: class-name
// tslint:disable: semicolon
import { Component, OnInit, Input, Output, EventEmitter, OnChanges, OnDestroy, SimpleChanges } from '@angular/core';
import { MonthView } from 'calendar-utils';
import * as moment from 'moment';
import { CalendarUtils } from '../../models/calendar-utils.provider';
import * as _ from 'lodash';
import { Utilities } from '../../utilities';

class WeeklyColumn {
  hours: number;
}

class headerEntry {
  title: string;
  isPast: boolean;
  isFuture: boolean;
  isToday: boolean;
  isWeekend;
  boolean;
}

@Component({
  selector: 'app-calendar',
  styleUrls: ['./calendar.component.scss'],
  templateUrl: './calendar.component.html',
  // tslint:disable-next-line: use-host-property-decorator
  host: {
    class: 'cal-cell cal-day-cell',
    '[class.cal-past]': 'day?.isPast',
    '[class.cal-today]': 'day?.isToday',
    '[class.cal-future]': 'day?.isFuture',
    '[class.cal-weekend]': 'day?.isWeekend',
    '[class.cal-in-month]': 'day?.inMonth',
    '[class.cal-out-month]': '!day?.inMonth',
    '[class.cal-has-events]': 'day?.events?.length > 0',
    '[class.cal-event-highlight]': '!!day?.backgroundColor'
  }
})
export class CalendarComponent implements OnInit, OnChanges, OnDestroy {
  @Input() events: any;
  // this will be true for the participation tracking period whcih is 16 to 15.
  @Input() isPTPeriod = false;
  // this is for full and no participation buttons.
  @Input() extraColumnWithButtons: false;
  @Input() isWeeklyHoursRequired = false;
  @Input() viewDate: any;
  @Input() eventsName: string;
  @Input() isForwardingDisabled = false;
  @Input() isBackwardingDisabled = false;
  @Input() iseventsLoaded = false;
  @Input() isCloseButtonRequired = true;
  @Input() canEdit = false;

  @Output() closeCalendar = new EventEmitter<any>();
  @Output() activityClicked = new EventEmitter<any>();
  @Output() buttonClicked = new EventEmitter<any>();
  @Output() previousMonthClicked = new EventEmitter<any>();
  @Output() nextMonthClicked = new EventEmitter<any>();

  public view: MonthView;
  public viewRender: any;
  public weekStartsOn = 0;
  public headerMonth = moment().format('MMMM');
  public headerNextMonth;
  public columnHeaders: any[];

  trackByRowOffset = (index: number, offset: number) =>
    this.viewRender.days
      .slice(offset, this.viewRender.totalDaysVisibleInWeek)
      .map(day => {
        if (day.date) {
          return day.date.toISOString();
        } else {
          return day.hours;
        }
      })
      .join('-');

  trackByDate = (index: number, day: any) => {
    if (day.date) {
      return day.date.toISOString();
    } else {
      return day;
    }
  };

  trackByWeekDayHeaderDate = (index: number, day: any) => {
    if (day.date) {
      return day.date.toISOString();
    } else {
      return day;
    }
  };

  constructor(private utils: CalendarUtils) {}

  ngOnInit() {
    this.headerMonth = moment(this.viewDate).format('MMMM');
    this.initData();
  }

  private initData() {
    this.refreshHeader();
    this.getEvents();
  }

  ngOnChanges(changes: SimpleChanges) {
    for (const propName in changes) {
      if (changes.hasOwnProperty(propName)) {
        switch (propName) {
          case 'viewDate':
            this.headerMonth = moment(this.viewDate).format('MMMM');
            break;
          case 'iseventsLoaded':
            this.initData();
            break;
          default:
            break;
        }
      }
    }
  }

  prepareEvents() {
    if (!this.events) {
      return;
    } else {
      this.events.map(e => {
        if (!e.start) {
          e.start = e.participationDate;
        }
        e.start = moment(e.start).toDate();
      });
    }
  }
  getViewToRender() {
    this.view = this.utils.getMonthView({
      events: this.events,
      viewDate: this.viewDate,
      weekStartsOn: this.weekStartsOn,
      viewStart: this.viewDate,
      viewEnd: this.isPTPeriod
        ? moment(this.viewDate)
            .add(1, 'M')
            .subtract(1, 'd')
            .toDate()
        : moment(this.viewDate)
            .add(1, 'M')
            .toDate()
    });
    // making a copy to that we can modify according to our means
    this.viewRender = this.view;
    this.headerNextMonth = moment(this.view.days[this.view.days.length - 1].date).format('MMMM');
    this.viewRender.rowOffSetsWithWeekly = [];
    this.viewRender.totalDaysVisibleInWeek = this.viewRender.totalDaysVisibleInWeek + 1;
  }

  addColumnAfterWeekEndOrAtTheBeginning(view) {
    let count = 1;
    // Manipulating some data to adjust to the weekstartson. Weekstartson can be any day and the data will be adjusted accordngly.
    if (this.isWeeklyHoursRequired) {
      view.rowOffsets.forEach((item, index) => {
        if (item === 0) {
          return;
        } else {
          view.days.splice(item, 0, null);
          if (index !== view.rowOffsets.length - 1) view.rowOffsets[index + 1] = view.rowOffsets[index + 1] + count;
          if (view.days.length - item === this.viewRender.totalDaysVisibleInWeek) view.days.splice(view.days.length, 0, null);
          count++;
        }
      });
    } else {
      view.rowOffsets.forEach((item, index) => {
        view.days.splice(item, 0, null);
        if (index !== view.rowOffsets.length - 1) view.rowOffsets[index + 1] = view.rowOffsets[index + 1] + count;
        if (view.days.length - item === this.viewRender.totalDaysVisibleInWeek) view.days.splice(view.days.length, 0, null);
        count++;
      });
    }

    this.viewRender.days = view.days;
    this.calculateRowOffSet(this.viewRender);
  }

  calculateRowOffSet(view) {
    const rows: number = Math.floor(view.days.length / view.totalDaysVisibleInWeek);
    for (let i = 0; i < rows; i++) {
      view.rowOffSetsWithWeekly.push(i * view.totalDaysVisibleInWeek);
    }
  }

  getEvents() {
    this.prepareEvents();
    this.getViewToRender();
    this.addColumnAfterWeekEndOrAtTheBeginning(this.view);
    this.calculateWeeklyHours();
  }

  nextClicked() {
    this.viewDate = moment(this.viewDate)
      .add(1, 'M')
      .toDate();
    this.headerMonth = moment(this.viewDate).format('MMMM');
    this.initData();
    this.nextMonthClicked.emit(this.viewDate);
  }

  previousClicked() {
    this.viewDate = moment(this.viewDate)
      .subtract(1, 'M')
      .toDate();
    this.headerMonth = moment(this.viewDate).format('MMMM');
    this.initData();
    this.previousMonthClicked.emit(this.viewDate);
  }

  calculateWeeklyHours() {
    if (!this.extraColumnWithButtons) {
      let hours = [];
      this.viewRender.days.forEach((d, i) => {
        if (!d) {
          d = new WeeklyColumn();
          d.hours = _.sum(hours);
          d.isPast = false;
          d.isFuture = false;
          d.isWeekend = true;
          d.isToday = false;
          this.viewRender.days[i] = d;
          hours = [];
          return;
        }
        if (d.events) {
          d.events.forEach(e => {
            hours.push(+e.hours);
          });
        }
      });
    } else {
      this.viewRender.days.forEach((d, i) => {
        if (!d) {
          const totalWeek = this.viewRender.days.slice(i, i + 8);
          const allEventsForaWeek = [];
          d = new WeeklyColumn();
          this.viewRender.days[i] = d;
          totalWeek.forEach(day => {
            if (day && day.events) {
              allEventsForaWeek.push(...day.events);
            }
          });
          //TODO: Re-think this whole logic or simply calculate everything in the component and pass in a flag to add buttons.
          //  we do not add the buttons on the left column if any of the events have participatedHours already saved or if the present week(sun-sat) is not completed
          if (
            this.canEdit &&
            this.isPTPeriod &&
            !this.isWeeklyHoursRequired &&
            allEventsForaWeek.length > 0 &&
            // Here in the some call back the second condition is only checked if the first condition(e.participatedHours === null || e.participatedHours === undefined) is true
            allEventsForaWeek.every(
              e =>
                (e.participatedHours === null || e.participatedHours === undefined) &&
                moment(
                  moment(new Date(e.participationDate))
                    .endOf('week')
                    .format('MM/DD/YYYY')
                ).isSameOrBefore(moment(Utilities.currentDate)) &&
                e.canEditBasedOnOrg
            )
          ) {
            d.buttons = ['Full Participation', 'No Participation'];
            d.weekNumber = this.view.rowOffsets.indexOf(i);
            d.totalWeek = totalWeek;
            d.allEventsForaWeek = allEventsForaWeek;
            return;
          } else if (
            this.canEdit &&
            !this.isPTPeriod &&
            !this.isWeeklyHoursRequired &&
            allEventsForaWeek.length > 0 &&
            // Here in the some call back the second condition is only checked if the first condition(e.didParticipate === null || e.didParticipate === undefined) is true
            allEventsForaWeek.every(
              e =>
                (e.didParticipate === null || e.didParticipate === undefined) &&
                moment(
                  moment(new Date(e.participationDate))
                    .endOf('week')
                    .format('MM/DD/YYYY')
                ).isSameOrBefore(moment(Utilities.currentDate))
            )
          ) {
            d.buttons = ['Full Participation', 'No Participation'];
            d.weekNumber = this.view.rowOffsets.indexOf(i);
            d.totalWeek = totalWeek;
            d.allEventsForaWeek = allEventsForaWeek;
            return;
          }
        }
      });
    }
  }

  protected refreshHeader(): void {
    this.columnHeaders = this.utils.getWeekViewHeader({
      viewDate: this.viewDate,
      weekStartsOn: this.weekStartsOn
    });
    const d = new headerEntry();
    if (this.isWeeklyHoursRequired) {
      d.title = 'Weekly Hours';
      d.isPast = false;
      d.isFuture = false;
      d.isWeekend = true;
      d.isToday = false;
      this.columnHeaders.push(d);
    } else {
      this.columnHeaders = [d, ...this.columnHeaders];
    }
  }

  close() {
    this.closeCalendar.emit(false);
  }

  clickedActivity(e) {
    this.activityClicked.emit(e);
  }

  clickedBtn(e) {
    this.buttonClicked.emit(e);
  }

  ngOnDestroy() {
    this.viewDate = moment().toDate();
  }
}
