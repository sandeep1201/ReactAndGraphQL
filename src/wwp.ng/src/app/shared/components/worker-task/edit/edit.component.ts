import { FieldDataTypes } from './../../../enums/field-data-types.enum';
import { FieldDataService } from 'src/app/shared/services/field-data.service';
import { WorkerTaskService } from './../worker-task.service';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ModelErrors } from 'src/app/shared/interfaces/model-errors';
import { ValidationManager } from 'src/app/shared/models/validation';
import { AppService } from 'src/app/core/services/app.service';
import { take, concatMap } from 'rxjs/operators';
import { of, forkJoin } from 'rxjs';
import { WorkerTask } from 'src/app/features-modules/worker-task/models/worker-task.model';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { Participant } from 'src/app/shared/models/participant';

@Component({
  selector: 'app-worker-task-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss']
})
export class WorkerTaskEditComponent implements OnInit {
  @Output() isInEditMode = new EventEmitter<any>();
  public isSaving = false;
  public modelErrors: ModelErrors = {};
  public isLoaded = false;
  public isSectionValid = true;
  public isSectionModified = false;
  public hasTriedSave = false;
  public hadSaveError = false;
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public isReadOnly = false;
  public model = new WorkerTask();
  public cachedModel = new WorkerTask();
  categoriesDrop: DropDownField[] = [];
  statusDrop: DropDownField[] = [];
  prioritiesDrop: DropDownField[] = [];
  participant: Participant;

  constructor(private appService: AppService, private workerTaskService: WorkerTaskService, private fdService: FieldDataService) {}

  ngOnInit() {
    this.workerTaskService.modeForWorkerTask
      .pipe(
        take(1),
        concatMap(res => {
          this.isReadOnly = res.readOnly;
          this.participant = res.participant;
          return forkJoin(
            of(res.data ? res.data : WorkerTask.create(this.participant.id)),
            this.fdService.getFieldDataByField(FieldDataTypes.WorkerTaskCategories),
            this.fdService.getFieldDataByField(FieldDataTypes.WorkerTaskStatus),
            this.fdService.getFieldDataByField(FieldDataTypes.WorkerTaskPriorities)
          );
        })
      )
      .pipe(take(1))
      .subscribe(results => {
        this.model = results[0];
        WorkerTask.clone(results[0], this.cachedModel);
        this.categoriesDrop = results[1];
        this.statusDrop = results[2].filter(x => x.code !== 'CL');
        this.prioritiesDrop = results[3];
        this.isLoaded = true;
      });
  }

  validate() {
    this.isSectionModified = true;
    if (this.hasTriedSave) {
      this.validationManager.resetErrors();
      const result = this.model.validate(this.validationManager, this.cachedModel);
      this.isSectionValid = result.isValid;
      this.modelErrors = result.errors;
      if (this.isSectionValid) this.hasTriedSave = false;
      if (this.modelErrors) this.isSaving = false;
    }
  }

  exitWTEditIgnoreChanges(e) {
    this.appService.isChildDialogPresent = false;
    this.workerTaskService.modeForWorkerTask.next({ readOnly: false, isInEditMode: false, participant: null, data: null });
    this.isInEditMode.emit(false);
  }

  public exit() {
    if (this.isSectionModified) {
      this.appService.isUrlChangeBlocked = false;
      this.appService.isDialogPresent = true;
      this.appService.isChildDialogPresent = true;
    } else {
      this.workerTaskService.modeForWorkerTask.next({ readOnly: false, isInEditMode: false, participant: null, data: null });
      this.isInEditMode.emit(false);
    }
  }

  save() {
    if (this.isSectionValid) {
      this.hadSaveError = false;
      this.isSaving = true;
      this.workerTaskService.saveWorkerTask(this.model).subscribe(
        res => {
          this.workerTaskService.modeForWorkerTask.next({ readOnly: false, isInEditMode: false, participant: null, data: null });
          this.isSaving = false;
          this.isInEditMode.emit(false);
        },
        error => {
          this.isInEditMode.emit(true);
          this.isSaving = false;
          this.hadSaveError = false;
        }
      );
    }
  }

  saveAndExit() {
    this.hasTriedSave = true;
    this.validate();
    this.save();
  }
}
