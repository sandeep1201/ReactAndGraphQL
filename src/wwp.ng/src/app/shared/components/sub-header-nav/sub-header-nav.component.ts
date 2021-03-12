import { Component, Input, OnChanges } from '@angular/core';
import { Router } from '@angular/router';
import { Participant } from '../../models/participant';
import { InformalAssessment } from '../../models/informal-assessment';
import { AppService } from 'src/app/core/services/app.service';
import { InformalAssessmentService } from '../../services/informal-assessment.service';
import { Authorization } from '../../models/authorization';
import { EnrolledProgramStatus } from '../../enums/enrolled-program-status.enum';

@Component({
  selector: 'app-sub-header-nav',
  templateUrl: './sub-header-nav.component.html',
  styleUrls: ['./sub-header-nav.component.css']
})
export class SubHeaderNavComponent implements OnChanges {
  Auth = Authorization;
  iaNavUrl: string;
  eiaNavUrl: string;
  contactsNavUrl: string;
  workListUrl: string;
  timelimitsNavUrl: string;
  actionNeededNavUrl: string;
  participantBarriersUrl: string;
  testScoresNavUrl: string;
  overviewNavUrl: string;
  epEditUrl: string;
  isReadOnly = true;

  @Input() activeItem: string;
  @Input() mode: string;
  @Input() pin: string;
  @Input() participant: Participant;
  @Input() assessment: InformalAssessment;
  @Input() epId: string;
  @Input() canEditEP = true;
  public isSaving = false;

  constructor(public appService: AppService, private iaService: InformalAssessmentService, private router: Router) {
    // Default the mode to None
    this.mode = 'None';
    // Default the active item to None
    this.activeItem = 'None';
  }

  ngOnChanges() {
    this.iaNavUrl = `/pin/${this.pin}/assessment`;
    this.contactsNavUrl = `/pin/${this.pin}/contacts`;
    this.workListUrl = `/pin/${this.pin}/work-history`;
    this.participantBarriersUrl = `/pin/${this.pin}/participant-barriers`;
    this.testScoresNavUrl = `/pin/${this.pin}/test-scores`;
    this.timelimitsNavUrl = `/pin/${this.pin}/time-limits`;
    this.actionNeededNavUrl = `/pin/${this.pin}/action-needed`;
    this.eiaNavUrl = `/pin/${this.pin}/assessment/edit`;
    this.overviewNavUrl = `/pin/${this.pin}`;
    this.epEditUrl = `/pin/${this.pin}/employability-plan/${this.epId}`;
  }

  public isNewAssessmentEnabled(): boolean {
    if (this.participant) this.checkAccess();
    return this.assessment != null && (this.assessment.id === 0 || this.assessment.submitDate != null) && !this.isReadOnly;
  }

  public isEditAssessmentEnabled(): boolean {
    if (this.participant) this.checkAccess();
    return this.assessment != null && (this.assessment.id > 0 || this.assessment.submitDate != null) && !this.isReadOnly;
  }

  checkAccess() {
    if (this.participant) {
      let hasAuth = false;
      hasAuth = this.appService.isUserAuthorized(Authorization.canAccessInformalAssessment_Edit, this.participant);
      this.isReadOnly = this.appService.checkReadOnlyAccess(this.participant, [EnrolledProgramStatus.enrolled], hasAuth);
    }
  }
  public onNewAssessment(): void {
    this.isSaving = true;
    this.iaService.createNewAssessment(this.participant).subscribe(
      n => n,
      e => {
        this.isSaving = false;
      },
      () => {
        this.isSaving = false;
        this.router.navigateByUrl(this.eiaNavUrl);
      }
    );
  }
}
