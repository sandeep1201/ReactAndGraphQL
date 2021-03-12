import { Component, OnInit, Input } from '@angular/core';
import { RFAProgram, OldRfaProgram } from '../../../shared/models/rfa.model';
import { RfaService } from '../../../shared/services/rfa.service';
import { Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { Utilities } from '../../../shared/utilities';
import * as _ from 'lodash';
import { AccessType } from '../../../shared/enums/access-type.enum';
import { AppService } from './../../../core/services/app.service';
import { Authorization } from '../../../shared/models/authorization';
import { Roles } from '../../../shared/enums/user-roles.enum';

@Component({
  selector: 'app-rfa-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css'],
  providers: [RfaService]
})
export class RfaListComponent implements OnInit {
  @Input() pin;

  @Input() isReadOnly = false;
  @Input() accessType: AccessType;
  AccessType = AccessType;

  public isInEdit = false;

  public programList: RFAProgram[] = [];
  public oldProgramList: OldRfaProgram[] = [];

  public allProgramsList: Array<RFAProgram | OldRfaProgram> = [];

  public isSortByStatusToggled = true;

  public editId = 0;

  public isLoaded = false;
  public showOldRfas = true;
  public showCaresRfaButton = true;
  constructor(public rfaService: RfaService, private router: Router, private appService: AppService) {}

  ngOnInit() {
    this.allProgramsList = [];
    this.loadRfaList();
  }

  public onSearch() {}

  public sortByStatus() {
    this.allProgramsList.reverse();
    this.isSortByStatusToggled = !this.isSortByStatusToggled;
  }

  displayEdit(value: boolean, id = 0) {
    this.isInEdit = value;
    this.editId = id;
    // Reload list when we close the edit view.
    if (!value) {
      this.ngOnInit();
    }
  }

  goToSingleView(id: number): void {
    if (this.isInEdit) {
      return;
    }
    this.router.navigateByUrl(`/pin/${this.pin}/rfa/${id}`);
  }

  private loadRfaList() {
    this.isLoaded = false;
    forkJoin(this.rfaService.getRfasByPin(this.pin), this.rfaService.getOldRfasByPin(this.pin))
      .pipe()
      .subscribe(results => {
        this.programList = this.sortRfaList(results[0]);
        if (this.allProgramsList) {
          for (const rfa of this.programList) {
            this.allProgramsList.push(rfa);
          }
        }
        this.setAccessType();
        this.isLoaded = true;

        this.oldProgramList = results[1];
      });
  }
  private getOldRfas() {
    this.showOldRfas = !this.showOldRfas;
    if (this.allProgramsList) {
      for (const rfa of this.oldProgramList) {
        if (rfa) {
          this.allProgramsList.push(rfa);
          this.allProgramsList = _.sortBy(this.allProgramsList, [
            function(o) {
              return o.isOldRfa !== true;
            }
          ]);
        }
      }
    }
  }
  toggleOldRfas() {
    this.showOldRfas = !this.showOldRfas;
    this.allProgramsList = _.remove(this.allProgramsList, function(o) {
      return o.isOldRfa !== true;
    });
  }
  private setAccessType() {
    for (const program of this.programList) {
      if (this.appService.user.agencyCode === program.contractorCode && this.appService.isUserAuthorized(Authorization[`canAccessProgram_${program.programCode.trim()}`])) {
        program.canEditRfa = true;
      }
    }

    if (this.appService.user.roles && !this.appService.user.roles.includes(',') && this.appService.user.roles.toLowerCase() === Roles.fcdp) {
      this.showCaresRfaButton = false;
    }
  }
  private sortRfaList(rfaPrograms: RFAProgram[]): RFAProgram[] {
    const sortOrder = { 'In Progress': 1, Referred: 2, Enrolled: 3, Disenrolled: 4, 'RFA Denied': 5, 'RFA Denied – System': 6 };

    const sorted = _.sortBy<RFAProgram>(rfaPrograms, [
      function(o) {
        return sortOrder[o.statusName];
      },
      function(o) {
        if (o.applicationDate != null && o.applicationDate.trim() !== '') {
          return new Date(o.applicationDate);
        }
      }
    ]);

    return sorted;
  }

  isStringEmptyOrNull(str) {
    return Utilities.isStringEmptyOrNull(str);
  }
}
