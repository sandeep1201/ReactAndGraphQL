import { AppService } from '../../../core/services/app.service';
import { LogService } from '../../../shared/services/log.service';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { ActionNeededTask, ActionNeeded } from '../models/action-needed-new';
import { Utilities } from '../../../shared/utilities';
import { AuthHttpClient } from '../../../core/interceptors/AuthHttpClient';

import * as _ from 'lodash';
import { BaseService } from '../../../core/services/base.service';

@Injectable()
export class ActionNeededService extends BaseService {
  private actionNeededUrl: string;
  private actionNeededTaskUrl: string;

  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService) {
    super(http, logService);
    this.actionNeededUrl = this.appService.apiServer + 'api/action-needed/';
    this.actionNeededTaskUrl = this.appService.apiServer + 'api/action-needed-task/';
  }

  public getList() {
    return this.http.get(this.actionNeededUrl + this.getPin()).pipe(map(this.extractList), catchError(this.handleError));
  }

  public getTask(id: number) {
    return this.http.get(this.actionNeededTaskUrl + this.getPin() + '/' + id).pipe(map(this.extractActionNeededTask), catchError(this.handleError));
  }

  public getActionNeeded(pageCode: string) {
    return this.http.get(this.actionNeededUrl + this.getPin() + '/' + pageCode).pipe(map(this.extractActionNeeded), catchError(this.handleError));
  }

  public postTask(actionNeededTask: ActionNeededTask): Observable<ActionNeededTask> {
    const body = JSON.stringify(actionNeededTask);

    return this.http.post(this.actionNeededTaskUrl + this.getPin(), body).pipe(map(this.extractActionNeededTask), catchError(this.handleError));
  }

  public postActionNeeded(actionNeeded: ActionNeeded): Observable<ActionNeeded> {
    const body = JSON.stringify(actionNeeded);
    return this.http.post(this.actionNeededUrl + this.getPin() + '/' + _.kebabCase(actionNeeded.pageName), body).pipe(map(this.extractActionNeeded), catchError(this.handleError));
  }

  public deleteTask(id: number) {
    return this.http.delete(this.actionNeededTaskUrl + this.getPin() + '/' + id).pipe(catchError(this.handleError));
  }

  private extractActionNeeded(res: ActionNeeded): ActionNeeded {
    const body = res as ActionNeeded;
    const an = new ActionNeeded().deserialize(body);
    return an || null;
  }

  private extractActionNeededTask(res: ActionNeededTask[]): ActionNeededTask {
    const body = res as ActionNeededTask[];
    const an = new ActionNeededTask().deserialize(body);
    return an || null;
  }

  private extractList(res: ActionNeededTask[]): ActionNeededTask[] {
    const body = res as ActionNeededTask[];
    const data: ActionNeededTask[] = [];
    for (const pb of body) {
      data.push(new ActionNeededTask().deserialize(pb));
    }
    return data || null;
  }
}
