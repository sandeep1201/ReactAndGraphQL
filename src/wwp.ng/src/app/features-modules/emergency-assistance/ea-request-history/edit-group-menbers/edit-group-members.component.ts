import { Router } from '@angular/router';
import { Component, OnInit, Input } from '@angular/core';
import { Participant } from 'src/app/shared/models/participant';
import { EmergencyAssistanceService } from '../../services/emergancy-assistance.service';
import { take, concatMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { AppService } from 'src/app/core/services/app.service';
import { EAPreviousGroupMembers, EARequestParticipant } from '../../models';
import { EAGroupMembers } from '../../models/ea-request-participant.model';
import { EARequestEditService } from '../../services/ea-request-edit.service';
import * as _ from 'lodash';

@Component({
  selector: 'app-ea-group-edit',
  templateUrl: './edit-group-members.component.html',
  styleUrls: ['./edit-group-members.component.scss']
})
export class EAGroupEditComponent implements OnInit {
  @Input() participant: Participant;
  @Input() eaModel: EAGroupMembers;
  @Input() requestId: number;
  public isLoaded = false;
  public hadSaveError = false;
  public isSaving = false;
  public isSectionModified = false;
  public isInPreviousMember = false;
  public isSearchMode = false;
  private agModel: EAPreviousGroupMembers[] = [];
  public eaRequestModel: EARequestParticipant[] = [];
  public cachedEaRequestModel: EARequestParticipant[] = [];
  private cachedItem: EARequestParticipant = new EARequestParticipant();
  public isApplicantId: number;

  //Search
  isSearching = false;
  errorMessage = '';
  searchQuery: number;
  cachedSearchQuery: number;
  isPinFieldDisabled = false;

  constructor(private requestEditService: EARequestEditService, private eaService: EmergencyAssistanceService, private appService: AppService, private router: Router) {}

  ngOnInit() {
    this.eaService.modeForEARequest
      .pipe(
        take(1),
        concatMap(res => {
          this.isInPreviousMember = res.groupMemberMode;
          this.isSearchMode = res.isSearchMode;
          return this.isInPreviousMember ? this.eaService.getPerviousEAGroup(this.participant.pin, this.requestId) : of([]);
        })
      )
      .pipe(take(1))
      .subscribe(res => {
        this.agModel = res;
        this.initModel();
      });
  }

  initModel() {
    if (this.isInPreviousMember) {
      this.agModel = _.orderBy(this.agModel, ['otherPersonLastName', 'otherPersonFirstName']);
      // Cloning from EAAGModel to EARequestModel.
      this.agModel.map(x => this.eaRequestModel.push(EAPreviousGroupMembers.createRequestParticipantModel(x)));
      // Adding Already Added boolean and creating cached model
      this.eaRequestModel.map(
        x => (
          this.eaModel.eaGroupMembers.map(y =>
            y.pinNumber === x.pinNumber ? ((x.alreadyAdded = true), (x.eaIndividualTypeId = y.eaIndividualTypeId), (x.eaRelationTypeId = y.eaRelationTypeId)) : null
          ),
          EARequestParticipant.clone(x, this.cachedItem),
          this.cachedEaRequestModel.push({ ...this.cachedItem } as EARequestParticipant)
        )
      );
    }
    this.isLoaded = true;
  }

  onSearch() {
    if (this.cachedSearchQuery !== this.searchQuery) {
      this.isSearching = true;
      this.errorMessage = '';
      this.eaRequestModel = [];
      this.cachedSearchQuery = this.searchQuery;
      if (this.searchQuery && /^\d{10}$/.test(this.searchQuery.toString())) {
        this.eaService
          .searchParticipant(this.searchQuery.toString())
          .pipe(take(1))
          .subscribe(res => {
            this.isSearching = false;
            if (res) {
              let requestParticipant = new EARequestParticipant();
              requestParticipant = EAPreviousGroupMembers.createRequestParticipantModel(res as EAPreviousGroupMembers);
              requestParticipant.pinNumber = +this.searchQuery;
              requestParticipant.alreadyAdded = this.eaModel.eaGroupMembers.some(x => x.pinNumber === requestParticipant.pinNumber);
              this.eaRequestModel = [requestParticipant];
            } else this.errorMessage = 'PIN not found';
          });
      } else {
        this.isSearching = false;
        this.errorMessage = 'PIN not valid. Please enter a valid 10 digit PIN number.';
      }
    }
  }

  onClearance() {
    this.eaService.modeForEARequest.next({ groupMemberMode: false, isSearchMode: false });
    this.router.navigate(['/clearance'], {
      state: { from: 'EA', pin: this.participant.pin, id: this.eaModel.requestId, agency: this.requestEditService.model.organizationCode }
    });
  }

  exit() {
    if (this.isSectionModified) {
      this.appService.isUrlChangeBlocked = false;
      this.appService.isDialogPresent = true;
    } else {
      this.eaService.modeForEARequest.next({ groupMemberMode: false, isSearchMode: false });
    }
  }

  exitEAEditIgnoreChanges() {
    this.eaService.modeForEARequest.next({ groupMemberMode: false, isSearchMode: false });
  }

  onAdd() {
    this.isSaving = true;
    const model = new EAGroupMembers();
    if (this.isInPreviousMember) this.eaModel.isPreviousMemberClicked = true;
    EAGroupMembers.clone(this.eaModel, model);
    model.eaGroupMembers = [...model.eaGroupMembers, ...this.eaRequestModel.filter(x => x.add)];
    this.requestEditService.postHouseholdMemberseSection(model, true, false).subscribe(
      res => {
        this.isSaving = false;
        this.eaService.modeForEARequest.next({ groupMemberMode: false, isSearchMode: false });
      },
      error => {
        this.isSaving = false;
        this.hadSaveError = true;
      }
    );
  }
}
