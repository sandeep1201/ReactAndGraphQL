import { Input, ComponentRef, EventEmitter, OnChanges, Output } from '@angular/core';

import { AppService } from 'src/app/core/services/app.service';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { HistoryManager } from '../../../shared/models/history-manager';
import { InformalAssessmentService } from '../../../shared/services/informal-assessment.service';
import { Utilities } from '../../../shared/utilities';
import { ModalService } from 'src/app/core/modal/modal.service';
import { InputHistoryComponent } from '../../../shared/components/input-history/input-history.component';
import { LanguagesSection } from '../../../shared/models/languages-section';
import { WorkHistorySection } from '../../../shared/models/work-history-section';
import { WorkProgramsSection } from '../../../shared/models/work-programs-section';
import { EducationHistorySection } from '../../../shared/models/education-history-section';
import { PostSecondaryEducationSection } from '../../../shared/models/post-secondary-education-section';
import { MilitarySection } from '../../../shared/models/military-section';
import { HousingSection } from '../../../shared/models/housing-section';
import { TransportationSection } from '../../../shared/models/transportation-section';
import { LegalIssuesSection } from '../../../shared/models/legal-issues-section';
import { ParticipantBarriersSection } from '../../../shared/models/participant-barriers-section';
import { ChildAndYouthSupportsSection } from '../../../shared/models/child-youth-supports-section';
import { NonCustodialParentsReferralSection } from '../../../shared/models/non-custodial-parents-referral-section';

import { UpdateSectionCallback } from '../../../shared/types/update-section-callback';

import * as _ from 'lodash';
import { NonCustodialParentsSection } from '../../../shared/models/non-custodial-parents-section';
import { FamilyBarriersSection } from '../../../shared/models/family-barriers-section';

export abstract class BaseOverviewSecton implements OnChanges {
  Utilities: Utilities;

  @Input('modelErrors')
  set modelErrors(value: ModelErrors) {
    // Do not allow modelErrors to become undefined at any point.
    if (value != null) {
      this._modelErrors = value;
    }
  }

  get modelErrors() {
    return this._modelErrors;
  }

  // Note: this must be initialized to an empty object for the UI in initial state.
  @Input()
  pin: string;
  @Input()
  hasEditAccess = false;
  @Output()
  reenableValidation = new EventEmitter();

  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public historyManager: HistoryManager;
  public isCollapsed = false;
  public cachedHistory: any[];
  public cachedSection: any;
  private tempModalRef: ComponentRef<InputHistoryComponent>;
  private modalServiceBase: ModalService;
  private _modelErrors: ModelErrors = {};
  public isHistoryActive = false;

  constructor(public appService: AppService, public iaService: InformalAssessmentService, public modalService: ModalService) {
    this.modalServiceBase = modalService;
  }

  /**
   *  Initializes HistoryManager and grabs our history. Only need repeatedObject, repeaterName
   *  and propertyId when are we dealing with a repeater.
   *
   * @param {string} sectionName
   * @memberof BaseOverviewSecton
   */
  getHistory(routeName: string) {
    this.iaService.getHistoryBySection(this.pin, routeName).subscribe(data => this.initHistory(data, routeName));
  }

  ngOnChanges() {}

  protected toggleHistory($event: any, sectionName: string, section: any, usc: UpdateSectionCallback) {
    this.isHistoryActive = $event;
    if (this.isHistoryActive === true) {
      this.cachedSection = section;
      this.getHistory(sectionName);

      // Disable validation.
      this.validationManager.resetErrors();
      this.modelErrors = {};
    } else {
      if (this.cachedSection) {
        usc(this.cachedSection);

        // Re-enable the validation.
        this.emitReenableValidation();
      }
    }
  }

  private initHistory(jsonString: string, routeName: string) {
    this.historyManager = new HistoryManager(this.iaService, this.pin);
    if (jsonString != null) {
      const sections = jsonString;
      for (const section of sections) {
        let sectionHistory;

        switch (routeName) {
          case 'languages':
            sectionHistory = new LanguagesSection();
            break;

          case 'work-history':
            sectionHistory = new WorkHistorySection();
            break;

          case 'work-programs':
            sectionHistory = new WorkProgramsSection();
            break;

          case 'education':
            sectionHistory = new EducationHistorySection();
            break;

          case 'post-secondary':
            sectionHistory = new PostSecondaryEducationSection();
            break;

          case 'military':
            sectionHistory = new MilitarySection();
            break;

          case 'housing':
            sectionHistory = new HousingSection();
            break;

          case 'transportation':
            sectionHistory = new TransportationSection();
            break;

          case 'legal-issues':
            sectionHistory = new LegalIssuesSection();
            break;

          case 'participant-barriers':
            sectionHistory = new ParticipantBarriersSection();
            break;

          case 'child-youth-supports':
            sectionHistory = new ChildAndYouthSupportsSection();
            break;

          case 'family-barriers':
            sectionHistory = new FamilyBarriersSection();
            break;

          case 'non-custodial-parents-referral':
            sectionHistory = new NonCustodialParentsReferralSection();
            break;

          case 'non-custodial-parents':
            sectionHistory = new NonCustodialParentsSection();
            break;
        }

        this.historyManager.history.push(sectionHistory.deserialize(section));
      }

      this.historyManager.initHistoryDrop();
    }
  }

  toggleCollapse($event) {
    this.isCollapsed = $event;
  }

  isStringEmptyOrNull(str: string): boolean {
    return Utilities.isStringEmptyOrNull(str);
  }

  isModelErrorsItemInvalid(i: number, repeaterName: string, property: string): boolean {
    return Utilities.isModelErrorsItemInvalid(this.modelErrors[repeaterName] as ModelErrors[], i, property);
  }

  isModelErrorsChildItemInvalid(i: number, j: number, repeaterName: string, childRepeaterName: string, property: string): any {
    const errors = this.modelErrors[repeaterName] as ModelErrors;
    return Utilities.isModelErrorsChildItemInvalid(errors, childRepeaterName, i, j, property);
  }

  protected emitReenableValidation() {
    this.reenableValidation.emit();
  }
}
