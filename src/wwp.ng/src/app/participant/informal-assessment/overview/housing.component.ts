import { Component, OnInit, Input, OnChanges, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { BaseOverviewSecton } from '../overview/base-overview';
import { AppService } from 'src/app/core/services/app.service';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { InformalAssessmentService } from '../../../shared/services/informal-assessment.service';
import { ModalService } from 'src/app/core/modal/modal.service';
import { HousingSection } from '../../../shared/models/housing-section';

@Component({
  selector: 'app-housing-overview',
  templateUrl: 'housing.component.html',
  styleUrls: ['./overview.css']
})
export class HousingOverviewComponent extends BaseOverviewSecton implements OnInit, OnChanges, OnDestroy {
  @Input()
  section: HousingSection;
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

  ngOnChanges() {
    if (this.section != null) {
      this.cachedSection = new HousingSection();
      HousingSection.clone(this.section, this.cachedSection);
    }
  }

  ngOnDestroy() {
    this.routeSub.unsubscribe();
  }

  onEdit($event) {
    const route = `/pin/${this.pin}/assessment/edit/housing`;
    this.router.navigateByUrl(route);
  }

  public isEditAssessmentEnabled(): boolean {
    return this.section != null;
  }

  toggleHistory($event) {
    super.toggleHistory($event, 'housing', this.section, cs => {
      this.section = cs;
    });
    this.iaService.isHousingHistoryActive = this.isHistoryActive;
  }

  loadHistorySection($event) {
    if (this.isHistoryActive === true) {
      this.section = this.historyManager.getHistoryAtIndex($event);
    }
  }
}
