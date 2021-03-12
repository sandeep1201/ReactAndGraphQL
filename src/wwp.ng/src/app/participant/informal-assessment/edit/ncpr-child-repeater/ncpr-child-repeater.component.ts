import { Component, forwardRef, Input, Output, OnChanges, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { DropDownField } from '../../../../shared/models/dropdown-field';
import { BaseRepeaterComponent } from '../../../../shared/components/base-repeater-component';
import { NonCustodialOtherParent, NonCustodialReferralChild } from '../../../../shared/models/non-custodial-parents-referral-section';

import * as _ from 'lodash';

@Component({
  selector: 'app-ncpr-child-repeater',
  templateUrl: './ncpr-child-repeater.component.html',
  styleUrls: ['./ncpr-child-repeater.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => NcprChildRepeaterComponent),
      multi: true
    }
  ]
})
export class NcprChildRepeaterComponent extends BaseRepeaterComponent<NonCustodialReferralChild> implements ControlValueAccessor, OnChanges {
  @Input() contactIntervalDrop: DropDownField[];

  @Input() availableParentsDrop: DropDownField[];
  @Input() nonCustodialOtherParentModel: NonCustodialOtherParent;

  @Output() moveNonCustodialChild = new EventEmitter<{ nonCustodialReferralChild: NonCustodialReferralChild; parentGuid: string }>();

  public parentGuid: string;

  // Our template uses the cached array because we only want it to update when the array really changes.
  public cachedAvailableParentsDrop: DropDownField[] = [];
  constructor() {
    super(NonCustodialReferralChild.create);
  }
  emitMovedNonCustodialChild(nonCustodialReferralChild: NonCustodialReferralChild) {
    if (this.parentGuid != null && nonCustodialReferralChild != null) {
      this.moveNonCustodialChild.emit({ nonCustodialReferralChild: nonCustodialReferralChild, parentGuid: this.parentGuid });
      this.parentGuid = null;
    }
  }
  isEmptyObject(obj) {
    return obj && Object.keys(obj).length === 0;
  }

  ngOnChanges() {
    if (!_.isEqual(this.cachedAvailableParentsDrop.sort(), this.availableParentsDrop.sort())) {
      this.cachedAvailableParentsDrop = Array.from(this.availableParentsDrop);
    }
  }
}
