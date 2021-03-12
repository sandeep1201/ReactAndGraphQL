import { Component, forwardRef, Input, OnChanges, OnInit } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

import { BaseComponent } from '../base-component';


export const PhoneNumberControlValueAccessor: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => PhoneNumberInputComponent),
  multi: true
};

@Component({
  selector: 'app-phone-number-input',
  templateUrl: './phone-number-input.component.html',
  styleUrls: ['./phone-number-input.component.css'],
  providers: [PhoneNumberControlValueAccessor]
})
export class PhoneNumberInputComponent extends BaseComponent implements ControlValueAccessor, OnChanges, OnInit {

  @Input() index: number = 0;
  @Input() isDisabled: boolean = false;
  @Input() isInvalid: boolean = false;
  @Input() placeholder: string;

  @Input() isRequired: boolean;
  @Input() maxLength: number;

  constructor() {
    super();
  }

  ngOnInit() {
  }

  ngOnChanges() {
  }

  get value(): any {
    return this.innerValue;
  };


  set value(v: any) {
    if (v !== this.innerValue) {
      this.innerValue = this.cleanseValue(v);
      this.onChangeCallback(this.innerValue);
    }
  }

  cleanseValue(value: string): string {
    if (!value) {
      return '';
    }

    if (value === '') {
      return value;
    }

    let cleansed = value.replace(/[A-z]/g, '').trim();
    if (cleansed === '') {
      cleansed = ' ';     // Hack for not being able to clean it to an empty string.
    }

    return cleansed;
  }

  onBlur() {
    this.onTouchedCallback();
  }

  writeValue(value: any) {
    if (value !== this.innerValue) {
      this.innerValue = this.cleanseValue(value);
    }
  }


  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }


  registerOnTouched(fn: any) {
    this.onTouchedCallback = fn;
  }


  keyDown(e: KeyboardEvent): boolean {
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
      } else if (e.keyCode === 189 || e.keyCode === 109 || e.keyCode === 173) {   // Dash
        isGood = true;
      }
    } else if (e.shiftKey && e.keyCode === 9) { // Shift Tab
      isGood = true;
    } else if (e.ctrlKey && e.keyCode === 65) { // ctrl + A
      isGood = true;
    } else if (e.ctrlKey && e.keyCode === 67) { // ctrl + C
      isGood = true;
    } else if (e.ctrlKey && e.keyCode === 88) { // ctrl + X
      isGood = true;
    } else if (e.ctrlKey && e.keyCode === 86) { // ctrl + V
      isGood = true;
    } else if ((e.shiftKey && e.keyCode == 57) || (e.shiftKey && e.keyCode == 48)) { // ()
      isGood = true;
    } else if ((e.shiftKey && e.keyCode == 37) || (e.shiftKey && e.keyCode == 39)) { // Shit + left or right
      isGood = true;
    } else if (e.ctrlKey && e.keyCode === 90) { // ctrl + Z
      isGood = true;
    }

    if (isGood) {
      return true;
    }

    // Now look for key modifieres that are allowed.
    if (e.ctrlKey || e.metaKey || e.altKey) {
      return true;
    }

    return false;
  }

  validateInput(e: KeyboardEvent) {
  }

}
