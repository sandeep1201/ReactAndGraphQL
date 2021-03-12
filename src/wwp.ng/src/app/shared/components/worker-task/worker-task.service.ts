import { BaseService } from 'src/app/core/services/base.service';
import { EnrolledProgram } from 'src/app/shared/models/enrolled-program.model';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
import { AppService } from 'src/app/core/services/app.service';
import { Authorization } from '../../models/authorization';
import { Participant } from '../../models/participant';
import { EnrolledProgramStatus } from '../../enums/enrolled-program-status.enum';
import { WorkerTask } from 'src/app/features-modules/worker-task/models/worker-task.model';
import { LogService } from '../../services/log.service';

@Injectable()
export class WorkerTaskService extends BaseService {
  public participantPin: string;
  public modeForWorkerTask = new BehaviorSubject<any>({ id: null, readOnly: false, isInEditMode: false });
  private workerTaskUrl: string;

  constructor(http: AuthHttpClient, logService: LogService, private appService: AppService) {
    super(http, logService);
    this.workerTaskUrl = this.appService.apiServer + 'api/worker-task/';
  }

  public getWorkerTaskList(wiuid): Observable<WorkerTask[]> {
    return this.http.get(`${this.workerTaskUrl}${wiuid}`).pipe(map(this.extractWorkerTaskList), catchError(this.handleError));
  }

  public reassignWorker(id, workerId) {
    return this.http.put(`${this.workerTaskUrl}${id}/${workerId}`).pipe(catchError(this.handleError));
  }

  public saveCompletedWorkerTask(workerTask: WorkerTask) {
    const body = JSON.stringify(workerTask);

    return this.http.post(this.workerTaskUrl, body).pipe(
      map(this.extractWorkerTask),
      catchError(e => {
        return this.handleError(e);
      })
    );
  }

  public saveWorkerTask(workerTask: WorkerTask) {
    const body = JSON.stringify(workerTask);

    return this.http.post(this.workerTaskUrl, body).pipe(map(this.extractWorkerTask), catchError(this.handleError));
  }

  private extractWorkerTaskList(res: WorkerTask[]): WorkerTask[] {
    const jsonObjs = res as WorkerTask[];
    const objs: WorkerTask[] = [];
    for (const obj of jsonObjs) {
      objs.push(new WorkerTask().deserialize(obj));
    }
    return objs || [];
  }

  private extractWorkerTask(res: WorkerTask): WorkerTask {
    const body = res as WorkerTask;
    const workerTask = new WorkerTask().deserialize(body);
    return workerTask || null;
  }

  public canAccess(participant: Participant): boolean {
    return (
      this.appService.isUserAuthorized(Authorization.canAccessWorkerTask_Edit) &&
      participant &&
      (this.appService.isUserEASupervisor() ||
        this.appService.isUserEAWorker() ||
        EnrolledProgram.filterByStatuses(participant.getMostRecentProgramsUserHasAccessTo(this.appService.user, this.appService), [EnrolledProgramStatus.enrolled]).length > 0)
    );
  }
}
