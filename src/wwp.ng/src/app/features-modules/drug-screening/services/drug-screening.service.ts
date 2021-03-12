import { Injectable } from '@angular/core';
import { BaseService } from '../../../core/services/base.service';
import { AuthHttpClient } from '../../../core/interceptors/AuthHttpClient';
import { LogService } from '../../../shared/services/log.service';
import { AppService } from '../../../core/services/app.service';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { DrugScreening } from '../models/drug-screening.model';

@Injectable()
export class DrugScreeningService extends BaseService {
  private drugScreeningUrl: string;

  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService) {
    super(http, logService);
    this.drugScreeningUrl = this.appService.apiServer + 'api/drug-screening/';
  }

  public getDrugScreeningData(pin: string): Observable<DrugScreening> {
    return this.http.get(`${this.drugScreeningUrl}get/${this.getPin() ? this.getPin() : pin}`).pipe(map(this.extractDrugScreeningData), catchError(this.handleError));
  }

  public saveDrugScreening(pin: string, drugScreening: DrugScreening): Observable<DrugScreening> {
    return this.http
      .post(`${this.drugScreeningUrl}post/${this.getPin() ? this.getPin() : pin}`, drugScreening)
      .pipe(map(this.extractDrugScreeningData), catchError(this.handleError));
  }

  private extractDrugScreeningData(res: DrugScreening): DrugScreening {
    if (res) {
      const obj = new DrugScreening().deserialize(res);
      return obj;
    } else {
      return null;
    }
  }
}
