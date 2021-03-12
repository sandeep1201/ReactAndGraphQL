import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from './../../../core/services/app.service';
import { BaseParticipantComponent } from '../../../shared/components/base-participant-component';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ParticipantService } from '../../../shared/services/participant.service';
import { AccessType } from '../../../shared/enums/access-type.enum';

@Component({
  selector: 'app-rfa-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.css']
})
export class RfaPageComponent extends BaseParticipantComponent implements OnInit, OnDestroy {
  public participant;
  public pin;
  public goBackUrl;

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

  getAccess(): AccessType {
    if (this.appService.coreAccessContext) return this.appService.coreAccessContext.evaluate();
    else return AccessType.none;
  }
}
