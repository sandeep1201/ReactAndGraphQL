import { Component, forwardRef, Input, Output, OnChanges, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { DropDownField } from '../../../../shared/models/dropdown-field';
import { BaseRepeaterComponent } from '../../../../shared/components/base-repeater-component';
import { NonCustodialOtherParent, NonCustodialReferralChild } from '../../../../shared/models/non-custodial-parents-referral-section';
import { ModelErrors } from '../../../../shared/interfaces/model-errors';

@Component({
  selector: 'app-ncpr-other-parent-repeater',
  templateUrl: './ncpr-other-parent-repeater.component.html',
  styleUrls: ['./ncpr-other-parent-repeater.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => NcprOtherParentRepeaterComponent),
      multi: true
    }
  ]
})
export class NcprOtherParentRepeaterComponent extends BaseRepeaterComponent<NonCustodialOtherParent> implements ControlValueAccessor {
  @Input() pin: number;
  @Input() contactIntervalDrop: DropDownField[];

  @Input() availableParentsDrop: DropDownField[];
  @Output() moveNonCustodialChildRequest = new EventEmitter<{ nonCustodialReferralChild: NonCustodialReferralChild; parentGuid: string }>();

  constructor() {
    super(NonCustodialOtherParent.create);
  }

  moveNonCustodialChild(childMoveRequest: { nonCustodialReferralChild: NonCustodialReferralChild; parentGuid: string }) {
    this.moveNonCustodialChildRequest.emit({ nonCustodialReferralChild: childMoveRequest.nonCustodialReferralChild, parentGuid: childMoveRequest.parentGuid });
  }

  areChildrenEmpty(i) {
    return this.models[i].isNonCustodialChildrenEmpty();
  }
}
