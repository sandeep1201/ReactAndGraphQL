import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit, OnDestroy } from '@angular/core';

import { AppService } from './../../../core/services/app.service';
import { BaseParticipantComponent } from '../../../shared/components/base-participant-component';
import { EnrolledProgramStatus } from '../../../shared/enums/enrolled-program-status.enum';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { ParticipantService } from '../../../shared/services/participant.service';
import { Authorization } from '../../../shared/models/authorization';

@Component({
  selector: 'app-work-history-list-page',
  templateUrl: './list-page.component.html',
  styleUrls: ['./list-page.component.css'],
  providers: [FieldDataService]
})
export class WorkHistoryListPageComponent extends BaseParticipantComponent implements OnInit, OnDestroy {
  public isLoaded = false;
  public goBackUrl: string;
  public pin: string;
  public isReadOnly = true;
  public isHD = false;
  public hasEditAuth = false;

  constructor(route: ActivatedRoute, router: Router, private appService: AppService, partService: ParticipantService) {
    super(route, router, partService);
  }

  ngOnInit() {
    super.onInit();
    this.isHD = this.appService.user.roles.indexOf('W-2 Help Desk') >= 0;
  }

  ngOnDestroy() {
    super.onDestroy();
  }

  onPinInit() {
    this.goBackUrl = '/pin/' + this.pin;
  }

  public onParticipantInit() {
    if (this.participant) {
      this.hasEditAuth = false;
      this.hasEditAuth = this.appService.isUserAuthorized(Authorization.canAccessWorkHistoryApp_Edit, this.participant);
      this.isReadOnly = this.appService.checkReadOnlyAccess(this.participant, [EnrolledProgramStatus.enrolled, EnrolledProgramStatus.disenrolled], this.hasEditAuth) && !this.isHD;
    }

    this.isLoaded = true;
  }
}
