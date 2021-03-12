import { Component, OnInit, Input } from '@angular/core';
import * as _ from 'lodash';

import { OrganizationInformationService } from 'src/app/features-modules/organization-info/services/organization-information.service';
import { FinalistLocation, OrganizationInformation } from 'src/app/features-modules/organization-info/models/organization-information.model';
import { Utilities } from 'src/app/shared/utilities';

@Component({
  selector: 'app-org-info-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class OrganizationInormationListComponent implements OnInit {
  @Input() isReadOnly = false;
  @Input() organizationId: number;
  @Input() orgInfo: OrganizationInformation;
  @Input() programId: number;

  public isInEditMode = false;
  public isLoaded = true;
  //canAdd -- controls the Add Location button visibility
  //canEdit -- controls the Editing of the location displayed on the list
  public canAdd = true;
  public canEdit = false;
  public locations: FinalistLocation[];
  public locationId = 0;
  public finalistlocations: FinalistLocation[] = [];
  public finalistLocation = new FinalistLocation();
  public orgInfoId = 0;

  constructor(private oiService: OrganizationInformationService) {}

  ngOnInit() {
    this.finalistlocations = this.orgInfo.locations;
    this.orgInfoId = this.orgInfo.id;
    //Show/Hide the edit location modal popup
    this.oiService.modeForFinalistLocation.subscribe(res => {
      this.isInEditMode = res.isInEditMode;
    });
  }

  /**
   * Invoked when Add Button is clicked
   */
  onAdd() {
    this.isInEditMode = true;
    this.finalistLocation.id = 0;
    this.oiService.modeForFinalistLocation.next({ orgInfo: this.orgInfo, finalistLocation: this.finalistLocation, readOnly: false, isInEditMode: this.isInEditMode });
  }

  /**
   * Invoked when Edit Button is clicked
   */
  edit(a, readOnly) {
    this.isInEditMode = true;
    this.oiService.modeForFinalistLocation.next({ orgInfo: this.orgInfo, finalistLocation: a, readOnly: readOnly, isInEditMode: this.isInEditMode });
  }

  /**
   * Controls the visibilty of Edit Location icons
   */
  isEditable(finalistLocation: FinalistLocation): boolean {
    let canEdit = false;
    //US3428
    if (!finalistLocation.endDate || Utilities.currentDate.format('MM/DD/YYYY') < finalistLocation.endDate) {
      canEdit = true;
    }
    return canEdit;
  }

  /**
   * Invoked when Window close or cancel Button is clicked
   */
  closeEdit(e: boolean) {
    this.isInEditMode = e;
  }
}
