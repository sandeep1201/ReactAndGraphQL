import { AuxiliaryService } from '../services/auxiliary.service';
import { Component, OnInit } from '@angular/core';
import { Auxiliary } from '../models/auxiliary.model';
import { AppService } from 'src/app/core/services/app.service';
import { AccessType } from 'src/app/shared/enums/access-type.enum';
import { Utilities } from 'src/app/shared/utilities';
import { AuxiliaryStatusTypes } from '../enums/auxiliary-status-types.enums';

@Component({
  selector: 'app-aux-approver-list',
  templateUrl: './approver-list.component.html',
  styleUrls: ['./approver-list.component.scss']
})
export class AuxiliaryApproverListComponent implements OnInit {
  public isLoaded = false;
  public isInEditMode = false;
  public auxiliaries: Auxiliary[];
  public isSortByDateToggled = false;

  constructor(private auxService: AuxiliaryService, private appService: AppService) {}

  ngOnInit(): void {
    this.auxService.modeForAuxiliary.subscribe(res => {
      this.isInEditMode = res.isInEditMode;
      if (!this.isInEditMode) {
        this.getAuxData();
      }
    });
  }
  public getAuxData() {
    this.auxService.getAuxiliaryData().subscribe(res => {
      this.auxiliaries = res;
      this.isLoaded = true;
    });
  }
  edit(a) {
    this.isInEditMode = true;
    const canApprove = this.canApproveAux();
    this.auxService.modeForAuxiliary.next({
      aux: a,
      canApprove: canApprove,
      readonly: canApprove && a.auxiliaryStatusTypeCode === AuxiliaryStatusTypes.RETURN,
      isInEditMode: this.isInEditMode
    });
  }
  canApproveAux() {
    return this.appService.coreAccessContext.evaluate() === AccessType.edit;
  }

  sortByDate() {
    this.auxiliaries = Utilities.sortArrayByDate(this.auxiliaries, 'auxiliaryStatusDate', this.isSortByDateToggled);
    this.isSortByDateToggled = !this.isSortByDateToggled;
  }
}
