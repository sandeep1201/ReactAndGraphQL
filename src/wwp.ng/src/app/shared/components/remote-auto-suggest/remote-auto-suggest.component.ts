import { Component, Input, OnInit, OnDestroy, forwardRef, OnChanges, HostListener, Output, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { Subscription } from 'rxjs';
import * as _ from 'lodash';

import { BaseComponent } from '../base-component';
import { GoogleApiService } from '../../services/google-api.service';
import { School, StreetAddress } from '../../models/google-api';

const noop = () => {
};

export const REMOTE_AUTOSUGGEST_INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => RemoteAutoSuggestComponent),
  multi: true
};

@Component({
  selector: 'app-remote-auto-suggest',
  templateUrl: './remote-auto-suggest.component.html',
  styleUrls: ['./remote-auto-suggest.component.css'],
  providers: [REMOTE_AUTOSUGGEST_INPUT_CONTROL_VALUE_ACCESSOR, GoogleApiService]
})
export class RemoteAutoSuggestComponent extends BaseComponent implements OnInit, OnDestroy, OnChanges, ControlValueAccessor {

  @Input() isInvalidbyParent: boolean = false;
  @Input() isRequired: boolean;
  @Input() googlePlaceIdInput: string;
  @Input() searchType: string;
  @Input() isDisabled = false;
  @Input() index = 0;
  @Input() isValidbyCollegeParentRepeater: any[];
  @Output() googlePlaceId = new EventEmitter<string>();

  private results: any[] = [];
  private debouncedSearch: any;
  private gapiSub: Subscription;
  private gglePlaceId: string = '';
  public isDisabledInput: boolean = true;
  public isInvalid: boolean = false;
  public isFetching: boolean = false;

  // public query = '';
  public filteredList: any = [];
  public elementRef: any;

  @HostListener('document: click', ['$event.target'])
  onClick(target: HTMLElement) {
    this.removeDropDown();
  }

  constructor(private googleApiService: GoogleApiService) {
    super();
    this.debouncedSearch = _.debounce(this.search, 200, { 'maxWait': 10000 });
  }

  ngOnInit() {
  }

  ngOnDestroy() {
    if (this.gapiSub != null) {
      this.gapiSub.unsubscribe();
    }
  }

  // TODO: Clean Up this method by removing unneeded checks.
  ngOnChanges() {

    if (this.isInvalidbyParent === true) {
      this.isInvalid = true;
    } else {
      this.isInvalid = false;
    }

    this.gglePlaceId = this.googlePlaceIdInput;
    if (this.gglePlaceId == null || this.isDisabled === true) {
      if (this.isDisabled !== true) {
        this.writeValue('');
      }
      this.isDisabledInput = true;
    } else {
      this.isDisabledInput = false;
    }

  }

  // Get accessor
  get value(): any {
    return this.innerValue;
  };

  // Set accessor including call the onchange callback
  set value(v: any) {
    if (v !== this.innerValue) {
      this.innerValue = v;
      this.onChangeCallback(v);
    }
  }

  writeValue(value: any) {
    if (value !== this.innerValue) {
      this.innerValue = value;
    }
  }

  // From ControlValueAccessor interface.
  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  // From ControlValueAccessor interface.
  registerOnTouched(fn: any) {
    this.onTouchedCallback = fn;
  }

  search() {
    if (this.value == null || this.value.trim() === '') {
      this.removeDropDown();
      return false;
    }
    const lookupString = JSON.parse(JSON.stringify(this.value));
    // this.value = this.query.toString();
    this.isFetching = true;

    if (this.searchType === 'streetAddresses') {
      this.gapiSub = this.googleApiService.searchForAddresses(lookupString, this.gglePlaceId).subscribe(response => {
        this.results = response.streetAddresses;
        this.isFetching = false;
        if (this.innerValue !== '') {
          this.filteredList = this.results;
        }
      },
        error => {
          console.warn('Google Street Addresses Search Errored');
          this.isFetching = false;
        });
    }

    if (this.searchType === 'streetAddresses') {
      this.gapiSub = this.googleApiService.searchForAddresses(lookupString, this.gglePlaceId).subscribe(
        response => {
          this.results = response.streetAddresses;
          this.isFetching = false;
          if (this.innerValue !== '') {
            this.filteredList = this.results;
          }
        },
        error => {
          console.warn('Google Street Addresses Search Errored');
          this.isFetching = false;
        }
      );
    }
    if (this.searchType === 'colleges') {
      this.gapiSub = this.googleApiService.searchForColleges(lookupString, this.gglePlaceId).subscribe(response => {
        this.isFetching = false;
        if (this.innerValue !== '') {
          this.filteredList = response.schools;
        }
      },
        error => {
          console.warn('Google Colleges Search Errored');
          this.isFetching = false;
        });
    }

    if (this.searchType === 'schools') {
      this.gapiSub = this.googleApiService.searchForSchools(lookupString, this.gglePlaceId).subscribe(response => {
        this.results = response.schools;
        this.isFetching = false;
        if (this.innerValue !== '') {
          this.filteredList = this.results;
        }
      },
        error => {
          console.warn('Google Schools Search Errored');
          this.isFetching = false;
        });
    }
  }
  filter() {
    this.debouncedSearch();
  }

  removeDropDown() {
    this.filteredList = [];
  }

  selectSchool(item: School) {
    this.filteredList = [];
    this.value = item.name;
  }

  selectAddress(item: StreetAddress) {
    this.value = item.mainAddress;
    this.filteredList = [];
    this.googlePlaceId.emit(item.placeId);
  }

}
