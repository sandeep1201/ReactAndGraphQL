import { W2Plan, W2PlanSection } from './../models/w-2-plan.model';
import { AppService } from 'src/app/core/services/app.service';
import { Injectable } from '@angular/core';
import { BaseService } from 'src/app/core/services/base.service';
import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
import { LogService } from 'src/app/shared/services/log.service';
import { Observable, BehaviorSubject } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

@Injectable()
export class W2PlansService extends BaseService {
  private w2PlansUrl: string;
  public routeState = new BehaviorSubject<any>({});
  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService) {
    super(http, logService);
    this.w2PlansUrl = this.appService.apiServer + 'api/w-2-plans/';
  }

  public getW2PlansByParticipant(participantId: number): Observable<W2Plan[]> {
    const url = `${this.w2PlansUrl}${participantId}`;
    return this.http.get(url).pipe(map(this.extractW2PlansData), catchError(this.handleError));
  }

  public saveW2PlansSection(sectionModel: W2PlanSection, participantId: number) {
    const url = `${this.w2PlansUrl}section/${participantId}`;
    const body = JSON.stringify(sectionModel);
    return this.http.post(url, body).pipe(map(this.extractW2PlansSectionData));
  }

  private extractW2PlansSectionData(res: W2PlanSection): W2PlanSection {
    const jsonObjs = res as W2PlanSection;
    const objs = new W2PlanSection().deserialize(jsonObjs['value']);
    return objs;
  }
  private extractW2PlansData(res: W2Plan[]): W2Plan[] {
    const jsonObjs = res as W2Plan[];
    const objs: W2Plan[] = [];
    for (const obj of jsonObjs) {
      objs.push(new W2Plan().deserialize(obj));
    }
    return objs || [];
  }
}
