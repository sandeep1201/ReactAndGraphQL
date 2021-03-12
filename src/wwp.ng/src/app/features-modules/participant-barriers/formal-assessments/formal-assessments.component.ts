import { Component, OnInit, OnDestroy, forwardRef, Input, EventEmitter, Output } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';

import { AppService } from './../../../core/services/app.service';
import { BaseRepeaterComponent } from '../../../shared/components/base-repeater-component';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { FormalAssessment } from '../../../shared/models/participant-barriers-app';
import { ParticipantBarrierAppService } from '../../../shared/services/participant-barrier-app.service';
import { ParticipantService } from '../../../shared/services/participant.service';

import { DropDownField } from '../../../shared/models/dropdown-field';
import { Utilities } from '../../../shared/utilities';
@Component({
  selector: 'app-formal-assessments',
  templateUrl: './formal-assessments.component.html',
  styleUrls: ['./formal-assessments.component.css'],
  providers: [
    FieldDataService,
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => BarriersFormalAssessmentRepeaterComponent),
      multi: true
    }
  ]
})
export class BarriersFormalAssessmentRepeaterComponent extends BaseRepeaterComponent<FormalAssessment> implements OnInit, OnDestroy, ControlValueAccessor {
  private maxFormalAssessments = 3;
  private sSub: Subscription;
  private itSub: Subscription;
  public symptomsDrop: DropDownField[] = [];
  // public model: FormalAssessment[] = [];

  public intervalTypesDrop: DropDownField[];
  public intervalTypeDayId: number;
  public intervalTypeWeekId: number;
  public payload: object;
  @Output() intervalTypeDataEmit: EventEmitter<object> = new EventEmitter<object>();

  @Input() pin: number;

  @Input() isDisabled = false;
  constructor(
    private appService: AppService,
    private participantBarrierAppService: ParticipantBarrierAppService,
    private fdService: FieldDataService,
    route: ActivatedRoute,
    router: Router,
    partService: ParticipantService
  ) {
    super(FormalAssessment.create);
  }

  ngOnInit() {
    this.sSub = this.fdService.getSymptoms().subscribe(data => this.initSymptoms(data));
    this.itSub = this.fdService.getIntervalTypes().subscribe(data => this.initIntervalTypes(data));
  }

  initSymptoms(data) {
    this.symptomsDrop = data;
  }
  initIntervalTypes(data) {
    this.intervalTypesDrop = data.filter(item => item.name === 'Week' || item.name === 'Day');
    this.intervalTypeDayId = Utilities.idByFieldDataName('Day', this.intervalTypesDrop, true);
    this.intervalTypeWeekId = Utilities.idByFieldDataName('Week', this.intervalTypesDrop, true);
    this.payload = {
      intervalTypeDrop: this.intervalTypesDrop,
      intervalTypeDayId: this.intervalTypeDayId,
      intervalTypeWeekId: this.intervalTypeWeekId
    };
    this.intervalTypeDataEmit.emit(this.payload);
  }

  /**
   * Adds disable css to add button and wont allow additon of new Formal Assessments.
   *
   * @private
   * @returns {boolean}
   *
   * @memberOf BarriersFormalAssessmentRepeaterComponent
   */
  private allowAddition(): boolean {
    if (this.models != null && this.models.length < this.maxFormalAssessments) {
      return true;
    } else {
      return false;
    }
  }

  isRepeaterRowRequired(i: number): boolean {
    return Utilities.isRepeaterRowRequired(this.models, i);
  }
  isAssessmentDateEntered(fa: FormalAssessment) {
    return fa.isAssessmentDateEntered();
  }

  isAssessmentNotCompleted(fa: FormalAssessment) {
    return fa.isAssessmentNotCompleted();
  }

  isReferralDeclined(fa: FormalAssessment) {
    return fa.isReferralDeclined();
  }

  isAssessmentNotNeeded(fa: FormalAssessment) {
    return fa.isAssessmentNotNeeded();
  }

  isParticipationHoursInformationHidden(fa: FormalAssessment) {
    return fa.isParticipationHoursInformationHidden();
  }
  isParticipationHoursInformationEntereds(fa: FormalAssessment): any {
    return fa.isParticipationHoursInformationEntered();
  }

  ngOnDestroy() {
    if (this.sSub != null) {
      this.sSub.unsubscribe();
    }
    if (this.itSub != null) {
      this.itSub.unsubscribe();
    }
  }
}
