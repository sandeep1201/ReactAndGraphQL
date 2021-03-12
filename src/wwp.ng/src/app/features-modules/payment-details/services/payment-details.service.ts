import { Injectable } from '@angular/core';
import { BaseService } from 'src/app/core/services/base.service';
import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
import { LogService } from 'src/app/shared/services/log.service';
import { AppService } from 'src/app/core/services/app.service';
import { Observable } from 'rxjs';
import { PaymentDetails } from '../models/payment-details.model';
import { map, catchError } from 'rxjs/operators';

@Injectable()
export class PaymentDetailsService extends BaseService {
  private paymentDetailsUrl: string;
  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService) {
    super(http, logService);
    this.paymentDetailsUrl = this.appService.apiServer + 'api/Participant/';
  }

  public getPaymentDetails(pin: string, participationPeriod: string, year: string, caseNumber: number): Observable<PaymentDetails> {
    const url = `${this.paymentDetailsUrl}${pin}/paymentDetails/${participationPeriod}/${year}/${caseNumber}`;
    return this.http.get(url).pipe(map(this.extractPaymentDetailsData), catchError(this.handleError));
  }
  public getCaseNumbers(pin: string, participationPeriod: string, year: string): Observable<any> {
    const url = `${this.paymentDetailsUrl}${pin}/caseNumber/${participationPeriod}/${year}`;
    return this.http.get(url).pipe(map(this.extractCaseNumbers), catchError(this.handleError));
  }

  private extractCaseNumbers(res: any) {
    return res || null;
  }

  private extractPaymentDetailsData(res: PaymentDetails[]) {
    const body = res as PaymentDetails[];
    const paymentDetails = new PaymentDetails().deserialize(body);
    return paymentDetails || null;
  }
}
