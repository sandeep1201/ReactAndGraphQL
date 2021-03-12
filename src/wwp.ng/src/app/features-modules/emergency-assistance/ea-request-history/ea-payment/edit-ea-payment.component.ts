import { Utilities } from 'src/app/shared/utilities';
import { EARequest } from './../../models/ea-request.model';
import { ActivatedRoute } from '@angular/router';
import { EAPayment } from './../../models/ea-request-payment.model';
import { Component, OnInit, Input } from '@angular/core';
import { Participant } from 'src/app/shared/models/participant';
import { EmergencyAssistanceService } from '../../services/emergancy-assistance.service';
import { take, concatMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { AppService } from 'src/app/core/services/app.service';
import * as _ from 'lodash';
import { ModelErrors } from 'src/app/shared/interfaces/model-errors';
import { ValidationManager } from 'src/app/shared/models/validation';

@Component({
  selector: 'app-ea-payment-edit',
  templateUrl: './edit-ea-payment.component.html',
  styleUrls: ['./edit-ea-payment.component.scss']
})
export class EAPaymentEditComponent implements OnInit {
  @Input() participant: Participant = new Participant();
  @Input() eaRequestModel: EARequest;
  @Input() totalAmount: string;
  public isLoaded = false;
  public hadSaveError = false;
  public isSaving = false;
  public hasTriedSave = false;
  public isSectionModified = false;
  public isSectionValid = true;
  public isInEditMode = false;
  public isReadOnly = false;
  model: EAPayment = new EAPayment();
  cachedModel: EAPayment = new EAPayment();
  validationManager: ValidationManager = new ValidationManager(this.appService);
  public modelErrors: ModelErrors = {};
  public mailingAddressValidateStatus = false;

  constructor(private eaService: EmergencyAssistanceService, private appService: AppService, private route: ActivatedRoute) {}

  ngOnInit() {
    this.eaService.modeForEAPayment
      .pipe(
        take(1),
        concatMap(res => {
          this.isInEditMode = res.isInEditMode;
          this.isReadOnly = res.readOnly;
          return res.data ? of(res.data) : of(EAPayment.create(this.route.snapshot.params.id));
        })
      )
      .pipe(take(1))
      .subscribe(res => {
        this.model = res;
        EAPayment.clone(this.model, this.cachedModel);
        this.isLoaded = true;
      });
  }

  validate() {
    this.isSectionModified = true;
    if (this.hasTriedSave) {
      this.validationManager.resetErrors();
      const result = this.model.validate(this.validationManager, this.eaRequestModel, this.totalAmount);
      this.isSectionValid = result.isValid;
      this.modelErrors = result.errors;
      if (this.isSectionValid) this.hasTriedSave = false;
      if (this.modelErrors) {
        this.isSaving = false;
      }
    }
  }

  exit() {
    if (this.isSectionModified) {
      this.appService.isUrlChangeBlocked = false;
      this.appService.isDialogPresent = true;
    } else {
      this.eaService.modeForEAPayment.next({ readOnly: false, isInEditMode: false, data: null });
    }
  }

  exitEAEditIgnoreChanges() {
    this.eaService.modeForEAPayment.next({ readOnly: false, isInEditMode: false, data: null });
  }

  onSave() {
    this.hasTriedSave = true;
    this.validate();
    if (this.isSectionValid) {
      this.hadSaveError = false;
      this.isSaving = true;
      Utilities.cleanseModelForApi(this.model);
      this.eaService.saveEAPayment(this.model, this.participant.pin).subscribe(
        res => {
          this.isSaving = false;
          this.eaService.modeForEAPayment.next({ readOnly: false, isInEditMode: false, data: null });
        },
        error => {
          this.isSaving = false;
          this.hadSaveError = true;
        }
      );
    }
  }
}
