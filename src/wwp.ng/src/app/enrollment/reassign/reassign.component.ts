import { Component, OnInit } from '@angular/core';
import { DestroyableComponent } from 'src/app/core/modal/index';
import { ModalBase } from 'src/app/core/modal/modal-base';
import { DropDownField } from '../../shared/models/dropdown-field';
import { AgencyService } from '../../shared/services/agency.service';
import { AgencyWorker } from '../../shared/models/agency-worker';
import { AppService } from './../../core/services/app.service';
import { ParticipantService } from '../../shared/services/participant.service';
import { Participant } from '../../shared/models/participant';
import { EnrolledProgram } from '../../shared/models/enrolled-program.model';
import { Utilities } from '../../shared/utilities';
import { EnrolledProgramCode } from 'src/app/shared/enums/enrolled-program-code.enum';
import { Observable, of } from 'rxjs';
import { WorkerTaskService } from 'src/app/shared/components/worker-task/worker-task.service';

@Component({
  selector: 'app-reassign',
  templateUrl: './reassign.component.html',
  styleUrls: ['./reassign.component.css'],
  providers: [AgencyService]
})
export class ReassignComponent extends ModalBase implements OnInit, DestroyableComponent {
  public workerDrop: DropDownField[] = [];
  public isLoaded = false;
  public pin: string;
  public hadSaveError = false;
  public isSaving = false;
  public isSaveAble = false;
  public model: Participant;
  public programDrop: DropDownField[] = [];
  public selectedWorkerId: number;
  public selectedProgram: EnrolledProgram;
  public selectedProgramNameDisabled = false;
  public selectedProgramName: string;
  public originalSelectedProgramName: string;
  public workerId = null;
  public workerTaskId: number;
  public workerTaskWkrId: number;
  public originalWorkerId = null;
  public pepW2Id = 0;
  public pepTJId = 0;
  public pepLFId = 0;
  public pepCFId = 0;
  public pepTmjId = 0;
  public pepFcdpId = 0;
  public isProgramDropDisabled = false;
  public programId = null;
  public originalProgramId = null;
  public isWorkerTask = false;

  constructor(private agencyService: AgencyService, private appService: AppService, private partService: ParticipantService, private workerTaskService: WorkerTaskService) {
    super();
  }

  ngOnInit() {
    if (!this.isWorkerTask)
      this.partService.getParticipant(this.pin).subscribe(part => {
        this.initPart(part);
      });
    else this.isLoaded = true;
  }

  initPart(data: Participant) {
    this.model = data;

    if (this.model.programs == null) {
      this.model.programs = [];
    }

    this.initProgramDrop();
    this.isLoaded = true;
  }

  saveAndExit() {
    this.isSaving = true;
    this.hadSaveError = false;
    let request: Observable<any>;

    if (this.isWorkerTask) request = this.workerTaskWkrId === this.workerId ? of(null) : this.workerTaskService.reassignWorker(this.workerTaskId, this.workerId);
    else {
      // Find the program that is selected.
      const program = this.model.programs.find(x => x.id === this.programId);
      request = this.partService.reassignParticipant(program);
      program.assignedWorker.id = this.workerId;
    }

    // Update the assigned worker to the selected one
    request.subscribe(
      () => {
        if (!this.isWorkerTask) this.partService.clearCachedParticipant();
        this.exit();
      },
      () => {
        this.isSaving = false;
        this.hadSaveError = true;
      }
    );
  }

  initProgramDrop() {
    this.programDrop = [];

    let eps = this.model.getCurrentEnrolledProgramsByAgency(this.appService.user.agencyCode);
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
    this.pepFcdpId = Utilities.idByFieldDataName(EnrolledProgramCode.fcdp, Utilities.lowerCaseArrayTrimAsNotNull(this.programDrop, 'code'), true, 'code');

    // If only one program we set the default on the disabled dropdown.
    if (this.programDrop != null && this.programDrop.length === 1) {
      this.programId = this.programDrop[0].id;
      this.isProgramDropDisabled = true;
      this.originalProgramId = this.programId;
      this.programChange();
    }
  }

  private initWorkerDrop(data: AgencyWorker[]) {
    this.workerDrop = [];

    if (data != null && data.length > 0) {
      // sort
      for (const item of data) {
        const dd = new DropDownField();
        dd.id = item.id;
        dd.name = item.fullNameWithMiddleInitialTitleCase;
        this.workerDrop.push(dd);
      }
    }
  }

  loadWorkers($event) {
    if (this.programId == null || this.programId === 0) {
      return;
    }

    const program = this.model.programs.find(x => x.id === this.programId);

    this.agencyService.getAgencyWorkersByAgencyCodeAndProgramCode(program.agencyCode, program.programCd).subscribe(data => {
      this.initWorkerDrop(data);
    });
  }

  programChange() {
    // Reset all data.
    this.isSaveAble = false;
    this.workerId = null;
    this.originalWorkerId = null;
    this.workerDrop = [];

    if (this.programId > 0) {
      this.loadWorkers(this.programId);
    }
  }

  workerChange($event) {
    this.workerId = $event;

    if (this.workerId == null) {
      this.isSaveAble = false;
    } else if ((this.programId == null || this.programId === 0) && !this.isWorkerTask) {
      this.isSaveAble = false;
    } else {
      this.isSaveAble = true;
    }
  }
}
