import { Injectable } from '@angular/core';
import { Http, Response } from '@angular/http';
import { Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

import { AppService } from './../../core/services/app.service';
import { BaseService } from './../../core/services/base.service';
import { ParticipantBarrier } from '../models/participant-barriers-app';
import { Utilities } from '../utilities';
import { AuthHttpClient } from './../../core/interceptors/AuthHttpClient';
import { LogService } from './log.service';

@Injectable()
export class ParticipantBarrierAppService extends BaseService {
  private participantBarriersUrl: string;

  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService) {
    super(http, logService);
    this.participantBarriersUrl = this.appService.apiServer + 'api/participant-barriers/';
  }

  public getParticipantBarrier(id: number) {
    return this.http.get(this.participantBarriersUrl + this.getPin() + '/' + id).pipe(map(this.extractParticipantBarrierData), catchError(this.handleError));
  }

  public getParticipantBarriers() {
    return this.http.get(this.participantBarriersUrl + this.getPin()).pipe(map(this.extractParticipantBarriersData), catchError(this.handleError));
  }

  public postParticipantBarrier(model: ParticipantBarrier) {
    const body = JSON.stringify(model);

    return this.http.post(this.participantBarriersUrl + this.getPin() + '/' + model.id, body).pipe(map(this.extractParticipantBarrierData), catchError(this.handleError));
  }

  public deleteParticipantBarrier(id: number) {
    return this.http.delete(this.participantBarriersUrl + 'delete/' + this.getPin() + '/' + id).pipe(catchError(this.handleError));
  }

  private extractParticipantBarriersData(res: ParticipantBarrier[]): ParticipantBarrier[] {
    const body = res as ParticipantBarrier[];
    const participantBarriers: ParticipantBarrier[] = [];
    for (const pb of body) {
      participantBarriers.push(new ParticipantBarrier().deserialize(pb));
    }
    return participantBarriers || null;
  }

  private extractParticipantBarrierData(res: ParticipantBarrier): ParticipantBarrier {
    const body = res as ParticipantBarrier;
    let pb = null;
    if (body != null) {
      pb = new ParticipantBarrier().deserialize(body);
    }
    return pb;
  }
}
