import { Utilities } from 'src/app/shared/utilities';
import { Component, Input, forwardRef, OnChanges, SimpleChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { BaseComponent } from '../base-component';

@Component({
  selector: 'app-text-area',
  templateUrl: './text-area.component.html',
  styleUrls: ['./text-area.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => TextAreaComponent),
      multi: true
    }
  ]
})
export class TextAreaComponent extends BaseComponent implements ControlValueAccessor, OnChanges {
  @Input() isRequired = false;
  @Input() isReadOnly = false;
  @Input() isDetails = false;
  @Input() isInvalid = false;
  @Input() isAutoSizeable = false;
  @Input() maxlength = 1000;
  @Input() placeholder = 'Notes';
  @Input() autoResize: boolean;
  @Input() shrinkOnBlur: boolean;
  @Input() isDisabled = false;
  @Input() textLength: number;
  @Input() isTextLengthGiven = false;
  @Input() restrictCharactersForEA = false;
  @Input() height: string;
  @Input() doNotShowRemainigCharacters = false;
  private isDirty = false;
  private wasOriginalNull = false;
  public lengthOfString = 0;
  public isNearLimit = false;
  public isInputNearLimit = false;

  constructor() {
    super();
  }

  // get accessor
  get value(): any {
    return this.innerValue;
  }

  // set accessor including call the onchange callback
  set value(v: any) {
    if (v !== this.innerValue) {
      this.innerValue = v;
      // We do this for an IE 11 issue with placeholders triggering key events.
      if (this.isDirty === true) {
        if (v === '' && this.wasOriginalNull === true) {
          this.innerValue = null;
          this.onChangeCallback(null);
        } else {
          this.onChangeCallback(v);
        }
        //  this.onChangeCallback(v);
      }
      this.countCharacters(v);
    }
  }

  // Set touched on blur
  onBlur() {
    this.onTouchedCallback();
  }

  // From ControlValueAccessor interface
  writeValue(value: any) {
    if (value == null) {
      this.wasOriginalNull = true;
      this.innerValue = null;
    } else {
      this.wasOriginalNull = false;
      this.innerValue = value;
    }
    if (value != null) {
      this.countCharacters(value);
    }

    // }
  }

  // From ControlValueAccessor interface
  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  // From ControlValueAccessor interface
  registerOnTouched(fn: any) {
    this.onTouchedCallback = fn;
  }

  countCharacters(text: string) {
    if (text != null) {
      const lengthOfString = text.length;
      this.lengthOfString = lengthOfString;
      this.lengthOfString > 980 ? (this.isNearLimit = true) : (this.isNearLimit = false);
    }
  }

  charWarning(textLength: number) {
    this.maxlength - textLength < 20 ? (this.isInputNearLimit = true) : (this.isInputNearLimit = false);
    return this.isInputNearLimit;
  }

  inputNearLimit() {}

  // Mark Dirty on keyup because IE11 marks dirty on load with placeholder.
  keypress() {
    this.isDirty = true;
  }

  stripRestrictedCharactersForEA() {
    if (this.restrictCharactersForEA && !Utilities.isStringEmptyOrNull(this.value)) {
      this.value = this.value.replace(/\n|\t/g, ''); //strip enter and tab space
      this.value = this.value.replace(/([^\u0020-\u007E\u00A1-\u00FF])/g, ''); //strip other restricted characters
    }
  }

  // Implemented onChanges interface to detect isDisabled state and null the innerValue. This effects the frontend display only. You must clear the data on the backend.
  ngOnChanges(changes: SimpleChanges): void {
    if (changes['isDisabled']) {
      if (this.isDisabled === true) {
        this.innerValue = null;
      }
    }
  }
}
