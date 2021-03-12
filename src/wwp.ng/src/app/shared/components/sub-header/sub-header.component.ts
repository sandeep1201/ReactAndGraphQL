// tslint:disable: no-output-on-prefix
import { Component, Input, Output, OnInit, EventEmitter } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { take } from 'rxjs/operators';

import { AppService } from 'src/app/core/services/app.service';
import { Participant } from '../../models/participant';

@Component({
  selector: 'app-sub-header',
  templateUrl: './sub-header.component.html',
  styleUrls: ['./sub-header.component.css']
})
export class SubHeaderComponent implements OnInit {
  @Input() participant: Participant;

  @Input() pageTitle: string;

  @Input() goBackUrl: string;

  @Input() hasUnsavedChanges: boolean;

  @Input() helpComponent: string;

  @Input() isLoading = false;
  @Input() isElapsedActivity;
  @Input() pin;
  @Input() employabilityPlan;
  @Input() isSectionModified = false;
  @Output() exitToEpView = new EventEmitter();
  @Output() onExitEditMode = new EventEmitter<boolean>();
  exitCheck = false;

  featureName: string;
  pinForHistoricalComponent: string;
  constructor(private router: Router, private route: ActivatedRoute, public appService: AppService) {}

  ngOnInit() {
    switch (this.helpComponent) {
      case 'tl-overview': {
        this.featureName = 'timelimit-overview';
        break;
      }
      case 'clearance': {
        this.featureName = 'clearance-help';
        break;
      }
      case 'webi': {
        this.featureName = 'webi-help';
        break;
      }
      case 'participant-summary': {
        this.featureName = 'participant-summary-help';
        break;
      }
      case 'rfa-list': {
        this.featureName = 'rfa-list-help';
        break;
      }
      case 'rfa-single': {
        this.featureName = 'rfa-single-help';
        break;
      }
      case 'ia-summary': {
        this.featureName = 'ia-summary-help';
        break;
      }
      case 'work-history-app-list': {
        this.featureName = 'work-history-app-list-help';
        break;
      }
      case 'work-history-app-single': {
        this.featureName = 'work-history-app-single-help';
        break;
      }
      case 'participant-barriers-list': {
        this.featureName = 'participant-barriers-list-help';
        break;
      }
      case 'participant-barriers-single': {
        this.featureName = 'participant-barriers-single-help';
        break;
      }
      case 'contacts-list': {
        this.featureName = 'contacts-list-help';
        break;
      }
      case 'contacts-single': {
        this.featureName = 'contacts-single-help';
        break;
      }
      case 'action-needed-list': {
        this.featureName = 'action-needed-list-help';
        break;
      }
      case 'test-scores-card': {
        this.featureName = 'test-scores-card-help';
        break;
      }
      default: {
        if (this.helpComponent && this.helpComponent.trim() !== '') {
          this.featureName = this.helpComponent;
        }
        break;
      }
    }
    this.route.params.subscribe(params => {
      this.pinForHistoricalComponent = params.pin;
    });
  }
  openHelp() {
    if (this.helpComponent != null) {
      window.open('/help?pg=' + this.helpComponent);
    }
  }
  goBack() {
    if (this.isSectionModified && ['Job Readiness', 'Drug Screening', 'TJ/TMJ Employment Verification'].includes(this.pageTitle)) {
      this.appService.isDialogPresent = true;
    } else if (this.isElapsedActivity) {
      this.exitToEpView.emit();
    } else {
      this.appService.setDialogueFromDriverFlow = false;
      this.appService.componentDataModified.pipe(take(1)).subscribe(res => {
        this.appService.isDialogPresent = res.dataModified;
        if (!this.appService.isDialogPresent) {
          this.router.navigateByUrl(this.goBackUrl);
        }
      });
    }
  }

  showHistoricalGoals() {
    const url = `/pin/${this.pinForHistoricalComponent}/employability-plan/historical-goals`;
    this.appService.participantInfo.next(this.participant);
    this.router.navigateByUrl(url);
  }

  showHistoricalActivities() {
    const url = `/pin/${this.pinForHistoricalComponent}/employability-plan/historical-activities`;
    this.appService.participantInfo.next(this.participant);
    this.router.navigateByUrl(url);
  }

  exitIgnoreChanges($event) {
    this.onExitEditMode.emit();
    this.router.navigateByUrl(this.goBackUrl);
  }
}
