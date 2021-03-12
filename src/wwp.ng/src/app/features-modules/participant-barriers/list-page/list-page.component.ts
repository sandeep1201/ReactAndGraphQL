import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { AppService } from './../../../core/services/app.service';
import { BaseParticipantComponent } from '../../../shared/components/base-participant-component';
import { ParticipantService } from '../../../shared/services/participant.service';

@Component({
  selector: 'app-participant-barriers-list-page',
  templateUrl: './list-page.component.html',
  styleUrls: ['./list-page.component.css']
})
export class ParticipantBarriersListPageComponent extends BaseParticipantComponent implements OnInit, OnDestroy {
  public isLoaded = false;
  public goBackUrl: string;
  public pin: string;
  public isReadOnly = true;

  constructor(route: ActivatedRoute, router: Router, private appService: AppService, partService: ParticipantService) {
    super(route, router, partService);
  }

  ngOnInit() {
    super.onInit();
  }

  ngOnDestroy() {
    super.onDestroy();
  }

  onPinInit() {
    this.goBackUrl = '/pin/' + this.pin;
  }

  onParticipantInit() {
    // TODO: Add some Rxjs goodness to make sure both observables are complete (base class and here)
    // before setting isLoaded.
    if (this.appService.isParticipantEditable(this.participant) && this.appService.user.roles !== 'FCDP Query Only Access') {
      this.isReadOnly = false;
    } else {
      this.isReadOnly = true;
    }
    this.isLoaded = true;
  }
}
