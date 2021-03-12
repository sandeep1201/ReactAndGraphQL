import { POPClaimStatusTypes } from './../../enums/pop-claim-status-types.enum';
import { Utilities } from 'src/app/shared/utilities';
import { FieldDataTypes } from 'src/app/shared/enums/field-data-types.enum';
import { FieldDataService } from 'src/app/shared/services/field-data.service';
import { forkJoin } from 'rxjs';
import { PopClaimsService } from './../../services/pop-claims.service';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { POPClaim } from '../../models/pop-claim.model';
import * as _ from 'lodash';

@Component({
  selector: 'app-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class AdjudicatorLevelListComponent implements OnInit {
  public isLoaded = false;
  public popClaims: POPClaim[];
  public originalPOPClaimsCopy: POPClaim[];
  public POPClaimTypes: DropDownField[];
  public POPClaimStatusTypes: DropDownField[];
  public isSortByDateToggled = false;
  agencyNames: DropDownField[] = [];
  filterQueryTypes = [];
  filteredStatusTypesDrop: DropDownField[] = [];
  public inSingleEntryView = false;
  public clickedPOPClaim: POPClaim;
  @Output() onExitSingleEntry = new EventEmitter<any>();
  constructor(private popClaimsService: PopClaimsService, private fieldDataService: FieldDataService) {}

  ngOnInit() {
    this.initData();
  }

  initData() {
    this.retriveDataFromMultipleSources().subscribe(res => {
      this.popClaims = res[0].filter(pop => pop.claimStatusTypeCode === POPClaimStatusTypes.APPROVE);
      this.popClaims = Utilities.sortArrayByDate(this.popClaims, 'claimStatusDate', this.isSortByDateToggled);
      this.isSortByDateToggled = true;
      this.originalPOPClaimsCopy = this.popClaims.slice();
      this.POPClaimTypes = res[1];
      this.POPClaimStatusTypes = res[2];
      this.filteredStatusTypesDrop = this.POPClaimStatusTypes.filter(
        x => x.code === POPClaimStatusTypes.VALIDATE || x.code === POPClaimStatusTypes.DENY || x.code === POPClaimStatusTypes.RETURN
      );
      this.initAgencyOptions();
      this.isLoaded = true;
    });
  }

  retriveDataFromMultipleSources() {
    return forkJoin([
      this.popClaimsService.getPopClaims(),
      this.fieldDataService.getFieldDataByField(FieldDataTypes.POPCliamTypes),
      this.fieldDataService.getFieldDataByField(FieldDataTypes.POPClaimStatusTypes)
    ]);
  }

  initAgencyOptions() {
    if (this.popClaims) {
      const popClaimsCopy = this.popClaims.slice();
      for (let i = 0; i < popClaimsCopy.length; i++) {
        const agencyDropDown = new DropDownField();
        agencyDropDown.id = i;
        agencyDropDown.name = popClaimsCopy[i].agencyName;
        this.agencyNames.push(agencyDropDown);
      }

      this.agencyNames = _.orderBy<DropDownField>(_.uniqBy(this.agencyNames, 'name'), [i => i.name], ['asc']);
    }
  }
  onSingleItemClick(pop) {
    this.inSingleEntryView = true;
    this.clickedPOPClaim = pop;
  }
  sortByDate() {
    this.popClaims = Utilities.sortArrayByDate(this.popClaims, 'claimStatusDate', this.isSortByDateToggled);
    this.isSortByDateToggled = !this.isSortByDateToggled;
  }
  filterOnAgency() {
    this.popClaims = this.originalPOPClaimsCopy;
    let filteredPOPs = [];
    if (this.filterQueryTypes.length > 0) {
      let filterQueryTypeNames = [];
      this.filterQueryTypes.forEach(type => {
        filterQueryTypeNames.push(Utilities.fieldDataNameById(type, this.agencyNames));
      });
      filteredPOPs = this.popClaims.filter(pop => filterQueryTypeNames.indexOf(pop.agencyName) > -1);
      this.popClaims = filteredPOPs;
    } else {
      this.popClaims = this.originalPOPClaimsCopy;
    }
  }
  exitSingleEntryView() {
    this.isSortByDateToggled = false;
    this.initData();
    this.inSingleEntryView = false;
  }
}
