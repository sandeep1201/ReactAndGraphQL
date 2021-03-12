import { Component, OnInit, OnDestroy, ComponentRef, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';

import { ActionNeededEditComponent } from './edit/edit.component';
import { ModalService } from 'src/app/core/modal/modal.service';
import { BaseParticipantComponent } from 'src/app/shared/components/base-participant-component';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { ActionNeededTask } from 'src/app/features-modules/actions-needed/models/action-needed-new';
import { ActionNeededService } from 'src/app/features-modules/actions-needed/services/action-needed.service';
import { FieldDataService } from 'src/app/shared/services/field-data.service';
import { AccessType } from 'src/app/shared/enums/access-type.enum';
import { AppService } from 'src/app/core/services/app.service';
import { Utilities } from 'src/app/shared/utilities';
import { ActionNeededListPipe } from './pipes/action-needed-list.pipe';
import { IMultiSelectOption } from 'src/app/shared/components/multi-select-dropdown/multi-select-dropdown.component';
import { ParticipantService } from 'src/app/shared/services/participant.service';

@Component({
  selector: 'app-actions-needed',
  templateUrl: './actions-needed.component.html',
  styleUrls: ['./actions-needed.component.css']
})
export class ActionsNeededComponent extends BaseParticipantComponent implements OnInit, OnDestroy {
  public searchQuery: string;

  private prioritiesDrop: DropDownField[];
  public actionNeededPagesDrop: DropDownField[];

  public isLoaded = false;
  public goBackUrl: string;
  public pin: string;

  public isFilterAsc = false;
  public showSortOptions = false;

  public actionNeededs: ActionNeededTask[];

  public filterOptions: DropDownField[];

  public selectedFilters: number[] = [];

  public filterOnGoing = true;
  public filterCompleted = false;
  public filterNotCompleted = false;
  public filterRecentActions = true;

  public filteredList = [];
  public isReadOnly = true;
  private modalServiceBase: ModalService;
  private tempModalRef: ComponentRef<ActionNeededEditComponent>;

  private actionNeededService: ActionNeededService;

  private fdService: FieldDataService;

  private acSub: Subscription;

  private prSub: Subscription;

  private aNPSub: Subscription;

  readonly ongoingText = 'Ongoing';
  readonly recentActionsText = 'Recent Actions';
  readonly completedText = 'Completed';
  readonly didNotCompleteText = 'Did Not Complete';
  AccessType = AccessType;

  constructor(
    actionNeededService: ActionNeededService,
    fdService: FieldDataService,
    private appService: AppService,
    route: ActivatedRoute,
    router: Router,
    partService: ParticipantService,
    modalService: ModalService
  ) {
    super(route, router, partService);
    this.actionNeededService = actionNeededService;
    this.fdService = fdService;
    this.modalServiceBase = modalService;
  }

  ngOnInit() {
    super.onInit();
    this.initFilterOptions();
  }

  public onPinInit() {
    this.goBackUrl = '/pin/' + this.pin;
    this.actionNeededService.setPin(this.pin);
    this.getModel();
  }

  public onParticipantInit() {
    if (this.appService.isParticipantEditable(this.participant)) {
      this.isReadOnly = false;
    } else {
      this.isReadOnly = true;
    }
  }

  public addTask() {
    if (this.tempModalRef && this.tempModalRef.instance) {
      this.tempModalRef.instance.destroy();
    }
    this.modalServiceBase.create<ActionNeededEditComponent>(ActionNeededEditComponent).subscribe(x => {
      x.instance.pin = this.pin;
      x.instance.modelId = 0;
      this.tempModalRef = x;
      this.tempModalRef.hostView.onDestroy(() => this.getModel());
    });
  }

  public getModel() {
    this.acSub = this.actionNeededService.getList().subscribe(data => {
      this.initTasks(data);
      this.isLoaded = true;
    });
  }

  ngOnDestroy() {
    if (this.acSub != null) {
      this.acSub.unsubscribe();
    }
    if (this.prSub != null) {
      this.prSub.unsubscribe();
    }
    if (this.aNPSub != null) {
      this.aNPSub.unsubscribe();
    }
  }

  public filter() {
    this.filterOnGoing = false;
    this.filterCompleted = false;
    this.filterNotCompleted = false;
    this.filterRecentActions = false;

    if (this.selectedFilters.indexOf(Utilities.idByFieldDataName(this.ongoingText, this.filterOptions)) > -1) {
      this.filterOnGoing = true;
    }

    if (this.selectedFilters.indexOf(Utilities.idByFieldDataName(this.recentActionsText, this.filterOptions)) > -1) {
      this.filterRecentActions = true;
    }

    if (this.selectedFilters.indexOf(Utilities.idByFieldDataName(this.completedText, this.filterOptions)) > -1) {
      this.filterCompleted = true;
    }

    if (this.selectedFilters.indexOf(Utilities.idByFieldDataName(this.didNotCompleteText, this.filterOptions)) > -1) {
      this.filterNotCompleted = true;
    }
    this.getFilteredList();
  }

  public getFilteredList() {
    const x = new ActionNeededListPipe();
    this.filteredList = x.transform(this.actionNeededs, this.filterOnGoing, this.filterRecentActions, this.filterCompleted, this.filterNotCompleted);
  }

  public reverseList() {
    if (this.actionNeededs != null) {
      this.actionNeededs = Array.from(this.actionNeededs.reverse());
    }
  }

  private defaultFilter() {
    const recentId = Utilities.idByFieldDataName(this.recentActionsText, this.filterOptions);
    const onGoingId = Utilities.idByFieldDataName(this.ongoingText, this.filterOptions);
    this.selectedFilters.push(recentId);
    this.selectedFilters.push(onGoingId);
  }

  private initFilterOptions() {
    const options = [this.ongoingText, this.recentActionsText, this.completedText, this.didNotCompleteText];
    const descriptions = [
      'Tasks that are awaiting completion',
      'Tasks that have been completed or marked as "did not complete" within the last 30 days',
      'All tasks that have been completed',
      'All tasks that have been marked as "did not complete'
    ];

    this.filterOptions = options.map(function(option, index) {
      const x: IMultiSelectOption = {
        name: option,
        isDisabled: false,
        description: descriptions[index],
        disablesOthers: false,
        id: ++index
      };

      return x;
    });

    // Since we have the filter dropdown, lets set the default filter.
    this.defaultFilter();
  }

  private initTasks(data) {
    this.filteredList = this.actionNeededs = data;
  }

  getAccess(): AccessType {
    let accessType = AccessType.none;
    if (this.appService && this.appService.coreAccessContext) accessType = this.appService.coreAccessContext.evaluate();

    return accessType;
  }
}
