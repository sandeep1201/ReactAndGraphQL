import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { BaseOverviewSecton } from '../overview/base-overview';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { FamilyBarriersSection } from '../../../shared/models/family-barriers-section';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { AppService } from 'src/app/core/services/app.service';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { InformalAssessmentService } from '../../../shared/services/informal-assessment.service';
import { ModalService } from 'src/app/core/modal/modal.service';
import { Authorization } from '../../../shared/models/authorization';

@Component({
  selector: 'app-family-barriers-overview',
  templateUrl: 'family-barriers.component.html',
  styleUrls: ['./overview.css']
})
export class FamilyBarriersOverviewComponent extends BaseOverviewSecton implements OnInit, OnDestroy {
  @Input() section: FamilyBarriersSection;
  @Input() validationManager: ValidationManager;
  @Input() hasFcdpRole = false;

  public ssiApplicationStatuses: DropDownField[];

  private routeSub: Subscription;
  private ssiAppSub: Subscription;
  private fbSub: Subscription;
  public hasFbEditAccess: boolean;
  public canRequestFBAccess: boolean;
  public showHistoryIcon: boolean;

  get canAccessFamilyBarriersSsi(): boolean {
    return this.appService.isUserAuthorized(Authorization.canAccessFamilyBarriersSsi, null);
  }

  constructor(
    public appService: AppService,
    public iaService: InformalAssessmentService,
    private route: ActivatedRoute,
    public modalService: ModalService,
    private fdService: FieldDataService,
    private router: Router
  ) {
    super(appService, iaService, modalService);
  }

  ngOnInit() {
    this.routeSub = this.route.params.subscribe(params => {
      this.pin = params['pin'];
    });
    this.ssiAppSub = this.fdService.getSsiApplicationStatuses().subscribe(data => this.initSsiApplicationStatuses(data));
    this.fbSub = this.appService.FBSection.subscribe(res => {
      this.canRequestFBAccess = res.canRequestFBAccess;
      if (res.hasFBAccessBol && !this.canRequestFBAccess) {
        this.showHistoryIcon = true;
      } else {
        this.showHistoryIcon = false;
      }
    });
  }

  initSsiApplicationStatuses(data) {
    this.ssiApplicationStatuses = data;
  }

  ngOnDestroy() {
    if (this.routeSub != null) {
      this.routeSub.unsubscribe();
    }
    if (this.ssiAppSub != null) {
      this.ssiAppSub.unsubscribe();
    }
  }

  onEdit($event) {
    const route = `/pin/${this.pin}/assessment/edit/family-barriers`;
    this.router.navigateByUrl(route);
  }

  public isEditAssessmentEnabled(): boolean {
    return this.section != null;
  }

  toggleHistory($event) {
    super.toggleHistory($event, 'family-barriers', this.section, cs => {
      this.section = cs;
    });
    this.iaService.isFamilyBarriersHistoryActive = this.isHistoryActive;
  }

  loadHistorySection($event) {
    if (this.isHistoryActive === true) {
      this.section = this.historyManager.getHistoryAtIndex($event);
    }
  }
}
