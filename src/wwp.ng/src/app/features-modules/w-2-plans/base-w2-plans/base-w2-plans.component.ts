import { W2PlansService } from './../services/w-2-plans.service';
import { W2AuxiliaryApproversComponent } from './../../auxiliary/w2-auxiliary-approvers/w2-auxiliary-approvers.component';
import { Utilities } from './../../../shared/utilities';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { W2PlanSection, W2PlanSectionResource } from './../models/w-2-plan.model';
import { Component, Input, OnInit, SimpleChanges } from '@angular/core';
import { ValidationCode, ValidationManager, ValidationResult } from 'src/app/shared/models/validation';
import { ModelErrors } from 'src/app/shared/interfaces/model-errors';
import * as util from 'src/app/shared/utilities';
import { PlanSectionTypes, W2PlanSections } from '../enums/w-2-plans-sections.enum';
import { Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { AppService } from 'src/app/core/services/app.service';

@Component({
  selector: 'app-base-w2-plans',
  templateUrl: './base-w2-plans.component.html',
  styleUrls: ['./base-w2-plans.component.scss']
})
export class BaseW2PlansComponent implements OnInit {
  public hadSaveError = false;

  public isSaving = false;
  public participantId: number;
  public id: number;
  public planTypeId: number;
  public planSections: DropDownField[] = [];
  public pin: string;
  public isDisableFields = false;
  public isSectionModified = false;
  public hasTriedSave = false;
  public isSectionValid = true;
  public modelErrors: ModelErrors = {};
  public goToUrl;

  @Input() sectionName: W2PlanSections;
  @Input() sectionNameForId: string;
  public sectionModel: W2PlanSection;
  public cachedSectionModel: W2PlanSection;

  public validationManager: ValidationManager;
  isLoaded: boolean;

  constructor(private router: Router, private plansService: W2PlansService, private appService: AppService) {}

  ngOnInit() {
    this.setUpInitialDataParent();
  }
  checkState() {
    if (this.sectionModel.isNotNeeded) {
      this.isDisableFields = true;
    } else {
      this.isDisableFields = false;
    }
  }

  validate() {
    this.isSectionModified = true;
    if (this.hasTriedSave) {
      this.validationManager.resetErrors();
      const result = new ValidationResult();
      if (!this.sectionModel.isNotNeeded) {
        if (
          util.isUndefined(this.sectionModel.longTermPlanOfAction) ||
          util.isUndefined(this.sectionModel.shortTermPlanOfAction) ||
          Utilities.isStringEmptyOrNull(this.sectionModel.longTermPlanOfAction) ||
          Utilities.isStringEmptyOrNull(this.sectionModel.shortTermPlanOfAction) ||
          this.sectionModel.planSectionResources.forEach(res => {
            util.isUndefined(res.resource) || Utilities.isStringEmptyOrNull(res.resource);
          })
        ) {
          this.validationManager.addErrorWithDetail(
            ValidationCode.RequiredInformationMissing_Details,
            'Data must be populated for Resources, Short Term Plan of action, and Long Term Plan of Action or Not Needed must be checked in order to proceed.”, disable the Save+Continue or Save+Exit button, and do not proceed to the next section.'
          );
          result.addError(`${this.sectionModel.id}`);
        }
      }
      this.isSectionValid = result.isValid;
      this.modelErrors = result.errors;
      if (!this.isSectionValid) {
        this.hadSaveError = true;
      }
    }
  }
  navigateTo(section: string) {
    this.router.navigateByUrl(`/pin/${this.pin}/w-2-plans/edit/${this.id}/${section}`);
  }
  goToSection(section: W2PlanSections) {
    this.goToUrl = section;
    this.navigateTo(section);
  }
  setUpInitialDataParent() {
    this.plansService.routeState.pipe(take(1)).subscribe(res => {
      this.pin = res.pin;
      this.planSections = res.planSections;
      this.planTypeId = res.planTypeId;
      this.participantId = res.participantId;
      if (!this.sectionModel || !this.sectionModel.hasOwnProperty('id')) {
        this.sectionModel = W2PlanSection.create();
        this.cachedSectionModel = W2PlanSection.create();
        this.sectionModel.planSectionTypeId = Utilities.idByFieldDataName(PlanSectionTypes[`${this.sectionNameForId}`], this.planSections);
        this.sectionModel.planSectionTypeName = Utilities.fieldDataNameById(this.sectionModel.planSectionTypeId, this.planSections);
        this.validationManager = new ValidationManager(this.appService);
      }
      if (this.sectionModel.planSectionResources !== null && this.sectionModel.planSectionResources.length === 0) {
        this.sectionModel.planSectionResources.push(W2PlanSectionResource.create());
      }
      this.isLoaded = true;
    });
  }
  saveAndContinue() {
    this.isSaving = true;
    this.hasTriedSave = true;
    this.validate();
    if (this.isSectionValid) {
      this.sectionModel.planTypeId = this.planTypeId;
      this.plansService.saveW2PlansSection(this.sectionModel, this.participantId).subscribe(
        res => {
          if (res) {
            this.id = res.planId;
            const w2PlanSectionsArray = Object.values(W2PlanSections);
            const indexOfCurrentSection = w2PlanSectionsArray.indexOf(this.sectionName);
            this.navigateTo(w2PlanSectionsArray[indexOfCurrentSection + 1]);
          }
        },
        err => {
          this.hadSaveError = true;
          this.isSaving = false;
        }
      );
    }
  }

  // ngOnChanges(changes: SimpleChanges) {
  //   for (const propName in changes) {
  //     if (changes.hasOwnProperty(propName)) {
  //       switch (propName) {
  //         case 'sectionName':
  //           this.setUpInitialDataParent();
  //           break;
  //         default:
  //           break;
  //       }
  //     }
  //   }
  // }
}
