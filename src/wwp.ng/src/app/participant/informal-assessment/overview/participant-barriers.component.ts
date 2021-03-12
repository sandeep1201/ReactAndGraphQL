import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AppService } from 'src/app/core/services/app.service';
import { BaseOverviewSecton } from '../overview/base-overview';
import { ParticipantBarriersSection } from '../../../shared/models/participant-barriers-section';
import { ParticipantBarrier } from '../../../shared/models/participant-barriers-app';
import { ParticipantBarrierAppService } from '../../../shared/services/participant-barrier-app.service';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { InformalAssessmentService } from '../../../shared/services/informal-assessment.service';
import { ModalService } from 'src/app/core/modal/modal.service';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { Utilities } from '../../../shared/utilities';

@Component({
  selector: 'app-participant-barriers-overview',
  templateUrl: 'participant-barriers.component.html',
  styleUrls: ['./overview.css'],
  providers: [ParticipantBarrierAppService]
})
export class ParticipantBarriersOverviewComponent extends BaseOverviewSecton implements OnInit, OnDestroy {
  @Input()
  section: ParticipantBarriersSection;
  @Input()
  validationManager: ValidationManager;
  @Input()
  pin: string;
  @Input()
  hasPBAccess: boolean;
  @Input()
  hasFcdpRole = false;
  public canRequestPBAccess: boolean;

  private routeSub: Subscription;
  private pbListSub: Subscription;
  private pbSub: Subscription;
  public allParticipantBarriers: ParticipantBarrier[];
  public isDVCollapsed = false;
  public showHistoryIcon = true;

  public yesId = 0;

  constructor(
    public appService: AppService,
    private fdService: FieldDataService,
    private participantBarrierAppService: ParticipantBarrierAppService,
    public iaService: InformalAssessmentService,
    public modalService: ModalService,
    private router: Router
  ) {
    super(appService, iaService, modalService);
  }

  ngOnInit() {
    this.getYesNoRefusedDrop();
    this.getParticipantBarrierList();
    this.pbSub = this.appService.PBSection.subscribe(res => {
      this.canRequestPBAccess = res.canRequestPBAccess;
      if (res.hasPBAccessBol && !this.canRequestPBAccess) {
        this.showHistoryIcon = true;
      } else {
        this.showHistoryIcon = false;
      }
    });
  }
  getParticipantBarrierList(): void {
    this.participantBarrierAppService.setPin(this.pin);
    this.pbListSub = this.participantBarrierAppService.getParticipantBarriers().subscribe(participantBarriers => {
      if (participantBarriers != null) {
        this.initParticipantBarriers(participantBarriers);
      }
    });
  }
  initParticipantBarriers(data: ParticipantBarrier[]): void {
    this.allParticipantBarriers = data;
  }
  toggleDVCollapse() {
    this.isDVCollapsed = !this.isDVCollapsed;
  }

  ngOnDestroy() {
    if (this.routeSub != null) {
      this.routeSub.unsubscribe();
    }
    if (this.pbSub != null) {
      this.pbSub.unsubscribe();
    }
  }

  onEdit($event) {
    const route = `/pin/${this.pin}/assessment/edit/participant-barriers`;
    this.router.navigateByUrl(route);
  }

  public isEditAssessmentEnabled(): boolean {
    return this.section != null;
  }

  getYesNoRefusedDrop(): void {
    this.pbListSub = this.fdService.getPolarRefused().subscribe(data => this.initYesNoRefusedDrop(data));
  }

  initYesNoRefusedDrop(data: DropDownField[]): void {
    this.yesId = Utilities.idByFieldDataName('Yes', data);
  }

  toggleHistory($event) {
    super.toggleHistory($event, 'participant-barriers', this.section, cs => {
      this.section = cs;
    });
    this.iaService.isParticipantBarriersHistoryActive = this.isHistoryActive;
  }

  loadHistorySection($event) {
    if (this.isHistoryActive === true) {
      this.section = this.historyManager.getHistoryAtIndex($event);
    }
  }
}
