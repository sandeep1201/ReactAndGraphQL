import { FieldDataTypes } from 'src/app/shared/enums/field-data-types.enum';
import { Component, OnInit } from '@angular/core';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { FieldDataService } from 'src/app/shared/services/field-data.service';
import { PaymentDetailsService } from './services/payment-details.service';
import { Utilities } from 'src/app/shared/utilities';
import { PaymentDetails, ParticipantPaymentHistory } from './models/payment-details.model';
import { ActivatedRoute } from '@angular/router';
import { forkJoin } from 'rxjs';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { concatMap, take } from 'rxjs/operators';
import { Participant } from 'src/app/shared/models/participant';
import { ReportService } from 'src/app/shared/services/report.service';
import * as _ from 'lodash';

@Component({
  selector: 'app-payment-details',
  templateUrl: './payment-details.component.html',
  styleUrls: ['./payment-details.component.scss']
})
export class PaymentDetailsComponent implements OnInit {
  public isLoaded = false;
  public goBackUrl: string;
  public participationPeriodDrop: DropDownField[] = [];
  public yearsDrop: DropDownField[] = [];
  public caseNumberDrop: DropDownField[] = [];
  private caseNumbersString: string;
  public paymentDetailsModel: PaymentDetails;
  public participationPeriodId: number;
  public periodYear: number;
  public pin: string;
  public participant: Participant;
  public isPaymentdetailsExists = false;
  public isWeeklyPaymentAvailable = false;
  public showBatchDetailsButton = false;
  public responseLoaded = false;
  public caseNumberDataLoaded = false;
  public cachedPeriodId: number;
  public cachedYear: number;
  public caseNumberId: number;
  public nonParticipationReductionText = 'Pending';
  public participationPeriod;
  public isPdfLoading = false;
  public defaultFinalPayment = '0.00';
  math = Math;

  public issuanceMonth: string;
  public switchHeader: Readonly<{ [key: string]: string }> = {
    isNotOriginalAndIsNotDelayed: '00',
    isOriginalAndIsNotDelayed: '10',
    isNotOriginalAndIsDelayed: '01',
    isOriginalAndIsDelayed: '11'
  };
  constructor(
    private route: ActivatedRoute,
    private fdService: FieldDataService,
    private partService: ParticipantService,
    private paymentService: PaymentDetailsService,
    private reportService: ReportService
  ) {}

  ngOnInit() {
    this.route.params
      .pipe(
        concatMap(result => {
          this.pin = result.pin;
          this.goBackUrl = '/pin/' + this.pin;
          return forkJoin(this.partService.getCachedParticipant(this.pin).pipe(take(1)), this.fdService.getFieldDataByField(FieldDataTypes.ParticipationPeriod).pipe(take(1)));
        })
      )
      .subscribe(results => {
        this.participant = results[0];
        this.participationPeriodDrop = results[1];
        this.yearsDrop = this.initYearDrop();
        this.cachedPeriodId = this.participationPeriodId;
        this.cachedYear = this.periodYear;
        this.isLoaded = true;
      });
  }

  calculateDifferenceOfPayments(index): number {
    if (index === 0) {
      return +this.paymentDetailsModel.participantPaymentHistories[index].finalPayment;
    } else {
      return Math.round(((+this.paymentDetailsModel.participantPaymentHistories[index].finalPayment - +this.getPreviousFinalPayment(index)) * 100) / 100);
    }
  }
  getDetailsBasedonParticipationPeriod() {
    if (this.caseNumberId) {
      this.responseLoaded = false;
      this.paymentService.getPaymentDetails(this.pin, this.participationPeriod, this.periodYear.toString(), this.caseNumberId).subscribe(
        res => {
          if (res) {
            this.paymentDetailsModel = res;
            this.issuanceMonth = null;
            this.isWeeklyPaymentAvailable = res.participantPaymentHistories && res.participantPaymentHistories.some(i => !i.isOriginal);
            this.showBatchDetailsButton = res.participantPaymentHistories && res.participantPaymentHistories.length > 0;
            this.isPaymentdetailsExists = this.showBatchDetailsButton || (res.manualAuxiliaries && res.manualAuxiliaries.length > 0) || !!res.UnAppliedSanctionableHours;
            this.paymentDetailsModel.participantPaymentHistories.forEach(i => {
              if (!this.issuanceMonth) {
                this.issuanceMonth = i.issuanceMonth;
              }
              i.pmtLbl = i.isDelayed ? 'Base W-2 Payment (Delayed Cycle)' : 'Base W-2 Payment';
              i.switchCaseForHeader = `${+i.isOriginal}${+i.isDelayed}`;
              switch (i.switchCaseForHeader) {
                case this.switchHeader.isNotOriginalAndIsNotDelayed:
                  i.header = `Payment Changes (Weekly Batch ${i.createdDate})`;
                  this.assignPullDownValuesAndReCalculateAdjustedPaymentAndFinalPayment(i);
                  break;
                case this.switchHeader.isOriginalAndIsNotDelayed:
                  i.header = 'Original Payment';
                  break;
                case this.switchHeader.isNotOriginalAndIsDelayed:
                  i.header = `Payment Changes - Delayed Cycle (${i.createdDate})`;
                  break;
                case this.switchHeader.isOriginalAndIsDelayed:
                  i.header = 'Original Partial Payment - Delayed';
                  break;
              }
            });
          } else {
            this.isPaymentdetailsExists = false;
            this.isWeeklyPaymentAvailable = false;
            this.showBatchDetailsButton = false;
          }

          this.responseLoaded = true;
          this.cachedPeriodId = this.participationPeriodId;
          this.cachedYear = this.periodYear;
        },
        error => {
          this.isPaymentdetailsExists = false;
          this.isWeeklyPaymentAvailable = false;
          this.showBatchDetailsButton = false;
          this.paymentDetailsModel = null;
          this.responseLoaded = false;
        }
      );
    } else {
      this.isPaymentdetailsExists = false;
      this.isWeeklyPaymentAvailable = false;
      this.showBatchDetailsButton = false;
      this.paymentDetailsModel = null;
    }
  }

  private assignPullDownValuesAndReCalculateAdjustedPaymentAndFinalPayment(i: ParticipantPaymentHistory) {
    let desiredDelayedCycleRecord = this.getDesiredDelayedCycleRecord();
    i.baseW2PaymentDelayedCycle = desiredDelayedCycleRecord.length > 0 ? desiredDelayedCycleRecord[0].baseW2Payment : null;
    i.baseW2Payment = (+i.baseW2Payment - +i.baseW2PaymentDelayedCycle).toFixed(2);
    i.drugFelonPenaltyDelayedCycle = desiredDelayedCycleRecord.length > 0 ? desiredDelayedCycleRecord[0].drugFelonPenalty : null;
    i.drugFelonPenalty = (+i.drugFelonPenalty - +i.drugFelonPenaltyDelayedCycle).toFixed(2);
  }

  getPreviousFinalPayment(index) {
    if (index > 0) {
      if (
        this.paymentDetailsModel &&
        !this.paymentDetailsModel.participantPaymentHistories[index].isOriginal &&
        !this.paymentDetailsModel.participantPaymentHistories[index].isDelayed
      ) {
        if (this.paymentDetailsModel.participantPaymentHistories[index - 1].isOriginal) {
          let desiredDelayedCycleRecord = this.getDesiredDelayedCycleRecord();
          return (
            (desiredDelayedCycleRecord.length > 0 ? +desiredDelayedCycleRecord[0].finalPayment : 0) + +this.paymentDetailsModel.participantPaymentHistories[index - 1].finalPayment
          ).toFixed(2);
        } else {
          return (+this.paymentDetailsModel.participantPaymentHistories[index - 1].finalPayment).toFixed(2);
        }
      } else {
        return (+this.paymentDetailsModel.participantPaymentHistories[index - 1].finalPayment).toFixed(2);
      }
    }
  }

  private getDesiredDelayedCycleRecord() {
    let filteredDelayedCycleRecords = this.paymentDetailsModel.participantPaymentHistories.filter(i => i.isDelayed);
    let desiredDelayedCycleRecord;
    if (filteredDelayedCycleRecords && filteredDelayedCycleRecords.length > 1) {
      desiredDelayedCycleRecord = _.orderBy(filteredDelayedCycleRecords, ['createdDate'], ['desc']);
    } else {
      desiredDelayedCycleRecord = filteredDelayedCycleRecords;
    }
    return desiredDelayedCycleRecord;
  }

  getCaseNumbersBasedOnParticipationPeriod() {
    this.caseNumberDataLoaded = false;
    if (this.participationPeriodId && this.periodYear) {
      this.participationPeriod = Utilities.fieldDataNameById(this.participationPeriodId, this.participationPeriodDrop, true);
      this.paymentService.getCaseNumbers(this.pin, this.participationPeriod, this.periodYear.toString()).subscribe(res => {
        this.caseNumberDrop = [];
        this.paymentDetailsModel = null;
        this.isPaymentdetailsExists = false;
        this.isWeeklyPaymentAvailable = false;
        this.showBatchDetailsButton = false;
        this.caseNumberDataLoaded = true;
        if (res.length > 0) {
          this.caseNumbersString = res.join(',');
          res.forEach(i => {
            this.caseNumberId = null;
            const caseNumberEntry = { id: i, name: i };
            this.caseNumberDrop.push(caseNumberEntry);
          });
        }
        if (this.caseNumberDrop.length === 1) {
          this.caseNumberId = +this.caseNumberDrop[0].id;
          this.getDetailsBasedonParticipationPeriod();
        }
      });
    }
  }

  initYearDrop() {
    const currentDate = Utilities.currentDate;
    const cutOverDate = this.participant.cutOverDate ? this.participant.cutOverDate : currentDate.clone().format('MM/DD/YYYY');
    const minYear = new Date(cutOverDate).getFullYear();
    const maxYear = new Date(currentDate.clone().format('MM/DD/YYYY')).getFullYear();
    const years = Utilities.getRangeBetweenNums(minYear, maxYear, 1, true);
    const drop = [];

    years.forEach(i => {
      const year = { id: i, name: i.toString() };
      drop.push(year);
    });
    return drop;
  }

  generatePdf() {
    this.isPdfLoading = true;
    this.participationPeriod = Utilities.fieldDataNameById(this.participationPeriodId, this.participationPeriodDrop, true);
    this.reportService.getBatchDetailsReport(this.pin, this.participationPeriod, this.periodYear.toString(), this.caseNumbersString).subscribe(
      data => {
        const blob: any = new Blob([data], { type: 'application/pdf' });
        if (blob) {
          //IE doesn't allow using a blob object directly as link href instead it is necessary to use msSaveOrOpenBlob
          if (window.navigator && window.navigator.msSaveOrOpenBlob) {
            window.navigator.msSaveOrOpenBlob(blob, `BatchDetails_${this.pin}.pdf`);
            this.isPdfLoading = false;
            return;
          }

          // For other browsers:
          // Create a link pointing to the ObjectURL containing the blob.
          const blobUrl = URL.createObjectURL(blob);
          window.open(blobUrl, `BatchDetails_${this.pin}.pdf`);
          setTimeout(() => {
            //Revoke the blob url after the tab is opened
            window.URL.revokeObjectURL(blobUrl);
          }, 1000);
          this.isPdfLoading = false;
        }
      },
      () => {
        this.isPdfLoading = false;
      }
    );
  }
}
