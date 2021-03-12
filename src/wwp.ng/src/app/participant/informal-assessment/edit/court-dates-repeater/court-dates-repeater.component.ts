// tslint:disable: no-output-on-prefix
import { Component, Input, forwardRef, Output, EventEmitter } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

import { BaseRepeaterComponent } from '../../../../shared/components/base-repeater-component';
import { CourtDate } from '../../../../shared/models/legal-issues-section';
import { Participant } from '../../../../shared/models/participant';

@Component({
  selector: 'app-court-dates-repeater',
  templateUrl: './court-dates-repeater.component.html',
  styleUrls: ['./court-dates-repeater.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => CourtDatesRepeaterComponent),
      multi: true
    }
  ]
})
export class CourtDatesRepeaterComponent extends BaseRepeaterComponent<CourtDate> implements ControlValueAccessor {
  @Input() ParticipantInfo: Participant;
  @Output() onCheckState = new EventEmitter<boolean>();

  constructor() {
    super(CourtDate.create);
  }
}
