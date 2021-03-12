import { Component, forwardRef, Input } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { BaseRepeaterComponent } from '../../shared/components/base-repeater-component';
import { PersonAlias } from '../../shared/models/alias.model';
import { DropDownField } from '../../shared/models/dropdown-field';

@Component({
  selector: 'app-alias-repeater',
  templateUrl: './alias-repeater.component.html',
  styleUrls: ['./alias-repeater.component.css'],
  providers: [{
    provide: NG_VALUE_ACCESSOR,
    useExisting: forwardRef(() => AliasRepeaterComponent),
    multi: true
  }]
})


export class AliasRepeaterComponent extends BaseRepeaterComponent<PersonAlias> implements ControlValueAccessor {

  @Input() suffixTypesDrop: DropDownField[];
  @Input() aliasTypesDrop: DropDownField[];
  @Input() isReadOnly = false;

  // This differs from readonly because a user can still delete a row.
  @Input() areInputFieldsReadonly = false;

  // Many Br's change once we are working with a non clearance Alias.
  @Input() isNonSearch = false;
  public maxAlias = 2;

  constructor() { super(PersonAlias.create); }

  isReadOnlyIfEmpty(isEmpty: boolean, id: string): boolean {
    if (!isEmpty && this.isNonSearch && +id > 0) {
      return true;
    } else if (this.isReadOnly) {
      return true;
    } else {
      return false;
    }
  }
}
