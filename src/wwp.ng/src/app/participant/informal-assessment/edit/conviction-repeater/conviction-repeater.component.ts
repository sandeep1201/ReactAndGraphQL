// tslint:disable: no-output-on-prefix
import { Component, Input, forwardRef, Output, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

import { BaseRepeaterComponent } from '../../../../shared/components/base-repeater-component';
import { CriminalCharge } from '../../../../shared/models/legal-issues-section';
import { DropDownField } from '../../../../shared/models/dropdown-field';
import { Participant } from '../../../../shared/models/participant';

@Component({
  selector: 'app-conviction-repeater',
  templateUrl: './conviction-repeater.component.html',
  styleUrls: ['./conviction-repeater.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ConvictionRepeaterComponent),
      multi: true
    }
  ]
})
export class ConvictionRepeaterComponent extends BaseRepeaterComponent<CriminalCharge> implements ControlValueAccessor {
  @Input() isPendingChargesRepeater = false;
  @Input() DateLabel: string;
  @Input() repeaterTitle: string;
  @Input() addTitle: string;
  @Input() LegalIssuesDrop: DropDownField;
  @Input() ParticipantInfo: Participant;
  @Output() onCheckState = new EventEmitter<boolean>();

  constructor() {
    super(CriminalCharge.create);
  }
}
