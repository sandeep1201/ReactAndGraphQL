import { Component, OnInit, forwardRef, Input, OnChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { BaseComponent } from '../base-component';
import { YesNoStatus } from '../../models/primitives';
import { DropDownField } from '../../models/dropdown-field';
import { Utilities } from '../../utilities';

@Component({
  selector: 'app-yes-no-refuse',
  templateUrl: './yes-no-refuse.component.html',
  styleUrls: ['./yes-no-refuse.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => YesNoRefuseComponent),
      multi: true
    }
  ]
})
export class YesNoRefuseComponent extends BaseComponent implements OnInit, OnChanges, ControlValueAccessor {
  isYes: boolean;
  isError = false;
  isDetailsError = false;
  isDefault = true;
  isRefused = false;
  details = '';
  oldValue: boolean;

  public isLoaded = false;

  // _value: YesNoStatus = new YesNoStatus();

  @Input()
  isDetailsRequired = false;
  @Input()
  question = '';
  @Input()
  isDisabled = false;
  @Input()
  isDetailsInvalid = false;
  @Input()
  isYesNoInvalid = false;
  @Input()
  isRequired = true;

  _yesNoRefusedDrop: DropDownField[];
  get yesNoRefusedDrop(): DropDownField[] {
    return this._yesNoRefusedDrop;
  }

  @Input('yesNoRefusedDrop')
  set yesNoRefusedDrop(value: DropDownField[]) {
    this.yesId = Utilities.idByFieldDataName('Yes', value);
    this.noId = Utilities.idByFieldDataName('No', value);
    this.refusedId = Utilities.idByFieldDataName('Refused', value);
    this._yesNoRefusedDrop = value;
    if (this.innerValue != null) {
      this.mapValueToBool(this.innerValue.statusName, true);
    }
  }

  @Input() originalModel: YesNoStatus;

  private yesId = 0;
  private noId = 0;
  private refusedId = 0;

  constructor() {
    super();
  }

  ngOnInit() { }

  ngOnChanges() {
    if (this.isYesNoInvalid != null && this.isYesNoInvalid === true) {
      this.isError = true;
    } else {
      this.isError = false;
    }
    if (this.isDetailsInvalid != null && this.isDetailsInvalid === true) {
      this.isDetailsError = true;
    } else {
      this.isDetailsError = false;
    }
  }

  get isDirty() {
    if (this.originalModel == null || this.value == null) {
      return false;
    } else if (this.value.status == null && this.originalModel.status != null) {
      return true;
    } else if (+this.value.status !== +this.originalModel.status) {
      return true;
    } else {
      return false;
    }
  }

  private mapValueToBool(state: string, isFromLoad: boolean) {
    if (state === 'Yes') {
      this.isYes = true;
      this.isRefused = false;
      this.value.status = this.yesId;
      this.value.statusName = state;

      // this.value.status = true;
      // this.value.refused = null;
    } else if (state === 'No') {
      this.isYes = false;
      this.isRefused = false;
      this.value.status = this.noId;
      this.value.statusName = state;
    } else if (state === 'Refused') {
      this.isYes = null;
      // this.value.status = null;
      // this.isRefused = !this.isRefused;
      if (isFromLoad === true) {
        this.isRefused = true;
      }
      if (this.isRefused) {
        this.value.status = this.refusedId;
        this.value.statusName = state;
      } else {
        this.value.status = null;
        this.value.statusName = null;
      }
    }
  }

  // Changes the background Color
  click(state: string) {
    this.mapValueToBool(state, false);
    this.onChangeCallback(this.value);
  }

  // Get accessor.
  get value(): YesNoStatus {
    return this.innerValue;
  }

  // Set accessor including call the onchange callback.
  set value(v: YesNoStatus) {
    if (v !== this.innerValue) {
      this.innerValue = v;
      this.onChangeCallback(v);
    }
  }

  // Set touched on blur.
  onBlur() {
    this.onTouchedCallback();
  }

  // From ControlValueAccessor interface.
  writeValue(value: YesNoStatus) {
    if (value !== this.innerValue) {
      if (value != null) {
        this.innerValue = value;
        this.mapValueToBool(value.statusName, true);
      }

      // this.click(this.innerValue);
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
}
