import { AuthHttpClient } from '../../core/interceptors/AuthHttpClient';
// import { LogService } from './log.service';
import { Injectable } from '@angular/core';
import { AppService } from '../../core/services/app.service';
import { BaseService } from '../../core/services/base.service';
import { ContactInfo } from '../models/contact-info.model';
import { catchError, map } from 'rxjs/operators';
import { LogService } from './log.service';
import { Contact } from 'src/app/features-modules/contacts/models/contact';

@Injectable()
export class ContactInfoService extends BaseService {
  private contactInfoUrl: string;

  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService) {
    super(http, logService);
    this.contactInfoUrl = this.appService.apiServer + 'api/contacts/';
  }

  /**
   * get the contact info for logged in worker
   * @param {number} workerId
   */
  public getWorkerContactInfo() {
    return this.http.get(this.contactInfoUrl + 'worker').pipe(map(this.extractList), catchError(this.handleError));
  }

  /**
   * saves the contact info for logged in worker
   * @param {number} workerId
   */
  public saveWorkerContactInfo(contactInfo: ContactInfo) {
    const body = JSON.stringify(contactInfo);

    return this.http.post(this.contactInfoUrl + 'contactInfo', body).pipe(map(this.extractList), catchError(this.handleError));
  }

  private extractList(res: ContactInfo): ContactInfo {
    const body = res;
    let data: ContactInfo;
    data = new ContactInfo().deserialize(res);
    return data || null;
  }
}
