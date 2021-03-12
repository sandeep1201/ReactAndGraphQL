import { Participant } from 'src/app/shared/models/participant';
import { AuxiliaryStatusTypes } from './../enums/auxiliary-status-types.enums';
import { AuxiliaryService } from '../services/auxiliary.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { Auxiliary } from '../models/auxiliary.model';
import { concatMap } from 'rxjs/operators';
import { AppService } from 'src/app/core/services/app.service';
import { Authorization } from 'src/app/shared/models/authorization';
import { Utilities } from 'src/app/shared/utilities';

@Component({
  selector: 'app-aux-worker-list',
  templateUrl: './worker-list.component.html',
  styleUrls: ['./worker-list.component.scss']
})
export class AuxiliaryWorkerListComponent implements OnInit {
  public goBackUrl = '';
  public isLoaded = false;
  public isInEditMode = false;
  public participationDetails: any;
  public auxiliaryList: Auxiliary[];
  public participantId: number;
  public pin;
  public participant: Participant;
  public isSortByDateToggled = false;

  constructor(private route: ActivatedRoute, private partService: ParticipantService, private auxService: AuxiliaryService, private appService: AppService) {}

  ngOnInit() {
    this.route.params
      .pipe(
        concatMap(result => {
          this.pin = result.pin;
          this.goBackUrl = '/pin/' + this.pin;
          return this.partService.getCachedParticipant(this.pin);
        })
      )
      .subscribe(res => {
        this.participant = res;
        this.participantId = res.id;
        this.onParticipantInit();
        this.initAuxData();
      });
  }

  initAuxData() {
    this.auxService.modeForAuxiliary.subscribe(res => {
      this.isInEditMode = res.isInEditMode;
      if (!this.isInEditMode) {
        this.getAuxData();
      }
    });
  }

  public onParticipantInit() {
    this.partService.getParticipantSummaryDetails(this.pin).subscribe(res => {
      this.participationDetails = res;
    });
  }

  public getAuxData() {
    this.auxService.getAuxiliaryData(this.participant.id).subscribe(res => {
      this.auxiliaryList = res;
      this.isLoaded = true;
    });
  }

  onAdd() {
    this.isInEditMode = true;
    this.auxService.modeForAuxiliary.next({ readonly: false, isInEditMode: this.isInEditMode });
  }

  edit(a, readonly) {
    this.isInEditMode = true;
    if (a.auxiliaryStatusTypeCode === AuxiliaryStatusTypes.RETURN && this.canEditAux()) {
      readonly = false;
    }
    this.auxService.modeForAuxiliary.next({ aux: a, readonly: readonly, isInEditMode: this.isInEditMode });
  }

  sortByDate() {
    this.auxiliaryList = Utilities.sortArrayByDate(this.auxiliaryList, 'auxiliaryStatusDate', this.isSortByDateToggled);
    this.isSortByDateToggled = !this.isSortByDateToggled;
  }

  canEditAux() {
    return this.appService.isUserAuthorized(Authorization.canAccessAuxiliary_Edit, this.participant);
  }
}
