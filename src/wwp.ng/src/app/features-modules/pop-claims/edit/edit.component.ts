import { POPClaimStatusTypes } from './../enums/pop-claim-status-types.enum';
import { Participant } from './../../../shared/models/participant';
import { PopClaimsService } from './../services/pop-claims.service';
import { DropDownField } from './../../../shared/models/dropdown-field';
import { JobTypeName } from '../../../shared/enums/job-type-name.enum';
import { FieldDataService } from './../../../shared/services/field-data.service';
import { Component, OnInit, Input, Output, EventEmitter, OnDestroy } from '@angular/core';
import { AppService } from 'src/app/core/services/app.service';
import { ValidationManager } from 'src/app/shared/models/validation';
import { POPClaim } from '../models/pop-claim.model';
import { ModelErrors } from 'src/app/shared/interfaces/model-errors';
import { Utilities } from 'src/app/shared/utilities';
import * as _ from 'lodash';
import { WhyReason } from 'src/app/shared/models/why-reasons.model';
import { POPClaimEmployment } from '../models/pop-claim-employment.model';
import { FieldDataTypes } from 'src/app/shared/enums/field-data-types.enum';
import { POPClaimTypes } from 'src/app/shared/enums/pop-claim.enum';
import * as moment from 'moment';
@Component({
  selector: 'app-pop-claims-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss']
})
export class PopClaimsEditComponent implements OnInit, OnDestroy {
  public isLoaded = false;
  @Input() pin: any;
  @Input() participant: Participant;

  @Input() popClaims: POPClaim[];
  public model = new POPClaim();
  public cachedModel = new POPClaim();

  public isReadOnly = false;
  public isSaving = false;
  public isSectionValid = true;
  public tempEmps: any;
  @Output() onExitEditMode = new EventEmitter<any>();
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public precheck: WhyReason = new WhyReason();
  @Input() POPClaimTypes: DropDownField[];
  public originalPOPClaimTypes: DropDownField[];
  hasTriedSave: boolean;
  isSectionModified: boolean;
  public modelErrors: ModelErrors = {};
  public isPrimary = false;
  public primaryPOPEmployment: POPClaimEmployment;
  POPClaimsStatusTypesDrop: DropDownField[];
  isSectionValidFromPreCheck = true;

  canUsePEBDForCPBD: boolean;
  selectedClaimsTypesWhileAddingAPOPClaim = [];

  isHighWagePOPClaimSelectedbool = false;
  constructor(private appService: AppService, private popClaimService: PopClaimsService, private fdService: FieldDataService) {}

  ngOnInit() {
    this.originalPOPClaimTypes = [...this.POPClaimTypes];
    this.POPClaimTypes = this.POPClaimTypes.filter(i => i.isSystemUseOnly === false);
    this.initPOPModel();
    this.initPOPStatusTypes();
  }

  initPOPModel() {
    this.model.id = 0;
    this.model.participantId = this.participant.id;
    this.model.pinNumber = this.pin;
    if (this.model.id === 0) {
      this.popClaimService.getEmployments(this.pin, this.model.id).subscribe(res => {
        this.tempEmps = res.slice(0);
        this.filterEmploymentsForPop();
        this.checkForEmployments();
        POPClaim.clone(this.model, this.cachedModel);
      });
    }
  }
  initPOPStatusTypes() {
    this.fdService.getFieldDataByField(FieldDataTypes.POPClaimStatusTypes).subscribe(res => {
      this.POPClaimsStatusTypesDrop = res;
      this.isLoaded = true;
    });
  }
  isPEBDToCPBDDisabled(): boolean {
    return !this.primaryPOPEmployment;
  }

  getInitialModelValue(i: number, property: string) {
    return Utilities.getPropertybyIdAndName(this.model.popClaimEmployments, i, property);
  }

  exitAuxiliaryEditIgnoreChanges() {}

  calculateClaimPeriodBeginDate() {
    if (this.canUsePEBDForCPBD) {
      this.model.claimPeriodBeginDate = this.primaryPOPEmployment.jobBeginDate;
      this.calculateClaimEffectiveDate();
    } else if (!!this.model.claimPeriodBeginDate) {
      this.model.claimPeriodBeginDate = null;
      this.calculateClaimEffectiveDate();
    }
  }

  calculateClaimEffectiveDate() {
    // Since we are including the the first day in the calculation i am adding 30 and 92 days only instead of 31 and 93 days...
    if (
      this.model.claimPeriodBeginDate &&
      this.model.claimPeriodBeginDate.length === 10 &&
      moment(this.model.claimPeriodBeginDate, 'MM/DD/YYYY').isValid() &&
      (this.model.popClaimTypeId === +Utilities.fieldDataIdByCode(POPClaimTypes.jobAttainmentCd, this.POPClaimTypes) ||
        this.model.popClaimTypeId === +Utilities.fieldDataIdByCode(POPClaimTypes.longTermCd, this.POPClaimTypes) ||
        this.model.popClaimTypeId === +Utilities.fieldDataIdByCode(POPClaimTypes.jobAttainmentWithHighWageCd, this.POPClaimTypes))
    ) {
      this.model.claimEffectiveDate = moment(this.model.claimPeriodBeginDate, 'MM/DD/YYYY')
        .add(30, 'days')
        .format('MM/DD/YYYY');
    } else if (
      this.model.claimPeriodBeginDate &&
      this.model.claimPeriodBeginDate.length === 10 &&
      moment(this.model.claimPeriodBeginDate, 'MM/DD/YYYY').isValid() &&
      this.model.popClaimTypeId === +Utilities.fieldDataIdByCode(POPClaimTypes.jobRetentionCd, this.POPClaimTypes)
    ) {
      this.model.claimEffectiveDate = moment(this.model.claimPeriodBeginDate, 'MM/DD/YYYY')
        .add(92, 'days')
        .format('MM/DD/YYYY');
    } else if (!!this.model.claimEffectiveDate) {
      this.model.claimEffectiveDate = null;
    }
  }

  checkForEmployments() {
    if (this.model.popClaimEmployments.length === 0) {
      this.precheck.errors = this.precheck.errors
        ? [...this.precheck.errors, 'At least one valid job must be entered in Work History.']
        : ['At least one valid job must be entered in Work History.'];
      this.isSectionValidFromPreCheck = false;
    } else {
      if (this.precheck.errors && this.precheck.errors.indexOf('At least one valid job must be entered in Work History.') > -1) {
        this.precheck.errors.splice(this.precheck.errors.indexOf('At least one valid job must be entered in Work History.'), 1);
      }
      if (_.isEmpty(this.precheck.errors)) this.isSectionValidFromPreCheck = true;
    }
  }
  checkForAlreadySubmittedPOPClaims() {
    const claimTypeName = Utilities.fieldDataNameById(this.model.popClaimTypeId, this.POPClaimTypes);
    if (claimTypeName == POPClaimTypes.jobAttainmentDesc) {
      this.checkForAlreadySubmittedJAHWPOPClaims(claimTypeName);
    }
    if (
      this.popClaims.some(
        pop => pop.popClaimTypeId === this.model.popClaimTypeId && pop.claimStatusTypeId === +Utilities.fieldDataIdByCode(POPClaimStatusTypes.SUBMIT, this.POPClaimsStatusTypesDrop)
      )
    ) {
      if (this.selectedClaimsTypesWhileAddingAPOPClaim.indexOf(claimTypeName) === -1) {
        this.cleansePreCheckErrorsForClaimType();
        this.selectedClaimsTypesWhileAddingAPOPClaim.push(claimTypeName);
        this.precheck.errors = this.precheck.errors
          ? [...this.precheck.errors, `A duplicate ${claimTypeName} POP Claim exists in "Submitted" status. You cannot submit another ${claimTypeName} POP Claim.`]
          : [`A duplicate ${claimTypeName} POP Claim exists in "Submitted" status. You cannot submit another ${claimTypeName} POP Claim.`];
        this.isSectionValidFromPreCheck = false;
      }
    } else {
      if (this.precheck.errors && this.selectedClaimsTypesWhileAddingAPOPClaim.length > 0) {
        this.cleansePreCheckErrorsForClaimType();
      }
      if (_.isEmpty(this.precheck.errors)) this.isSectionValidFromPreCheck = true;
    }
  }

  checkForAlreadySubmittedJAHWPOPClaims(claimTypeName: string) {
    if (
      this.popClaims.some(
        pop =>
          pop.popClaimTypeId === +Utilities.fieldDataIdByCode(POPClaimTypes.jobAttainmentWithHighWageCd, this.originalPOPClaimTypes) &&
          pop.claimStatusTypeId === +Utilities.fieldDataIdByCode(POPClaimStatusTypes.SUBMIT, this.POPClaimsStatusTypesDrop)
      )
    ) {
      if (this.selectedClaimsTypesWhileAddingAPOPClaim.indexOf(claimTypeName) === -1) {
        this.cleansePreCheckErrorsForClaimType();
        this.selectedClaimsTypesWhileAddingAPOPClaim.push(claimTypeName);
        this.precheck.errors = this.precheck.errors
          ? [...this.precheck.errors, `A Job Attainment with High Wage POP Claim exists in "Submitted" status. You cannot submit Job Attainment POP Claim.`]
          : [`A Job Attainment with High Wage POP Claim exists in "Submitted" status. You cannot submit Job Attainment POP Claim.`];
        this.isSectionValidFromPreCheck = false;
      }
    } else {
      if (this.precheck.errors && this.selectedClaimsTypesWhileAddingAPOPClaim.length > 0) {
        this.cleansePreCheckErrorsForClaimType();
      }
      if (_.isEmpty(this.precheck.errors)) this.isSectionValidFromPreCheck = true;
    }
  }

  cleansePreCheckErrorsForClaimType() {
    this.selectedClaimsTypesWhileAddingAPOPClaim.forEach(claimType => {
      if (this.precheck.errors.indexOf(`A duplicate ${claimType} POP Claim exists in "Submitted" status. You cannot submit another ${claimType} POP Claim.`) > -1) {
        this.precheck.errors.splice(
          this.precheck.errors.indexOf(`A duplicate ${claimType} POP Claim exists in "Submitted" status. You cannot submit another ${claimType} POP Claim.`),
          1
        );
      }
    });
    this.selectedClaimsTypesWhileAddingAPOPClaim = [];
  }

  public filterEmployments(employment: any) {
    return (
      employment.deletedReasonId === null &&
      (employment.jobTypeName === JobTypeName.unSubsidized ||
        employment.jobTypeName === JobTypeName.intership ||
        employment.jobTypeName === JobTypeName.selfEmployed ||
        employment.jobTypeName === JobTypeName.tempNonCustodialParentUnsubsidized ||
        employment.jobTypeName === JobTypeName.tempCustodialParentUnsubsidized ||
        employment.jobTypeName === JobTypeName.staffingAgency)
    );
  }

  isHighWagePOPClaimSelected() {
    if (this.model.popClaimTypeId === +Utilities.fieldDataIdByCode(POPClaimTypes.jobAttainmentWithHighWageCd, this.POPClaimTypes)) {
      this.isHighWagePOPClaimSelectedbool = true;
    } else {
      this.isHighWagePOPClaimSelectedbool = false;
    }
  }

  filterEmploymentsForPop() {
    this.model.popClaimEmployments = this.tempEmps.filter(this.filterEmployments);
    if (this.model.popClaimEmployments.length === 1) {
      this.primaryPOPEmployment = this.model.popClaimEmployments[0];
      this.model.popClaimEmployments[0].isPrimary = true;
    }
  }
  exitEditIgnoreChanges(e) {
    this.onExitEditMode.emit();
  }

  selectedPrimaryEmployment(selectedEmployment) {
    this.model.popClaimEmployments.forEach(emp => (emp.isPrimary = false));
    selectedEmployment.isPrimary = true;
    if (this.isHighWagePOPClaimSelectedbool) {
      selectedEmployment.isSelected = true;
    }
    this.calculateClaimPeriodBeginDate();
  }

  cancel(e) {
    if (this.isSectionModified) {
      this.appService.isUrlChangeBlocked = false;
      this.appService.isDialogPresent = true;
    } else {
      this.onExitEditMode.emit();
    }
  }

  isChildModelErrorsItemInvalid(i: number, childRepeaterName: string, property: string): boolean {
    if (!_.isEmpty(this.modelErrors)) {
      return Utilities.isChildModelErrorsItemInvalid(this.modelErrors, childRepeaterName, i, property);
    }
  }

  validate() {
    this.isSectionModified = true;
    this.isSectionValid = true;
    if (this.hasTriedSave === true) {
      this.validationManager.resetErrors();
      const result = this.model.validate(this.validationManager, true, this.isHighWagePOPClaimSelectedbool, this.POPClaimsStatusTypesDrop);
      this.isSectionValid = result.isValid;
      this.modelErrors = result.errors;
      if (this.isSectionValid) {
        this.hasTriedSave = false;
      }
    }
  }
  savePOP() {
    this.popClaimService.savePOP(this.pin, this.model.id, this.model).subscribe(res => {
      this.onExitEditMode.emit();
    });
  }
  save() {
    this.isSaving = true;
    this.hasTriedSave = true;
    this.model.isSubmit = true;
    this.model.claimStatusTypeCode = POPClaimStatusTypes.SUBMIT;
    this.model.claimStatusTypeId = +Utilities.fieldDataIdByCode(POPClaimStatusTypes.SUBMIT, this.POPClaimsStatusTypesDrop);
    this.model.popClaimTypeCode = Utilities.fieldDataCodeById(this.model.popClaimTypeId, this.POPClaimTypes);
    this.model.popClaimTypeName = Utilities.fieldDataNameById(this.model.popClaimTypeId, this.POPClaimTypes);
    this.validate();
    if (this.isSectionValid) {
      this.popClaimService.preAdd(this.pin, this.model.id, this.model).subscribe(res => {
        if (!_.isEmpty(res.errors)) {
          this.isSectionValid = false;
          this.precheck.errors = res.errors;
          this.isSaving = false;
        } else {
          this.savePOP();
        }
      });
    } else {
      this.isSaving = false;
    }
  }
  ngOnDestroy() {
    this.isPrimary = false;
  }
}
