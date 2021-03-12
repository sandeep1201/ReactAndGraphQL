import { Utilities } from './../../shared/utilities';
import { TimeLimitsService } from '../../shared/services/timelimits.service';
import { Authorization } from '../../shared/models/authorization';
import { Dictionary } from '../../shared/dictionary';
import { AppService } from './../../core/services/app.service';
import { Router } from '@angular/router';
import {
  Component,
  AfterViewInit,
  OnInit,
  Input,
  Output,
  EventEmitter,
  ChangeDetectionStrategy,
  ViewChildren,
  ViewChild,
  QueryList,
  ElementRef,
  Renderer,
  ComponentRef,
  ChangeDetectorRef,
  NgZone
} from '@angular/core';
import { Timeline, TimelineMonth, ClockTypes, Extension, ClockState, ClockStates } from '../../shared/models/time-limits';
import { Participant } from '../../shared/models/participant';
import { PositionService } from '../../shared/position';
import { Tween, Easing } from '../../shared/tween';

import { MonthBoxComponent } from './month-box/month-box.component';

import * as moment from 'moment';

import { DateRange, range, rangeInterval } from '../../shared/moment-range';
import { Subject, Observable, Observer } from 'rxjs';

@Component({
  selector: 'app-limits-timeline',
  templateUrl: './limits-timeline.component.html',
  styleUrls: ['./limits-timeline.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TimelimitsTimelineComponent implements OnInit {
  clockTypes = ClockTypes;
  _timeline: Timeline;

  @ViewChild('timelineScroll', { static: false }) scrollWrapper: ElementRef<HTMLDivElement>;
  @ViewChild('timelineScrollContainer', { static: false }) scrollContainer: ElementRef<HTMLDivElement>;

  @ViewChildren(MonthBoxComponent) monthBoxComponents: QueryList<MonthBoxComponent>;

  @Input() get timeline(): Timeline {
    return this._timeline || new Timeline(this.appService);
  }

  @Input() participant: Participant;
  @Input() pin: string;

  @Output() monthSelected = new EventEmitter<TimelineMonth>();

  @Output() addExtension = new EventEmitter();

  @Output() editExtension = new EventEmitter<Extension>();

  compactMode: 'ALL' | 'UNUSED' | 'NONE' = 'NONE';

  selectedIndex: number;

  todayIndex: number;
  isLoaded: boolean;

  get selectedMonth() {
    return this.months[this.selectedIndex];
  }

  set timeline(val) {
    this._timeline = val;
    if (this.isLoaded) {
      this.initTimelineMonths();
    }
  }

  get canEditBasedOnSecurity(): boolean {
    let canEdit = false;

    // W-2 HelpDesk.
    if (this.appService.isUserAuthorized(Authorization.timeLimitsEditAll)) {
      canEdit = true;
    } else if (this.appService.isUserAuthorized(Authorization.timeLimitsEditInAgency) && this.participant.isParticipantServedByAgency(this.appService.user.agencyCode)) {
      // QC and case management can only edit own agency pins.
      canEdit = true;
    } else if (this.appService.isUserAuthorized(Authorization.timeLimitsEdit) && this.participant.isParticipantServedByWorker(this.appService.user.wiuid)) {
      // FEP can only edit own pin.
      canEdit = true;
    }
    return canEdit;
  }
  get canAddExtension() {
    if (this.timeline) {
      const hasPermission = this.canEditBasedOnSecurity;
      const timelineState = this.timeline.timelineState;
      return hasPermission && timelineState.state.CanBeExtended;
    }
    return false;
  }

  months: TimelineMonth[] = [];

  constructor(private renderer: Renderer, private router: Router, private appService: AppService, private changeDetectorRef: ChangeDetectorRef, private zone: NgZone) {}

  ngOnInit() {
    this.initTimelineMonths();
    this.isLoaded = true;
  }

  initTimelineMonths() {
    this.months = [];
    let w2StartDate = TimeLimitsService.w2StartDate;
    let dob = this.participant && this.participant.dateOfBirth ? moment(this.participant.dateOfBirth) : moment('1990-01-01');
    let eighteenthBirthday = dob.clone().add(18, 'years');

    // Participant is not 18. We aren't gonna show any months
    if (eighteenthBirthday.isSameOrAfter(Utilities.currentDate)) {
      return;
    }

    let start = dob.isValid() ? moment.max(eighteenthBirthday, w2StartDate) : w2StartDate;
    let timelineMonths = this.timeline ? this.timeline.timelineMonths : new Dictionary<string, TimelineMonth>();
    let firstTickedMonth = moment.min(
      ...timelineMonths
        .values()
        .filter(x => {
          return !x.tick.clockTypes.eql(ClockTypes.None);
        })
        .map(x => {
          return moment(x.date);
        })
    );
    if (firstTickedMonth) {
      start = moment.min(firstTickedMonth, start);
    }
    //    let displayRange = range(start, moment(Utilities.currentDate).add(14, 'months'));
    let displayRange = range(start, moment(Utilities.currentDate).add(14, 'months'));

    let months = Array.from(displayRange.by('month'));

    for (let i = 0; i < months.length; i++) {
      let date = months[i];
      let key = Timeline.getKey(date);
      let tMonth = timelineMonths.getValue(key);
      if (tMonth == null) {
        tMonth = new TimelineMonth(date);

        // push into timeline so we bind any relevant data
        this.timeline.setTimelineMonth(tMonth, false);
      }

      // TODO: get/set Range and selectedMonth out of localStorage

      if (this.selectedIndex == null && tMonth.date.isSame(Utilities.currentDate, 'month')) {
        this.todayIndex = this.months.length - 1;
      }
      if (tMonth.tick.tickType == ClockTypes.CMC && !tMonth.tick.clockTypes.state.State && !tMonth.tick.clockTypes.state.Federal) tMonth = new TimelineMonth(date);
      this.months.push(tMonth);
    }
    this.timeline.clearCachedData();
  }

  selectMonth(index: number) {
    if (index > this.months.length - 1) {
      index = this.months.length - 1;
      //TODO: Load more months?
    }

    if (index < 0) {
      index = 0;
    }

    let animate = this.selectedIndex !== index;
    this.selectedIndex = index;

    this.zone.runOutsideAngular(() => {
      this.scrollToSelectedItem(animate);
    });
    this.monthSelected.emit(this.months[index]);
    // this.changeDetectorRef.detectChanges();
    //this.changeDetectorRef.markForCheck();
  }
  onExtensionClick(item: TimelineMonth, clockType: ClockTypes) {
    let ext = item.extensions.find(c => c.clockType && c.clockType.eql(clockType));
    this.editExtension.emit(ext);
  }

  ngAfterViewInit() {
    // TODO: Scroll if months get added?
    // bind to changes to months[]
    //this.selectedIndex = this.todayIndex;
    //this.selectMonth(this.todayIndex);
    if (this.isLoaded == true) {
      this.scrollTimeLineToItem(this.monthBoxComponents.last, false);
    }
  }

  toggleCompactMode(mode: 'ALL' | 'UNUSED') {
    // handle toggle off
    if (this.compactMode === mode) {
      this.compactMode = 'NONE';
      return;
    }

    // Otherwise switch to new mode
    this.compactMode = mode;
  }

  AddDenyExtension() {
    this.editExtension.emit();
  }

  goToExtensionsPage() {
    this.router.navigateByUrl('/pin/' + this.pin + '/time-limits/extensions').catch(this.handleError);
  }

  stepTimelineBackwards() {
    if (!this.selectedIndex) {
      return;
      // TODO: Load range from server and append
    }

    const monthIndexToSelect = this.selectedIndex - 12 || 0;
    this.selectMonth(monthIndexToSelect);
  }

  stepTimelineLeft() {
    if (!this.selectedIndex) {
      return;
      // TODO: Load range from server and append
    }

    const monthIndexToSelect = this.selectedIndex - 1 || 0;
    this.selectMonth(monthIndexToSelect);
  }
  stepTimelineCurrent() {
    if (!this.todayIndex) {
      return;
      // TODO: Load range from server and append
    }
    this.selectMonth(this.todayIndex || 0);
  }

  stepTimelineRight() {
    if (!this.selectedIndex) {
      return;
      // TODO: Load range from server and append
    }

    const monthIndexToSelect = this.selectedIndex + 1;
    this.selectMonth(monthIndexToSelect);
  }
  stepTimelineForward() {
    if (!this.selectedIndex) {
      return;
      // TODO: Load range from server and append
    }

    const monthIndexToSelect = this.selectedIndex + 12;
    this.selectMonth(monthIndexToSelect);
  }

  private handleError(error: any) {
    console.log(error);
    throw error;
  }

  private previousAnimation: Tween;

  animationSpeed: 1000;
  private scrollTimeLineToItem(monthboxComp: MonthBoxComponent, animate: boolean) {
    if (monthboxComp && monthboxComp.elementRef && monthboxComp.elementRef.nativeElement) {
      let scrollToOffset = monthboxComp.elementRef.nativeElement.offsetLeft; //note, jquery position() is relative to the current scrolled amount, not the absolute position regardles of scrolling that we want
      scrollToOffset -= this.scrollContainer.nativeElement.offsetWidth / 2 || 0; // substract 1/2 the container width
      scrollToOffset -= monthboxComp.elementRef.nativeElement.offsetWidth / 2 || 0; // subtract 1/2 the box width to center
      let scrollEnd = scrollToOffset || 0;

      scrollEnd = scrollEnd > 0 ? scrollEnd : 0;

      if (!animate) {
        this.renderer.setElementProperty(this.scrollContainer.nativeElement, 'scrollLeft', scrollEnd);
        return;
      }

      //tween randoObject, but use renderer in Update Callback
      let tween = new Tween(this.scrollContainer.nativeElement).to({ scrollLeft: scrollEnd }, this.animationSpeed).easing(Easing.Quadratic.Out);

      if (this.previousAnimation) {
        this.previousAnimation.stop();
      }
      this.previousAnimation = tween;
      tween.start(); // So animateTweens will update this animation

      this.animateTweens();
    }
  }

  private scrollToSelectedItem(animate: boolean) {
    let component = this.monthBoxComponents.find(x => {
      return x.index === this.selectedIndex;
    });
    if (!component) {
      console.warn(`scroll requested for item I could not find at index: ${this.selectedIndex}`);
      return;
    }
    this.scrollTimeLineToItem(component, animate);
  }

  private animationId: number;
  private animateTweens = () => {
    this.animationId = requestAnimationFrame(this.animateTweens);
    //update all Tweens

    if (!Tween.update()) {
      //cancel next one cuz we didn't do anything.
      cancelAnimationFrame(this.animationId);
    }
  };
}
