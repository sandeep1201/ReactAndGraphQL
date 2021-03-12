// tslint:disable: no-output-on-prefix
import { Subscription } from 'rxjs';
import { Component, OnInit, Input, Output, EventEmitter, OnDestroy } from '@angular/core';

import { AppService } from 'src/app/core/services/app.service';
import { DropDownField } from '../../../../shared/models/dropdown-field';
import { InformalAssessmentService } from '../../../../shared/services/informal-assessment.service';
import { ParticipantGuard } from '../../../../shared/guards/participant-guard';

@Component({
  selector: 'app-overview-card-header',
  templateUrl: './card-header.component.html',
  styleUrls: ['./card-header.component.css']
})
export class CardHeaderComponent implements OnInit, OnDestroy {
  @Output()
  onHistory = new EventEmitter<boolean>();
  @Output()
  onHistorySelected = new EventEmitter<Number>();
  @Output()
  onCollapse = new EventEmitter<boolean>();
  @Output()
  onEdit = new EventEmitter<boolean>();
  @Input()
  rowVersion;
  @Input()
  title = '';

  @Input()
  isEditHidden = false;
  @Input() showHistoryIcon = true;
  @Input()
  historyDrop: DropDownField[];
  @Input()
  historyArrayLength;

  public hasPBAccessBol: boolean;
  public canRequestPBAccess: boolean;
  public hasFBAccessBol: boolean;
  public canRequestFBAccess: boolean;
  public requestedElevatedAccess: boolean;

  historyIndex = 0;
  isHistoryActive = false;
  isCollapsed = false;
  private pbSub: Subscription;
  private fbSub: Subscription;

  public get isHistoryLoading(): boolean {
    return this.isHistoryActive && this.historyDrop == null;
  }

  constructor(private participantGuard: ParticipantGuard, private iaService: InformalAssessmentService, private appService: AppService) {}

  ngOnInit() {
    this.pbSub = this.appService.PBSection.subscribe(res => {
      this.hasPBAccessBol = res.hasPBAccessBol;
      this.canRequestPBAccess = res.canRequestPBAccess;
      this.requestedElevatedAccess = res.requestedElevatedAccess;
    });
    this.fbSub = this.appService.FBSection.subscribe(res => {
      this.hasFBAccessBol = res.hasFBAccessBol;
      this.canRequestFBAccess = res.canRequestFBAccess;
    });
  }

  toggleHistory() {
    this.isHistoryActive = !this.isHistoryActive;

    if (this.isHistoryActive === false) {
      // If we are disabling history, reset the dropdowns
      this.historyDrop = null;
      this.historyIndex = 0;
    }

    this.onHistory.emit(this.isHistoryActive);
  }

  forward() {
    if (this.historyIndex > 0) {
      this.historyIndex--;
    } else {
      this.historyIndex = this.historyArrayLength - 1;
    }

    this.onHistorySelected.emit(this.historyIndex);
  }

  back() {
    if (this.historyIndex < this.historyArrayLength - 1) {
      this.historyIndex++;
    } else {
      this.historyIndex = 0;
    }
    this.onHistorySelected.emit(this.historyIndex);
  }

  loadHistorySection() {
    if (this.historyIndex != null) {
      this.onHistorySelected.emit(this.historyIndex);
    } else {
      this.historyIndex = 0;
      this.onHistorySelected.emit(this.historyIndex);
    }
  }

  toggleCollapse() {
    if (this.title === 'Participant Barriers' && (!this.hasPBAccessBol || this.canRequestPBAccess)) {
      this.participantGuard.showModel().subscribe(res => {
        this.isCollapsed = false;
        this.appService.PBSection.next({ hasPBAccessBol: true, canRequestPBAccess: false }); // since already requested.
        this.appService.FBSection.next({ hasFBAccessBol: true, canRequestFBAccess: false });
        this.onCollapse.emit(this.isCollapsed);
      });
    } else if (this.title === 'Family Barriers' && (!this.hasFBAccessBol || this.canRequestFBAccess)) {
      this.participantGuard.showModelsForPHI().subscribe(res => {
        this.isCollapsed = false;
        this.appService.FBSection.next({ hasFBAccessBol: true, canRequestFBAccess: false }); // since already requested.
        this.appService.PBSection.next({ hasPBAccessBol: true, canRequestPBAccess: false });
        this.onCollapse.emit(this.isCollapsed);
      });
    } else {
      this.isCollapsed = !this.isCollapsed;
      this.onCollapse.emit(this.isCollapsed);
    }
  }

  isEditAssessmentEnabled() {}

  clickEdit() {
    this.onEdit.emit(true);
  }
  ngOnDestroy() {
    if (this.pbSub != null) {
      this.pbSub.unsubscribe();
    }
    if (this.fbSub != null) {
      this.fbSub.unsubscribe();
    }
  }
}
