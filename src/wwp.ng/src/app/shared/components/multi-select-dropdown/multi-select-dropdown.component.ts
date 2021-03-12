// tslint:disable: no-use-before-declare
// tslint:disable: no-output-on-prefix
/*
 * Angular 2 Dropdown Multiselect for Bootstrap
 * Current version: 0.3.2
 *
 * Simon Lindh
 * https://github.com/softsimon/angular-2-dropdown-multiselect
 */

import {
  // NgModule,
  Component,
  OnInit,
  DoCheck,
  OnChanges,
  HostListener,
  Input,
  ElementRef,
  Output,
  EventEmitter,
  forwardRef,
  IterableDiffers
} from '@angular/core';
// import { CommonModule } from '@angular/common';
import { FormsModule, NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

import { BaseComponent } from '../base-component';

const MULTISELECT_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => MultiSelectDropdownComponent),
  multi: true
};

export interface IMultiSelectOption {
  id: number;
  name: string;
  isDisabled: boolean;
  isPermanentlyDisabled?: boolean;
  description?: string;
  disablesOthers: boolean;
  disabledIds?: number[];
}

export interface IMultiSelectSettings {
  pullRight?: boolean;
  enableSearch?: boolean;
  checkedStyle?: 'checkboxes' | 'glyphicon';
  buttonClasses?: string;
  selectionLimit?: number;
  closeOnSelect?: boolean;
  autoUnselect?: boolean;
  showCheckAll?: boolean;
  showUncheckAll?: boolean;
  dynamicTitleMaxItems?: number;
  maxHeight?: string;
}

export interface IMultiSelectTexts {
  checkAll?: string;
  uncheckAll?: string;
  checked?: string;
  checkedPlural?: string;
  searchPlaceholder?: string;
  defaultTitle?: string;
}

@Component({
  selector: 'app-multi-select-dropdown',
  templateUrl: './multi-select-dropdown.component.html',
  styleUrls: ['./multi-select-dropdown.component.css'],
  providers: [MULTISELECT_VALUE_ACCESSOR]
})
export class MultiSelectDropdownComponent extends BaseComponent implements OnInit, DoCheck, OnChanges, ControlValueAccessor {
  mySettings: IMultiSelectSettings = {
    pullRight: false,
    enableSearch: false,
    checkedStyle: 'checkboxes',
    buttonClasses: 'btn btn-default',
    selectionLimit: 0,
    closeOnSelect: false,
    showCheckAll: false,
    showUncheckAll: false,
    dynamicTitleMaxItems: 3,
    maxHeight: '300px'
  };

  private myTexts: IMultiSelectTexts = {
    checkAll: 'Check all',
    uncheckAll: 'Uncheck all',
    checked: 'checked',
    checkedPlural: 'checked',
    searchPlaceholder: 'Search...',
    defaultTitle: 'Select'
  };

  @Input() isValueLocked = false; // Disabled CSS but with no indenting.
  @Input() placeholder: string;
  @Input() isInvalid = false;
  @Input() isReadOnly = false;
  @Input() isFilter = false;
  @Input() options: Array<IMultiSelectOption>;

  // We can override otherwise it will take mysettings.
  @Input() settings: IMultiSelectSettings = this.mySettings;
  texts: IMultiSelectTexts = this.myTexts;
  @Input() isRequired: boolean;
  @Input() isDisabled: boolean;
  @Input() isHistorical = false;
  @Input() customWidth = 0;

  @Input() isAdvRulesMulti = false;
  // @Input() defaultLabel: string;
  @Output() selectionLimitReached = new EventEmitter();
  @Output() dropdownClosed = new EventEmitter();
  @Output() onReverseSort = new EventEmitter();
  private isDirty = false;
  public width = 25;
  public isFilterAsc = false;
  cachedOptions: Array<IMultiSelectOption> = [];
  // innerValue: number[];
  title: string;
  differ: any;
  numSelected = 0;
  isVisible = false;
  searchFilterText = '';
  @Input() icon = 'fa-filter';
  defaultSettings: IMultiSelectSettings = {
    pullRight: false,
    enableSearch: false,
    checkedStyle: 'checkboxes',
    buttonClasses: 'btn btn-default',
    selectionLimit: 0,
    closeOnSelect: false,
    autoUnselect: false,
    showCheckAll: false,
    showUncheckAll: false,
    dynamicTitleMaxItems: 3,
    maxHeight: '300px'
  };
  defaultTexts: IMultiSelectTexts = {
    checkAll: 'Check all',
    uncheckAll: 'Uncheck all',
    checked: 'checked',
    checkedPlural: 'checked',
    searchPlaceholder: 'Search...',
    defaultTitle: 'Multi-select'
  };

  constructor(private element: ElementRef, private differs: IterableDiffers) {
    super();
    this.differ = differs.find([]).create(null);
  }

  onModelChange: Function = (_: any) => {};
  onModelTouched: Function = () => {};

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
      this.isVisible = false;
    }
  }
  @HostListener('document: dragstart', ['$event'])
  ondrag(e) {
    e.preventDefault();
    return;
  }

  ngOnInit() {
    this.settings = Object.assign(this.defaultSettings, this.settings);
    this.texts = Object.assign(this.defaultTexts, this.texts);
    if (this.placeholder != null) {
      this.title = this.placeholder;
      this.texts.defaultTitle = this.placeholder;
    } else {
      this.title = this.texts.defaultTitle;
    }

    if (this.isHistorical) {
      this.settings.dynamicTitleMaxItems = 9999;
    }
  }

  writeValue(value: any): void {
    if (value != null) {
      this.innerValue = value;
    } else {
      this.innerValue = [];
    }
  }

  registerOnChange(fn: Function): void {
    this.onModelChange = fn;
  }

  registerOnTouched(fn: Function): void {
    this.onModelTouched = fn;
  }

  ngDoCheck() {
    const changes = this.differ.diff(this.innerValue);
    if (changes) {
      this.updateNumSelected();
      this.updateTitle();
    }
  }

  ngOnChanges() {
    if (JSON.stringify(this.options) !== JSON.stringify(this.cachedOptions)) {
      if (this.options != null) {
        this.cachedOptions = Array.from(this.options);
      }
    }

    this.updateNumSelected();
    this.updateTitle();
    this.calculateWidth();
  }

  calculateWidth() {
    if (this.options != null) {
      let longestWidth = this.texts.defaultTitle.length;
      for (const o of this.options) {
        if (o.name.length > longestWidth) {
          longestWidth = o.name.length;
        }
      }
      if (longestWidth < 24) {
        longestWidth = 24;
      }
      longestWidth = longestWidth * 0.75;
      this.width = this.customWidth && this.customWidth !== 0 ? this.customWidth : longestWidth;
    }
  }

  toggleFilter() {
    this.isFilterAsc = !this.isFilterAsc;
    this.onReverseSort.emit();
  }

  toggleDropdown() {
    if (this.isDisabled) {
      return false;
    }
    this.isVisible = !this.isVisible;
    if (!this.isVisible) {
      this.dropdownClosed.emit();
    }
  }

  isSelected(option: IMultiSelectOption): boolean {
    return this.innerValue && this.innerValue.indexOf(option.id) > -1;
  }

  setSelected(event: Event, option: IMultiSelectOption) {
    if (!this.innerValue) {
      this.innerValue = [];
    }

    if (this.isAdvRulesMulti && option.isDisabled) {
      return;
    }
    const index = this.innerValue.indexOf(option.id);

    // Already was selected.
    if (index > -1) {
      this.innerValue.splice(index, 1);
      if (option.disablesOthers === true && this.isAdvRulesMulti === false) {
        this.innerValue = [];
        for (const x of this.options) {
          x.isDisabled = false;
        }
      }

      // When unselected lets switch the disabled flag to false for any that were disbaled by the option.
      if (option.disabledIds != null && this.isAdvRulesMulti === false) {
        for (const id of option.disabledIds) {
          const foundObj = this.options.find(x => x.id === id);
          if (foundObj != null) {
            foundObj.isDisabled = false;
          }
        }
      } else if (this.isAdvRulesMulti === true) {
        for (const id of option.disabledIds) {
          const foundObj = this.options.find(x => x.id === id);
          if (foundObj != null) {
            foundObj.isDisabled = false;
          }
        }
        // isPermanentlyDisabled
      }
    } else {
      if (this.settings.selectionLimit === 0 || this.innerValue.length < this.settings.selectionLimit) {
        // Null innerValue when option disables others.

        if (option.disablesOthers === true && this.isAdvRulesMulti === false) {
          this.innerValue = [];
        }

        // disable others if they have ids.
        if (option.disabledIds != null && this.isAdvRulesMulti === false) {
          for (const id of option.disabledIds) {
            const foundObj = this.options.find(x => x.id === id);
            if (foundObj != null) {
              foundObj.isDisabled = true;
            }
          }
        }

        // Lets remove all disablesOthers that are true but keep the false ones;
        if (option.disablesOthers === false && this.isAdvRulesMulti === false) {
          for (const o of this.options) {
            if (o.disablesOthers === true) {
              for (const m of this.innerValue) {
                if (o.id === m) {
                  const i = this.innerValue.indexOf(m);
                  this.innerValue.splice(i, 1);
                }
              }
            }
          }
        }

        // disable others if they have ids.
        if (option.disabledIds != null && option.isDisabled !== true && option.isPermanentlyDisabled !== true && this.isAdvRulesMulti === true) {
          for (const id of option.disabledIds) {
            const foundObj = this.options.find(x => x.id === id);
            if (foundObj != null) {
              const i = this.innerValue.indexOf(foundObj.id);
              if (i > -1) {
                this.innerValue.splice(i, 1);
              }
              foundObj.isDisabled = true;
            }
          }
        }

        if (option.isDisabled !== true && !this.isAdvRulesMulti) {
          this.innerValue.push(option.id);
        }

        if (option.isDisabled !== true && option.isPermanentlyDisabled !== true && this.isAdvRulesMulti) {
          this.innerValue.push(option.id);
        }
      } else {
        if (this.settings.autoUnselect) {
          this.innerValue.push(option.id);
          this.innerValue.shift();
        } else {
          this.selectionLimitReached.emit(this.innerValue.length);
          return;
        }
      }
    }
    if (this.settings.closeOnSelect) {
      this.toggleDropdown();
    }
    this.onModelChange(this.innerValue);
  }

  updateNumSelected() {
    this.numSelected = (this.innerValue && this.innerValue.length) || 0;
  }

  updateTitle() {
    if (this.numSelected === 0) {
      this.title = this.texts.defaultTitle;
    } else if (this.settings.dynamicTitleMaxItems >= this.numSelected) {
      if (this.options != null) {
        this.title = this.options
          .filter((option: IMultiSelectOption) => this.innerValue && this.innerValue.indexOf(option.id) > -1)
          .map((option: IMultiSelectOption) => option.name)
          .join(', ');
      }
    } else {
      this.title = this.numSelected + ' ' + (this.numSelected === 1 ? this.texts.checked : this.texts.checkedPlural);
    }
  }

  checkAll() {
    this.innerValue = this.options.map(option => option.id);
    this.onModelChange(this.innerValue);
  }

  uncheckAll() {
    this.innerValue = [];
    this.onModelChange(this.innerValue);
  }
}
