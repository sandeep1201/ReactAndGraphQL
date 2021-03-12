import { EAHouseholdFinancialsSection } from './../models/ea-request-financials.model';
import { FieldDataService } from 'src/app/shared/services/field-data.service';
import { BaseService } from 'src/app/core/services/base.service';
import { Injectable } from '@angular/core';
import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
import { LogService } from 'src/app/shared/services/log.service';
import { Observable, of, forkJoin } from 'rxjs';
import { map, catchError, flatMap, take, concatMap } from 'rxjs/operators';
import { AppService } from 'src/app/core/services/app.service';
import { EARequestDemographicsSection } from '../models/ea-request-demographics.model';
import { EARequestSections } from '../models/ea-request-sections.enum';
import { Participant } from 'src/app/shared/models/participant';
import { Dictionary } from 'src/app/shared/dictionary';
import { ModelErrors } from 'src/app/shared/interfaces/model-errors';
import { EARequestEditComponent } from '../ea-request-history/edit/edit.component';
import { SectionComponent } from 'src/app/shared/interfaces/section-component';
import { EARequest } from '../models';
import { EARequestEmergencyTypeSection } from '../models/ea-request-emergency-type.model';
import { EAGroupMembers, EARequestParticipant } from '../models/ea-request-participant.model';
import { EAAgencySummarySection } from '../models/ea-request-agency-summary.model';
import { BooleanDictionary } from 'src/app/shared/interfaces/boolean-dictionary';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { FieldDataTypes } from 'src/app/shared/enums/field-data-types.enum';
import { ValidationManager } from 'src/app/shared/models/validation-manager';
import { ValidationResult } from 'src/app/shared/models/validation-result';

type ValidateCallback = (vm: ValidationManager) => ValidationResult;

@Injectable()
export class EARequestEditService extends BaseService {
  modifiedTracker = new EARequestModifiedTracker();
  eaRequestUrl: string;
  participant: Participant;
  parentComponent: EARequestEditComponent;
  sectionComponent: SectionComponent;
  model: EARequest;
  lastSavedModel: EARequest;

  public validationManagers: Dictionary<string, ValidationManager> = new Dictionary<string, ValidationManager>();
  public modelErrors: ModelErrors = {};
  public isSectionValidOnLoad: BooleanDictionary = {};
  private eaStatusesDrop: DropDownField[] = [];
  private eaIndividualTypesDrop: DropDownField[] = [];

  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService, private fdService: FieldDataService) {
    super(http, logService);
    this.eaRequestUrl = this.appService.apiServer + 'api/ea/request/';
  }

  public loadValidationContextsAndValidate(eaSection: EARequest, onSubmit = false): Observable<void> {
    this.validationManagers.setValue(EARequestSections.Demographics, new ValidationManager(this.appService));
    this.validationManagers.setValue(EARequestSections.Emergency, new ValidationManager(this.appService));
    this.validationManagers.setValue(EARequestSections.Members, new ValidationManager(this.appService));
    this.validationManagers.setValue(EARequestSections.Financials, new ValidationManager(this.appService));
    this.validationManagers.setValue(EARequestSections.AgencySummary, new ValidationManager(this.appService));
    return new Observable(o => {
      const result0 = this.eaStatusesDrop.length === 0 ? this.fdService.getFieldDataByField(FieldDataTypes.EAStatuses) : of(null);
      const result1 = this.eaIndividualTypesDrop.length === 0 ? this.fdService.getFieldDataByField(FieldDataTypes.EAIndividualTypes) : of(null);
      forkJoin(result0, result1)
        .pipe(
          take(1),
          concatMap(x => {
            if (x[0]) this.eaStatusesDrop = x[0];
            if (x[1]) this.eaIndividualTypesDrop = x[1];
            this.validate(eaSection, onSubmit);
            return of(null);
          })
        )
        .pipe(take(1))
        .subscribe(r => {
          this.modifiedTracker.isDemographics = {
            ...this.modifiedTracker.isDemographics,
            saved: (eaSection.eaDemographics.isSubmittedViaDriverFlow || onSubmit) && this.isSectionValidOnLoad[EARequestSections.Demographics],
            error: (eaSection.eaDemographics.isSubmittedViaDriverFlow || onSubmit) && !this.isSectionValidOnLoad[EARequestSections.Demographics],
            previouslyErrored: (eaSection.eaDemographics.isSubmittedViaDriverFlow || onSubmit) && !this.isSectionValidOnLoad[EARequestSections.Demographics]
          };
          this.modifiedTracker.isTypeOfEmergency = {
            ...this.modifiedTracker.isTypeOfEmergency,
            saved: (eaSection.eaEmergencyType.isSubmittedViaDriverFlow || onSubmit) && this.isSectionValidOnLoad[EARequestSections.Emergency],
            error: (eaSection.eaEmergencyType.isSubmittedViaDriverFlow || onSubmit) && !this.isSectionValidOnLoad[EARequestSections.Emergency],
            previouslyErrored: (eaSection.eaEmergencyType.isSubmittedViaDriverFlow || onSubmit) && !this.isSectionValidOnLoad[EARequestSections.Emergency]
          };
          this.modifiedTracker.isHouseholdMembers = {
            ...this.modifiedTracker.isHouseholdMembers,
            saved: (eaSection.eaGroupMembers.isSubmittedViaDriverFlow || onSubmit) && this.isSectionValidOnLoad[EARequestSections.Members],
            error: (eaSection.eaGroupMembers.isSubmittedViaDriverFlow || onSubmit) && !this.isSectionValidOnLoad[EARequestSections.Members],
            previouslyErrored: (eaSection.eaGroupMembers.isSubmittedViaDriverFlow || onSubmit) && !this.isSectionValidOnLoad[EARequestSections.Members]
          };
          this.modifiedTracker.isHouseholdFinancials = {
            ...this.modifiedTracker.isHouseholdFinancials,
            saved: (eaSection.eaHouseHoldFinancials.isSubmittedViaDriverFlow || onSubmit) && this.isSectionValidOnLoad[EARequestSections.Financials],
            error: (eaSection.eaHouseHoldFinancials.isSubmittedViaDriverFlow || onSubmit) && !this.isSectionValidOnLoad[EARequestSections.Financials],
            previouslyErrored: (eaSection.eaHouseHoldFinancials.isSubmittedViaDriverFlow || onSubmit) && !this.isSectionValidOnLoad[EARequestSections.Financials]
          };
          this.modifiedTracker.isAgencySummary = {
            ...this.modifiedTracker.isAgencySummary,
            saved: (eaSection.eaAgencySummary.isSubmittedViaDriverFlow || onSubmit) && this.isSectionValidOnLoad[EARequestSections.AgencySummary],
            error: (eaSection.eaAgencySummary.isSubmittedViaDriverFlow || onSubmit) && !this.isSectionValidOnLoad[EARequestSections.AgencySummary],
            previouslyErrored: (eaSection.eaAgencySummary.isSubmittedViaDriverFlow || onSubmit) && !this.isSectionValidOnLoad[EARequestSections.AgencySummary]
          };
          o.complete();
        });
    });
  }

  public validate(ea: EARequest, onSubmit = false): void {
    // Validate each section.
    if (ea.eaDemographics !== null && (ea.eaDemographics.isSubmittedViaDriverFlow || onSubmit)) {
      this.validateSectionOnLoad(EARequestSections.Demographics, vm => {
        return ea.eaDemographics.validate(vm);
      });
    }
    if (ea.eaEmergencyType !== null && (ea.eaEmergencyType.isSubmittedViaDriverFlow || onSubmit)) {
      this.validateSectionOnLoad(EARequestSections.Emergency, vm => {
        return ea.eaEmergencyType.validate(vm, ea);
      });
    }
    if (ea.eaGroupMembers !== null && (ea.eaGroupMembers.isSubmittedViaDriverFlow || onSubmit)) {
      this.validateSectionOnLoad(EARequestSections.Members, vm => {
        return EARequestParticipant.validate(vm, ea.eaGroupMembers.eaGroupMembers, this.eaIndividualTypesDrop);
      });
    }
    if (ea.eaHouseHoldFinancials !== null && (ea.eaHouseHoldFinancials.isSubmittedViaDriverFlow || onSubmit)) {
      this.validateSectionOnLoad(EARequestSections.Financials, vm => {
        return ea.eaHouseHoldFinancials.validate(vm);
      });
    }
    if (ea.eaAgencySummary !== null && (ea.eaAgencySummary.isSubmittedViaDriverFlow || onSubmit)) {
      this.validateSectionOnLoad(EARequestSections.AgencySummary, vm => {
        return ea.eaAgencySummary.validate(vm, this.eaStatusesDrop, ea);
      });
    }
  }

  private validateSectionOnLoad(sectionName: EARequestSections, validateCallback: ValidateCallback): void {
    const vm = this.validationManagers.getValue(sectionName);

    if (vm == null) {
      this.logService.error(sectionName + ' validation manager is null');
    } else {
      vm.resetErrors();
      const result = validateCallback(vm);

      this.isSectionValidOnLoad[sectionName] = result.isValid;
      this.modelErrors[sectionName] = result.errors;
    }
  }

  setParentComponent(parent: EARequestEditComponent) {
    this.parentComponent = parent;
  }

  setSectionComponent(child: SectionComponent) {
    this.sectionComponent = child;
  }

  setSaveButtonStatus(status = false) {
    this.modifiedTracker.isSaveDisabled = status;
  }

  isSectionValid(): boolean {
    return this.sectionComponent.isValid();
  }

  setModifiedModel(section: 'isDemographics' | 'isTypeOfEmergency' | 'isHouseholdMembers' | 'isHouseholdFinancials' | 'isAgencySummary', isModified: boolean) {
    this.modifiedTracker[section] = {
      ...this.modifiedTracker.sectionStatuses,
      validated: this.modifiedTracker[section].validated,
      previouslyErrored: this.modifiedTracker[section].previouslyErrored,
      modified: isModified,
      error: isModified ? false : this.modifiedTracker[section].previouslyErrored
    };
    this.updateModifiedTrackerDetectChange();
  }

  getSavableSectionModel(): any {
    return this.sectionComponent.prepareToSaveWithErrors();
  }

  refreshModel() {
    this.sectionComponent.refreshModel();
  }

  sectionIsNowValid(section: 'isDemographics' | 'isTypeOfEmergency' | 'isHouseholdMembers' | 'isHouseholdFinancials' | 'isAgencySummary') {
    this.parentComponent.sectionIsNowValid();
    this.modifiedTracker[section] = {
      ...this.modifiedTracker[section],
      validated: false
    };
  }

  validateSection(section: 'isDemographics' | 'isTypeOfEmergency' | 'isHouseholdMembers' | 'isHouseholdFinancials' | 'isAgencySummary') {
    this.sectionComponent.validate();
    this.modifiedTracker[section] = {
      ...this.modifiedTracker[section],
      validated: true
    };
  }

  updateModifiedTrackerDetectChange() {
    if (
      this.modifiedTracker.isDemographics.modified ||
      this.modifiedTracker.isDemographics.saving ||
      this.modifiedTracker.isTypeOfEmergency.modified ||
      this.modifiedTracker.isTypeOfEmergency.saving ||
      this.modifiedTracker.isHouseholdMembers.modified ||
      this.modifiedTracker.isHouseholdMembers.saving ||
      this.modifiedTracker.isHouseholdFinancials.modified ||
      this.modifiedTracker.isHouseholdFinancials.saving ||
      this.modifiedTracker.isAgencySummary.modified ||
      this.modifiedTracker.isAgencySummary.saving
    ) {
      this.modifiedTracker.isAnyChanged = true;
    } else this.modifiedTracker.isAnyChanged = false;
    this.appService.isUrlChangeBlocked = this.modifiedTracker.isAnyChanged;
  }

  public getEARequest(pin: string, id: string): Observable<EARequest> {
    if (this.model != null && this.model.id === +id) {
      return of(this.model);
    }
    if (id === '0') {
      this.model = EARequest.create();
      this.lastSavedModel = EARequest.create();
      return of(this.model);
    }
    const url = `${this.eaRequestUrl}${pin}/${id}`;
    return this.http.get(url).pipe(
      map(this.extractEAData),
      map(x => {
        this.model = x;
        this.lastSavedModel = new EARequest();
        EARequest.clone(x, this.lastSavedModel);
        return x;
      }),
      catchError(this.handleError)
    );
  }

  public getAgencySummary(pin: string, id: string): Observable<EARequest> {
    const url = `${this.eaRequestUrl}agency-summary/${pin}/${id}`;
    return this.model
      ? this.http.get(url).pipe(
          map(this.extractAgencySummarySectionData),
          map(x => {
            this.model.eaAgencySummary = x as EAAgencySummarySection;
            this.lastSavedModel.eaAgencySummary = new EAAgencySummarySection();
            EAAgencySummarySection.clone(x, this.lastSavedModel.eaAgencySummary);
            return this.model;
          }),
          catchError(this.handleError)
        )
      : this.getEARequest(pin, id);
  }

  private extractEAData(res: EARequest): EARequest {
    const body = res as EARequest;
    const eaRequest = new EARequest().deserialize(body);
    return eaRequest || null;
  }

  postDemographicsSection(section: EARequestDemographicsSection, isValid: boolean): Observable<EARequest> {
    this.modifiedTracker.isDemographics = {
      modified: false,
      saving: true,
      saved: false,
      error: false,
      previouslyErrored: false,
      validated: this.modifiedTracker.isDemographics.validated
    };

    return this.http.post(`${this.eaRequestUrl}demographics/${this.participant.pin}`, section).pipe(
      map(this.extractEAData),
      flatMap(x => {
        this.model = x as EARequest;
        this.lastSavedModel = new EARequest();
        EARequest.clone(x, this.lastSavedModel);

        this.modifiedTracker.isDemographics = {
          modified: false,
          saving: false,
          saved: isValid,
          error: !isValid,
          previouslyErrored: !isValid,
          validated: this.modifiedTracker.isDemographics.validated
        };
        this.updateModifiedTrackerDetectChange();
        return of(x);
      }),
      catchError(e => {
        this.modifiedTracker.isDemographics = {
          modified: false,
          saving: false,
          saved: false,
          error: true,
          previouslyErrored: true,
          validated: this.modifiedTracker.isDemographics.validated
        };
        this.updateModifiedTrackerDetectChange();
        return this.handleError(e);
      })
    );
  }

  postEmergencyTypeSection(section: EARequestEmergencyTypeSection, isValid: boolean): Observable<EARequestEmergencyTypeSection> {
    this.modifiedTracker.isTypeOfEmergency = {
      modified: false,
      saving: true,
      saved: false,
      error: false,
      previouslyErrored: false,
      validated: this.modifiedTracker.isTypeOfEmergency.validated
    };

    section.requestId = this.model.id;
    return this.http.post(`${this.eaRequestUrl}emergencytype/${this.participant.pin}`, section).pipe(
      map(this.extractEmergencyTypeSectionData),
      flatMap(x => {
        // When we attempt a post, we need to save the local.
        this.model.eaEmergencyType = x as EARequestEmergencyTypeSection;
        this.lastSavedModel.eaEmergencyType = new EARequestEmergencyTypeSection();
        EARequestEmergencyTypeSection.clone(x, this.lastSavedModel.eaEmergencyType);

        this.modifiedTracker.isTypeOfEmergency = {
          modified: false,
          saving: false,
          saved: isValid,
          error: !isValid,
          previouslyErrored: !isValid,
          validated: this.modifiedTracker.isTypeOfEmergency.validated
        };
        this.updateModifiedTrackerDetectChange();
        return of(x);
      }),
      catchError(e => {
        this.modifiedTracker.isTypeOfEmergency = {
          modified: false,
          saving: false,
          saved: false,
          error: true,
          previouslyErrored: true,
          validated: this.modifiedTracker.isTypeOfEmergency.validated
        };
        this.updateModifiedTrackerDetectChange();
        return this.handleError(e);
      })
    );
  }

  private extractEmergencyTypeSectionData(res: EARequestEmergencyTypeSection) {
    const body = res as EARequestEmergencyTypeSection;
    const ls = new EARequestEmergencyTypeSection().deserialize(body);
    return ls || null;
  }

  postHouseholdMemberseSection(section: EAGroupMembers, isValid: boolean, validationRequired = true): Observable<EAGroupMembers> {
    if (validationRequired) {
      this.modifiedTracker.isHouseholdMembers = {
        modified: false,
        saving: true,
        saved: false,
        error: false,
        previouslyErrored: false,
        validated: this.modifiedTracker.isHouseholdMembers.validated
      };
    }

    section.requestId = this.model.id;
    return this.http.post(`${this.eaRequestUrl}householdmembers/${this.participant.pin}`, section).pipe(
      map(this.extractHouseholdMembersSectionData),
      flatMap(x => {
        // When we attempt a post, we need to save the local.
        this.model.eaGroupMembers = x as EAGroupMembers;
        this.lastSavedModel.eaGroupMembers = new EAGroupMembers();
        EAGroupMembers.clone(x, this.lastSavedModel.eaGroupMembers);

        if (validationRequired) {
          this.modifiedTracker.isHouseholdMembers = {
            modified: false,
            saving: false,
            saved: isValid,
            error: !isValid,
            previouslyErrored: !isValid,
            validated: this.modifiedTracker.isHouseholdMembers.validated
          };
        }
        this.updateModifiedTrackerDetectChange();
        return of(x);
      }),
      catchError(e => {
        if (validationRequired) {
          this.modifiedTracker.isHouseholdMembers = {
            modified: false,
            saving: false,
            saved: false,
            error: true,
            previouslyErrored: true,
            validated: this.modifiedTracker.isHouseholdMembers.validated
          };
        }
        this.updateModifiedTrackerDetectChange();
        return this.handleError(e);
      })
    );
  }

  private extractHouseholdMembersSectionData(res: EAGroupMembers) {
    const body = res as EAGroupMembers;
    const ls = new EAGroupMembers().deserialize(body);
    return ls || null;
  }

  postHouseholdFinancialsSection(section: EAHouseholdFinancialsSection, isValid: boolean): Observable<EAHouseholdFinancialsSection> {
    this.modifiedTracker.isHouseholdFinancials = {
      modified: false,
      saving: true,
      saved: false,
      error: false,
      previouslyErrored: false,
      validated: this.modifiedTracker.isHouseholdFinancials.validated
    };

    section.requestId = this.model.id;
    return this.http.post(`${this.eaRequestUrl}householdfinancials/${this.participant.pin}`, section).pipe(
      map(this.extractHouseholdFinancialsSectionData),
      flatMap(x => {
        // When we attempt a post, we need to save the local.
        this.model.eaHouseHoldFinancials = x as EAHouseholdFinancialsSection;
        this.lastSavedModel.eaHouseHoldFinancials = new EAHouseholdFinancialsSection();
        EAHouseholdFinancialsSection.clone(x, this.lastSavedModel.eaHouseHoldFinancials);

        this.modifiedTracker.isHouseholdFinancials = {
          modified: false,
          saving: false,
          saved: isValid,
          error: !isValid,
          previouslyErrored: !isValid,
          validated: this.modifiedTracker.isHouseholdFinancials.validated
        };
        this.updateModifiedTrackerDetectChange();
        return of(x);
      }),
      catchError(e => {
        this.modifiedTracker.isHouseholdFinancials = {
          modified: false,
          saving: false,
          saved: false,
          error: true,
          previouslyErrored: true,
          validated: this.modifiedTracker.isHouseholdFinancials.validated
        };
        this.updateModifiedTrackerDetectChange();
        return this.handleError(e);
      })
    );
  }

  private extractHouseholdFinancialsSectionData(res: EAHouseholdFinancialsSection) {
    const body = res as EAHouseholdFinancialsSection;
    const ls = new EAHouseholdFinancialsSection().deserialize(body);
    return ls || null;
  }

  postAgencySummarySection(section: EAAgencySummarySection, isValid: boolean): Observable<EAAgencySummarySection> {
    this.modifiedTracker.isAgencySummary = {
      modified: false,
      saving: true,
      saved: false,
      error: false,
      previouslyErrored: false,
      validated: this.modifiedTracker.isAgencySummary.validated
    };

    section.requestId = this.model.id;
    section.isSubmit = this.modifiedTracker.isSubmitEnabled;
    return this.http.post(`${this.eaRequestUrl}agencysummary/${this.participant.pin}`, section).pipe(
      map(this.extractAgencySummarySectionData),
      flatMap(x => {
        // When we attempt a post, we need to save the local.
        this.model.eaAgencySummary = x as EAAgencySummarySection;
        this.lastSavedModel.eaAgencySummary = new EAAgencySummarySection();
        EAAgencySummarySection.clone(x, this.lastSavedModel.eaAgencySummary);

        this.modifiedTracker.isAgencySummary = {
          modified: false,
          saving: false,
          saved: isValid,
          error: !isValid,
          previouslyErrored: !isValid,
          validated: this.modifiedTracker.isAgencySummary.validated
        };
        this.updateModifiedTrackerDetectChange();
        return of(x);
      }),
      catchError(e => {
        this.modifiedTracker.isAgencySummary = {
          modified: false,
          saving: false,
          saved: false,
          error: true,
          previouslyErrored: true,
          validated: this.modifiedTracker.isAgencySummary.validated
        };
        this.updateModifiedTrackerDetectChange();
        return this.handleError(e);
      })
    );
  }

  private extractAgencySummarySectionData(res: EAAgencySummarySection) {
    const body = res as EAAgencySummarySection;
    const ls = new EAAgencySummarySection().deserialize(body);
    return ls || null;
  }

  getSectionLabel(section: EARequestSections) {
    let result: 'isDemographics' | 'isTypeOfEmergency' | 'isHouseholdMembers' | 'isHouseholdFinancials' | 'isAgencySummary';
    if (section === EARequestSections.Demographics) result = 'isDemographics';
    else if (section === EARequestSections.Emergency) result = 'isTypeOfEmergency';
    else if (section === EARequestSections.Members) result = 'isHouseholdMembers';
    else if (section === EARequestSections.Financials) result = 'isHouseholdFinancials';
    else if (section === EARequestSections.AgencySummary) result = 'isAgencySummary';
    return result;
  }
}

class EARequestModifiedTracker {
  public sectionStatuses = {
    modified: false,
    saving: false,
    saved: false,
    error: false,
    validated: false,
    previouslyErrored: false
  };

  isAnyChanged = false;
  isSaveDisabled = false;
  isSubmitEnabled = false;

  isDemographics = this.sectionStatuses;
  isTypeOfEmergency = this.sectionStatuses;
  isHouseholdMembers = this.sectionStatuses;
  isHouseholdFinancials = this.sectionStatuses;
  isAgencySummary = this.sectionStatuses;
}
