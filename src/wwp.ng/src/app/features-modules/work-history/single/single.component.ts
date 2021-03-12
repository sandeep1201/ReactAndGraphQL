import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import * as moment from 'moment';

import { BaseParticipantComponent } from '../../../shared/components/base-participant-component';
import { Employment } from '../../../shared/models/work-history-app';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { ParticipantService } from '../../../shared/services/participant.service';
import { WorkHistoryAppService } from '../../../shared/services/work-history-app.service';
import { Utilities } from '../../../shared/utilities';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { JobTypeName } from '../../../shared/enums/job-type-name.enum';

@Component({
  selector: 'app-work-history-single',
  templateUrl: './single.component.html',
  styleUrls: ['./single.component.css'],
  providers: [FieldDataService]
})
export class WorkHistorySingleComponent extends BaseParticipantComponent implements OnInit {
  @Input()
  hasEditAccess = false;
  private epSub: Subscription;
  public model: Employment;
  private routeIdSub: Subscription;
  private employmentId: number;
  public employerOfRecordSelectedValue: string;
  public employerOfRecordTypes: DropDownField[];
  public workPrograms: DropDownField[];
  public selectedWorkProgram: string;
  public goBackUrl: string;

  constructor(
    private singleRoute: ActivatedRoute,
    private workHistoryAppService: WorkHistoryAppService,
    private fdService: FieldDataService,
    router: Router,
    partService: ParticipantService
  ) {
    super(singleRoute, router, partService);
  }

  ngOnInit() {
    super.onInit();
    this.fdService.getEmployerOfRecordTypes().subscribe(data => (this.employerOfRecordTypes = data));
    this.fdService.getWorkPrograms().subscribe(data => (this.workPrograms = data));

    this.routeIdSub = this.singleRoute.params.subscribe(params => {
      if (this.pin == null || this.pin.trim() === '') {
        console.warn('PIN on WorkHistorySingleComponent is null or empty');
      }

      this.employmentId = params['id'];
      this.goBackUrl = '/pin/' + this.pin + '/work-history';

      this.workHistoryAppService.setPin(this.pin);
      this.epSub = this.workHistoryAppService.getEmployment(this.employmentId).subscribe(data => this.initEmployment(data));
    });
  }

  initEmployment(data: Employment): void {
    this.model = data;
    if (this.model != null) {
      this.calculateJobDuration();
      if (this.model.employerOfRecordId) {
        const otherId = Utilities.idByFieldDataName('Other', this.employerOfRecordTypes);
        const workSiteId = Utilities.idByFieldDataName('Work Site', this.employerOfRecordTypes);
        const contractorId = Utilities.idByFieldDataName('Contractor', this.employerOfRecordTypes);
        switch (+this.model.employerOfRecordId) {
          case otherId:
            this.employerOfRecordSelectedValue = 'Other';
            break;
          case workSiteId:
            this.employerOfRecordSelectedValue = 'Work Site';
            break;
          case contractorId:
            this.employerOfRecordSelectedValue = 'Contractor';
            break;
          default:
            this.employerOfRecordSelectedValue = '';
            break;
        }
      }
      if (this.model.workProgramId) {
        this.selectedWorkProgram = Utilities.fieldDataNameById(this.model.workProgramId, this.workPrograms);
      }
    }
  }

  calculateHourlyWage(e: Employment) {
    if (e.wageHour != null) {
      return e.wageHour.calculateHourlyWage();
    }
  }
  canShowProgramTypeName(jobTypeName) {
    if (
      jobTypeName === JobTypeName.tjSubsidised ||
      jobTypeName === JobTypeName.tmjSubsidised ||
      jobTypeName === JobTypeName.tjUnsubsidised ||
      jobTypeName === JobTypeName.tmjUnsubsidised
    ) {
      return false;
    } else {
      return true;
    }
  }

  calculateJobDuration() {
    if (this.model.jobEndDate != null && this.model.jobBeginDate != null) {
      if (this.model.jobEndDate != null && this.model.jobBeginDate != null) {
        this.model.jobDateDuration = Utilities.getDurationBetweenDates(this.model.jobBeginDate, this.model.jobEndDate);
      }
    } else if (this.model.jobBeginDate != null && this.model.isCurrentlyEmployed === true) {
      this.model.jobDateDuration = Utilities.getDurationBetweenDates(this.model.jobBeginDate, Utilities.currentDate.format('MM/DD/YYYY'));
    }
  }

  goBackToList() {
    this.router.navigateByUrl(`/pin/${this.pin}/work-history`);
  }

  toggledHistory($event) {
    this.toggleHistory($event, 'work-history-app', this.model, this.employmentId, cs => {
      this.model = cs;
    });
  }

  loadHistorySection($event) {
    if (this.isHistoryActive === true) {
      this.model = this.appHistoryManager.getHistoryAtIndex($event);
      this.calculateJobDuration();
    }
  }
}
