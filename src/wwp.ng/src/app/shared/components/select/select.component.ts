import { Component, forwardRef, OnInit, Input, OnChanges, Output, EventEmitter, HostListener, ElementRef } from '@angular/core';
import { BaseComponent } from '../../components/base-component';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

import { DropDownField } from '../../models/dropdown-field';
import { WorkProgramError } from '../../models/work-programs-section-error';
// import * as _ from 'lodash';

const noop = () => {};

export const SELECT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => SelectComponent),
  multi: true
};

@Component({
  selector: 'app-select',
  templateUrl: './select.component.html',
  styleUrls: ['./select.component.css'],
  providers: [SELECT_CONTROL_VALUE_ACCESSOR]
})
export class SelectComponent extends BaseComponent implements ControlValueAccessor, OnChanges {
  public localDropList: DropDownField[];
  public _isInvalid: boolean;
  @Input() index: number;
  @Input() isRequired = false;
  @Input() isInvalid = false;
  @Input() isCompact = false;
  @Input() isDisabled = false;
  @Input() isReadOnly = false;
  @Input() isDisabledWithNoValue = false;
  @Input() isFormControl = true;
  @Input() isFilter = false;
  @Input() isIdString = false;
  @Input() hasEmptyOption = true;

  // We cant set the model because we have to simulate a click thus we have defaultValueId.
  @Input() defaultValueId = 0;
  @Input() icon = 'fa-filter';
  @Input() isResizable = false;
  @Input() floatRight = true;

  @Input()
  set dropDownList(dropDownList: DropDownField[]) {
    this.localDropList = dropDownList;
    if (this.isFilter === true && this.defaultValueId > 0) {
      // const found = _.find(this.localDropList, ['id', this.defaultValueId]);
      const found = this.localDropList.find(x => x.id == this.defaultValueId);
      if (found != null) {
        this.select(found);
      }
    }
  }

  @Output() onReverseSort = new EventEmitter();

  public isFilterAsc = false;
  public showOptions = false;
  public sortName = '';

  constructor(private element: ElementRef) {
    super();
  }

  get value(): any {
    return this.innerValue;
  }

  set value(v: any) {
    if (v !== this.innerValue) {
      if (this.isIdString === false) {
        if (v == null || v === '') {
          this.innerValue = null;
          this.onChangeCallback(null);
        } else {
          this.innerValue = +v;
          this.onChangeCallback(+v);
        }
      } else {
        this.innerValue = v;
        this.onChangeCallback(v);
      }
    }
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

  toggleFilter() {
    this.isFilterAsc = !this.isFilterAsc;
    this.onReverseSort.emit();
  }

  // Used for non HTML 5 selects.
  toggleSortMenu() {
    this.showOptions = !this.showOptions;
  }

  select(item: DropDownField) {
    this.sortName = '';
    this.sortName = item.name;
    this.value = item.id;
    this.showOptions = false;
  }

  ngOnChanges() {
    if (this.isInvalid === true) {
      this._isInvalid = true;
    } else {
      this._isInvalid = false;
    }
  }

  @HostListener('document: click', ['$event.target'])
  onClick(target: HTMLElement) {
    let parentFound = false;
    while (target != null && !parentFound) {
      if (target === this.element.nativeElement) {
        parentFound = true;
      }
      target = target.parentElement;
    }
    if (!parentFound) {
      this.showOptions = false;
    }
  }
}
