// tslint:disable: import-blacklist
import { Component, OnInit, Input, Output, EventEmitter, ViewChild } from '@angular/core';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { ValidationManager } from '../../../shared/models/validation';
import { AppService } from '../../../core/services/app.service';
import { OrganizationInformationService } from 'src/app/features-modules/organization-info/services/organization-information.service';
import { FinalistLocation, OrganizationInformation } from 'src/app/features-modules/organization-info/models/organization-information.model';
import { FinalistAddress } from 'src/app/shared/models/finalist-address.model';

@Component({
  selector: 'app-org-info-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss']
})
export class OrganizationInformationEditComponent implements OnInit {
  @Input() finalistLocation: FinalistLocation;
  @Input() organizationId: number;
  @Input() programId: number;

  @Output() isInEditMode = new EventEmitter<any>();

  public isSaving = false;
  public isDisabled = false;
  public modelErrors: ModelErrors = {};
  public isLoaded = true;
  public isSectionValid = true;
  public isAddressSectionValid = false;
  public originalModel: FinalistLocation;
  public isSectionModified = false;
  public isAddressSectionModified = false;
  public hadSaveError = false;
  public hasTriedSave = false;
  public location: FinalistLocation;
  public useSuggestedAddress = false;
  public useEnteredAddress = false;
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public organization = new OrganizationInformation();
  public organizationModel = new OrganizationInformation();
  public isReadOnly = false;
  public orgInfoId = 0;
  public isAddressValidated = false;

  constructor(private appService: AppService, private orgInfoService: OrganizationInformationService) {}

  ngOnInit() {
    this.orgInfoService.modeForFinalistLocation.subscribe(res => {
      this.isReadOnly = res.readOnly;
      if (this.finalistLocation.id === 0) {
        this.newLocation();
      } else {
        //this.hasTriedSave = true;
        this.isDisabled = true;
        this.location = res.finalistLocation;
        this.isAddressValidated = true;
      }
      this.organizationModel = res.orgInfo;
      if (res.orgInfo) this.orgInfoId = res.orgInfo.id;
    });

    //Populate FinalistAddress original Model
    this.originalModel = new FinalistLocation();
    FinalistLocation.clone(this.location, this.originalModel);
  }

  /**
   * Initializes the finalistlocation
   */
  newLocation() {
    const location = new FinalistLocation();
    location.id = 0;
    this.location = location;
    this.location.finalistAddress = new FinalistAddress();
  }

  /**
   * Invoked when there is address update on FinalistAddressComponent
   */
  validateAddress(finalistAddress: FinalistAddress) {
    if (this.hasTriedSave) {
      this.validateSave();
    } else {
      if (finalistAddress) {
        this.location.finalistAddress = finalistAddress;
        this.isAddressValidated = this.location.finalistAddress.isAddressValidated;
        this.isAddressSectionModified = true;
      }
      const isValid = this.location.finalistAddress.validate(this.validationManager);
      this.isAddressSectionValid = isValid;
    }
  }

  /**
   * Invoked when there is update on End Date and EffectiveDate
   */
  validate() {
    if (this.hasTriedSave) {
      this.validateSave();
    } else {
      this.isSectionModified = true;
      const isValid = this.location.validate(this.validationManager);
      this.isSectionValid = isValid;
    }
  }

  /**
   * Invoked when there is update on End Date and EffectiveDate
   */
  validateSave() {
    this.isSectionModified = true;
    this.isAddressSectionModified = true;
    this.validationManager.resetErrors();
    const result = this.location.validateSave(this.validationManager, this.organizationModel, this.isDisabled);
    const resultAddress = this.location.finalistAddress.validateSave(this.validationManager);
    this.isSectionValid = result.isValid;
    this.isAddressSectionValid = resultAddress.isValid;
    this.modelErrors = Object.assign(resultAddress.errors, result.errors);
    if (this.isSectionValid) {
      this.hasTriedSave = false;
    }
    if (this.modelErrors) {
      this.isSaving = false;
    }
  }

  isSaveEnabled(): boolean {
    if (
      (!this.location.finalistAddress.addressLine1 || (this.location.finalistAddress.addressLine1 && this.location.finalistAddress.addressLine1.trim() === '')) &&
      (!this.location.finalistAddress.city || (this.location.finalistAddress.city && this.location.finalistAddress.city.trim() === '')) &&
      (!this.location.finalistAddress.zip || (this.location.finalistAddress.zip && this.location.finalistAddress.zip.trim() === ''))
    )
      return this.isSectionValid;
    else return this.isAddressValidated && this.isSectionValid;
  }

  /**
   * Invoked when selects Ignore changes from the save confirmation
   */
  exitFinalistLocationEditIgnoreChanges(e) {
    this.orgInfoService.modeForFinalistLocation.next({ readOnly: false, isInEditMode: false });
    this.isInEditMode.emit(false);
  }

  /**
   * Invoked when user select Address radio button is fired in the FinalistAddressComponent
   */
  setSelectedAddress(selectedAddress: any) {
    this.isAddressSectionValid = true;
    this.isAddressValidated = selectedAddress.isAddressValidated;
    this.useSuggestedAddress = selectedAddress.useSuggestedAddress;
    this.useEnteredAddress = selectedAddress.useEnteredAddress;
  }

  /**
   * Invoked when Save is clicked
   */
  save() {
    if (this.isSectionValid && this.isAddressSectionValid && this.isAddressValidated) {
      this.isSaving = true;
      this.populateOrganisationContract();
      //Invoke save service call
      this.orgInfoService.saveOrganizationInformation(this.organizationId, this.organization).subscribe(
        res => {
          this.orgInfoService.modeForFinalistLocation.next({ readOnly: false, isInEditMode: false });
          this.isSaving = false;
          this.isInEditMode.emit(false);
        },
        error => {
          this.isInEditMode.emit(true);
          this.isSaving = false;
          this.hadSaveError = true;
        }
      );
    }
  }

  /**
   * Reconstruct the Organization object
   */
  populateOrganisationContract() {
    this.organization.locations = [];
    this.organization.id = this.orgInfoId;
    this.organization.organizationId = this.organizationId;
    this.organization.programId = this.programId;

    if (this.location.id === 0) {
      this.location.finalistAddress.useEnteredAddress = this.useEnteredAddress;
      this.location.finalistAddress.useSuggestedAddress = this.useSuggestedAddress;
      this.organization.locations.push(this.location);
    } else {
      this.location.finalistAddress.useEnteredAddress = this.useEnteredAddress;
      this.location.finalistAddress.useSuggestedAddress = this.useSuggestedAddress;
      this.organization.locations.push(this.location);
    }
  }

  /**
   * Invoked when Save is clicked
   */
  saveAndExit() {
    this.hasTriedSave = true;
    this.validateSave();
    this.save();
  }

  /**
   * Invoked when Exit button is clicked from the modal popup
   */
  exit() {
    if (this.isSectionModified || this.isAddressSectionModified) {
      this.appService.isUrlChangeBlocked = false;
      this.appService.isDialogPresent = true;
    } else {
      this.orgInfoService.modeForFinalistLocation.next({ readOnly: false, isInEditMode: false });
      this.isInEditMode.emit(false);
    }
  }
}
