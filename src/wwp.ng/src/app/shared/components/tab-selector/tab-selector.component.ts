import { DropDownMultiField } from '../../models/dropdown-multi-field';
import { Component, forwardRef, OnInit, Input, OnChanges } from '@angular/core';
import { NgClass } from '@angular/common';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { BaseComponent } from '../base-component';
import { DropDownField } from '../../models/dropdown-field';

const noop = () => {};

export const TabSelectorComponent_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => TabSelectorComponent),
  multi: true
};

@Component({
  selector: 'app-tab-selector',
  templateUrl: './tab-selector.component.html',
  styleUrls: ['./tab-selector.component.css'],
  providers: [TabSelectorComponent_CONTROL_VALUE_ACCESSOR]
})
export class TabSelectorComponent extends BaseComponent implements ControlValueAccessor, OnChanges {
  @Input() tabs: DropDownField[] | DropDownMultiField[];
  @Input() isInValid = false;
  @Input() isRequired = false;
  @Input() isDisabled = false;
  @Input() isBorderRequired = true;

  private currentTab: number;

  constructor() {
    super();
  }

  ngOnChanges() {}

  isSelected(tab: number) {
    if (this.currentTab === tab) {
      return true;
    } else {
      return false;
    }
  }

  setTab(tabId: number, isClick: boolean) {
    if (this.isDisabled === true && isClick) {
      return;
    }
    this.currentTab = tabId;
    this.value = tabId;
  }

  isTabDisabled(tab: DropDownField | DropDownMultiField) {
    if (tab instanceof DropDownField) {
      return this.isDisabled;
    } else {
      return tab.isDisabled || this.isDisabled;
    }
  }

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
      this.setTab(value, false);
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
