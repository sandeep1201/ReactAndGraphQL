import { Component, forwardRef, Input, Output, OnChanges, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { DropDownField } from '../../../../shared/models/dropdown-field';
import { BaseRepeaterComponent } from '../../../../shared/components/base-repeater-component';
import { NonCustodialCaretaker } from '../../../../shared/models/non-custodial-parents-section';
import { NonCustodialChild } from '../../../../shared/models/non-custodial-parents-section';

@Component({
  selector: 'app-ncp-caretaker-repeater',
  templateUrl: 'ncp-caretaker-repeater.component.html',
  styleUrls: ['./ncp-caretaker-repeater.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => NcpCaretakerRepeaterComponent),
      multi: true
    }
  ]
})
export class NcpCaretakerRepeaterComponent extends BaseRepeaterComponent<NonCustodialCaretaker> implements ControlValueAccessor {
  @Input() ncpRelationshipDrop: DropDownField[];
  @Input() polarDrop: DropDownField[];
  @Input() contactIntervalDrop: DropDownField[];
  _availableCaretackersDrop: DropDownField[];
  get availableCaretackersDrop(): DropDownField[] {
    return this._availableCaretackersDrop;
  }
  @Input('availableCaretackersDrop') set availableCaretackersDrop(value: DropDownField[]) {
    this._availableCaretackersDrop = value;
  }

  @Output() moveNonCustodialChildRequest = new EventEmitter<{ nonCustodialChild: NonCustodialChild; parentGuid: string }>();
  constructor() {
    super(NonCustodialCaretaker.create);
  }
  moveNonCustodialChild(childMoveRequest: { nonCustodialChild: NonCustodialChild; parentGuid: string }) {
    this.moveNonCustodialChildRequest.emit({
      nonCustodialChild: childMoveRequest.nonCustodialChild,
      parentGuid: childMoveRequest.parentGuid
    });
  }

  areChildrenEmpty(i) {
    return this.models[i].isNonCustodialChildrenEmpty();
  }
}
