import { PadPipe } from 'src/app/shared/pipes/pad.pipe';
import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { DateMmDdYyyyPipe } from 'src/app/shared/pipes/date-mm-dd-yyyy.pipe';
import { AppService } from 'src/app/core/services/app.service';
import { FieldDataTypes } from 'src/app/shared/enums/field-data-types.enum';
import { ValidationManager } from 'src/app/shared/models/validation';
import { ModelErrors } from 'src/app/shared/interfaces/model-errors';
import * as _ from 'lodash';
import { POPClaim } from '../models/pop-claim.model';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { FieldDataService } from 'src/app/shared/services/field-data.service';
import { POPClaimStatusTypeNames } from '../enums/pop-claim-status-type-names.enum';
import { POPClaimStatusTypes } from '../enums/pop-claim-status-types.enum';
import { Utilities } from 'src/app/shared/utilities';
import { PopClaimsService } from '../services/pop-claims.service';
import { POPClaimTypes } from 'src/app/shared/enums/pop-claim.enum';

@Component({
  selector: 'app-pop-claim-single-entry',
  templateUrl: './single-entry.component.html',
  styleUrls: ['./single-entry.component.scss']
})
export class POPClaimSingleEntryComponent implements OnInit {
  isLoaded = false;
  public isReadOnly = true;
  @Input() popClaim: POPClaim;
  @Input() POPClaimTypes;
  @Output() onExitSingleEntryMode = new EventEmitter<any>();
  public isAdjudicator = false;
  public isApprover = false;
  public isWorker = false;
  public popClaimsStatusTypesDrop: DropDownField[];
  @Input() filteredStatusTypesDrop: DropDownField[];
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  isDeniedPOP = false;
  isApprovedPOP = false;
  isSaving = false;
  isSectionValid = true;
  hasTriedSave: boolean;
  isSectionModified = false;

  public modelErrors: ModelErrors = {};
  cachedModel: POPClaim = new POPClaim();
  isEmploymentsEmpty = false;
  isFooterReadOnly = true;
  isStatusEditable = false;
  isDetailsEditable = false;
  @Input() isPinBased = true;
  isHighWagePOPClaimSelectedbool: boolean;

  constructor(private appService: AppService, private fdService: FieldDataService, private popClaimService: PopClaimsService) {}

  ngOnInit() {
    const dateMmDdYyyyPipe = new DateMmDdYyyyPipe();
    const padPide = new PadPipe();
    this.fdService.getFieldDataByField(FieldDataTypes.POPClaimStatusTypes).subscribe(res => {
      this.popClaimsStatusTypesDrop = res;
      this.isAdjudicator = this.appService.isAdjudictor;
      this.isApprover = this.appService.isPOPClaimApprover;
      this.isWorker = this.appService.isPOPClaimWorker;
      this.popClaim.claimPeriodBeginDate = dateMmDdYyyyPipe.transform(this.popClaim.claimPeriodBeginDate);
      this.isDeniedPOP = this.popClaim.claimStatusTypeDisplayName === POPClaimStatusTypeNames.DENIED;
      this.isApprovedPOP = this.popClaim.claimStatusTypeDisplayName === POPClaimStatusTypeNames.APPROVED;
      this.isHighWagePOPClaimSelected();
      POPClaim.clone(this.popClaim, this.cachedModel);
      this.popClaim.claimEffectiveDate = dateMmDdYyyyPipe.transform(this.popClaim.claimEffectiveDate);
      this.popClaim.pinNumber = padPide.transform(this.popClaim.pinNumber, 10);
      if (!Utilities.isStringEmptyOrNull(this.popClaim.activityBeginDate)) {
        this.popClaim.activityBeginDate = dateMmDdYyyyPipe.transform(this.popClaim.activityBeginDate);
      }
      if (!Utilities.isStringEmptyOrNull(this.popClaim.activityEndDate)) {
        this.popClaim.activityEndDate = dateMmDdYyyyPipe.transform(this.popClaim.activityEndDate);
      }
      this.isEmploymentsEmpty = Object.keys(this.popClaim.popClaimEmployments[0]).length === 0;
      this.isLoaded = true;
      this.filterStatusDropBasedOnRole();
      this.setIsFooterReadOnly();
    });
  }

  isHighWagePOPClaimSelected() {
    if (this.popClaim.popClaimTypeId === +Utilities.fieldDataIdByCode(POPClaimTypes.jobAttainmentWithHighWageCd, this.POPClaimTypes)) {
      this.isHighWagePOPClaimSelectedbool = true;
    } else {
      this.isHighWagePOPClaimSelectedbool = false;
    }
  }

  cancel() {
    if (this.isSectionModified) {
      this.appService.isUrlChangeBlocked = false;
      this.appService.isDialogPresent = true;
    } else {
      this.onExitSingleEntryMode.emit();
    }
  }

  filterStatusDropBasedOnRole() {
    if (_.isEmpty(this.filteredStatusTypesDrop)) {
      if (this.isApprover && this.isPinBased) {
        this.filteredStatusTypesDrop = this.popClaimsStatusTypesDrop.filter(x => x.code === POPClaimStatusTypes.WITHDRAW);
      } else if (this.isAdjudicator) {
        this.filteredStatusTypesDrop = this.popClaimsStatusTypesDrop.filter(x => x.code === POPClaimStatusTypes.APPROVE);
      } else if (this.isWorker) {
        this.filteredStatusTypesDrop = this.popClaimsStatusTypesDrop.filter(x => x.code === POPClaimStatusTypes.APPROVE);
      } else {
        this.filteredStatusTypesDrop = this.popClaimsStatusTypesDrop;
      }
    }
  }

  exitEditIgnoreChanges(e) {
    this.onExitSingleEntryMode.emit();
  }

  validate() {
    this.isSectionModified = true;
    if (this.hasTriedSave === true) {
      this.validationManager.resetErrors();
      const result = this.popClaim.validate(this.validationManager, false, false, this.filteredStatusTypesDrop);
      this.isSectionValid = result.isValid;
      this.modelErrors = result.errors;
      if (this.isSectionValid) this.hasTriedSave = false;
    }
  }
  save() {
    this.hasTriedSave = true;
    this.validate();
    this.isSaving = true;
    if (this.isSectionValid) {
      this.savePOP();
    } else {
      this.isSaving = false;
    }
  }

  setClaimStatusTypeCode() {
    this.popClaim.claimStatusTypeCode = Utilities.fieldDataCodeById(this.popClaim.claimStatusTypeId, this.popClaimsStatusTypesDrop);
  }
  savePOP() {
    this.popClaim.isSubmit = false;
    this.popClaimService.savePOP(this.popClaim.pinNumber.toString(), this.popClaim.id, this.popClaim).subscribe(res => {
      this.onExitSingleEntryMode.emit();
    });
  }
  setIsFooterReadOnly() {
    if (this.isAdjudicator && this.isDeniedPOP) {
      this.isFooterReadOnly = false;
      this.isStatusEditable = true;
      this.isDetailsEditable = true;
    } else if (this.isApprover && this.isApprovedPOP) {
      this.isFooterReadOnly = false;
      this.isStatusEditable = true;
      this.isDetailsEditable = true;
    } else if (!this.isPinBased) {
      this.isFooterReadOnly = false;
      this.isStatusEditable = true;
      this.isDetailsEditable = true;
    }
  }
}
