import { FieldDataTypes } from 'src/app/shared/enums/field-data-types.enum';
import { FieldDataService } from './../../services/field-data.service';
import { Component, Input, Output, OnInit, EventEmitter, OnChanges, SimpleChanges } from '@angular/core';
import { FinalistAddressService } from '../../services/finalist-address.service';
import { FinalistAddress } from '../../models/finalist-address.model';
import { ModelErrors } from '../../interfaces/model-errors';
import { ValidationManager } from '../../models/validation';
import { AppService } from '../../../core/services/app.service';
import { Utilities } from '../../utilities';
import { DropDownField } from '../../models/dropdown-field';
import { take } from 'rxjs/operators';
import { isEmpty } from 'lodash';

@Component({
  selector: 'app-finalist-address',
  templateUrl: './finalist-address.component.html',
  styleUrls: ['./finalist-address.component.css']
})
export class FinalistAddressComponent implements OnInit, OnChanges {
  @Input() locationAddress: FinalistAddress;
  @Input() cachedInnerValue: FinalistAddress = new FinalistAddress();
  @Input() isReadOnly = false;
  @Input() isDisplayed = false;
  @Input() isRequired = true;
  @Input() reValidationRequired = false;
  @Input() modelErrors: ModelErrors = {};
  @Input() additionalValidationErrorParameter = '';
  @Input() isWIDefault = true;
  @Input() addressHeader = 'Address';
  @Input() addressLine1GridSize = 'col-md-6';

  //Event --modelChange: Emits model changes in the Address section
  //Event --selectedAddress: Emits event when selectedAddress checkbox is selected in the Address section

  @Output() modelChange = new EventEmitter();
  @Output() selectedAddress = new EventEmitter<any>();
  @Output() validateButtonStatus = new EventEmitter<boolean>();
  //@Output() finalistAddress = new EventEmitter<any>();

  public userEnteredAddressLine1: string;
  public userEnteredZip: string;
  public userEnteredState: string;
  public userEnteredCity: string;
  public useSuggestedAddress = false;
  public useEnteredAddress = false;
  public isAddressEntered = false;

  public originalModel = new FinalistAddress();
  public isAddressDisabled = false;
  public isAddressValidated = false;

  public addressIsValid: boolean;
  public errorMsg: string[];
  public resubmitAddress = false;
  public finalistAddressLoaded = false;
  public addressForFinalist: FinalistAddress;

  public suggestedAddressFromFinalist: string;
  public activeStateID: number;
  public cachedStateID: number;
  public statesDrop: DropDownField[] = [];
  //public isHouseHoldAddrValidated = false;

  public validationManager: ValidationManager = new ValidationManager(this.appService);

  constructor(private appService: AppService, private finalistAddressService: FinalistAddressService, private fdService: FieldDataService) {}

  ngOnInit() {
    if (!this.locationAddress) {
      this.locationAddress = new FinalistAddress();
    }
    isEmpty(this.cachedInnerValue) ? FinalistAddress.clone(this.locationAddress, this.originalModel) : FinalistAddress.clone(this.cachedInnerValue, this.originalModel);
    if (!this.isWIDefault) {
      this.fdService
        .getFieldDataByField(FieldDataTypes.States)
        .pipe(take(1))
        .subscribe(x => {
          this.statesDrop = x;
          if (this.locationAddress.state) this.activeStateID = +Utilities.fieldDataIdByCode(this.locationAddress.state, x);
          if (this.originalModel.state) this.cachedStateID = +Utilities.fieldDataIdByCode(this.originalModel.state, x);
        });
    } else {
      this.locationAddress.state = 'WI';
      this.originalModel.state = 'WI';
    }
    this.isAddressEntered =
      Utilities.lowerCaseTrimAsNotNull(this.locationAddress.addressLine1) !== Utilities.lowerCaseTrimAsNotNull(this.originalModel.addressLine1) ||
      Utilities.lowerCaseTrimAsNotNull(this.locationAddress.city) !== Utilities.lowerCaseTrimAsNotNull(this.originalModel.city) ||
      Utilities.lowerCaseTrimAsNotNull(this.locationAddress.state) !== Utilities.lowerCaseTrimAsNotNull(this.originalModel.state) ||
      Utilities.lowerCaseTrimAsNotNull(this.locationAddress.zip) !== Utilities.lowerCaseTrimAsNotNull(this.originalModel.zip);
    this.validateButtonStatus.emit(this.isValidateDisabled);
  }

  ngOnChanges(changes: SimpleChanges) {
    if (this.cachedInnerValue && changes && changes.cachedInnerValue && changes.cachedInnerValue.currentValue) {
      FinalistAddress.clone(this.cachedInnerValue, this.originalModel);
      this.cachedStateID = this.cachedInnerValue.state ? +Utilities.fieldDataIdByCode(this.cachedInnerValue.state, this.statesDrop) : null;
    }
    if (
      changes &&
      changes.reValidationRequired &&
      (changes.reValidationRequired.previousValue !== changes.reValidationRequired.currentValue || !!changes.reValidationRequired.currentValue)
    ) {
      this.isAddressDisabled = false;
      this.useSuggestedAddress = false;
      this.useEnteredAddress = false;
      this.resubmitAddress = false;
      this.isAddressValidated = false;
      this.finalistAddressLoaded = false;
      this.errorMsg = [];
      this.validateButtonStatus.emit(this.isValidateDisabled);
    }
  }

  setActiveStateCode() {
    this.locationAddress.state = Utilities.fieldDataCodeById(this.activeStateID, this.statesDrop);
  }

  /**
   * Invoked when User clicks suggested address checkbox
   */
  validateUserClickSuggested() {
    if (this.useSuggestedAddress) {
      this.resubmitAddress = false;
      this.useEnteredAddress = false;
      this.isAddressDisabled = true;
      const suggestedAddress = this.suggestedAddressFromFinalist.split(', ');
      if (this.isAddressValidated) {
        this.locationAddress.addressLine1 = suggestedAddress[0];
        this.locationAddress.city = suggestedAddress[1];
        this.locationAddress.state = suggestedAddress[2].split(' ')[0];
        this.locationAddress.zip = suggestedAddress[2].split(' ')[1];
      }
    } else this.isAddressDisabled = false;
    this.modelChange.emit(this.locationAddress);
    this.validateButtonStatus.emit(this.isValidateDisabled);
  }

  /**
   * Invoked when User clicks resubmit address checkbox
   */
  validateUserClickResubmit() {
    if (this.resubmitAddress) {
      this.useSuggestedAddress = false;
      this.useEnteredAddress = false;
      this.isAddressDisabled = false;
      this.finalistAddressLoaded = false;
      if (this.isAddressValidated) {
        this.locationAddress.addressLine1 = this.userEnteredAddressLine1;
        this.locationAddress.city = this.userEnteredCity;
        this.locationAddress.state = this.userEnteredState;
        this.locationAddress.zip = this.userEnteredZip;
      }
    } else this.useEnteredAddress = false;
    this.validateButtonStatus.emit(this.isValidateDisabled);
  }

  /**
   * Invoked when User clicks entered checkbox
   */
  validateUserClickEntered() {
    if (this.useEnteredAddress) {
      //this.selectedAddress.emit({useEnteredAddress: true, useSuggestedAddress: false});
      this.useSuggestedAddress = false;
      this.resubmitAddress = false;
      this.isAddressDisabled = true;
      if (this.isAddressValidated) {
        this.locationAddress.addressLine1 = this.userEnteredAddressLine1;
        this.locationAddress.city = this.userEnteredCity;
        this.locationAddress.state = this.userEnteredState;
        this.locationAddress.zip = this.userEnteredZip;
      }
    } else this.isAddressDisabled = false;
    this.modelChange.emit(this.locationAddress);
    this.validateButtonStatus.emit(this.isValidateDisabled);
  }

  /**
   * Invoked when User clicks Validate Address button
   */
  validateAddressFromFinalist() {
    if (this.locationAddress.addressLine1 !== null && this.locationAddress.city !== null && this.locationAddress.state !== null && this.locationAddress.zip !== null) {
      this.addressForFinalist = this.locationAddress;
      this.isAddressValidated = false;
    }

    this.finalistAddressService.getFinalistAddress(this.addressForFinalist).subscribe(res => {
      this.suggestedAddressFromFinalist = res.fullAddress;
      this.addressIsValid = res.isValid;
      this.isAddressValidated = true;
      this.errorMsg = res.errorMsg;
      this.useSuggestedAddress = false;
      this.useEnteredAddress = false;
      this.resubmitAddress = false;
      this.finalistAddressLoaded = true;
      if (this.errorMsg.length > 0) {
        this.suggestedAddressFromFinalist = this.errorMsg.join('\n');
      }
      //this.finalistAddress.emit(this.locationAddress);
      this.validateButtonStatus.emit(this.isValidateDisabled);
      this.modelChange.emit(this.locationAddress);
      // We'll store the address when the user enters validate button because the address can change until the user hits Save.
      if (this.isAddressValidated) {
        this.userEnteredAddressLine1 = this.locationAddress.addressLine1;
        this.userEnteredZip = this.locationAddress.zip;
        this.userEnteredCity = this.locationAddress.city;
        this.userEnteredState = this.locationAddress.state;
      }
    });
  }

  public validateFinalistAddress() {
    if (!this.useSuggestedAddress && !this.useEnteredAddress) {
      this.isAddressValidated = false;
    } else {
      this.isAddressValidated = true;
    }
    this.selectedAddress.emit({ useEnteredAddress: false, useSuggestedAddress: true, isAddressValidated: this.isAddressValidated });
  }

  /**
   * Invoked when there is change in Address section
   */
  registerModelChange(fn: any) {
    this.isAddressEntered =
      Utilities.lowerCaseTrimAsNotNull(this.locationAddress.addressLine1) !== Utilities.lowerCaseTrimAsNotNull(this.originalModel.addressLine1) ||
      Utilities.lowerCaseTrimAsNotNull(this.locationAddress.city) !== Utilities.lowerCaseTrimAsNotNull(this.originalModel.city) ||
      Utilities.lowerCaseTrimAsNotNull(this.locationAddress.state) !== Utilities.lowerCaseTrimAsNotNull(this.originalModel.state) ||
      Utilities.lowerCaseTrimAsNotNull(this.locationAddress.zip) !== Utilities.lowerCaseTrimAsNotNull(this.originalModel.zip);
    this.locationAddress.isAddressValidated = this.isAddressValidated;
    this.validateButtonStatus.emit(this.isValidateDisabled);
    this.modelChange.emit(this.locationAddress);
  }

  get isValidateDisabled(): boolean {
    return this.isAddressDisabled || !this.isAddressEntered;
  }
}
