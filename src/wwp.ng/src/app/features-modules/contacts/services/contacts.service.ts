// tslint:disable: deprecation
import { AuthHttpClient } from '../../../core/interceptors/AuthHttpClient';
import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { AppService } from '../../../core/services/app.service';
import { BaseService } from '../../../core/services/base.service';
import { Contact } from '../models/contact';
import { LogService } from '../../../shared/services/log.service';

@Injectable()
export class ContactsService extends BaseService {
  private contactsDeleteUrl: string;
  private contactsUrl: string;

  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService) {
    super(http, logService);
    this.contactsUrl = this.appService.apiServer + 'api/contacts/';
    this.contactsDeleteUrl = this.appService.apiServer + 'api/contacts/delete/';
  }

  /**
   * Makes a call to the API to retreive a Contact record by its ID.
   *
   * @param {number} id
   * @param {string} pin
   * @returns {Observable<Contact>}
   *
   * @memberOf ContactsService
   */
  public getContactById(id: number, pin: string): Observable<Contact> {
    return this.http.getResponse(this.contactsUrl + pin + '/' + id).pipe(
      map(resp => {
        if (resp) {
          if (resp.status === 200) return this.extractContactData(resp.body);
        }
      }),
      catchError(this.handleError)
    );
  }

  /**
   * Makes a call to the API to retreive all the Contacts for a PIN.
   *
   * @param {string} pin
   * @returns {Observable<Contact[]>}
   *
   * @memberOf ContactsService
   */
  public getContactsByPin(pin: string): Observable<Contact[]> {
    return this.http.get(this.contactsUrl + pin).pipe(map(this.extractContactsData), catchError(this.handleError));
  }

  /**
   * Calls the API to delete a Contact record given its ID.  Note any references
   * to the Contact will be nullified.
   *
   * @param {string} id
   * @param {string} pin
   * @returns {Observable<Response>}
   *
   * @memberOf ContactsService
   */
  public deleteContactById(id: number, pin: string): Observable<Response> {
    // Setup the API call.

    return this.http.delete(this.contactsDeleteUrl + pin + '/' + id);
  }

  /**
   * Saves a Contact object to the API server.
   *
   * @param {string} pin
   * @param {Contact} model
   * @returns {Observable<Contact>}
   *
   * @memberOf ContactsService
   */
  saveContact(pin: string, model: Contact): Observable<Contact> {
    const body = JSON.stringify(model);

    return this.http.post(this.contactsUrl + pin + '/' + model.id, body).pipe(map(this.extractContactData), catchError(this.handleError));
  }

  private extractContactData(res: Contact): Contact {
    const obj = new Contact().deserialize(res);
    return obj || null;
  }

  private extractContactsData(res: Contact[]): Contact[] {
    const jsonObjs = res as Contact[];
    const objs: Contact[] = [];

    for (const obj of jsonObjs) {
      objs.push(new Contact().deserialize(obj));
    }

    return objs || [];
  }
}
