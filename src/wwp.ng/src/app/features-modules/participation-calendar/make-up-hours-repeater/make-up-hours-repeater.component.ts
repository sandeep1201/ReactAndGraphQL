import { BaseRepeaterComponent } from 'src/app/shared/components/base-repeater-component';
import { Component, OnInit, forwardRef, Input } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { MakeUpEntries } from 'src/app/shared/models/participation-makeup.model';

@Component({
  selector: 'app-make-up-hours-repeater',
  templateUrl: './make-up-hours-repeater.component.html',
  styleUrls: ['./make-up-hours-repeater.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => MakeUpHoursRepeaterComponent),
      multi: true
    }
  ]
})
export class MakeUpHoursRepeaterComponent extends BaseRepeaterComponent<MakeUpEntries> implements OnInit, ControlValueAccessor {
  constructor() {
    super(MakeUpEntries.create);
  }

  ngOnInit() {}
}
