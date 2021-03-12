import { SubHeaderComponent } from './../../../shared/components/sub-header/sub-header.component';
import { AppService } from './../../../core/services/app.service';
// tslint:disable: import-blacklist
// tslint:disable: deprecation
import { Component, OnInit, OnDestroy, HostListener, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription, Observable } from 'rxjs';
import { BooleanDictionary } from '../../../shared/interfaces/boolean-dictionary';
import { InformalAssessment } from '../../../shared/models/informal-assessment';
import { InformalAssessmentEditService } from '../../../shared/services/informal-assessment-edit.service';
import { InformalAssessmentService } from '../../../shared/services/informal-assessment.service';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { LanguagesSection } from '../../../shared/models/languages-section';
import { EducationHistorySection } from '../../../shared/models/education-history-section';
import { PostSecondaryEducationSection } from '../../../shared/models/post-secondary-education-section';
import { MilitarySection } from '../../../shared/models/military-section';
import { WorkProgramsSection } from '../../../shared/models/work-programs-section';
import { ChildAndYouthSupportsSection } from '../../../shared/models/child-youth-supports-section';
import { HousingSection } from '../../../shared/models/housing-section';
import { LegalIssuesSection } from '../../../shared/models/legal-issues-section';
import { WorkHistorySection } from '../../../shared/models/work-history-section';
import { FamilyBarriersSection } from '../../../shared/models/family-barriers-section';
import { ParticipantBarriersSection } from '../../../shared/models/participant-barriers-section';
import { NonCustodialParentsSection } from '../../../shared/models/non-custodial-parents-section';
import { NonCustodialParentsReferralSection } from '../../../shared/models/non-custodial-parents-referral-section';
import { TransportationSection } from '../../../shared/models/transportation-section';
import { Participant } from '../../../shared/models/participant';
import { ParticipantService } from '../../../shared/services/participant.service';
import { map, catchError } from 'rxjs/operators';

declare var $: any;

@Component({
  selector: 'app-edit-informal-assessment',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css'],
  providers: [FieldDataService, InformalAssessmentEditService, InformalAssessmentService]
})
export class EditComponent implements OnInit, OnDestroy {
  @ViewChild('subHeader', { read: SubHeaderComponent, static: false }) subHeader: SubHeaderComponent;

  iaSectionSub: Subscription;

  // Be sure to keep this in sync with default route for Edit Informal Assessment!!
  activeSection = 'languages';
  goBackUrl: string;
  isAssessmentLoaded = false;
  isSectionLoaded = false;
  isSectionNeedingValidation = true;
  isSidebarCollapsed = false;
  partSub: Subscription;
  participant: Participant;
  pin: string;
  pinSub: Subscription;
  stickyElements: any;
  public isSaving = false;
  public assessment: InformalAssessment;
  public isSectionValid: BooleanDictionary = {};
  private isInitialRouteSpecified = false;
  public hasPBAccessBol: boolean;
  public canRequestPBAccess: boolean;
  public pbSub: Subscription;
  public hasFcdpRole = false;
  public languageSectionSaved = false;
  public childYouthSectionSaved = false;
  public workProgramsSectionSaved = false;
  public familyBarriersSectionSaved = false;

  constructor(
    private appService: AppService,
    private route: ActivatedRoute,
    private router: Router,
    private partService: ParticipantService,
    public eiaService: InformalAssessmentEditService,
    private iaService: InformalAssessmentService
  ) {
    // jQuery for sidebar
    $(window).on('scroll', function() {
      const o = $('#sidebar');
      if (1.2 * $('#sidebar').outerHeight() < $(window).height()) {
        const e = $(window).scrollTop(),
          s = e - $('app-header').outerHeight() - $('app-sub-header').outerHeight() + 40;
        s > 0 ? o.css('top', s + 'px') : o.css('top', '0');
      } else o.css('top', '0');
    });
  }

  // To handle keypresses, look at this example:
  // https://plnkr.co/edit/Aubybjbkp7p8FPxqM0zx?p=preview

  @HostListener('window:keydown', ['$event'])
  keyboardInput(event: KeyboardEvent) {
    // We disable hotkeys when we have modals.
    if (this.eiaService.disableHotKeys === true) {
      return;
    }

    // Ignore any use of Alt key and Ctrl key
    if (event.altKey || event.ctrlKey) {
      return;
    }

    switch (event.keyCode) {
      case 119: // F8 : Exit
        event.preventDefault();
        event.stopPropagation();
        this.subHeader.goBack();
        break;
      case 120: // F9 : Save (& Continue)
        event.preventDefault();
        event.stopPropagation();
        if (event.shiftKey) {
          this.saveAndContinueCurrentSection();
        } else {
          this.handleSaveOnly();
        }
        break;
      case 33: // PageUp : Previous
        event.preventDefault();
        event.stopPropagation();
        this.goToPreviousSection();
        break;
      case 34: // PageDown : Next
        event.preventDefault();
        event.stopPropagation();
        this.goToNextSection();
        break;
    }
  }

  ngOnInit() {
    const segments = this.router.url.split('/');
    if (segments.length > 5) {
      this.isInitialRouteSpecified = true;
      this.updateActiveSectionForSidebar(segments[segments.length - 1]);
    }
    this.pinSub = this.route.params.subscribe(params => {
      this.pin = params['pin'];
      this.goBackUrl = `/pin/${this.pin}/assessment`;
      this.eiaService.setPin(this.pin);
      this.eiaService.setParentComponent(this);
      this.partSub = this.partService.getParticipant(this.pin).subscribe(part => {
        this.participant = part;
        this.eiaService.participant = part;
        this.onParticipantInit();
      });
    });
    this.pbSub = this.appService.PBSection.subscribe(res => {
      this.hasPBAccessBol = res.hasPBAccessBol;
      this.canRequestPBAccess = res.canRequestPBAccess;
    });
  }

  onParticipantInit() {
    this.iaService.getInformalAssessment(this.pin).subscribe(ia => {
      this.assessment = ia;
      // initializing the PBSection Behavior subject here
      this.iaService.initPbvars(this.pin);

      if (this.assessment != null) {
        this.iaService.loadValidationContextsAndValidate(this.participant, this.assessment).subscribe(
          n => n,
          e => e,
          () => {
            // The next and error events above are not needed.  We only care about
            // knowing when the validation is complete so we can update our class
            // properties to those from the service.
            this.isSectionValid = this.iaService.isSectionValid;

            this.eiaService.modifiedTracker.isLanguageSaved =
              !this.assessment.submitDate && this.assessment.languagesSection.isSubmittedViaDriverFlow && this.isSectionValid[InformalAssessment.LanguagesSectionName];
            if (!this.eiaService.modifiedTracker.isLanguageSaved) {
              this.eiaService.modifiedTracker.isLanguageError =
                this.assessment.languagesSection.isSubmittedViaDriverFlow && !this.isSectionValid[InformalAssessment.LanguagesSectionName];
            }

            this.eiaService.modifiedTracker.isWorkHistorySaved =
              !this.assessment.submitDate && this.assessment.workHistorySection.isSubmittedViaDriverFlow && this.isSectionValid[InformalAssessment.WorkHistorySectionName];
            if (!this.eiaService.modifiedTracker.isWorkHistorySaved) {
              this.eiaService.modifiedTracker.isWorkHistoryError =
                this.assessment.workHistorySection.isSubmittedViaDriverFlow && !this.isSectionValid[InformalAssessment.WorkHistorySectionName];
            }

            this.eiaService.modifiedTracker.isWorkProgramsSaved =
              !this.assessment.submitDate && this.assessment.workProgramSection.isSubmittedViaDriverFlow && this.isSectionValid[InformalAssessment.WorkProgramsSectionName];
            if (!this.eiaService.modifiedTracker.isWorkProgramsSaved) {
              this.eiaService.modifiedTracker.isWorkProgramsError =
                this.assessment.workProgramSection.isSubmittedViaDriverFlow && !this.isSectionValid[InformalAssessment.WorkProgramsSectionName];
            }

            this.eiaService.modifiedTracker.isEducationSaved =
              !this.assessment.submitDate && this.assessment.educationSection.isSubmittedViaDriverFlow && this.isSectionValid[InformalAssessment.EducationHistorySectionName];
            if (!this.eiaService.modifiedTracker.isEducationSaved) {
              this.eiaService.modifiedTracker.isEducationError =
                this.assessment.educationSection.isSubmittedViaDriverFlow && !this.isSectionValid[InformalAssessment.EducationHistorySectionName];
            }

            this.eiaService.modifiedTracker.isPostSecondarySaved =
              !this.assessment.submitDate &&
              this.assessment.postSecondarySection.isSubmittedViaDriverFlow &&
              this.isSectionValid[InformalAssessment.PostSecondaryEducationSectionName];
            if (!this.eiaService.modifiedTracker.isPostSecondarySaved) {
              this.eiaService.modifiedTracker.isPostSecondaryError =
                this.assessment.postSecondarySection.isSubmittedViaDriverFlow && !this.isSectionValid[InformalAssessment.PostSecondaryEducationSectionName];
            }

            this.eiaService.modifiedTracker.isMilitarySaved =
              !this.assessment.submitDate && this.assessment.militarySection.isSubmittedViaDriverFlow && this.isSectionValid[InformalAssessment.MilitaryTrainingSectionName];
            if (!this.eiaService.modifiedTracker.isMilitarySaved) {
              this.eiaService.modifiedTracker.isMilitaryError =
                this.assessment.militarySection.isSubmittedViaDriverFlow && !this.isSectionValid[InformalAssessment.MilitaryTrainingSectionName];
            }

            this.eiaService.modifiedTracker.isHousingSaved =
              !this.assessment.submitDate && this.assessment.housingSection.isSubmittedViaDriverFlow && this.isSectionValid[InformalAssessment.HousingSectionName];
            if (!this.eiaService.modifiedTracker.isHousingSaved) {
              this.eiaService.modifiedTracker.isHousingError =
                this.assessment.housingSection.isSubmittedViaDriverFlow && !this.isSectionValid[InformalAssessment.HousingSectionName];
            }

            this.eiaService.modifiedTracker.isTransportationSaved =
              !this.assessment.submitDate && this.assessment.transportationSection.isSubmittedViaDriverFlow && this.isSectionValid[InformalAssessment.TransportationSectionName];
            if (!this.eiaService.modifiedTracker.isTransportationSaved) {
              this.eiaService.modifiedTracker.isTransportationError =
                this.assessment.transportationSection.isSubmittedViaDriverFlow && !this.isSectionValid[InformalAssessment.TransportationSectionName];
            }

            this.eiaService.modifiedTracker.isLegalIssuesSaved =
              !this.assessment.submitDate && this.assessment.legalIssuesSection.isSubmittedViaDriverFlow && this.isSectionValid[InformalAssessment.LegalIssuesSectionName];
            if (!this.eiaService.modifiedTracker.isLegalIssuesSaved) {
              this.eiaService.modifiedTracker.isLegalIssuesError =
                this.assessment.legalIssuesSection.isSubmittedViaDriverFlow && !this.isSectionValid[InformalAssessment.LegalIssuesSectionName];
            }

            this.eiaService.modifiedTracker.isParticipantBarriersSaved =
              !this.assessment.submitDate &&
              this.assessment.participantBarriersSection.isSubmittedViaDriverFlow &&
              this.isSectionValid[InformalAssessment.ParticipantBarriersSectionName];
            if (!this.eiaService.modifiedTracker.isParticipantBarriersSaved) {
              this.eiaService.modifiedTracker.isParticipantBarriersError =
                this.assessment.participantBarriersSection.isSubmittedViaDriverFlow && !this.isSectionValid[InformalAssessment.ParticipantBarriersSectionName];
            }

            this.eiaService.modifiedTracker.isChildCareSaved =
              !this.assessment.submitDate &&
              this.assessment.childYouthSupportsSection.isSubmittedViaDriverFlow &&
              this.isSectionValid[InformalAssessment.ChildYouthSupportsSectionName];
            if (!this.eiaService.modifiedTracker.isChildCareSaved) {
              this.eiaService.modifiedTracker.isChildCareError =
                this.assessment.childYouthSupportsSection.isSubmittedViaDriverFlow && !this.isSectionValid[InformalAssessment.ChildYouthSupportsSectionName];
            }

            this.eiaService.modifiedTracker.isFamilyBarriersSaved =
              !this.assessment.submitDate && this.assessment.familyBarriersSection.isSubmittedViaDriverFlow && this.isSectionValid[InformalAssessment.FamilyBarriersSectionName];
            if (!this.eiaService.modifiedTracker.isFamilyBarriersSaved) {
              this.eiaService.modifiedTracker.isFamilyBarriersError =
                this.assessment.familyBarriersSection.isSubmittedViaDriverFlow && !this.isSectionValid[InformalAssessment.FamilyBarriersSectionName];
            }

            this.eiaService.modifiedTracker.isNonCustodialParentsSaved =
              !this.assessment.submitDate &&
              this.assessment.nonCustodialParentsSection.isSubmittedViaDriverFlow &&
              this.isSectionValid[InformalAssessment.NonCustodialParentsSectionName];
            if (!this.eiaService.modifiedTracker.isNonCustodialParentsSaved) {
              this.eiaService.modifiedTracker.isNonCustodialParentsError =
                this.assessment.nonCustodialParentsSection.isSubmittedViaDriverFlow && !this.isSectionValid[InformalAssessment.NonCustodialParentsSectionName];
            }

            this.eiaService.modifiedTracker.isNonCustodialParentsReferralSaved =
              !this.assessment.submitDate &&
              this.assessment.nonCustodialParentsReferralSection.isSubmittedViaDriverFlow &&
              this.isSectionValid[InformalAssessment.NonCustodialParentsReferralSectionName];
            if (!this.eiaService.modifiedTracker.isNonCustodialParentsReferralSaved) {
              this.eiaService.modifiedTracker.isNonCustodialParentsReferralError =
                this.assessment.nonCustodialParentsReferralSection.isSubmittedViaDriverFlow && !this.isSectionValid[InformalAssessment.NonCustodialParentsReferralSectionName];
            }

            // If we are initializing this page when on a non-specific section route, then we will
            // auto-select the place we left off in the driver flow.
            if (!this.isInitialRouteSpecified) {
              if (!this.eiaService.modifiedTracker.isLanguageSaved) {
                // No need to do anything... we'll route to the languages page.
              } else if (!this.eiaService.modifiedTracker.isWorkHistorySaved) {
                this.onWorkHistory();
              } else if (!this.eiaService.modifiedTracker.isWorkProgramsSaved) {
                this.onWorkPrograms();
              } else if (!this.eiaService.modifiedTracker.isEducationSaved) {
                this.onEducation();
              } else if (!this.eiaService.modifiedTracker.isPostSecondarySaved) {
                this.onPostSecondaryEducation();
              } else if (!this.eiaService.modifiedTracker.isMilitarySaved) {
                this.onMilitaryTraining();
              } else if (!this.eiaService.modifiedTracker.isHousingSaved) {
                this.onHousing();
              } else if (!this.eiaService.modifiedTracker.isTransportationSaved) {
                this.onTransportation();
              } else if (!this.eiaService.modifiedTracker.isLegalIssuesSaved) {
                this.onLegalIssues();
              } else if (!this.eiaService.modifiedTracker.isParticipantBarriersSaved) {
                this.onParticipantBarriers();
              } else if (!this.eiaService.modifiedTracker.isChildCareSaved) {
                this.onChildCare();
              } else if (!this.eiaService.modifiedTracker.isFamilyBarriersSaved) {
                this.onFamilyBarriers();
              } else if (!this.eiaService.modifiedTracker.isNonCustodialParentsSaved) {
                this.onNonCustodialParents();
              } else if (!this.eiaService.modifiedTracker.isNonCustodialParentsReferralSaved) {
                this.onNonCustodialParentsReferral();
              }
            }
            this.isAssessmentLoaded = true;
            this.isSectionLoaded = true;
          }
        );
      }
    });
  }

  ngOnDestroy() {
    if (this.pinSub != null) {
      this.pinSub.unsubscribe();
    }
    if (this.partSub != null) {
      this.partSub.unsubscribe();
    }
    if (this.iaSectionSub != null) {
      this.iaSectionSub.unsubscribe();
    }
    if (this.pbSub != null) {
      this.pbSub.unsubscribe();
    }
  }

  private routeToEditSection(section: string): void {
    const url = `/pin/${this.pin}/assessment/edit/${section}`;
    this.router.navigateByUrl(url);
    this.updateActiveSectionForSidebar(section);
  }

  private routeToAssessmentSummary(): void {
    const url = `/pin/${this.pin}/assessment`;
    this.router.navigateByUrl(url);
  }

  // Sidebar Navigation
  onLanguages() {
    this.routeToEditSection('languages');
  }

  onEducation() {
    this.routeToEditSection('education');
  }

  onWorkPrograms() {
    this.routeToEditSection('work-programs');
  }

  onChildCare() {
    this.routeToEditSection('child-youth-supports');
  }

  onHousing() {
    this.routeToEditSection('housing');
  }

  onLegalIssues() {
    this.routeToEditSection('legal-issues');
  }

  onWorkHistory() {
    this.routeToEditSection('work-history');
  }

  onPostSecondaryEducation() {
    this.routeToEditSection('post-secondary');
  }

  onMilitaryTraining() {
    this.routeToEditSection('military');
  }

  onFamilyBarriers() {
    this.routeToEditSection('family-barriers');
  }

  onParticipantBarriers() {
    this.routeToEditSection('participant-barriers');
  }

  onNonCustodialParents() {
    this.routeToEditSection('non-custodial-parents');
  }

  onNonCustodialParentsReferral() {
    this.routeToEditSection('non-custodial-parents-referral');
  }

  onTransportation() {
    this.routeToEditSection('transportation');
  }

  toggleSidebar() {
    this.isSidebarCollapsed = !this.isSidebarCollapsed;
  }

  updateActiveSectionForSidebar(section: string) {
    this.isSectionLoaded = false;
    this.activeSection = section;
    // We need to determine if the section is needing validation
    switch (section) {
      case 'languages':
        this.isSectionNeedingValidation = !this.eiaService.hasLanguageValidated;
        break;
      case 'education':
        this.isSectionNeedingValidation = !this.eiaService.hasEducationValidated;
        break;
      case 'post-secondary':
        this.isSectionNeedingValidation = !this.eiaService.hasPostSecondaryValidated;
        break;
      case 'military':
        this.isSectionNeedingValidation = !this.eiaService.hasMilitaryValidated;
        break;
      case 'work-programs':
        this.isSectionNeedingValidation = !this.eiaService.hasWorkProgramsValidated;
        break;
      case 'child-youth-supports':
        this.isSectionNeedingValidation = !this.eiaService.hasChildCareValidated;
        break;
      case 'housing':
        this.isSectionNeedingValidation = !this.eiaService.hasHousingValidated;
        break;
      case 'transportation':
        this.isSectionNeedingValidation = !this.eiaService.hasTransportationValidated;
        break;
      case 'legal-issues':
        this.isSectionNeedingValidation = !this.eiaService.hasLegalIssuesValidated;
        break;
      case 'work-history':
        this.isSectionNeedingValidation = !this.eiaService.hasWorkHistoryValidated;
        break;
      case 'family-barriers':
        this.isSectionNeedingValidation = !this.eiaService.hasFamilyBarriersValidated;
        break;
      case 'participant-barriers':
        this.isSectionNeedingValidation = !this.eiaService.hasParticipantBarriersValidated;
        break;
      case 'non-custodial-parents':
        this.isSectionNeedingValidation = !this.eiaService.hasNonCustodialParentsValidated;
        break;
      case 'non-custodial-parents-referral':
        this.isSectionNeedingValidation = !this.eiaService.hasNonCustodialParentsReferralValidated;
        break;
    }

    setTimeout(() => {
      this.isSectionLoaded = true;
    }, 1);
  }

  saveAndContinueCurrentSection() {
    if (this.canSaveEmptySection()) {
      this.goToNextSection();
      return;
    }

    if (this.isSectionNeedingValidation) {
      this.eiaService.validateSection(this.activeSection);
      this.isSectionNeedingValidation = false;

      if (this.eiaService.isSectionValid()) {
        if (this.iaSectionSub != null) {
          this.iaSectionSub.unsubscribe();
        }
        this.isSaving = true;
        this.iaSectionSub = this.saveCurrentSection()
          .pipe(
            catchError(x => {
              this.isSaving = false;
              throw new Error('Save Failed');
            })
          )
          .subscribe(x => {
            this.isSaving = false;
            // In order to skip going into validation mode when we come back, we'll
            // call this.
            this.eiaService.sectionIsNowValid(this.activeSection);
            this.goToNextSection();
          });
      } else {
        this.eiaService.sectionComponent.scrollToTop();
        // Section Invalid... change button label.
      }
    } else {
      this.saveCurrentSection().subscribe(x => {
        this.goToNextSection();
      });
    }
  }

  handleSaveOnly() {
    if (this.canSaveEmptySection()) {
      return;
    }

    if (this.isSectionNeedingValidation) {
      this.eiaService.validateSection(this.activeSection);
      this.isSectionNeedingValidation = false;
    }

    if (this.iaSectionSub != null) {
      this.iaSectionSub.unsubscribe();
    }

    if (this.eiaService.isSectionValid()) {
      this.isSectionNeedingValidation = true;
      this.isSaving = true;
      this.iaSectionSub = this.saveCurrentSection()
        .pipe(
          catchError(x => {
            this.isSaving = false;
            throw new Error('Save Failed');
          })
        )
        .subscribe(x => {
          this.isSaving = false;
        });
    }
  }

  canSaveEmptySection(): boolean {
    let canSave = false;
    let model = null;
    let isNullModel = false;
    this.hasFcdpRole = this.appService.user.roles.indexOf('FCDP Case Manager') > -1;
    const isLanguageSectionModified =
      this.eiaService.modifiedTracker.isLanguageModified ||
      this.eiaService.modifiedTracker.isLanguageError ||
      this.eiaService.modifiedTracker.isLanguageSaved ||
      this.eiaService.modifiedTracker.isLanguageSaving;
    const isCysSectionModified =
      this.eiaService.modifiedTracker.isChildCareModified ||
      this.eiaService.modifiedTracker.isChildCareError ||
      this.eiaService.modifiedTracker.isChildCareSaved ||
      this.eiaService.modifiedTracker.isChildCareSaving;
    const isWorkProgramsSectionModified =
      this.eiaService.modifiedTracker.isWorkProgramsModified ||
      this.eiaService.modifiedTracker.isWorkProgramsError ||
      this.eiaService.modifiedTracker.isWorkProgramsSaved ||
      this.eiaService.modifiedTracker.isWorkProgramsSaving;
    const isFamilyBarriersSectionModified =
      this.eiaService.modifiedTracker.isFamilyBarriersModified ||
      this.eiaService.modifiedTracker.isFamilyBarriersError ||
      this.eiaService.modifiedTracker.isFamilyBarriersSaved ||
      this.eiaService.modifiedTracker.isFamilyBarriersSaving;

    if (this.activeSection === 'languages') {
      model = this.eiaService.getSavableSectionModel();
      if (model && !model.modifiedBy) isNullModel = true;
      canSave = this.hasFcdpRole && isNullModel && !isLanguageSectionModified;
      if (canSave) {
        this.languageSectionSaved = true;
      }
      return canSave;
    }

    if (this.activeSection === 'child-youth-supports') {
      model = this.eiaService.getSavableSectionModel();
      if (model && !model.modifiedBy) isNullModel = true;
      canSave = this.hasFcdpRole && isNullModel && !isCysSectionModified;
      if (canSave) {
        this.childYouthSectionSaved = true;
      }
      return canSave;
    }

    if (this.activeSection === 'work-programs') {
      model = this.eiaService.getSavableSectionModel();
      if (model && !model.modifiedBy) isNullModel = true;
      canSave = this.hasFcdpRole && isNullModel && !isWorkProgramsSectionModified;
      if (canSave) {
        this.workProgramsSectionSaved = true;
      }
      return canSave;
    }

    if (this.activeSection === 'family-barriers') {
      model = this.eiaService.getSavableSectionModel();
      if (model && !model.modifiedBy) isNullModel = true;
      canSave = this.hasFcdpRole && isNullModel && !isFamilyBarriersSectionModified;
      if (canSave) {
        this.familyBarriersSectionSaved = true;
      }
      return canSave;
    }

    return canSave;
  }

  sectionIsNowValid() {
    // console.log('change button label to Save');
    this.isSectionNeedingValidation = true;
  }

  saveCurrentSection(): Observable<any> {
    if (this.activeSection === 'languages') {
      return this.saveLanguages();
    } else if (this.activeSection === 'education') {
      return this.saveEducation();
    } else if (this.activeSection === 'post-secondary') {
      return this.savePostSecondary();
    } else if (this.activeSection === 'military') {
      return this.saveMilitary();
    } else if (this.activeSection === 'work-programs') {
      return this.saveWorkPrograms();
    } else if (this.activeSection === 'child-youth-supports') {
      return this.saveChildCare();
    } else if (this.activeSection === 'housing') {
      return this.saveHousing();
    } else if (this.activeSection === 'legal-issues') {
      return this.saveLegalIssues();
    } else if (this.activeSection === 'work-history') {
      return this.saveWorkHistory();
    } else if (this.activeSection === 'family-barriers') {
      return this.saveFamilyBarriers();
    } else if (this.activeSection === 'participant-barriers') {
      return this.saveParticipantBarriers();
    } else if (this.activeSection === 'non-custodial-parents') {
      return this.saveNonCustodialParents();
    } else if (this.activeSection === 'non-custodial-parents-referral') {
      return this.saveNonCustodialParentsReferral();
    } else if (this.activeSection === 'transportation') {
      return this.saveTransportation();
    }
  }

  goToNextSection() {
    if (this.activeSection === 'languages') {
      this.onWorkHistory();
    } else if (this.activeSection === 'work-history') {
      this.onWorkPrograms();
    } else if (this.activeSection === 'work-programs') {
      this.onEducation();
    } else if (this.activeSection === 'education') {
      this.onPostSecondaryEducation();
    } else if (this.activeSection === 'post-secondary') {
      this.onMilitaryTraining();
    } else if (this.activeSection === 'military') {
      this.onHousing();
    } else if (this.activeSection === 'housing') {
      this.onTransportation();
    } else if (this.activeSection === 'transportation') {
      this.onLegalIssues();
    } else if (this.activeSection === 'legal-issues') {
      if (this.hasPBAccessBol) {
        this.onParticipantBarriers();
      } else {
        this.onChildCare();
      }
    } else if (this.activeSection === 'participant-barriers') {
      this.onChildCare();
    } else if (this.activeSection === 'child-youth-supports') {
      this.onFamilyBarriers();
    } else if (this.activeSection === 'family-barriers') {
      this.onNonCustodialParents();
    } else if (this.activeSection === 'non-custodial-parents') {
      this.onNonCustodialParentsReferral();
    } else if (this.activeSection === 'non-custodial-parents-referral') {
      this.routeToAssessmentSummary();
    }
    // else do nothing
  }

  goToPreviousSection() {
    if (this.activeSection === 'non-custodial-parents-referral') {
      this.onNonCustodialParents();
    } else if (this.activeSection === 'non-custodial-parents') {
      this.onFamilyBarriers();
    } else if (this.activeSection === 'family-barriers') {
      this.onChildCare();
    } else if (this.activeSection === 'child-youth-supports') {
      if (this.hasPBAccessBol) {
        this.onParticipantBarriers();
      } else {
        this.onLegalIssues();
      }
    } else if (this.activeSection === 'participant-barriers') {
      this.onLegalIssues();
    } else if (this.activeSection === 'legal-issues') {
      this.onTransportation();
    } else if (this.activeSection === 'transportation') {
      this.onHousing();
    } else if (this.activeSection === 'housing') {
      this.onMilitaryTraining();
    } else if (this.activeSection === 'military') {
      this.onPostSecondaryEducation();
    } else if (this.activeSection === 'post-secondary') {
      this.onEducation();
    } else if (this.activeSection === 'education') {
      this.onWorkPrograms();
    } else if (this.activeSection === 'work-programs') {
      this.onWorkHistory();
    } else if (this.activeSection === 'work-history') {
      this.onLanguages();
    }
    // else do nothing
  }

  saveLanguages(): Observable<LanguagesSection> {
    return this.eiaService.postLanguageSection(this.eiaService.getSavableSectionModel(), this.eiaService.isSectionValid()).pipe(
      map(x => {
        this.eiaService.refreshModel();
        return x;
      })
    );
  }

  saveEducation(): Observable<EducationHistorySection> {
    return this.eiaService.postEducationSection(this.eiaService.getSavableSectionModel(), this.eiaService.isSectionValid()).pipe(
      map(x => {
        this.eiaService.refreshModel();
        return x;
      })
    );
  }

  savePostSecondary(): Observable<PostSecondaryEducationSection> {
    return this.eiaService.postPostSecondarySection(this.eiaService.getSavableSectionModel(), this.eiaService.isSectionValid()).pipe(
      map(x => {
        this.eiaService.refreshModel();
        return x;
      })
    );
  }

  saveMilitary(): Observable<MilitarySection> {
    return this.eiaService.postMilitarySection(this.eiaService.getSavableSectionModel(), this.eiaService.isSectionValid()).pipe(
      map(x => {
        this.eiaService.refreshModel();
        return x;
      })
    );
  }

  saveWorkPrograms(): Observable<WorkProgramsSection> {
    return this.eiaService.postWorkProgramsSection(this.eiaService.getSavableSectionModel(), this.eiaService.isSectionValid()).pipe(
      map(x => {
        this.eiaService.refreshModel();
        return x;
      })
    );
  }

  saveChildCare(): Observable<ChildAndYouthSupportsSection> {
    return this.eiaService.postChildCareSection(this.eiaService.getSavableSectionModel(), this.eiaService.isSectionValid()).pipe(
      map(x => {
        this.eiaService.refreshModel();
        return x;
      })
    );
  }

  saveHousing(): Observable<HousingSection> {
    return this.eiaService.postHousingSection(this.eiaService.getSavableSectionModel(), this.eiaService.isSectionValid()).pipe(
      map(x => {
        this.eiaService.refreshModel();
        return x;
      })
    );
  }

  saveLegalIssues(): Observable<LegalIssuesSection> {
    return this.eiaService.postLegalIssuesSection(this.eiaService.getSavableSectionModel(), this.eiaService.isSectionValid()).pipe(
      map(x => {
        this.eiaService.refreshModel();
        return x;
      })
    );
  }

  saveWorkHistory(): Observable<WorkHistorySection> {
    return this.eiaService.postWorkHistorySection(this.eiaService.getSavableSectionModel(), this.eiaService.isSectionValid()).pipe(
      map(x => {
        this.eiaService.refreshModel();
        return x;
      })
    );
  }

  saveFamilyBarriers(): Observable<FamilyBarriersSection> {
    return this.eiaService.postFamilyBarriersSection(this.eiaService.getSavableSectionModel(), this.eiaService.isSectionValid()).pipe(
      map(x => {
        this.eiaService.refreshModel();
        return x;
      })
    );
  }

  saveParticipantBarriers(): Observable<ParticipantBarriersSection> {
    return this.eiaService.postParticipantBarriersSection(this.eiaService.getSavableSectionModel(), this.eiaService.isSectionValid()).pipe(
      map(x => {
        this.eiaService.refreshModel();
        return x;
      })
    );
  }

  saveNonCustodialParents(): Observable<NonCustodialParentsSection> {
    return this.eiaService.postNonCustodialParentsSection(this.eiaService.getSavableSectionModel(), this.eiaService.isSectionValid()).pipe(
      map(x => {
        this.eiaService.refreshModel();
        return x;
      })
    );
  }

  saveNonCustodialParentsReferral(): Observable<NonCustodialParentsReferralSection> {
    return this.eiaService.postNonCustodialParentsReferralSection(this.eiaService.getSavableSectionModel(), this.eiaService.isSectionValid()).pipe(
      map(x => {
        this.eiaService.refreshModel();
        return x;
      })
    );
  }

  saveTransportation(): Observable<TransportationSection> {
    return this.eiaService.postTransportationSection(this.eiaService.getSavableSectionModel(), this.eiaService.isSectionValid()).pipe(
      map(x => {
        this.eiaService.refreshModel();
        return x;
      })
    );
  }
}
