import { Component, Input, OnChanges, forwardRef, HostListener, ElementRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

import { BaseComponent } from '../base-component';
import { DropDownField } from '../../models/dropdown-field';
import { ModelErrors } from '../../interfaces/model-errors';

export const AUTO_COMPLETE_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => AutoCompleteComponent),
  multi: true
};

@Component({
  selector: 'app-auto-complete',
  templateUrl: './auto-complete.component.html',
  styleUrls: ['./auto-complete.component.css'],
  providers: [AUTO_COMPLETE_CONTROL_VALUE_ACCESSOR]
})
export class AutoCompleteComponent extends BaseComponent implements OnChanges, ControlValueAccessor {
  // Save Validation state for repeaters.
  @Input() isValidByParentRepeater: ModelErrors[];

  // Save Validation state.
  @Input() isValidByParent: boolean;

  @Input() isShortHeightStyle = false;
  @Input() placeHolder = '';
  @Input() isRepeater = false;
  @Input() index = 0;

  // Marks autocomplete required when true.
  @Input() defaultRequired: Boolean;
  @Input() errorIndicators: any[] = [];
  @Input() errorIndicatorPropertyName: string;
  @Input() isRequired = false;
  @Input() isDisabled = false;
  // This is the dropdown list for the autocomplete.
  @Input()
  set dropDownList(dropDownList: DropDownField[]) {
    this.localDropList = dropDownList;
  }

  private localDropList: DropDownField[];
  private idSelected: number;
  public isDefaultCss = true;
  public isErrorCss: boolean;
  private clonedint: number;
  private clonedQuery: string = null;
  public query = '';
  public filteredList: any = [];
  public elementRef: any;

  @HostListener('document:click', ['$event'])
  clickout(event) {
    if (this.eRef.nativeElement.contains(event.target)) {
      // Nothing for inside click.
    } else {
      this.removeDropDown();
    }
  }

  constructor(private eRef: ElementRef) {
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

  onBlur() {
    this.onTouchedCallback();
  }

  writeValue(value: any) {
    if (value !== this.innerValue) {
      this.innerValue = value;
      this.idSelected = value;

      if (value != null) {
        this.start(value);
      } else {
        this.query = null;
        this.idSelected = null;
        this.resetSelectedInDropDown();
      }
    }
  }

  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  registerOnTouched(fn: any) {
    this.onTouchedCallback = fn;
  }

  ngOnChanges() {
    if (this.innerValue != null || this.innerValue !== undefined) {
      this.start(this.innerValue);
    }

    this.isErrorCss = this.isValidByParent;
  }

  // Logic to bind langId to Lang object
  start(value: number) {
    if (value != null && this.localDropList != null) {
      for (const item of this.localDropList) {
        if (item.id == value) {
          this.select(item);
        }
      }
    }
  }

  filter() {
    // Removes Previous selected flag.
    if (this.idSelected != null) {
      for (const l of this.localDropList) {
        if (l.id === this.idSelected) {
          l.isSelected = null;
        }
      }
    }

    // Clears Id values when input is changed
    if (this.clonedQuery !== this.query) {
      this.idSelected = null;
      this.value = null;
    }

    if (this.query !== '') {
      this.filteredList = this.localDropList.filter(
        function(el: any) {
          const isMatching = el.name.toLowerCase().indexOf(this.query.toLowerCase()) > -1;
          // If the name matches exactly set that as selected
          // (if there are two or more exact same names in the dropdown, the last one's id will be selected)
          if (isMatching && el.name === this.query) {
            el.isSelected = true;
            this.idSelected = el.id;
            this.value = el.id;
          }
          return isMatching;
        }.bind(this)
      );
      // Check to see if query is equal to drop down list item
    } else {
      // this.filteredList = this.localDropList;
    }
  }

  showDropDown() {
    if (this.isDisabled) {
      return;
    }
    this.filteredList = this.localDropList;
  }

  removeDropDown() {
    let found = false;
    if (this.localDropList != null) {
      for (const l of this.localDropList) {
        if (this.query === l.name) {
          found = true;
        }
      }
      if (found === false) {
        this.query = '';
      }
      this.filteredList = [];
    }
  }

  select(item: any) {
    this.query = item.name;
    this.idSelected = item.id;
    this.filteredList = [];
    this.value = item.id;

    for (const l of this.localDropList) {
      if (l.id === this.clonedint) {
        l.isSelected = null;
      }
    }

    for (const l of this.localDropList) {
      if (l.id === item.id) {
        this.resetSelectedInDropDown();
        l.isSelected = true;
      }
    }
    this.clonedint = item.id;
  }

  resetSelectedInDropDown() {
    if (this.localDropList != null) {
      for (const dd of this.localDropList) {
        dd.isSelected = false;
      }
    }
  }
}
