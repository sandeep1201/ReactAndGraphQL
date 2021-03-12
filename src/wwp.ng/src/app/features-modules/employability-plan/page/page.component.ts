import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { BaseParticipantComponent } from 'src/app/shared/components/base-participant-component';
import { AppService } from 'src/app/core/services/app.service';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { AccessType } from 'src/app/shared/enums/access-type.enum';

@Component({
  selector: 'app-ep-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.scss']
})
export class EmployabilityPlanPageComponent extends BaseParticipantComponent implements OnInit, OnDestroy {
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

  public onParticipantInit() {
    // Per US2431, workers can edit EP records if:
    //   1. EP App shall updateable by workers that have the security role for an enrolled
    //      program.
    //   2. EP App shall be read only by workers that have the security role for a
    //      program that the latest instance of "enrolled" occurred.

    // First get the enrolled programs the user has access to.
    const enrolledProgs = this.participant.getCurrentEnrolledProgramsUserHasAccessTo(this.appService.user, this.appService);
    // Only has access to CF, LF and W2.
    let isReadOnly = true;
    if (enrolledProgs != null) {
      for (const p of enrolledProgs) {
        if (!p.isFCDP && this.appService.coreAccessContext.evaluate() === AccessType.edit) {
          isReadOnly = false;
          break;
        }
      }
    }

    // If we have any programs after the filtering, then the worker can edit.
    this.isReadOnly = isReadOnly;

    this.isLoaded = true;
  }
}
