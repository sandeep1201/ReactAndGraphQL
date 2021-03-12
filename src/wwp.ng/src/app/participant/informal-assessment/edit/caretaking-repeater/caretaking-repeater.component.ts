import { Component, Input, forwardRef } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { BaseRepeaterComponent } from '../../../../shared/components/base-repeater-component';
import { DropDownField } from '../../../../shared/models/dropdown-field';
import { ModelErrors } from '../../../../shared/interfaces/model-errors';
import { FamilyMember } from '../../../../shared/models/family-barriers-section';

@Component({
  selector: 'app-caretaking-repeater',
  templateUrl: './caretaking-repeater.component.html',
  styleUrls: ['./caretaking-repeater.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CaretakingRepeaterComponent),
      multi: true
    }
  ]
})
export class CaretakingRepeaterComponent extends BaseRepeaterComponent<FamilyMember> implements ControlValueAccessor {
  @Input() modelErrors: ModelErrors[] = [];
  @Input() relationships: DropDownField[];

  public familyMembers: FamilyMember[];

  constructor() {
    super(FamilyMember.create);
  }
}
