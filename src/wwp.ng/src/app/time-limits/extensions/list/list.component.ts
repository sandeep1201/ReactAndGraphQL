import { Authorization } from '../../../shared/models/authorization';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { EnumEx, Utilities } from '../../../shared/utilities';
import * as arrayUtils from '../../../shared/arrays';
import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { ClockTypeNamePipe } from '../../pipes/clock-type-name.pipe';

import { Observable, Subscription, forkJoin, empty } from 'rxjs';
import { finalize, take } from 'rxjs/operators';
import { ClockType, ClockTypes, Extension, ExtensionDecision, ExtensionSequence, Timeline, ClockStates, ClockState, ExtensionReason } from '../../../shared/models/time-limits';
import { TimeLimitsService } from '../../../shared/services/timelimits.service';
import { IMultiSelectOption } from '../../../shared/components/multi-select-dropdown/multi-select-dropdown.component';
import { coerceNumberProperty } from '../../../shared/decorators/number-property';
import { deleteWarningType } from '../delete-warning/delete-warning.component';
import { Participant } from '../../../shared/models/participant';
import { AppService } from 'src/app/core/services/app.service';

type ExtensionSortColumn = 'DiscussionDate' | 'DecisionDate' | 'BeginDate' | 'EndDate';

// note: I am grouping these here because the ngFor template would call extensionsSequence.CurrentExtension over and over...
// which doen't need to be recalculated unless a change occurs the the sequence. Probably a small optimization
// since these lists are supposed to be small, but ya know...
interface ExtensionSequenceExtensionMap {
  extension: Extension;
  sequence: ExtensionSequence;
}

@Component({
  selector: 'app-extension-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css']
})
export class ExtensionListComponent implements OnInit {
  clockTypeNameFilter: ClockTypeNamePipe;
  deleteWarningType: deleteWarningType = 'end';

  // ENUMS for UI
  ClockTypes = ClockTypes;
  ExtensionDecision = ExtensionDecision;
  selectedEditExtension: Extension;
  selectedDeleteExtension: Extension;
  @Input() pin: string;
  @Input() participant: Participant;
  public isLoading = false;
  public isSaving = false;
  public hadSaveError = false;
  public errorMessage: string;
  public allExtensionSequences: ExtensionSequence[] = [];
  public extensionSequencesList: ExtensionSequenceExtensionMap[] = [];
  public timeline: Timeline;
  private extensionSub: Subscription;
  private _clockTypeFilter: ClockTypes;
  private approvalReasons: Map<number, ExtensionReason>;
  private denialReasons: Map<number, ExtensionReason>;
  // public clockTypesDrop: DropDownField[] = [];
  public clockTypesDropMulti: IMultiSelectOption[] = [];
  public sortColumn: ExtensionSortColumn = 'DecisionDate';
  public sortAscending: boolean = false;
  public showDeleted: boolean = false;
  public printEnabled: boolean = false;

  get clockTypeFilter(): ClockTypes {
    return this._clockTypeFilter || ClockTypes.None;
  }

  set clockTypeFilter(val) {
    if (val == null || val.toString() === '') {
      this._clockTypeFilter = ClockTypes.None;
    }
    this._clockTypeFilter = +val; // TODO validate this is an extensible type?
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
    if (this.timeline && this.participant) {
      const hasPermission = this.canEditBasedOnSecurity;
      const timelineState = this.timeline.timelineState;
      return hasPermission && timelineState.state.CanBeExtended;
    }
    return false;
  }

  get canEndExtensions() {
    return this.participant && this.appService.isUserAuthorizedToEditTimeLimits(this.participant);
  }

  public canDeleteExtension() {
    return this.participant && this.appService.isUserAuthorizedToDeleteTimeLimitsExtension(this.participant);
  }

  canAppendToSequence(extSequence: ExtensionSequence, ext: Extension = null) {
    if (!this.timeline || !this.participant) {
      return false;
    }
    let hasPermission = this.appService.isUserAuthorizedToEditTimeLimits(this.participant);
    //let sequenceIds = this.timeline.extensionSequences.filter(x=> x.clockType.valueOf() === extSequence.clockType.valueOf()).map(x=>x.sequenceId)
    let currentExtension = this.timeline.getCurrentExtension(extSequence.clockType.valueOf());
    let isCurrentExtensionDeny = false;
    if (currentExtension && currentExtension != null) isCurrentExtensionDeny = currentExtension.decision === ExtensionDecision.Deny;
    let isCurrentExtension = currentExtension && extSequence.currentExtension.id === this.timeline.getCurrentExtension(extSequence.clockType.valueOf()).id;

    let isLastestSequence = true; //Math.max(...sequenceIds) === extSequence.sequenceId;

    let clockState = new ClockState(this.timeline.clockStates.get(extSequence.clockType));
    return (
      hasPermission &&
      (clockState.state.SequenceCanBeDenied || clockState.state.CanBeExtended || clockState.state.WillCauseGapExtension) &&
      isLastestSequence &&
      isCurrentExtension &&
      !extSequence.currentExtension.hasStarted &&
      isCurrentExtensionDeny
    );
  }

  constructor(private timeLimitsService: TimeLimitsService, private appService: AppService, private router: Router) {
    this.clockTypeNameFilter = new ClockTypeNamePipe();
  }

  canEndExtension(extension: Extension) {
    return this.canEndExtensions && extension.hasStarted && !extension.hasElapsed && extension.dateRange && !extension.dateRange.end.isSame(Utilities.currentDate, 'month');
  }

  ngOnInit() {
    this.isLoading = true;
    forkJoin(this.timeLimitsService.getExtensionReasons('Approval').pipe(take(1)), this.timeLimitsService.getExtensionReasons('Denial').pipe(take(1)))
      .pipe(take(1))
      .subscribe(extReason => {
        this.approvalReasons = extReason[0];
        this.denialReasons = extReason[1];
        this.loadData();
      });
  }

  applySorting(sortColumn?: ExtensionSortColumn) {
    // if we don't pass in a new sort column, assume we need filter (by deleted or type)

    sortColumn = sortColumn || this.sortColumn; // so show deleted can call without knowing the type

    if (sortColumn === this.sortColumn) {
      // If you click on the same button, we will only switch the
      // sortAscending Flag and reload
      this.sortAscending = !this.sortAscending;
    } else {
      // otherwise just switch the type and revert the sortAcending to default
      this.sortAscending = false;
      this.sortColumn = sortColumn;
    }
    this.applySortAndFilterOnData();
  }

  private loadData() {
    this.extensionSub = this.timeLimitsService
      .getTimeline(this.pin)
      .pipe(take(1)) // unsubscribe once we get the timeline
      .pipe(finalize(() => (this.isLoading = false)))
      .subscribe(t => {
        this.timeline = t;
        this.allExtensionSequences = t.extensionSequences.filter(x => x.extensions && x.extensions.length > 0);
        this.initClockTypeDropDown();
        this.applySortAndFilterOnData();
      });
  }

  private initClockTypeDropDown() {
    // const dropValues: DropDownField[] = [];
    const availClockTypesMap = new Map<ClockTypes, ClockTypes>();
    if (this.allExtensionSequences.length > 0) {
      this.allExtensionSequences.map(x => {
        if (!availClockTypesMap.has(x.clockType.valueOf())) {
          availClockTypesMap.set(x.clockType.valueOf(), x.clockType.valueOf());
          // dropValues.push(new DropDownField(x.clockType.valueOf(), ClockTypes[x.clockType.valueOf()] === ClockTypes.W2T.toString() ? "W-2 T" : ClockTypes[x.clockType.valueOf()].toString() ));
        }
      });
    }
    // this.clockTypesDrop = dropValues;
    const clockTypeOptions = EnumEx.getNamesAndValues(ClockTypes);
    this.clockTypesDropMulti = [];
    for (let i = 0; i < clockTypeOptions.length; i++) {
      const x = clockTypeOptions[i];
      if (x.value === ClockTypes.TNP || x.value === ClockTypes.TMP) {
        continue;
      }
      if (x.value === ClockTypes.TEMP || (ClockType.IsSingleFlag(x.value) && ClockType.any(x.value, ClockTypes.ExtensableTypes))) {
        let name = this.clockTypeNameFilter.transform(x.value, false);
        const option = { id: x.value, name: name, isDisabled: !availClockTypesMap.has(x.value), disablesOthers: false };
        this.clockTypesDropMulti.push(option);
      }
    }

    // this.clockTypesDropMulti = dropValues.map(x=>   } );
  }

  filterByClockType(val: ClockTypes) {
    const clockType = val == null || val.toString() === '' ? ClockTypes.None : +val;
    this.clockTypeFilter = clockType;
    this.applySortAndFilterOnData();
  }

  toggleDeleted() {
    this.showDeleted = !this.showDeleted;
    this.applySortAndFilterOnData();
  }

  private applySortAndFilterOnData() {
    let data: ExtensionSequenceExtensionMap[] = this.allExtensionSequences.map(x => {
      const reason = this.getExtensionReason(x.currentExtension);
      return { extension: x.currentExtension, sequence: x, extensionReason: reason };
    });

    // Filter deleted if needed
    if (!this.showDeleted) {
      data = data.filter((x, i, arr) => {
        return !x.extension.isDeleted;
      });
    }

    // Filter by type if needed
    if (this.clockTypeFilter != null && this.clockTypeFilter !== ClockTypes.None) {
      data = data.filter(x => {
        return x.extension.clockType.any(this.clockTypeFilter);
      });
    }

    // .sort defaults to ascending
    if (this.sortColumn === 'DiscussionDate') {
      data = data.sort((x, y) => {
        if (!x.extension.discussionDate || !y.extension.discussionDate) {
          return 0;
        }

        return x.extension.discussionDate.diff(y.extension.discussionDate);
      });
    }

    if (this.sortColumn === 'DecisionDate') {
      data = data.sort((x, y) => {
        return x.extension.decisionDate.diff(y.extension.decisionDate);
      });
    }

    if (this.sortColumn === 'BeginDate') {
      data = data.sort((x, y) => {
        // Denials are "older" and won't have .start
        if (x.extension.decision === ExtensionDecision.Deny) {
          return y.extension.decision === ExtensionDecision.Deny ? 0 : -1;
        }

        if (y.extension.decision === ExtensionDecision.Deny) {
          return 1;
        }

        return x.extension.dateRange.start.diff(y.extension.dateRange.start);
      });
    }

    if (this.sortColumn === 'EndDate') {
      data = data.sort((x, y) => {
        // Denials are "older"
        if (x.extension.decision === ExtensionDecision.Deny) {
          return y.extension.decision === ExtensionDecision.Deny ? 0 : -1;
        }

        if (y.extension.decision === ExtensionDecision.Deny) {
          return 1;
        }

        return x.extension.dateRange.end.diff(y.extension.dateRange.end);
      });
    }

    // reverse if needed
    if (!this.sortAscending) {
      data.reverse();
    }
    this.extensionSequencesList = data;
  }
  showExtensionForm(extensionSequence?: ExtensionSequence) {
    const newEx = new Extension();
    // const extensionSequence = arrayUtils.FirstOrDefault(this.allExtensionSequences, ((x: ExtensionSequence) => { return x.clockType === clockType &&  x.sequenceId === sequenceId; })) || new ExtensionSequence();
    const currentEx = extensionSequence ? extensionSequence.currentExtension : null;

    if (currentEx) {
      newEx.sequenceId = currentEx.sequenceId;
      // newEx.dateRange = currentEx.dateRange.clone();
      newEx.clockType = new ClockType(currentEx.clockType.valueOf());
      // newEx.decision = currentEx.decision ? ExtensionDecision.Deny : ExtensionDecision.Approve;
      // TODO - LOCK FORM to correct decision/timelimit type
    }
    this.selectedEditExtension = newEx;
  }

  public extensionAdded(val: ExtensionSequenceExtensionMap) {
    this.isLoading = true;
    this.selectedEditExtension = null;
    this.loadData();
  }

  navigateToExtension(extensionId?: number) {
    if (extensionId) {
      this.router.navigateByUrl('/pin/' + this.pin + '/time-limits/extensions/' + extensionId);
    }
  }

  private getExtensionReason(extension: Extension) {
    let extReason = { id: 0, name: '', validFor: extension.clockType.valueOf() };
    if (extension) {
      if (extension.decision === ExtensionDecision.Approve && this.approvalReasons.has(extension.approvalReasonId)) {
        extReason = this.approvalReasons.get(extension.approvalReasonId);
      } else if (this.denialReasons.has(extension.denialReasonId)) {
        extReason = this.denialReasons.get(extension.denialReasonId);
      }
    }
    return extReason;
  }
  public clockTypesArray: ClockTypes[] = [];
  public setClockTypeFilter(clockTypes: any[]) {
    this.clockTypesArray = [];
    this.clockTypeFilter = ClockTypes.None;
    if (clockTypes && clockTypes.length) {
      clockTypes.map(x => {
        const clockType = new ClockType(coerceNumberProperty(x));
        if (this.clockTypesArray.indexOf(clockType.valueOf()) < 0) {
          this.clockTypesArray.push(clockType.valueOf());
        }
        this.clockTypeFilter |= clockType.valueOf();
      });
    }
    this.applySortAndFilterOnData();
  }

  public printExtensionForm(ext) {}

  showExtensionDeleteWarning(ext: Extension, deleteType: 'end' | 'delete') {
    this.selectedDeleteExtension = ext;
    this.deleteWarningType = deleteType;
  }

  public onExtensionSaved(ext?: Extension) {
    this.cancelEditExtension();
    if (ext) {
      this.isLoading = true;
      // reload all the data
      this.loadData();
    }
  }

  private cancelDelete() {
    this.selectedDeleteExtension = null;
    this.isSaving = false;
    this.hadSaveError = false;
    this.isLoading = false;
  }

  cancelEditExtension() {
    this.selectedEditExtension = null;
    this.isSaving = false;
    this.hadSaveError = false;
    this.isLoading = false;
  }

  private handleSaveError(error: any, obs: Observable<ExtensionSequence>) {
    this.hadSaveError = true;
    this.isLoading = false;
    let errMsg = error && error.message ? error.message : '';
    this.errorMessage = 'There was an error saving your changes, please try again. ' + errMsg;
    return empty();
  }
}
