import { Component, forwardRef, Input, OnChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

import { BaseComponent } from '../base-component';


@Component({
  selector: 'app-date-input',
  templateUrl: './date-input.component.html',
  styleUrls: ['./date-input.component.css'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => DateInputComponent),
    multi: true
  }]
})
export class DateInputComponent extends BaseComponent implements ControlValueAccessor, OnChanges {

  @Input() isRequired: boolean;
  @Input() isValidByParent: boolean;
  @Input() DOB: string;

  public isInvalid: boolean = false;


  constructor() { super(); }


  get value(): any {
    return this.innerValue;
  };


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

  private correctedInput: number = 99;
  private fix: number = 12;


  ngOnChanges() {
    if (this.isValidByParent === true) {
      this.isInvalid = true;
    } else {
      this.isInvalid = false;
    }
  }


  keyDown(e: KeyboardEvent) {

    // Assume we don't have an acceptable keyCode.
    let isGood = false;

    // Don't allow modifiers.
    if (!e.shiftKey && !e.altKey && !e.ctrlKey) {
      if (e.keyCode >= 48 && e.keyCode <= 57) {
        isGood = true;
      } else if (e.keyCode >= 96 && e.keyCode <= 105) {
        isGood = true;
      } else if (e.keyCode === 46 || e.keyCode === 8) {   // Backspace/Delete
        isGood = true;
      } else if (e.keyCode === 37 || e.keyCode === 39) {   // Left/Right Arrow
        isGood = true;
      } else if (e.keyCode === 9) {   // Tab
        isGood = true;
      }
    } else if (e.altKey && e.keyCode === 110) { // Numpad Delete
      isGood = true;
    } else if (e.shiftKey && e.keyCode === 9) { // Shift Tab
      isGood = true;
    }

    if (isGood) {
      if (isNaN(this.correctedInput) === false) {
        this.correctedInput = this.correctedInput;
      } else {
        this.correctedInput = null;
      }
    } else {
      return false;
    }


  }

  validateInputBlur() {

    let _d = new Date();
    if (this.DOB != null && this.innerValue !== '' && this.innerValue != null) {
      if (this.innerValue.length >= 1) {
        let _dob = this.DOB.split('-');
        let _dobYear = Number(_dob[0]);
        if (Number(this.innerValue) <= _dobYear || Number(this.innerValue) > _d.getFullYear()) {
          this.isInvalid = true;
        } else {
          this.isInvalid = false;
        }
      } else {
        this.isInvalid = false;
      }
    } else {
      this.isInvalid = false;
    }
  }

  validateInput() {


    let _d = new Date();

    if (this.DOB != null && this.innerValue !== '' && this.innerValue != null) {
      if (this.innerValue.length === 4) {
        let _dob = this.DOB.split('-');
        let _dobYear = Number(_dob[0]);
        if (Number(this.innerValue) <= _dobYear || Number(this.innerValue) > _d.getFullYear()) {
          this.isInvalid = true;
        } else {
          this.isInvalid = false;
        }
      } else {
        this.isInvalid = false;
      }
    } else {
      this.isInvalid = false;
    }

  }

}
