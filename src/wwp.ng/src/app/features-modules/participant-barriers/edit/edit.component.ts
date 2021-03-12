// tslint:disable: no-output-on-prefix
import { Component, EventEmitter, Output, Input, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AppService } from './../../../core/services/app.service';
import { BarrierSubType } from '../../../shared/models/job-actions';
import { BaseParticipantComponent } from '../../../shared/components/base-participant-component';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { DropDownMultiField } from '../../../shared/models/dropdown-multi-field';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { ModelErrors } from '../../../shared/interfaces/model-errors';
import { ParticipantBarrier } from '../../../shared/models/participant-barriers-app';
import { ParticipantBarrierAppService } from '../../../shared/services/participant-barrier-app.service';
import { ParticipantService } from '../../../shared/services/participant.service';
import { ValidationManager } from '../../../shared/models/validation-manager';
import { Utilities } from '../../../shared/utilities';

declare var $: any;

@Component({
  selector: 'app-participant-barriers-edit-app',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css'],
  providers: [FieldDataService]
})
export class ParticipantBarriersEditComponent extends BaseParticipantComponent implements OnInit, OnDestroy {
  @Output() onSaveAndExit = new EventEmitter();

  // Set to 0 for new records.
  @Input() barrierId = 0;

  @Input() defaultbarrierType = '';

  private pbdSub: Subscription;
  private pbSub: Subscription;

  // This is an array with arrays.
  private allparticipantBarriersDrop: any;

  public model: ParticipantBarrier;
  public originalModel: ParticipantBarrier;
  public participantBarriersDrop: DropDownField[] = [];
  public subBarriersDrop: DropDownMultiField[] = [];

  private physicalHealthId: number;
  private mentalHealthId: number;
  public domesticViolenceId: number;
  public aodaId: number;
  private cognitiveLearningId: number;

  public isCollapsed = false;
  public hadSaveError = false;
  private hasTriedSave = false;
  public isSaving = false;
  private isSectionModified = false;
  private isSectionValid = true;
  public otherPCId: number;
  public otherMCId: number;
  public intervalTypeDayId: number;
  public intervaTypeWeekId: number;
  public intervalTypeDrop: DropDownField[];
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public modelErrors: ModelErrors = {};
  public isConvertedRecord = false;

  constructor(
    public appService: AppService,
    private participantBarrierAppService: ParticipantBarrierAppService,
    private fdService: FieldDataService,
    route: ActivatedRoute,
    router: Router,
    partService: ParticipantService
  ) {
    super(route, router, partService);
  }

  ngOnInit() {
    super.onInit();

    // We need the subBarrier drop before we apply the data.
    this.pbdSub = this.fdService.getParticipantBarriers().subscribe(data => {
      this.initParticipantBarriers(data);
      if (this.barrierId !== 0) {
        this.pbSub = this.participantBarrierAppService.getParticipantBarrier(this.barrierId).subscribe(brData => this.initParticipantBarrier(brData));
      } else {
        this.model = new ParticipantBarrier().create();
        this.originalModel = new ParticipantBarrier().create();
        this.originalModel.barrierSubType = new BarrierSubType();
        this.originalModel.barrierSubType.barrierSubTypes = [];
        if (this.defaultbarrierType !== '' && this.originalModel != null) {
          switch (this.defaultbarrierType) {
            case 'Physical Health': {
              this.model.barrierTypeId = this.physicalHealthId;
              this.originalModel.barrierTypeId = this.physicalHealthId;
              this.initSubBarriersDrop(false);
              break;
            }
            case 'Mental Health': {
              this.model.barrierTypeId = this.mentalHealthId;
              this.originalModel.barrierTypeId = this.mentalHealthId;
              this.initSubBarriersDrop(false);
              break;
            }
            case 'AODA': {
              this.model.barrierTypeId = this.aodaId;
              this.originalModel.barrierTypeId = this.aodaId;
              this.initSubBarriersDrop(false);
              break;
            }
            case 'Domestic Violence': {
              this.model.barrierTypeId = this.domesticViolenceId;
              this.originalModel.barrierTypeId = this.domesticViolenceId;
              this.initSubBarriersDrop(false);
              break;
            }
            case 'Cognitive and Learning': {
              this.model.barrierTypeId = this.cognitiveLearningId;
              this.originalModel.barrierTypeId = this.cognitiveLearningId;
              this.initSubBarriersDrop(false);
              break;
            }
            default: {
              console.warn('Default Barrier Type Did Not Match');
              break;
            }
          }
        }
      }
    });

    // TODO: Find the Angular way!
    const body = document.getElementsByTagName('body')[0];
    body.classList.add('noscroll');

    // jQuery for sidebar
    $('app-participant-barriers-edit-app .app-content').on('scroll', function() {
      const t = $(this),
        o = $('app-participant-barriers-edit-app aside');
      if (1.2 * o.outerHeight() < t.height()) {
        const e = t.scrollTop(),
          s = e - $('app-participant-barriers-edit-app .nav-header').outerHeight() + 50;
        s > 0 ? o.css('top', s + 'px') : o.css('top', '0');
        // console.log(e);
      } else o.css('top', '0');
    });

    // jQuery for scrolling to sections
    $(document).on('click', '#sidebar li.menu-item', function() {
      const sec = $(this).data('goto'),
        sh = $('#' + sec).position().top;
      $('app-participant-barriers-edit-app .app-content').stop();
      if (sec) {
        $('app-participant-barriers-edit-app .app-content').animate(
          {
            scrollTop: sh
          },
          1000
        );
      }
    });
  }

  initParticipantBarrier(data) {
    this.model = data;
    this.originalModel = new ParticipantBarrier().create();
    ParticipantBarrier.clone(data, this.originalModel);
    this.initSubBarriersDrop();

    if (this.model.isConverted === true) {
      this.isConvertedRecord = true;
    }
  }
  onIntervalTypeDataEmit(payload): void {
    this.intervalTypeDrop = payload.intervalTypeDrop;
    this.intervalTypeDayId = payload.intervalTypeDayId;
    this.intervaTypeWeekId = payload.intervalTypeWeekId;
  }

  // We create sub type lists and main list for our barrier dropdowns.
  initParticipantBarriers(data: DropDownField[]) {
    this.allparticipantBarriersDrop = data;
    this.physicalHealthId = Utilities.idByFieldDataName('Physical Health', data);
    this.mentalHealthId = Utilities.idByFieldDataName('Mental Health', data);
    this.aodaId = Utilities.idByFieldDataName('AODA', data);
    this.domesticViolenceId = Utilities.idByFieldDataName('Domestic Violence', data);
    this.cognitiveLearningId = Utilities.idByFieldDataName('Cognitive and Learning', data);
    for (const d of data) {
      const dd = new DropDownField();
      dd.id = d.id;
      dd.name = d.name;
      this.participantBarriersDrop.push(dd);
    }
  }

  initSubBarriersDrop(isInital?: boolean) {
    if (this.model == null || this.allparticipantBarriersDrop == null) {
      return false;
    }
    this.subBarriersDrop = [];
    if (isInital !== false) {
      for (const d of this.allparticipantBarriersDrop) {
        if (d.id === Number(this.model.barrierTypeId)) {
          for (const dd of d.subTypes) {
            const ddd = new DropDownMultiField();
            ddd.id = dd.id;
            ddd.name = dd.name;
            ddd.disablesOthers = dd.disablesOthers;
            this.subBarriersDrop.push(ddd);
          }
        }
      }
    } else {
      // When barrier type is changed lets clear out selected options.
      for (const d of this.allparticipantBarriersDrop) {
        if (d.id === Number(this.model.barrierTypeId)) {
          for (const dd of d.subTypes) {
            const ddd = new DropDownMultiField();
            ddd.id = dd.id;
            ddd.name = dd.name;
            ddd.disablesOthers = dd.disablesOthers;
            this.subBarriersDrop.push(ddd);
          }
        }
      }
      this.model.barrierSubType = null;
      this.model.barrierSubType = new BarrierSubType();
      this.model.barrierSubType.barrierSubTypes = [];
      this.model.barrierSubType.barrierSubTypeNames = [];
    }
    if (this.subBarriersDrop) {
      this.otherPCId = Utilities.idByFieldDataName('Other Physical Health Condition', this.subBarriersDrop, true);
      this.otherMCId = Utilities.idByFieldDataName('Other Mental Health Limitations', this.subBarriersDrop, true);
    }
  }

  getSubBarrierDrop(barrerID: number) {
    for (const b of this.participantBarriersDrop) {
      if (b.id === barrerID) {
        for (const bb of b.subTypes) {
          this.subBarriersDrop.push(bb);
        }
      }
    }
  }

  sidebarToggle() {
    this.isCollapsed = !this.isCollapsed;
    return this.isCollapsed;
  }

  // making the detals required if the user selected the barrierSubtype as other for Physical and Mental health
  subBarrierHasValue(barrierSubType: number[]): boolean {
    if (barrierSubType) {
      if (barrierSubType.indexOf(this.otherPCId) !== -1 || barrierSubType.indexOf(this.otherMCId) !== -1) {
        return true;
      } else {
        return false;
      }
    } else {
      return false;
    }
  }

  isSubBarrierTypeRequired(barrier: ParticipantBarrier) {
    return barrier.isSubBarrierTypeRequired();
  }

  isSubBarrierTypeDisabled(barrier: ParticipantBarrier) {
    return barrier.isSubBarrierTypeDisabled(this.domesticViolenceId, this.aodaId);
  }

  isFormalAssessmentDisplayed() {
    return this.model.isFormalAssessmentDisplayed(this.domesticViolenceId);
  }

  isAccommodationNeededRequired() {
    return this.model.isAccommodationNeededRequired();
  }

  exitPartBars() {
    if (this.isSectionModified === true) {
      this.appService.isDialogPresent = true;
    } else {
      this.onSaveAndExit.emit();
    }
  }

  saveAndExit() {
    // Clear all previous errors.
    //Comparing model at loadtime and the save time
    this.hasTriedSave = true;
    this.validate();
    if (this.isSectionValid === true) {
      if (this.isConvertedRecord) {
        this.model.isConverted = false;
      }
      this.isSaving = true;
      this.participantBarrierAppService.postParticipantBarrier(this.model).subscribe(
        resp => {
          this.onSaveAndExit.emit();
        },
        error => {
          this.isSaving = false;
          this.hadSaveError = true;
          throw error;
        }
      );
    }
  }

  validate() {
    this.isSectionModified = true;
    if (this.hasTriedSave === true) {
      this.validationManager.resetErrors();

      // Call the model's validate method.
      // tslint:disable-next-line:max-line-length
      let result;
      // if (this.isConvertedRecord) {
      //   result = this.model.validateHistorical(this.validationManager);
      // } else {
      result = this.model.validate(
        this.validationManager,
        this.participantDOB.format('MM/DD/YYYY'),
        this.domesticViolenceId,
        this.aodaId,
        this.otherPCId,
        this.otherMCId,
        this.intervalTypeDrop,
        this.intervalTypeDayId,
        this.intervaTypeWeekId
      );
      // }

      this.isSectionValid = result.isValid;
      this.modelErrors = result.errors;

      if (this.isSectionValid === true) {
        this.hasTriedSave = false;
      }
    }
  }

  exitPartBarsIgnoreChanges(e: Event) {
    this.onSaveAndExit.emit();
  }

  ngOnDestroy() {
    super.onDestroy();
    if (this.pbSub != null) {
      this.pbSub.unsubscribe();
    }
    if (this.pbdSub != null) {
      this.pbdSub.unsubscribe();
    }

    // TODO: Find the Angular way!
    const body = document.getElementsByTagName('body')[0];
    body.classList.remove('noscroll');
  }
}
