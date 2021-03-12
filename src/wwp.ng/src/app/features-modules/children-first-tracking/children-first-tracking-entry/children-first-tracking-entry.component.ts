import { FieldDataTypes } from 'src/app/shared/enums/field-data-types.enum';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { FieldDataService } from './../../../shared/services/field-data.service';
import { ChildrenFirstTrackingService } from './../services/children-first-tracking.service';
import { ChildrenFirstTracking } from './../models/children-first-tracking.model';
import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { AppService } from 'src/app/core/services/app.service';
import { DateMmDdYyyyPipe } from 'src/app/shared/pipes/date-mm-dd-yyyy.pipe';
import { ModelErrors } from 'src/app/shared/interfaces/model-errors';
import { forkJoin } from 'rxjs';
import { ValidationManager } from 'src/app/shared/models/validation';

@Component({
  selector: 'app-children-first-tracking-entry',
  templateUrl: './children-first-tracking-entry.component.html',
  styleUrls: ['./children-first-tracking-entry.component.scss']
})
export class ChildrenFirstTrackingEntryComponent implements OnInit, OnDestroy {
  @Input() singleCFEntry: ChildrenFirstTracking;
  // This used to disable or enable form fields.
  @Input() canEdit: boolean;
  public isLoaded = false;
  public isSectionModified = false;

  public hasTriedSave = false;
  public isSectionValid = false;
  public nonParticipationReasons: DropDownField[];
  public goodCauseDeniedReasons: DropDownField[];
  public goodCauseGrantedReasons: DropDownField[];
  public model: ChildrenFirstTracking = new ChildrenFirstTracking();
  public cachedModel: ChildrenFirstTracking = new ChildrenFirstTracking();
  public modelErrors: ModelErrors = {};
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public isSaving = false;
  @Input() pin: string;

  constructor(private cftService: ChildrenFirstTrackingService, public appService: AppService, private fieldDataService: FieldDataService) {}

  ngOnInit() {
    ChildrenFirstTracking.clone(this.singleCFEntry, this.model);
    const pipe = new DateMmDdYyyyPipe();
    this.model.participationDate = pipe.transform(this.model.participationDate);
    ChildrenFirstTracking.clone(this.model, this.cachedModel);
    forkJoin(
      this.fieldDataService.getFieldDataByField(FieldDataTypes.GoodCauseGrantedReasons, 'cf'),
      this.fieldDataService.getFieldDataByField(FieldDataTypes.GoodCauseDeniedReasons, 'cf'),
      this.fieldDataService.getFieldDataByField(FieldDataTypes.NonParticipationReasons, 'cf')
    ).subscribe(res => {
      this.goodCauseGrantedReasons = res[0];
      this.goodCauseDeniedReasons = res[1];
      this.nonParticipationReasons = res[2];
      this.isLoaded = true;
    });
  }

  exit() {
    if (this.isSectionModified === true) {
      this.appService.isDialogPresent = true;
    } else {
      this.cftService.modeForCFParticipationEntry.next({ readOnly: false, inEditView: false });
    }
  }
  save() {
    if (this.isSectionValid) {
      this.cftService.postChildrenFirstTrackingDetails(this.pin, this.model, 'cf').subscribe(res => {
        this.cftService.modeForCFParticipationEntry.next({ readOnly: false, inEditView: false });
      });
    }
  }
  validate() {
    this.isSectionModified = true;
    this.isSectionValid = true;
    if (this.hasTriedSave === true) {
      this.validationManager.resetErrors();
      const result = this.model.validate(this.validationManager, this.nonParticipationReasons, this.goodCauseGrantedReasons, this.goodCauseDeniedReasons);
      this.isSectionValid = result.isValid;
      this.modelErrors = result.errors;
      if (this.modelErrors) {
        this.isSaving = false;
      }
    }
  }

  saveAndExit() {
    this.hasTriedSave = true;
    this.validate();
    this.save();
  }

  dialogueClose() {
    this.cftService.modeForCFParticipationEntry.next({ readOnly: false, inEditView: false });
  }
  ngOnDestroy() {
    this.cftService.modeForCFParticipationEntry.next({ readOnly: false, inEditView: false });
  }
}
