import { Component, OnInit, OnDestroy, forwardRef, Input, EventEmitter, Output, OnChanges } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { BaseRepeaterComponent } from 'src/app/shared/components/base-repeater-component';
import { SupportiveService } from 'src/app/features-modules/employability-plan/models/supportive-service.model';
import { Utilities } from 'src/app/shared/utilities';
import { DropDownField } from 'src/app/shared/models/dropdown-field';

@Component({
  selector: 'app-service-repeater',
  templateUrl: './service-repeater.component.html',
  styleUrls: ['./service-repeater.component.scss'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ServiceRepeaterComponent),
      multi: true
    }
  ]
})
export class ServiceRepeaterComponent extends BaseRepeaterComponent<SupportiveService> implements ControlValueAccessor, OnChanges {
  @Input() serviceDropList: DropDownField[];
  private otherSupportiveServiceId: number;
  public maxNumberOfItems = 10;

  constructor() {
    super(SupportiveService.create);
  }

  ngOnChanges() {
    if (this.serviceDropList != null) {
      this.otherSupportiveServiceId = Utilities.idByFieldDataName('Other', this.serviceDropList);
    }
  }

  isDetailsRequired(ss: SupportiveService) {
    return ss.isDetailsRequired(this.otherSupportiveServiceId);
  }

  removeDisableOnServices(types) {
    if (types == null || types.supportiveServiceId < 1 || this.serviceDropList == null) {
      return;
    }

    for (const dd of this.serviceDropList) {
      if (+dd.id === +types.supportiveServiceId) {
        dd.isSelected = false;
        // We break because there only be 1.
        break;
      }
    }
  }
}
