import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseParticipantComponent } from 'src/app/shared/components/base-participant-component';
import { AppService } from 'src/app/core/services/app.service';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { AccessType } from 'src/app/shared/enums/access-type.enum';

@Component({
  selector: 'app-contacts-list-page',
  templateUrl: './list-page.component.html',
  styleUrls: ['./list-page.component.css']
})
export class ContactsListPageComponent extends BaseParticipantComponent implements OnInit, OnDestroy {
  public isLoaded = false;
  public goBackUrl: string;
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
    // TODO: Add some Rxjs goodness to make sure both observables are complete (base class and here)
    if (this.appService.coreAccessContext.evaluate() === AccessType.edit && this.appService.isParticipantEditable(this.participant)) {
      this.isReadOnly = false;
    } else {
      this.isReadOnly = true;
    }
    this.isLoaded = true;
  }
}
