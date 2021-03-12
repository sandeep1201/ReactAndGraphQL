// tslint:disable: no-use-before-declare
// tslint:disable: deprecation
import { Component, forwardRef, Input, OnChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { BaseComponent } from '../base-component';
const noop = () => {};

export const NUMERICAL_INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => NumericalInputComponent),
  multi: true
};

@Component({
  selector: 'app-numerical-input',
  templateUrl: './numerical-input.component.html',
  styleUrls: ['./numerical-input.component.css'],
  providers: [NUMERICAL_INPUT_CONTROL_VALUE_ACCESSOR]
})
export class NumericalInputComponent extends BaseComponent implements ControlValueAccessor, OnChanges {
  @Input() DOB = '';
  @Input() index: number;
  // TODO: get rid of this... make generic
  //  @Input() isValidByParentRepeater: PostSecondaryDegreeError[] = [];
  @Input() isValidByParentRepeater: any[] = [];
  @Input() NGMaxLength: number;
  @Input() NGPlaceHolder = '';
  @Input() isInvalid = false;
  @Input() isDisabled = false;
  @Input() isReadOnly = false;
  @Input() isRequired: boolean;
  @Input() maxValue: number;
  @Input() allowDash: boolean;
  @Input() allowPeriod: boolean;
  @Input() isSSN = false;
  @Input() isPhoneNumber = false;
  @Input() isFEIN = false;
  @Input() isMoney = false;
  @Input() isHrs = false;
  @Input() isBorderRequired = true;
  public maxLength = this.NGMaxLength;

  ngOnChanges() {
    if (this.isValidByParentRepeater != null && this.isValidByParentRepeater[this.index] != null) {
      const model = this.isValidByParentRepeater[this.index];
      if (model.yearAttained === true) {
        this.isInvalid = true;
      } else {
        this.isInvalid = false;
      }
    }
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

  blur() {
    if (this.maxValue != null) {
      if (Number(this.maxValue) < this.value) {
        this.value = this.maxValue;
      }
    }
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

  validate() {
    if (this.DOB !== '') {
      const partDOB = new Date(this.DOB);
      const currentDate = new Date();

      this.isInvalid = false;
      if (this.value != null && this.value.length === 4) {
        if (this.value < partDOB.getFullYear()) {
          this.isInvalid = true;
        }

        if (this.value > currentDate.getFullYear()) {
          this.isInvalid = true;
        }
      }
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
      } else if (e.keyCode === 46 || e.keyCode === 8) {
        // Backspace/Delete
        isGood = true;
      } else if (e.keyCode === 37 || e.keyCode === 39) {
        // Left/Right Arrow
        isGood = true;
      } else if (e.keyCode === 9) {
        // Tab
        isGood = true;
      } else if ((e.keyCode === 189 || e.keyCode === 109 || e.keyCode === 173) && this.allowDash === true) {
        // Dash
        isGood = true;
      } else if ((e.keyCode === 190 || e.keyCode === 110) && this.allowPeriod === true && !this.hasDecimalPointBeenAdded()) {
        // Period for decimals.
        isGood = true;
      }
    } else if (e.altKey && e.keyCode === 110) {
      // Numpad Delete
      isGood = true;
    } else if (e.shiftKey && e.keyCode === 9) {
      // Shift Tab
      isGood = true;
    } else if (e.ctrlKey && e.keyCode === 65) {
      // ctrl + A
      isGood = true;
    } else if (e.ctrlKey && e.keyCode === 67) {
      // ctrl + C
      isGood = true;
    } else if (e.ctrlKey && e.keyCode === 88) {
      // ctrl + X
      isGood = true;
    } else if (e.ctrlKey && e.keyCode === 86) {
      // ctrl + V
      isGood = true;
    }

    if (!isGood) {
      return false;
    }
  }

  private hasDecimalPointBeenAdded(): boolean {
    if (this.value != null) {
      const valueString = this.value.toString();
      if (valueString.indexOf('.') > -1) {
        return true;
      }
    }
    return false;
  }

  public formatHrDecimal(val: string) {
    if (val === '.') val = '0.';

    if (val.includes('.')) {
      const index = val.indexOf('.') + 1;
      if (this.NGMaxLength > index) this.maxLength = index + 1;
    } else this.maxLength = this.NGMaxLength;
    return val;
  }
}
