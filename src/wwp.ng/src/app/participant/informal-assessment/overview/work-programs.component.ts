import { Component, OnInit, Input, OnChanges, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { BaseOverviewSecton } from '../overview/base-overview';
import { AppService } from 'src/app/core/services/app.service';
import { WorkProgramsSection, WorkProgram } from '../../../shared/models/work-programs-section';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { InformalAssessmentService } from '../../../shared/services/informal-assessment.service';
import { ModalService } from 'src/app/core/modal/modal.service';
import * as _ from 'lodash';

@Component({
  selector: 'app-work-programs-overview',
  templateUrl: 'work-programs.component.html',
  styleUrls: ['./overview.css']
})
export class WorkProgramsOverviewComponent extends BaseOverviewSecton implements OnInit, OnChanges, OnDestroy {
  @Input()
  section: WorkProgramsSection;
  @Input()
  validationManager: ValidationManager;
  @Input()
  hasFcdpRole = false;

  private routeSub: Subscription;

  private history: any;

  constructor(
    public appService: AppService,
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
    this.section.workPrograms = this.section.defaultSort(this.section.workPrograms);
  }

  ngOnChanges() {
    if (this.section != null) {
      this.cachedSection = new WorkProgramsSection();
      WorkProgramsSection.clone(this.section, this.cachedSection);
    }
  }

  ngOnDestroy() {
    if (this.routeSub != null) {
      this.routeSub.unsubscribe();
    }
  }

  toggleHistory($event) {
    super.toggleHistory($event, 'work-programs', this.section, cs => {
      this.section = cs;
    });
    this.iaService.isWorkProgramsHistoryActive = this.isHistoryActive;
  }

  loadHistorySection($event) {
    if (this.isHistoryActive === true) {
      this.section = this.historyManager.getHistoryAtIndex($event);
    }
  }

  onEdit($event) {
    const route = `/pin/${this.pin}/assessment/edit/work-programs`;
    this.router.navigateByUrl(route);
  }

  public isEditAssessmentEnabled(): boolean {
    return this.section != null;
  }
}
