// tslint:disable: deprecation
import { ClockType } from '../../../shared/models/time-limits/clocktypes';
import { deleteWarningType } from '../delete-warning/delete-warning.component';
import { AppService } from 'src/app/core/services/app.service';
import { coerceNumberProperty } from '../../../shared/decorators/number-property';
import { Extension, ExtensionDecision, ExtensionSequence, Timeline, ExtensionReason, ClockTypes, ClockState } from '../../../shared/models/time-limits';
import { Observable, Subscription, forkJoin, empty } from 'rxjs';
import { Component, OnInit, OnDestroy, ChangeDetectionStrategy, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Location } from '@angular/common';

import { ParticipantService } from '../../../shared/services/participant.service';
import { BaseParticipantComponent } from '../../../shared/components/base-participant-component';
import { TimeLimitsService } from '../../../shared/services/timelimits.service';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-extension-detail',
  templateUrl: './detail.component.html',
  styleUrls: ['./detail.component.css'],
  providers: [Location],
  changeDetection: ChangeDetectionStrategy.Default
})
export class ExtensionDetailComponent extends BaseParticipantComponent implements OnInit, OnDestroy {
  ExtensionDecision = ExtensionDecision;
  ClockTypes = ClockTypes;
  @Input()
  extensionSequence: ExtensionSequence;
  @Input()
  timeline: Timeline;
  @Input()
  extId: number;

  approvalReasons: Map<number, ExtensionReason>;
  denialReasons: Map<number, ExtensionReason>;
  public printEnabled = false;

  allExtensions: Extension[] = [];
  get extensions(): Extension[] {
    if (this.showDeleted) {
      return this.allExtensions;
    } else {
      return this.allExtensions.filter(x => !x.isDeleted);
    }
  }

  public selectedExtension: Extension;
  public selectedDeleteExtension: Extension;
  public selectedEditExtension: Extension;
  public deleteWarningType: deleteWarningType = 'end';
  public isLoading = false;
  public isSaving = false;
  public hadSaveError = false;
  public showDeleted = true;
  public errorMessage: string;
  public goBackUrl: string;
  private routeParamSub: Subscription;

  constructor(
    private timeLimitsService: TimeLimitsService,
    private appService: AppService,
    private location: Location,
    route: ActivatedRoute,
    router: Router,
    partService: ParticipantService
  ) {
    super(route, router, partService);
    this.usePEPAgency = true;
  }

  onPinInit() {
    this.goBackUrl = '/pin/' + this.pin + '/time-limits/extensions';
  }

  onParticipantInit() {
    // TODO: Validate participant loaded
  }

  ngOnInit() {
    super.onInit();
    forkJoin(
      this.timeLimitsService.getTimeline(this.pin).pipe(take(1)),
      this.timeLimitsService.getExtensionReasons('Approval').pipe(take(1)),
      this.timeLimitsService.getExtensionReasons('Denial').pipe(take(1))
    )
      .pipe(take(1))
      .subscribe(results => {
        this.timeline = results[0];
        this.approvalReasons = results[1];
        this.denialReasons = results[2];

        this.routeParamSub = this.route.params.subscribe(params => {
          this.extId = coerceNumberProperty(params['id']);
          this.initExtensionSequence();
        });
      });
  }

  toggleDeleted() {
    this.showDeleted = !this.showDeleted;
  }

  public initExtensionSequence() {
    const extensionSequence = this.timeline.extensionSequences.find(x => {
      return (
        x.extensions.find(y => {
          return y.id === this.extId;
        }) != null
      );
    });
    if (this.extensionSequence && this.extensionSequence.extensions.length === 0) {
      this.router.navigateByUrl(this.goBackUrl);
      return;
    }

    this.extensionSequence = extensionSequence || new ExtensionSequence();
    this.allExtensions = extensionSequence.extensions.slice(0);
    this.allExtensions.sort((a, b) => {
      return b.decisionDate.diff(a.decisionDate);
    });
    this.selectExtension(this.extId);
  }

  ngOnDestroy() {
    super.onDestroy();
    if (this.routeParamSub) {
      this.routeParamSub.unsubscribe();
    }
  }

  public selectExtension(extId: number) {
    this.extId = extId;
    //if it is already selected, ignore it
    if (this.selectedExtension && this.selectedExtension.id === extId) {
      return;
    } else if (this.extensions) {
      this.selectedExtension = this.allExtensions.find(x => x.id === extId);
      if (this.selectedExtension.isDeleted) {
        this.showDeleted = true;
      }
      this.location.go('/pin/' + this.pin + '/time-limits/extensions/' + this.extId);
    }
  }

  public canDeleteSelectedDecision() {
    if (this.selectedExtension && !this.selectedExtension.isDeleted) {
      return this.participant && this.appService.isUserAuthorizedToDeleteTimeLimitsExtension(this.participant);
    }

    return false;
  }

  public canAppendToSequence() {
    if (this.extensionSequence) {
      const hasPermission = this.appService.isUserAuthorizedToEditTimeLimits(this.participant);
      //let sequenceIds = this.timeline.extensionSequences.filter(x=> x.clockType.valueOf() === extSequence.clockType.valueOf()).map(x=>x.sequenceId)
      const currentExtension = this.timeline.getCurrentExtension(this.extensionSequence.clockType.valueOf());
      let isCurrentExtensionDeny = false;
      if (currentExtension && currentExtension != null) isCurrentExtensionDeny = currentExtension.decision === ExtensionDecision.Deny;
      const sequenceExtensionDecision = this.extensionSequence.currentExtension;
      const isCurrentExtension = currentExtension && sequenceExtensionDecision.id === this.timeline.getCurrentExtension(this.extensionSequence.clockType.valueOf()).id;

      const isLastestSequence = true; //Math.max(...sequenceIds) === extSequence.sequenceId;

      const clockState = new ClockState(this.timeline.clockStates.get(this.extensionSequence.clockType));
      return (
        hasPermission &&
        (clockState.state.SequenceCanBeDenied || clockState.state.CanBeExtended || clockState.state.WillCauseGapExtension) &&
        isLastestSequence &&
        isCurrentExtension &&
        !sequenceExtensionDecision.hasStarted &&
        isCurrentExtensionDeny
      );
    }
  }
  public getExtReason(extension: Extension): ExtensionReason {
    let extReason = new ExtensionReason();
    extReason.id = 0;
    extReason.name = '';
    extReason.validFor = extension.clockType.valueOf();
    if (extension) {
      if (extension.decision === ExtensionDecision.Approve && this.approvalReasons.has(extension.approvalReasonId)) {
        extReason = this.approvalReasons.get(extension.approvalReasonId);
      } else if (this.denialReasons.has(extension.denialReasonId)) {
        extReason = this.denialReasons.get(extension.denialReasonId);
      }
    }
    return extReason;
  }

  showExtensionDeleteWarning(deleteType: 'end' | 'delete') {
    this.selectedDeleteExtension = this.selectedExtension;
    this.deleteWarningType = deleteType;
  }

  public showExtensionForm() {
    this.selectedEditExtension = new Extension();
    this.selectedEditExtension.sequenceId = this.extensionSequence.sequenceId;
    this.selectedEditExtension.clockType = new ClockType(this.extensionSequence.clockType.valueOf());
  }

  public onExtensionSaved(ext?: Extension) {
    if (ext) {
      this.isLoading = true;
      this.selectedDeleteExtension = null;
      this.timeLimitsService
        .getTimeline(this.pin)
        .pipe(take(1))
        .subscribe(x => {
          this.timeline = x;
          this.initExtensionSequence();
        });
      // reload all the data
    } else {
      this.cancelDelete();
    }
  }
  private cancelDelete() {
    this.selectedDeleteExtension = null;
    this.isSaving = false;
    this.hadSaveError = false;
    this.isLoading = false;
  }
  public cancelEditExtension() {
    this.selectedEditExtension = null;
    this.isSaving = false;
    this.hadSaveError = false;
    this.isLoading = false;
  }

  public extensionAdded(val: any) {
    this.isLoading = true;
    this.selectedEditExtension = null;
    this.initExtensionSequence();
  }

  private handleSaveError(error: any, obs: Observable<ExtensionSequence>) {
    this.hadSaveError = true;
    this.isLoading = false;
    const errMsg = error && error.message ? error.message : '';
    this.errorMessage = 'There was an error saving your changes, please try again. ' + errMsg;
    return empty();
  }
}
