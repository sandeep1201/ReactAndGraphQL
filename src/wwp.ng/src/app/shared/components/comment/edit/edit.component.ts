import { Router } from '@angular/router';
import { FieldDataTypes } from 'src/app/shared/enums/field-data-types.enum';
// tslint:disable: import-blacklist
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { IMultiSelectSettings } from '../../multi-select-dropdown/multi-select-dropdown.component';
import { ModelErrors } from 'src/app/shared/interfaces/model-errors';
import { ValidationManager } from 'src/app/shared/models/validation';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { AppService } from 'src/app/core/services/app.service';
import { PinCommentsService } from 'src/app/shared/services/pin-comments.service';
import { FieldDataService } from 'src/app/shared/services/field-data.service';
import { take, concatMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { Utilities } from 'src/app/shared/utilities';
import { CommentModel } from '../comment.model';
import { CommentType } from '../comment-type.model';
import { CommentsService } from '../comments.service';

enum CommentTypeEnum {
  PIN = 'PIN',
  EA = 'EA'
}

@Component({
  selector: 'app-comments-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss']
})
export class CommentsEditComponent implements OnInit {
  commentTypeSettings: IMultiSelectSettings = {
    pullRight: false,
    enableSearch: false,
    checkedStyle: 'checkboxes',
    buttonClasses: 'btn btn-default',
    selectionLimit: 0,
    closeOnSelect: false,
    showCheckAll: false,
    showUncheckAll: false,
    dynamicTitleMaxItems: 3,
    maxHeight: '160px'
  };
  @Output() isInEditMode = new EventEmitter<any>();
  public isSaving = false;
  public modelErrors: ModelErrors = {};
  public isLoaded = false;
  public isSectionValid = true;
  public isSectionModified = false;
  public hasTriedSave = false;
  public hadSaveError = false;
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public model: CommentModel;
  public cachedModel: CommentModel = new CommentModel();
  public commentTypesDrop: DropDownField[];
  public isReadOnly = false;
  private requestId: number;
  public commentModeType = CommentTypeEnum.PIN;

  constructor(
    private appService: AppService,
    private commentsService: CommentsService,
    private pinCommentsService: PinCommentsService,
    private fdService: FieldDataService,
    private router: Router
  ) {}

  ngOnInit() {
    if (this.commentsService.participantPin) {
      this.initCommentModel();
    }
  }

  initCommentModel() {
    this.commentsService.modeForComment
      .pipe(
        take(1),
        concatMap(res => {
          this.isReadOnly = res.readOnly;
          if (res.commentType === CommentTypeEnum.EA) {
            this.commentModeType = res.commentType;
            this.requestId = +this.router.url.split('/')[5];
          }
          return res.id > 0 ? (res.data ? of(res.data) : this.pinCommentsService.getPinComment(this.commentsService.participantPin, res.id)) : of(null);
        })
      )
      .subscribe(data => {
        this.model = data ? data : CommentModel.create();
        this.initCommentTypesDrop();
        CommentModel.clone(this.model, this.cachedModel);
      });
  }

  validate() {
    this.isSectionModified = true;
    if (this.hasTriedSave === true) {
      this.validationManager.resetErrors();
      const result = this.model.validate(this.validationManager);
      this.isSectionValid = result.isValid;
      this.modelErrors = result.errors;
      if (this.modelErrors) {
        this.isSaving = false;
      }
    }
  }

  selectCommentTypes(commentTypeIds) {
    this.model.commentTypeIds = commentTypeIds;
    this.model.commentTypes = new Array<CommentType>();

    this.model.commentTypeIds.forEach(id => {
      const commentType = new CommentType();
      let fieldId = +id;
      commentType.commentTypeId = +id;
      commentType.commentTypeName = Utilities.fieldDataNameById(fieldId++, this.commentTypesDrop);
      this.model.commentTypes.push(commentType);
    });
  }

  exitPCEditIgnoreChanges(e) {
    this.appService.isChildDialogPresent = false;
    this.commentsService.modeForComment.next({ readOnly: false, isInEditMode: false, commentType: null });
    this.isInEditMode.emit(false);
  }

  initCommentTypesDrop() {
    this.fdService
      .getFieldDataByField(this.commentModeType === CommentTypeEnum.EA ? FieldDataTypes.EACommentTypes : FieldDataTypes.PinCommentTypes)
      .pipe(take(1))
      .subscribe(res => {
        this.commentTypesDrop = res;
        this.isLoaded = true;
      });
  }

  public exit() {
    if (this.isSectionModified) {
      this.appService.isUrlChangeBlocked = false;
      this.appService.isDialogPresent = true;
      this.appService.isChildDialogPresent = true;
    } else {
      this.commentsService.modeForComment.next({ readOnly: false, isInEditMode: false, commentType: null });
      this.isInEditMode.emit(false);
    }
  }

  save() {
    if (this.isSectionValid) {
      this.isSaving = true;
      this.commentsService.saveComment(this.model, this.commentsService.participantPin, this.commentModeType, this.requestId).subscribe(
        res => {
          this.commentsService.modeForComment.next({ readOnly: false, isInEditMode: false, commentType: null });
          if (
            this.router.url.endsWith('edit/agency-summary') &&
            this.commentsService.modeOnSaveComment &&
            this.commentsService.modeOnSaveComment.value &&
            !this.commentsService.modeOnSaveComment.value.commentSaved
          )
            this.commentsService.modeOnSaveComment.next({ id: this.requestId, commentSaved: true });
          this.isSaving = false;
          this.isInEditMode.emit(false);
        },
        error => {
          this.isInEditMode.emit(true);
          this.isSaving = false;
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
