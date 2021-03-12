import { Event } from './../../../shared/models/event.model';
import { Injectable } from '@angular/core';
import { AuthHttpClient } from '../../../core/interceptors/AuthHttpClient';
import { BaseService } from '../../../core/services/base.service';
import { AppService } from '../../../core/services/app.service';
import { LogService } from '../../../shared/services/log.service';
import { EmployabilityPlan } from '../models/employability-plan.model';
import { BehaviorSubject, of, Observable } from 'rxjs';
import { Goal } from '../models/goal.model';
import { Activity } from '../models/activity.model';
import { DropDownMultiField } from '../../../shared/models/dropdown-multi-field';
import { WhyReason } from '../../../shared/models/why-reasons.model';
import * as moment from 'moment';
import { EpEmployment } from '../models/ep-employment.model';
import { EndEP } from '../models/end-ep.model';
import { map, catchError, tap } from 'rxjs/operators';
import { ChildCareAuthorizations } from '../models/child-care-authorizations.model';

@Injectable()
export class EmployabilityPlanService extends BaseService {
  maxBackDate: string;
  private epUrl: string;
  private Url: string;
  public types: any;

  public backdate: number;

  public goalsCache: Goal[] = [];
  public EditActivitySection = new BehaviorSubject<any>({ readOnly: true, inEditView: false, showControls: false });
  public EditGoalSection = new BehaviorSubject<any>({ readOnly: true, inEditView: false, inHistory: false });
  public componentDataModified = new BehaviorSubject<any>({ dataModified: false });

  // this is  only for EP.
  public viewDate = new BehaviorSubject<any>({ viewDate: moment().toDate() });

  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService) {
    super(http, logService);
    this.epUrl = this.appService.apiServer + 'api/employability-plan/';
    this.Url = this.appService.apiServer + 'api/';
    this.maxBackDate = this.appService.apiServer + 'api/FieldData/max-days/';
  }

  // tslint:disable: deprecation
  // tslint:disable: no-shadowed-variable

  public findMinAndMaxDatesForActivity(activities: Activity[]) {
    ////**************////////////

    activities.map(activities => {
      let setMaxDateFlag: Boolean;
      let scheduleStartDates = [];
      // Push all schedule start dates to an aray
      activities.activitySchedules.forEach(activitySchedule => {
        scheduleStartDates.push(new Date(activitySchedule.scheduleStartDate));
      });
      // Get the max start date from the array
      // activities.minStartDate = moment(new Date(Math.min.apply(null, scheduleStartDates)).toString()).format('MM/DD/YYYY');
      // If the activity has not ended (Activity doesn't have a completion reason)
      let scheduleEndDates = [];
      if (!activities.activityCompletionReasonId) {
        // If every schedule in the activity is recurring
        if (activities.activitySchedules.every(schedule => schedule.isRecurring === true)) {
          // Push all the schedule end date to an array
          activities.activitySchedules.forEach(schedule => {
            scheduleEndDates.push(new Date(schedule.scheduleEndDate));
          });
        }
        // If all the schedules are non-recurring in an activity
        else if (activities.activitySchedules.every(schedule => schedule.isRecurring === false)) {
          // Push all schedule start dates to an array
          activities.activitySchedules.forEach(schedule => {
            scheduleEndDates.push(new Date(schedule.scheduleStartDate));
          });
        }
        // If the activity has a combination fo recurring and non-recurring schedules
        else {
          scheduleEndDates = [];
          scheduleStartDates = [];
          // Set a flag
          setMaxDateFlag = true;
          activities.activitySchedules.forEach(schedule => {
            scheduleStartDates.push(new Date(schedule.scheduleStartDate));
            // Push all schedule end dates to array
            if (schedule.scheduleEndDate !== null) scheduleEndDates.push(new Date(schedule.scheduleEndDate));
          });
        }
        // If flag set go into this logic to calculate max date from all the start dates and end dates of all the schedules in an activity
        if (setMaxDateFlag) {
          activities.maxEndDate = moment(new Date(Math.max.apply(null, scheduleEndDates)).toString()).format('MM/DD/YYYY');
          const maxStartDate = moment(new Date(Math.max.apply(null, scheduleStartDates)).toString()).format('MM/DD/YYYY');
          activities.maxEndDate = moment.max(moment(maxStartDate), moment(activities.maxEndDate)).format('MM/DD/YYYY');
        }
        // Else just calculate the max end date of the schedules
        else {
          activities.maxEndDate = moment(new Date(Math.max.apply(null, scheduleEndDates)).toString()).format('MM/DD/YYYY');
        }
      }
      ////**************////////////
      // IF THE ACTIVITY IS NOT ENDED YET
      else {
        if (activities.activitySchedules.every(schedule => schedule.isRecurring === true)) {
          // Push all the schedule end date to an array
          activities.activitySchedules.forEach(schedule => {
            scheduleEndDates.push(new Date(schedule.scheduleEndDate));
          });
        }
        // If all the schedules are non-recurring in an activity
        else if (activities.activitySchedules.every(schedule => schedule.isRecurring === false)) {
          // Push all schedule start dates to an array
          activities.activitySchedules.forEach(schedule => {
            scheduleEndDates.push(new Date(schedule.scheduleStartDate));
          });
        }
        // If the activity has a combination fo recurring and non-recurring schedules
        else {
          scheduleEndDates = [];
          scheduleStartDates = [];
          // Set a flag
          setMaxDateFlag = true;
          activities.activitySchedules.forEach(schedule => {
            scheduleStartDates.push(new Date(schedule.scheduleStartDate));
            // Push all schedule end dates to array
            if (schedule.scheduleEndDate !== null) scheduleEndDates.push(new Date(schedule.scheduleEndDate));
          });
        }
        // If flag set go into this logic to calculate max date from all the start dates and end dates of all the schedules in an activity
        if (setMaxDateFlag) {
          activities.maxEndDate = moment(new Date(Math.max.apply(null, scheduleEndDates)).toString()).format('MM/DD/YYYY');
          const maxStartDate = moment(new Date(Math.max.apply(null, scheduleStartDates)).toString()).format('MM/DD/YYYY');
          activities.maxEndDate = moment.max(moment(maxStartDate), moment(activities.maxEndDate)).format('MM/DD/YYYY');
        }
        // Else just calculate the max end date of the schedules
        else {
          activities.maxEndDate = moment(new Date(Math.max.apply(null, scheduleEndDates)).toString()).format('MM/DD/YYYY');
        }
      }
    });
  }

  public filterActivities(activities: Activity[]) {
    activities.forEach(activity => {
      if (activity.activitySchedules.every(schedule => schedule.isRecurring)) {
        return moment(activity.maxEndDate, 'MM/DD/YYYY').isSameOrBefore(moment());
      } else if (activity.activitySchedules.every(schedule => !schedule.isRecurring)) {
        return moment(activity.maxEndDate, 'MM/DD/YYYY').isSameOrBefore(moment());
      } else {
        return moment(activity.maxEndDate, 'MM/DD/YYYY').isSameOrBefore(moment());
      }
    });
  }
  public getEmployabilityPlans(pin: string): Observable<EmployabilityPlan[]> {
    return this.http.get(this.epUrl + pin).pipe(
      tap(res => console.log(res)),
      map(this.extractEpsData),
      catchError(this.handleError)
    );
  }

  public getDateForBackDating(programName): Observable<any> {
    return this.http.get(this.maxBackDate + programName).pipe(map(this.extractMultiFieldData), catchError(this.handleError));
  }

  public getActivities(pin: string, epId: string): Observable<Activity[]> {
    return this.http.get(this.epUrl + pin + '/employment-plan/' + epId + '/activities').pipe(map(this.extractActivitiesData), catchError(this.handleError));
  }
  public getEvents(pin: string, epId: number, programId: number, epBeginDate: string) {
    epBeginDate = new Date(epBeginDate).toJSON();
    return this.http.get(`${this.epUrl}${pin}/get-events/${epId}/${programId}/${epBeginDate}`).pipe(map(this.extractEventsData), catchError(this.handleError));
  }
  public getActivitiesByPep(pin: string, pepId: number): Observable<Activity[]> {
    return this.http.get(this.epUrl + pin + '/employment-plan/activities/pep/' + pepId).pipe(map(this.extractActivitiesData), catchError(this.handleError));
  }

  public getActivitiesForPin(pin: string, fromEvents = false): Observable<Activity[]> {
    return this.http.get(`${this.epUrl}${pin}/employment-plan/activities/${fromEvents}`).pipe(map(this.extractActivitiesData), catchError(this.handleError));
  }

  public getActivity(pin: string, Id: string, epId: string): Observable<Activity> {
    return this.http.get(this.epUrl + pin + '/employment-plan/activity/' + Id + '/' + epId).pipe(map(this.extractActivityData), catchError(this.handleError));
  }
  public deleteActivity(pin: string, id: number, epId: string, fromEndEp: boolean) {
    return this.http.delete(`${this.epUrl}delete/${pin}/employment-plan/${epId}/activity/${id}/${fromEndEp}`).pipe(catchError(this.handleError));
  }
  public deleteEP(pin: string, epId: string, autoDelete: boolean) {
    return this.http.delete(`${this.epUrl}delete/${pin}/employment-plan/${epId}/${autoDelete}`).pipe(catchError(this.handleError));
  }

  public getEpById(pin: string, epId: string): Observable<EmployabilityPlan> {
    return this.http.get(this.epUrl + pin + '/employment-plan/' + epId).pipe(map(this.extractEpDataById), catchError(this.handleError));
  }

  public getGoals(pin: string, epId: string): Observable<Goal[]> {
    return this.http.get(this.epUrl + pin + '/employment-plan/' + epId + '/goals').pipe(map(this.extractGoals), catchError(this.handleError));
  }

  public getGoalsForPin(pin: string): Observable<Goal[]> {
    return this.http.get(this.epUrl + pin + '/employment-plan/goals').pipe(map(this.extractGoals), catchError(this.handleError));
  }

  public getGoal(pin: string, Id: string): Observable<Goal> {
    return this.http.get(this.epUrl + pin + '/employment-plan/goal/' + Id).pipe(map(this.extractGoalData), catchError(this.handleError));
  }

  public deleteGoal(pin: string, id: number, epId: string) {
    return this.http.delete(`${this.epUrl}delete/${pin}/employment-plan/${epId}/goal/${id}`).pipe(catchError(this.handleError));
  }

  public saveEmployabilityPlan(pin: string, employabilityPlan: EmployabilityPlan, subsequentEPId: number) {
    const body = JSON.stringify(employabilityPlan);

    return this.http.post(this.epUrl + pin + '/' + employabilityPlan.id + '/' + subsequentEPId, body).pipe(map(this.extractEpData), catchError(this.handleError));
  }
  public saveEmploymentForEP(employment: EpEmployment[], pin: string, epId: number) {
    const body = JSON.stringify(employment);
    return this.http.post(`${this.epUrl}${pin}/employment-plan/${epId}/employment`, body).pipe(map(this.extractEpEmploymentsData), catchError(this.handleError));
  }
  public getEmploymentForEP(pin: string, epId: number, epBeginDate: string, enrolledProgramCd: string) {
    return this.http
      .get(`${this.epUrl}${pin}/employment-plan/${epId}/employment/${epBeginDate}/${enrolledProgramCd}`)
      .pipe(map(this.extractEpEmploymentsData), catchError(this.handleError));
  }

  public getChildCareAuthorizationsByPin(pin: string): Observable<ChildCareAuthorizations> {
    return this.http.get(`${this.epUrl}${pin}/child-care-authorizations`).pipe(map(this.extractChildCareAuthorizations), catchError(this.handleError));
  }

  public canSaveEP(pin: string, partId: number, isSubmittingEP: boolean, employabilityPlan: EmployabilityPlan): Observable<WhyReason> {
    const body = JSON.stringify(employabilityPlan);
    const url = `${this.epUrl}${pin}/employment-plan/presave/${partId}/${isSubmittingEP}`;
    return this.http.post(url, body).pipe(map(this.extractRuleReasonData), catchError(this.handleError));
  }

  canAddActivity(pin: string, epId: number, activityTypeId?: string): Observable<WhyReason> {
    const reqUrl = `${this.epUrl}${pin}/employment-plan/activity/presave/${epId}/${activityTypeId}`;
    return this.http.post(reqUrl).pipe(
      map(this.extractRuleReasonData),
      catchError(err => {
        this.handleError(err);
        return Observable.throw(err);
      })
    );
  }

  public saveActivity(pin: string, epId: number, activity: Activity, subsequentEPId: number) {
    const body = JSON.stringify(activity);

    return this.http
      .post(this.epUrl + pin + '/employment-plan/' + epId + '/' + 'activity' + '/' + subsequentEPId, body)
      .pipe(map(this.extractActivityData), catchError(this.handleError));
  }

  public saveEndEP(pin: string, epId: number, endEP: EndEP) {
    const body = JSON.stringify(endEP);

    return this.http.post(this.epUrl + pin + '/employment-plan/end-ep/' + epId, body).pipe(map(this.extractEndEPData), catchError(this.handleError));
  }

  public saveElapsedActivties(pin: string, epId: number, elapsedActivties: Activity[], fromEpOverView: boolean) {
    const body = JSON.stringify(elapsedActivties);

    return this.http
      .post(this.epUrl + pin + '/employment-plan/' + epId + '/' + 'elapsed-activities/' + fromEpOverView, body)
      .pipe(map(this.extractActivitiesValue), catchError(this.handleError));
  }

  public saveGoals(pin: string, epId: number, goal: Goal) {
    const body = JSON.stringify(goal);

    return this.http.post(this.epUrl + pin + '/employment-plan/' + epId + '/goal', body).pipe(map(this.extractGoal), catchError(this.handleError));
  }
  public submitEp(pin: string, epId: number) {
    const body = {};
    return this.http.post(`${this.epUrl}${pin}/employment-plan/submit/${epId}`, body).pipe(map(this.extractEpData), catchError(this.handleError));
  }

  private extractChildCareAuthorizations(res: ChildCareAuthorizations): ChildCareAuthorizations {
    const jsonObjs = res as ChildCareAuthorizations;
    const objs = new ChildCareAuthorizations().deserialize(jsonObjs);
    return objs;
  }

  private extractEpData(res: EmployabilityPlan): EmployabilityPlan {
    const jsonObjs = res as EmployabilityPlan;
    const objs = new EmployabilityPlan().deserialize(jsonObjs['value']);
    return objs;
  }
  private extractEventsData(res: Event[]): Event[] {
    const jsonObjs = res as Event[];
    const objs: Event[] = [];

    for (const obj of jsonObjs) {
      objs.push(new Event().deserialize(obj));
    }
    return objs || [];
  }
  private extractEpDataById(res: EmployabilityPlan): EmployabilityPlan {
    const jsonObjs = res as EmployabilityPlan;
    const objs = new EmployabilityPlan().deserialize(jsonObjs);
    return objs;
  }

  private extractEpsData(res: EmployabilityPlan[]): EmployabilityPlan[] {
    const jsonObjs = res as EmployabilityPlan[];
    const objs: EmployabilityPlan[] = [];

    for (const obj of jsonObjs) {
      objs.push(new EmployabilityPlan().deserialize(obj));
    }

    return objs || [];
  }

  private extractGoal(res: Goal): Goal {
    const jsonObjs = res as Goal;
    const objs = new Goal().deserialize(jsonObjs['value']);

    return objs;
  }

  private extractGoals(res: Goal[]): Goal[] {
    const jsonObjs = res as Goal[];
    const objs: Goal[] = [];

    for (const obj of jsonObjs) {
      objs.push(new Goal().deserialize(obj));
    }

    return objs || [];
  }
  private extractEpEmploymentsData(res: EpEmployment[]): EpEmployment[] {
    const body = res as EpEmployment[];
    const employments: EpEmployment[] = [];
    for (const emp of body) {
      employments.push(new EpEmployment().deserialize(emp));
    }
    return employments || null;
  }
  private extractMultiFieldData(res: DropDownMultiField) {
    const body = res;
    return body || [];
  }
  private extractActivityData(res: Activity): Activity {
    const jsonObjs = res as Activity;
    const objs = new Activity().deserialize(jsonObjs['value']);
    return objs;
  }
  private extractActivitiesData(res: Activity[]): Activity[] {
    const jsonObjs = res as Activity[];
    const objs: Activity[] = [];

    for (const obj of jsonObjs) {
      objs.push(new Activity().deserialize(obj));
    }

    return objs || [];
  }

  private extractActivitiesValue(res: Activity[]): Activity[] {
    const jsonObjs = res as Activity[];
    const objs: Activity[] = [];

    for (const obj of jsonObjs) {
      objs.push(new Activity().deserialize(obj));
    }

    return objs || [];
  }

  private extractEndEPData(res: EndEP): EndEP | null {
    const jsonObjs = res as EndEP;
    const objs: EndEP = new EndEP().deserialize(jsonObjs);
    return objs || null;
  }

  private extractGoalData(res: Goal[]): Goal {
    const jsonObjs = res as Goal[];
    const objs = new Goal().deserialize(jsonObjs['value']);
    return objs;
  }
}
