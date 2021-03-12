import { Component, forwardRef, Input, OnChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

import { BaseComponent } from '../base-component';
import { ModelErrors } from '../../interfaces/model-errors';

export const YesNoBasic_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => YesNoBasicComponent),
  multi: true
};

@Component({
  selector: 'app-yes-no-basic',
  templateUrl: './yes-no-basic.component.html',
  styleUrls: ['./yes-no-basic.component.css'],
  providers: [YesNoBasic_CONTROL_VALUE_ACCESSOR]
})
export class YesNoBasicComponent extends BaseComponent implements ControlValueAccessor, OnChanges {

  @Input() isRequired: boolean = false;
  @Input() isDisabled: boolean = false;
  @Input() isRepeater: boolean = false;
  @Input() index: number = 0;
  @Input() isReadOnly: boolean = false;

  @Input() errorIndicator: boolean = false;

  @Input() radioButtonStyle: boolean;

  // Save Validation state.
  @Input() isValidByParent: boolean;

  // Save Validation state for repeaters.
  @Input() isValidByParentRepeater: ModelErrors[];
  @Input() isValidbyCollegeParentRepeater: any[];

  // One for each button.
  @Input()
  isYes: boolean;
  IsError: boolean = false;
  isDefault: boolean = true;

  constructor() { super(); }

  ngOnChanges() {
    if (this.errorIndicator != null && this.errorIndicator === true) {
      this.IsError = true;
    } else {
      this.IsError = false;
    }

    if (this.isValidByParent === true) {
      console.warn('isValidByParent is deprecated. On View Change isValidByParent to errorIndicator');
    }

    if (this.isValidbyCollegeParentRepeater != null && this.isValidbyCollegeParentRepeater[this.index] != null) {
      // let model = this.isValidbyCollegeParentRepeater[this.index];
      // if (model.hasGraduated === true) {
      //   this.IsError = true;
      // } else {
      //   this.IsError = false;
      // }
      console.warn('Remove!!!!');
    }
  }

  // Changes the background Color
  click(state: any) {
    this.value = state;
    if (state === true) {
      this.isYes = true;
    } else if (state === false) {
      this.isYes = false;
    } else if (state == null) {
      this.isYes = null;
    }
  }

  // Get accessor.
  get value(): any {
    return this.innerValue;
  };

  // Set accessor including call the onchange callback.
  set value(v: any) {
    if (v !== this.innerValue) {
      this.innerValue = v;
      this.onChangeCallback(v);
    }
  }

  // Set touched on blur.
  onBlur() {
    this.onTouchedCallback();
  }

  // From ControlValueAccessor interface.
  writeValue(value: any) {
    if (value !== this.innerValue) {
      this.innerValue = value;
      this.click(this.innerValue);
    }
  }

  // From ControlValueAccessor interface.
  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  // From ControlValueAccessor interface.
  registerOnTouched(fn: any) {
    this.onTouchedCallback = fn;
  }

}
