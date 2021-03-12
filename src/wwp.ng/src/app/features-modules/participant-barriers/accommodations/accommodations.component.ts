import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit, OnDestroy, forwardRef, Input } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { Subscription } from 'rxjs';

import { AppService } from './../../../core/services/app.service';
import { BarrierAccommodation } from '../../../shared/models/participant-barriers-app';
import { BaseRepeaterComponent } from '../../../shared/components/base-repeater-component';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { ParticipantBarrierAppService } from '../../../shared/services/participant-barrier-app.service';
import { ParticipantService } from '../../../shared/services/participant.service';

@Component({
  selector: 'app-accommodations',
  templateUrl: './accommodations.component.html',
  styleUrls: ['./accommodations.component.css'],
  providers: [
    FieldDataService,
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => BarriersAccommodationsRepeaterComponent),
      multi: true
    }
  ]
})
export class BarriersAccommodationsRepeaterComponent extends BaseRepeaterComponent<BarrierAccommodation> implements ControlValueAccessor, OnInit, OnDestroy {
  @Input() modelErrors: ModelErrors[] = [];
  @Input() isDisabled = false;
  private acSub: Subscription;
  private accommodationsDrop: DropDownField[] = [];
  public model: BarrierAccommodation[] = [];

  constructor(
    private appService: AppService,
    private participantBarrierAppService: ParticipantBarrierAppService,
    private fdService: FieldDataService,
    route: ActivatedRoute,
    router: Router,
    partService: ParticipantService
  ) {
    super(BarrierAccommodation.create);
  }

  ngOnInit() {
    this.acSub = this.fdService.getAccommodations().subscribe(data => this.initAccommodations(data));
  }

  initAccommodations(data) {
    this.accommodationsDrop = data;
  }

  ngOnDestroy() {
    if (this.acSub != null) {
      this.acSub.unsubscribe();
    }
  }
}
