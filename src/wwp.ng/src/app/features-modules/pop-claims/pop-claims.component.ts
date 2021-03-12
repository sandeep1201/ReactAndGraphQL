import { LegalIssuesEditComponent } from './../../participant/informal-assessment/edit/legal-issues.component';
import { FieldDataService } from 'src/app/shared/services/field-data.service';
import { Participant } from 'src/app/shared/models/participant';
import { PopClaimsService } from './services/pop-claims.service';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { concatMap } from 'rxjs/operators';
import { forkJoin } from 'rxjs';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { POPClaim } from './models/pop-claim.model';
import { FieldDataTypes } from 'src/app/shared/enums/field-data-types.enum';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { POPClaimStatusTypes } from './enums/pop-claim-status-types.enum';
import { AppService } from 'src/app/core/services/app.service';
@Component({
  selector: 'app-pop-claims',
  templateUrl: './pop-claims.component.html',
  styleUrls: ['./pop-claims.component.scss']
})
export class PopClaimsComponent implements OnInit {
  public inEditMode = false;
  public isPinBased = false;
  public pin: any;
  public employments = [];
  public participant: Participant;
  public popClaims: POPClaim[];
  public isLoaded = false;
  public POPClaimTypes: DropDownField[];
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private popClaimsService: PopClaimsService,
    private partService: ParticipantService,
    private fdService: FieldDataService,
    private appService: AppService
  ) {}

  ngOnInit() {
    this.isPinBased = this.router.url.includes('pin');
    this.fdService.getFieldDataByField(FieldDataTypes.POPCliamTypes).subscribe(res => {
      this.POPClaimTypes = res;
    });
    if (this.isPinBased) {
      this.route.parent.params
        .pipe(
          concatMap(result => {
            this.pin = result.pin;
            return forkJoin([this.partService.getCachedParticipant(this.pin)]);
          })
        )
        .subscribe(res => {
          this.participant = res[0];
          this.initPOPClaims();
        });
    } else {
      this.initPOPClaimsByStatusAndAgency();
    }
  }

  initPOPClaims() {
    if (this.isLoaded) {
      this.isLoaded = false;
    }
    this.popClaimsService.getPopClaims(this.participant ? this.participant.id : null).subscribe(res => {
      this.popClaims = res;
      this.isLoaded = true;
    });
  }
  initPOPClaimsByStatusAndAgency() {
    if (this.isLoaded) {
      this.isLoaded = false;
    }
    const statuses = [POPClaimStatusTypes.SUBMIT, POPClaimStatusTypes.RETURN];
    const agencyCode = this.appService.user.agencyCode;
    this.popClaimsService.getPopClaimsBasedOnStatusesAndAgencyCode(statuses, agencyCode).subscribe(res => {
      this.popClaims = res;
      this.isLoaded = true;
    });
  }

  exitSingleEntryView() {
    this.initPOPClaimsByStatusAndAgency();
  }
  exitEditView() {
    this.isLoaded = false;
    this.initPOPClaims();
    this.inEditMode = false;
  }
  onAddPOP() {
    this.inEditMode = true;
  }
}
