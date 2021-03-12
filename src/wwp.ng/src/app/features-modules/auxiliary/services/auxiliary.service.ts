import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { Injectable } from '@angular/core';
import { BaseService } from '../../../core/services/base.service';
import { AuthHttpClient } from '../../../core/interceptors/AuthHttpClient';
import { LogService } from '../../../shared/services/log.service';
import { AppService } from '../../../core/services/app.service';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { Auxiliary } from '../models/auxiliary.model';

@Injectable()
export class AuxiliaryService extends BaseService {
  private auxiliaryUrl: string;
  public modeForAuxiliary = new BehaviorSubject<any>({ readOnly: false, isInEditMode: false });
  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService) {
    super(http, logService);
    this.auxiliaryUrl = this.appService.apiServer + 'api/auxiliary/';
  }

  public getAuxiliaryData(participantId?: number): Observable<Auxiliary[]> {
    const url = !participantId ? `${this.auxiliaryUrl}list` : `${this.auxiliaryUrl}list/${participantId}`;
    return this.http.get(url).pipe(map(this.extractAuxiliariesData), catchError(this.handleError));
  }

  public getAuxiliaryDataById(auxiliaryId?: number): Observable<Auxiliary> {
    return this.http.get(`${this.auxiliaryUrl}${auxiliaryId}`).pipe(map(this.extractAuxiliaryData), catchError(this.handleError));
  }

  public getDetailsBasedonParticipationPeriod(pin: string, participantId: number, participationPeriod: string, year: number) {
    return this.http
      .get(`${this.auxiliaryUrl}${pin}/${participantId}/paymentDetails/${participationPeriod}/${year}`)
      .pipe(map(this.extractAuxiliaryData), catchError(this.handleError));
  }

  public saveAuxiliary(model: Auxiliary, pin: string) {
    const requestUrl = `${this.auxiliaryUrl}${pin}`;
    return this.http.post(requestUrl, model).pipe(
      catchError(err => {
        return this.handleError(err);
      })
    );
  }

  public extractAuxiliaryData(res: Auxiliary[]) {
    const body = res as Auxiliary[];
    const auxiliary = new Auxiliary().deserialize(body);
    return auxiliary || null;
  }

  private extractAuxiliariesData(res: Auxiliary[]): Auxiliary[] {
    const jsonObjs = res as Auxiliary[];
    const objs: Auxiliary[] = [];
    for (const obj of jsonObjs) {
      objs.push(new Auxiliary().deserialize(obj));
    }
    return objs || [];
  }
}
