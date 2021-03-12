// tslint:disable: import-blacklist
// tslint:disable: deprecation
// tslint:disable: no-shadowed-variable
import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { Observable, forkJoin, throwError } from 'rxjs';
import { ActionNeeded } from '../../features-modules/actions-needed/models/action-needed';
import { AppService } from './../../core/services/app.service';
import { AuthHttpClient } from './../../core/interceptors/AuthHttpClient';
import { BooleanDictionary } from '../interfaces/boolean-dictionary';
import { Dictionary } from '../dictionary';
import { DropDownField } from '../models/dropdown-field';
import { Employment } from '../models/work-history-app';
import { FieldDataService } from '../services/field-data.service';
import { InformalAssessment } from '../models/informal-assessment';
import { LogService } from './log.service';
import { ModelErrors } from '../interfaces/model-errors';
import { Participant } from '../models/participant';
import { Utilities } from '../utilities';
import { ValidationManager } from '../models/validation-manager';
import { ValidationResult } from '../models/validation-result';
import { WorkHistoryAppService } from '../services/work-history-app.service';
import * as moment from 'moment';
import { Authorization } from '../models/authorization';
import { ParticipantService } from './participant.service';
import { map, catchError } from 'rxjs/operators';

type ValidateCallback = (vm: ValidationManager) => ValidationResult;

@Injectable()
export class InformalAssessmentService {
  private iaUrl: string;

  private historyIaUrl: string;

  private pin: string;

  public participant: Participant;

  private participantDob: moment.Moment;

  // History
  public isChildCareHistoryActive = false;
  public isEducationHistoryActive = false;
  public isFamilyBarriersHistoryActive = false;
  public isHousingHistoryActive = false;
  public isLanguageHistoryActive = false;
  public isLegalIssuesHistoryActive = false;
  public isMilitaryHistoryActive = false;
  public isNonCustodialParentsHistoryActive = false;
  public isPostSecondaryHistoryActive = false;
  public isParticipantBarriersHistoryActive = false;
  public isTransportationHistoryActive = false;
  public isWorkHistoryHistoryActive = false;
  public isWorkProgramsHistoryActive = false;
  //public canRequestPBAccess = false;

  // Validation context properties
  // Language
  public englishId: number;

  // Work Histpry
  private workBarriersActionNeededs: DropDownField[];
  private workHistoryEmploymentStatuses: DropDownField[];
  private employments: Employment[];
  private whPolarDrop: DropDownField[];

  // Work Programs
  private workProgramStatusesDrop: DropDownField[];
  private otherWorkProgramId: number;

  // Education History.
  private educationDiplomaDrop: DropDownField[];
  private diplomaId: number;
  private gedId: number;
  private hsedId: number;
  private noneId: number;

  // Housing.
  private otherHousingSituationId: number;
  private homelessId: number;
  private housingActionNeededs: ActionNeeded[];

  // Transportation.
  private driverLicenseStatusesDrop: DropDownField[];
  private transportationTypes: DropDownField[];
  private transportationActionNeededs: ActionNeeded[];

  // Legal Issues.
  private legalIssuesActionNeededs: ActionNeeded[];

  // Child and Youth.
  private childYouthActionNeededs: ActionNeeded[];

  // Family Barriers.
  private ssiApplicationStatuses: DropDownField[];
  private famBarActionNeededs: ActionNeeded[];

  // Participant Barriers.
  private polarRefusedYesId: number;

  // NCP.
  private ncpRelationshipDrop: DropDownField[];
  private contactIntervalDrop: DropDownField[];

  private ncpActionNeededs: ActionNeeded[];

  // NCPR.
  private polarDrop: DropDownField[];
  public validationManagers: Dictionary<string, ValidationManager> = new Dictionary<string, ValidationManager>();
  public modelErrors: ModelErrors = {};
  public isSectionValid: BooleanDictionary = {};
  public hasPBAccessBol: boolean;
  public requestedElevatedAccess: boolean;
  public canRequestPBAccess: boolean;
  public hasFBAccessBol: boolean;
  public canRequestFBAccess: boolean;
  public showCareerFeature = false;
  get canAccessFamilyBarriersSsi(): boolean {
    return this.appService.isUserAuthorized(Authorization.canAccessFamilyBarriersSsi, null);
  }

  constructor(
    private http: AuthHttpClient,
    private appService: AppService,
    private logService: LogService,
    private fdService: FieldDataService,
    private whaService: WorkHistoryAppService,
    private participantService: ParticipantService
  ) {
    this.iaUrl = this.appService.apiServer + 'api/ia/';
    this.historyIaUrl = this.appService.apiServer + 'api/history/ia/';
  }

  public initPbvars(pin: string) {
    this.participantService.getCachedParticipant(pin).subscribe(part => {
      if (part) {
        this.appService.hasPBAccess(part);
        this.appService.hasPHIAccess(part);
        this.appService.PBSection.subscribe(data => {
          this.hasPBAccessBol = data.hasPBAccessBol;
          this.canRequestPBAccess = data.canRequestPBAccess;
          this.requestedElevatedAccess = data.requestedElevatedAccess;
        });
        this.appService.FBSection.subscribe(data => {
          this.hasFBAccessBol = data.hasFBAccessBol;
          this.canRequestFBAccess = data.canRequestFBAccess;
        });
      }
    });
  }

  public getInformalAssessment(pin: string): Observable<InformalAssessment> {
    return this.http.get(this.iaUrl + pin).pipe(
      map(this.extractData),
      catchError(err => this.handleError(err))
    );
  }

  public getHistoryBySection(pin: string, section: string): Observable<any> {
    return this.http.get(this.historyIaUrl + section + '/' + pin).pipe(
      map(this.extractSectionData),
      catchError(err => this.handleError(err))
    );
  }

  private extractSectionData(res: Response) {
    const body = res;
    return body || null;
  }

  private extractData(res: InformalAssessment) {
    const body = res as InformalAssessment;
    const ia = new InformalAssessment().deserialize(body);
    return ia || null;
  }

  private handleError(error: any) {
    const errMsg = error.message ? error.message : error.status ? `${error.status} - ${error.statusText}` : 'Server error';

    if (this.logService) {
      this.logService.error(errMsg);
    }

    return throwError(errMsg);
  }

  public createNewAssessment(participant: Participant): Observable<void> {
    const body = JSON.stringify('');

    return this.http.post(this.iaUrl + 'new/' + participant.pin, body).pipe(catchError(this.handleError));
  }

  public submitAssessment(participant: Participant): Observable<void> {
    const body = JSON.stringify('');

    return this.http.post(this.iaUrl + 'submit/' + participant.pin, body).pipe(catchError(this.handleError));
  }

  public loadValidationContextsAndValidate(participant: Participant, assessment: InformalAssessment): Observable<void> {
    // This is the first time we obtaint the participant so save it.
    this.participant = participant;
    this.participantDob = moment(this.participant.dateOfBirth);
    this.pin = participant.pin;
    this.whaService.setPin(this.pin);

    // Initialize dictionaries.
    this.validationManagers.setValue(InformalAssessment.LanguagesSectionName, new ValidationManager(this.appService));
    this.validationManagers.setValue(InformalAssessment.WorkHistorySectionName, new ValidationManager(this.appService));
    this.validationManagers.setValue(InformalAssessment.WorkProgramsSectionName, new ValidationManager(this.appService));
    this.validationManagers.setValue(InformalAssessment.EducationHistorySectionName, new ValidationManager(this.appService));
    this.validationManagers.setValue(InformalAssessment.PostSecondaryEducationSectionName, new ValidationManager(this.appService));
    this.validationManagers.setValue(InformalAssessment.MilitaryTrainingSectionName, new ValidationManager(this.appService));
    this.validationManagers.setValue(InformalAssessment.HousingSectionName, new ValidationManager(this.appService));
    this.validationManagers.setValue(InformalAssessment.TransportationSectionName, new ValidationManager(this.appService));
    this.validationManagers.setValue(InformalAssessment.LegalIssuesSectionName, new ValidationManager(this.appService));
    this.validationManagers.setValue(InformalAssessment.ParticipantBarriersSectionName, new ValidationManager(this.appService));
    this.validationManagers.setValue(InformalAssessment.ChildYouthSupportsSectionName, new ValidationManager(this.appService));
    this.validationManagers.setValue(InformalAssessment.FamilyBarriersSectionName, new ValidationManager(this.appService));
    this.validationManagers.setValue(InformalAssessment.NonCustodialParentsSectionName, new ValidationManager(this.appService));
    this.validationManagers.setValue(InformalAssessment.NonCustodialParentsReferralSectionName, new ValidationManager(this.appService));
    this.showCareerFeature = this.appService.getFeatureToggleDate('CareerAndJobReadiness');

    const obs = Observable.create(o => {
      // Use a forkJoin so that all the requests can kick off in parallel.
      forkJoin(
        this.fdService.getLanguages().pipe(map(data => (this.englishId = Utilities.idByFieldDataName('English', data)))),
        this.fdService.getEmploymentPreventionFactors().pipe(map(data => (this.workBarriersActionNeededs = data))),
        // Work History.
        this.fdService.getWorkHistoryEmploymentStatuses().pipe(map(data => (this.workHistoryEmploymentStatuses = data))),
        this.whaService.getEmploymentList().pipe(map(data => (this.employments = data))),
        this.fdService.getPolarUnknown().pipe(map(data => (this.whPolarDrop = data))),
        // Work Programs.
        this.fdService.getWorkProgramStatuses().pipe(map(data => (this.workProgramStatusesDrop = data))),
        this.fdService.getWorkPrograms().pipe(
          map(data => {
            this.otherWorkProgramId = Utilities.idByFieldDataName('Other', data);
          })
        ),
        // Education History.
        this.fdService.getEducationDiplomaTypes().pipe(
          map(data => {
            (this.diplomaId = Utilities.idByFieldDataName('Diploma', data)),
              (this.gedId = Utilities.idByFieldDataName('GED', data)),
              (this.hsedId = Utilities.idByFieldDataName('HSED', data)),
              (this.noneId = Utilities.idByFieldDataName('NONE', data));
          })
        ),

        // Housing.
        this.fdService.getHousingSituations().pipe(
          map(data => {
            (this.homelessId = Utilities.idByFieldDataName('Homeless (Outside of shelter)', data)), (this.otherHousingSituationId = Utilities.idByFieldDataName('Other', data));
          })
        ),

        // Transportation.
        this.fdService.getDriverLicenseStatuses().pipe(map(data => (this.driverLicenseStatusesDrop = data))),
        this.fdService.getTransportationTypes().pipe(map(data => (this.transportationTypes = data))),
        this.fdService.getPolarRefused().pipe(
          map(data => {
            this.polarRefusedYesId = Utilities.idByFieldDataName('Yes', data);
          })
        ),
        // Family Barriers.
        this.fdService.getSsiApplicationStatuses().pipe(map(data => (this.ssiApplicationStatuses = data))),
        // NCP.
        this.fdService.getNcpRelationships().pipe(map(data => (this.ncpRelationshipDrop = data))),
        this.fdService.getContactIntervals().pipe(map(data => (this.contactIntervalDrop = data))),
        // NCPR.
        this.fdService.getPolarInput().pipe(map(data => (this.polarDrop = data)))
      ).subscribe(r => {
        this.validate(assessment);
        // We don't need to indicate to the observer that there is a next...
        // we only care about indicating we are coplete.
        // o.next();
        o.complete();
      });
    });

    return obs;
  }

  public validate(ia: InformalAssessment): void {
    // Validate each section.
    if (ia.languagesSection != null && ia.languagesSection.isSubmittedViaDriverFlow) {
      this.validateSection(InformalAssessment.LanguagesSectionName, vm => {
        return ia.languagesSection.validate(vm, this.englishId);
      });
    }

    if (ia.workHistorySection != null && ia.workHistorySection.isSubmittedViaDriverFlow) {
      this.validateSection(InformalAssessment.WorkHistorySectionName, vm => {
        return ia.workHistorySection.validate(
          vm,
          this.workHistoryEmploymentStatuses,
          this.workBarriersActionNeededs,
          this.employments,
          this.whPolarDrop,
          this.showCareerFeature,
          !this.appService.isUserAuthorizedToEditWorkHistory(this.participant)
        );
      });
    }

    if (ia.workProgramSection != null && ia.workProgramSection.isSubmittedViaDriverFlow) {
      this.validateSection(InformalAssessment.WorkProgramsSectionName, vm => {
        return ia.workProgramSection.validate(vm, this.workProgramStatusesDrop, this.otherWorkProgramId, this.participantDob);
      });
    }

    if (ia.educationSection != null && ia.educationSection.isSubmittedViaDriverFlow) {
      this.validateSection(InformalAssessment.EducationHistorySectionName, vm => {
        return ia.educationSection.validate(vm, this.participantDob, this.diplomaId, this.gedId, this.hsedId, this.noneId);
      });
    }

    if (ia.postSecondarySection != null && ia.postSecondarySection.isSubmittedViaDriverFlow) {
      this.validateSection(InformalAssessment.PostSecondaryEducationSectionName, vm => {
        return ia.postSecondarySection.validate(vm, this.participantDob);
      });
    }

    if (ia.militarySection != null && ia.militarySection.isSubmittedViaDriverFlow) {
      this.validateSection(InformalAssessment.MilitaryTrainingSectionName, vm => {
        return ia.militarySection.validate(vm, this.participantDob.format('MM/DD/YYYY'));
      });
    }

    if (ia.housingSection != null && ia.housingSection.isSubmittedViaDriverFlow) {
      this.validateSection(InformalAssessment.HousingSectionName, vm => {
        return ia.housingSection.validate(vm, this.participantDob.format('MM/DD/YYYY'), this.otherHousingSituationId, this.homelessId);
      });
    }

    if (ia.transportationSection != null && ia.transportationSection.isSubmittedViaDriverFlow) {
      this.validateSection(InformalAssessment.TransportationSectionName, vm => {
        return ia.transportationSection.validate(vm, this.participantDob.format('MM/DD/YYYY'), this.transportationTypes, this.driverLicenseStatusesDrop);
      });
    }

    if (ia.legalIssuesSection != null && ia.legalIssuesSection.isSubmittedViaDriverFlow) {
      this.validateSection(InformalAssessment.LegalIssuesSectionName, vm => {
        return ia.legalIssuesSection.validate(vm, this.participantDob.format('MM/DD/YYYY'));
      });
    }
    // todo handle this to work with new access rules
    if (ia.participantBarriersSection != null && ia.participantBarriersSection.isSubmittedViaDriverFlow) {
      this.validateSection(InformalAssessment.ParticipantBarriersSectionName, vm => {
        return ia.participantBarriersSection.validate(vm, this.polarRefusedYesId, this.hasPBAccessBol, this.canRequestPBAccess, this.requestedElevatedAccess);
      });
    }

    if (ia.childYouthSupportsSection != null && ia.childYouthSupportsSection.isSubmittedViaDriverFlow) {
      this.validateSection(InformalAssessment.ChildYouthSupportsSectionName, vm => {
        return ia.childYouthSupportsSection.validate(vm, this.participant, this.logService);
      });
    }

    if (ia.familyBarriersSection != null && ia.familyBarriersSection.isSubmittedViaDriverFlow) {
      this.validateSection(InformalAssessment.FamilyBarriersSectionName, vm => {
        return ia.familyBarriersSection.validate(vm, this.participantDob, this.ssiApplicationStatuses, this.canAccessFamilyBarriersSsi);
      });
    }

    if (ia.nonCustodialParentsSection != null && ia.nonCustodialParentsSection.isSubmittedViaDriverFlow) {
      this.validateSection(InformalAssessment.NonCustodialParentsSectionName, vm => {
        return ia.nonCustodialParentsSection.validate(vm, this.polarDrop, this.ncpRelationshipDrop, this.contactIntervalDrop, this.participantDob.format('MM/DD/YYYY'));
      });
    }

    if (ia.nonCustodialParentsReferralSection != null && ia.nonCustodialParentsReferralSection.isSubmittedViaDriverFlow) {
      this.validateSection(InformalAssessment.NonCustodialParentsReferralSectionName, vm => {
        return ia.nonCustodialParentsReferralSection.validate(vm, this.polarDrop);
      });
    }
  }

  private validateSection(sectionName: string, validateCallback: ValidateCallback): void {
    const vm = this.validationManagers.getValue(sectionName);

    if (vm == null) {
      this.logService.error(sectionName + ' validation manager is null');
    } else {
      vm.resetErrors();
      const result = validateCallback(vm);

      this.isSectionValid[sectionName] = result.isValid;
      this.modelErrors[sectionName] = result.errors;
    }
  }

  public areAllSectionsValid(canSubmitEmptyLanguage = false, isCysSectionEmpty = false, canSubmitEmptyWorkProgram = false, canSubmitEmptyFamilyBarriers = false): boolean {
    if (this.hasPBAccessBol) {
      return (
        (this.isSectionValid[InformalAssessment.LanguagesSectionName] || canSubmitEmptyLanguage) &&
        this.isSectionValid[InformalAssessment.WorkHistorySectionName] &&
        (this.isSectionValid[InformalAssessment.WorkProgramsSectionName] || canSubmitEmptyWorkProgram) &&
        this.isSectionValid[InformalAssessment.EducationHistorySectionName] &&
        this.isSectionValid[InformalAssessment.PostSecondaryEducationSectionName] &&
        this.isSectionValid[InformalAssessment.MilitaryTrainingSectionName] &&
        this.isSectionValid[InformalAssessment.HousingSectionName] &&
        this.isSectionValid[InformalAssessment.TransportationSectionName] &&
        this.isSectionValid[InformalAssessment.LegalIssuesSectionName] &&
        this.isSectionValid[InformalAssessment.ParticipantBarriersSectionName] &&
        (this.isSectionValid[InformalAssessment.ChildYouthSupportsSectionName] || isCysSectionEmpty) &&
        (this.isSectionValid[InformalAssessment.FamilyBarriersSectionName] || canSubmitEmptyFamilyBarriers) &&
        this.isSectionValid[InformalAssessment.NonCustodialParentsSectionName] &&
        this.isSectionValid[InformalAssessment.NonCustodialParentsReferralSectionName]
      );
    } else {
      return (
        this.isSectionValid[InformalAssessment.LanguagesSectionName] &&
        this.isSectionValid[InformalAssessment.WorkHistorySectionName] &&
        this.isSectionValid[InformalAssessment.WorkProgramsSectionName] &&
        this.isSectionValid[InformalAssessment.EducationHistorySectionName] &&
        this.isSectionValid[InformalAssessment.PostSecondaryEducationSectionName] &&
        this.isSectionValid[InformalAssessment.MilitaryTrainingSectionName] &&
        this.isSectionValid[InformalAssessment.HousingSectionName] &&
        this.isSectionValid[InformalAssessment.TransportationSectionName] &&
        this.isSectionValid[InformalAssessment.LegalIssuesSectionName] &&
        this.isSectionValid[InformalAssessment.ChildYouthSupportsSectionName] &&
        this.isSectionValid[InformalAssessment.FamilyBarriersSectionName] &&
        this.isSectionValid[InformalAssessment.NonCustodialParentsSectionName] &&
        this.isSectionValid[InformalAssessment.NonCustodialParentsReferralSectionName]
      );
    }
  }
}
