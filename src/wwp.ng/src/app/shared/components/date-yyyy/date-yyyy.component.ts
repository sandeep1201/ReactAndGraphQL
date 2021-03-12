// tslint:disable: deprecation
import { Component, forwardRef, Input } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

import { BaseComponent } from '../base-component';

@Component({
  selector: 'app-date-yyyy',
  templateUrl: './date-yyyy.component.html',
  styleUrls: ['./date-yyyy.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => DateYyyyComponent),
      multi: true
    }
  ]
})
export class DateYyyyComponent extends BaseComponent implements ControlValueAccessor {
  @Input() isDisabled = false;
  @Input() isInvalid = false;
  @Input() isRequired = false;

  constructor() {
    super();
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

  keyDown(e: KeyboardEvent) {
    // Assume we don't have an acceptable keyCode.
    let isGood = false;

    // Don't allow modifiers.
    if (!e.shiftKey && !e.altKey && !e.ctrlKey) {
      if (e.keyCode >= 48 && e.keyCode <= 57) {
        isGood = true;
      } else if (e.keyCode >= 96 && e.keyCode <= 105) {
        // num keypad on Windows
        isGood = true;
      } else if (e.keyCode === 46 || e.keyCode === 8) {
        // Backspace/Delete
        isGood = true;
      } else if (e.keyCode === 37 || e.keyCode === 39) {
        // Left/Right Arrow
        isGood = true;
      } else if (e.keyCode === 9) {
        // Tab
        isGood = true;
      }
    } else if (e.altKey && e.keyCode === 110) {
      // Numpad Delete
      isGood = true;
    } else if (e.shiftKey && e.keyCode === 9) {
      // Shift Tab
      isGood = true;
    }

    return isGood;
  }
}
