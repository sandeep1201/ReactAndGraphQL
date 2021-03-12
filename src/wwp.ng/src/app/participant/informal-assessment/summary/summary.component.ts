import { Utilities } from './../../../shared/utilities';
// tslint:disable: import-blacklist
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { Authorization } from '../../../shared/models/authorization';
import { BaseParticipantComponent } from '../../../shared/components/base-participant-component';
import { BooleanDictionary } from '../../../shared/interfaces/boolean-dictionary';
import { Dictionary } from '../../../shared/dictionary';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { InformalAssessment } from '../../../shared/models/informal-assessment';
import { InformalAssessmentService } from '../../../shared/services/informal-assessment.service';
import { LogService } from '../../../shared/services/log.service';
import { ParticipantService } from '../../../shared/services/participant.service';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { AppService } from 'src/app/core/services/app.service';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { AccessType } from '../../../shared/enums/access-type.enum';
import { EnrolledProgramStatus } from '../../../shared/enums/enrolled-program-status.enum';

declare var $: any;

@Component({
  selector: 'app-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.css'],
  providers: [InformalAssessmentService, FieldDataService]
})
export class SummaryComponent extends BaseParticipantComponent implements OnInit, OnDestroy {
  Auth = Authorization;
  AccessType = AccessType;
  public assessment: InformalAssessment;
  public goBackUrl: string;
  public isSidebarCollapsed = false;
  public pin: string;
  public hasPBAccessBol: boolean;
  public canRequestPBAccess: boolean;
  public hasFBAccessBol: boolean;
  public canRequestFBAccess: boolean;
  public fbAccessType: AccessType;
  public pbSub: Subscription;
  public fbSub: Subscription;

  public isLoaded = false;
  public isReadOnly = true;
  public isSubmitable = false;
  public showSubmitSuccess = false;
  public showSubmitFail = false;
  public isSectionValid: BooleanDictionary = {};
  public modelErrors: ModelErrors = {};
  public isSaving = false;
  public validationManagers: Dictionary<string, ValidationManager>;

  get canAccessPB(): boolean {
    return this.appService.isUserAuthorized(Authorization.canAccessPB, null);
  }
  get canRequestRestrictedAccess(): boolean {
    return this.appService.isUserAuthorized(Authorization.canRequestRestrictedAccess, null);
  }

  get hasFcdpRole(): boolean {
    return this.appService.user.roles.indexOf('FCDP Case Manager') > -1 || this.appService.user.roles.indexOf('FCDP Query Only Access') > -1;
  }

  constructor(
    public appService: AppService,
    protected route: ActivatedRoute,
    protected router: Router,
    private logService: LogService,
    protected partService: ParticipantService,
    private iaService: InformalAssessmentService
  ) {
    super(route, router, partService);
    
    // jQuery for sidebar
    $(window).on('scroll', function() {
      const o = $('#sidebar');
      if (1.2 * $('#sidebar').outerHeight() < $(window).height()) {
        const e = $(window).scrollTop(),
          s = e - $('app-header').outerHeight() - $('app-sub-header').outerHeight() + 0;
        s > 0 ? o.css('top', s + 'px') : o.css('top', '0');
      } else o.css('top', '0');      

    });

    // jQuery for scrolling to sections
    $(document).on('click', '#sidebar li.menu-item', function() {
      const sec = $(this).data('goto');
      $('html, body').stop();
      if (sec) {
        $('html, body').animate(
          {
            scrollTop: $('#' + sec).offset().top - 75
          },
          1000
        );
      }
    });
  }

  ngOnInit() {
    super.onInit();
  }

  onPinInit() {
    this.goBackUrl = '/pin/' + this.pin;
  }

  onParticipantInit() {
    this.iaService.getInformalAssessment(this.pin).subscribe(ia => {
      this.assessment = ia;
      // initializing the PBSection Behavior subject here
      this.iaService.initPbvars(this.pin);

      if (this.assessment != null && this.assessment.id !== 0) {
        this.iaService.loadValidationContextsAndValidate(this.participant, this.assessment).subscribe(
          n => n,
          e => e,
          () => {
            // The next and error events above are not needed.  We only care about
            // knowing when the validation is complete so we can update our class
            // properties to those from the service.
            this.isSectionValid = this.iaService.isSectionValid;
            this.modelErrors = this.iaService.modelErrors;
            this.validationManagers = this.iaService.validationManagers;

            // We should enable the Submit when we are in an assessment and all sections
            // are valid.
            if (this.assessment.id === 0 || this.assessment.submitDate != null) {
              // We either don't have an assessment or it's already been submitted.
              this.isSubmitable = false;
            } else {
              // We have one so check if all the sections are valid.
              const hasFcdpRole = this.appService.user.roles.indexOf('FCDP Case Manager') > -1;
              const isLanguageSectionEmpty = this.assessment.languagesSection.modifiedBy === null;
              const isCysSectionEmpty = this.assessment.childYouthSupportsSection.modifiedBy === null;
              const canSubmitEmptyLanguage = hasFcdpRole && isLanguageSectionEmpty;
              const canSubmitEmptyCys = hasFcdpRole && isCysSectionEmpty;
              const isWorkProgramsSectionEmpty = this.assessment.workProgramSection.modifiedBy === null;
              const isFamilyBarriersSectionEmpty = this.assessment.familyBarriersSection.modifiedBy === null;
              const canSubmitEmptyWorkProgram = hasFcdpRole && isWorkProgramsSectionEmpty;
              const canSubmitEmptyFamilyBarriers = hasFcdpRole && isFamilyBarriersSectionEmpty;
              this.isSubmitable = this.iaService.areAllSectionsValid(canSubmitEmptyLanguage, canSubmitEmptyCys, canSubmitEmptyWorkProgram, canSubmitEmptyFamilyBarriers);
            }

            this.iaService.participant = this.participant;
            this.checkAccess();
            this.isLoaded = true;
            this.appService.hasPBAccess(this.participant);
            this.appService.hasPHIAccess(this.participant);
            this.pbSub = this.appService.PBSection.subscribe(res => {
              this.hasPBAccessBol = res.hasPBAccessBol;
              this.canRequestPBAccess = res.canRequestPBAccess;
            });
            this.fbSub = this.appService.FBSection.subscribe(res => {
              this.hasFBAccessBol = res.hasFBAccessBol;
              this.canRequestFBAccess = res.canRequestFBAccess;
            });
          }
        );
      } else {
        this.checkAccess();
        this.isLoaded = true;
      }
    });
  }

  checkAccess() {
    if (this.participant) {
      let hasAuth = false;
      hasAuth = this.appService.isUserAuthorized(Authorization.canAccessInformalAssessment_Edit, this.participant);
      this.isReadOnly = this.appService.checkReadOnlyAccess(this.participant, [EnrolledProgramStatus.enrolled], hasAuth);
    }
  }

  ngOnDestroy() {
    if (this.pbSub != null) {
      this.pbSub.unsubscribe();
    }
  }

  // onDashboard() {
  //   this.router.navigateByUrl('home');
  // }

  onEdit() {
    this.router.navigateByUrl(`/pin/${this.pin}/assessment/edit`);
  }

  onNew() {
    this.isSaving = true;
    this.iaService.createNewAssessment(this.participant).subscribe(
      n => n,
      e => {
        this.isSaving = false;
      },
      () => {
        // The next and error events above are not needed.  We only care about
        // knowing when the call is complete.
        this.isSaving = false;
        this.onEdit();
      }
    );
  }

  onSubmit() {
    this.iaService.submitAssessment(this.participant).subscribe(
      n => n,
      e => e,
      () => {
        // To optimize the performance, we'll just manually set the assessment submit date.
        this.assessment.submitDate = Utilities.currentDate;
        this.isSubmitable = false;
        this.showSubmitSuccess = true;
      }
    );
  }

  closeSubmit() {
    this.showSubmitSuccess = false;
    this.showSubmitFail = false;
  }

  toggleSidebar() {
    this.isSidebarCollapsed = !this.isSidebarCollapsed;
  }

  hasEditIaAccess(): boolean {
    if (this.appService.coreAccessContext) {
      if (
        this.appService.coreAccessContext.evaluate() === AccessType.edit &&
        this.participant.isCurrentlyEnrolled(this.appService.user.agencyCode) === true &&
        this.assessment != null &&
        (this.assessment.id > 0 || this.assessment.submitDate !== null)
      ) {
        return true;
      } else {
        return false;
      }
    } else {
      return false;
    }
  }

  onReenableValidation() {
    this.iaService.validate(this.assessment);
  }
}
