import { Component, OnInit, OnDestroy } from '@angular/core';
import { DestroyableComponent } from 'src/app/core/modal/index';
import { ModalBase } from 'src/app/core/modal/modal-base';
import { DropDownField } from '../../shared/models/dropdown-field';
import { AgencyService } from '../../shared/services/agency.service';
import { AgencyWorker } from '../../shared/models/agency-worker';
import { AppService } from './../../core/services/app.service';
import { ParticipantService } from '../../shared/services/participant.service';
import { Participant } from '../../shared/models/participant';
import { AssignedWorker } from '../../shared/models/assigned-worker.model';
import { WhyReason } from '../../shared/models/why-reasons.model';
import { EnrolledProgram } from '../../shared/models/enrolled-program.model';
import { ModelErrors } from '../../shared/interfaces/model-errors';
import { ValidationManager } from '../../shared/models/validation-manager';
import { Status } from '../../shared/models/status.model';
import { Utilities } from '../../shared/utilities';
import { EnrolledProgramCode } from '../../shared/enums/enrolled-program-code.enum';

@Component({
  selector: 'app-enrollment',
  templateUrl: './enrollment.component.html',
  styleUrls: [
    // './../../core/modal/modal-placeholder/modal-error-touched.scss',
    // './../../core/modal/modal-placeholder/modal-placeholder.component.scss',
    './enrollment.component.css'
  ],
  providers: [AgencyService]
})
export class EnrollmentComponent extends ModalBase implements OnInit, DestroyableComponent {
  public workerDrop: DropDownField[] = [];

  public isLoaded = false;
  public isModified = false;
  public pin: string;

  public preCheckError = false;
  public hadSaveError = false;
  public isSaving = false;
  public isSaveAble = false;

  public isPrecheckLoading = false;
  public model: Participant;
  public programDrop: DropDownField[] = [];
  public selectedWorkerId: number;
  public selectedProgram: number;
  public originalSelectedWorkerId: number;
  public selectedProgramNameDisabled = false;
  public selectedProgramCd: string;
  public selectedProgramName: string;

  public savingStatus: Status;
  public enrollmentEligibility: WhyReason;
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public modelErrors: ModelErrors = {}; // Note: this must be initialized to an empty object for the UI in initial state.
  private agencyWorkers: AgencyWorker[];

  constructor(private aService: AgencyService, private appService: AppService, private partService: ParticipantService) {
    super();
  }

  ngOnInit() {
    if (this.appService != null && this.appService.user != null) {
      this.partService.getParticipant(this.pin).subscribe(data => {
        this.initPart(data);
        this.isLoaded = true;
        // Only load Workers if we have set the default program.
        if (this.selectedProgramName != null) {
          this.programChange();
        }
      });
    }
  }

  initPart(data) {
    this.model = data;

    if (this.model.programs == null || this.model.currentEnrolledProgram.assignedWorker == null) {
      this.model.programs = [];
      this.model.currentEnrolledProgram.assignedWorker = new AssignedWorker();
    }

    this.initProgramsDrop();
  }

  initProgramsDrop() {
    this.programDrop = [];
    if (this.model != null && this.model.programs != null) {
      let refPrograms = this.model.getCurrentReferredProgramsByAgency(this.appService.user.agencyCode);
      refPrograms = this.appService.filterProgramsForUserAuthorized<EnrolledProgram>(refPrograms);

      if (refPrograms != null) {
        for (const pro of refPrograms) {
          const x = new DropDownField();
          x.id = pro.programCode;
          x.name = pro.programCode;
          x.code = pro.programCd;
          this.programDrop.push(x);
        }

        if (this.programDrop.length === 1) {
          this.selectedProgramNameDisabled = true;
          this.selectedProgramCd = this.programDrop[0].code;
          this.selectedProgramName = this.programDrop[0].id;
          this.programChange();
        }
      }
    }
  }

  programChange() {
    this.resetPrecheck();
    this.selectedWorkerId = null;
    this.originalSelectedWorkerId = null;

    if (Utilities.isStringEmptyOrNull(this.selectedProgramName)) {
      return;
    }

    this.selectedProgramCd = this.model.programs.find(x => x.programCode === this.selectedProgramName).programCd;
    this.aService.getAgencyWorkersByAgencyCodeAndProgramCode(this.appService.user.agencyCode, this.selectedProgramCd).subscribe(wdata => {
      this.initWorkerDrop(wdata);
      this.setDefaultWorker();
      if (this.selectedProgramCd !== EnrolledProgramCode.fcdp) this.preCheckEnrollment();
    });
  }

  resetPrecheck() {
    this.isSaveAble = false;
    this.enrollmentEligibility = null;
  }

  loadWorkers(programCode: string) {
    this.aService.getAgencyWorkersByAgencyCodeAndProgramCode(this.appService.user.agencyCode, programCode).subscribe(data => this.initWorkerDrop(data));
  }

  setDefaultWorker() {
    // Set work Program office to be that of workers.
    if (this.agencyWorkers == null) {
      return;
    }

    for (const aw of this.agencyWorkers) {
      if (aw.wamsId === this.appService.user.username) {
        this.selectedWorkerId = +aw.id;
        this.originalSelectedWorkerId = +aw.id;
      }
    }
  }

  validate() {
    this.validationManager.resetErrors();
    let precheckStatus = true;
    if (this.enrollmentEligibility == null || this.enrollmentEligibility.status !== true) {
      precheckStatus = false;
    }
    const result = EnrolledProgram.enrollmentValidate(this.selectedWorkerId, this.selectedProgramName, this.validationManager, precheckStatus);
    // Update our properties so the UI can bind to the results.
    this.modelErrors = result.errors;

    this.isSaveAble = result.isValid;
  }

  preCheckEnrollment() {
    this.isPrecheckLoading = true;
    this.preCheckError = false;
    this.partService.canEnrollParticipant(this.pin, this.model.currentReferredProgramByName(this.selectedProgramName).id).subscribe(
      data => {
        this.initPreCheckStatus(data);
        this.isPrecheckLoading = false;
      },
      error => {
        this.isPrecheckLoading = false;
        this.preCheckError = true;
        this.isSaveAble = false;
      }
    );
  }

  initPreCheckStatus(data) {
    this.enrollmentEligibility = data;
    this.validate();
  }

  saveAndExit() {
    if (this.isSaveAble !== true) {
      return;
    }
    this.isSaving = true;
    this.savingStatus = null;
    this.hadSaveError = false;
    const pep = this.model.currentReferredProgramByName(this.selectedProgramName);
    pep.assignedWorker = new AssignedWorker();
    pep.assignedWorker.id = this.selectedWorkerId;
    pep.assignedWorker.workerId = this.selectedWorkerId.toString();
    this.partService.enrollParticipant(pep).subscribe(
      data => {
        this.savingStatus = data;
        this.isSaving = false;
        // Stay on enrollment when errored.
        if (this.savingStatus.errorMessages == null || this.savingStatus.errorMessages.length === 0) {
          this.partService.clearCachedParticipant();
          this.exit();
        }
      },
      error => {
        this.isSaving = false;
        this.hadSaveError = true;
      }
    );
  }

  private initWorkerDrop(data: AgencyWorker[]) {
    this.workerDrop = [];
    if (data != null) {
      this.agencyWorkers = data;
      for (const item of data) {
        const dd = new DropDownField();
        dd.id = item.id;
        dd.name = item.fullNameWithMiddleInitialTitleCase;
        this.workerDrop.push(dd);
      }
    }
  }
}
