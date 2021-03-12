import { Absence } from './../../../shared/models/work-history-app';
import { Component, OnInit, OnDestroy, Input, ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { PaginationInstance } from 'ng2-pagination';
import { Subscription } from 'rxjs';

import { DropDownField } from '../../../shared/models/dropdown-field';
import { Employment } from '../../../shared/models/work-history-app';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { ParticipantService } from '../../../shared/services/participant.service';
import { Utilities } from '../../../shared/utilities';
import { WorkHistoryAppService } from '../../../shared/services/work-history-app.service';
import { WhyReason } from '../../../shared/models/why-reasons.model';
import { AppService } from './../../../core/services/app.service';
import { JobTypeName } from '../../../shared/enums/job-type-name.enum';
import { Participant } from '../../../shared/models/participant';
import * as moment from 'moment';
import { EnrolledProgramStatus } from 'src/app/shared/enums/enrolled-program-status.enum';

@Component({
  selector: 'app-work-history-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class WorkHistoryListComponent implements OnInit, OnDestroy {
  // tslint:disable: no-shadowed-variable
  public config: PaginationInstance = {
    id: 'custom',
    itemsPerPage: 5,
    currentPage: 1
  };

  @Input() isReadOnly = true;
  @Input() pin: string;
  @Input() isHD = false;
  @Input() hasEditAuth = false;

  private eListSub: Subscription;
  private delSub: Subscription;
  public isPrecheckLoading = false;
  public preCheckError = false;
  public deleteEligibility: WhyReason;

  private ptSub: Subscription;
  // Server sends data sorted by date.
  public isLoaded = false;
  private isSortByDateToggled = false;
  public isShowAllToggled = false;
  public isShowDelToggled = false;
  public isNonDeletedOnlyShown = true;
  private isSortByTypeNotToggled = true;
  public searchQuery = '';
  public employments: Employment[];
  public allEmployments: Employment[];
  public inConfirmDeleteView = false;
  private deleteSelectedId: number;
  private deleteReasonId: number;

  public hoursPerWeek = ' hr/week';
  // private perHour: string = '/hr';
  public inEditView = false;
  public employmentRecordId = 0;
  private noPayId: number;
  public employerOfRecordTypes: DropDownField[];
  public showError = false;

  public precheck: WhyReason = new WhyReason();
  public errorMsg: string;
  public hasEmploymentOnEp = false;
  public participant: Participant;
  public canViewPrintWHButton = false;
  public showFcdpFeature = false;
  public fcdpFeatureToggleDate: string;
  public openPrintWhWindow = false;
  public showPrintWHFeature = false;
  public hasOnlyFcdp = false;
  public totalLifeTimeTMJTJSubsidizedHours = 0;
  public maximumLifeTimeSubsidizedHoursAllowed = Employment.maximumLifeTimeSubsidizedHoursAllowed;

  constructor(
    private appService: AppService,
    private workHistoryAppService: WorkHistoryAppService,
    private fdService: FieldDataService,
    private partService: ParticipantService,
    private router: Router,
    private cdRef: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    if (this.pin == null) {
      console.warn('PIN is not set');
    }
    this.showPrintWHFeature = this.appService.getFeatureToggleDate('PrintWH');
    this.workHistoryAppService.setPin(this.pin);
    this.fdService.getEmployerOfRecordTypes().subscribe(data => (this.employerOfRecordTypes = data));
    this.getEmploymentsList();
    this.ptSub = this.fdService.getWageTypes().subscribe(data => this.initPayTypes(data));
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
  canAddAndShowTMJTJSubsidizedHours(employment) {
    // checkReadOnlyAccess will check for enrolled program and also the most recent enrolled program user has access to.
    return Employment.canAddAndShowTMJTJSubsidizedHours(employment, this.participant, this.appService, this.hasEditAuth);
  }

  goToHourlyEntry(e: Employment) {
    this.router.navigate([`/pin/${this.pin}/work-history/${e.id}/hourly-entry`], {
      state: {
        employment: e,
        pin: this.pin,
        participant: this.participant,
        totalLifeTimeTMJTJSubsidizedHours: this.totalLifeTimeTMJTJSubsidizedHours,
        isStateStaff: this.appService.isStateStaff
      }
    });
  }
  hasDeletedEmployment(employments: Employment[]): boolean {
    let ret = false;
    if (employments) {
      for (let i = 0; i < employments.length; i++) {
        if (employments[i].deleteReasonId != null) {
          ret = true;
          break;
        }
      }
    }
    return ret;
  }

  initSection(data: Employment[]): Employment[] {
    const model = data;
    this.allEmployments = data;

    if (model != null) {
      for (const emp of model) {
        if (emp.jobEndDate != null && emp.jobBeginDate != null) {
          emp.jobDateDuration = Utilities.getDurationBetweenDates(emp.jobBeginDate, emp.jobEndDate);
        } else if (emp.jobBeginDate != null && emp.isCurrentlyEmployed === true) {
          emp.jobDateDuration = Utilities.getDurationBetweenDates(emp.jobBeginDate, Utilities.currentDate.format('MM/DD/YYYY'));
        }
      }
      this.partService.getCurrentParticipant().subscribe(result => {
        this.participant = result;
        const programCds = [];
        this.participant.programs.forEach(i => {
          if (i.status !== 'Referred') programCds.push(i.programCd);
        });
        this.hasOnlyFcdp = this.appService.checkForFCDP(programCds);
        this.canViewPrintWHButton = this.appService.isUserAuthorizedToPrintWorkHistory(this.participant);
        this.calculateTotalLifeTImeTjTmjSubsidizedHours(this.allEmployments);
        this.isLoaded = true;
      });
      this.sortByDate();
      // Data has loaded.
      this.isShowDelToggled = this.hasDeletedEmployment(this.allEmployments);
      this.filter();
    } else {
      this.isLoaded = true;
    }

    return model;
  }

  filterOnToggleIsDeletedRecords() {
    this.isNonDeletedOnlyShown = !this.isNonDeletedOnlyShown;
    this.filter();
  }

  initPayTypes(data) {
    this.noPayId = Utilities.idByFieldDataName('No Pay', data);
  }

  getEmploymentsList(id?: number): void {
    this.eListSub = this.workHistoryAppService.getEmploymentList().subscribe(employments => {
      if (employments != null) {
        this.employments = this.initSection(employments);
        this.allEmployments = this.initSection(employments);
        this.setEmployerOfRecordSelectedValue(this.employments);
        this.updatePagination();

        if (id != null) {
          this.employments.find(i => i.id === this.deleteSelectedId).showDel = false;
        }
      }
    });
  }

  calculateHourlyWage(e: Employment) {
    if (e.wageHour != null) {
      // console.log(e.wageHour.calculateHourlyWage());
      return e.wageHour.calculateHourlyWage();
    }
  }
  calculateTotalLifeTImeTjTmjSubsidizedHours(allEmployments: Employment[]) {
    this.totalLifeTimeTMJTJSubsidizedHours = 0;
    for (const emp of allEmployments) {
      // calculating total life time tmj tj subsidized hours
      if (this.canAddAndShowTMJTJSubsidizedHours(emp)) {
        this.totalLifeTimeTMJTJSubsidizedHours += emp.totalSubsidizedHours;
      }
    }
  }

  printWorkHistory() {
    this.openPrintWhWindow = !this.openPrintWhWindow;
    this.cdRef.detectChanges();
  }

  setEmployerOfRecordSelectedValue(e) {
    e.forEach((employment, index) => {
      if (employment.employerOfRecordId) {
        const otherId = Utilities.idByFieldDataName('Other', this.employerOfRecordTypes);
        const workSiteId = Utilities.idByFieldDataName('Work Site', this.employerOfRecordTypes);
        const contractorId = Utilities.idByFieldDataName('Contractor', this.employerOfRecordTypes);
        switch (+e[index].employerOfRecordId) {
          case otherId:
            e[index].employerOfRecordSelectedValue = 'Other';
            break;
          case workSiteId:
            e[index].employerOfRecordSelectedValue = 'Work Site';
            break;
          case contractorId:
            e[index].employerOfRecordSelectedValue = 'Contractor';
            break;
          default:
            e[index].employerOfRecordSelectedValue = '';
            break;
        }
      }
      if (employment.JobTypeName === JobTypeName.tjSubsidised || employment.JobTypeName === JobTypeName.tmjSubsidised) {
        //calculate subsidizedHours.
      }
    });
  }
  isWageDisplayed(e: Employment): boolean {
    if (e == null || e.wageHour == null || e.wageHour.currentPayType == null) {
      return false;
    }

    if (e.jobTypeName !== 'Volunteer' || e.wageHour.currentPayType.jobActionTypes.indexOf(this.noPayId) > -1) {
      return true;
    }

    return false;
  }

  addWorkHistory(): void {
    this.employmentRecordId = 0;
    this.inEditView = true;
  }

  editWorkHistory(id: number): void {
    this.employmentRecordId = id;
    this.inEditView = true;
  }

  delete(em: Employment) {
    this.deleteReasonId = em.deleteReasonId;
    this.deleteSelectedId = em.id;
    this.hasEmploymentOnEp = em.hasEmploymentOnEp;

    this.preCheckError = false;
    this.delSub = this.workHistoryAppService.deletePreCheck(this.deleteSelectedId).subscribe(
      data => {
        this.deleteEligibility = data;
        this.isPrecheckLoading = false;
        if (em.isVerified) {
          if (this.deleteEligibility == null) this.deleteEligibility = new WhyReason();
          this.deleteEligibility.status = false;
          this.deleteEligibility.errors = [...this.deleteEligibility.errors, 'This job is verified through TJ/TMJ 60 Day Employment Verification. This job cannot be deleted.'];
        }
        if (this.deleteEligibility == null || this.deleteEligibility.status !== true) {
          this.inConfirmDeleteView = false;
          this.showError = true;
          this.precheck.errors = [...this.deleteEligibility.errors];
          this.precheck.warnings = [...this.deleteEligibility.warnings];
          this.getEmploymentsList(this.deleteSelectedId);
        } else {
          this.inConfirmDeleteView = true;
          this.showError = false;
        }
      },
      error => {
        this.preCheckError = true;
      }
    );
  }

  exitWorkHistory(event): void {
    if (event.isHourlyEntryClicked) {
      this.goToHourlyEntry(event.employment);
    } else {
      this.inEditView = false;
      this.getEmploymentsList();
      this.showError = false;
    }
  }

  goToSingleView(id: number): void {
    this.router.navigateByUrl(`/pin/${this.pin}/work-history/${id}`);
  }

  onCancelDelete() {
    this.inConfirmDeleteView = false;
    this.getEmploymentsList();
    this.hasEmploymentOnEp = false;
  }

  onConfirmDelete() {
    this.workHistoryAppService.deleteEmployment(this.deleteSelectedId, this.deleteReasonId).subscribe(complete => this.getEmploymentsList());
    this.inConfirmDeleteView = false;
    this.hasEmploymentOnEp = false;
  }

  sortByDate(): void | boolean {
    if (this.employments == null) {
      return false;
    }

    this.isSortByTypeNotToggled = null;

    this.employments.sort(function(a, b) {
      const aDate = new Date(a.jobBeginDate);
      const bDate = new Date(b.jobBeginDate);
      return aDate > bDate ? -1 : aDate < bDate ? 1 : 0;
    });

    if (this.isSortByDateToggled) {
      this.employments.reverse();
    }
    this.isSortByDateToggled = !this.isSortByDateToggled;
  }

  sortByType(): void | boolean {
    if (this.employments == null) {
      return false;
    }

    this.isSortByDateToggled = null;

    this.isSortByTypeNotToggled = !this.isSortByTypeNotToggled;
    if (this.isSortByTypeNotToggled) {
      this.employments.sort(function(a, b) {
        const nameA = a.jobTypeName.toLowerCase();
        const nameB = b.jobTypeName.toLowerCase();
        if (nameA < nameB) {
          return -1;
        } else if (nameA > nameB) {
          return 1;
        }
        return 0;
      });
    } else {
      this.employments.sort(function(a, b) {
        const nameA = a.jobTypeName.toLowerCase();
        const nameB = b.jobTypeName.toLowerCase();
        if (nameA < nameB) {
          return -1;
        } else if (nameA > nameB) {
          return 1;
        }
        return 0;
      });
      this.employments.reverse();
    }
  }

  onSearch(): void {
    const query = this.searchQuery.trim().toLowerCase();

    if (query !== '' && this.allEmployments != null) {
      this.employments = [];

      for (const emp of this.allEmployments) {
        if (emp.wageHour != null) {
          emp.wageHour.currentAverageWeeklyHours += this.hoursPerWeek;
          emp.wageHour.currentPayRate += '/' + emp.wageHour.currentPayRateIntervalName;
          emp.wageHour.currentPayRate = '$' + emp.wageHour.currentPayRate;
        }
      }

      const filteredEmploymentProgramTypeNameList = this.allEmployments.filter(
        function(el: Employment) {
          if (el.employmentProgramTypeName != null) {
            return el.employmentProgramTypeName.toLowerCase().indexOf(query) > -1;
          }
        }.bind(this)
      );

      for (const x of filteredEmploymentProgramTypeNameList) {
        this.employments.push(x);
      }

      const filteredJobTypeNameList = this.allEmployments.filter(
        function(el: Employment) {
          if (el.jobTypeName != null) {
            return el.jobTypeName.toLowerCase().indexOf(query) > -1;
          }
        }.bind(this)
      );

      for (const x of filteredJobTypeNameList) {
        this.employments.push(x);
      }

      const filteredJobBeginDateList = this.allEmployments.filter(
        function(el: Employment) {
          if (el.jobBeginDate != null) {
            return el.jobBeginDate.toLowerCase().indexOf(query) > -1;
          }
        }.bind(this)
      );

      for (const x of filteredJobBeginDateList) {
        this.employments.push(x);
      }

      const filteredJobEndDateList = this.allEmployments.filter(
        function(el: Employment) {
          if (el.jobEndDate != null) {
            return el.jobEndDate.toLowerCase().indexOf(query) > -1;
          }
        }.bind(this)
      );

      for (const x of filteredJobEndDateList) {
        this.employments.push(x);
      }

      const filteredJobPositionList = this.allEmployments.filter(
        function(el: Employment) {
          if (el.jobPosition != null) {
            return el.jobPosition.toLowerCase().indexOf(query) > -1;
          }
        }.bind(this)
      );

      for (const x of filteredJobPositionList) {
        this.employments.push(x);
      }

      const filteredCompanyNameList = this.allEmployments.filter(
        function(el: Employment) {
          if (el.companyName != null) {
            return el.companyName.toLowerCase().indexOf(query) > -1;
          }
        }.bind(this)
      );

      for (const x of filteredCompanyNameList) {
        this.employments.push(x);
      }

      const filteredAddresslocationList = this.allEmployments.filter(
        function(el: Employment) {
          if (el.location != null && el.location.fullAddress != null) {
            return el.location.fullAddress.toLowerCase().indexOf(query) > -1;
          }
        }.bind(this)
      );

      for (const x of filteredAddresslocationList) {
        this.employments.push(x);
      }

      const filteredCityStatelocationList = this.allEmployments.filter(
        function(el: Employment) {
          if (el.location != null && el.location.description != null) {
            return el.location.description.toLowerCase().indexOf(query) > -1;
          }
        }.bind(this)
      );

      for (const x of filteredCityStatelocationList) {
        this.employments.push(x);
      }

      const filteredReasonsForleavingList = this.allEmployments.filter(
        function(el: Employment) {
          if (el.leavingReasonName != null && el.leavingReasonName != null) {
            return el.leavingReasonName.toLowerCase().indexOf(query) > -1;
          }
        }.bind(this)
      );

      for (const x of filteredReasonsForleavingList) {
        this.employments.push(x);
      }

      const filteredAverageWeeklyHoursList = this.allEmployments.filter(
        function(el: Employment) {
          if (el.wageHour != null && el.wageHour.currentAverageWeeklyHours != null) {
            return el.wageHour.currentAverageWeeklyHours.toLowerCase().indexOf(query) > -1;
          }
        }.bind(this)
      );

      for (const x of filteredAverageWeeklyHoursList) {
        this.employments.push(x);
      }

      const filteredPayRateList = this.allEmployments.filter(
        function(el: Employment) {
          if (el.wageHour != null && el.wageHour.currentPayRate != null) {
            return el.wageHour.currentPayRate.toLowerCase().indexOf(query) > -1;
          }
        }.bind(this)
      );

      for (const x of filteredPayRateList) {
        this.employments.push(x);
      }

      const filteredDateDurationList = this.allEmployments.filter(
        function(el: Employment) {
          if (el.jobDateDuration != null) {
            return el.jobDateDuration.toLowerCase().indexOf(query) > -1;
          }
        }.bind(this)
      );

      for (const x of filteredDateDurationList) {
        this.employments.push(x);
      }

      const uniqueEmployments = this.employments.filter(function(item, pos, self) {
        return self.indexOf(item) === pos;
      });

      for (const emp of this.allEmployments) {
        if (emp.wageHour != null) {
          emp.wageHour.currentAverageWeeklyHours = emp.wageHour.currentAverageWeeklyHours.replace(this.hoursPerWeek, '');
          emp.wageHour.currentPayRate = emp.wageHour.currentPayRate.replace('/' + emp.wageHour.currentPayRateIntervalName, '');
          emp.wageHour.currentPayRate = emp.wageHour.currentPayRate.replace('$', '');
        }
      }

      this.employments = uniqueEmployments;
    } else {
      this.employments = this.allEmployments;
    }
  }

  numberInView(config, p, max: number) {
    return Utilities.numberOfItemsOnCurrentPage(config, p, max);
  }

  showAll() {
    if (!this.isShowAllToggled) {
      this.config.itemsPerPage = 10000;
      this.config.currentPage = 1;
    } else {
      this.config.itemsPerPage = 5;
    }
    this.isShowAllToggled = !this.isShowAllToggled;
  }

  ngOnDestroy(): void {
    if (this.eListSub != null) {
      this.eListSub.unsubscribe();
    }
  }

  filter() {
    let isFiltered = false;
    this.employments = Array.from(this.allEmployments);

    // Deleted vs Non Deleted Barriers.
    const cloneEmployments = Array.from(this.employments);
    if (this.isNonDeletedOnlyShown) {
      for (const e of cloneEmployments) {
        if (e.deleteReasonId != null) {
          isFiltered = true;
          const index = this.employments.indexOf(e);
          this.employments.splice(index, 1);
        }
      }
    } else if (!this.isNonDeletedOnlyShown) {
      this.employments = Array.from(this.allEmployments);
    }
    this.updatePagination();
    if (!isFiltered) {
      this.employments = Array.from(this.allEmployments);
    }
  }

  updatePagination() {
    const firstIndexCurrentPage = this.config.itemsPerPage * this.config.currentPage - (this.config.itemsPerPage - 1);
    if (this.employments.length < firstIndexCurrentPage) {
      this.config = {
        ...this.config,
        currentPage: Math.ceil(this.employments.length / this.config.itemsPerPage)
      };
    }
  }

  loaInView(beginDate, endDate?): boolean {
    const isBeginDateBefore = moment(beginDate, 'MM/DD/YYYY').isSameOrBefore(moment(Utilities.currentDate, 'MM/DD/YYYY'));
    const isEndDateAfter = moment(endDate, 'MM/DD/YYYY').isAfter(moment(Utilities.currentDate, 'MM/DD/YYYY'));
    return isBeginDateBefore && (!endDate || isEndDateAfter);
  }
}
