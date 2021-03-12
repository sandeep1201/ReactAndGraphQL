import { Input } from '@angular/core';

import { BaseComponent } from './base-component';
import { Clearable } from '../interfaces/clearable';
import { Common } from '../interfaces/common';
import { IsEmpty } from '../interfaces/is-empty';
import { ModelErrors } from '../interfaces/model-errors';
import { Utilities } from '../utilities';

/**
 * Base class for standard repeater functionality.
 *
 * @export
 * @class BaseRepeaterComponent
 * @extends {BaseComponent}
 */
export abstract class BaseRepeaterComponent<T extends Clearable & IsEmpty & Common> extends BaseComponent {
  private _modelErrors: ModelErrors[] = [];

  @Input() parentIndex: number;
  @Input() parentRepeaterModel: any;
  @Input() modelsDeleted: T[];
  @Input() cachedInitialModels: any[];

  get modelErrors(): ModelErrors[] {
    return this._modelErrors;
  }
  @Input('modelErrors')
  set modelErrors(value: ModelErrors[]) {
    if (value != null) {
      this._modelErrors = value;
    }
  }

  private createModel: () => T;
  public models: T[] = [];

  constructor(createFunction: () => T) {
    super();
    this.createModel = createFunction;
  }

  // get accessor
  get value(): any {
    return this.innerValue;
  }

  // set accessor including call the onchange callback
  set value(v: any) {
    if (v !== this.innerValue) {
      this.innerValue = v;
      this.onChangeCallback(v);
    }
  }

  // Set touched on blur
  onBlur() {
    this.onTouchedCallback();
  }

  // From ControlValueAccessor interface
  writeValue(value: any) {
    if (value !== this.innerValue) {
      this.innerValue = value;
      this.models = value;
    }
  }

  // From ControlValueAccessor interface
  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  // From ControlValueAccessor interface
  registerOnTouched(fn: any) {
    this.onTouchedCallback = fn;
  }

  eraseFromRepeater(model: T) {
    // Clear the values that are displayed on the screen.
    model.clear();

    // We have to trigger error model changes from repeater.
    this.onChangeCallback(this.models);
  }

  // Find Id from array and delete it.
  deleteFromRepeater(model: T, minNumber = 1) {
    // IndexOf method does not work in IE 7 and 8
    const index = this.models.indexOf(model);
    if (index === 0 && this.models.length === minNumber && this.modelErrors == null) {
      this.eraseFromRepeater(model);
    } else {
      // ModelsDeleted is only initialized if we are tracking delete reasons.
      if (this.modelsDeleted != null && model.id != null && model.id !== 0) {
        this.modelsDeleted.push(model);
      }
      this.models.splice(index, 1);

      if (this.modelErrors && this.modelErrors.length >= index + 1) {
        this.modelErrors.splice(index, 1);
      }
    }

    // Always add a new record if collection length is 0.
    if (this.models != null && this.models.length === 0 && minNumber >= 1) {
      this.models.push(this.createModel());
    }
    // We have to trigger error model changes from repeater.
    this.onChangeCallback(this.models);
  }

  public repeaterItemToBeDeleted(value: any) {
    if (value != null) {
      this.deleteFromRepeater(value);
    }
  }

  addToRepeater() {
    this.models.push(this.createModel());

    // We have to trigger error model changes from repeater.
    this.onChangeCallback(this.models);
  }

  getInitialModelValue(i: number, property: string) {
    return Utilities.getPropertybyIdAndName(this.cachedInitialModels, i, property);
  }

  isRepeaterRowRequired(i: number): boolean {
    return Utilities.isRepeaterRowRequired(this.models, i);
  }

  isChildRepeaterRowRequired(i: number, parentRepeaterModel: any, pi: number): boolean {
    return Utilities.isChildRepeaterRowRequired(this.models, i, parentRepeaterModel, pi);
  }

  isModelErrorsItemInvalid(i: number, property: string): boolean {
    return Utilities.isModelErrorsItemInvalid(this.modelErrors, i, property);
  }

  getModelErrors(index: number, property: string) {
    if (this.modelErrors[index] != null) {
      return this.modelErrors[index][property];
    }
  }
}
