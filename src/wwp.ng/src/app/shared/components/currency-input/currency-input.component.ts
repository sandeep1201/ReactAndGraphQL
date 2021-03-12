// tslint:disable: no-use-before-declare
import { Component, OnChanges, forwardRef, Output, EventEmitter, Input } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { BaseComponent } from '../base-component';

export const CurrencyInputComponent_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => CurrencyInputComponent),
  multi: true
};

@Component({
  selector: 'app-currency-input',
  templateUrl: './currency-input.component.html',
  styleUrls: ['./currency-input.component.css'],
  providers: [CurrencyInputComponent_CONTROL_VALUE_ACCESSOR]
})
export class CurrencyInputComponent extends BaseComponent implements OnChanges, ControlValueAccessor {
  @Input() isDisabled = false;
  @Input() isReadOnly = false;
  @Input() isRequired = false;
  @Input() NGMaxLength = 9;
  @Input() isInvalid = false;
  @Input() index: number;
  @Input() errorIndicator = false;
  @Input() errorIndicators: any[] = [];
  @Input() errorIndicatorPropertyName: string;

  @Output() isFocused = new EventEmitter<boolean>();
  @Output() isBlured = new EventEmitter<boolean>();

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

  onFocus() {
    this.isFocused.emit(true);
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

  keyup() {
    if (this.value != null && (this.value.trim() === '.' || this.value.trim() === '. 0')) {
      this.value = '';
    }
  }

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
        const model = this.errorIndicators[this.index];
        if (model != null && model[this.errorIndicatorPropertyName] === true) {
          this.isInvalid = true;
        } else {
          this.isInvalid = false;
        }
      }
    }
  }
}
