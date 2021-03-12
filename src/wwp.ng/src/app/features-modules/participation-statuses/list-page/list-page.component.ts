import { BaseParticipantComponent } from '../../../shared/components/base-participant-component';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from '../../../core/services/app.service';
import { ParticipantService } from '../../../shared/services/participant.service';
import { Observable, forkJoin } from 'rxjs';
import { ParticipationStatus } from '../../../shared/models/participation-statuses.model';
import { Participant } from '../../../shared/models/participant';
import { AccessType } from '../../../shared/enums/access-type.enum';
import { Authorization } from '../../../shared/models/authorization';
import { take, map } from 'rxjs/operators';

@Component({
  selector: 'app-participation-statuses-list-page',
  templateUrl: './list-page.component.html',
  styleUrls: ['./list-page.component.scss']
})
export class ParticipationStatusesListPageComponent implements OnInit {
  public goBackUrl: string;
  public isReadOnly: boolean;
  public isInEditMode: boolean;
  public canEdit: boolean;
  public canView: boolean;
  public pin: string;
  public participationStatuses: ParticipationStatus[];
  public isLoaded = false;
  public participant: Participant;
  constructor(private route: ActivatedRoute, private router: Router, private partService: ParticipantService, private appService: AppService) { }

  ngOnInit() {
    forkJoin(this.route.params.pipe(take(1)), this.partService.modeForParticipationStatuses.pipe(take(1)), this.partService.getCurrentParticipant().pipe(take(1))).subscribe(result => {
      this.pin = result[0].pin;
      this.goBackUrl = '/pin/' + this.pin;
      this.isReadOnly = result[1].readOnly;
      this.isInEditMode = result[1].inEditMode;
      this.participant = result[2];
      this.isLoaded = true;
    });
  }
}
