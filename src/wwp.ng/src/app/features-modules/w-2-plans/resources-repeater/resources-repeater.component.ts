import { W2PlanSectionResource } from './../models/w-2-plan.model';
import { BaseRepeaterComponent } from 'src/app/shared/components/base-repeater-component';
import { Component, forwardRef, Input, OnInit } from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';

@Component({
  selector: 'app-resources-repeater',
  templateUrl: './resources-repeater.component.html',
  styleUrls: ['./resources-repeater.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ResourcesRepeaterComponent),
      multi: true
    }
  ]
})
export class ResourcesRepeaterComponent extends BaseRepeaterComponent<W2PlanSectionResource> implements OnInit {
  @Input() isReadOnly = false;
  @Input() disabled = false;
  constructor() {
    super(W2PlanSectionResource.create);
  }

  ngOnInit() {}

  validate() {}
}
