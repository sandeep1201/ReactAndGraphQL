import { Component, OnInit, Input, OnChanges, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { BaseOverviewSecton } from '../overview/base-overview';
import { AppService } from 'src/app/core/services/app.service';
import { TransportationSection } from '../../../shared/models/transportation-section';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { InformalAssessmentService } from '../../../shared/services/informal-assessment.service';
import { ModalService } from 'src/app/core/modal/modal.service';

@Component({
  selector: 'app-transportation-overview',
  templateUrl: 'transportation.component.html',
  styleUrls: ['./overview.css']
})
export class TransportationOverviewComponent extends BaseOverviewSecton implements OnInit, OnChanges, OnDestroy {
  @Input()
  section: TransportationSection;
  @Input()
  validationManager: ValidationManager;
  @Input()
  hasFcdpRole = false;

  private routeSub: Subscription;

  // todo: we can construct an object from the model and reuse the method isVehicleInfoDisplayed(will do in the next version)
  public isPrivate: boolean;

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
    this.isPrivate = this.displayVehicleinfo(this.section.transportationMethods);
    this.routeSub = this.route.params.subscribe(params => {
      this.pin = params['pin'];
    });
  }

  displayVehicleinfo(arr) {
    if (arr.length) {
      return arr.indexOf('Personal Vehicle') !== -1 || arr.indexOf('Borrowed Vehicle') !== -1;
    }
  }

  ngOnChanges() {
    if (this.section != null) {
      this.cachedSection = new TransportationSection();
      TransportationSection.clone(this.section, this.cachedSection);
    }
  }

  ngOnDestroy() {
    this.routeSub.unsubscribe();
  }

  onEdit($event) {
    const route = `/pin/${this.pin}/assessment/edit/transportation`;
    this.router.navigateByUrl(route);
  }

  public isEditAssessmentEnabled(): boolean {
    return this.section != null;
  }

  toggleHistory($event) {
    super.toggleHistory($event, 'transportation', this.section, cs => {
      this.section = cs;
    });
    this.iaService.isTransportationHistoryActive = this.isHistoryActive;
  }

  loadHistorySection($event) {
    if (this.isHistoryActive === true) {
      this.section = this.historyManager.getHistoryAtIndex($event);
    }
  }
}
