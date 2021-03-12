// tslint:disable: no-output-on-prefix
// tslint:disable: import-blacklist
import { Component, OnInit, Input, Output, EventEmitter, OnChanges, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { PaginationInstance } from 'ng2-pagination';

import { Employment } from '../../../shared/models/work-history-app';
import { WorkHistoryAppService } from '../../../shared/services/work-history-app.service';
import { Utilities } from '../../../shared/utilities';
import * as _ from 'lodash';
import * as moment from 'moment';

@Component({
  selector: 'app-work-history-embed',
  templateUrl: './embed.component.html',
  styleUrls: ['./embed.component.css']
})
export class WorkHistoryEmbedComponent implements OnInit, OnChanges, OnDestroy {
  public config: PaginationInstance = {
    id: 'custom',
    itemsPerPage: 5,
    currentPage: 1
  };

  @Input() pin;
  @Input() isReadOnly = false;
  @Input() isInvalid = false;
  @Input() hideDeleted = false;
  @Input() hasOnlyFcdp = false;
  @Output() onInEditView = new EventEmitter<boolean>();
  @Output() onUpdate = new EventEmitter<Employment[]>();
  @Output() checkState = new EventEmitter<boolean>();
  @Output() loaded = new EventEmitter<boolean>();

  public allEmployments: Employment[];
  public inConfirmDeleteView = false;
  private _inEditView = false;
  private deleteSelectedId: number;
  private deleteReasonId: number;
  private hasLoaded = false;
  public hasEmploymentOnEp = false;

  get inEditView(): boolean {
    return this._inEditView;
  }

  set inEditView(value: boolean) {
    this._inEditView = value;
    this.onInEditView.emit(this._inEditView);
  }

  private eListSub: Subscription;
  public employmentRecordId = 0;
  public workHistoryUrl: string;

  constructor(private workHistoryAppService: WorkHistoryAppService) {}

  ngOnInit() {
    if (this.pin == null) {
      console.warn('PIN is not set');
    }
    this.workHistoryAppService.setPin(this.pin);
    this.getEmploymentsList();
  }

  ngOnChanges() {
    // Anytime we have a pin, update the URL.
    if (this.pin != null) {
      this.workHistoryUrl = `/pin/${this.pin}/work-history`;
    }
  }

  ngOnDestroy() {
    if (this.eListSub != null) {
      this.eListSub.unsubscribe();
    }
  }

  getEmploymentsList(): void {
    this.eListSub = this.workHistoryAppService.getEmploymentList().subscribe(employments => {
      if (employments != null) {
        if (this.hideDeleted) {
          this.allEmployments = this.initSection(_.remove(employments, x => x.deleteReasonId === null));
        } else {
          this.allEmployments = this.initSection(employments);
        }
        this.onUpdate.emit(this.allEmployments);
        if (this.hasLoaded === true) {
          this.checkState.emit(true);
        } else {
          this.hasLoaded = true;
          this.loaded.emit(true);
        }
      }
      this.sortByDate();
    });
  }

  sortByDate(): void | boolean {
    if (this.allEmployments == null) {
      return false;
    }

    this.allEmployments.sort(function(a, b) {
      const aDate = new Date(a.jobBeginDate);
      const bDate = new Date(b.jobBeginDate);
      return aDate > bDate ? -1 : aDate < bDate ? 1 : 0;
    });
  }

  edit(id: number): void {
    this.inEditView = true;
    this.employmentRecordId = id;
  }

  add(): void {
    this.employmentRecordId = 0;
    this.inEditView = true;
  }

  delete(em: Employment) {
    this.deleteReasonId = em.deleteReasonId;
    this.deleteSelectedId = em.id;
    this.inConfirmDeleteView = true;
    this.hasEmploymentOnEp = em.hasEmploymentOnEp;
  }

  onCancelDelete() {
    this.inConfirmDeleteView = false;
    this.hasEmploymentOnEp = false;
  }

  onConfirmDelete() {
    this.workHistoryAppService.deleteEmployment(this.deleteSelectedId, this.deleteReasonId).subscribe(complete => this.getEmploymentsList());

    this.inConfirmDeleteView = false;
    this.hasEmploymentOnEp = false;
  }

  exitWorkHistory(): void {
    this.inEditView = false;
    this.getEmploymentsList();
  }

  calculateHourlyWage(e: Employment) {
    if (e.wageHour != null) {
      return e.wageHour.calculateHourlyWage();
    }
  }

  initSection(data: Employment[]): Employment[] {
    const model = data;

    if (model != null) {
      for (const emp of model) {
        if (emp.jobEndDate != null && emp.jobBeginDate != null) {
          if (emp.jobEndDate != null && emp.jobBeginDate != null) {
            emp.jobDateDuration = Utilities.getDurationBetweenDates(emp.jobBeginDate, emp.jobEndDate);
          } else if (emp.jobBeginDate != null && emp.isCurrentlyEmployed === true) {
            emp.jobDateDuration = Utilities.getDurationBetweenDates(emp.jobBeginDate, Utilities.currentDate.format('MM/DD/YYYY'));
          }
        }
      }
    }
    return model;
  }

  numberInView(config, p, max: number) {
    return Utilities.numberOfItemsOnCurrentPage(config, p, max);
  }

  goToSingleView(id: number): void {
    const url = '/pin/' + this.pin + '/work-history/' + id;
    const win = window.open(url, '_blank');
  }

  loaInView(beginDate, endDate?): boolean {
    const isBeginDateBefore = moment(beginDate, 'MM/DD/YYYY').isSameOrBefore(moment(Utilities.currentDate, 'MM/DD/YYYY'));
    const isEndDateAfter = moment(endDate, 'MM/DD/YYYY').isAfter(moment(Utilities.currentDate, 'MM/DD/YYYY'));
    return isBeginDateBefore && (!endDate || isEndDateAfter);
  }
}
