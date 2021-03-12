import { AppService } from './../../../core/services/app.service';
// tslint:disable: import-blacklist
import { Component, OnInit, OnDestroy, ViewContainerRef } from '@angular/core';
import { Subscription, Observable, forkJoin } from 'rxjs';
import { BaseInformalAssessmentSecton } from './base-informal-assessment';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { InformalAssessmentEditService } from '../../../shared/services/informal-assessment-edit.service';
import { LanguagesSection, KnownLanguage } from '../../../shared/models/languages-section';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { ModalService } from 'src/app/core/modal/modal.service';
import { SectionComponent } from '../../../shared/interfaces/section-component';
import { Utilities } from '../../../shared/utilities';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-languages-edit',
  templateUrl: 'languages.component.html',
  styleUrls: ['./edit.component.css']
})
export class LanguagesEditComponent extends BaseInformalAssessmentSecton implements OnInit, OnDestroy, SectionComponent {
  public englishId: number;
  private sectionSub: Subscription;
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public model: LanguagesSection;
  public modelErrors: ModelErrors = {}; // Note: this must be initialized to an empty object for the UI in initial state.
  public languageDrop: DropDownField[] = [];
  public isActive = true;
  public cached = new LanguagesSection();

  constructor(
    private appService: AppService,
    private eiaService: InformalAssessmentEditService,
    private fdService: FieldDataService,
    viewContainer: ViewContainerRef,
    public modalService: ModalService
  ) {
    super(modalService, eiaService);
  }

  ngOnInit() {
    this.eiaService.setSectionComponent(this);

    forkJoin(this.fdService.getLanguages().pipe(take(1)))
      .pipe(take(1))
      .subscribe(results => {
        this.initLanguageDropDowns(results[0]);
        this.loadModel();
      });
  }

  private loadModel() {
    this.sectionSub = this.eiaService.getLanguageSection().subscribe(data => {
      this.initSection(data);
      // HACK: the modifiedTracker IF was added to check when loading up the main page.
      this.modifiedTrackerForcedValidation();
      this.sectionLoaded();
    });
  }

  modifiedTrackerForcedValidation() {
    if (this.eiaService.hasLanguageValidated || this.eiaService.modifiedTracker.isLanguageError) {
      this.isSectionValid = false;
      this.validate();
    }
  }

  private initLanguageDropDowns(data) {
    this.englishId = Utilities.idByFieldDataName('English', data);
    this.languageDrop = data;
  }

  private initSection(model: LanguagesSection) {
    this.model = model;
    this.eiaService.languagesModel = model;
    // We use clone to check which fields are modified.
    this.cached = this.eiaService.lastSavedLanguagesModel;

    // Cloned string for equality checking
    this.cloneModelString = JSON.stringify(model);

    // Pushes blank object inside of repeater when model has none.
    if (model.knownLanguages.length === 0) {
      const x = new KnownLanguage();
      this.model.knownLanguages.push(x);
    }
  }

  isValid(): boolean {
    return this.isSectionValid;
  }

  prepareToSaveWithErrors(): LanguagesSection {
    // make a clone
    const model = JSON.parse(JSON.stringify(this.model)) as LanguagesSection;

    // TODO: Clean up any data on the model in a bad state.

    // The following removes a knowlang with no lang selected.
    const removeList: number[] = [];
    // Add to temp int list of indexes to remove.
    for (let i = 0; i < model.knownLanguages.length; i++) {
      if (model.knownLanguages[i].languageId == null) {
        removeList.push(i);
      }
    }
    // Remove from list;
    for (let i = 0; i < removeList.length; i++) {
      model.knownLanguages.splice(removeList[i], 1);
    }

    return model;
  }

  refreshModel() {
    if (this.model != null && this.eiaService.languagesModel != null) {
      this.initSection(this.eiaService.languagesModel);
      this.modifiedTrackerForcedValidation();
    }
    this.isActive = false;
    setTimeout(() => (this.isActive = true), 0);
  }

  isHomeLangRequired(ls: LanguagesSection) {
    return ls.isHomeLangRequired();
  }

  // Validation For model Object
  validate(): void {
    // Clear all previous errors.
    this.validationManager.resetErrors();

    // Call the model's validate method.
    const result = this.model.validate(this.validationManager, this.englishId);

    // Update our properties so the UI can bind to the results.
    this.isSectionValid = result.isValid;
    this.modelErrors = result.errors;

    if (this.isSectionValid === true) {
      this.eiaService.sectionIsNowValid('languages');
    }
  }

  checkState() {
    if (!this.isSectionValid) {
      this.validate();
    }

    this.eiaService.setModifiedModel('languages', this.cloneModelString !== JSON.stringify(this.eiaService.languagesModel));
  }

  ngOnDestroy() {
    if (this.sectionSub != null) {
      this.sectionSub.unsubscribe();
    }
  }
}
