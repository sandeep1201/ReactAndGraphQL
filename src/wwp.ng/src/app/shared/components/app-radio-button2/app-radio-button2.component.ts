import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { Component, OnInit, forwardRef, Input, OnChanges } from '@angular/core';
import { BaseComponent } from '../base-component';

export interface RadioButtonItem {
  name: string;
  value: boolean;
}
export const RADIO_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => AppRadioButton2Component),
  multi: true
};

@Component({
  selector: 'app-radio-button2',
  templateUrl: './app-radio-button2.component.html',
  styleUrls: ['./app-radio-button2.component.scss'],
  providers: [RADIO_VALUE_ACCESSOR]
})
export class AppRadioButton2Component extends BaseComponent implements OnInit, ControlValueAccessor, OnChanges {
  @Input() NgLabel: string = '';
  @Input() index: number;
  @Input() errorIndicators: any[] = [];
  @Input() errorIndicatorPropertyName: string;
  @Input() isReadOnly = false;
  @Input() isDisabled = false;

  @Input() name: string;
  public isInvalid = false;
  public uniqueId: string;
  @Input() valueBinding: any;

  constructor() {
    super();
    this.uniqueId = this.generateRandomId();
  }

  ngOnInit() {}

  get value(): string | number | boolean {
    return this.innerValue;
  }
  set value(v: string | number | boolean) {
    if (v !== this.innerValue) {
      this.innerValue = v;
      this.change(v);
    }
  }

  onChange: Function;
  onTouched: Function;

  writeValue(value: string | number | boolean) {
    if (value !== this.innerValue) {
      this.innerValue = value;
    }
  }
  registerOnChange(fn: Function): void {
    this.onChange = fn;
  }

  registerOnTouched(fn: Function): void {
    this.onTouched = fn;
  }

  change(value: string | number | boolean) {
    this.innerValue = value;
    this.onChange(value);
    this.onTouched(value);
  }

  ngOnChanges() {
    if (this.errorIndicators != null && this.errorIndicators[this.index] != null) {
      if (this.errorIndicatorPropertyName == null || this.errorIndicatorPropertyName === '') {
        console.log('WARNING: errorIndicatorPropertyName has not been set, so indicating errors will not work!');
      } else {
        let model = this.errorIndicators[this.index];
        if (model[this.errorIndicatorPropertyName] === true) {
          this.isInvalid = true;
        } else {
          this.isInvalid = false;
        }
      }
    }
  }

  private generateRandomId(): string {
    let charSet = 'ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789';
    let charSetSize = 36;

    let id = '';
    for (let i = 1; i <= 8; i++) {
      let randPos = Math.floor(Math.random() * charSetSize);
      id += charSet[randPos];
    }

    return id;
  }
}
