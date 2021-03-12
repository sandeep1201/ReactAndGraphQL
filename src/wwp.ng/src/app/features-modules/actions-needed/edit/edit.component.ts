import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { Observable, Subscription, forkJoin } from 'rxjs';

import { DestroyableComponent } from 'src/app/core/modal/index';

import * as _ from 'lodash';
import { take } from 'rxjs/operators';
import { ActionNeededTask } from 'src/app/features-modules/actions-needed/models/action-needed-new';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { Participant } from 'src/app/shared/models/participant';
import { ValidationManager } from 'src/app/shared/models/validation';
import { AppService } from 'src/app/core/services/app.service';
import { FieldDataService } from 'src/app/shared/services/field-data.service';
import { ActionNeededService } from 'src/app/features-modules/actions-needed/services/action-needed.service';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { ModelErrors } from 'src/app/shared/interfaces/model-errors';

@Component({
  selector: 'app-action-needed-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css']
})
export class ActionNeededEditComponent implements OnInit, DestroyableComponent, OnDestroy {
  @Input() modelId = 0;
  @Input() pin: string;
  public model: ActionNeededTask;
  public cachedModel: ActionNeededTask;
  public actionNeededAssigneeDrop: DropDownField[];
  public actionItemsDrop: DropDownField[];
  public prioritiesDrop: DropDownField[];
  public actionNeededPagesDrop: DropDownField[];
  public isSaving = false;
  public hadSaveError = false;
  public modelErrors: ModelErrors = {}; // Note: this must be initialized to an empty object for the UI in initial state.
  public title = 'New Task';
  public isSaveAble = false;
  private allActionItems: DropDownField[];
  private participant: Participant;
  private asSub: Subscription;
  private aiSub: Subscription;
  private prSub: Subscription;
  private aPSub: Subscription;
  private modelSub: Subscription;
  private partSub: Subscription;
  private validationManager: ValidationManager = new ValidationManager(this.appService);

  // private debouncedIsSaveEnabled: Function = () => { };
  public destroy: Function = () => {};
  public closeDialog: Function = () => {};

  constructor(private appService: AppService, private fdService: FieldDataService, private acService: ActionNeededService, private partService: ParticipantService) {
    //this.debouncedIsSaveEnabled = _.debounce(this.isSaveEnabled, 100, { 'maxWait': 100 });
  }

  ngOnInit() {
    this.acService.setPin(this.pin);

    forkJoin(
      this.partService.getParticipant(this.pin).pipe(take(1)),
      this.fdService.getActionNeededAssignee().pipe(take(1)),
      this.fdService.getActionNeededPages().pipe(take(1)),
      this.fdService.getAllActionNeeded().pipe(take(1)),
      this.fdService.getActionNeededPriorities().pipe(take(1))
    )
      .pipe(take(1))
      .subscribe(results => {
        this.initParticipant(results[0]);
        this.initAssigneeDrop(results[1]);
        this.initActionNeededPages(results[2]);
        this.initAllActionItemsDrop(results[3]);
        this.initPrioritiesDrop(results[4]);
        this.loadModel();
      });
  }

  public validate() {
    this.isSaveAble = false;
    if (this.participant == null || this.actionNeededPagesDrop == null) {
      return false;
    }

    this.validationManager.resetErrors();

    const result = this.model.validate(this.validationManager, this.actionNeededPagesDrop, this.actionItemsDrop, this.participant);

    // Update our properties so the UI can bind to the resultss.
    this.modelErrors = result.errors;

    this.isSaveAble = result.isValid;
  }

  public initPageSpecificActionsItems() {
    for (const x of this.allActionItems) {
      if (+this.model.pageId === x.id) {
        this.actionItemsDrop = x.subTypes;
      }
    }
  }

  private loadModel() {
    if (this.modelId > 0) {
      this.title = 'Edit Task';
      this.modelSub = this.acService.getTask(this.modelId).subscribe(data => this.initModel(data));
    } else {
      this.model = ActionNeededTask.create();
      this.cachedModel = new ActionNeededTask();
      ActionNeededTask.clone(this.model, this.cachedModel);
      this.validate();
    }
  }

  private initModel(data) {
    this.model = data;
    this.initPageSpecificActionsItems();
    this.cachedModel = new ActionNeededTask();
    ActionNeededTask.clone(this.model, this.cachedModel);
    this.validate();
  }

  private initAssigneeDrop(data) {
    this.actionNeededAssigneeDrop = data;
  }

  private initAllActionItemsDrop(data) {
    this.allActionItems = data;
  }

  private initPrioritiesDrop(data) {
    this.prioritiesDrop = data;
  }
  private initActionNeededPages(data) {
    this.actionNeededPagesDrop = data;
  }

  private initParticipant(data: Participant) {
    this.participant = data;
  }

  ngOnDestroy() {
    if (this.asSub != null) {
      this.asSub.unsubscribe();
    }
    if (this.aiSub != null) {
      this.aiSub.unsubscribe();
    }
    if (this.modelSub != null) {
      this.modelSub.unsubscribe();
    }
    if (this.aPSub != null) {
      this.aPSub.unsubscribe();
    }
    if (this.prSub != null) {
      this.prSub.unsubscribe();
    }
    if (this.partSub != null) {
      this.partSub.unsubscribe();
    }
  }

  saveAndExit() {
    if (this.isSaveAble !== true) {
      return;
    }

    this.isSaving = true;
    this.hadSaveError = false;

    if (this.modelSub != null) {
      this.modelSub.unsubscribe();
      this.modelSub = null;
    }

    // Call the service to save the data.
    this.modelSub = this.acService.postTask(this.model).subscribe(
      data => {
        this.exit();
      },
      error => {
        this.isSaving = false;
        this.hadSaveError = true;
      }
    );
  }

  onisNoCompletionDate(e) {
    return e;
  }

  exit() {
    this.closeDialog();
    this.destroy();
  }
}
