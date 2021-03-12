import { EnrolledProgramCode } from './../../shared/enums/enrolled-program-code.enum';
// tslint:disable: import-blacklist
import { Component, OnInit } from '@angular/core';
import { DropDownField } from '../../shared/models/dropdown-field';
import { FieldDataService } from '../../shared/services/field-data.service';
import { OrganizationInformation, FinalistLocation } from './models/organization-information.model';
import { ModelErrors } from '../../shared/interfaces/model-errors';
import { Router } from '@angular/router';
import { OrganizationInformationService } from './services/organization-information.service';
import { ValidationManager } from '../../shared/models/validation';
import { AppService } from '../../core/services/app.service';
import { FieldDataTypes } from 'src/app/shared/enums/field-data-types.enum';

@Component({
  selector: 'app-organization-information',
  templateUrl: './organization-information.component.html',
  styleUrls: ['./organization-information.component.scss']
})
export class OrganizationInformationComponent implements OnInit {
  public isLoaded = false;
  public programId: number;
  public cachedProgramId: number;
  public organizationId: number;
  public cachedOrganizationId: number;
  public programDrop: DropDownField[] = [];
  public orgDrop: DropDownField[] = [];
  public model: OrganizationInformation;
  public originalModel: OrganizationInformation;
  public cachedModel: OrganizationInformation = new OrganizationInformation();
  public modelErrors: ModelErrors = {};
  public goBackUrl: string;
  public isInEditMode = false;
  public enrolledProgram = EnrolledProgramCode;

  constructor(private oiService: OrganizationInformationService, public appService: AppService, private fdService: FieldDataService, private router: Router) {}

  ngOnInit() {
    this.goBackUrl = `/home`;
    this.fdService.getFieldDataByField(FieldDataTypes.Programs).subscribe(result => {
      this.initProgramsDrop(result);
    });

    //Reload the list if new location is added
    this.oiService.modeForFinalistLocation.subscribe(res => {
      this.isInEditMode = res.isInEditMode;
      if (!this.isInEditMode) {
        this.initModel();
      }
    });
  }

  initModel(): void {
    this.isLoaded = false;
    if (this.programId && this.organizationId) {
      this.oiService.getOrganizationInformation(this.programId, this.organizationId).subscribe(result => {
        this.model = result;

        if (this.model !== null) {
          OrganizationInformation.clone(this.model, this.cachedModel);
          this.isLoaded = true;
        } else {
          this.newModel();
          this.model.locations = [];
          OrganizationInformation.clone(this.model, this.cachedModel);
          this.isLoaded = true;
        }
      });
    }
  }

  /**
   * Initializes the new Organization Information model
   */
  newModel() {
    const model = new OrganizationInformation();
    model.id = 0;
    model.locations = [];
    this.model = model;
  }

  /**
   * Load the Programs DropDown
   */
  private initProgramsDrop(data): void {
    this.cachedProgramId = null;
    this.cachedOrganizationId = null;
    this.programDrop = data
      .filter(p => p.code !== 'FCD')
      .sort(function(a, b) {
        if (a.name < b.name) {
          return -1;
        }
        if (a.name > b.name) {
          return 1;
        }
        return 0;
      });
  }

  /**
   * Load the Organization Dropdown
   */
  initOrganization(programId): void {
    this.isLoaded = false;
    this.organizationId = null;
    if (this.programId)
      this.fdService.getFieldDataByField(FieldDataTypes.ProgramOrganizations, programId).subscribe(res => {
        this.orgDrop = res;
      });
    else this.orgDrop = [];
  }
}
