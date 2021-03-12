import { FieldDataService } from 'src/app/shared/services/field-data.service';
import { AppService } from './../../core/services/app.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Participant } from 'src/app/shared/models/participant';
import { Component, OnInit } from '@angular/core';
import { forkJoin } from 'rxjs';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { take } from 'rxjs/operators';
import { DrugScreening } from './models/drug-screening.model';
import { ModelErrors } from 'src/app/shared/interfaces/model-errors';
import { DrugScreeningService } from './services/drug-screening.service';
import { ValidationManager } from 'src/app/shared/models/validation';
import * as _ from 'lodash';
import { DropDownField } from 'src/app/shared/models/dropdown-field';

@Component({
  selector: 'app-drug-screening',
  templateUrl: './drug-screening.component.html',
  styleUrls: ['./drug-screening.component.scss']
})
export class DrugScreeningComponent implements OnInit {
  goBackUrl: string;
  participant: Participant;
  isLoaded = false;
  public pin: string;
  public model = new DrugScreening();
  public originalModel = new DrugScreening();
  public modelErrors: ModelErrors = {};
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public isSectionModified = false;
  public isSectionValid = true;
  public hasTriedSave = false;
  public isSaving = false;
  public hadSaveError = false;
  public isReadOnly = false;
  public drugScreeningStatusTypeDrop: DropDownField[];

  constructor(
    private partService: ParticipantService,
    private route: ActivatedRoute,
    private router: Router,
    public appService: AppService,
    private drugScreeningService: DrugScreeningService,
    private fdService: FieldDataService
  ) {}

  ngOnInit() {
    this.pin = this.route.snapshot.params.pin;
    this.goBackUrl = '/pin/' + this.pin;

    forkJoin(
      this.partService.getCachedParticipant(this.pin).pipe(take(1)),
      this.drugScreeningService.getDrugScreeningData(this.pin).pipe(take(1)),
      this.fdService.getFieldDataByField('drug-screening-status-types').pipe(take(1))
    ).subscribe(results => {
      this.participant = results[0];
      results[1] ? (this.model = results[1]) : (this.model.id = 0);
      this.drugScreeningStatusTypeDrop = results[2];
      DrugScreening.clone(this.model, this.originalModel);
      this.isLoaded = true;
    });
  }

  validate(isOnSave = false) {
    this.isSectionModified = true;

    if (this.hasTriedSave === true) {
      this.validationManager.resetErrors();
      const result = this.model.validate(this.validationManager);
      this.isSectionValid = result.isValid;
      this.modelErrors = result.errors;
      if (this.isSectionValid) this.hasTriedSave = false;
      if (this.modelErrors) this.isSaving = false;
    }

    this.isSectionModified && !isOnSave ? (this.appService.isUrlChangeBlocked = true) : (this.appService.isUrlChangeBlocked = false);
  }

  save() {
    this.isSaving = true;
    this.hasTriedSave = true;
    this.validate(true);
    if (this.isSectionValid) {
      this.hadSaveError = false;
      this.isSaving = true;
      this.drugScreeningService.saveDrugScreening(this.pin, this.model).subscribe(
        success => {
          this.setFlags(false, false, false);
        },
        error => {
          this.setFlags(false, false, true);
        }
      );
    }
  }

  setFlags(isSaving, isSectionModified, hadSaveError) {
    this.isSaving = isSaving;
    this.isSectionModified = isSectionModified;
    this.hadSaveError = hadSaveError;
    if (!hadSaveError) this.router.navigateByUrl(`/pin/${this.pin}`);
  }

  exit() {
    if (this.isSectionModified) {
      this.appService.isUrlChangeBlocked = true;
      this.appService.isDialogPresent = true;
    } else {
      this.router.navigateByUrl(`/pin/${this.pin}`);
    }
  }

  exitDrugScreeningIngnoreChnages() {
    this.appService.isUrlChangeBlocked = false;
    this.router.navigateByUrl(`/pin/${this.pin}`);
  }
}
