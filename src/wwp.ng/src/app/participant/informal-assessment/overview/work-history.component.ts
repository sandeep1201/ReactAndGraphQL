// tslint:disable: import-blacklist
// tslint:disable: no-shadowed-variable
import { Component, OnInit, Input, OnDestroy, OnChanges } from '@angular/core';
import { BaseOverviewSecton } from '../overview/base-overview';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AppService } from 'src/app/core/services/app.service';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { WorkHistorySection } from '../../../shared/models/work-history-section';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { InformalAssessmentService } from '../../../shared/services/informal-assessment.service';
import { ModalService } from 'src/app/core/modal/modal.service';
import { ViewChild } from '@angular/core';
import { WorkHistoryEmbedComponent } from './../../../features-modules/work-history/embed/embed.component';

@Component({
  selector: 'app-work-history-overview',
  templateUrl: 'work-history.component.html',
  styleUrls: ['./overview.css']
})
export class WorkHistoryOverviewComponent extends BaseOverviewSecton implements OnInit, OnDestroy, OnChanges {
  @Input()
  section: WorkHistorySection;
  @Input()
  validationManager: ValidationManager;
  @Input()
  hasFcdpRole = false;
  @ViewChild(WorkHistoryEmbedComponent, { read: true, static: false })
  workHistoryEmbedComponent;

  public workHistoryEmploymentStatusesDrop: DropDownField[];
  private routeSub: Subscription;
  public showCareerFeature = false;

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
    this.showCareerFeature = this.appService.getFeatureToggleDate('CareerAndJobReadiness');
    this.fDataService.getWorkHistoryEmploymentStatuses().subscribe(result => {
      this.initWorkHistoryEmploymentStatuses(result);
    });
  }

  initWorkHistoryEmploymentStatuses(data) {
    this.workHistoryEmploymentStatusesDrop = data;
  }

  ngOnDestroy() {
    if (this.routeSub != null) {
      this.routeSub.unsubscribe();
    }
  }

  ngOnChanges() {
    if (this.section != null) {
      this.cachedSection = new WorkHistorySection();
      WorkHistorySection.clone(this.section, this.cachedSection);
    }
  }

  toggleHistory($event) {
    super.toggleHistory($event, 'work-history', this.section, cs => {
      this.section = cs;
    });
    this.iaService.isWorkHistoryHistoryActive = this.isHistoryActive;
  }

  loadHistorySection($event) {
    if (this.isHistoryActive === true) {
      this.section = this.historyManager.getHistoryAtIndex($event);
    }
  }

  onEdit($event) {
    const route = `/pin/${this.pin}/assessment/edit/work-history`;
    this.router.navigateByUrl(route);
  }

  public isEditAssessmentEnabled(): boolean {
    return this.section != null;
  }
}
