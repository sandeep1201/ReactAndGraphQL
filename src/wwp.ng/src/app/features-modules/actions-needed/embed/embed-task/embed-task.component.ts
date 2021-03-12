import { Component, OnInit, forwardRef, Input, EventEmitter, Output, HostListener } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import * as _ from 'lodash';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { ActionNeededTask } from 'src/app/features-modules/actions-needed/models/action-needed-new';
import { ActionNeededService } from 'src/app/features-modules/actions-needed/services/action-needed.service';

@Component({
  selector: 'app-action-needed-embed-task',
  templateUrl: './embed-task.component.html',
  styleUrls: ['./embed-task.component.css'],
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ActionNeededEmbedTaskComponent),
      multi: true
    }
  ]
})
export class ActionNeededEmbedTaskComponent extends BaseComponent implements OnInit, ControlValueAccessor {
  @Output() edit = new EventEmitter<ActionNeededTask>();
  @Output() delete = new EventEmitter<ActionNeededTask>();
  @Output() quickAdd = new EventEmitter<ActionNeededTask>();
  @Input() taskItemName: string;
  @Input() taskId = 0;
  @Input() readOnly = false;
  @Output() updated = new EventEmitter<Boolean>();

  public isLoading = false;
  private shiftKey: boolean;
  constructor(private actionNeededService: ActionNeededService) {
    super();
  }

  ngOnInit() {}

  // get accessor
  get value(): ActionNeededTask[] {
    return this.innerValue;
  }

  // set accessor including call the onchange callback
  set value(v: ActionNeededTask[]) {
    if (v !== this.innerValue) {
      this.innerValue = v;
      this.onChangeCallback(v);
    }
  }

  // Set touched on blur
  onBlur() {
    this.onTouchedCallback();
  }

  // From ControlValueAccessor interface
  writeValue(value: ActionNeededTask[]) {
    if (value !== this.innerValue) {
      this.innerValue = [];
      for (const v of value) {
        if (v.actionItemName === this.taskItemName && (v.isRecentTask() === true || v.getTaskStatus() === 'onGoing')) {
          this.taskId = v.actionItemId;
          this.innerValue.push(v);
        }
      }
    }
  }

  // From ControlValueAccessor interface
  registerOnChange(fn: any) {
    this.onChangeCallback = fn;
  }

  // From ControlValueAccessor interface
  registerOnTouched(fn: any) {
    this.onTouchedCallback = fn;
  }

  public tryDelete(task: ActionNeededTask) {
    this.delete.emit(task);
  }
  public editTask(task: ActionNeededTask) {
    this.edit.emit(task);
  }

  public quickAddTask() {
    const t = new ActionNeededTask();
    t.actionItemId = this.taskId;
    this.quickAdd.emit(t);
  }

  @HostListener('window:keydown', ['$event'])
  @HostListener('window:keyup', ['$event'])
  keyboardInput(event: KeyboardEvent) {
    this.shiftKey = event.shiftKey;
  }

  /**
   *  We use this method for the quick actions on each task. Completion Date being set marks task
   *  as completed.
   *
   * @memberof ActionNeededEmbedTaskComponent
   */
  public changeState(task: ActionNeededTask) {
    if (this.shiftKey === true) {
      this.isLoading = true;
      // State is not-completed thus nulled out.
      task.completionDate = null;
      task.isNoCompletionDate = true;
      this.actionNeededService.postTask(task).subscribe(data => this.updateTask(data));
    } else {
      if (task.completionDate != null && task.completionDate.trim() !== '') {
        return;
      }
      this.isLoading = true;
      // State is completed thus marked with current date.
      task.completionDate = new Date().toISOString();
      this.actionNeededService.postTask(task).subscribe(data => this.updateTask(data));
    }
  }

  updateTask(data: ActionNeededTask) {
    let found = _.find(this.value, ['id', data.id]);
    if (found != null) {
      found = data;
    }
    this.isLoading = false;
    this.updated.emit(true);
  }
}
