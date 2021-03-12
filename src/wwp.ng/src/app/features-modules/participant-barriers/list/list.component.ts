import { Component, OnInit, OnDestroy, Input, ChangeDetectionStrategy } from '@angular/core';
import { Router } from '@angular/router';

import { Subscription } from 'rxjs';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { ParticipantBarrier, FormalAssessment } from '../../../shared/models/participant-barriers-app';
import { ParticipantService } from '../../../shared/services/participant.service';
import { ParticipantBarrierAppService } from '../../../shared/services/participant-barrier-app.service';
import { IMultiSelectSettings } from '../../../shared/components/multi-select-dropdown/multi-select-dropdown.component';
import { Utilities } from '../../../shared/utilities';
import * as Fuse from 'fuse.js';
import * as _ from 'lodash';
import * as moment from 'moment';
import { PaginationInstance } from 'ng2-pagination';

@Component({
  selector: 'app-participant-barriers-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.css'],
  providers: [ParticipantBarrierAppService],
  changeDetection: ChangeDetectionStrategy.Default
})
export class ParticipantBarriersListComponent implements OnInit, OnDestroy {
  public config: PaginationInstance = {
    id: 'custom',
    itemsPerPage: 5,
    currentPage: 1
  };

  @Input() pin: string;
  @Input() isReadOnly = true;
  private pbListSub: Subscription;
  private bTypesSub: Subscription;

  public searchQuery = '';
  public filterQueryTypes: number[] = [];
  public participantBarriers: ParticipantBarrier[];
  public allParticipantBarriers: ParticipantBarrier[];
  public barrierTypes: DropDownField[];
  private barrierRecordId = 0;
  public inEditView = false;
  public inConfirmDeleteView = false;
  public isOpenOnlyShown = true;
  public isNonDeletedOnlyShown = true;
  private deleteSelectedId: number;
  private hasSortedByAscDate: boolean;
  public isLoaded = false;
  public isClosedBarrier = false;
  public isDeletedBarrier = false;

  public multSelectSettings: IMultiSelectSettings = {
    pullRight: false,
    enableSearch: false,
    checkedStyle: 'checkboxes',
    buttonClasses: 'btn btn-default',
    selectionLimit: 0,
    closeOnSelect: false,
    showCheckAll: false,
    showUncheckAll: false,
    dynamicTitleMaxItems: 1,
    maxHeight: '300px'
  };

  constructor(private participantBarrierAppService: ParticipantBarrierAppService, private fdService: FieldDataService, partService: ParticipantService, private router: Router) {}

  ngOnInit(): void {
    if (this.pin == null) {
      console.warn('PIN is not set');
    }
    this.participantBarrierAppService.setPin(this.pin);
    this.getParticipantBarrierList();
    this.bTypesSub = this.fdService.getParticipantBarriers().subscribe(data => this.initBarrierDropDowns(data));
  }

  getParticipantBarrierList(): void {
    this.pbListSub = this.participantBarrierAppService.getParticipantBarriers().subscribe(participantBarrier => {
      if (participantBarrier != null) {
        this.initSection(participantBarrier);
      }
    });
  }

  findClosedBarrier(data): boolean {
    let ret = false;
    if (data) {
      for (let i = 0; i < data.length; i++) {
        if (data[i].endDate) {
          ret = true;
          break;
        }
      }
    }
    return ret;
  }

  hasDeletedBarrier(barriers: ParticipantBarrier[]): boolean {
    let ret = false;
    if (barriers) {
      for (let i = 0; i < barriers.length; i++) {
        if (barriers[i].isDeleted) {
          ret = true;
          break;
        }
      }
    }
    return ret;
  }

  initSection(data: ParticipantBarrier[]): void {
    // reset section when get new data.
    this.hasSortedByAscDate = false;

    this.allParticipantBarriers = data;
    this.participantBarriers = Array.from(this.allParticipantBarriers);

    // Data has loaded.
    this.isClosedBarrier = this.findClosedBarrier(this.allParticipantBarriers);
    this.isDeletedBarrier = this.hasDeletedBarrier(this.allParticipantBarriers);
    this.isLoaded = true;
    this.filter();
    this.sortByDate();
  }

  initBarrierDropDowns(data) {
    this.barrierTypes = data;
  }

  addEditPartBarrier(id: number): void {
    this.barrierRecordId = id;
    this.inEditView = true;
  }

  filterOnType() {
    this.filter();
  }

  filterOnSearch() {
    this.filter();
  }

  filterOnToggleOpenCloseRecords() {
    this.isOpenOnlyShown = !this.isOpenOnlyShown;
    this.filter();
    this.sortByDate(false);
  }

  filterOnToggleIsDeletedRecords() {
    this.isNonDeletedOnlyShown = !this.isNonDeletedOnlyShown;
    this.filter();
    this.sortByDate(false);
  }

  sortByDate(allowReverse = true): void | boolean {
    if (this.participantBarriers == null) {
      return false;
    }

    this.participantBarriers.sort(function(a, b) {
      const aa = moment(a.onsetDate, 'MM/YYYY');
      const bb = moment(b.onsetDate, 'MM/YYYY');
      const aDate = new Date(aa.format('MM/DD/YYYY'));
      const bDate = new Date(bb.format('MM/DD/YYYY'));
      return aDate > bDate ? -1 : aDate < bDate ? 1 : 0;
    });

    if (allowReverse) {
      if (this.hasSortedByAscDate) {
        this.participantBarriers.reverse();
      }
      this.hasSortedByAscDate = !this.hasSortedByAscDate;
    }
  }

  filter() {
    let isFiltered = false;
    this.participantBarriers = Array.from(this.allParticipantBarriers);

    // Open vs Closed Barriers.
    const cloneParticipantBarriers = Array.from(this.participantBarriers);
    if (this.isOpenOnlyShown) {
      for (const e of cloneParticipantBarriers) {
        if (!e.isOpen) {
          isFiltered = true;
          const index = this.participantBarriers.indexOf(e);
          this.participantBarriers.splice(index, 1);
        }
      }
    } else if (!this.isOpenOnlyShown) {
      this.participantBarriers = Array.from(this.allParticipantBarriers);
    }

    // Deleted vs Non Deleted Barriers.
    const clonedParticipantBarriers = Array.from(this.participantBarriers);
    if (this.isNonDeletedOnlyShown) {
      for (const e of clonedParticipantBarriers) {
        if (e.isDeleted) {
          isFiltered = true;
          const index = this.participantBarriers.indexOf(e);
          this.participantBarriers.splice(index, 1);
        }
      }
    } else if (!this.isNonDeletedOnlyShown) {
      this.participantBarriers = Array.from(this.allParticipantBarriers.filter(x => x.endDate == null || x.wasClosedAtDisenrollment));
    }

    // Multi-select filter.
    if (this.filterQueryTypes != null && this.filterQueryTypes.length !== 0) {
      const filteredParticipantBarriers = Array.from(this.participantBarriers);

      const barrierIds = [];
      for (const b of this.barrierTypes) {
        barrierIds.push(b.id);
      }
      // Find all the types that need to be filted.
      const filterTypeIds = _.difference(barrierIds, this.filterQueryTypes);
      isFiltered = true;

      for (const filterTypeId of filterTypeIds) {
        for (const barrier of filteredParticipantBarriers) {
          const pb = _.find(this.participantBarriers, { barrierTypeId: filterTypeId });
          if (pb != null) {
            _.pull(this.participantBarriers, pb);
          }
        }
      }
    }

    // Search via text input goes here.
    if (this.searchQuery != null && this.searchQuery.trim() !== '') {
      const query = this.searchQuery.trim().toLowerCase();

      const options = {
        shouldSort: true,
        findAllMatches: false,
        threshold: 0.1,
        location: 0,
        distance: 100,
        maxPatternLength: 140,
        minMatchCharLength: 1,
        keys: ['barrierTypeName', 'barrierSubType.barrierSubTypeNames', 'onsetDate', 'endDate', 'formalAssessments.assessmentDate', 'formalAssessments.reassessmentRecommendedDate']
      };
      const fuse = new Fuse(this.participantBarriers, options);
      const searchResults = fuse.search<ParticipantBarrier>(query);
      this.participantBarriers = [];

      // If we have a valid query, then set isFiltered to true.
      isFiltered = true;
      // We have to find the deserialized object using the results of the search.
      for (const searchResult of searchResults) {
        // tslint:disable-next-line: triple-equals
        const pb = this.allParticipantBarriers.find(x => x.id == searchResult.id);
        // const pb = _.find(this.allParticipantBarriers, searchResult);
        this.participantBarriers.push(pb);
      }
    }

    if (!isFiltered) {
      this.participantBarriers = Array.from(this.allParticipantBarriers);
    }
  }

  numberInView(config, p, max: number) {
    return Utilities.numberOfItemsOnCurrentPage(config, p, max);
  }

  hasReassessmentRecommendedDatePassed(formalAssessment: FormalAssessment) {
    return formalAssessment.hasReassessmentRecommendedDatePassed;
  }

  goToSingleView(id: number): void {
    this.router.navigateByUrl(`/pin/${this.pin}/participant-barriers/${id}`);
  }

  delete(id: number) {
    this.deleteSelectedId = id;
    this.inConfirmDeleteView = true;
  }

  onConfirmDelete() {
    this.participantBarrierAppService.deleteParticipantBarrier(this.deleteSelectedId).subscribe(complete => this.getParticipantBarrierList());
    this.inConfirmDeleteView = false;
  }

  onCancelDelete() {
    this.inConfirmDeleteView = false;
  }

  exit(): void {
    this.inEditView = false;
    this.getParticipantBarrierList();
  }

  ngOnDestroy(): void {
    if (this.pbListSub != null) {
      this.pbListSub.unsubscribe();
    }
  }
}
