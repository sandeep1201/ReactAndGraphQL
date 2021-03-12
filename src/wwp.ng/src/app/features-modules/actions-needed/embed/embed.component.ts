import { Component, OnInit, ComponentRef, forwardRef, Input, OnDestroy, Output, EventEmitter, ChangeDetectionStrategy } from '@angular/core';
import { NG_VALUE_ACCESSOR, ControlValueAccessor } from '@angular/forms';
import { ActionNeededEditComponent } from '../edit/edit.component';
import { Subscription } from 'rxjs';
import * as _ from 'lodash';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { ActionNeededTask, ActionNeeded } from 'src/app/features-modules/actions-needed/models/action-needed-new';
import { ActionNeededService } from 'src/app/features-modules/actions-needed/services/action-needed.service';
import { FieldDataService } from 'src/app/shared/services/field-data.service';
import { ModalService } from 'src/app/core/modal';
import { BaseComponent } from 'src/app/shared/components/base-component';
import { Utilities } from 'src/app/shared/utilities';

@Component({
  selector: 'app-action-needed-embed',
  templateUrl: './embed.component.html',
  styleUrls: ['./embed.component.css'],
  providers: [
    ActionNeededService,
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => ActionNeededEmbedComponent),
      multi: true
    }
  ]
})
export class ActionNeededEmbedComponent extends BaseComponent implements OnInit, ControlValueAccessor, OnDestroy {
  @Input() isActionErrored = false;
  @Input() isRequired = true;
  @Input() pageName: string;
  @Input() readOnly = false;

  get participantPin(): string {
    return this._participantPin;
  }

  @Input('participantPin')
  set participantPin(value: string) {
    this._participantPin = value;
    this.actionNeededService.setPin(value);
  }
  // tslint:disable-next-line:no-output-on-prefix
  @Output() onAreAllTasksCompleted = new EventEmitter<boolean>();
  public inConfirmDeleteView = false;
  public allPossibleActionNeededs: DropDownField[];
  public displayedActionNeeded: ActionNeededTask[] = [];
  public pinActionNeededUrl: string;
  private tempModalRef: ComponentRef<ActionNeededEditComponent>;

  private ansSub: Subscription;

  private anSub: Subscription;

  private asSub: Subscription;

  private actionNeededAssigneeDrop: DropDownField[];

  private deletedTask: ActionNeededTask;

  private workerAssigneeId = 0;

  private _participantPin: string;

  constructor(private actionNeededService: ActionNeededService, private fdService: FieldDataService, private modalService: ModalService) {
    super();
  }

  ngOnInit() {
    this.ansSub = this.fdService.getActionNeededByPage(this.pageName).subscribe(data => this.initActionNeededsDrop(data));
    this.asSub = this.fdService.getActionNeededAssignee().subscribe(data => this.initAssigneeDrop(data));
    // Anytime we have a pin, update the URL.
    if (this.participantPin != null) {
      this.pinActionNeededUrl = `/pin/${this.participantPin}/action-needed`;
    }
  }

  // get accessor
  get value(): ActionNeeded {
    return this.innerValue;
  }

  // set accessor including call the onchange callback
  set value(v: ActionNeeded) {
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
  writeValue(value: ActionNeeded) {
    if (value !== this.innerValue) {
      this.innerValue = value;
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

  public deleteTask(task: ActionNeededTask) {
    this.actionNeededService.deleteTask(task.id).subscribe(data => {
      (this.inConfirmDeleteView = false), (this.deletedTask = null), this.forceReload(this.pageName);
    });
  }
  onConfirmDelete() {
    this.deleteTask(this.deletedTask);
  }

  onCancelDelete() {
    this.inConfirmDeleteView = false;
  }

  tryDelete(task: ActionNeededTask) {
    this.deletedTask = task;
    this.inConfirmDeleteView = true;
  }

  public postActionNeeded() {
    this.actionNeededService.postActionNeeded(this.value).subscribe(data => {});
  }

  public editTask(task: ActionNeededTask) {
    if (this.tempModalRef && this.tempModalRef.instance) {
      this.tempModalRef.instance.destroy();
    }
    this.modalService.create<ActionNeededEditComponent>(ActionNeededEditComponent).subscribe(x => {
      this.tempModalRef = x;
      this.tempModalRef.instance.pin = this.actionNeededService.getPin();
      this.tempModalRef.instance.modelId = task.id;
      this.tempModalRef.hostView.onDestroy(() => this.forceReload(this.pageName));
    });
  }

  public AddTask() {
    if (this.tempModalRef && this.tempModalRef.instance) {
      this.tempModalRef.instance.destroy();
    }
    this.modalService.create<ActionNeededEditComponent>(ActionNeededEditComponent).subscribe(x => {
      this.tempModalRef = x;
      this.tempModalRef.instance.pin = this.actionNeededService.getPin();
      this.tempModalRef.hostView.onDestroy(() => this.forceReload(this.pageName));
    });
  }

  public quickAddTask(task: ActionNeededTask) {
    const tsk = new ActionNeededTask();
    tsk.assigneeId = this.workerAssigneeId;
    tsk.pageId = this.value.pageId;
    tsk.actionItemId = task.actionItemId;
    this.actionNeededService.postTask(tsk).subscribe(data => this.forceReload(this.pageName));
  }

  forceReload(pageCode: string) {
    this.anSub = this.actionNeededService.getActionNeeded(pageCode).subscribe(data => this.initReload(data));
  }

  initReload(data) {
    this.value = data;
    this.displayedActionNeeded = [];
    this.initActionNeededsDrop(this.allPossibleActionNeededs);
    this.onChangeCallback(this.innerValue);
    this.areAllTasksCompleted();
  }

  public isAnyTaskNotCompleted() {
    if (this.value == null || this.value == undefined) {
      return;
    }
    for (const tsk of this.value.tasks) {
      const tskStatus = tsk.getTaskStatus();
      if (tskStatus === 'onGoing') {
        return true;
      }
    }
    return false;
  }

  public areAllTasksCompleted() {
    if (this.isAnyTaskNotCompleted() === false) {
      this.onAreAllTasksCompleted.emit((this.value.isNoActionNeeded = true));
    }
  }

  private initActionNeededsDrop(data) {
    this.allPossibleActionNeededs = data;
  }

  private initAssigneeDrop(data) {
    this.actionNeededAssigneeDrop = data;
    this.workerAssigneeId = Utilities.idByFieldDataName('Worker', this.actionNeededAssigneeDrop);
  }

  ngOnDestroy() {
    if (this.ansSub != null) {
      this.ansSub.unsubscribe();
    }
    if (this.anSub != null) {
      this.anSub.unsubscribe();
    }
    if (this.asSub != null) {
      this.asSub.unsubscribe();
    }
  }
}
