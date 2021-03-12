import { ClockState } from '../../shared/models/time-limits/clockstates';
import { coerceNumber } from '../../shared/decorators/number-property';
import { ExtensionSequence } from '../../shared/models/time-limits/extension';
import { Component, OnInit, OnDestroy, AfterViewInit, ViewChild, EventEmitter, ChangeDetectionStrategy, Input, Output } from '@angular/core';
import { Location } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';

import { Subscription, Observable, forkJoin } from 'rxjs';
import { catchError, take } from 'rxjs/operators';
import * as moment from 'moment';

import { AssistanceGroup, AssistanceGroupMember, ClockTypes, ClockType, Timeline, TimelineMonth, Extension, ClockStates } from '../../shared/models/time-limits';
import { LogService } from '../../shared/services/log.service';
import { ModalService } from 'src/app/core/modal/modal.service';
import { Participant } from '../../shared/models/participant';
import { ParticipantService } from '../../shared/services/participant.service';
import { TimeLimitsService } from '../../shared/services/timelimits.service';
import { TimeLimitOverviewHelpComponent } from '../../help/help-pages/tl-overview.component';

@Component({
  selector: 'app-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.css'],
  changeDetection: ChangeDetectionStrategy.Default,
  providers: [Location]
})
export class SummaryComponent implements OnInit, AfterViewInit, OnDestroy {
  ClockTypes = ClockTypes;
  ClockStates = ClockStates;
  timeline: Timeline;
  private partSub: Subscription;
  participant: Participant;
  assistanceGroup: AssistanceGroup;
  @coerceNumber() pin: number;
  goBackUrl: string;
  selectedClockType: ClockTypes;
  selectedSummaryMonths: TimelineMonth[] = [];
  selectedMonth: TimelineMonth;
  selectedExtension: Extension;
  public isLoading = false;
  public hadLoadError = false;
  public loadError: string;

  public helpComponentType = TimeLimitOverviewHelpComponent;
  get lastBatchRun(): moment.Moment {
    if (this.timeline) {
      return this.timeline.lastBatchRun;
    }
  }

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private timeLimitsService: TimeLimitsService,
    private participantService: ParticipantService,
    private location: Location,
    private modalService: ModalService,
    private logService: LogService
  ) {}

  // TODO: move logic into LoadData function(s)
  ngOnInit() {
    this.isLoading = true;

    // Measure performance.
    this.logService.timerStart('tl-summary-load');

    this.partSub = this.route.params.subscribe(params => {
      this.pin = params['pin'];
      this.goBackUrl = '/pin/' + this.pin;
      this.isLoading = true;
      this.timeLimitsService
        .getParticpantAssitanceGroup(this.pin.toString())
        .pipe(take(1)) // unsubscribe once we get the assistanceGroup
        .subscribe(ag => {
          if (!ag) {
            ag = new AssistanceGroup();
          }

          // use participant if no primary returned
          if (!ag.parents || !ag.parents.length) {
            forkJoin(
              this.timeLimitsService.getTimeline(this.pin.toString()).pipe(take(1)), // unsubscribe once we get the timeline,
              this.participantService.getParticipant(this.pin.toString(), true, true).pipe(take(1))
            )
              .pipe(take(1))
              .subscribe(x => {
                this.initParticipant(x[1]);
                this.timeline = x[0];
                this.isLoading = false;
                let agm = new AssistanceGroupMember();

                agm.isSelectable = true;
                agm.participant = this.participant;
                agm.timeline = this.timeline;
                agm.pin = +this.participant.pin;
                ag.parents.push(agm);
                this.logService.timerEndEvent('tl-summary-load');
              });
          } else {
            //get participant & timeline for each member in the group
            ag.parents.map(assistanceGroupMember => {
              // mark currently selected as self
              // if (assistanceGroupMember.pin === this.pin) {
              //   assistanceGroupMember.relationship = 'Self';
              // }
              this.participantService
                .getParticipant(assistanceGroupMember.pin.toString(), true, true)
                .pipe(take(1)) // unsubscribe once we get the participant
                .subscribe(part => {
                  if (assistanceGroupMember.pin === this.pin) {
                    this.initParticipant(part);
                  }
                  assistanceGroupMember.participant = part;
                });
              this.timeLimitsService
                .getTimeline(assistanceGroupMember.pin.toString())
                .pipe(take(1)) // unsubscribe once we get the timeline
                .subscribe(timeline => {
                  if (assistanceGroupMember.pin === this.pin) {
                    this.timeline = timeline;
                    this.isLoading = false;
                    this.logService.timerEndEvent('tl-summary-load');
                  }
                  assistanceGroupMember.timeline = timeline;
                });
            });
          }

          this.assistanceGroup = ag;
        }),
        catchError(this.handleLoadError);
    });
  }

  ngAfterViewInit() {}

  private initParticipant(part: Participant, reloadAGRelastionships: boolean = false) {
    this.participant = part;
    if (reloadAGRelastionships) {
      this.assistanceGroup.parents.map(x => (x.relationship = '...'));
      this.timeLimitsService
        .getParticpantAssitanceGroup(this.pin.toString())
        .pipe(take(1)) // unsubscribe once we get the assistanceGroup
        .pipe(catchError(this.handleLoadError))
        .subscribe(ag => {
          ag.parents.map(agm => {
            this.assistanceGroup.parents.filter(x => x.pin === agm.pin).map(y => (y.relationship = agm.relationship));
          });
        });
    }
  }

  ngOnDestroy() {
    if (this.partSub && !this.partSub.closed) {
      this.partSub.unsubscribe();
    }
  }

  onMonthSelected(val: TimelineMonth) {
    this.selectedMonth = val;
  }
  onMonthSaved(months: TimelineMonth[]) {
    if (months) {
      months.map(month => {
        this.timeline.setTimelineMonth(month);
      });
    }
    // Trigger re-binding so all sub components refresh
    this.timeline = this.timeline.clone();
    this.timeline.clearCachedData();

    let selectedMonthCopy = this.selectedMonth.clone();
    //  async - refresh entire timeline as soon as possible
    this.timeLimitsService
      .getTimeline(this.pin.toString())
      .pipe(take(1))
      .pipe(catchError(this.handleLoadError))
      .subscribe(x => {
        this.isLoading = false;
        this.timeline = x;
        this.selectedMonth = this.timeline.getTimelineMonth(selectedMonthCopy.date);
        this.assistanceGroup.parents.map(x => {
          if (x.pin == this.pin) {
            x.timeline = this.timeline;
          }
        });
        this.assistanceGroup = this.assistanceGroup.clone();
      });
  }

  onEditExtension(val: Extension) {
    this.selectedExtension = val || new Extension();
    this.logService.event('tl-extension-edit');
  }

  closeExtensionEditing(extensionSequence?: ExtensionSequence) {
    if (extensionSequence) {
      this.isLoading = true;
      this.hadLoadError = false;
      this.loadError = null;

      this.timeLimitsService
        .getTimeline(this.pin.toString())
        .pipe(take(1))
        .pipe(catchError(this.handleLoadError))
        .subscribe(x => {
          this.isLoading = false;
          this.timeline = x;
          this.assistanceGroup.parents.map(x => {
            if (x.pin == this.pin) {
              x.timeline = this.timeline;
            }
          });
          this.assistanceGroup = this.assistanceGroup.clone();
        });
    }
  }

  openSummaryDetails(clockType: ClockTypes) {
    this.logService.event('tl-summary-details');

    if (this.timeline) {
      this.selectedClockType = clockType;
      let includeNoPlacementLimit = ClockType.any(clockType, ClockTypes.State) || ClockType.any(clockType, ClockTypes.Federal) || ClockType.any(clockType, ClockTypes.OTHER);

      this.selectedSummaryMonths = this.timeline.getTicks(clockType, includeNoPlacementLimit);
    }

    // // TODO: replace this module once the PR for passing data gets merged
    // // https://github.com/angular/material2/pull/2266
    // let dialogRef = this.dialog.open(ClockSummaryDetailsComponent, this.modalConfig);
    // dialogRef.componentInstance.clockType = clocktype;
    // dialogRef.componentInstance.timeline = this.timeline;
    // if(this.dialogRef){
    //   this.dialogRef.instance.destroy();
    // }

    // this.modalService.create<ClockSummaryDetailsComponent>(ClockSummaryDetailsComponent, {clocktype, timeline: this.timeline}).subscribe((x)=>{
    //   this.dialogRef = x;
    // });
  }
  closeSummaryDetails() {
    this.selectedClockType = null;
  }
  navigateToTimelimitsPage(pin: number | string) {
    // Find them in the AG
    const otherParent = this.assistanceGroup.parents.find(x => x.pin === pin);
    if (!otherParent) {
      this.router.navigateByUrl(`/pin/${pin}/time-limits`);
    } else {
      this.location.go(`/pin/${otherParent.pin}/time-limits`);
      this.pin = otherParent.pin;
      this.selectedClockType = null;
      this.selectedExtension = null;
      this.selectedMonth = null;
      this.selectedSummaryMonths = [];
      this.participant = null;
      if (this.timeline) {
        this.timeline.clearCachedData();
      }
      this.initParticipant(otherParent.participant, true);
      this.timeline = otherParent.timeline;
    }
  }

  handleLoadError<T>(err, obs: Observable<T>): Observable<T> {
    this.isLoading = false;
    this.hadLoadError = true;
    if (err && err.message) {
      this.loadError = err.message;
    }
    this.logService.timerEndEvent('tl-summary-load');
    this.logService.error(err.message);
    return Observable.throw(err);
  }
}
