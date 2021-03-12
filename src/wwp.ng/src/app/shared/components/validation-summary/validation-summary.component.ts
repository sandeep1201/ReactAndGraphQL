import { Component, Input, DoCheck } from '@angular/core';

import { SectionError } from '../../models/section-error';
import { ValidationError } from '../../models/validation-error';

@Component({
  selector: 'app-validation-summary',
  templateUrl: './validation-summary.component.html',
  styleUrls: ['./validation-summary.component.css']
})
export class ValidationSummaryComponent implements DoCheck {
  @Input() errors: ValidationError[];
  constructor() { }

  _errors: ValidationError[];

  ngDoCheck() {
    this.forceUnique();
  }


  // get errors(): ValidationError[] {
  //   return this._errors;
  // }


  forceUnique() {
    this._errors = [];
    if (this.errors != null) {
      for (let _i = 0; _i < this.errors.length; _i++) {
        let existing = this._errors.find(ve => (ve.formatted as string) === (this.errors[_i].formatted as string));
        if (existing == null) {
          this._errors.push(JSON.parse(JSON.stringify(this.errors[_i])));
        }
      }
    }
  } 

}
