// tslint:disable: deprecation
import { LogService } from '../../../shared/services/log.service';
import { Injectable } from '@angular/core';
import { AppService } from '../../../core/services/app.service';
import { Response } from '@angular/http';
import { AuthHttpClient } from '../../../core/interceptors/AuthHttpClient';
import { BaseService } from '../../../core/services/base.service';
import { Observable, BehaviorSubject } from 'rxjs';
import { OrganizationInformation, FinalistLocation } from '../models/organization-information.model';
import { Utilities } from '../../../shared/utilities';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class OrganizationInformationService extends BaseService {
  private orgInfoUrl: string;
  public modeForFinalistLocation = new BehaviorSubject<any>({ orgInfoId : 0, finalistLocation: null, readOnly: false, isInEditMode: false });

  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService) {
    super(http, logService);
    this.orgInfoUrl = this.appService.apiServer + 'api/org-info/';
  }

  public getOrganizationInformation(progId: number, orgId: number): Observable<OrganizationInformation> {
    return this.http.get(this.orgInfoUrl + progId + '/' + orgId).pipe(map(this.extractOrganizationInformationData), catchError(this.handleError));
  }

  public saveOrganizationInformation(id: number, organizationInformation: OrganizationInformation) {
    const body = JSON.stringify(organizationInformation);

    return this.http.post(this.orgInfoUrl + id, body).pipe(map(this.extractOrganizationInformationData), catchError(this.handleError));
  }

  private extractOrganizationInformationData(res: OrganizationInformation): OrganizationInformation {
    const jsonObjs = res as OrganizationInformation;
    let objs: OrganizationInformation = null;

    if (jsonObjs) 
    {
      objs = new OrganizationInformation().deserialize(jsonObjs);
      objs.locations = [];
      jsonObjs["locations"].forEach(loc => {
          objs.locations.push(new FinalistLocation().deserialize(loc));
      });      
    }


    return objs;
  }
}
