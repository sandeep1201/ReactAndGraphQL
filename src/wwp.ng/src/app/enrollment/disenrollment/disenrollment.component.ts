import { Component, OnInit, OnDestroy } from '@angular/core';
import { DestroyableComponent } from 'src/app/core/modal/index';
import { DropDownField } from '../../shared/models/dropdown-field';
import { AppService } from './../../core/services/app.service';
import { ParticipantService } from '../../shared/services/participant.service';
import { FieldDataService } from '../../shared/services/field-data.service';
import { Participant } from '../../shared/models/participant';
import { WhyReason } from '../../shared/models/why-reasons.model';
import { EnrolledProgram } from '../../shared/models/enrolled-program.model';
import { Utilities } from '../../shared/utilities';
import { ModelErrors } from '../../shared/interfaces/model-errors';
import { ValidationManager } from '../../shared/models/validation-manager';
import { ModalBase } from 'src/app/core/modal/modal-base';

@Component({
  selector: 'app-disenrollment',
  templateUrl: './disenrollment.component.html',
  styleUrls: ['./disenrollment.component.css'],
  providers: [FieldDataService]
})
export class DisenrollmentComponent extends ModalBase implements OnInit, DestroyableComponent {
  public title = 'Disenroll';
  public isLoaded = false;
  public isModified = false;
  public pin: string;
  public preCheckError = false;
  public hadSaveError = false;
  public isSaving = false;
  public isSaveAble = false;
  private hasTriedSaving = false;
  public isPrecheckLoading = false;
  public participant: Participant;
  public disenrollmentEligibility: WhyReason;
  public programDrop: DropDownField[] = [];
  public completionReasonsDrop: DropDownField[] = [];
  public selectedProgramName: string;
  public originalSelectedProgramName: string;
  public model: EnrolledProgram;
  public originalModel: EnrolledProgram;
  public selectedProgramNameDisabled = false;

  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public modelErrors: ModelErrors = {}; // Note: this must be initialized to an empty object for the UI in initial state.

  constructor(private appService: AppService, private partService: ParticipantService, private fieldDataService: FieldDataService) {
    super();
  }

  ngOnInit() {
    this.partService.getParticipant(this.pin).subscribe(data => {
      this.initPart(data);
      this.initProgramsDrop();
      this.isLoaded = true;
    });
    this.isSaveAble = true;
  }

  initProgramsDrop() {
    this.programDrop = [];
    if (this.participant != null && this.participant.programs != null) {
      let refPrograms = this.participant.getCurrentEnrolledProgramsByAgency(this.appService.user.agencyCode);
      refPrograms = this.appService.filterProgramsForUserAuthorized(refPrograms);

      if (refPrograms != null) {
        for (const pro of refPrograms) {
          const x = new DropDownField();
          x.id = pro.programCode;
          x.name = pro.programCode;
          this.programDrop.push(x);
        }
      }

      if (this.programDrop.length === 1) {
        this.selectedProgramNameDisabled = true;
        this.selectedProgramName = this.programDrop[0].id;
        this.programChange();
      }
    }
  }

  initEnrollStatus(data: WhyReason) {
    this.disenrollmentEligibility = data;
    this.validate();
  }

  initPart(data: Participant) {
    this.participant = data;
  }

  validate() {
    let precheckStatus = true;
    if (this.disenrollmentEligibility == null || this.disenrollmentEligibility.status !== true) {
      precheckStatus = false;
      this.isSaveAble = false;
      return;
    }

    if (this.hasTriedSaving !== true) {
      return;
    }

    this.validationManager.resetErrors();
    const result = EnrolledProgram.disenrollmentValidate(this.model, this.selectedProgramName, this.validationManager, this.disenrollmentEligibility);
    // Update our properties so the UI can bind to the results.
    this.modelErrors = result.errors;

    this.isSaveAble = result.isValid;
  }

  programChange() {
    this.resetForm();

    if (Utilities.isStringEmptyOrNull(this.selectedProgramName)) {
      return;
    }

    this.model = this.getWorkingPep();
    if (this.model.isCFTmjTJFcdpProgram) {
      this.loadCompletionReasons();
    }
    this.preDisenrollCheck();
  }

  preDisenrollCheck() {
    this.isPrecheckLoading = true;
    this.preCheckError = false;
    this.partService.canDisenrollParticipant(this.pin, this.participant.currentEnrolledProgramByName(this.selectedProgramName).id).subscribe(
      data => {
        this.initEnrollStatus(data);
        this.isPrecheckLoading = false;
      },
      error => {
        this.isPrecheckLoading = false;
        this.preCheckError = true;
        this.isSaveAble = false;
      }
    );
  }

  saveAndExit() {
    this.hasTriedSaving = true;
    this.validate();

    // check precheck status and error msg.
    if (this.isSaveAble !== true) {
      return;
    }

    this.isSaving = true;

    this.hadSaveError = false;
    this.partService.disenrollParticipant(this.participant.currentEnrolledProgramByName(this.selectedProgramName)).subscribe(
      data => {
        this.partService.clearCachedParticipant();
        this.exit();
      },
      error => {
        this.isSaving = false;
        this.hadSaveError = true;
      }
    );
  }

  resetForm() {
    this.model = null;
    this.isSaveAble = true;
    this.disenrollmentEligibility = null;
  }

  private getWorkingPep(): EnrolledProgram {
    return this.participant.currentEnrolledProgramByName(this.selectedProgramName);
  }

  private loadCompletionReasons() {
    this.fieldDataService.getCompletionReasons(this.model.programCd).subscribe(data => this.initCompletion(data));
  }

  private initCompletion(data) {
    this.completionReasonsDrop = data;
    this.model.otherCompletionReasonId = Utilities.idByFieldDataName('Unsuccessful participation: Other', data);
  }
}
