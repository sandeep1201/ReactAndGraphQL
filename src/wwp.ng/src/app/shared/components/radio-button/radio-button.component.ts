import { Component, OnInit, Input, forwardRef, OnChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { BaseComponent } from '../base-component';

const noop = () => {};

export const RADIO_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => RadioButtonComponent),
  multi: true
};

@Component({
  selector: 'app-radio-button',
  templateUrl: './radio-button.component.html',
  styleUrls: ['./radio-button.component.css'],
  providers: [RADIO_CONTROL_VALUE_ACCESSOR]
})
export class RadioButtonComponent extends BaseComponent implements OnInit, ControlValueAccessor, OnChanges {
  @Input() NgLabel: string;
  @Input() index: number;
  @Input() errorIndicators: any[] = [];
  @Input() errorIndicatorPropertyName: string;
  @Input() isDisabled: boolean = false;

  @Input() isButtonStyled: boolean = false;
  @Input() icon = '';
  @Input() isReadOnly = false;

  @Input() scaleButton = false;

  constructor() {
    super();
    this.uniqueId = this.generateRandomId();
  }

  ngOnInit() {}

  // The internal data
  public isInvalid: boolean = false;
  public uniqueId: string;

  //get accessor
  get value(): any {
    return this.innerValue;
  }

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
