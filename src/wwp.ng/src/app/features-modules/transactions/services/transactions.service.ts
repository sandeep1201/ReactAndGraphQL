import { AppService } from 'src/app/core/services/app.service';
import { Injectable } from '@angular/core';
import { BaseService } from 'src/app/core/services/base.service';
import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
import { LogService } from 'src/app/shared/services/log.service';
import { TransactionModel } from '../transactions.model';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

@Injectable()
export class TransactionsService extends BaseService {
  private transactionUrl: string;
  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService) {
    super(http, logService);
    this.transactionUrl = this.appService.apiServer + 'api/transaction/';
  }

  public getTransactions(participantId: number): Observable<TransactionModel[]> {
    const url = `${this.transactionUrl}list/${participantId}`;
    return this.http.get(url).pipe(map(this.extractTransactionsData), catchError(this.handleError));
  }

  private extractTransactionsData(res: TransactionModel[]): TransactionModel[] {
    const jsonObjs = res as TransactionModel[];
    const objs: TransactionModel[] = [];
    for (const obj of jsonObjs) {
      objs.push(new TransactionModel().deserialize(obj));
    }
    return objs || [];
  }
}
