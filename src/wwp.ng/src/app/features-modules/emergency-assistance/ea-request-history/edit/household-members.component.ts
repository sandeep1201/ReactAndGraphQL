import { EAViewModes } from './../../models/ea-request-sections.enum';
import { FieldDataTypes } from './../../../../shared/enums/field-data-types.enum';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { Participant } from './../../../../shared/models/participant';
import { EmergencyAssistanceService } from './../../services/emergancy-assistance.service';
import { EAGroupMembers, EARequestParticipant } from './../../models/ea-request-participant.model';
import { FieldDataService } from './../../../../shared/services/field-data.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { SectionComponent } from 'src/app/shared/interfaces/section-component';
import { ValidationManager } from 'src/app/shared/models/validation';
import { ModelErrors } from 'src/app/shared/interfaces/model-errors';
import { EARequestEditService } from '../../services/ea-request-edit.service';
import { AppService } from 'src/app/core/services/app.service';
import { ActivatedRoute } from '@angular/router';
import { concatMap } from 'rxjs/operators';
import { forkJoin, of } from 'rxjs';
import { EARequestSections, EAStatusCodes, EAIndividualType } from '../../models/ea-request-sections.enum';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { Utilities } from 'src/app/shared/utilities';
import { SubSink } from 'subsink';
import * as _ from 'lodash';
import * as moment from 'moment';

enum MemberCodes {
  OtherCaretaker = 'OCTR',
  Caretaker = 'CTR',
  Self = 'SF'
}
@Component({
  selector: 'app-ea-request-edit-household-members',
  templateUrl: './household-members.component.html',
  styles: []
})
export class EARequestHouseholdMembersEditComponent implements OnInit, SectionComponent, OnDestroy {
  requestId: string;
  pin: string;
  viewMode = EAViewModes.View;
  isSectionLoaded = false;
  public isActive = true;
  participant: Participant;
  model: EAGroupMembers = new EAGroupMembers();
  cachedModel: EAGroupMembers;
  validationManager: ValidationManager = new ValidationManager(this.appService);
  public modelErrors: ModelErrors = {};
  public isSectionValid = true;
  public isSectionModified = false;
  public hadSaveError = false;
  public hasTriedSave = false;
  public isReadOnly = false;
  public eaIndividualTypesDrop: DropDownField[] = [];
  public filteredEaIndividualTypesDrop: DropDownField[] = [];
  public eaRelationshipTypesDrop: DropDownField[] = [];
  public filteredEARelationshipTypeDropForNonOCTR: DropDownField[] = [];
  public filteredEARelationshipTypeDropForOCTR: DropDownField[] = [];
  public eaSSNExemptsDrop: DropDownField[] = [];
  public isApplicantId: number;
  public otherCareTakeId: number;
  public groupMemberMode = false;
  public isSearchMode = false;
  private requestSub = new SubSink();
  public isMinor = false;

  constructor(
    private requestEditService: EARequestEditService,
    private eaService: EmergencyAssistanceService,
    private appService: AppService,
    private route: ActivatedRoute,
    private fdService: FieldDataService,
    private partService: ParticipantService
  ) {
    scrollTo(0, 0);
  }

  ngOnInit() {
    this.requestEditService.setSectionComponent(this);
    this.requestId = this.route.parent.snapshot.paramMap.get('id');
    this.pin = this.route.parent.snapshot.paramMap.get('pin');
    this.viewMode = this.route.parent.snapshot.paramMap.get('mode') as EAViewModes;

    this.requestSub.add(
      this.eaService.modeForEARequest
        .pipe(
          concatMap(res => {
            this.groupMemberMode = res.groupMemberMode;
            this.isSearchMode = res.isSearchMode;
            const result0 = !this.participant ? this.partService.getCachedParticipant(this.pin) : of(null);
            const result1 = !this.groupMemberMode ? this.requestEditService.getEARequest(this.pin, this.requestId) : of(null);
            const result2 = this.eaIndividualTypesDrop.length === 0 ? this.fdService.getFieldDataByField(FieldDataTypes.EAIndividualTypes) : of(null);
            const result3 = this.eaRelationshipTypesDrop.length === 0 ? this.fdService.getFieldDataByField(FieldDataTypes.EARelationshipTypes) : of(null);
            const result4 = this.eaSSNExemptsDrop.length === 0 ? this.fdService.getFieldDataByField(FieldDataTypes.EASSNExempts) : of(null);
            const results = forkJoin(result0, result1, result2, result3, result4);
            return results;
          })
        )
        .subscribe(results => {
          if (results[0]) this.participant = results[0];
          if (results[2]) this.eaIndividualTypesDrop = results[2];
          if (results[1]) this.initSection(results[1].eaGroupMembers);
          if (results[3]) this.eaRelationshipTypesDrop = results[3];
          if (results[4]) this.eaSSNExemptsDrop = results[4];
          this.isApplicantId = +Utilities.fieldDataIdByCode(MemberCodes.Caretaker, this.eaIndividualTypesDrop);
          this.otherCareTakeId = +Utilities.fieldDataIdByCode(MemberCodes.OtherCaretaker, this.eaIndividualTypesDrop);
          this.filteredEARelationshipTypeDropForOCTR = this.eaRelationshipTypesDrop.filter(x => x.code !== MemberCodes.Self);
          this.filteredEARelationshipTypeDropForNonOCTR = this.eaRelationshipTypesDrop.filter(x => x.code !== MemberCodes.Self && x.optionCd !== 'OnlyForCR');
          this.filteredEaIndividualTypesDrop = this.eaIndividualTypesDrop.filter(x => x.code !== MemberCodes.Caretaker);
          this.modifiedTrackerForcedValidation();
          this.isSectionLoaded = true;
        })
    );
  }

  private initSection(model: EAGroupMembers) {
    this.model = model;
    this.requestEditService.model.eaGroupMembers = model;
    this.cachedModel = this.requestEditService.lastSavedModel.eaGroupMembers;
    this.isReadOnly =
      this.viewMode === EAViewModes.View || (this.requestEditService.lastSavedModel.statusCode !== EAStatusCodes.InProgress && !this.appService.isUserEASupervisor());
    this.setMinorOtherCareTakerRelative();
    this.setSaveButtonEnableOrDisable();
  }

  setIndividualTypeCode($event): EAIndividualType {
    return Utilities.fieldDataCodeById($event, this.eaIndividualTypesDrop) as EAIndividualType;
  }

  setMinorOtherCareTakerRelative() {
    const currentDate = Utilities.currentDate.clone();
    this.isMinor = this.model.eaGroupMembers.some(
      x =>
        Utilities.fieldDataCodeById(x.eaIndividualTypeId, this.eaIndividualTypesDrop) === EAIndividualType.OtherCaretakerRelative &&
        currentDate.diff(x.participantDOB, 'years') < 18
    );
  }

  isValid(): boolean {
    return this.isSectionValid;
  }

  scrollToTop() {
    scrollTo(0, 0);
  }

  prepareToSaveWithErrors() {
    this.model.eaGroupMembers.forEach(x => {
      if (x.ssn) {
        x.ssnAppliedDate = null;
        x.ssnExemptTypeId = null;
      }
    });
    return this.model;
  }

  openHelp() {}

  editGroup(addMode?) {
    if (addMode) {
      this.isSearchMode = true;
      this.eaService.modeForEARequest.next({ isSearchMode: this.isSearchMode });
    } else {
      this.groupMemberMode = true;
      this.eaService.modeForEARequest.next({ groupMemberMode: this.groupMemberMode });
    }
  }

  refreshModel() {
    if (this.model != null && this.requestEditService.model.eaGroupMembers != null) {
      this.initSection(this.requestEditService.model.eaGroupMembers);
      this.modifiedTrackerForcedValidation();
    }
    this.isActive = false;
    setTimeout(() => (this.isActive = true), 0);
  }

  modifiedTrackerForcedValidation() {
    if (this.requestEditService.modifiedTracker.isHouseholdMembers.validated || this.requestEditService.modifiedTracker.isHouseholdMembers.error) {
      this.isSectionValid = false;
      this.validate();
    }
  }

  validate(): void {
    this.validationManager.resetErrors();
    this.isSectionValid = false;
    this.modelErrors = {};
    const cannotUnincludeParticipants = this.model.eaGroupMembers
      .filter((x, index) => x.isIncluded !== this.cachedModel.eaGroupMembers[index].isIncluded && !x.isIncluded)
      .filter(
        x =>
          this.requestEditService.model.eaHouseHoldFinancials.eaHouseHoldIncomes.some(y => y.groupMember === x.participantId) ||
          this.requestEditService.model.eaHouseHoldFinancials.eaAssets.some(y => y.assetOwner === x.participantId) ||
          this.requestEditService.model.eaHouseHoldFinancials.eaVehicles.some(y => y.vehicleOwner === x.participantId)
      );
    const result = EARequestParticipant.validate(this.validationManager, this.model.eaGroupMembers, this.eaIndividualTypesDrop, cannotUnincludeParticipants);
    this.isSectionValid = result.isValid;
    this.modelErrors = result.errors;
    if (this.isSectionValid) this.requestEditService.sectionIsNowValid(this.requestEditService.getSectionLabel(EARequestSections.Members));
    this.setSaveButtonEnableOrDisable();
  }

  setSaveButtonEnableOrDisable() {
    this.requestEditService.modifiedTracker.isSaveDisabled = this.isReadOnly || !!this.modelErrors[`disableSave`];
  }

  checkState() {
    if (!this.isSectionValid) {
      this.validate();
    }
    this.requestEditService.setModifiedModel(
      this.requestEditService.getSectionLabel(EARequestSections.Members),
      !_.isEqual(this.cachedModel, this.requestEditService.model.eaGroupMembers)
    );
  }

  ngOnDestroy() {
    if (this.requestSub) this.requestSub.unsubscribe();
    this.requestEditService.modifiedTracker.isSaveDisabled = false;
  }
}
