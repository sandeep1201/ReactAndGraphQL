import { WeeklyHoursWorked } from './../models/weekly-hours-worked.model';
import { LogService } from './../../../shared/services/log.service';
import { Injectable } from '@angular/core';
import { BaseService } from 'src/app/core/services/base.service';
import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
import { AppService } from 'src/app/core/services/app.service';
import { map, catchError } from 'rxjs/operators';
import { of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class WeeklyHoursWorkedService extends BaseService {
  public weeklyHoursWorkedUrl: string;

  constructor(public http: AuthHttpClient, public logService: LogService, private appService: AppService) {
    super(http, logService);
    this.weeklyHoursWorkedUrl = this.appService.apiServer + 'api/weekly-hours-worked/';
  }

  public getWeeklyHoursWorkedEntries(pin, employmentInformationId: number) {
    return this.http.get(`${this.weeklyHoursWorkedUrl}${pin}/${employmentInformationId}`).pipe(map(this.extractWeeklyHoursWorkedEntriesData), catchError(this.handleError));
  }
  public getWeeklyHoursWorkedEntry(pin, employmentInformationId: number, id: number) {
    return this.http.get(`${this.weeklyHoursWorkedUrl}${pin}/${employmentInformationId}/${id}`).pipe(map(this.extractWeeklyHoursWorkedEntryData), catchError(this.handleError));
  }

  public saveWeeklyHoursWorkedEntry(pin, weeklyHoursWorked: WeeklyHoursWorked) {
    return this.http
      .post(`${this.weeklyHoursWorkedUrl}${pin}/${weeklyHoursWorked.employmentInformationId}/${weeklyHoursWorked.id}`, weeklyHoursWorked)
      .pipe(map(this.extractWeeklyHoursWorkedEntriesData), catchError(this.handleError));
  }

  public deleteWeeklyHoursWorkedEntry(pin, weeklyHoursWorked: WeeklyHoursWorked) {
    return this.http
      .delete(`${this.weeklyHoursWorkedUrl}${pin}/${weeklyHoursWorked.employmentInformationId}/${weeklyHoursWorked.id}`)
      .pipe(map(this.extractWeeklyHoursWorkedEntriesData), catchError(this.handleError));
  }

  private extractWeeklyHoursWorkedEntriesData(res: WeeklyHoursWorked[]): WeeklyHoursWorked[] | null {
    const jsonObjs = res as WeeklyHoursWorked[];
    const objs: WeeklyHoursWorked[] = [];

    for (const obj of jsonObjs) {
      objs.push(new WeeklyHoursWorked().deserialize(obj));
    }

    return objs || [];
  }

  private extractWeeklyHoursWorkedEntryData(res: WeeklyHoursWorked): WeeklyHoursWorked {
    const objs = new WeeklyHoursWorked().deserialize(res);
    return objs;
  }
}
