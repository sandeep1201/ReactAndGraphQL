import { EAViewModes } from './../../models/ea-request-sections.enum';
import { Utilities } from './../../../../shared/utilities';
import { FieldDataTypes } from './../../../../shared/enums/field-data-types.enum';
import { FieldDataService } from 'src/app/shared/services/field-data.service';
import { AppService } from 'src/app/core/services/app.service';
import { SubSink } from 'subsink';
import { Participant } from './../../../../shared/models/participant';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { EmergencyAssistanceService } from '../../services/emergancy-assistance.service';
import { ActivatedRoute, Router } from '@angular/router';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { concatMap } from 'rxjs/operators';
import { forkJoin, of, combineLatest } from 'rxjs';
import * as moment from 'moment';
import * as _ from 'lodash';
import { SystemClockService } from 'src/app/shared/services/system-clock.service';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { EARequest } from '../../models';
import { CommentModel } from 'src/app/shared/components/comment/comment.model';
import { CommentsService } from 'src/app/shared/components/comment/comments.service';
import { Authorization } from 'src/app/shared/models/authorization';
import { EAPayment } from '../../models/ea-request-payment.model';
import { EAStatusCodes } from '../../models/ea-request-sections.enum';

@Component({
  selector: 'app-ea-request-details',
  templateUrl: './single.component.html',
  styleUrls: ['./single.component.scss']
})
export class EARequestDetailsComponent implements OnInit, OnDestroy {
  public isLoaded = false;
  public model: EARequest;
  public filteredComments: CommentModel[] = [];
  public isInEditMode = false;
  public isPaymentMode = false;
  public goBackUrl: string;
  public showEdit = false;
  public participant: Participant;
  private requestSub = new SubSink();
  public isCommentsCollapsed = false;
  public originalEaCommentTypesDrop: DropDownField[] = [];
  public eaCommentTypesDrop: DropDownField[] = [];
  public commentTypes: number[] = [];
  public showEditButtons = true;
  public showApplicationEditButtons = false;
  public hasReadOnlyAccess = false;
  public totalPayment: string;
  public isNewPaymentsAllowed = true;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private appService: AppService,
    private partService: ParticipantService,
    private eaService: EmergencyAssistanceService,
    private fdService: FieldDataService,
    private commentsService: CommentsService
  ) {}

  ngOnInit() {
    this.showEdit = true;
    const id = this.route.snapshot.params.id;
    const pin = this.route.snapshot.params.pin;
    this.goBackUrl = `/pin/${pin}/emergency-assistance/ea-application-history`;

    this.requestSub.add(
      combineLatest(this.commentsService.modeForComment, this.eaService.modeForEAPayment)
        .pipe(
          concatMap(res => {
            this.isInEditMode = res[0].isInEditMode;
            this.isPaymentMode = res[1].isInEditMode;
            const result0 = !this.participant ? this.partService.getCachedParticipant(pin) : of(null);
            const result1 = !this.isInEditMode ? this.eaService.getEARequest(pin, id) : of(null);
            const result2 = this.originalEaCommentTypesDrop.length === 0 ? this.fdService.getFieldDataByField(FieldDataTypes.EACommentTypes) : of(null);
            const results = forkJoin(result0, result1, result2);

            return results;
          })
        )
        .subscribe(results => {
          if (results[0]) this.participant = results[0];
          if (results[1]) this.model = results[1];
          if (results[2]) this.originalEaCommentTypesDrop = results[2];
          this.calculateTotal(this.model.eaPayments);
          this.initCommentDrop();
          this.filterComment();
          this.showApplicationEditButtons =
            (this.appService.isUserEASupervisor() || (this.appService.isUserEAWorker() && [EAStatusCodes.InProgress, EAStatusCodes.Pending].includes(this.model.statusCode))) &&
            !moment(this.model.eaDemographics.applicationDate).isSameOrBefore(Utilities.currentDate.clone().subtract(1, 'year'));
          this.showEditButtons = this.appService.isUserAuthorized(Authorization.canAccessEA_Edit, null) && this.model.organizationCode === this.appService.user.agencyCode;
          this.hasReadOnlyAccess = this.showEditButtons || this.appService.isUserDCFMonitoring();
          this.isLoaded = true;
        })
    );
  }

  edit(viewMode: EAViewModes) {
    if (viewMode === EAViewModes.View && !this.hasReadOnlyAccess) return;
    this.router.navigateByUrl(`${this.goBackUrl}/${this.model.id}/${viewMode}`, { state: { id: this.model.id, agency: this.model.organizationCode } });
  }

  editGroup(viewMode: EAViewModes) {
    if (viewMode === EAViewModes.View && !this.hasReadOnlyAccess) return;
    this.router.navigateByUrl(`${this.goBackUrl}/${this.model.id}/${viewMode}/household-members`, { state: { id: this.model.id, agency: this.model.organizationCode } });
  }

  calculateTotal(payments: EAPayment[]) {
    let total = 0;
    if (payments) {
      payments.forEach(x => {
        if (x.voucherOrCheckAmount && x.voucherOrCheckAmount !== '') {
          total += Utilities.currencyToNumber(x.voucherOrCheckAmount);
        }
      });
    }
    this.totalPayment = total.toLocaleString('en-US', { minimumFractionDigits: 2 });
    this.isNewPaymentsAllowed =
      this.model.statusCode === EAStatusCodes.Approved && Utilities.currencyToNumber(this.totalPayment) < Utilities.currencyToNumber(this.model.approvedPaymentAmount);
  }

  editComment(comment?) {
    this.isInEditMode = true;
    if (comment) this.commentsService.modeForComment.next({ id: comment.id, readOnly: false, isInEditMode: this.isInEditMode, commentType: 'EA', data: comment });
    else this.commentsService.modeForComment.next({ id: 0, readOnly: false, isInEditMode: this.isInEditMode, commentType: 'EA' });
  }

  editPayment(comment?, readOnly = false) {
    this.isPaymentMode = true;
    if (comment && !readOnly) this.eaService.modeForEAPayment.next({ readOnly: readOnly, isInEditMode: true, data: comment });
    else if (comment && readOnly) this.eaService.modeForEAPayment.next({ readOnly: readOnly, isInEditMode: true, data: comment });
    else this.eaService.modeForEAPayment.next({ readOnly: readOnly, isInEditMode: true });
  }

  initCommentDrop() {
    const filteredCommentTypeIds: number[] = [];
    this.model.eaComments.forEach(x =>
      x.commentTypeIds.forEach(y => (filteredCommentTypeIds.length > 0 && filteredCommentTypeIds.includes(y) ? null : filteredCommentTypeIds.push(y)))
    );
    this.eaCommentTypesDrop = this.originalEaCommentTypesDrop.filter(x => filteredCommentTypeIds.indexOf(x.id) > -1);
  }

  filterComment() {
    let commentTypes: any[];
    this.filteredComments = [];
    if (this.commentTypes) commentTypes = this.commentTypes.length > 0 ? this.commentTypes : this.eaCommentTypesDrop.map(i => i.id);
    this.filteredComments = this.model.eaComments.filter(i => i.commentTypeIds && i.commentTypeIds.find(j => commentTypes.indexOf(j) > -1));
  }

  isEditableComment(comment: CommentModel): boolean {
    const formattedCreatedDate = moment(comment.createdDate).format('MM/DD/YYYY');
    return SystemClockService.appDateTime.format('MM/DD/YYYY') === formattedCreatedDate && this.appService.user.wiuid === comment.wiuid;
  }

  ngOnDestroy() {
    if (this.requestSub) this.requestSub.unsubscribe();
  }
}
