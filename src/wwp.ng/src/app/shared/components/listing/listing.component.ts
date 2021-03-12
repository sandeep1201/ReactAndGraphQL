import { Component, forwardRef, Input, EventEmitter, ChangeDetectorRef, OnChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { BaseComponent } from '../base-component';
import { TextDetail } from '../../models/primitives';
const noop = () => {
};

export const ListingComponent_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => ListingComponent),
  multi: true
};

@Component({
  selector: 'app-listing',
  templateUrl: './listing.component.html',
  styleUrls: ['./listing.component.css'],
  providers: [ListingComponent_CONTROL_VALUE_ACCESSOR]
})
export class ListingComponent extends BaseComponent implements ControlValueAccessor, OnChanges {

  @Input() isRequired: boolean;
  @Input() errorIndicator: boolean;
  public enterPressed = false;
  public isListingFocused = false;
  public isErrored = false;

  constructor(private cdr: ChangeDetectorRef) { super(); }

  ngOnChanges() {
    if (this.errorIndicator === true) {
      this.isErrored = true;
    } else {
      this.isErrored = false;
    }
  }

  onEnter() {
    this.enterPressed = true;
    let x = new TextDetail();
    x.id = 0;
    x.details = '';
    x.isDeleted = false;
    this.value.push(x);
    this.cdr.detectChanges();
    return false;
  }

  onBackspacePress(x: TextDetail) {
    if (x.details != null && x.details.trim() === '' && this.value != null && this.value.length > 1) {
      x.isDeleted = true;
      let deletedItemIndex = this.value.indexOf(x);
      this.value.splice(deletedItemIndex, 1);
    }
  }

  get value(): TextDetail[] {
    return this.innerValue;
  }

  set value(v: TextDetail[]) {
    if (v !== this.innerValue) {
      this.innerValue = v;
      this.onChangeCallback(v);
    }
  }

  onBlur() {
    this.onTouchedCallback();
  }

  writeValue(value: TextDetail[]) {
    if (value !== this.innerValue) {
      this.innerValue = value;
    }
  }

  isFocused(value: boolean) {
    this.isListingFocused = value;
  }

  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  registerOnTouched(fn: any) {
    this.onTouchedCallback = fn;
  }

  // trackByFn(index, item: TextDetail) {
  //   return item.id;
  // }

}
