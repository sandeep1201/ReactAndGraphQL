import { EAViewModes } from './../../models/ea-request-sections.enum';
import { AppService } from 'src/app/core/services/app.service';
import { Utilities } from './../../../../shared/utilities';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Participant } from 'src/app/shared/models/participant';
import { Observable, Subscription } from 'rxjs';
import { take, catchError, map, concatMap } from 'rxjs/operators';
import * as _ from 'lodash';
import { ActivatedRoute, Router } from '@angular/router';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { EARequestEditService } from '../../services/ea-request-edit.service';
import { EARequestSections, EAStatusCodes } from '../../models/ea-request-sections.enum';
import { Location } from '@angular/common';
import { EARequest } from '../../models';
import { BooleanDictionary } from 'src/app/shared/interfaces/boolean-dictionary';

@Component({
  selector: 'app-ea-request-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss'],
  providers: [EARequestEditService]
})
export class EARequestEditComponent implements OnInit, OnDestroy {
  erSectionSub: Subscription;
  participant: Participant;
  goBackUrl: string;
  pin: string;
  requestId: string;
  viewMode = EAViewModes.View;
  goToUrl: EARequestSections = null;
  isEARequestLoaded = false;
  isSectionLoaded = false;
  isSectionNeedingValidation = true;
  isSidebarCollapsed = false;
  isSaving = false;
  pageTitleType = '';
  activeSection = EARequestSections.Demographics;
  sectionsArray = Object.values(EARequestSections);
  public isSectionValid: BooleanDictionary = {};

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private partService: ParticipantService,
    public requestEditService: EARequestEditService,
    private location: Location,
    public appService: AppService
  ) {
    // jQuery for sidebar
    Utilities.asideSlider();
  }

  ngOnInit(): void {
    const segments = this.router.url.split('/');
    if (segments.length === 8) {
      // this.isInitialRouteSpecified = true;
      this.updateActiveSectionForSidebar(segments[segments.length - 1] as EARequestSections);
    }
    this.route.params
      .pipe(
        concatMap(params => {
          this.requestId = params['id'];
          this.pin = params['pin'];
          this.viewMode = params['mode'];
          this.goBackUrl = `/pin/${this.pin}/emergency-assistance/ea-application-history`;
          if (+this.requestId !== 0) {
            this.goBackUrl += `/${this.requestId}`;
          }
          if (this.viewMode === EAViewModes.Edit) this.pageTitleType = +this.requestId !== 0 ? 'Edit' : 'New';
          this.requestEditService.setParentComponent(this);
          return this.partService.getCachedParticipant(this.pin);
        })
      )
      .pipe(take(1))
      .subscribe(x => {
        this.participant = x as Participant;
        this.requestEditService.participant = this.participant;
        this.initSection();
      });
  }

  initSection() {
    this.requestEditService.getEARequest(this.pin, this.requestId).subscribe(ea => {
      const request = ea;
      if (request !== null && request.statusCode === EAStatusCodes.InProgress && this.viewMode === EAViewModes.Edit) {
        this.requestEditService.loadValidationContextsAndValidate(request).subscribe(
          n => n,
          e => e,
          () => {
            this.isSectionValid = this.requestEditService.isSectionValidOnLoad;
            this.isSectionLoaded = true;
            this.isEARequestLoaded = true;
          }
        );
      } else {
        this.isSectionLoaded = true;
        this.isEARequestLoaded = true;
      }
    });
  }

  updateActiveSectionForSidebar(section: EARequestSections) {
    this.isSectionLoaded = false;
    this.activeSection = section;
    this.isSectionNeedingValidation = !this.requestEditService.modifiedTracker[this.requestEditService.getSectionLabel(section)].validated;

    setTimeout(() => {
      this.isSectionLoaded = true;
    }, 1);
  }

  goToSection(section: EARequestSections) {
    if (section !== this.activeSection) {
      if (this.requestEditService.modifiedTracker[this.requestEditService.getSectionLabel(this.activeSection)].modified) {
        this.appService.isDialogPresent = true;
        this.goToUrl = section;
      } else this.navigateTo(section);
    }
  }

  exitEAEditIgnoreChanges() {
    if (this.goToUrl !== null) {
      EARequest.clone(this.requestEditService.lastSavedModel, this.requestEditService.model);
      this.requestEditService.setModifiedModel(this.requestEditService.getSectionLabel(this.activeSection), false);
      this.navigateTo(this.goToUrl);
      this.goToUrl = null;
    }
  }

  navigateTo(section: EARequestSections) {
    if (section !== this.activeSection) {
      this.router.navigateByUrl(`/pin/${this.pin}/emergency-assistance/ea-application-history/${this.requestId}/${this.viewMode}/${section}`);
      this.updateActiveSectionForSidebar(section);
    }
  }

  goToNextSection() {
    const index = this.sectionsArray.indexOf(this.activeSection);
    if (this.sectionsArray.length - 1 === index) {
      this.router.navigateByUrl(`/pin/${this.pin}/emergency-assistance/ea-application-history/${this.requestId}`);
    } else this.navigateTo(this.sectionsArray[index + 1]);
  }

  goToPreviousSection() {
    const index = this.sectionsArray.indexOf(this.activeSection);
    if (this.sectionsArray.length === 0) {
      this.router.navigateByUrl(this.goBackUrl);
    } else this.navigateTo(this.sectionsArray[index - 1]);
  }

  handleSaveOnly() {
    if (!this.requestEditService.modifiedTracker.isSaveDisabled) {
      if (this.isSectionNeedingValidation) {
        this.requestEditService.validateSection(this.requestEditService.getSectionLabel(this.activeSection));
        this.isSectionNeedingValidation = false;
      }
      if (this.erSectionSub != null) this.erSectionSub.unsubscribe();
      if (this.requestEditService.isSectionValid()) {
        this.isSectionNeedingValidation = true;
        this.isSaving = true;
        this.erSectionSub = this.saveCurrentSection()
          .pipe(
            catchError(x => {
              this.isSaving = false;
              throw new Error('Save Failed');
            })
          )
          .subscribe(x => {
            this.isSaving = false;
          });
      } else {
        //scroll to the top
        this.requestEditService.sectionComponent.scrollToTop();
      }
    }
  }

  saveAndContinueCurrentSection() {
    if (this.isSectionNeedingValidation) {
      this.requestEditService.validateSection(this.requestEditService.getSectionLabel(this.activeSection));
      this.isSectionNeedingValidation = false;

      if (this.requestEditService.isSectionValid()) {
        if (this.erSectionSub != null) {
          this.erSectionSub.unsubscribe();
        }
        this.isSaving = true;
        this.erSectionSub = this.saveCurrentSection()
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
            this.requestEditService.sectionIsNowValid(this.requestEditService.getSectionLabel(this.activeSection));
            this.goToNextSection();
          });
      } else {
        this.requestEditService.sectionComponent.scrollToTop();
        // Section Invalid... change button label.
      }
    } else {
      this.isSaving = true;
      this.saveCurrentSection()
        .pipe(
          catchError(x => {
            this.isSaving = false;
            throw new Error('Save Failed');
          })
        )
        .subscribe(x => {
          this.isSaving = false;
          this.goToNextSection();
        });
    }
  }

  sectionIsNowValid() {
    this.isSectionNeedingValidation = true;
  }

  saveCurrentSection(): Observable<any> {
    let result: Observable<any>;

    if (this.activeSection === EARequestSections.Demographics)
      result = this.requestEditService.postDemographicsSection(this.requestEditService.getSavableSectionModel(), this.requestEditService.isSectionValid()).pipe(
        map(response => {
          if (this.requestId === '0' && response.id >= 0) {
            this.location.replaceState(`/pin/${this.pin}/emergency-assistance/ea-application-history/${this.requestEditService.model.eaDemographics.requestId}/edit`);
          }
          this.requestId = response.id.toString();
          this.requestEditService.refreshModel();
        })
      );

    if (this.activeSection === EARequestSections.Emergency)
      result = this.requestEditService.postEmergencyTypeSection(this.requestEditService.getSavableSectionModel(), this.requestEditService.isSectionValid()).pipe(
        map(response => {
          this.requestEditService.refreshModel();
        })
      );

    if (this.activeSection === EARequestSections.Members)
      result = this.requestEditService.postHouseholdMemberseSection(this.requestEditService.getSavableSectionModel(), this.requestEditService.isSectionValid()).pipe(
        map(response => {
          this.requestEditService.refreshModel();
        })
      );

    if (this.activeSection === EARequestSections.Financials)
      result = this.requestEditService.postHouseholdFinancialsSection(this.requestEditService.getSavableSectionModel(), this.requestEditService.isSectionValid()).pipe(
        map(response => {
          this.requestEditService.refreshModel();
        })
      );

    if (this.activeSection === EARequestSections.AgencySummary)
      result = this.requestEditService.postAgencySummarySection(this.requestEditService.getSavableSectionModel(), this.requestEditService.isSectionValid()).pipe(
        map(response => {
          this.requestEditService.refreshModel();
        })
      );
    return result;
  }

  ngOnDestroy() {
    if (this.erSectionSub != null) {
      this.erSectionSub.unsubscribe();
    }
  }
}
