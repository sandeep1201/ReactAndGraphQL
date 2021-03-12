import { WorkerService } from './../../../shared/services/worker.service';
import { FieldDataService } from './../../../shared/services/field-data.service';
import { Utilities } from './../../../shared/utilities';
import { Component, OnInit } from '@angular/core';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { take } from 'rxjs/operators';
import { AgencyWorker } from 'src/app/shared/models/agency-worker';
import { AppService } from 'src/app/core/services/app.service';
import { forkJoin, of } from 'rxjs';
import { FieldDataTypes } from 'src/app/shared/enums/field-data-types.enum';
import { EnrolledProgramCode } from 'src/app/shared/enums/enrolled-program-code.enum';

@Component({
  selector: 'app-w2-auxiliary-approvers',
  templateUrl: './w2-auxiliary-approvers.component.html',
  styleUrls: ['./w2-auxiliary-approvers.component.scss']
})
export class W2AuxiliaryApproversComponent implements OnInit {
  public agencyMap = new Map<number, string>();
  public isLoaded = false;
  agenciesDrop: DropDownField[] = [];
  agencyWorkers: AgencyWorker[] = [];
  selectedAgency: number;
  isWorkersLoaded = false;
  isAgencyDropAllowed = this.appService.isStateStaff;

  constructor(private workerService: WorkerService, private appService: AppService, private fdService: FieldDataService) {}

  ngOnInit() {
    this.initAgencyAndWorker(this.appService.user.agencyCode, true);
  }

  initAgencyAndWorker(selectedAgencyCode: string, isFromOnInit = false) {
    forkJoin(
      this.isAgencyDropAllowed && isFromOnInit ? this.fdService.getFieldDataByField(FieldDataTypes.ProgramOrganizations, EnrolledProgramCode.w2Id).pipe(take(1)) : of(null),
      this.workerService.getWorkersByOrgAndRole(selectedAgencyCode, 'W2AAPR').pipe(take(1))
    ).subscribe(res => {
      if (res[0]) {
        this.agenciesDrop = res[0];
        this.selectedAgency = +Utilities.fieldDataIdByCode(this.appService.user.agencyCode, this.agenciesDrop);
      }

      this.agencyWorkers = res[1].filter(i => i.isActive);
      this.isWorkersLoaded = true;
      this.isLoaded = true;
    });
  }

  selectAgency($event) {
    this.isWorkersLoaded = false;
    this.initAgencyAndWorker(Utilities.fieldDataCodeById($event, this.agenciesDrop));
  }
}
