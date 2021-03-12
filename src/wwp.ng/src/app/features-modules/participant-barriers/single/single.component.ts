import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { FieldDataService } from '../../../shared/services/field-data.service';
import { ParticipantService } from '../../../shared/services/participant.service';
import { ParticipantBarrierAppService } from '../../../shared/services/participant-barrier-app.service';
import { BaseParticipantComponent } from '../../../shared/components/base-participant-component';
import { ParticipantBarrier } from '../../../shared/models/participant-barriers-app';
import { AppHistoryManager } from '../../../shared/models/app-history-manager';

@Component({
  selector: 'app-participant-barriers-single',
  templateUrl: './single.component.html',
  styleUrls: ['./single.component.css'],
  providers: [ParticipantBarrierAppService, FieldDataService]
})
export class ParticipantBarriersSingleComponent extends BaseParticipantComponent implements OnInit {
  @Input()
  hasEditAccess = false;

  public appHistoryManager: AppHistoryManager;
  public isCollapsed = false;
  private brSub: Subscription;
  public model: ParticipantBarrier;
  private routeIdSub: Subscription;
  private barrierId: number;
  public goBackUrl: string;

  constructor(
    private singleRoute: ActivatedRoute,
    private participantBarrierAppService: ParticipantBarrierAppService,
    private fdService: FieldDataService,
    router: Router,
    partService: ParticipantService
  ) {
    super(singleRoute, router, partService);
  }

  ngOnInit() {
    super.onInit();

    this.routeIdSub = this.singleRoute.params.subscribe(params => {
      if (this.pin == null || this.pin.trim() === '') {
        console.warn('PIN on ParticipantBarriersSingleComponent is null or empty');
      }

      this.barrierId = params['id'];
      this.goBackUrl = '/pin/' + this.pin + '/participant-barriers';

      this.participantBarrierAppService.setPin(this.pin);
      this.brSub = this.participantBarrierAppService.getParticipantBarrier(this.barrierId).subscribe(data => this.initModel(data));
    });
  }

  initModel(data) {
    this.model = data;
  }

  goBackToList() {
    this.router.navigateByUrl(`/pin/${this.pin}/participant-barriers`);
  }

  toggledHistory($event) {
    this.toggleHistory($event, 'participant-barriers-app', this.model, this.barrierId, cs => {
      this.model = cs;
    });
  }

  loadHistorySection($event) {
    if (this.isHistoryActive === true) {
      this.model = this.appHistoryManager.getHistoryAtIndex($event);
    }
  }
}
