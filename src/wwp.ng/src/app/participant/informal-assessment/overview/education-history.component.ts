import { Component, OnInit, Input, OnChanges, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { AppService } from 'src/app/core/services/app.service';
import { BaseOverviewSecton } from '../overview/base-overview';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { EducationHistorySection } from '../../../shared/models/education-history-section';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { InformalAssessmentService } from '../../../shared/services/informal-assessment.service';
import { ModalService } from 'src/app/core/modal/modal.service';

@Component({
  selector: 'app-education-history-overview',
  templateUrl: 'education-history.component.html',
  styleUrls: ['./overview.css']
})
export class EducationHistoryOverviewComponent extends BaseOverviewSecton implements OnInit, OnChanges, OnDestroy {
  @Input()
  section: EducationHistorySection;
  @Input()
  validationManager: ValidationManager;
  @Input()
  hasFcdpRole = false;

  public educationDiplomaTypesDrop: DropDownField[];

  private routeSub: Subscription;

  private eSubDrop: Subscription;

  constructor(
    public appService: AppService,
    public fDataService: FieldDataService,
    public iaService: InformalAssessmentService,
    private route: ActivatedRoute,
    public modalService: ModalService,
    private router: Router
  ) {
    super(appService, iaService, modalService);
  }

  ngOnInit() {
    this.routeSub = this.route.params.subscribe(params => {
      this.pin = params['pin'];
    });
    this.eSubDrop = this.fDataService.getEducationDiplomaTypes().subscribe(data => this.initEducationDiplomaTypes(data));
  }

  initEducationDiplomaTypes(data) {
    this.educationDiplomaTypesDrop = data;
  }

  ngOnChanges() {
    if (this.section != null) {
      this.cachedSection = new EducationHistorySection();
      EducationHistorySection.clone(this.section, this.cachedSection);
    }
  }

  ngOnDestroy() {
    if (this.routeSub != null) {
      this.routeSub.unsubscribe();
    }
    if (this.eSubDrop != null) {
      this.eSubDrop.unsubscribe();
    }
  }

  onEdit($event) {
    const route = `/pin/${this.pin}/assessment/edit/education`;
    this.router.navigateByUrl(route);
  }

  public isEditAssessmentEnabled(): boolean {
    return this.section != null;
  }

  toggleHistory($event) {
    super.toggleHistory($event, 'education', this.section, cs => {
      this.section = cs;
    });
    this.iaService.isEducationHistoryActive = this.isHistoryActive;
  }

  loadHistorySection($event) {
    if (this.isHistoryActive === true) {
      this.section = this.historyManager.getHistoryAtIndex($event);
    }
  }
}
