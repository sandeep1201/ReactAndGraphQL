import { Component, forwardRef, Input } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';


import { BaseComponent } from '../base-component';

@Component({
  selector: 'app-date-mm-dd-yyyy-new',
  templateUrl: './date-mm-dd-yyyy-new.component.html',
  styleUrls: ['./date-mm-dd-yyyy-new.component.css'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => DateMmDdYyyyNewComponent),
    multi: true
  }]

})
export class DateMmDdYyyyNewComponent extends BaseComponent implements ControlValueAccessor {

  private correctedInput: number;

  @Input() isDisabled = false;
  @Input() isReadOnly = false;
  @Input() isInvalid = false;
  @Input() isRequired = false;
  @Input() maxLength: number = null;
  @Input() placeholder: string;
  @Input() datePickerRequired = false;


  constructor() {
    super();
  }

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
      } else if ((e.keyCode === 191 || e.keyCode === 111) && (this.value.length === 1
        || this.value.length === 2 || this.value.length === 4 || this.value.length === 5)) {   // forward slash  and divide
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

    // If we have a slash char, ignore it once we have 2.  We'll
    // use the split function since it's fast but it returns the
    // count of sections in between / characters:
    //  1 = no slashes
    //  2 = 1 slash
    //  3 = 2 slashes
    if (this.value != null) {
      let countOfSegments = this.value.split('/').length;
      if ((e.keyCode === 191 || e.keyCode === 111) && countOfSegments >= 3) {
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

      // Look at the case where we have 1 character and they type a second /.
      // if the value is 0, this is invalid... don't allow it.
      if (this.value != null && this.value.length === 4 && (e.keyCode === 191 || e.keyCode === 111)) {
        if (this.value == '0') {
          return false;
        } else {
          this.value = [this.value.slice(0, 3), '0', this.value.slice(3)].join('');
        }
      }
    }
  }

  keyUp(e: KeyboardEvent) {
    if (this.value != null && this.value !== '' && this.value.length === 2 && e.keyCode >= 48 && e.keyCode <= 57
      || e.keyCode >= 96 && e.keyCode <= 105 && this.value != null && this.value.length === 2) {
      if (this.value.search('/') === -1) {
        this.value = this.value + '/';
      }
    }

    if (this.value != null && this.value !== '' && this.value.length === 5 && e.keyCode >= 48 && e.keyCode <= 57
      || e.keyCode >= 96 && e.keyCode <= 105 && this.value != null && this.value.length === 5) {
      if (this.value.search('/') === 2) {
        this.value = this.value + '/';
      }
    }
  }
}
