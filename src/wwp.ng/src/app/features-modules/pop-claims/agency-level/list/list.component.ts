import { FieldDataService } from './../../../../shared/services/field-data.service';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { POPClaimStatusTypes } from '../../enums/pop-claim-status-types.enum';
import { POPClaim } from '../../models/pop-claim.model';
import { FieldDataTypes } from 'src/app/shared/enums/field-data-types.enum';
import { PopClaimsService } from '../../services/pop-claims.service';
import { AppService } from 'src/app/core/services/app.service';

@Component({
  selector: 'app-agency-level-pop-claims-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class AgencyLevelPOPClaimsListComponent implements OnInit {
  public isLoaded = false;
  @Input() popClaims: POPClaim[];
  @Input() POPClaimTypes: DropDownField[];
  @Input() POPClaimStatusTypes: DropDownField[];
  public inSingleEntryView = false;
  public clickedPOPClaim: POPClaim;
  public popClaimStatusTypes: DropDownField[] = [];
  filteredPOPClaimStatusTypes: DropDownField[] = [];

  @Output() onExitSingleEntry = new EventEmitter<any>();
  constructor(private fieldDataService: FieldDataService, private popClaimsService: PopClaimsService, private appService: AppService) {}

  ngOnInit() {
    this.fieldDataService.getFieldDataByField(FieldDataTypes.POPClaimStatusTypes).subscribe(res => {
      this.popClaimStatusTypes = res;
      this.filteredPOPClaimStatusTypes = this.popClaimStatusTypes.filter(x => x.code === POPClaimStatusTypes.APPROVE || x.code === POPClaimStatusTypes.WITHDRAW);
      this.isLoaded = true;
    });
  }

  refreshListView() {
    this.isLoaded = false;
    const statuses = [POPClaimStatusTypes.SUBMIT, POPClaimStatusTypes.RETURN];
    const agencyCode = this.appService.user.agencyCode;
    this.popClaimsService.getPopClaimsBasedOnStatusesAndAgencyCode(statuses, agencyCode).subscribe(res => {
      this.popClaims = res;
      this.inSingleEntryView = false;
      this.isLoaded = true;
    });
  }

  onSingleItemClick(item: POPClaim) {
    this.inSingleEntryView = true;
    this.clickedPOPClaim = item;
  }
  exitSingleEntryView() {
    this.inSingleEntryView = false;
    this.onExitSingleEntry.emit();
  }
}
