// tslint:disable: deprecation
// tslint:disable: no-use-before-declare
import { Utilities } from './../../utilities';
import { BaseComponent } from '../../components/base-component';
import { Component, forwardRef, Input, OnChanges, DoCheck, ElementRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

// import { ChildrenError, TeenError } from '../../models/child-care-section-error';

import * as moment from 'moment';

const noop = () => {};

export const DateMmDdYyyyComponent_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => DateMmDdYyyyComponent),
  multi: true
};

@Component({
  selector: 'app-date-mm-dd-yyyy',
  templateUrl: './date-mm-dd-yyyy.component.html',
  styleUrls: ['./date-mm-dd-yyyy.component.css'],
  providers: [DateMmDdYyyyComponent_CONTROL_VALUE_ACCESSOR]
})
export class DateMmDdYyyyComponent extends BaseComponent implements ControlValueAccessor, OnChanges, DoCheck {
  // TODO: Clean Up and make more generic as we go. Currently Cleaning this up as I work on Work History.
  private correctedInput: number;
  @Input() isInvalid = false;
  @Input() isDisabled = false;
  @Input() isReadOnly = false;
  @Input() isRequired = false;
  @Input() datePickerRequired = false;

  @Input() allowOnlySundays = false;
  @Input() minDate: moment.Moment;
  @Input() maxDate: moment.Moment;
  @Input() minYearsFromToday: number;
  @Input() maxYearsFromToday: number;
  @Input() participantDob: string;
  @Input() index: number;
  @Input() validateNotPastParticpantDobPlus120 = false;
  @Input() validateNotInPast = false;
  @Input() useIsoFormat = false;
  @Input() forceIsDirty = false;
  @Input() isBorderRequired = true;
  public _isInvalid = false;
  public _forceIsDirty = false;

  constructor(private el: ElementRef) {
    super();
  }

  get value(): any {
    return this.innerValue;
  }

  // Set accessor including call the onchange callback
  set value(v: any) {
    if (v !== this.innerValue) {
      if (this.useIsoFormat === true) {
        this.innerValue = v;
        if (v != null && v.length === 10 && moment(v).isValid()) {
          this.onChangeCallback(moment(v).toISOString());
        } else {
          this.onChangeCallback(v);
        }
      } else if (this.useIsoFormat === false) {
        if (v && v.length > 10 && moment(v).isValid()) {
          v = moment(v).format('MM/DD/YYYY');
          if (moment(v).isSame(this.innerValue)) {
            return;
          } else {
            this.innerValue = v;
            this.onChangeCallback(v);
          }
        } else {
          this.innerValue = v;
          this.onChangeCallback(v);
        }
      }
    }
  }

  ngOnChanges() {
    if (this.isInvalid != null && this.isInvalid === true) {
      this._isInvalid = true;
    } else {
      this._isInvalid = false;
    }
  }

  ngDoCheck() {
    if (this.forceIsDirty != null && this.forceIsDirty === true) {
      this._forceIsDirty = true;
    } else {
      this._forceIsDirty = false;
    }
  }

  // Set touched on blur
  onBlur() {
    this.onTouchedCallback();
  }

  // From ControlValueAccessor interface
  writeValue(value: any) {
    if (value !== this.innerValue) {
      if (this.useIsoFormat === true) {
        if (value != null) {
          this.innerValue = moment(value).format('MM/DD/YYYY');
        } else {
          this.innerValue = value;
        }
      } else if (this.useIsoFormat === false) {
        this.innerValue = value;
      }
    }
  }

  // From ControlValueAccessor interface
  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  // From ControlValueAccessor interface
  registerOnTouched(fn: any) {
    this.onTouchedCallback = fn;
  }

  keyDown(e: KeyboardEvent) {
    // Dont set value on read only
    if (this.isReadOnly) {
      return;
    }

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
      } else if ((e.keyCode === 191 || e.keyCode === 111) && (this.value.length === 1 || this.value.length === 2 || this.value.length === 4 || this.value.length === 5)) {
        // forward slash  and divide
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
      const countOfSegments = this.value.split('/').length;
      if ((e.keyCode === 191 || e.keyCode === 111) && countOfSegments >= 3) {
        return false;
      }

      if (
        (this.value != null && this.value.length === 1 && e.keyCode >= 48 && e.keyCode <= 57) ||
        (e.keyCode >= 96 && e.keyCode <= 105 && this.value != null && this.value.length === 1)
      ) {
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
        if (this.value === '0') {
          return false;
        } else {
          this.value = '0' + this.value;
        }
      }

      // Look at the case where we have 1 character and they type a second /.
      // if the value is 0, this is invalid... don't allow it.
      if (this.value != null && this.value.length === 4 && (e.keyCode === 191 || e.keyCode === 111)) {
        if (this.value === '0') {
          return false;
        } else {
          this.value = [this.value.slice(0, 3), '0', this.value.slice(3)].join('');
        }
      }
    }
  }

  // validateInputBlur() {
  //   // On blur, if the parent is telling us the control/value is invalid then we
  //   // will honor what they tell us.
  //   if (this.isValidByParent != null) {
  //     this.isInvalid = !this.isValidByParent;
  //   } else {
  //     // Only vaidate when value is set.
  //     if (this.innerValue != null && this.innerValue !== '') {
  //       if (this.innerValue.length !== 10 || this.innerValue.indexOf('/') !== 2 || this.innerValue.lastIndexOf('/') !== 5
  //         || (this.innerValue.match(/[/]/g) || []).length !== 2) {
  //         this.isInvalid = true;
  //       } else {

  //         // If input is valid lets check it aganist the max date input if its set.
  //         let inputDate = moment(this.innerValue, 'MM/DD/YYYY');
  //         if (inputDate.isValid()) {
  //           if (this.maxDate != null && this.maxDate < inputDate) {
  //             this.isInvalid = true;
  //           } else {
  //             this.isInvalid = false;
  //           }
  //         }
  //       }
  //     } else {
  //       this.isInvalid = false;
  //     }
  //   }
  // }

  validateInput(e: KeyboardEvent) {
    if (this.innerValue != null && this.innerValue !== '') {
      if (this.innerValue.length === 10 && this.innerValue.indexOf('/') === 2 && this.innerValue.lastIndexOf('/') === 5 && (this.innerValue.match(/[/]/g) || []).length === 2) {
        const inputDate = moment(this.innerValue, 'MM/DD/YYYY');

        if (inputDate.isValid()) {
          // Assume it's valid... just look for invalid states.
          this.isInvalid = false;

          // TODO: This could be simplified.
          if (this.minYearsFromToday != null) {
            const minDate = moment(Utilities.currentDate).subtract(this.minYearsFromToday, 'years');
            if (inputDate.isAfter(minDate)) {
              // We can't have a date before the min.
              this.isInvalid = true;
            }
          }

          if (this.maxYearsFromToday != null) {
            const maxDate = moment(Utilities.currentDate).subtract(this.maxYearsFromToday, 'years');
            if (inputDate.isBefore(maxDate)) {
              // We can't have a date before the min.
              this.isInvalid = true;
            }
          }

          if (this.minDate != null && this.minDate > inputDate) {
            this.isInvalid = true;
          }

          if (this.maxDate != null && this.maxDate < inputDate) {
            this.isInvalid = true;
          }

          if (this.validateNotInPast === true) {
            const today = moment(Utilities.currentDate);
            if (today.isAfter(inputDate, 'day')) {
              // We can't have a date before today
              this.isInvalid = true;
            }
          }

          if (this.validateNotPastParticpantDobPlus120 === true) {
            if (this.participantDob == null || this.participantDob === '') {
              console.warn('WARNING: participantDob has not been set, so indicating errors will not work!');
            } else {
              const dob = moment(this.participantDob);
              if (dob.isValid()) {
                dob.add(120, 'years'); // this gives us the day after their birthdate
                dob.subtract(1, 'day'); // we'll adjust by 1 day to handle edge case
                if (inputDate.isAfter(dob)) {
                  // We can't have a date before today
                  this.isInvalid = true;
                }
              }
            }
          }
        } else {
          this.isInvalid = true;
        }
      } else {
        // this.isInvalid = true;
      }
    } else {
      this.isInvalid = false;
    }

    if (
      (this.value != null && this.value !== '' && this.value.length === 2 && e.keyCode >= 48 && e.keyCode <= 57) ||
      (e.keyCode >= 96 && e.keyCode <= 105 && this.value != null && this.value.length === 2)
    ) {
      if (this.value.search('/') === -1) {
        this.value = this.value + '/';
      }
    }

    if (
      (this.value != null && this.value !== '' && this.value.length === 5 && e.keyCode >= 48 && e.keyCode <= 57) ||
      (e.keyCode >= 96 && e.keyCode <= 105 && this.value != null && this.value.length === 5)
    ) {
      if (this.value.search('/') === 2) {
        this.value = this.value + '/';
      }
    }
  }
  public modelChanged(e) {
    console.log(e);
  }
}
