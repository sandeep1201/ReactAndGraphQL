import { FieldDataService } from './../../shared/services/field-data.service';
// tslint:disable: import-blacklist
import { Component, OnInit, ComponentRef, Input } from '@angular/core';
import { WorkerTask } from './models/worker-task.model';
import { AppService } from '../../core/services/app.service';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import * as _ from 'lodash';
import { Utilities } from 'src/app/shared/utilities';
import { take, concatMap } from 'rxjs/operators';
import { WorkerService } from 'src/app/shared/services/worker.service';
import { forkJoin, of } from 'rxjs';
import { ReassignComponent } from 'src/app/enrollment/reassign/reassign.component';
import { ModalService } from 'src/app/core/modal/modal.service';
import { WorkerTaskService } from 'src/app/shared/components/worker-task/worker-task.service';
import { FieldDataTypes } from 'src/app/shared/enums/field-data-types.enum';
import { Router } from '@angular/router';

@Component({
  selector: 'app-worker-task-list',
  templateUrl: './worker-task.component.html',
  styleUrls: ['./worker-task.component.scss']
})
export class WorkerTaskComponent implements OnInit {
  public isLoaded = false;
  public dueDateId;
  public searchQuery: string;
  public selectedSort: number;
  public categoryTypeDrop: DropDownField[] = [];
  private origCategoryTypeDrop: DropDownField[] = [];
  public originalWorkerTasks: WorkerTask[] = [];
  public categoryTypes = [];
  public filterOptions: DropDownField[] = [
    { id: 1, name: 'System' },
    { id: 2, name: 'Manual' }
  ];
  public sortDrop: DropDownField[] = [];
  public workerDrop: DropDownField[] = [];
  public reAssignWorkerDrop: DropDownField[] = [];
  public statusDrop: DropDownField[] = [];
  public workerTasks: WorkerTask[];
  public filteredTasks: WorkerTask[] = [];
  public goBackUrl: string;
  public selectedTask: WorkerTask;
  public selectedWorker = '';
  public showWorkerDrop = false;
  private tempModalReassignComponent: ComponentRef<ReassignComponent>;
  public isInEditMode = false;
  filterQueryTypes = [];

  constructor(
    private workerTaskService: WorkerTaskService,
    public appService: AppService,
    private workerService: WorkerService,
    private modalService: ModalService,
    private fdService: FieldDataService,
    private router: Router
  ) {}

  ngOnInit() {
    this.goBackUrl = `/home`;
    this.initModel();
  }

  initModel(selectedWorker = null): void {
    this.showWorkerDrop =
      this.appService.user.roles.indexOf('W-2 QC Staff') >= 0 ||
      this.appService.user.roles.indexOf('W-2 Case Management Supervisor') >= 0 ||
      this.appService.user.roles.indexOf('EA Supervisor') >= 0;
    this.selectedWorker = selectedWorker ? selectedWorker : this.appService.user.wiuid;
    this.workerTaskService.modeForWorkerTask
      .pipe(
        concatMap(res => {
          this.isInEditMode = res.isInEditMode;
          const result0 = !this.isInEditMode ? this.workerService.getWorkersByOrgAndRole(this.appService.user.agencyCode).pipe(take(1)) : of(this.workerDrop);
          const result1 = !this.isInEditMode ? this.fdService.getFieldDataByField(FieldDataTypes.WorkerTaskStatus).pipe(take(1)) : of(this.statusDrop);
          return forkJoin(this.workerTaskService.getWorkerTaskList(this.selectedWorker).pipe(take(1)), result0, result1);
        })
      )
      .subscribe(results => {
        this.workerTasks = results[0];
        this.resetAllFilters();
        this.originalWorkerTasks = results[0];
        this.sortTasks();
        this.initSortDrop();
        this.initCategoryTypes();
        this.origCategoryTypeDrop = this.categoryTypeDrop;
        this.initWorkerDrop(results[1]);
        this.statusDrop = results[2];

        this.isLoaded = true;
      });
  }

  resetAllFilters() {
    this.categoryTypes = [];
    this.searchQuery = null;
    this.filterQueryTypes = [];
    this.filteredTasks = [];
  }

  changeState(task: WorkerTask) {
    task.workerTaskStatusId = +Utilities.fieldDataIdByCode('CP', this.statusDrop);
    this.selectedTask = task;
    this.workerTaskService
      .saveCompletedWorkerTask(task)
      .pipe(take(1))
      .subscribe(data => {
        this.selectedTask.statusDate = data.statusDate;
        this.selectedTask.workerTaskStatusCode = data.workerTaskStatusCode;
      });
  }

  private initWorkerDrop(result) {
    const sortedWorkers = Utilities.sortArrayByProperty(result, 'lastName');

    this.workerDrop = [];
    this.reAssignWorkerDrop = [];
    for (const item of sortedWorkers) {
      const dd1 = new DropDownField();
      dd1.id = item.wiuid;
      dd1.name = item.fullNameWithMiddleInitialTitleCase;
      this.workerDrop.push(dd1);

      const dd2 = new DropDownField();
      dd2.id = item.id;
      dd2.name = item.fullNameWithMiddleInitialTitleCase;
      this.reAssignWorkerDrop.push(dd2);
    }
  }

  public selectedWorkerNameChange() {
    this.isLoaded = false;
    if (this.selectedWorker) {
      this.workerTaskService.getWorkerTaskList(this.selectedWorker).subscribe(result => {
        this.workerTasks = result;
        this.originalWorkerTasks = result;
        this.sortTasks();
        this.resetAllFilters();
        this.initCategoryTypes();
        this.isLoaded = true;
      });
    } else {
      this.workerTasks = [];
      this.originalWorkerTasks = [];
      this.categoryTypes = [];
      this.initCategoryTypes();
      this.isLoaded = true;
    }
  }

  forceUpdate() {
    this.initModel(this.selectedWorker);
  }

  public reassign(workerTask: WorkerTask) {
    if (this.tempModalReassignComponent && this.tempModalReassignComponent.instance) {
      this.tempModalReassignComponent.instance.destroy();
    }
    this.modalService.create<ReassignComponent>(ReassignComponent).subscribe(x => {
      this.reAssignWorkerDrop.forEach(i => {
        i.isSelected = false;
      });
      this.tempModalReassignComponent = x;
      this.tempModalReassignComponent.instance.workerTaskId = workerTask.id;
      this.tempModalReassignComponent.instance.workerTaskWkrId = workerTask.workerId;
      this.tempModalReassignComponent.instance.isWorkerTask = true;
      this.tempModalReassignComponent.instance.workerDrop = this.reAssignWorkerDrop;
      this.tempModalReassignComponent.hostView.onDestroy(() => this.forceUpdate());
    });
  }

  public reverseClick() {
    this.reverseList();
  }

  public reverseList() {
    if (this.workerTasks != null) {
      this.workerTasks = Array.from(this.workerTasks.reverse());
    }
  }

  public sortTasks() {
    this.workerTasks = this.sortDueDate(this.workerTasks, this.selectedSort === Utilities.idByFieldDataName('Date and Priority', this.sortDrop));
  }

  private sortDueDate(workerTask: WorkerTask[], dateAndPriority = false): WorkerTask[] {
    const sortedByDueDate = _.sortBy<WorkerTask>(workerTask, [(o: any) => (dateAndPriority ? o.actionPriorityId : o), (o: any) => new Date(o.taskDate)]);

    return sortedByDueDate;
  }

  private initSortDrop() {
    const options = ['Date', 'Date and Priority'];

    this.sortDrop = options.map(function(option, index) {
      const x = new DropDownField();
      x.id = index;
      x.name = option;
      return x;
    });

    this.dueDateId = Utilities.idByFieldDataName('Date', this.sortDrop);
  }

  filterOnGeneratedType() {
    if (this.filterQueryTypes.length > 0) {
      let filterQueryTypeNames = [];
      this.filterQueryTypes.forEach(type => {
        filterQueryTypeNames.push(Utilities.fieldDataNameById(type, this.filterOptions));
      });
      if (filterQueryTypeNames.indexOf('System') > -1 && filterQueryTypeNames.indexOf('Manual') === -1) {
        this.filteredTasks = !_.isEmpty(this.filteredTasks)
          ? this.filteredTasks.filter(task => task.isSystemGenerated)
          : this.originalWorkerTasks.filter(task => task.isSystemGenerated);
      } else if (filterQueryTypeNames.indexOf('System') === -1 && filterQueryTypeNames.indexOf('Manual') > -1) {
        this.filteredTasks = !_.isEmpty(this.filteredTasks)
          ? this.filteredTasks.filter(task => !task.isSystemGenerated)
          : this.originalWorkerTasks.filter(task => !task.isSystemGenerated);
      } else if (filterQueryTypeNames.indexOf('System') > -1 && filterQueryTypeNames.indexOf('Manual') > -1) {
        this.filteredTasks = !_.isEmpty(this.filteredTasks) ? this.filteredTasks : this.originalWorkerTasks;
      }
      this.workerTasks = this.filteredTasks;
    } else {
      this.workerTasks = !_.isEmpty(this.filteredTasks) ? this.filteredTasks : this.originalWorkerTasks;
    }
  }

  initCategoryTypes() {
    if (!_.isEmpty(this.filteredTasks)) {
      const categoryTypeNames: DropDownField[] = [];
      const workerTasks = this.filteredTasks;
      workerTasks.forEach(j => {
        const ctdd = new DropDownField();
        ctdd.id = j.categoryId;
        ctdd.name = j.categoryName;
        categoryTypeNames.push(ctdd);
      });

      this.categoryTypeDrop = _.orderBy<DropDownField>(_.uniqBy(categoryTypeNames, 'name'), [i => i.name], ['asc']);
    } else if (_.isEmpty(this.filteredTasks) && this.originalWorkerTasks) {
      const categoryTypeNames: DropDownField[] = [];
      const workerTasks = this.originalWorkerTasks;
      workerTasks.forEach(j => {
        const ctdd = new DropDownField();
        ctdd.id = j.categoryId;
        ctdd.name = j.categoryName;
        categoryTypeNames.push(ctdd);
      });

      this.categoryTypeDrop = _.orderBy<DropDownField>(_.uniqBy(categoryTypeNames, 'name'), [i => i.name], ['asc']);
    }

    this.categoryTypes = this.categoryTypeDrop && this.categoryTypeDrop.length > 0 ? this.categoryTypes : [];
  }

  filterByCategory() {
    if (this.categoryTypes && this.categoryTypes.length > 0) {
      let categoryTypeNames = [];
      this.categoryTypes.forEach(type => {
        categoryTypeNames.push(Utilities.fieldDataNameById(type, this.categoryTypeDrop));
      });
      if (!_.isEmpty(this.filteredTasks)) {
        this.filteredTasks = this.filteredTasks.filter(i => categoryTypeNames.indexOf(i.categoryName) > -1);
      } else {
        this.filteredTasks = this.originalWorkerTasks.filter(i => categoryTypeNames.indexOf(i.categoryName) > -1);
      }
    } else {
      this.workerTasks = !_.isEmpty(this.filteredTasks) ? this.filteredTasks : this.originalWorkerTasks;
    }
  }

  filterList(searchQuery: string) {
    if (searchQuery != null && searchQuery.trim() !== '' && this.workerTasks != null) {
      const query = searchQuery.trim().toLowerCase();
      this.filteredTasks = this.originalWorkerTasks.filter(
        x =>
          x.fullName.toLowerCase().indexOf(query) > -1 ||
          x.pin.toString().indexOf(query) > -1 ||
          x.taskDate.indexOf(query) > -1 ||
          x.categoryName
            .toLowerCase()
            .toString()
            .indexOf(query) > -1 ||
          x.taskDetails.toLowerCase().indexOf(query) > -1
      );

      this.workerTasks = this.filteredTasks;
    } else {
      this.workerTasks = !_.isEmpty(this.filteredTasks) ? this.filteredTasks : this.originalWorkerTasks;
    }
  }

  filter() {
    this.filteredTasks = [];
    this.filterList(this.searchQuery);
    this.filterByCategory();
    this.filterOnGeneratedType();
  }

  onEdit(workerTask: WorkerTask) {
    this.isInEditMode = true;
    this.workerTaskService.modeForWorkerTask.next({ id: 0, readOnly: false, isInEditMode: this.isInEditMode, data: workerTask });
  }

  onSelect(pin: string) {
    this.router.navigate(['pin', pin]);
  }
}
