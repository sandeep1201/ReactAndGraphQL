import { EAFinancialNeeds } from './../../models/ea-request-agency-summary.model';
import { EAStatusCodes, EAEmergencyCodes, EAViewModes } from './../../models/ea-request-sections.enum';
import { DropDownField } from './../../../../shared/models/dropdown-field';
import { FieldDataService } from 'src/app/shared/services/field-data.service';
import { SectionComponent } from 'src/app/shared/interfaces/section-component';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ValidationManager } from 'src/app/shared/models/validation';
import { ModelErrors } from 'src/app/shared/interfaces/model-errors';
import { EARequestEditService } from '../../services/ea-request-edit.service';
import { AppService } from 'src/app/core/services/app.service';
import { ActivatedRoute } from '@angular/router';
import { concatMap } from 'rxjs/operators';
import { forkJoin, of } from 'rxjs';
import { EARequestSections } from '../../models/ea-request-sections.enum';
import { EARequest } from '../../models';
import { FieldDataTypes } from 'src/app/shared/enums/field-data-types.enum';
import { Utilities } from 'src/app/shared/utilities';
import * as _ from 'lodash';
import { SubSink } from 'subsink';
import { CommentsService } from 'src/app/shared/components/comment/comments.service';
import { IMultiSelectSettings } from 'src/app/shared/components/multi-select-dropdown/multi-select-dropdown.component';

enum PendingStatusReason {
  Initial30 = 'NNHI',
  Additional30 = 'NNHA',
  CurrentLandlord = 'NCCI'
}

@Component({
  selector: 'app-ea-request-edit-agency-summary',
  templateUrl: './agency-summary.component.html',
  styles: []
})
export class EARequestAgencySummaryEditComponent implements OnInit, SectionComponent, OnDestroy {
  requestId: string;
  pin: string;
  isSectionLoaded = false;
  public isActive = true;
  public isReadOnly = false;
  public isStatusReadOnly = false;
  public viewMode = EAViewModes.View;
  model: EARequest = new EARequest();
  cachedModel: EARequest;
  validationManager: ValidationManager = new ValidationManager(this.appService);
  impendingHomelessReasonDrop: DropDownField[] = [];
  homelessReasonDrop: DropDownField[] = [];
  eaStatusesDrop: DropDownField[] = [];
  eaStatusReasonsDrop: DropDownField[] = [];
  filteredStatusReasonDrop: DropDownField[] = [];
  financialNeedsDrop: DropDownField[] = [];
  public modelErrors: ModelErrors = {};
  public isSectionValid = true;
  public isSectionModified = false;
  public hadSaveError = false;
  public hasTriedSave = false;
  eaStatusCodes = EAStatusCodes;
  public statusCode = EAStatusCodes.InProgress;
  otherSectionsErrored = false;
  requestSub = new SubSink();
  isStatusApprovedOrPending = false;

  public multSelectSettings: IMultiSelectSettings = {
    pullRight: false,
    enableSearch: false,
    checkedStyle: 'checkboxes',
    buttonClasses: 'btn btn-default',
    selectionLimit: 5,
    closeOnSelect: false,
    autoUnselect: false,
    showCheckAll: false,
    showUncheckAll: false,
    dynamicTitleMaxItems: 100,
    maxHeight: '200px'
  };

  constructor(
    private requestEditService: EARequestEditService,
    private appService: AppService,
    private route: ActivatedRoute,
    private fdService: FieldDataService,
    private commentService: CommentsService
  ) {
    scrollTo(0, 0);
  }

  ngOnInit() {
    this.requestEditService.setSectionComponent(this);
    this.requestId = this.route.parent.snapshot.paramMap.get('id');
    this.pin = this.route.parent.snapshot.paramMap.get('pin');
    this.viewMode = this.route.parent.snapshot.paramMap.get('mode') as EAViewModes;
    let onNavigate = true;
    this.requestSub.add(
      this.commentService.modeOnSaveComment
        .pipe(
          concatMap(res => {
            const result0 = onNavigate ? this.requestEditService.getAgencySummary(this.pin, this.requestId) : of(null);
            const result1 = this.impendingHomelessReasonDrop.length === 0 ? this.fdService.getFieldDataByField(FieldDataTypes.EAEmergencyTypeReasons) : of(null);
            const result2 = this.eaStatusesDrop.length === 0 ? this.fdService.getFieldDataByField(FieldDataTypes.EAStatuses) : of(null);
            const result3 = this.eaStatusReasonsDrop.length === 0 ? this.fdService.getFieldDataByField(FieldDataTypes.EAStatusReasons) : of(null);
            const result4 = res && res.id === +this.requestId ? of(res.commentSaved) : of(null);
            const result5 = this.financialNeedsDrop.length === 0 ? this.fdService.getFieldDataByField(FieldDataTypes.EAFinancialTypes) : of(null);
            onNavigate = false;
            this.setStatusApprovedOrPendingFlag();
            return forkJoin(result0, result1, result2, result3, result4, result5);
          })
        )
        .subscribe(result => {
          if (result[1]) this.impendingHomelessReasonDrop = result[1].filter(x => x.optionCd === EAEmergencyCodes.ImpendingHomelessness);
          if (result[1]) this.homelessReasonDrop = result[1].filter(x => x.optionCd === EAEmergencyCodes.Homelessness);
          if (result[2]) this.eaStatusesDrop = result[2];
          if (result[3]) this.eaStatusReasonsDrop = result[3];
          if (result[0]) this.initSection(result[0]);
          if (result[4] && !this.model.eaAgencySummary.hasComment) this.model.eaAgencySummary.hasComment = true;
          if (result[5]) this.financialNeedsDrop = result[5];
          this.modifiedTrackerForcedValidation();
          this.isSectionLoaded = true;
        })
    );
  }

  private initSection(model: EARequest) {
    this.model = model;
    this.requestEditService.model = model;
    this.cachedModel = this.requestEditService.lastSavedModel;
    this.filterReasonsDrop(this.model.eaAgencySummary.statusId);
    this.isReadOnly = this.viewMode === EAViewModes.View || (this.cachedModel.statusCode !== EAStatusCodes.InProgress && !this.appService.isUserEASupervisor());
    this.isStatusReadOnly =
      this.viewMode === EAViewModes.View ||
      !(
        this.appService.isUserEASupervisor() ||
        (this.appService.isUserEAWorker() && this.cachedModel.statusCode === EAStatusCodes.InProgress) ||
        this.cachedModel.statusCode === EAStatusCodes.Pending
      );
    this.setSaveButtonEnableOrDisable();
    if (this.cachedModel.statusId !== +Utilities.fieldDataIdByCode(EAStatusCodes.InProgress, this.eaStatusesDrop)) {
      this.eaStatusesDrop = this.eaStatusesDrop.filter(x => x.code !== EAStatusCodes.InProgress);
    }
    if (model.eaAgencySummary.eaFinancialNeeds !== null && model.eaAgencySummary.eaFinancialNeeds.length === 0) {
      this.model.eaAgencySummary.eaFinancialNeeds.push(EAFinancialNeeds.create());
    }
  }

  filterReasonsDrop(id: number) {
    this.statusCode = Utilities.fieldDataCodeById(this.model.eaAgencySummary.statusId, this.eaStatusesDrop) as EAStatusCodes;
    this.filteredStatusReasonDrop = this.eaStatusReasonsDrop.filter(x => x.optionCd === Utilities.fieldDataCodeById(id, this.eaStatusesDrop));
    this.setSubmitButtonStatus();
    // Pending Reason Logic
    if (this.statusCode === EAStatusCodes.Pending) {
      if (this.cachedModel.eaAgencySummary.statusReasonIds[0] === +Utilities.fieldDataIdByCode(PendingStatusReason.Additional30, this.filteredStatusReasonDrop))
        this.filteredStatusReasonDrop = this.eaStatusReasonsDrop.filter(x => x.code !== PendingStatusReason.Initial30 && x.optionCd === this.statusCode);
      else if (this.cachedModel.eaAgencySummary.statusReasonIds[0] === +Utilities.fieldDataIdByCode(PendingStatusReason.Initial30, this.filteredStatusReasonDrop))
        this.filteredStatusReasonDrop = this.eaStatusReasonsDrop.filter(x => x.optionCd === Utilities.fieldDataCodeById(this.model.eaAgencySummary.statusId, this.eaStatusesDrop));
      else if (this.cachedModel.eaAgencySummary.statusReasonIds[0] !== +Utilities.fieldDataIdByCode(PendingStatusReason.Initial30, this.filteredStatusReasonDrop))
        this.filteredStatusReasonDrop = this.eaStatusReasonsDrop.filter(x => x.code !== PendingStatusReason.Additional30 && x.optionCd === this.statusCode);
    }

    this.setStatusApprovedOrPendingFlag();
  }

  isValid(): boolean {
    return this.isSectionValid;
  }

  scrollToTop() {
    scrollTo(0, 0);
  }

  setStatusApprovedOrPendingFlag() {
    this.isStatusApprovedOrPending = this.statusCode === EAStatusCodes.Pending || this.statusCode === EAStatusCodes.Approved;
    return this.isStatusApprovedOrPending;
  }

  prepareToSaveWithErrors() {
    if (this.statusCode === EAStatusCodes.InProgress || this.statusCode === EAStatusCodes.Approved) {
      this.model.eaAgencySummary.statusReasonIds = [];
    }
    this.model.eaAgencySummary.eaFinancialNeeds = this.model.eaAgencySummary.eaFinancialNeeds.filter(x => !x.isEmpty());
    return this.model.eaAgencySummary;
  }

  openHelp() {}

  refreshModel() {
    if (this.model.eaAgencySummary != null && this.requestEditService.model.eaAgencySummary != null) {
      this.initSection(this.requestEditService.model);
      this.modifiedTrackerForcedValidation();
    }
    this.isActive = false;
    setTimeout(() => (this.isActive = true), 0);
  }

  modifiedTrackerForcedValidation() {
    if (
      this.requestEditService.modifiedTracker.isAgencySummary.validated ||
      this.requestEditService.modifiedTracker.isAgencySummary.error ||
      (!this.cachedModel.eaAgencySummary.hasComment && this.model.eaAgencySummary.hasComment)
    ) {
      this.isSectionValid = false;
      this.validate();
    }
  }

  validate(): void {
    this.otherSectionsErrored = false;
    this.validationManager.resetErrors();
    this.isSectionValid = false;
    this.modelErrors = {};
    const result = this.model.eaAgencySummary.validate(
      this.validationManager,
      this.eaStatusesDrop,
      this.model,
      this.appService.isUserEAWorker(),
      this.requestEditService.modifiedTracker.isSubmitEnabled
    );
    this.isSectionValid = result.isValid;
    this.modelErrors = result.errors;
    if (this.requestEditService.modifiedTracker.isSubmitEnabled && this.isStatusApprovedOrPending) {
      this.requestEditService.loadValidationContextsAndValidate(this.model, true).subscribe(
        n => n,
        e => e,
        () => {}
      );
      this.requestEditService.validate(this.model, true);
      this.otherSectionsErrored = !(
        this.requestEditService.isSectionValidOnLoad[EARequestSections.Demographics] &&
        this.requestEditService.isSectionValidOnLoad[EARequestSections.Emergency] &&
        this.requestEditService.isSectionValidOnLoad[EARequestSections.Members] &&
        this.requestEditService.isSectionValidOnLoad[EARequestSections.Financials]
      );
      this.isSectionValid = !this.otherSectionsErrored && result.isValid;
    }
    if (this.isSectionValid) this.requestEditService.sectionIsNowValid(this.requestEditService.getSectionLabel(EARequestSections.AgencySummary));
    this.setSaveButtonEnableOrDisable();
  }

  setSaveButtonEnableOrDisable() {
    this.requestEditService.modifiedTracker.isSaveDisabled =
      (this.isReadOnly && this.isStatusReadOnly) ||
      (this.statusCode === EAStatusCodes.Denied || this.statusCode === EAStatusCodes.WithDrawn
        ? !!this.modelErrors['disableDeined']
        : this.statusCode === EAStatusCodes.Approved || this.statusCode === EAStatusCodes.Pending
        ? !!this.modelErrors['disableApprove'] || !this.isSectionValid
        : false);
  }

  setSubmitButtonStatus() {
    this.requestEditService.modifiedTracker.isSubmitEnabled =
      this.statusCode !== EAStatusCodes.InProgress &&
      (this.cachedModel.eaAgencySummary.statusId !== this.model.eaAgencySummary.statusId ||
        (this.model.eaAgencySummary.statusReasonIds.length && !_.isEqual(this.cachedModel.eaAgencySummary.statusReasonIds, this.model.eaAgencySummary.statusReasonIds)));
    if (this.requestEditService.modifiedTracker.isSubmitEnabled) this.isSectionValid = false;
  }

  checkState() {
    if (!this.isSectionValid) {
      this.validate();
    }
    this.requestEditService.setModifiedModel(
      this.requestEditService.getSectionLabel(EARequestSections.AgencySummary),
      !_.isEqual(this.cachedModel.eaAgencySummary, this.requestEditService.model.eaAgencySummary)
    );
  }

  ngOnDestroy() {
    if (this.requestSub) this.requestSub.unsubscribe();
    this.commentService.modeOnSaveComment.next({ id: null, commentSaved: false });
    this.requestEditService.modifiedTracker.isSaveDisabled = false;
  }
}
