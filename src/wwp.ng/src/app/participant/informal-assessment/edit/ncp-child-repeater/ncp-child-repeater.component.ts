import { Component, forwardRef, Input, Output, EventEmitter, OnChanges } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { DropDownField } from '../../../../shared/models/dropdown-field';
import { BaseRepeaterComponent } from '../../../../shared/components/base-repeater-component';
import { NonCustodialCaretaker, NonCustodialChild } from '../../../../shared/models/non-custodial-parents-section';

import * as _ from 'lodash';

@Component({
  selector: 'app-ncp-child-repeater',
  templateUrl: './ncp-child-repeater.component.html',
  styleUrls: ['./ncp-child-repeater.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => NcpChildRepeaterComponent),
      multi: true
    }
  ]
})
export class NcpChildRepeaterComponent extends BaseRepeaterComponent<NonCustodialChild> implements ControlValueAccessor, OnChanges {
  @Input() contactIntervalDrop: DropDownField[];
  @Input() polarDrop: DropDownField[];
  @Input() availableCaretackersDrop: DropDownField[];

  @Input() nonCustodialCaretakerModel: NonCustodialCaretaker;
  @Output() moveNonCustodialChild = new EventEmitter<{ nonCustodialChild: NonCustodialChild; parentGuid: string }>();

  public parentGuid: string;

  // Our template uses the cached array because we only want it to update when the array really changes.
  public cachedAvailableCaretackersDrop: DropDownField[] = [];
  constructor() {
    super(NonCustodialChild.create);
  }

  emitMovedNonCustodialChild(nonCustodialChild: NonCustodialChild) {
    if (this.parentGuid != null && nonCustodialChild != null) {
      this.moveNonCustodialChild.emit({ nonCustodialChild: nonCustodialChild, parentGuid: this.parentGuid });
      this.parentGuid = null;
    }
  }

  isEmptyObject(obj) {
    return obj && Object.keys(obj).length === 0;
  }
  ngOnChanges() {
    if (!_.isEqual(this.cachedAvailableCaretackersDrop.sort(), this.availableCaretackersDrop.sort())) {
      this.cachedAvailableCaretackersDrop = Array.from(this.availableCaretackersDrop);
    }
  }
}
