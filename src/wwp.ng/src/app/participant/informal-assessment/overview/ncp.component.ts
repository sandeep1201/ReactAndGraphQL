import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AppService } from 'src/app/core/services/app.service';
import { BaseOverviewSecton } from '../overview/base-overview';
import { NonCustodialParentsSection } from '../../../shared/models/non-custodial-parents-section';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { InformalAssessmentService } from '../../../shared/services/informal-assessment.service';
import { ModalService } from 'src/app/core/modal/modal.service';

@Component({
  selector: 'app-ncp-overview',
  templateUrl: 'ncp.component.html',
  styleUrls: ['./overview.css']
})
export class NcpOverviewComponent extends BaseOverviewSecton implements OnInit, OnDestroy {
  @Input()
  section: NonCustodialParentsSection;
  @Input()
  validationManager: ValidationManager;
  @Input()
  hasFcdpRole = false;
  private routeSub: Subscription;

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
  }

  ngOnDestroy() {
    if (this.routeSub != null) {
      this.routeSub.unsubscribe();
    }
  }

  onEdit($event) {
    const route = `/pin/${this.pin}/assessment/edit/non-custodial-parents`;
    this.router.navigateByUrl(route);
  }

  public isEditAssessmentEnabled(): boolean {
    return this.section != null;
  }

  toggleHistory($event) {
    super.toggleHistory($event, 'non-custodial-parents', this.section, cs => {
      this.section = cs;
    });
    this.iaService.isNonCustodialParentsHistoryActive = this.isHistoryActive;
  }

  loadHistorySection($event) {
    if (this.isHistoryActive === true) {
      this.section = this.historyManager.getHistoryAtIndex($event);
    }
  }
}
