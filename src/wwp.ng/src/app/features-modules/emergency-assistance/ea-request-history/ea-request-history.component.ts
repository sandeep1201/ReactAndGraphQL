import { AppService } from './../../../core/services/app.service';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { concatMap, combineLatest } from 'rxjs/operators';
import { Participant } from 'src/app/shared/models/participant';
import { ActivatedRoute, Router } from '@angular/router';
import { EmergencyAssistanceService } from '../services/emergancy-assistance.service';
import { forkJoin, of } from 'rxjs';
import { SubSink } from 'subsink';
import { EARequest } from '../models';
import { Authorization } from 'src/app/shared/models/authorization';
import { EAIPV } from '../models/ea-ipv.model';
import { EAStatusCodes, EAIndividualType, EAIPVStatus } from '../models/ea-request-sections.enum';
import * as moment from 'moment';
import { Utilities } from 'src/app/shared/utilities';

@Component({
  selector: 'app-ea-request-history',
  templateUrl: './ea-request-history.component.html',
  styleUrls: ['./ea-request-history.component.scss']
})
export class EARequestHistoryComponent implements OnInit, OnDestroy {
  public isLoaded = false;
  public goBackUrl: string;
  public pin: string;
  public ipvId: number;
  public participant: Participant;
  public eaRequestListModel: EARequest[] = [];
  public ipvListModel: EAIPV[] = [];
  private requestSub = new SubSink();
  public isInEditMode = false;
  public showEditButtons = true;
  public canShowAddIPVButton = false;
  public canAddNewEA = false;
  public warningMessage = '';
  public showError = false;

  constructor(
    private route: ActivatedRoute,
    private partService: ParticipantService,
    private eaService: EmergencyAssistanceService,
    private router: Router,
    private appService: AppService
  ) {}

  ngOnInit() {
    this.pin = this.route.snapshot.params.pin;
    this.goBackUrl = '/pin/' + this.pin;

    this.requestSub.add(
      this.eaService.modeForIPVRequest
        .pipe(
          concatMap(res => {
            this.isInEditMode = res.isInEditMode;
            const result0 = !this.participant ? this.partService.getCachedParticipant(this.pin) : of(null);
            const result1 = !this.isInEditMode ? this.eaService.getEARequestList(this.pin) : of(null);
            const result2 = !this.isInEditMode ? this.eaService.getIPVList(this.pin) : of(null);
            const results = forkJoin(result0, result1, result2);

            return results;
          })
        )
        .subscribe(results => {
          if (results[0]) this.participant = results[0];
          if (results[1]) this.eaRequestListModel = results[1];
          if (results[2]) this.initIPVList(results[2]);

          this.showEditButtons = this.appService.isUserAuthorized(Authorization.canAccessEA_Edit, null);
          this.setAddIPVButtonStatus();
          this.setEAEligibilityWarningMessage();
          this.isLoaded = true;
        })
    );
  }

  initIPVList(result: EAIPV[]) {
    const statusCodesCanAllowEdit = [EAIPVStatus.Active, EAIPVStatus.Pending];
    this.ipvListModel = result;
    this.ipvListModel.forEach(i => {
      i.canView = this.appService.user.agencyCode === i.organizationCode || this.appService.isStateStaff;
      i.canEdit = i.canView && this.appService.isUserEASupervisor() && statusCodesCanAllowEdit.includes(i.statusCode);
    });
  }

  setEAEligibilityWarningMessage() {
    this.canAddNewEA = !this.eaRequestListModel.find(
      x =>
        (x.statusCode === EAStatusCodes.InProgress || x.statusCode === EAStatusCodes.Pending) &&
        x.eaGroupMembers.eaGroupMembers.some(
          y =>
            y.pinNumber === +this.participant.pin &&
            (y.eaIndividualTypeCode === EAIndividualType.CaretakerRelative || y.eaIndividualTypeCode === EAIndividualType.OtherCaretakerRelative)
        )
    );
    const approvedEAItem = this.eaRequestListModel.find(
      x =>
        !moment(x.eaDemographics.applicationDate).isSameOrBefore(Utilities.currentDate.clone().subtract(1, 'year')) &&
        x.statusCode === EAStatusCodes.Approved &&
        x.eaGroupMembers.eaGroupMembers.some(
          y =>
            y.pinNumber === +this.participant.pin &&
            (y.eaIndividualTypeCode === EAIndividualType.CaretakerRelative || y.eaIndividualTypeCode === EAIndividualType.OtherCaretakerRelative)
        )
    );
    const acticeIPVItem = this.ipvListModel.find(x => x.statusCode === EAIPVStatus.Active);

    this.warningMessage =
      !approvedEAItem && !acticeIPVItem
        ? 'Individual may be eligible for Emergency Assistance.'
        : !!approvedEAItem
        ? `Individual has received an Emergency Assistance payment within the last 12 months and is not eligible for another payment until ${moment(
            approvedEAItem.eaDemographics.applicationDate
          )
            .clone()
            .add(1, 'year')
            .format('MM/DD/YYYY')}.`
        : !!acticeIPVItem
        ? acticeIPVItem.penaltyEndDate
          ? `Individual has an Emergency Assistance IPV and is not eligible for an Emergency Assistance payment until ${moment(acticeIPVItem.displayPenaltyEndDate)
              .clone()
              .add(1, 'day')
              .format('MM/DD/YYYY')}.`
          : 'Individual has a permanent Emergency Assistance IPV and will never be eligible for an Emergency Assistance payment.'
        : '';
  }

  getIndividualType(model: EARequest) {
    return model.eaGroupMembers.eaGroupMembers.find(x => x.pinNumber === +this.participant.pin).eaIndividualTypeName;
  }

  onAdd() {
    if (this.canAddNewEA) this.router.navigateByUrl(`${this.goBackUrl}/emergency-assistance/ea-application-history/0/edit`);
    else {
      this.showError = true;
      window.scroll({ top: 0, left: 0, behavior: 'smooth' });
    }
  }

  single(r) {
    this.router.navigateByUrl(`${this.goBackUrl}/emergency-assistance/ea-application-history/${r.id}`, { state: { id: r.id, agency: r.organizationCode } });
  }

  setAddIPVButtonStatus() {
    const eaStatusCodesToConsiderForIPVAddButton = [EAStatusCodes.Approved, EAStatusCodes.Denied];
    const eaIndividualTypeCodesToConsiderForIPVAddButton = [EAIndividualType.CaretakerRelative, EAIndividualType.OtherCaretakerRelative];
    const ipvStatusCodesToConsiderForIPVAddButton = [EAIPVStatus.Pending, EAIPVStatus.Active];

    this.canShowAddIPVButton =
      this.appService.isUserEASupervisor() &&
      this.eaRequestListModel.some(
        i =>
          eaStatusCodesToConsiderForIPVAddButton.includes(i.statusCode) &&
          i.eaGroupMembers.eaGroupMembers.some(j => eaIndividualTypeCodesToConsiderForIPVAddButton.includes(j.eaIndividualTypeCode))
      ) &&
      !this.ipvListModel.some(i => ipvStatusCodesToConsiderForIPVAddButton.includes(i.statusCode));
  }

  onIPVAdd() {
    this.eaService.modeForIPVRequest.next({ isReadOnly: false, isInEditMode: true, ipvModel: EAIPV.create() });
  }

  onIpvSingleEntry($event) {
    this.eaService.modeForIPVRequest.next({ isReadOnly: $event.isReadOnly, isInEditMode: true, ipvModel: $event.ipvModel });
  }

  ngOnDestroy() {
    this.requestSub.unsubscribe();
  }
}
