import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { ParticipantService } from '../../../shared/services/participant.service';
import { BaseParticipantComponent } from '../../../shared/components/base-participant-component'


@Component({
  selector: 'app-list-page',
  templateUrl: './list-page.component.html',
  styleUrls: ['./list-page.component.css']
})
export class ExtensionListPageComponent extends BaseParticipantComponent implements OnInit, OnDestroy {

  public goBackUrl: string;

  constructor(route: ActivatedRoute, router: Router, partService: ParticipantService) {
    super(route, router, partService);
    this.usePEPAgency = true;
  }

  onPinInit() {
    this.goBackUrl = '/pin/' + this.pin + '/time-limits';
  }

  ngOnInit() {
    super.onInit();
  }

  ngOnDestroy() {
    super.onDestroy();
  }

}
