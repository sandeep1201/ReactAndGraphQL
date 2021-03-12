import { Component, forwardRef, OnInit, Input, OnChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

import { BaseComponent } from '../base-component';
import { ModelErrors } from '../../interfaces/model-errors';


const noop = () => {
};

export const YES_NO_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => YesNoComponent),
  multi: true
};

@Component({
  selector: 'app-yes-no',
  templateUrl: './yes-no.component.html',
  styleUrls: ['./yes-no.component.css'],
  providers: [YES_NO_CONTROL_VALUE_ACCESSOR],
  // directives: [NgClass]
})
export class YesNoComponent extends BaseComponent implements OnInit, ControlValueAccessor, OnChanges {

  @Input() ButtonsStates: any[] = [null, null, null];
  // Marks yesNo required when true.
  @Input() defaultRequired: boolean;


  // Input type for validation
  @Input() inputType: string;

  @Input() isRepeater: boolean = false;

  @Input() radioButtonStyle: boolean;
  @Input() isInvalid = false;

  // One for each button.
  isYes: boolean;

  isRequired: boolean;
  IsError: boolean = false;


  isDefault: boolean = true;
  constructor() { super(); }

  ngOnInit() {
  }

  ngOnChanges() {
    this.IsError = this.isInvalid;

    if (this.isRepeater !== true) {
      if (this.defaultRequired === true) {
        this.isRequired = true;
      }
    }
  }

  // Changes the background Color
  click(state: any) {

    this.value = state;
    if (state == true) {
      this.isYes = true;
    } else if (state == false) {
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
