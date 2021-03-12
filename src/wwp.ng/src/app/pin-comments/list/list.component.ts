import { Component, OnInit, Input } from '@angular/core';
import { Participant } from '../../shared/models/participant';
import { PinCommentsService } from '../../shared/services/pin-comments.service';
import * as moment from 'moment';
import { SystemClockService } from '../../shared/services/system-clock.service';
import { AppService } from '../../core/services/app.service';
import { AccessType } from '../../shared/enums/access-type.enum';
import * as _ from 'lodash';
import { DropDownField } from '../../shared/models/dropdown-field';
import { CommentModel } from 'src/app/shared/components/comment/comment.model';
import { CommentsService } from 'src/app/shared/components/comment/comments.service';

@Component({
  selector: 'app-pin-comments-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class PinCommentsListComponent implements OnInit {
  @Input() pin: string;
  @Input() isReadOnly = false;

  @Input() participant: Participant;

  public isInEditMode = false;
  public pinComments: CommentModel[] = [];
  public originalPinComments: CommentModel[] = [];
  public localPinComments: CommentModel[] = [];
  public isLoaded = false;
  public canAdd = true;
  public canView = true;
  public canEdit = false;
  public pinCommentId: number;
  public commentTypeDrop: DropDownField[] = [];
  private origCommentTypeDrop: DropDownField[] = [];
  public workerDrop: DropDownField[] = [];
  private origWorkerDrop: DropDownField[] = [];
  public commentTypes = [];
  public workers = [];
  public fromDate: string;
  public toDate: string;
  private dateFiltered = false;
  public goDisableFlag = true;
  constructor(public pinCommentsService: PinCommentsService, private appService: AppService, private commentService: CommentsService) {}

  ngOnInit() {
    this.commentService.modeForComment.subscribe(res => {
      this.pinCommentId = res.id;
      this.isInEditMode = res.isInEditMode;
      if (!this.isInEditMode) {
        this.getAllPinCommentsForPin();
      }
    });
  }

  getAllPinCommentsForPin() {
    this.pinCommentsService.getAllPinCommentsForPin(this.pin).subscribe(res => {
      this.pinComments = res;
      this.originalPinComments = res;
      this.initCommentTypes();
      this.origCommentTypeDrop = this.commentTypeDrop;
      this.initWorkers();
      this.origWorkerDrop = this.workerDrop;
      this.isLoaded = true;
    });
  }

  onAdd() {
    this.isInEditMode = true;
    this.pinCommentId = 0;
    this.commentService.modeForComment.next({ id: 0, readOnly: false, isInEditMode: this.isInEditMode });
  }

  edit(a, readOnly) {
    this.isInEditMode = true;
    this.pinCommentId = a.id;
    this.commentService.modeForComment.next({ id: a.id, readOnly: readOnly, isInEditMode: this.isInEditMode });
  }

  isEditable(comment: CommentModel, wiuid): boolean {
    let canEdit = false;
    if (comment.commentTypes.some(i => i.isSystemUseOnly)) canEdit = false;
    else {
      const formattedCreatedDate = moment(comment.createdDate).format('MM/DD/YYYY');
      if (
        SystemClockService.appDateTime.format('MM/DD/YYYY') === formattedCreatedDate &&
        this.appService.user.wiuid === wiuid &&
        this.appService.coreAccessContext.evaluate() === AccessType.edit
      )
        canEdit = true;
      else canEdit = false;
    }

    return canEdit;
  }

  initCommentTypes() {
    if (this.pinComments) {
      const commentTypeNames: DropDownField[] = [];
      const isFiltered = (this.workers && this.workers.length > 0) || this.dateFiltered;
      const pinComments = isFiltered ? this.pinComments : this.originalPinComments;
      pinComments.forEach(i => {
        if (i.commentTypes)
          i.commentTypes.forEach(j => {
            const ctdd = new DropDownField();
            ctdd.id = j.commentTypeId;
            ctdd.name = j.commentTypeName;
            commentTypeNames.push(ctdd);
          });
      });
      this.commentTypeDrop = _.orderBy<DropDownField>(
        _.uniqBy(commentTypeNames, 'id'),
        [
          function(o) {
            return o.name;
          }
        ],
        ['asc']
      );
    }

    this.commentTypes = this.commentTypeDrop && this.commentTypeDrop.length > 0 ? this.commentTypes : [];
  }

  initWorkers() {
    if (this.pinComments) {
      const workers: DropDownField[] = [];
      const isFiltered = (this.commentTypes && this.commentTypes.length > 0) || this.dateFiltered;
      const pinComments = isFiltered ? this.pinComments : this.originalPinComments;
      pinComments.forEach(i => {
        const wdd = new DropDownField();
        wdd.id = i.wiuid;
        wdd.name = i.modifiedBy;
        workers.push(wdd);
      });
      this.workerDrop = _.orderBy<DropDownField>(
        _.uniqBy(workers, 'id'),
        [
          function(o) {
            return o.name;
          }
        ],
        ['asc']
      );
    }

    this.workers = this.workerDrop && this.workerDrop.length > 0 ? this.workers : [];
  }

  filter(isDate = false, showNullWorker = false) {
    let commentTypes: any[];
    let workers: any[];
    if (this.commentTypes) commentTypes = this.commentTypes.length > 0 ? this.commentTypes : this.origCommentTypeDrop.map(i => i.id);
    if (this.workers) workers = this.workers.length > 0 ? this.workers : this.origWorkerDrop.map(i => i.id);
    if (this.originalPinComments && commentTypes && workers)
      this.pinComments = this.originalPinComments.filter(
        i => i.commentTypes && i.commentTypes.find(j => commentTypes.indexOf(j.commentTypeId) > -1) && (workers.indexOf(i.wiuid) > -1 || (showNullWorker && !i.wiuid))
      );

    if (isDate || this.dateFiltered) this.dateFilter();

    this.initCommentTypes();
    this.initWorkers();
  }

  dateFilter() {
    let fromDate: any;
    let toDate: any;
    const sorted = _.orderBy<CommentModel>(
      this.originalPinComments,
      [
        function(o) {
          return new Date(o.modifiedDate);
        }
      ],
      ['asc']
    );

    if (sorted) {
      fromDate = this.fromDate && this.fromDate.trim() !== '' ? moment(new Date(this.fromDate).toDateString()) : moment(new Date(sorted[0].modifiedDate).toDateString());
      toDate = this.toDate && this.toDate.trim() !== '' ? moment(new Date(this.toDate).toDateString()) : moment(new Date(sorted[sorted.length - 1].modifiedDate).toDateString());
    }

    this.pinComments = this.pinComments.filter(
      i => i.modifiedDate && moment(new Date(i.modifiedDate).toDateString()).isSameOrAfter(fromDate) && moment(new Date(i.modifiedDate).toDateString()).isSameOrBefore(toDate)
    );

    this.goDisableFlag = true;
    this.dateFiltered = true;
  }

  resetFilter() {
    this.pinComments = this.originalPinComments;
    this.commentTypeDrop = this.origCommentTypeDrop;
    this.workerDrop = this.origWorkerDrop;
    this.commentTypes = [];
    this.workers = [];
    this.fromDate = null;
    this.toDate = null;
    this.dateFiltered = false;
  }

  setGoFlag() {
    const fromDate = moment(this.fromDate, 'MM/DD/YYYY');
    const toDate = moment(this.toDate, 'MM/DD/YYYY');
    const currentDate = moment(moment().format('MM/DD/YYYY'), 'MM/DD/YYYY');
    const toBeforeFrom = toDate.isBefore(fromDate);
    let isFromValid = false;
    let isToValid = false;
    if (this.fromDate && this.fromDate.trim() !== '' && this.fromDate.length === 10 && fromDate.isValid() && fromDate.isSameOrBefore(currentDate)) isFromValid = true;
    if (this.toDate && this.toDate.trim() !== '' && this.toDate.length === 10 && toDate.isValid() && toDate.isSameOrBefore(currentDate)) isToValid = true;
    this.goDisableFlag = !isFromValid || !isToValid || toBeforeFrom;
  }

  closeEdit(e: boolean) {
    this.isInEditMode = e;
  }
}
