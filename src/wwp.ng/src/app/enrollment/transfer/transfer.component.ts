import { EnrolledProgramCode } from './../../shared/enums/enrolled-program-code.enum';
import { Component, OnInit } from '@angular/core';
import { DestroyableComponent } from 'src/app/core/modal/index';
import { ModalBase } from 'src/app/core/modal/modal-base';
import { EnrolledProgram } from '../../shared/models/enrolled-program.model';
import { DropDownField } from '../../shared/models/dropdown-field';
import { AgencyService } from '../../shared/services/agency.service';
import { AgencyWorker } from '../../shared/models/agency-worker';
import { AppService } from './../../core/services/app.service';
import { ParticipantService } from '../../shared/services/participant.service';
import { Participant, ParticipantDetails } from '../../shared/models/participant';
import { OfficeSummary } from '../../shared/models/office-summary.model';
import { OfficeService } from '../../shared/services/office.service';
import { PadPipe } from '../../shared/pipes/pad.pipe';
import { Utilities } from '../../shared/utilities';
import { WhyReason } from '../../shared/models/why-reasons.model';
import { forkJoin } from 'rxjs';
import { take, concatMap } from 'rxjs/operators';

@Component({
  selector: 'app-transfer',
  templateUrl: './transfer.component.html',
  styleUrls: ['./transfer.component.css'],
  providers: [AgencyService, OfficeService]
})
export class TransferComponent extends ModalBase implements OnInit, DestroyableComponent {
  public officeDrop: DropDownField[] = [];
  public programDrop: DropDownField[] = [];
  public workerDrop: DropDownField[] = [];
  public isLoaded = false;

  public isPrecheckLoading = false;
  public isModified = false;
  public pin: string;

  public preCheckError = false;
  public hadSaveError = false;
  public isSaving = false;
  public isSaveAble = false;
  isProgramDropDisabled = false;
  public programId = null;
  public originalProgramId = null;
  public workProgramOfficeId = null;
  public originalWorkProgramOfficeId = null;
  public workerId = null;
  public originalWorkerId = null;
  public pepW2Id = 0;
  public pepTJId = 0;
  public pepLFId = 0;
  public pepCFId = 0;
  public pepTmjId = 0;
  public participantDetails: ParticipantDetails;
  public model: Participant;

  public precheckMsgs: WhyReason;
  public isValid = false;
  public isWorkerRequired = true;

  private officesList: OfficeSummary[] = [];

  constructor(private aService: AgencyService, private officeService: OfficeService, private appService: AppService, private partService: ParticipantService) {
    super();
  }

  ngOnInit() {
    this.partService
      .getParticipant(this.pin)
      .pipe(
        take(1),
        concatMap(part => {
          this.partService.isFromPartSumGuard.next(false);
          this.initPart(part);
          return this.partService.getParticipantSummaryDetails(this.pin).pipe(take(1));
        })
      )
      .subscribe(partD => {
        this.initDetailsParticipant(partD);
        this.initProgramDrop();
        this.isLoaded = true;
      });
  }

  programChange() {
    // Reset all data.
    this.workProgramOfficeId = null;
    this.originalWorkProgramOfficeId = null;
    this.workerId = null;
    this.originalWorkerId = null;
    this.precheckMsgs = null;
    this.officesList = [];
    this.officeDrop = [];
    this.workerDrop = [];
    this.loadOffices();
  }

  loadOffices() {
    const program = this.model.programs.find(x => x.id === this.programId);

    if (program == null) {
      return;
    }
    if (program.officeCounty.trim() === 'MILWAUKEE') {
      this.isWorkerRequired = false;
    }

    this.officeService.getTransferOfficesByProgramWorkerSourceOffice(program.programCd, this.appService.user.mainFrameId, program.officeNumber).subscribe(data => {
      this.initOfficeDestinationsDrop(data);
    });
  }

  loadWorkers($event) {
    if (this.officeDrop == null) {
      return;
    }

    if (this.programId == null || this.programId <= 0) {
      return;
    }

    // Use the agency from the selected office.
    const selectedOrg = this.officesList.find(x => x.id === this.workProgramOfficeId).organizationCode;

    this.aService.getAgencyWorkersByAgencyCodeAndProgramCode(selectedOrg, this.model.programs.find(x => x.id === this.programId).programCd).subscribe(data => {
      this.initWorkerDrop(data);
    });
  }

  initOfficeDestinationsDrop(data: OfficeSummary[]) {
    this.officesList = data;

    // If there is a value for the office transfer ID, then things are out-of-sync.
    if (this.participantDetails != null && this.participantDetails.officeTransferId != null && this.participantDetails.officeTransferId > 0) {
      // We have CWW setting the default office for W-2.
      if (
        this.model.enrolledW2Program != null &&
        +this.model.enrolledW2Program.id === +this.programId &&
        this.model.enrolledW2Program.officeNumber !== this.participantDetails.officeTransferNumber // We need to ignore when the W2 Office Number matches the office transfer #
      ) {
        this.workProgramOfficeId = this.participantDetails.officeTransferId;
        this.originalWorkProgramOfficeId = this.participantDetails.officeTransferId;
      }

      if (this.workProgramOfficeId > 0) {
        this.preCheck();
        this.loadWorkers(this.workProgramOfficeId);
      }
    }

    if (data != null) {
      for (const o of data) {
        const dd = new DropDownField();
        dd.id = o.id;
        const officePadded = new PadPipe().transform(o.officeNumber, 4);
        dd.name = o.countyName + ' - ' + officePadded.toString();
        this.officeDrop.push(dd);
      }
    }
  }

  private initWorkerDrop(data: AgencyWorker[]) {
    this.workerDrop = [];
    if (data != null) {
      for (const item of data) {
        const dd = new DropDownField();
        dd.id = item.id;
        dd.name = item.fullNameWithMiddleInitialTitleCase;
        dd.code = item.workerId;
        this.workerDrop.push(dd);
      }
    }
    this.assignFep();
  }

  initProgramDrop() {
    let eps = this.model.getTransferableProgramsByAgency(this.appService.user.agencyCode);
    eps = this.appService.filterProgramsForUserAuthorized<EnrolledProgram>(eps);

    if (eps != null) {
      for (const ep of eps) {
        const dd = new DropDownField();
        dd.id = ep.id;
        dd.name = ep.programCode;
        dd.code = ep.programCd;
        this.programDrop.push(dd);
      }
    }

    this.pepW2Id = Utilities.idByFieldDataName(EnrolledProgramCode.ww, Utilities.lowerCaseArrayTrimAsNotNull(this.programDrop, 'code'), true, 'code');
    this.pepTJId = Utilities.idByFieldDataName(EnrolledProgramCode.tj, Utilities.lowerCaseArrayTrimAsNotNull(this.programDrop, 'code'), true, 'code');
    this.pepLFId = Utilities.idByFieldDataName(EnrolledProgramCode.lf, Utilities.lowerCaseArrayTrimAsNotNull(this.programDrop, 'code'), true, 'code');
    this.pepCFId = Utilities.idByFieldDataName(EnrolledProgramCode.cf, Utilities.lowerCaseArrayTrimAsNotNull(this.programDrop, 'code'), true, 'code');
    this.pepTmjId = Utilities.idByFieldDataName(EnrolledProgramCode.tmj, Utilities.lowerCaseArrayTrimAsNotNull(this.programDrop, 'code'), true, 'code');
    // If only one program we set the default on the disabled dropdown.
    if (this.programDrop != null && this.programDrop.length === 1) {
      this.programId = this.programDrop[0].id;
      this.isProgramDropDisabled = true;
      // set originalProgramId.
      this.originalProgramId = this.programId;
      this.loadOffices();
    }
  }

  private initPart(data) {
    this.model = data;
  }

  private initDetailsParticipant(partDetails: ParticipantDetails) {
    this.participantDetails = partDetails;
  }

  assignFep() {
    if (this.participantDetails) {
      if (this.participantDetails.cwwTransferDetails.fepOutOfSync && this.participantDetails.cwwTransferDetails.newFepId) {
        const cwwAssignedFep = this.workerDrop.find(x => x.code === this.participantDetails.cwwTransferDetails.newFepId);
        if (cwwAssignedFep) {
          this.workerId = cwwAssignedFep.id;
        }
      } else if (this.participantDetails.mostRecentFEPFromDB2_Result.mostRecentMFFepId) {
        const mostRecentAssignedFep = this.workerDrop.find(x => x.code === this.participantDetails.mostRecentFEPFromDB2_Result.mostRecentMFFepId);
        if (mostRecentAssignedFep) {
          this.workerId = mostRecentAssignedFep.id;
        }
      }
    }
  }

  saveAndExit() {
    if (this.isValid !== true) {
      return;
    }
    this.isSaving = true;

    // Post cleansed Model.
    const model = this.cleansedModel();

    this.model.programById(this.programId).officeId = this.workProgramOfficeId;
    this.model.programById(this.programId).assignedWorker.id = this.workerId;

    this.partService.transferParticipant(this.pin, model).subscribe(
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

  /**
   * Cleanse before we send model to api.
   *
   * @private
   * @memberof TransferComponent
   */
  private cleansedModel(): EnrolledProgram {
    // Create Post Model.
    const model = this.model.programById(this.programId);
    const postModel = new EnrolledProgram();
    EnrolledProgram.clone(model, postModel);

    // Set Model for precheck.
    postModel.officeId = this.workProgramOfficeId;
    postModel.assignedWorker.id = this.workerId;

    const selectedOffice = this.officesList.find(x => +x.id === +this.workProgramOfficeId);

    if (selectedOffice != null) {
      postModel.officeNumber = selectedOffice.officeNumber;
    } else {
      console.warn('Office not found');
    }

    return postModel;
  }

  preCheck() {
    this.precheckMsgs = null;
    this.isPrecheckLoading = true;
    this.preCheckError = false;

    // Post cleansed Model.
    const model = this.cleansedModel();

    this.partService.canTransferParticipant(this.pin, model).subscribe(
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
    this.precheckMsgs = data;
    this.validate();
  }

  validate() {
    if (this.workProgramOfficeId != null && this.workProgramOfficeId > 0 && this.precheckMsgs != null && this.precheckMsgs.status === true) {
      if (this.isWorkerRequired && (this.workerId == null || +this.workerId === 0)) {
        this.isValid = false;
      } else {
        this.isValid = true;
      }
    } else {
      this.isValid = false;
    }
  }
}
