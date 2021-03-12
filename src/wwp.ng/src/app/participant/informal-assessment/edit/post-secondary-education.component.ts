import { Utilities } from './../../../shared/utilities';
import { Component, OnInit, OnDestroy, DoCheck } from '@angular/core';
import { Subscription, forkJoin } from 'rxjs';
import * as moment from 'moment';

import { AppService } from 'src/app/core/services/app.service';
import { BaseInformalAssessmentSecton } from './base-informal-assessment';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { InformalAssessmentEditService } from '../../../shared/services/informal-assessment-edit.service';
import { LogService } from '../../../shared/services/log.service';
import { ModalService } from 'src/app/core/modal/modal.service';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { ParticipantService } from '../../../shared/services/participant.service';
import { PostSecondaryEducationSection, PostSecondaryCollege, PostSecondaryDegree, PostSecondaryLicense } from '../../../shared/models/post-secondary-education-section';
import { SectionComponent } from '../../../shared/interfaces/section-component';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-post-secondary-education-edit',
  templateUrl: 'post-secondary-education.component.html',
  styleUrls: ['./edit.component.css']
})
export class PostSecondaryEducationEditComponent extends BaseInformalAssessmentSecton implements OnInit, OnDestroy, SectionComponent, DoCheck {
  public model: PostSecondaryEducationSection;
  public originalModel: PostSecondaryEducationSection = new PostSecondaryEducationSection();
  public modelErrors: ModelErrors = {}; // Note: this must be initialized to an empty object for the UI in initial state
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public licenseTypeDrop: DropDownField[];
  public licenseValidDrop: DropDownField[];
  public collegesDrop: DropDownField[] = [];
  public degreeLevelDrop: DropDownField[];
  private checkThrottle = true;
  private currentDate: moment.Moment;
  private sectionSub: Subscription;

  constructor(
    private appService: AppService,
    private eiaService: InformalAssessmentEditService,
    private fdService: FieldDataService,
    private partService: ParticipantService,
    private logService: LogService,
    public modalService: ModalService
  ) {
    super(modalService, eiaService);
  }

  ngOnInit() {
    this.eiaService.setSectionComponent(this);

    forkJoin(
      this.partService.getParticipant(this.eiaService.getPin()).pipe(take(1)),
      this.fdService.getDegreeTypes().pipe(take(1)),
      this.fdService.getLicenseTypes().pipe(take(1)),
      this.fdService.getPolarInput().pipe(take(1))
    )
      .pipe(take(1))
      .subscribe(results => {
        this.initParticipantModel(results[0]);
        this.initDegreeLevelDropDowns(results[1]);
        this.initLicenseTypesDropDowns(results[2]);
        this.initIsValidDropDowns(results[3]);
        this.loadModel();
      });
  }

  private loadModel() {
    this.sectionSub = this.eiaService.getPostSecondaryEducationSection().subscribe(data => {
      this.initSection(data);
      // HACK: the modifiedTracker IF was added to check when loading up the main page.
      this.modifiedTrackerForcedValidation();
      this.sectionLoaded();
    });
  }

  modifiedTrackerForcedValidation() {
    if (this.eiaService.hasPostSecondaryValidated || this.eiaService.modifiedTracker.isPostSecondaryError) {
      this.isSectionValid = false;
      this.validate();
    }
  }

  ngOnDestroy() {
    if (this.sectionSub != null) {
      this.sectionSub.unsubscribe();
    }
  }

  private initDegreeLevelDropDowns(data) {
    this.degreeLevelDrop = data;
  }

  private initLicenseTypesDropDowns(data) {
    this.licenseTypeDrop = data;
  }

  private initIsValidDropDowns(data) {
    this.licenseValidDrop = data;
  }

  private initSection(model: PostSecondaryEducationSection) {
    // Ask the service if there are server errors.
    this.currentDate = Utilities.currentDate;
    this.currentDate.date(1);
    this.model = model;
    this.eiaService.postSecondaryEducationModel = model;

    // Cloned string for equality checking
    this.cloneModelString = JSON.stringify(model);

    // Pushes blank object inside of repeater when model has none.
    if (model.postSecondaryColleges != null) {
      if (model.postSecondaryColleges.length === 0) {
        const x = new PostSecondaryCollege();
        this.model.postSecondaryColleges.push(x);
      }
    }

    if (model.postSecondaryDegrees != null) {
      if (model.postSecondaryDegrees.length === 0) {
        const x = new PostSecondaryDegree();
        this.model.postSecondaryDegrees.push(x);
      }
    }

    if (model.postSecondaryLicenses != null) {
      if (model.postSecondaryLicenses.length === 0) {
        const x = new PostSecondaryLicense();
        this.model.postSecondaryLicenses.push(x);
      }
    }

    this.originalModel = this.eiaService.lastSavedPostSecondaryEducationModel;

    this.generateListOfColleges();
  }

  isValid(): boolean {
    return this.isSectionValid;
  }

  ngDoCheck() {}

  generateListOfColleges() {
    const dupList: string[] = [];
    if (this.model != null && this.model.postSecondaryColleges != null) {
      const clist: DropDownField[] = [];
      for (const c of this.model.postSecondaryColleges) {
        if (c.name != null && c.location != null) {
          const cTrim = c.name.trim();
          if (c.name != null && c.name.length !== 0) {
            const ddf = new DropDownField();
            ddf.name = c.name;
            ddf.id = c.name;
            if (dupList.indexOf(c.name) === -1) {
              clist.push(ddf);
            }
            dupList.push(c.name);
          }
        }
      }

      this.collegesDrop = clist;
    }
  }

  refreshModel() {
    if (this.model != null && this.eiaService.postSecondaryEducationModel != null) {
      this.initSection(this.eiaService.postSecondaryEducationModel);
      this.modifiedTrackerForcedValidation();
    }

    this.isSectionActive = false;
    setTimeout(() => (this.isSectionActive = true), 0);
  }

  validate(): void {
    // Clear all previous errors.
    this.validationManager.resetErrors();

    // Call the model's validate method.
    const result = this.model.validate(this.validationManager, this.participantDOB);

    // Update our properties so the UI can bind to the results.
    this.isSectionValid = result.isValid;
    this.modelErrors = result.errors;

    if (this.isSectionValid === true) {
      this.eiaService.sectionIsNowValid('post-secondary-education');
    }
  }

  checkState() {
    this.generateListOfColleges();

    if (!this.isSectionValid) {
      this.validate();
    }

    this.eiaService.setModifiedModel('post-secondary', this.cloneModelString !== JSON.stringify(this.eiaService.postSecondaryEducationModel));
  }

  onModelChange() {
    this.generateListOfColleges();

    // See if we are in "check state"
    if (!this.isSectionValid) {
      this.validate();
    }

    this.eiaService.setModifiedModel('post-secondary', this.cloneModelString !== JSON.stringify(this.eiaService.postSecondaryEducationModel));
  }

  prepareToSaveWithErrors(): PostSecondaryEducationSection {
    // make a clone
    const model = JSON.parse(JSON.stringify(this.model)) as PostSecondaryEducationSection;

    // Checking if expiredDate is valid.
    for (const element of model.postSecondaryLicenses) {
      if (element.expiredDate != null && element.expiredDate !== '') {
        if (element.expiredDate.length === 7 && element.expiredDate.indexOf('/') === 2 && element.expiredDate.lastIndexOf('/') === 2) {
          if (Number(element.expiredDate.split('/')[0]) <= 12 && Number(element.expiredDate.split('/')[0]) + 1 > 0) {
          } else {
            element.expiredDate = null;
          }
        } else {
          element.expiredDate = null;
        }
      }
    }

    for (const element of model.postSecondaryLicenses) {
      if (element.attainedDate != null && element.attainedDate !== '') {
        if (element.attainedDate.length === 7 && element.attainedDate.indexOf('/') === 2 && element.attainedDate.lastIndexOf('/') === 2) {
          if (Number(element.attainedDate.split('/')[0]) <= 12 && Number(element.attainedDate.split('/')[0]) + 1 > 0) {
          } else {
            element.attainedDate = null;
          }
        } else {
          element.attainedDate = null;
        }
      }
    }

    return model;
  }
}
