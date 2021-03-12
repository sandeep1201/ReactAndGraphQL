// tslint:disable: no-use-before-declare
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { NotificationsService } from 'angular2-notifications';
import { AppService } from './../../core/services/app.service';
import { EditComponent as EditInformalAssessmentComponent } from '../../participant/informal-assessment/edit/edit.component';
import { BaseService } from './../../core/services/base.service';
import { ChildAndYouthSupportsSection } from '../models/child-youth-supports-section';
import { EducationHistorySection } from '../models/education-history-section';
import { FamilyBarriersSection } from '../models/family-barriers-section';
import { HousingSection } from '../models/housing-section';
import { LanguagesSection } from '../models/languages-section';
import { LegalIssuesSection } from '../models/legal-issues-section';
import { LogService } from './log.service';
import { MilitarySection } from '../models/military-section';
import { NonCustodialParentsSection } from '../models/non-custodial-parents-section';
import { NonCustodialParentsReferralSection } from '../models/non-custodial-parents-referral-section';
import { Participant } from '../models/participant';
import { PostSecondaryEducationSection } from '../models/post-secondary-education-section';
import { SectionComponent } from '../interfaces/section-component';
import { TransportationSection } from '../models/transportation-section';
import { WorkHistorySection } from '../models/work-history-section';
import { WorkProgramsSection } from '../models/work-programs-section';
import { ParticipantBarriersSection } from '../models/participant-barriers-section';
import { AuthHttpClient } from './../../core/interceptors/AuthHttpClient';
import { map, catchError, flatMap } from 'rxjs/operators';

declare var jQuery: any;

@Injectable()
export class InformalAssessmentEditService extends BaseService {
  private childCareSectionUrl: string;
  private educationSectionUrl: string;
  private familyBarrierSectionUrl: string;
  private housingSectionUrl: string;
  private languagesSectionUrl: string;
  private legalIssuesSectionUrl: string;
  private militarySectionUrl: string;
  private nonCustodialParentsSectionUrl: string;
  private nonCustodialParentsReferralSectionUrl: string;
  private participantBarriersSectionUrl: string;
  private postSecondarySectionUrl: string;
  private transportationSectionUrl: string;
  private workHistorySectionUrl: string;
  private workProgramsSectionUrl: string;
  private parentComponent: EditInformalAssessmentComponent;
  public sectionComponent: SectionComponent;

  childAndYouthSupportsModel: ChildAndYouthSupportsSection;
  educationModel: EducationHistorySection;
  familyBarriersModel: FamilyBarriersSection;
  housingModel: HousingSection;
  languagesModel: LanguagesSection;
  legalIssuesModel: LegalIssuesSection;
  militaryModel: MilitarySection;
  postSecondaryEducationModel: PostSecondaryEducationSection;
  transportationSectionModel: TransportationSection;
  workHistoryModel: WorkHistorySection;
  workProgramsModel: WorkProgramsSection;
  participantBarriersModel: ParticipantBarriersSection;
  nonCustodialParentsModel: NonCustodialParentsSection;
  nonCustodialParentsReferralModel: NonCustodialParentsReferralSection;

  // These are the last saved values.
  lastSavedChildAndYouthSupportsModel: ChildAndYouthSupportsSection;
  lastSavedEducationModel: EducationHistorySection;
  lastSavedFamilyBarriersModel: FamilyBarriersSection;
  lastSavedHousingModel: HousingSection;
  lastSavedLanguagesModel: LanguagesSection;
  lastSavedLegalIssuesModel: LegalIssuesSection;
  lastSavedMilitaryModel: MilitarySection;
  lastSavedPostSecondaryEducationModel: PostSecondaryEducationSection;
  lastSavedTransportationSectionModel: TransportationSection;
  lastSavedWorkHistoryModel: WorkHistorySection;
  lastSavedWorkProgramsModel: WorkProgramsSection;
  lastSavedParticipantBarriersModel: ParticipantBarriersSection;
  lastSavedNonCustodialParentsModel: NonCustodialParentsSection;
  lastSavedNonCustodialParentsReferralModel: NonCustodialParentsReferralSection;

  public participant: Participant;
  public hasChildCareValidated = false;
  public hasEducationValidated = false;
  public hasFamilyBarriersValidated = false;
  public hasHousingValidated = false;
  public hasLanguageValidated = false;
  public hasLegalIssuesValidated = false;
  public hasMilitaryValidated = false;
  public hasNonCustodialParentsValidated = false;
  public hasPostSecondaryValidated = false;
  public hasParticipantBarriersValidated = false;
  public hasTransportationValidated = false;
  public hasWorkHistoryValidated = false;
  public hasWorkProgramsValidated = false;
  public hasNonCustodialParentsReferralValidated = false;

  public disableHotKeys = false;
  modifiedTracker: ModifiedTracker;

  constructor(http: AuthHttpClient, private appService: AppService, logService: LogService, private notificationsService: NotificationsService) {
    super(http, logService);
    this.modifiedTracker = new ModifiedTracker();
    this.childCareSectionUrl = this.appService.apiServer + 'api/ia/child-youth-supports/';
    this.educationSectionUrl = this.appService.apiServer + 'api/ia/education/';
    this.familyBarrierSectionUrl = this.appService.apiServer + 'api/ia/family-barriers/';
    this.housingSectionUrl = this.appService.apiServer + 'api/ia/housing/';
    this.languagesSectionUrl = this.appService.apiServer + 'api/ia/language/';
    this.legalIssuesSectionUrl = this.appService.apiServer + 'api/ia/legalissues/';
    this.militarySectionUrl = this.appService.apiServer + 'api/ia/military/';
    this.nonCustodialParentsSectionUrl = this.appService.apiServer + 'api/ia/non-custodial-parents/';
    this.nonCustodialParentsReferralSectionUrl = this.appService.apiServer + 'api/ia/non-custodial-parents-referral/';
    this.participantBarriersSectionUrl = this.appService.apiServer + 'api/ia/participant-barriers/';
    this.postSecondarySectionUrl = this.appService.apiServer + 'api/ia/postsecondary/';
    this.transportationSectionUrl = this.appService.apiServer + 'api/ia/transportation/';
    this.workHistorySectionUrl = this.appService.apiServer + 'api/ia/workhistory/';
    this.workProgramsSectionUrl = this.appService.apiServer + 'api/ia/workprogram/';
  }

  setParentComponent(parent: EditInformalAssessmentComponent) {
    this.parentComponent = parent;
  }

  setSectionComponent(child: SectionComponent) {
    this.sectionComponent = child;
  }

  setModifiedModel(section: string, isModified: boolean) {
    if (section === 'languages') {
      this.modifiedTracker.isLanguageModified = isModified;
      this.modifiedTracker.isLanguageSaving = false;
      this.modifiedTracker.isLanguageSaved = false;
      this.modifiedTracker.isLanguageError = false;
    } else if (section === 'education') {
      this.modifiedTracker.isEducationModified = isModified;
      this.modifiedTracker.isEducationSaving = false;
      this.modifiedTracker.isEducationSaved = false;
      this.modifiedTracker.isEducationError = false;
    } else if (section === 'post-secondary') {
      this.modifiedTracker.isPostSecondaryModified = isModified;
      this.modifiedTracker.isPostSecondarySaving = false;
      this.modifiedTracker.isPostSecondarySaved = false;
      this.modifiedTracker.isPostSecondaryError = false;
    } else if (section === 'military') {
      this.modifiedTracker.isMilitaryModified = isModified;
      this.modifiedTracker.isMilitarySaving = false;
      this.modifiedTracker.isMilitarySaved = false;
      this.modifiedTracker.isMilitaryError = false;
    } else if (section === 'work-programs') {
      this.modifiedTracker.isWorkProgramsModified = isModified;
      this.modifiedTracker.isWorkProgramsSaving = false;
      this.modifiedTracker.isWorkProgramsSaved = false;
      this.modifiedTracker.isWorkProgramsError = false;
    } else if (section === 'child-youth-supports') {
      this.modifiedTracker.isChildCareModified = isModified;
      this.modifiedTracker.isChildCareSaving = false;
      this.modifiedTracker.isChildCareSaved = false;
      this.modifiedTracker.isChildCareError = false;
    } else if (section === 'work-history') {
      this.modifiedTracker.isWorkHistoryModified = isModified;
      this.modifiedTracker.isWorkHistorySaving = false;
      this.modifiedTracker.isWorkHistorySaved = false;
      this.modifiedTracker.isWorkHistoryError = false;
    } else if (section === 'housing') {
      this.modifiedTracker.isHousingModified = isModified;
      this.modifiedTracker.isHousingSaving = false;
      this.modifiedTracker.isHousingSaved = false;
      this.modifiedTracker.isHousingError = false;
    } else if (section === 'legal-issues') {
      this.modifiedTracker.isLegalIssuesModified = isModified;
      this.modifiedTracker.isLegalIssuesSaving = false;
      this.modifiedTracker.isLegalIssuesSaved = false;
      this.modifiedTracker.isLegalIssuesError = false;
    } else if (section === 'family-barriers') {
      this.modifiedTracker.isFamilyBarriersModified = isModified;
      this.modifiedTracker.isFamilyBarriersSaving = false;
      this.modifiedTracker.isFamilyBarriersSaved = false;
      this.modifiedTracker.isFamilyBarriersError = false;
    } else if (section === 'participant-barriers') {
      this.modifiedTracker.isParticipantBarriersModified = isModified;
      this.modifiedTracker.isParticipantBarriersSaving = false;
      this.modifiedTracker.isParticipantBarriersSaved = false;
      this.modifiedTracker.isParticipantBarriersError = false;
    } else if (section === 'non-custodial-parents') {
      this.modifiedTracker.isNonCustodialParentsModified = isModified;
      this.modifiedTracker.isNonCustodialParentsSaving = false;
      this.modifiedTracker.isNonCustodialParentsSaved = false;
      this.modifiedTracker.isNonCustodialParentsError = false;
    } else if (section === 'transportation') {
      this.modifiedTracker.isTransportationModified = isModified;
      this.modifiedTracker.isTransportationSaving = false;
      this.modifiedTracker.isTransportationSaved = false;
      this.modifiedTracker.isTransportationError = false;
    } else if (section === 'non-custodial-parents-referral') {
      this.modifiedTracker.isNonCustodialParentsReferralModified = isModified;
      this.modifiedTracker.isNonCustodialParentsReferralSaving = false;
      this.modifiedTracker.isNonCustodialParentsReferralSaved = false;
      this.modifiedTracker.isNonCustodialParentsReferralError = false;
    }

    this.updateModifiedTrackerDetectChange();
  }

  updateModifiedTrackerDetectChange() {
    if (this.modifiedTracker.isLanguageModified || this.modifiedTracker.isLanguageSaving) {
      this.modifiedTracker.isAnyChanged = true;
    } else if (this.modifiedTracker.isWorkHistoryModified || this.modifiedTracker.isWorkHistorySaving) {
      this.modifiedTracker.isAnyChanged = true;
    } else if (this.modifiedTracker.isEducationModified || this.modifiedTracker.isEducationSaving) {
      this.modifiedTracker.isAnyChanged = true;
    } else if (this.modifiedTracker.isPostSecondaryModified || this.modifiedTracker.isPostSecondarySaving) {
      this.modifiedTracker.isAnyChanged = true;
    } else if (this.modifiedTracker.isMilitaryModified || this.modifiedTracker.isMilitarySaving) {
      this.modifiedTracker.isAnyChanged = true;
    } else if (this.modifiedTracker.isWorkProgramsModified || this.modifiedTracker.isWorkProgramsSaving) {
      this.modifiedTracker.isAnyChanged = true;
    } else if (this.modifiedTracker.isChildCareModified || this.modifiedTracker.isChildCareSaving) {
      this.modifiedTracker.isAnyChanged = true;
    } else if (this.modifiedTracker.isHousingModified || this.modifiedTracker.isHousingSaving) {
      this.modifiedTracker.isAnyChanged = true;
    } else if (this.modifiedTracker.isLegalIssuesModified || this.modifiedTracker.isLegalIssuesSaving) {
      this.modifiedTracker.isAnyChanged = true;
    } else if (this.modifiedTracker.isFamilyBarriersModified || this.modifiedTracker.isFamilyBarriersSaving) {
      this.modifiedTracker.isAnyChanged = true;
    } else if (this.modifiedTracker.isParticipantBarriersModified || this.modifiedTracker.isParticipantBarriersSaving) {
      this.modifiedTracker.isAnyChanged = true;
    } else if (this.modifiedTracker.isTransportationModified || this.modifiedTracker.isTransportationSaving) {
      this.modifiedTracker.isAnyChanged = true;
    } else if (this.modifiedTracker.isNonCustodialParentsModified || this.modifiedTracker.isNonCustodialParentsSaving) {
      this.modifiedTracker.isAnyChanged = true;
    } else if (this.modifiedTracker.isNonCustodialParentsReferralModified || this.modifiedTracker.isNonCustodialParentsReferralSaving) {
      this.modifiedTracker.isAnyChanged = true;
    } else {
      this.modifiedTracker.isAnyChanged = false;
    }
    this.appService.isUrlChangeBlocked = this.modifiedTracker.isAnyChanged;
  }

  sectionIsNowValid(section: string) {
    this.parentComponent.sectionIsNowValid();

    if (section === 'languages') {
      this.hasLanguageValidated = false;
    } else if (section === 'education') {
      this.hasEducationValidated = false;
    } else if (section === 'post-secondary') {
      this.hasPostSecondaryValidated = false;
    } else if (section === 'military') {
      this.hasMilitaryValidated = false;
    } else if (section === 'work-programs') {
      this.hasWorkProgramsValidated = false;
    } else if (section === 'child-youth-supports') {
      this.hasChildCareValidated = false;
    } else if (section === 'housing') {
      this.hasHousingValidated = false;
    } else if (section === 'legal-issues') {
      this.hasLegalIssuesValidated = false;
    } else if (section === 'family-barriers') {
      this.hasFamilyBarriersValidated = false;
    } else if (section === 'non-custodial-parents') {
      this.hasNonCustodialParentsValidated = false;
    } else if (section === 'non-custodial-parents-referral') {
      this.hasNonCustodialParentsReferralValidated = false;
    } else if (section === 'participant-barriers') {
      this.hasParticipantBarriersValidated = false;
    } else if (section === 'transportation') {
      this.hasTransportationValidated = false;
    } else if (section === 'work-history') {
      this.hasWorkHistoryValidated = false;
    }
  }

  getSavableSectionModel(): any {
    return this.sectionComponent.prepareToSaveWithErrors();
  }

  refreshModel() {
    this.sectionComponent.refreshModel();
  }

  getLanguageSection(): Observable<LanguagesSection> {
    if (this.languagesModel != null) {
      return of(this.languagesModel);
    }

    return this.http.get(this.languagesSectionUrl + this.getPin()).pipe(
      map(this.extractLanguageSectionData),
      map(x => {
        this.lastSavedLanguagesModel = new LanguagesSection();
        LanguagesSection.clone(x, this.lastSavedLanguagesModel);
        return x;
      }),
      catchError(this.handleError)
    );
  }

  getEducationHistorySection(): Observable<EducationHistorySection> {
    if (this.educationModel != null) {
      return of(this.educationModel);
    }

    return this.http.get(this.educationSectionUrl + this.getPin()).pipe(
      map(this.extractEducationSectionData),
      map(x => {
        this.lastSavedEducationModel = new EducationHistorySection();
        EducationHistorySection.clone(x, this.lastSavedEducationModel);
        return x;
      }),
      catchError(this.handleError)
    );
  }

  getMilitarySection(): Observable<MilitarySection> {
    if (this.militaryModel != null) {
      return of(this.militaryModel);
    }

    return this.http.get(this.militarySectionUrl + this.getPin()).pipe(
      map(this.extractMilitarySectionData),
      map(x => {
        this.lastSavedMilitaryModel = new MilitarySection();
        MilitarySection.clone(x, this.lastSavedMilitaryModel);
        return x;
      }),
      catchError(this.handleError)
    );
  }

  getWorkProgramsSection(): Observable<WorkProgramsSection> {
    if (this.workProgramsModel != null) {
      return of(this.workProgramsModel);
    }

    return this.http.get(this.workProgramsSectionUrl + this.getPin()).pipe(
      map(this.extractWorkProgramsSectionData),
      map(x => {
        this.lastSavedWorkProgramsModel = new WorkProgramsSection();
        WorkProgramsSection.clone(x, this.lastSavedWorkProgramsModel);
        return x;
      }),
      catchError(this.handleError)
    );
  }

  getPostSecondaryEducationSection(): Observable<PostSecondaryEducationSection> {
    if (this.postSecondaryEducationModel != null) {
      return of(this.postSecondaryEducationModel);
    }

    return this.http.get(this.postSecondarySectionUrl + this.getPin()).pipe(
      map(this.extractPostSecondaryEducationSectionData),
      map(x => {
        this.lastSavedPostSecondaryEducationModel = new PostSecondaryEducationSection();
        PostSecondaryEducationSection.clone(x, this.lastSavedPostSecondaryEducationModel);
        return x;
      }),
      catchError(this.handleError)
    );
  }

  getChildCareSection(): Observable<ChildAndYouthSupportsSection> {
    if (this.childAndYouthSupportsModel != null) {
      return of(this.childAndYouthSupportsModel);
    }

    return this.http.get(this.childCareSectionUrl + this.getPin()).pipe(
      map(this.extractChildCareSectionData),
      map(x => {
        this.lastSavedChildAndYouthSupportsModel = new ChildAndYouthSupportsSection();
        ChildAndYouthSupportsSection.clone(x, this.lastSavedChildAndYouthSupportsModel);
        return x;
      }),
      catchError(this.handleError)
    );
  }

  getHousingSection(): Observable<HousingSection> {
    if (this.housingModel != null) {
      return of(this.housingModel);
    }

    return this.http.get(this.housingSectionUrl + this.getPin()).pipe(
      map(this.extractHousingSectionData),
      map(x => {
        this.lastSavedHousingModel = new HousingSection();
        HousingSection.clone(x, this.lastSavedHousingModel);
        return x;
      }),
      catchError(this.handleError)
    );
  }

  getLegalIssuesSection(): Observable<LegalIssuesSection> {
    if (this.legalIssuesModel != null) {
      return of(this.legalIssuesModel);
    }

    return this.http.get(this.legalIssuesSectionUrl + this.getPin()).pipe(
      map(this.extractLegalIssuesSectionData),
      map(x => {
        this.lastSavedLegalIssuesModel = new LegalIssuesSection();
        LegalIssuesSection.clone(x, this.lastSavedLegalIssuesModel);
        return x;
      }),
      catchError(this.handleError)
    );
  }

  getWorkHistorySection(): Observable<WorkHistorySection> {
    if (this.workHistoryModel != null) {
      return of(this.workHistoryModel);
    }

    return this.http.get(this.workHistorySectionUrl + this.getPin()).pipe(
      map(this.extractWorkHistorySectionData),
      map(x => {
        this.lastSavedWorkHistoryModel = new WorkHistorySection();
        WorkHistorySection.clone(x, this.lastSavedWorkHistoryModel);
        return x;
      }),
      catchError(this.handleError)
    );
  }

  getFamilyBarriersSection(): Observable<FamilyBarriersSection> {
    if (this.familyBarriersModel != null) {
      return of(this.familyBarriersModel);
    }

    return this.http.get(this.familyBarrierSectionUrl + this.getPin()).pipe(
      map(this.extractFamilyBarriersSectionData),
      map(x => {
        this.lastSavedFamilyBarriersModel = x.clone();
        return x;
      }),
      catchError(this.handleError)
    );
  }

  getParticipantBarriersSection(): Observable<ParticipantBarriersSection> {
    if (this.participantBarriersModel != null) {
      return of(this.participantBarriersModel);
    }

    return this.http.get(this.participantBarriersSectionUrl + this.getPin()).pipe(
      map(this.extractParticipantBarriersSectionData),
      map(x => {
        this.lastSavedParticipantBarriersModel = new ParticipantBarriersSection();
        ParticipantBarriersSection.clone(x, this.lastSavedParticipantBarriersModel);
        return x;
      }),
      catchError(this.handleError)
    );
  }

  getNonCustodialParentsSection(): Observable<NonCustodialParentsSection> {
    if (this.nonCustodialParentsModel != null) {
      return of(this.nonCustodialParentsModel);
    }

    return this.http.get(this.nonCustodialParentsSectionUrl + this.getPin()).pipe(
      map(this.extractNonCustodialParentsSectionData),
      map(x => {
        this.lastSavedNonCustodialParentsModel = new NonCustodialParentsSection();
        NonCustodialParentsSection.clone(x, this.lastSavedNonCustodialParentsModel);
        return x;
      }),
      catchError(this.handleError)
    );
  }

  getNonCustodialParentsReferralSection(): Observable<NonCustodialParentsReferralSection> {
    if (this.nonCustodialParentsReferralModel != null) {
      return of(this.nonCustodialParentsReferralModel);
    }

    return this.http.get(this.nonCustodialParentsReferralSectionUrl + this.getPin()).pipe(
      map(this.extractNonCustodialParentsReferralSectionData),
      map(x => {
        this.lastSavedNonCustodialParentsReferralModel = new NonCustodialParentsReferralSection();
        NonCustodialParentsReferralSection.clone(x, this.lastSavedNonCustodialParentsReferralModel);
        return x;
      }),
      catchError(this.handleError)
    );
  }

  getTransportationSection(): Observable<TransportationSection> {
    if (this.transportationSectionModel != null) {
      return of(this.transportationSectionModel);
    }

    return this.http.get(this.transportationSectionUrl + this.getPin()).pipe(
      map(this.extractTransportationSectionData),
      map(x => {
        this.lastSavedTransportationSectionModel = new TransportationSection();
        TransportationSection.clone(x, this.lastSavedTransportationSectionModel);
        return x;
      }),
      catchError(this.handleError)
    );
  }

  openSectionHelp() {
    this.sectionComponent.openHelp();
  }

  validateSection(section: string) {
    // console.log('validateSection');
    this.sectionComponent.validate();

    if (section === 'languages') {
      this.hasLanguageValidated = true;
    } else if (section === 'education') {
      this.hasEducationValidated = true;
    } else if (section === 'post-secondary') {
      this.hasPostSecondaryValidated = true;
    } else if (section === 'military') {
      this.hasMilitaryValidated = true;
    } else if (section === 'work-programs') {
      this.hasWorkProgramsValidated = true;
    } else if (section === 'child-youth-supports') {
      this.hasChildCareValidated = true;
    } else if (section === 'housing') {
      this.hasHousingValidated = true;
    } else if (section === 'legal-issues') {
      this.hasLegalIssuesValidated = true;
    } else if (section === 'non-custodial-parents') {
      this.hasNonCustodialParentsValidated = true;
    } else if (section === 'non-custodial-parents-referral') {
      this.hasNonCustodialParentsReferralValidated = true;
    } else if (section === 'participant-barriers') {
      this.hasParticipantBarriersValidated = true;
    } else if (section === 'transportation') {
      this.hasTransportationValidated = true;
    } else if (section === 'work-history') {
      this.hasWorkHistoryValidated = true;
    } else if (section === 'family-barriers') {
      this.hasFamilyBarriersValidated = true;
    }
  }

  isSectionValid(): boolean {
    return this.sectionComponent.isValid();
  }

  postLanguageSection(section: LanguagesSection, isValid: boolean): Observable<LanguagesSection> {
    // When we attempt a post, we need to save the local.
    // this.languagesModel = section;

    this.modifiedTracker.isLanguageModified = false;
    this.modifiedTracker.isLanguageSaving = true;
    this.modifiedTracker.isLanguageSaved = false;
    this.modifiedTracker.isLanguageError = false;

    return this.http.post(this.languagesSectionUrl + this.getPin(), section).pipe(
      map(this.extractLanguageSectionData),
      flatMap(x => {
        this.languagesModel = x as LanguagesSection;
        this.lastSavedLanguagesModel = new LanguagesSection();
        LanguagesSection.clone(x, this.lastSavedLanguagesModel);

        this.modifiedTracker.isLanguageModified = false;
        this.modifiedTracker.isLanguageSaving = false;
        this.modifiedTracker.isLanguageSaved = isValid;
        this.modifiedTracker.isLanguageError = !isValid;

        this.updateModifiedTrackerDetectChange();

        return of(x);
      }),
      catchError(e => {
        this.modifiedTracker.isLanguageModified = false;
        this.modifiedTracker.isLanguageSaving = false;
        this.modifiedTracker.isLanguageSaved = false;
        this.modifiedTracker.isLanguageError = true;

        this.updateModifiedTrackerDetectChange();

        return this.handleError(e);
      })
    );
  }

  postEducationSection(section: EducationHistorySection, isValid: boolean): Observable<EducationHistorySection> {
    this.modifiedTracker.isEducationModified = false;
    this.modifiedTracker.isEducationSaving = true;
    this.modifiedTracker.isEducationSaved = false;
    this.modifiedTracker.isEducationError = false;

    return this.http.post(this.educationSectionUrl + this.getPin(), section).pipe(
      map(this.extractEducationSectionData),
      flatMap(x => {
        this.educationModel = x as EducationHistorySection;
        this.lastSavedEducationModel = new EducationHistorySection();
        EducationHistorySection.clone(x, this.lastSavedEducationModel);

        this.modifiedTracker.isEducationModified = false;
        this.modifiedTracker.isEducationSaving = false;
        this.modifiedTracker.isEducationSaved = isValid;
        this.modifiedTracker.isEducationError = !isValid;

        this.updateModifiedTrackerDetectChange();

        return of(x);
      }),
      catchError(e => {
        this.modifiedTracker.isEducationModified = false;
        this.modifiedTracker.isEducationSaving = false;
        this.modifiedTracker.isEducationSaved = false;
        this.modifiedTracker.isEducationError = true;

        this.updateModifiedTrackerDetectChange();

        return this.handleError(e);
      })
    );
  }

  postPostSecondarySection(section: PostSecondaryEducationSection, isValid: boolean): Observable<PostSecondaryEducationSection> {
    this.modifiedTracker.isPostSecondaryModified = false;
    this.modifiedTracker.isPostSecondarySaving = true;
    this.modifiedTracker.isPostSecondarySaved = false;
    this.modifiedTracker.isPostSecondaryError = false;

    return this.http.post(this.postSecondarySectionUrl + this.getPin(), section).pipe(
      map(this.extractPostSecondaryEducationSectionData),
      flatMap(x => {
        this.postSecondaryEducationModel = x as PostSecondaryEducationSection;
        this.lastSavedPostSecondaryEducationModel = new PostSecondaryEducationSection();
        PostSecondaryEducationSection.clone(x, this.lastSavedPostSecondaryEducationModel);

        this.modifiedTracker.isPostSecondaryModified = false;
        this.modifiedTracker.isPostSecondarySaving = false;
        this.modifiedTracker.isPostSecondarySaved = isValid;
        this.modifiedTracker.isPostSecondaryError = !isValid;

        this.updateModifiedTrackerDetectChange();

        return of(x);
      }),
      catchError(e => {
        this.modifiedTracker.isPostSecondaryModified = false;
        this.modifiedTracker.isPostSecondarySaving = false;
        this.modifiedTracker.isPostSecondarySaved = false;
        this.modifiedTracker.isPostSecondaryError = true;

        this.updateModifiedTrackerDetectChange();

        return this.handleError(e);
      })
    );
  }

  postMilitarySection(section: MilitarySection, isValid: boolean): Observable<MilitarySection> {
    this.modifiedTracker.isMilitaryModified = false;
    this.modifiedTracker.isMilitarySaving = true;
    this.modifiedTracker.isMilitarySaved = false;
    this.modifiedTracker.isMilitaryError = false;

    return this.http.post(this.militarySectionUrl + this.getPin(), section).pipe(
      map(this.extractMilitarySectionData),
      flatMap(x => {
        this.militaryModel = x as MilitarySection;
        this.lastSavedMilitaryModel = new MilitarySection();
        MilitarySection.clone(x, this.lastSavedMilitaryModel);

        this.modifiedTracker.isMilitaryModified = false;
        this.modifiedTracker.isMilitarySaving = false;
        this.modifiedTracker.isMilitarySaved = isValid;
        this.modifiedTracker.isMilitaryError = !isValid;

        this.updateModifiedTrackerDetectChange();

        return of(x);
      }),
      catchError(e => {
        this.modifiedTracker.isMilitaryModified = false;
        this.modifiedTracker.isMilitarySaving = false;
        this.modifiedTracker.isMilitarySaved = false;
        this.modifiedTracker.isMilitaryError = true;

        this.updateModifiedTrackerDetectChange();

        return this.handleError(e);
      })
    );
  }

  postWorkProgramsSection(section: WorkProgramsSection, isValid: boolean): Observable<WorkProgramsSection> {
    this.modifiedTracker.isWorkProgramsModified = false;
    this.modifiedTracker.isWorkProgramsSaving = true;
    this.modifiedTracker.isWorkProgramsSaved = false;
    this.modifiedTracker.isWorkProgramsError = false;

    return this.http.post(this.workProgramsSectionUrl + this.getPin(), section).pipe(
      map(this.extractWorkProgramsSectionData),
      flatMap(x => {
        this.workProgramsModel = x as WorkProgramsSection;
        this.lastSavedWorkProgramsModel = new WorkProgramsSection();
        WorkProgramsSection.clone(x, this.lastSavedWorkProgramsModel);

        this.modifiedTracker.isWorkProgramsModified = false;
        this.modifiedTracker.isWorkProgramsSaving = false;
        this.modifiedTracker.isWorkProgramsSaved = isValid;
        this.modifiedTracker.isWorkProgramsError = !isValid;

        this.updateModifiedTrackerDetectChange();

        return of(x);
      }),
      catchError(e => {
        this.modifiedTracker.isWorkProgramsModified = false;
        this.modifiedTracker.isWorkProgramsSaving = false;
        this.modifiedTracker.isWorkProgramsSaved = false;
        this.modifiedTracker.isWorkProgramsError = true;

        this.updateModifiedTrackerDetectChange();

        return this.handleError(e);
      })
    );
  }

  postChildCareSection(section: ChildAndYouthSupportsSection, isValid: boolean): Observable<ChildAndYouthSupportsSection> {
    this.modifiedTracker.isChildCareModified = false;
    this.modifiedTracker.isChildCareSaving = true;
    this.modifiedTracker.isChildCareSaved = false;
    this.modifiedTracker.isChildCareError = false;

    return this.http.post(this.childCareSectionUrl + this.getPin(), section).pipe(
      map(this.extractChildCareSectionData),
      flatMap(x => {
        this.childAndYouthSupportsModel = x as ChildAndYouthSupportsSection;
        this.lastSavedChildAndYouthSupportsModel = new ChildAndYouthSupportsSection();
        ChildAndYouthSupportsSection.clone(x, this.lastSavedChildAndYouthSupportsModel);

        this.modifiedTracker.isChildCareModified = false;
        this.modifiedTracker.isChildCareSaving = false;
        this.modifiedTracker.isChildCareSaved = isValid;
        this.modifiedTracker.isChildCareError = !isValid;

        this.updateModifiedTrackerDetectChange();

        return of(x);
      }),
      catchError(e => {
        this.modifiedTracker.isChildCareModified = false;
        this.modifiedTracker.isChildCareSaving = false;
        this.modifiedTracker.isChildCareSaved = false;
        this.modifiedTracker.isChildCareError = true;

        this.updateModifiedTrackerDetectChange();

        return this.handleError(e);
      })
    );
  }

  postHousingSection(section: HousingSection, isValid: boolean): Observable<HousingSection> {
    this.modifiedTracker.isHousingModified = false;
    this.modifiedTracker.isHousingSaving = true;
    this.modifiedTracker.isHousingSaved = false;
    this.modifiedTracker.isHousingError = false;

    return this.http.post(this.housingSectionUrl + this.getPin(), section).pipe(
      map(this.extractHousingSectionData),
      flatMap(x => {
        this.housingModel = x as HousingSection;
        this.lastSavedHousingModel = new HousingSection();
        HousingSection.clone(x, this.lastSavedHousingModel);

        this.modifiedTracker.isHousingModified = false;
        this.modifiedTracker.isHousingSaving = false;
        this.modifiedTracker.isHousingSaved = isValid;
        this.modifiedTracker.isHousingError = !isValid;

        this.updateModifiedTrackerDetectChange();

        return of(x);
      }),
      catchError(e => {
        this.modifiedTracker.isHousingModified = false;
        this.modifiedTracker.isHousingSaving = false;
        this.modifiedTracker.isHousingSaved = false;
        this.modifiedTracker.isHousingError = true;

        this.updateModifiedTrackerDetectChange();

        return this.handleError(e);
      })
    );
  }

  postLegalIssuesSection(section: LegalIssuesSection, isValid: boolean): Observable<LegalIssuesSection> {
    this.modifiedTracker.isLegalIssuesModified = false;
    this.modifiedTracker.isLegalIssuesSaving = true;
    this.modifiedTracker.isLegalIssuesSaved = false;
    this.modifiedTracker.isLegalIssuesError = false;

    return this.http.post(this.legalIssuesSectionUrl + this.getPin(), section).pipe(
      map(this.extractLegalIssuesSectionData),
      flatMap(x => {
        this.legalIssuesModel = x as LegalIssuesSection;
        this.lastSavedLegalIssuesModel = new LegalIssuesSection();
        LegalIssuesSection.clone(x, this.lastSavedLegalIssuesModel);

        this.modifiedTracker.isLegalIssuesModified = false;
        this.modifiedTracker.isLegalIssuesSaving = false;
        this.modifiedTracker.isLegalIssuesSaved = isValid;
        this.modifiedTracker.isLegalIssuesError = !isValid;

        this.updateModifiedTrackerDetectChange();

        return of(x);
      }),
      catchError(e => {
        this.modifiedTracker.isLegalIssuesModified = false;
        this.modifiedTracker.isLegalIssuesSaving = false;
        this.modifiedTracker.isLegalIssuesSaved = false;
        this.modifiedTracker.isLegalIssuesError = true;

        this.updateModifiedTrackerDetectChange();

        return this.handleError(e);
      })
    );
  }

  postWorkHistorySection(section: WorkHistorySection, isValid: boolean): Observable<WorkHistorySection> {
    this.modifiedTracker.isWorkHistoryModified = false;
    this.modifiedTracker.isWorkHistorySaving = true;
    this.modifiedTracker.isWorkHistorySaved = false;
    this.modifiedTracker.isWorkHistoryError = false;

    return this.http.post(this.workHistorySectionUrl + this.getPin(), section).pipe(
      map(this.extractWorkHistorySectionData),
      flatMap(x => {
        this.workHistoryModel = x as WorkHistorySection;
        this.lastSavedWorkHistoryModel = new WorkHistorySection();
        WorkHistorySection.clone(x, this.lastSavedWorkHistoryModel);

        this.modifiedTracker.isWorkHistoryModified = false;
        this.modifiedTracker.isWorkHistorySaving = false;
        this.modifiedTracker.isWorkHistorySaved = isValid;
        this.modifiedTracker.isWorkHistoryError = !isValid;

        this.updateModifiedTrackerDetectChange();

        return of(x);
      }),
      catchError(e => {
        this.modifiedTracker.isWorkHistoryModified = false;
        this.modifiedTracker.isWorkHistorySaving = false;
        this.modifiedTracker.isWorkHistorySaved = false;
        this.modifiedTracker.isWorkHistoryError = true;

        this.updateModifiedTrackerDetectChange();

        return this.handleError(e);
      })
    );
  }

  postFamilyBarriersSection(section: FamilyBarriersSection, isValid: boolean): Observable<FamilyBarriersSection> {
    this.modifiedTracker.isFamilyBarriersModified = false;
    this.modifiedTracker.isFamilyBarriersSaving = true;
    this.modifiedTracker.isFamilyBarriersSaved = false;
    this.modifiedTracker.isFamilyBarriersError = false;

    return this.http.post(this.familyBarrierSectionUrl + this.getPin(), section).pipe(
      map(this.extractFamilyBarriersSectionData),
      flatMap(x => {
        this.familyBarriersModel = x as FamilyBarriersSection;
        this.lastSavedFamilyBarriersModel = x.clone();

        this.modifiedTracker.isFamilyBarriersModified = false;
        this.modifiedTracker.isFamilyBarriersSaving = false;
        this.modifiedTracker.isFamilyBarriersSaved = isValid;
        this.modifiedTracker.isFamilyBarriersError = !isValid;

        this.updateModifiedTrackerDetectChange();

        return of(x);
      }),
      catchError(e => {
        this.modifiedTracker.isFamilyBarriersModified = false;
        this.modifiedTracker.isFamilyBarriersSaving = false;
        this.modifiedTracker.isFamilyBarriersSaved = false;
        this.modifiedTracker.isFamilyBarriersError = true;

        this.updateModifiedTrackerDetectChange();

        return this.handleError(e);
      })
    );
  }

  postParticipantBarriersSection(section: ParticipantBarriersSection, isValid: boolean): Observable<ParticipantBarriersSection> {
    this.modifiedTracker.isParticipantBarriersModified = false;
    this.modifiedTracker.isParticipantBarriersSaving = true;
    this.modifiedTracker.isParticipantBarriersSaved = false;
    this.modifiedTracker.isParticipantBarriersError = false;

    return this.http.post(this.participantBarriersSectionUrl + this.getPin(), section).pipe(
      map(this.extractParticipantBarriersSectionData),
      flatMap(x => {
        this.participantBarriersModel = x as ParticipantBarriersSection;
        this.lastSavedParticipantBarriersModel = new ParticipantBarriersSection();
        ParticipantBarriersSection.clone(x, this.lastSavedParticipantBarriersModel);

        this.modifiedTracker.isParticipantBarriersModified = false;
        this.modifiedTracker.isParticipantBarriersSaving = false;
        this.modifiedTracker.isParticipantBarriersSaved = isValid;
        this.modifiedTracker.isParticipantBarriersError = !isValid;

        this.updateModifiedTrackerDetectChange();

        return of(x);
      }),
      catchError(e => {
        this.modifiedTracker.isParticipantBarriersModified = false;
        this.modifiedTracker.isParticipantBarriersSaving = false;
        this.modifiedTracker.isParticipantBarriersSaved = false;
        this.modifiedTracker.isParticipantBarriersError = true;

        this.updateModifiedTrackerDetectChange();

        return this.handleError(e);
      })
    );
  }

  postNonCustodialParentsSection(section: NonCustodialParentsSection, isValid: boolean): Observable<NonCustodialParentsSection> {
    this.modifiedTracker.isNonCustodialParentsModified = false;
    this.modifiedTracker.isNonCustodialParentsSaving = true;
    this.modifiedTracker.isNonCustodialParentsSaved = false;
    this.modifiedTracker.isNonCustodialParentsError = false;

    return this.http.post(this.nonCustodialParentsSectionUrl + this.getPin(), section).pipe(
      map(this.extractNonCustodialParentsSectionData),
      flatMap(x => {
        this.nonCustodialParentsModel = x as NonCustodialParentsSection;
        this.lastSavedNonCustodialParentsModel = new NonCustodialParentsSection();
        NonCustodialParentsSection.clone(x, this.lastSavedNonCustodialParentsModel);

        this.modifiedTracker.isNonCustodialParentsModified = false;
        this.modifiedTracker.isNonCustodialParentsSaving = false;
        this.modifiedTracker.isNonCustodialParentsSaved = isValid;
        this.modifiedTracker.isNonCustodialParentsError = !isValid;

        this.updateModifiedTrackerDetectChange();

        return of(x);
      }),
      catchError(e => {
        this.modifiedTracker.isNonCustodialParentsModified = false;
        this.modifiedTracker.isNonCustodialParentsSaving = false;
        this.modifiedTracker.isNonCustodialParentsSaved = false;
        this.modifiedTracker.isNonCustodialParentsError = true;

        this.updateModifiedTrackerDetectChange();

        return this.handleError(e);
      })
    );
  }

  postNonCustodialParentsReferralSection(section: NonCustodialParentsReferralSection, isValid: boolean): Observable<NonCustodialParentsReferralSection> {
    this.modifiedTracker.isNonCustodialParentsReferralModified = false;
    this.modifiedTracker.isNonCustodialParentsReferralSaving = true;
    this.modifiedTracker.isNonCustodialParentsReferralSaved = false;
    this.modifiedTracker.isNonCustodialParentsReferralError = false;

    return this.http.post(this.nonCustodialParentsReferralSectionUrl + this.getPin(), section).pipe(
      map(this.extractNonCustodialParentsReferralSectionData),
      flatMap(x => {
        this.nonCustodialParentsReferralModel = x as NonCustodialParentsReferralSection;
        this.lastSavedNonCustodialParentsReferralModel = new NonCustodialParentsReferralSection();
        NonCustodialParentsReferralSection.clone(x, this.lastSavedNonCustodialParentsReferralModel);

        this.modifiedTracker.isNonCustodialParentsReferralModified = false;
        this.modifiedTracker.isNonCustodialParentsReferralSaving = false;
        this.modifiedTracker.isNonCustodialParentsReferralSaved = isValid;
        this.modifiedTracker.isNonCustodialParentsReferralError = !isValid;

        this.updateModifiedTrackerDetectChange();

        return of(x);
      }),
      catchError(e => {
        this.modifiedTracker.isNonCustodialParentsReferralModified = false;
        this.modifiedTracker.isNonCustodialParentsReferralSaving = false;
        this.modifiedTracker.isNonCustodialParentsReferralSaved = false;
        this.modifiedTracker.isNonCustodialParentsReferralError = true;

        this.updateModifiedTrackerDetectChange();

        return this.handleError(e);
      })
    );
  }

  postTransportationSection(section: TransportationSection, isValid: boolean): Observable<TransportationSection> {
    this.modifiedTracker.isTransportationModified = false;
    this.modifiedTracker.isTransportationSaving = true;
    this.modifiedTracker.isTransportationSaved = false;
    this.modifiedTracker.isTransportationError = false;

    return this.http.post(this.transportationSectionUrl + this.getPin(), section).pipe(
      map(this.extractTransportationSectionData),
      flatMap(x => {
        this.transportationSectionModel = x as TransportationSection;
        this.lastSavedTransportationSectionModel = new TransportationSection();
        TransportationSection.clone(x, this.lastSavedTransportationSectionModel);

        this.modifiedTracker.isTransportationModified = false;
        this.modifiedTracker.isTransportationSaving = false;
        this.modifiedTracker.isTransportationSaved = isValid;
        this.modifiedTracker.isTransportationError = !isValid;

        this.updateModifiedTrackerDetectChange();

        return of(x);
      }),
      catchError(e => {
        this.modifiedTracker.isTransportationModified = false;
        this.modifiedTracker.isTransportationSaving = false;
        this.modifiedTracker.isTransportationSaved = false;
        this.modifiedTracker.isTransportationError = true;

        this.updateModifiedTrackerDetectChange();

        return this.handleError(e);
      })
    );
  }

  private extractLanguageSectionData(res: LanguagesSection) {
    const body = res as LanguagesSection;
    const ls = new LanguagesSection().deserialize(body);
    return ls || null;
  }

  private extractEducationSectionData(res: EducationHistorySection): EducationHistorySection {
    const body = res as EducationHistorySection;
    const eds = new EducationHistorySection().deserialize(body);
    return eds || null;
  }

  private extractMilitarySectionData(res: MilitarySection): MilitarySection {
    const body = res as MilitarySection;
    const ms = new MilitarySection().deserialize(body);
    return ms || null;
  }

  private extractWorkProgramsSectionData(res: WorkProgramsSection): WorkProgramsSection {
    const body = res as WorkProgramsSection;
    const wps = new WorkProgramsSection().deserialize(body);
    return wps || null;
  }

  private extractPostSecondaryEducationSectionData(res: PostSecondaryEducationSection): PostSecondaryEducationSection {
    const body = res as PostSecondaryEducationSection;
    const pses = new PostSecondaryEducationSection().deserialize(body);
    return pses || null;
  }

  private extractChildCareSectionData(res: ChildAndYouthSupportsSection) {
    const body = res as ChildAndYouthSupportsSection;
    const cyss = new ChildAndYouthSupportsSection().deserialize(body);
    return cyss || null;
  }

  private extractHousingSectionData(res: HousingSection): HousingSection {
    const body = res as HousingSection;
    const hs = new HousingSection().deserialize(body);
    return hs || null;
  }

  private extractLegalIssuesSectionData(res: LegalIssuesSection) {
    const body = res as LegalIssuesSection;
    const ls = new LegalIssuesSection().deserialize(body);
    return ls || null;
  }

  private extractWorkHistorySectionData(res: WorkHistorySection): WorkHistorySection {
    const body = res as WorkHistorySection;
    const wh = new WorkHistorySection().deserialize(body);
    return wh || null;
  }

  private extractFamilyBarriersSectionData(res: FamilyBarriersSection) {
    const body = res as FamilyBarriersSection;
    const sec = new FamilyBarriersSection().deserialize(body);
    return sec || null;
  }

  private extractParticipantBarriersSectionData(res: ParticipantBarriersSection) {
    const body = res as ParticipantBarriersSection;
    const pbs = new ParticipantBarriersSection().deserialize(body);
    return pbs || null;
  }

  private extractNonCustodialParentsSectionData(res: any) {
    const body = res as any;
    const ncps = new NonCustodialParentsSection().deserialize(body);
    return ncps || null;
  }

  private extractNonCustodialParentsReferralSectionData(res: any) {
    const body = res as any;
    const ncprs = new NonCustodialParentsReferralSection().deserialize(body);
    return ncprs || null;
  }

  private extractTransportationSectionData(res: any) {
    const body = res as any;
    const ts = new TransportationSection().deserialize(body);
    return ts || null;
  }
}

class ModifiedTracker {
  isLanguageModified = false;
  isLanguageSaving = false;
  isLanguageSaved = false;
  isLanguageError = false;

  isEducationModified = false;
  isEducationSaving = false;
  isEducationSaved = false;
  isEducationError = false;

  isPostSecondaryModified = false;
  isPostSecondarySaving = false;
  isPostSecondarySaved = false;
  isPostSecondaryError = false;

  isMilitaryModified = false;
  isMilitarySaving = false;
  isMilitarySaved = false;
  isMilitaryError = false;

  isWorkProgramsModified = false;
  isWorkProgramsSaving = false;
  isWorkProgramsSaved = false;
  isWorkProgramsError = false;

  isChildCareModified = false;
  isChildCareSaving = false;
  isChildCareSaved = false;
  isChildCareError = false;

  isHousingModified = false;
  isHousingSaving = false;
  isHousingSaved = false;
  isHousingError = false;

  isLegalIssuesModified = false;
  isLegalIssuesSaving = false;
  isLegalIssuesSaved = false;
  isLegalIssuesError = false;

  isWorkHistoryModified = false;
  isWorkHistorySaving = false;
  isWorkHistorySaved = false;
  isWorkHistoryError = false;

  isFamilyBarriersModified = false;
  isFamilyBarriersSaving = false;
  isFamilyBarriersSaved = false;
  isFamilyBarriersError = false;

  isParticipantBarriersModified = false;
  isParticipantBarriersSaving = false;
  isParticipantBarriersSaved = false;
  isParticipantBarriersError = false;

  isTransportationModified = false;
  isTransportationSaving = false;
  isTransportationSaved = false;
  isTransportationError = false;

  isNonCustodialParentsModified = false;
  isNonCustodialParentsSaving = false;
  isNonCustodialParentsSaved = false;
  isNonCustodialParentsError = false;

  isNonCustodialParentsReferralModified = false;
  isNonCustodialParentsReferralSaving = false;
  isNonCustodialParentsReferralSaved = false;
  isNonCustodialParentsReferralError = false;
  isAnyChanged = false;
}
