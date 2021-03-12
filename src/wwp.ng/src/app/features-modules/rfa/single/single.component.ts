import { ActivatedRoute, Router } from '@angular/router';
import { BaseParticipantComponent } from '../../../shared/components/base-participant-component';
import { Component, OnInit } from '@angular/core';
import { DropDownField } from '../../../shared/models/dropdown-field';
import { FieldDataService } from '../../../shared/services/field-data.service';
import { ParticipantService } from '../../../shared/services/participant.service';
import { RFAProgram } from '../../../shared/models/rfa.model';
import { RfaService } from '../../../shared/services/rfa.service';
import { Utilities } from '../../../shared/utilities';
import { concatMap } from 'rxjs/operators';

@Component({
  selector: 'app-single',
  templateUrl: './single.component.html',
  styleUrls: ['./single.component.css'],
  providers: [RfaService, FieldDataService]
})
export class RfaSingleComponent extends BaseParticipantComponent implements OnInit {
  fcdpCourtOrderId: number;
  public goBackUrl: string;
  public model: RFAProgram;
  public programDrop: DropDownField[] = [];
  public completionReasonDrop: DropDownField[] = [];
  public completionReason: string;

  public otherCompletionReasonId: number;
  private rfaId = 0;
  public fcdpCourtOrderDrop: DropDownField[] = [];
  constructor(
    private activatedRoute: ActivatedRoute,
    router: Router,
    private rfaService: RfaService,
    participantService: ParticipantService,
    private fieldDataService: FieldDataService
  ) {
    super(activatedRoute, router, participantService);
  }

  ngOnInit() {
    super.onInit();

    this.activatedRoute.params.subscribe(params => {
      this.goBackUrl = '/pin/' + this.pin + '/rfa';
      this.rfaService.setPin(this.pin);
      this.rfaId = params['id'];
    });

    this.rfaService
      .getRfaById(this.rfaId.toString())
      .pipe(
        concatMap(res => {
          this.initModel(res);
          return this.fieldDataService.getCompletionReasons(res.programCode);
        })
      )
      .subscribe(res => {
        this.completionReasonDrop = res;
        this.findCompletionReason();
      });

    this.fcdpCourtOrderDrop = [
      {
        id: 1,
        name: 'Court Ordered'
      },
      {
        id: 2,
        name: 'Voluntary'
      }
    ];
  }

  private initModel(data) {
    this.model = data;
    if (this.model.isVoluntary) {
      this.fcdpCourtOrderId = 2;
    } else {
      this.fcdpCourtOrderId = 1;
    }
  }

  private findCompletionReason() {
    this.completionReason = Utilities.fieldDataNameById(this.model.completionReasonId, this.completionReasonDrop);
    this.otherCompletionReasonId = Utilities.idByFieldDataName('Unsuccessful participation: Other', this.completionReasonDrop);
  }

  private displayCompletionReasonDetails() {
    if (this.model.isCFProgram && this.model.completionReasonId === this.otherCompletionReasonId) {
      return true;
    } else {
      return false;
    }
  }
  private displayFcdpCompletionReasonDetails() {
    if (this.model.isFCDPProgram && this.model.completionReasonId === this.otherCompletionReasonId) {
      return true;
    } else {
      return false;
    }
  }
}
