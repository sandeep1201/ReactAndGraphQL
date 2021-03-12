// tslint:disable: no-use-before-declare
import { Component, forwardRef, Input, OnChanges, OnInit } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

import { BaseComponent } from '../base-component';

export const GENTEXTINPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => GenericTextInputComponent),
  multi: true
};

@Component({
  selector: 'app-generic-text-input',
  templateUrl: './generic-text-input.component.html',
  styleUrls: ['./generic-text-input.component.css'],
  providers: [GENTEXTINPUT_CONTROL_VALUE_ACCESSOR]
})
export class GenericTextInputComponent extends BaseComponent implements ControlValueAccessor, OnChanges, OnInit {
  @Input() index = 0;
  @Input() isDisabled = false;
  @Input() isInvalid = false;
  @Input() isValidByParent: boolean;
  @Input() ngPlaceholder: string;
  @Input() maskType: string; // Can be 'phone' or left unset (which makes it a generic text input).

  // TODO: lowercase input property names. -- actually get rid of these
  @Input() IsValidByLicenseParentRepeater: any[];
  @Input() isRequired = false;
  @Input() isValidbyDegreeParentRepeater: any[];
  @Input() NGMaxLength: number;
  @Input() NGName: string;
  @Input() textLength: number;
  @Input() isTextLengthGiven = false;
  @Input() isBorderRequired = true;

  public isError = false;
  public isInputNearLimit = false;

  constructor() {
    super();
  }

  ngOnInit() {}

  ngOnChanges() {
    if (this.isValidbyDegreeParentRepeater != null && this.isValidbyDegreeParentRepeater[this.index] != null) {
      // let model = this.isValidbyDegreeParentRepeater[this.index];
      // if (model.name === true) {
      //   this.isError = true;
      // } else {
      //   this.isError = false;
      // }
      console.warn('Remove!!!!');
    }

    if (this.IsValidByLicenseParentRepeater != null && this.IsValidByLicenseParentRepeater[this.index] != null) {
      // let model = this.IsValidByLicenseParentRepeater[this.index];
      // if (model.name === true && this.NGName === 'name') {
      //   this.isError = true;
      // } else if (this.NGName === 'name') {
      //   this.isError = false;
      // }
      // if (model.issuer === true && this.NGName === 'issuer') {
      //   this.isError = true;
      // } else if (this.NGName === 'issuer') {
      //   this.isError = false;
      // }
      console.warn('Remove!!!!');
    }

    if (this.isInvalid != null && this.isInvalid === true) {
      this.isError = true;
    } else {
      this.isError = false;
    }
  }

  charWarning(textLength: number) {
    this.NGMaxLength - textLength < 20 ? (this.isInputNearLimit = true) : (this.isInputNearLimit = false);
    return this.isInputNearLimit;
  }

  get value(): any {
    return this.innerValue;
  }

  set value(v: any) {
    if (v !== this.innerValue) {
      this.innerValue = v;
      this.onChangeCallback(v);
    }
  }

  onBlur() {
    this.onTouchedCallback();
  }

  writeValue(value: any) {
    if (value !== this.innerValue) {
      this.innerValue = value;
    }
  }

  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  registerOnTouched(fn: any) {
    this.onTouchedCallback = fn;
  }
}
