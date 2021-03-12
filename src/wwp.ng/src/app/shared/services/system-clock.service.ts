import { Injectable, NgZone } from '@angular/core';
// tslint:disable no-unused-expression
import { LogService } from './log.service';
import * as moment from 'moment';
import { SimulatedDate } from '../models/simulated-date.model';
import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
import { BaseService } from 'src/app/core/services/base.service';

@Injectable()
export class SystemClockService extends BaseService {
  public constructor(http: AuthHttpClient, logService: LogService, private zone: NgZone) {
    super(http, logService);

    if (localStorage.getItem('_simulatedDateTime')) {
      SystemClockService._simulatedDateTime = moment(localStorage.getItem('_simulatedDateTime'), moment.defaultFormat);
    }
    if (SystemClockService._simulatedDateTime && SystemClockService._simulatedDateTime.isValid() && localStorage.getItem('_isTimeSimulated')) {
      SystemClockService._isTimeSimulated === true;
    }
  }

  private static _isTimeSimulated = false;
  private static _serverDateTime: moment.Moment;
  private static _simulatedDateTime: moment.Moment;

  public static get isTimeSimulated(): boolean {
    return !!this._isTimeSimulated;
  }

  public static get serverDateTime(): moment.Moment {
    return this._serverDateTime ? this._serverDateTime.clone() : null;
  }

  public static get simulatedDateTime(): moment.Moment {
    return this._simulatedDateTime ? this._simulatedDateTime.clone() : null;
  }

  public static get appDateTime(): moment.Moment {
    return (this.isTimeSimulated ? this.simulatedDateTime : this.serverDateTime) || moment();
  }

  public static simulateClientDateTime(simulatedDate: SimulatedDate) {
    SystemClockService._simulatedDateTime = simulatedDate.cdoDateMoment;
    SystemClockService._isTimeSimulated = true;
    localStorage.setItem('_isTimeSimulated', SystemClockService._isTimeSimulated.toString());
    localStorage.setItem('_simulatedDateTime', SystemClockService._simulatedDateTime.format(moment.defaultFormat));
  }

  public static cancelSimulateClientDateTime() {
    SystemClockService._simulatedDateTime = null; //this.serverDateTime;
    SystemClockService._isTimeSimulated = false;
    localStorage.setItem('_isTimeSimulated', SystemClockService._isTimeSimulated.toString());
    localStorage.removeItem('_simulatedDateTime');
  }
}
