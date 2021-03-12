import { EAIPVStatus } from './../../../models/ea-request-sections.enum';
import { EAIPV } from './../../../models/ea-ipv.model';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FieldDataService } from 'src/app/shared/services/field-data.service';
import { AppService } from 'src/app/core/services/app.service';
import { EmergencyAssistanceService } from '../../../services/emergancy-assistance.service';
import { ValidationManager } from 'src/app/shared/models/validation';
import { ModelErrors } from 'src/app/shared/interfaces/model-errors';
import { forkJoin } from 'rxjs';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { FieldDataTypes } from 'src/app/shared/enums/field-data-types.enum';
import { Utilities } from 'src/app/shared/utilities';
import { IMultiSelectSettings } from 'src/app/shared/components/multi-select-dropdown/multi-select-dropdown.component';
import * as _ from 'lodash';

@Component({
  selector: 'app-ipv-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss']
})
export class EAIPVEditComponent implements OnInit {
  pin: string;
  ipvModel: EAIPV;
  cachedIpvModel: EAIPV = new EAIPV();
  isReadOnly = false;
  isLoaded = false;
  isSectionModified = false;
  hasTriedSave = false;
  isSaving = false;
  hadSaveError = false;
  isSectionValid = true;
  mailingAddressValidateStatus = false;
  isOverTurnDisabled = false;
  isOverTurnedDateDisabled = false;
  activeStatusId: number;
  reasonTypes: DropDownField[] = [];
  occurrenceTypes: DropDownField[] = [];
  statusTypes: DropDownField[] = [];
  countiesDrop: DropDownField[] = [];
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public modelErrors: ModelErrors = {};
  public multSelectSettings: IMultiSelectSettings = {
    pullRight: false,
    enableSearch: false,
    checkedStyle: 'checkboxes',
    buttonClasses: 'btn btn-default',
    selectionLimit: 8,
    closeOnSelect: false,
    autoUnselect: false,
    showCheckAll: false,
    showUncheckAll: false,
    dynamicTitleMaxItems: 100,
    maxHeight: '300px'
  };
  constructor(private route: ActivatedRoute, private fdService: FieldDataService, private appService: AppService, private eaService: EmergencyAssistanceService) {}

  ngOnInit() {
    this.pin = this.route.snapshot.params.pin;
    if (this.eaService.modeForIPVRequest && this.eaService.modeForIPVRequest.value) {
      this.isReadOnly = this.eaService.modeForIPVRequest.value.isReadOnly;
      this.ipvModel = this.eaService.modeForIPVRequest.value.ipvModel;
    }

    this.requestDataFromMultipleSource().subscribe(results => {
      this.reasonTypes = results[0];
      this.occurrenceTypes = results[1];
      this.statusTypes = results[2];
      this.countiesDrop = results[3];
      this.activeStatusId = +Utilities.fieldDataIdByCode(EAIPVStatus.Active, this.statusTypes);
      this.ipvModel.isOverTurned = Utilities.fieldDataCodeById(this.ipvModel.statusId, this.statusTypes) === EAIPVStatus.Overturned;
      EAIPV.clone(this.ipvModel, this.cachedIpvModel);
      this.isLoaded = true;
    });
  }

  requestDataFromMultipleSource() {
    const source0 = this.fdService.getFieldDataByField(FieldDataTypes.EAIPVReasons);
    const source1 = this.fdService.getFieldDataByField(FieldDataTypes.EAIPVOccurrences);
    const source2 = this.fdService.getFieldDataByField(FieldDataTypes.EAIPVStatuses);
    const source3 = this.fdService.getFieldDataByField(FieldDataTypes.CountiesNumeric);

    return forkJoin(source0, source1, source2, source3);
  }

  showOverTurned() {
    return this.ipvModel.statusCode === EAIPVStatus.Active || this.ipvModel.statusCode === EAIPVStatus.Pending;
  }

  setOverTurnedFields($event) {
    this.ipvModel.isOverTurned = $event;

    if (this.ipvModel.isOverTurned && this.ipvModel.statusCode === EAIPVStatus.Pending) {
      this.ipvModel.overTurnedDate = this.ipvModel.penaltyStartDate;
      this.cachedIpvModel.overTurnedDate = this.ipvModel.penaltyStartDate;
      this.isOverTurnedDateDisabled = true;
    }
  }

  setOverTurnDisabled() {
    this.isOverTurnDisabled = !_.isEqual(
      _.omit(this.ipvModel, ['canEdit', 'canView', 'iPVNumber', 'mailingAddress.isAddressValidated', 'isOverTurned', 'overTurnedDate']),
      _.omit(this.cachedIpvModel, ['canEdit', 'canView', 'iPVNumber', 'mailingAddress.isAddressValidated', 'isOverTurned', 'overTurnedDate'])
    );
  }

  validate() {
    this.setOverTurnDisabled();
    this.isSectionModified = true;
    if (this.hasTriedSave === true) {
      this.validationManager.resetErrors();
      const result = this.ipvModel.validate(this.validationManager, this.activeStatusId);
      this.isSectionValid = result.isValid;
      this.modelErrors = result.errors;
      if (this.isSectionValid) this.hasTriedSave = false;
    }
  }

  CleanseModelForApi() {
    this.ipvModel.description = Utilities.isStringEmptyOrNull(this.ipvModel.description) ? null : this.ipvModel.description;
    this.ipvModel.notes = Utilities.isStringEmptyOrNull(this.ipvModel.notes) ? null : this.ipvModel.notes;
    this.ipvModel.overTurnedDate = this.ipvModel.isOverTurned ? this.ipvModel.overTurnedDate : null;
  }

  cancel() {
    if (this.isSectionModified) {
      this.appService.isUrlChangeBlocked = true;
      this.appService.isDialogPresent = true;
    } else {
      this.exitEdit();
    }
  }

  submit() {
    this.isSaving = true;
    this.hasTriedSave = true;
    this.validate();
    if (this.isSectionValid === true) {
      this.saveIPV();
    } else {
      this.isSaving = false;
    }
  }

  private saveIPV() {
    this.CleanseModelForApi();
    const statusId = this.ipvModel.statusId;
    this.ipvModel.statusId = this.ipvModel.statusId = this.ipvModel.isOverTurned ? +Utilities.fieldDataIdByCode(EAIPVStatus.Overturned, this.statusTypes) : this.ipvModel.statusId;
    this.eaService.saveIPV(this.ipvModel, this.pin).subscribe(
      data => {
        this.isSaving = false;
        this.exitEdit();
      },
      error => {
        this.isSaving = false;
        this.hadSaveError = true;
        this.ipvModel.statusId = statusId;
      }
    );
  }

  exitEdit() {
    this.eaService.modeForIPVRequest.next({ isInEditMode: false });
  }
}
