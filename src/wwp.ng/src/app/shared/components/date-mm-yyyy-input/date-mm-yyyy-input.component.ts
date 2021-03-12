import { AppService } from 'src/app/core/services/app.service';
import { Timeline } from '../../models/time-limits/timeline';
import { Component, OnInit, forwardRef, Input, OnChanges, DoCheck } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { NgClass } from '@angular/common';
import * as moment from 'moment';
import { BaseComponent } from '../../components/base-component';

import { WorkProgramError } from '../../models/work-programs-section-error';

const noop = () => {
};

export const DATE_MMYYY_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => DateMmYyyyInputComponent),
  multi: true
};

@Component({
  selector: 'app-date-mm-yyyy-input',
  templateUrl: './date-mm-yyyy-input.component.html',
  styleUrls: ['./date-mm-yyyy-input.component.css'],
  providers: [DATE_MMYYY_CONTROL_VALUE_ACCESSOR]
})
export class DateMmYyyyInputComponent extends BaseComponent implements OnInit, ControlValueAccessor, OnChanges, DoCheck {

  @Input() isDisabled = false;
  @Input() isRequired = false;
  @Input() index = 0;
  @Input() Name: string;
  @Input() minDateRange: moment.Moment;
  @Input() errorIndicator: boolean;
  @Input() errorIndicators: any[] = [];
  @Input() errorIndicatorPropertyName: string;
  @Input() isValidByParent;
  @Input() forceIsDirty = false;
  public _forceIsDirty = false;
  constructor() { super(); }

  ngOnInit() {
  }

  public isInvalid = false;

  //get accessor
  get value(): any {
    return this.innerValue;
  };

  //set accessor including call the onchange callback
  set value(v: any) {
    if (v !== this.innerValue) {
      this.innerValue = v;
      this.onChangeCallback(v);
    }
  }

  //Set touched on blur
  onBlur() {
    this.onTouchedCallback();
  }

  //From ControlValueAccessor interface
  writeValue(value: any) {
    if (value !== this.innerValue) {
      this.innerValue = value;
    }
  }

  //From ControlValueAccessor interface
  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  //From ControlValueAccessor interface
  registerOnTouched(fn: any) {
    this.onTouchedCallback = fn;
  }

  private correctedInput = 99;
  private fix = 12;


  ngOnChanges() {

    if (this.errorIndicator === true) {
      this.isInvalid = true;
    } else {
      this.isInvalid = false;
    }


    if (this.errorIndicators != null && this.errorIndicators[this.index] != null) {
      if (this.errorIndicatorPropertyName == null || this.errorIndicatorPropertyName === '') {
        console.warn('WARNING: errorIndicatorPropertyName has not been set, so indicating errors will not work!');
      } else {
        let model = this.errorIndicators[this.index];
        if (model != null && model[this.errorIndicatorPropertyName] === true) {
          this.isInvalid = true;
        } else {
          this.isInvalid = false;
        }
      }
    }

    if (this.isValidByParent != null) {
      this.isInvalid = !this.isValidByParent;
    }
  }

  ngDoCheck() {
    if (this.forceIsDirty != null && this.forceIsDirty === true) {
      this._forceIsDirty = true;
    } else {
      this._forceIsDirty = false;
    }
  }

  keyDown(e: KeyboardEvent) {

    // Assume we don't have an acceptable keyCode.
    let isGood = false;

    // Don't allow modifiers.
    if (!e.shiftKey && !e.altKey && !e.ctrlKey) {
      if (e.keyCode >= 48 && e.keyCode <= 57) {
        isGood = true;
      } else if (e.keyCode >= 96 && e.keyCode <= 105) {   // num keypad on Windows
        isGood = true;
      } else if (e.keyCode === 46 || e.keyCode === 8) {   // Backspace/Delete
        isGood = true;
      } else if (e.keyCode === 37 || e.keyCode === 39) {   // Left/Right Arrow
        isGood = true;
      } else if (e.keyCode === 9) {   // Tab
        isGood = true;
      } else if ((e.keyCode === 191 || e.keyCode === 111) && (this.value.length === 1 || this.value.length === 2)) {   // forward slash  and divide
        isGood = true;
      }
    } else if (e.altKey && e.keyCode === 110) { // Numpad Delete
      isGood = true;
    } else if (e.shiftKey && e.keyCode === 9) { // Shift Tab
      isGood = true;
    }

    if (isGood) {
      if (isNaN(this.correctedInput) === false) {
        //  this.correctedInput = this.correctedInput;
      } else {
        this.correctedInput = null;
      }
    } else {
      return false;
    }

    if ((e.keyCode === 191 || e.keyCode === 111) && this.value.search('/') !== -1) {
      return false;
    }

    if (this.value != null && this.value.length === 1 && e.keyCode >= 48 && e.keyCode <= 57
      || e.keyCode >= 96 && e.keyCode <= 105 && this.value != null && this.value.length === 1) {
      if (this.value.search('/') === -1) {
        let charCode = e.keyCode;
        if (e.keyCode >= 96) {
          charCode = e.keyCode - 48;
        }
        this.value = this.value + String.fromCharCode(charCode) + '/';
        // Since we're handling the keystroke, cancel it by returning false.
        return false;
      }
    }

    // Look at the case where we have 1 character and they type /.
    // if the value is 0, this is invalid... don't allow it.
    if (this.value != null && this.value.length === 1 && (e.keyCode === 191 || e.keyCode === 111)) {
      if (this.value == '0') {
        return false;
      } else {
        this.value = '0' + this.value;
      }
    }
  }
}
