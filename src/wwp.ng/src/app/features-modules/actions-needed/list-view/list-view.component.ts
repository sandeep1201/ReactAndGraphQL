import { Component, OnInit, Input, ComponentRef, EventEmitter, Output } from '@angular/core';

import * as Fuse from 'fuse.js';
import * as _ from 'lodash';
import * as moment from 'moment';
import * as jsPDF from 'jspdf';

import { ActionNeededListPipe } from '../pipes/action-needed-list.pipe';
import { AccessType } from 'src/app/shared/enums/access-type.enum';
import { ActionNeededTask } from 'src/app/features-modules/actions-needed/models/action-needed-new';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { Utilities } from 'src/app/shared/utilities';

@Component({
  selector: 'app-action-needed-list-view',
  templateUrl: './list-view.component.html',
  styleUrls: ['./list-view.component.css']
})
export class ActionNeededListViewComponent implements OnInit {
  @Output()
  reload = new EventEmitter();
  @Input()
  listName: string;
  @Input()
  isReadOnly = true;
  @Input()
  accessType: AccessType;
  AccessType = AccessType;

  @Input()
  participantName: string;

  private _filterNotCompleted = false;

  get filterNotCompleted(): boolean {
    return this._filterNotCompleted;
  }

  @Input('filterNotCompleted')
  set filterNotCompleted(value: boolean) {
    this._filterNotCompleted = value;
    this.updateFilteredTasks();
  }

  private _filterCompleted = false;

  get filterCompleted(): boolean {
    return this._filterCompleted;
  }

  @Input('filterCompleted')
  set filterCompleted(value: boolean) {
    this._filterCompleted = value;
    this.updateFilteredTasks();
  }

  private _filterOnGoing = false;

  get filterOnGoing(): boolean {
    return this._filterOnGoing;
  }

  @Input('filterOnGoing')
  set filterOnGoing(value: boolean) {
    this._filterOnGoing = value;
    this.updateFilteredTasks();
  }

  private _filterRecentActions = false;

  get filterRecentActions(): boolean {
    return this._filterRecentActions;
  }

  @Input('filterRecentActions')
  set filterRecentActions(value: boolean) {
    this._filterRecentActions = value;
    this.updateFilteredTasks();
  }

  @Input()
  actionNeededPagesDrop: DropDownField[];
  public dueDateId;
  public isLoaded = false;

  get allTasks(): ActionNeededTask[] {
    return this._allTasks;
  }

  @Input('allTasks')
  set allTasks(value: ActionNeededTask[]) {
    if (value != null) {
      this._allTasks = [];
      for (const an of value) {
        if (an.assigneeName === this.listName) {
          this._allTasks.push(an);
        }
      }

      this.tasks = this.allTasks;
      this.updateFilteredTasks();

      // Only setDefaultSort on first load.
      if (this.isLoaded === false) {
        this.initSortDrop();
        this.setDefaultSort();
        this.isLoaded = true;
      } else {
        this.sortTasks();
      }
    }
  }

  public sortDrop: DropDownField[] = [];
  /**
   *  Populates list on view.
   *
   * @type {ActionNeededTask[]}
   * @memberof ActionNeededListViewComponent
   */
  public tasks: ActionNeededTask[] = [];

  public filteredTasks: ActionNeededTask[] = [];

  public selectedSort: number;
  private _searchQuery: string;
  /**
   *  These are filtered tasks based on list type.
   *
   * @private
   * @type {ActionNeededTask[]}
   * @memberof ActionNeededListViewComponent
   */
  private _allTasks: ActionNeededTask[] = [];
  public isPrinting = false;

  // We'll use the same pipe the markup uses to.
  private pipe: ActionNeededListPipe = new ActionNeededListPipe();

  ngOnInit() {}

  @Input()
  get searchQuery(): string {
    return this._searchQuery;
  }

  set searchQuery(v: string) {
    if (v !== this._searchQuery) {
      this._searchQuery = v;
      this.search(this._searchQuery);
    }
  }

  public sortTasks() {
    if (+this.selectedSort === Utilities.idByFieldDataName('Completion Date', this.sortDrop)) {
      this.tasks = this.sortCompletionDate(this.tasks);
    } else if (+this.selectedSort === Utilities.idByFieldDataName('Priority', this.sortDrop)) {
      this.tasks = this.sortPriority(this.tasks);
    } else if (+this.selectedSort === Utilities.idByFieldDataName('Due Date', this.sortDrop)) {
      this.tasks = this.sortDueDate(this.tasks);
    } else if (+this.selectedSort === Utilities.idByFieldDataName('Page', this.sortDrop)) {
      this.tasks = this.sortPage(this.tasks);
    }
  }

  public reverseClick() {
    this.reverseList();
  }

  public forceReload(e) {
    this.reload.emit();
  }

  public printCol() {
    // For now we only use PDF printing for Participant Action Neededs
    if (this.listName.indexOf('Participant') >= 0) {
      this.printToPdf();
    } else {
      const perm = 'javascript:setTimeout(function () {window.close()}, 500);';
      const c = window;
      const w = window.open(perm, 'printing...', '_blank', false);
      w.blur();
      c.focus();
      w.onunload = () => {
        this.isPrinting = false;
      };
      this.isPrinting = true;
      setTimeout(function() {
        window.print();
      }, 250);
    }
  }

  private printToPdf() {
    const doc = new jsPDF('portrait', 'in', 'letter');

    doc.setProperties({
      title: 'W-2 Action Needed',
      subject: 'This is the subject',
      author: 'Author Name',
      creator: 'Department of Children & Families'
    });

    const pageHeight = 11; //doc.internal.pageSize.height || doc.internal.pageSize.getHeight();
    const pageWidth = 8.5; //doc.internal.pageSize.width || doc.internal.pageSize.getWidth();
    const margin = 0.5;
    const marginOffset = 0.01;
    const lineHeight = 0.167;
    const today = Utilities.currentDate.format('MM/DD/YYYY');

    this.updateFilteredTasks();
    const data = {
      participantName: this.participantName,
      date: today,
      tasks: this.filteredTasks
    };

    this.startPage(doc, pageWidth, margin, data.participantName);
    let offset = margin + 0.6;

    for (let index = 0; index < data.tasks.length; index++) {
      const task = data.tasks[index];
      const calcOffset = this.calculateTaskOffset(doc, task, offset, pageWidth, margin);

      if (calcOffset > pageHeight - margin * 2) {
        // Create a new page.
        this.writeFooter(doc, data.date, pageWidth, pageHeight, margin);
        doc.addPage();
        this.startPage(doc, pageWidth, margin, data.participantName);
        offset = margin + 0.6;
      }

      offset = this.writeTask(doc, task, offset, pageWidth, margin);

      // Draw a dividing line after the task.
      doc.setLineWidth(0.005);
      doc.line(margin - marginOffset, offset, pageWidth - margin + marginOffset, offset);
      offset += 0.25;
    }

    this.writeFooter(doc, data.date, pageWidth, pageHeight, margin);

    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      // doc.output('save', 'action-needed.pdf');
      const blob = doc.output('blob');
      window.navigator.msSaveOrOpenBlob(blob);
    } else {
      window.open(doc.output('bloburl'));
    }
  }

  private startPage(doc: jsPDF, pageWidth: number, margin: number, participantName: string) {
    const lineHeight = 0.15;
    const lineOffset = 0.05;
    const marginOffset = 0.01;

    doc.setFontSize(16);
    doc.setTextColor(150);

    doc.text('Action Items for: ' + participantName, margin, margin + lineHeight, 'left');
    doc.setLineWidth(0.01);
    doc.line(margin - marginOffset, margin + lineHeight + lineOffset, pageWidth - margin + marginOffset, margin + lineHeight + lineOffset);
  }

  private writeFooter(doc: jsPDF, date: string, pageWidth: number, pageHeight: number, margin: number) {
    doc.setFont('helvetica');
    doc.setTextColor(150);
    doc.setFontSize(10);
    doc.text(date, pageWidth - margin, pageHeight - margin, 'right');
  }

  private writeTask(doc: jsPDF, task: ActionNeededTask, offset: number, pageWidth: number, margin: number): number {
    // -- "Task" name
    // -- Priority
    // -- Details
    // -- Completion Date
    // -- Due Date
    const lineHeight = 0.167;
    const lineBuffer = 0.084;

    doc.setFont('helvetica');
    doc.setTextColor(0);
    doc.setFontSize(10);

    if (task.priorityName != null && task.priorityName !== '') {
      doc.text(`Priority: ${task.priorityName}`, pageWidth - margin, offset, 'right');
      offset = offset + lineHeight + lineBuffer;
    }

    if (task.followUpTask != null && task.followUpTask !== '') {
      doc.setFontType('bold');
      doc.text(task.followUpTask, margin, offset, 'left');
      offset = offset + lineHeight + lineBuffer;
    }

    doc.setFontType('normal');

    if (task.details != null && task.details !== '') {
      const lines = doc.splitTextToSize(task.details, pageWidth - margin * 2);
      doc.text(margin, offset, lines);
      offset = offset + lineHeight * lines.length + lineBuffer;
    }

    if (task.dueDate != null) {
      const formatted = moment(task.dueDate).format('MM/DD/YYYY');
      doc.text(`Due Date: ${formatted}`, margin, offset, 'left');
    }

    if (task.isNoCompletionDate === true) {
      doc.text('Date Completed: Did Not Complete', (pageWidth - margin) / 2, offset, 'left');
    } else if (task.completionDate != null) {
      const formatted = moment(task.completionDate).format('MM/DD/YYYY');
      doc.text(`Date Completed: ${formatted}`, (pageWidth - margin) / 2, offset, 'left');
    }

    // This is some tricky logic to handle the 3 variables: due date, completion
    // date and is not completed.  Note that the completion date will have a
    // value when the user clicks the Did Not Complete.
    if (task.dueDate != null) {
      offset = offset + lineHeight;
    } else if (task.completionDate != null || task.isNoCompletionDate === true) {
      offset = offset + lineHeight;
    }

    return offset;
  }

  private calculateTaskOffset(doc: jsPDF, task, offset: number, pageWidth: number, margin: number): number {
    const lineHeight = 0.167;
    const lineBuffer = 0.084;

    // Task title
    if (task.followUpTask != null && task.followUpTask !== '') {
      offset = offset + lineHeight + lineBuffer;
    }

    if (task.details != null && task.details !== '') {
      doc.setFont('helvetica');
      doc.setFontType('normal');
      const lines = doc.splitTextToSize(task.details, pageWidth - margin * 2);
      offset = offset + lineHeight * lines.length + lineBuffer;
    }

    // This is some tricky logic to handle the 3 variables: due date, completion
    // date and is not completed.  Note that the completion date will have a
    // value when the user clicks the Did Not Complete.
    if (task.dueDate != null) {
      offset = offset + lineHeight;
    } else if (task.completionDate != null && task.isNoCompletionDate != true) {
      offset = offset + lineHeight;
    }

    return offset;
  }

  private initSortDrop() {
    const options = ['Page', 'Priority', 'Completion Date', 'Due Date'];

    this.sortDrop = options.map(function(option, index) {
      const x = new DropDownField();
      x.id = index;
      x.name = option;
      return x;
    });

    this.dueDateId = Utilities.idByFieldDataName('Due Date', this.sortDrop);
  }

  /**
   *  Fuzzy searches json fields 'followUpTask',  'actionItemName', 'pageName' and 'details'.
   *  US 1262
   * @private
   * @param {string} searchQuery
   * @memberof ActionNeededListViewComponent
   */
  private search(searchQuery: string) {
    if (searchQuery != null && searchQuery.trim() !== '' && this.tasks != null) {
      const query = searchQuery.trim().toLowerCase();

      const options = {
        shouldSort: true,
        findAllMatches: true,
        threshold: 0.1,
        location: 0,
        distance: 100,
        maxPatternLength: 140,
        minMatchCharLength: 1,
        keys: ['followUpTask', 'actionItemName', 'pageName', 'details']
      };
      const fuse = new Fuse(this.allTasks, options);
      const searchResults = fuse.search<ActionNeededTask>(query);
      this.tasks = [];

      // We have to find the deserialized object using the results of the search.
      for (const searchResult of searchResults) {
        const t = this.allTasks.find(x => x.id == searchResult.id);
        // const t = _.find(this.allTasks, searchResult);
        this.tasks.push(t);
      }
    } else {
      this.tasks = this.allTasks;
    }

    this.updateFilteredTasks();
  }

  private updateFilteredTasks() {
    this.filteredTasks = this.pipe.transform(this.tasks, this.filterOnGoing, this.filterRecentActions, this.filterCompleted, this.filterNotCompleted);
  }

  private reverseList(): void {
    if (this.tasks == null) {
      return;
    }
    this.tasks = Array.from(this.tasks.reverse());
  }

  private sortPage(actionNeededs: ActionNeededTask[]): ActionNeededTask[] {
    const iaPagePriority = {
      High: 1,
      Medium: 2,
      Low: 3
    };

    const iaPageOrder = {
      Housing: 1,
      Transportation: 2,
      'Legal Issues': 3,
      'Child and Youth Supports': 4,
      'Family Barriers': 5,
      'Non-Custodial Parents': 6,
      Other: 7
    };

    const sortedByPage = _.sortBy<ActionNeededTask>(actionNeededs, [
      function(o) {
        return iaPageOrder[o.pageName];
      },
      function(o) {
        if (o.dueDate != null && o.dueDate.trim() !== '') {
          return new Date(o.dueDate);
        } else {
          return null;
        }
      },
      function(o) {
        return iaPagePriority[o.priorityName];
      }
    ]);

    return sortedByPage;
  }

  private sortPriority(actionNeededs: ActionNeededTask[]): ActionNeededTask[] {
    const iaPagePriority = {
      High: 1,
      Medium: 2,
      Low: 3
    };

    const iaPageOrder = {
      Housing: 1,
      Transportation: 2,
      'Legal Issues': 3,
      'Child and Youth Supports': 4,
      'Family Barriers': 5,
      'Non-Custodial Parents': 6,
      Other: 7
    };

    const sortedByPriority = _.sortBy<ActionNeededTask>(actionNeededs, [
      function(o: any) {
        return iaPagePriority[o.priorityName];
      },
      function(o) {
        if (o.dueDate != null && o.dueDate.trim() !== '') {
          return new Date(o.dueDate);
        } else {
          return null;
        }
      },
      function(o) {
        return iaPageOrder[o.pageName];
      }
    ]);

    return sortedByPriority;
  }

  private sortCompletionDate(actionNeededs: ActionNeededTask[]): ActionNeededTask[] {
    const sortedByCompletionDate = _.orderBy<ActionNeededTask>(
      actionNeededs,
      [
        function(o: any) {
          if (o.completionDate != null && o.completionDate.trim() !== '' && o.isNoCompletionDate !== true) {
            return new Date(o.completionDate);
          } else if (o.isNoCompletionDate === true) {
            // Displayed last.
            return new Date(-8640000000000000);
          }
          // On going will be in the middle.
          return new Date(-8639999999999999);
        }
      ],
      ['asc']
    );

    return sortedByCompletionDate.reverse();
  }

  private sortDueDate(actionNeededs: ActionNeededTask[]): ActionNeededTask[] {
    const iaPagePriority = {
      High: 1,
      Medium: 2,
      Low: 3
    };

    const iaPageOrder = {
      Housing: 1,
      Transportation: 2,
      'Legal Issues': 3,
      'Child and Youth Supports': 4,
      'Family Barriers': 5,
      'Non-Custodial Parents': 6,
      Other: 7
    };

    const sortedByDueDate = _.sortBy<ActionNeededTask>(actionNeededs, [
      function(o: any) {
        if (o.dueDate != null && o.dueDate.trim() !== '' && o.isNoDueDate !== true) {
          return new Date(o.dueDate);
        } else if (o.isNoDueDate === true) {
          return null;
        } else {
          return new Date();
        }
      },
      function(o) {
        return iaPagePriority[o.priorityName];
      },
      function(o) {
        return iaPageOrder[o.pageName];
      }
    ]);

    return sortedByDueDate;
  }

  private setDefaultSort(): void {
    this.selectedSort = this.dueDateId;
    this.sortTasks();
  }
}
