import { Component, Input, OnInit, OnChanges, OnDestroy, forwardRef, HostListener } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

import { Subscription } from 'rxjs';
import * as _ from 'lodash';

import { BaseComponent } from '../../components/base-component';
import { City } from '../../models/google-api';
import { GoogleLocation } from '../../models/google-location';
import { GoogleApiService } from '../../services/google-api.service';
import { debounceTime, distinctUntilChanged } from 'rxjs/operators';

@Component({
  selector: 'app-remote-auto-complete',
  templateUrl: './remote-auto-complete.component.html',
  styleUrls: ['./remote-auto-complete.component.css'],
  providers: [
    GoogleApiService,
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => RemoteAutoCompleteComponent),
      multi: true
    }
  ]
})
export class RemoteAutoCompleteComponent extends BaseComponent implements OnInit, OnChanges, OnDestroy, ControlValueAccessor {
  private gapiSub: Subscription;
  public isFetching = false;
  public isInvalid = false;
  private debouncedSearch: any;
  public query = '';
  public filteredList: any = [];
  public elementRef: any;
  private hasBeenClicked = false;
  hasBeenSelected = false;
  googlePlaceID: string;
  private cities: City[] = [];

  @Input() errorIndicator = false;
  @Input() isRequired = false;
  @Input() isDisabled = false;

  @Input() usCitiesOnly = false;
  @Input() wiCitiesOnly = false;

  @HostListener('document: click', ['$event.target'])
  onClick(target: HTMLElement) {
    this.removeDropDown();
  }

  constructor(private googleApiService: GoogleApiService) {
    super();
    this.debouncedSearch = _.debounce(this.search, 150, { maxWait: 10000 });
  }

  ngOnInit() {}

  get value(): GoogleLocation {
    return this.innerValue;
  }

  set value(v: GoogleLocation) {
    if (v !== this.innerValue) {
      this.innerValue = v;
      this.onChangeCallback(v);
    }
  }

  onBlur() {
    this.onTouchedCallback();
  }

  writeValue(value: GoogleLocation) {
    if (value !== this.innerValue && value != null) {
      this.query = value.description;
      this.innerValue = value;
    }
  }

  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  registerOnTouched(fn: any) {
    this.onTouchedCallback = fn;
  }

  ngOnChanges() {
    if (this.errorIndicator === true) {
      this.isInvalid = true;
    } else {
      this.isInvalid = false;
    }
  }

  blur() {
    this.hasBeenClicked = true;
  }

  filter() {
    this.debouncedSearch();
  }

  search() {
    if (this.query == null || this.query === '') {
      this.setToEmpty();
      return false;
    }
    this.innerValue = new GoogleLocation();
    this.isFetching = true;
    if (this.usCitiesOnly) {
      this.gapiSub = this.googleApiService
        .searchForUsCities(this.query)
        .pipe(debounceTime(1000), distinctUntilChanged())
        .subscribe(
          r => {
            this.filteredList = r.cities;
            this.cities = r.cities;
            this.isFetching = false;
          },
          error => {
            // TODO: show a red mark
            console.log('errored');
            this.isFetching = false;
          }
        );
    } else if (this.wiCitiesOnly) {
      this.gapiSub = this.googleApiService
        .searchForWiCities(this.query)
        .pipe(debounceTime(1000), distinctUntilChanged())
        .subscribe(
          r => {
            this.filteredList = r.cities;
            this.cities = r.cities;
            this.isFetching = false;
          },
          error => {
            // TODO: show a red mark
            console.log('errored');
            this.isFetching = false;
          }
        );
    } else {
      this.gapiSub = this.googleApiService
        .searchForCities(this.query)
        .pipe(debounceTime(1000), distinctUntilChanged())
        .subscribe(
          r => {
            this.filteredList = r.cities;
            this.cities = r.cities;
            this.isFetching = false;
          },
          error => {
            // TODO: show a red mark
            console.log('errored');
            this.isFetching = false;
          }
        );
    }
  }

  removeDropDown() {
    if (!this.isSelectedValid()) {
      this.setToEmpty();
    }
    this.filteredList = [];
  }

  setToEmpty() {
    this.query = '';
    const gl = new GoogleLocation();
    this.query = gl.description;
    this.innerValue = null;
    this.onChangeCallback(this.value);
  }

  isSelectedValid(): boolean {
    let isValid = false;
    if (this.innerValue != null && this.query === this.innerValue.description) {
      isValid = true;
    }
    for (const l of this.cities) {
      if (this.query === l.cityStateCountry) {
        isValid = true;
      }
    }
    return isValid;
  }

  select(item: City) {
    this.hasBeenSelected = true;
    this.query = item.cityStateCountry;
    this.googlePlaceID = item.placeId;
    this.filteredList = [];
    this.value.description = item.cityStateCountry;
    this.value.city = item.city;
    this.value.state = item.state;
    this.value.country = item.country;
    this.value.googlePlaceId = item.placeId;
    this.hasBeenClicked = false;
    this.onChangeCallback(this.value);
  }

  ngOnDestroy() {
    if (this.gapiSub != null) {
      this.gapiSub.unsubscribe();
    }
  }
}
