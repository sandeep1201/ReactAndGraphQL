// tslint:disable: no-use-before-declare
// tslint:disable: member-ordering
import { Component, forwardRef, OnInit, Input, OnChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

const noop = () => {};

export const EducationTabComponent_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => EducationTabComponent),
  multi: true
};

@Component({
  selector: 'app-education-tab',
  templateUrl: './education-tab.component.html',
  styleUrls: ['./education-tab.component.css'],
  providers: [EducationTabComponent_CONTROL_VALUE_ACCESSOR]
})
export class EducationTabComponent implements OnInit, ControlValueAccessor, OnChanges {
  constructor() {}

  ngOnInit() {}

  @Input() isInValid = false;

  private pageEducationTab: number;

  ngOnChanges() {}

  getEducationTab(tab: number) {
    this.pageEducationTab = tab;
    this.value = tab;
    this.isDiplomaSelected = false;
    this.isGEDSelected = false;
    this.isHSEDSelected = false;
    this.isNoneSelected = false;
    if (tab === 1) {
      this.isDiplomaSelected = true;
    } else if (tab === 2) {
      this.isGEDSelected = true;
    } else if (tab === 3) {
      this.isHSEDSelected = true;
    } else if (tab === 4) {
      this.isNoneSelected = true;
    }
  }

  //The internal data model
  private innerValue: any = '';
  public isDiplomaSelected: boolean;
  public isGEDSelected: boolean;
  public isHSEDSelected: boolean;
  public isNoneSelected: boolean;

  //Placeholders for the callbacks which are later providesd
  //by the Control Value Accessor
  private onTouchedCallback: () => void = noop;
  private onChangeCallback: (_: any) => void = noop;

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
      this.getEducationTab(value);
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
}
