import { Component, OnInit, OnDestroy, Input, ComponentRef, Output, EventEmitter, HostListener } from '@angular/core';
import { ActionNeededEditComponent } from '../edit/edit.component';
import { ModalService } from 'src/app/core/modal/modal.service';
import { AccessType } from 'src/app/shared/enums/access-type.enum';
import { ActionNeededTask } from 'src/app/features-modules/actions-needed/models/action-needed-new';
import { ActionNeededService } from 'src/app/features-modules/actions-needed/services/action-needed.service';

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.css']
})
export class ActionNeededTaskComponent implements OnInit {
  shiftKey: boolean;

  @Output() reload = new EventEmitter<boolean>();
  public inConfirmDeleteView = false;
  public isLoading = false;

  private modalServiceBase: ModalService;

  private tempModalRef: ComponentRef<ActionNeededEditComponent>;

  @Input() isReadOnly = true;
  @Input() accessType: AccessType;
  AccessType = AccessType;
  @Input() task: ActionNeededTask;

  constructor(private actionNeededService: ActionNeededService, modalService: ModalService) {
    this.modalServiceBase = modalService;
  }

  ngOnInit() {}

  @HostListener('window:keydown', ['$event'])
  @HostListener('window:keyup', ['$event'])
  keyboardInput(event: KeyboardEvent) {
    this.shiftKey = event.shiftKey;
  }

  /**
   *  We use this method for the quick actions on each task. Completion Date being set marks task
   *  as completed.
   *
   * @memberof ActionNeededTaskComponent
   */
  public changeState() {
    if (this.isReadOnly === true) {
      return;
    }

    if (this.accessType !== AccessType.edit) {
      return;
    }

    if (this.shiftKey === true) {
      this.isLoading = true;
      // State is not-completed thus nulled out.
      this.task.completionDate = null;
      this.task.isNoCompletionDate = true;
      this.actionNeededService.postTask(this.task).subscribe(data => this.initTask(data));
    } else {
      if (this.task.completionDate != null && this.task.completionDate.trim() !== '') {
        return;
      }
      this.isLoading = true;
      // State is completed thus marked with current date.
      this.task.completionDate = new Date().toISOString();
      this.actionNeededService.postTask(this.task).subscribe(data => this.initTask(data));
    }
  }

  public deleteTask() {
    this.actionNeededService.deleteTask(this.task.id).subscribe(data => {
      (this.inConfirmDeleteView = false), this.forceReload();
    });
  }

  onConfirmDelete() {
    this.deleteTask();
  }

  onCancelDelete() {
    this.inConfirmDeleteView = false;
  }

  tryDelete() {
    this.inConfirmDeleteView = true;
  }
  initTask(data) {
    this.task = data;
    this.isLoading = false;
  }

  forceReload() {
    this.reload.emit(true);
  }

  public editTask() {
    if (this.tempModalRef && this.tempModalRef.instance) {
      this.tempModalRef.instance.destroy();
    }
    this.modalServiceBase.create<ActionNeededEditComponent>(ActionNeededEditComponent).subscribe(x => {
      this.tempModalRef = x;
      this.tempModalRef.instance.pin = this.actionNeededService.getPin();
      this.tempModalRef.instance.modelId = this.task.id;
      this.tempModalRef.hostView.onDestroy(() => this.forceReload());
    });
  }
}
