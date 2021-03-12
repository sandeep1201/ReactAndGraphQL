import { Participant } from './../../models/participant';
import { ParticipantService } from './../../services/participant.service';
import { Component } from '@angular/core';
import { Router, ActivationEnd } from '@angular/router';
import { AppService } from 'src/app/core/services/app.service';
import { forkJoin, of } from 'rxjs';
import * as moment from 'moment';
import { take, concatMap } from 'rxjs/operators';
import { WorkerTaskService } from './worker-task.service';
import { Utilities } from '../../utilities';

@Component({
  selector: 'app-worker-task',
  templateUrl: './worker-task.component.html',
  styleUrls: ['./worker-task.component.scss']
})
export class WorkerTaskComponent {
  public participantPin: string;
  public isPinBased = false;
  public isInEditMode = false;
  public hideWorkerTask = false;
  public commentId: number;
  public showWorkerTaskFeature = false;
  public participant = new Participant();

  constructor(private router: Router, public appService: AppService, public workerTaskService: WorkerTaskService, public partService: ParticipantService) {
    this.router.events.subscribe(val => {
      if (val instanceof ActivationEnd) {
        // Everytime the router changes, lets clear pin.
        this.participantPin = val.snapshot.params['pin'] ? val.snapshot.params['pin'] : null;
        this.workerTaskService.participantPin = this.participantPin;
        this.isPinBased = !!this.participantPin;
        this.hideWorkerTask = val.snapshot.routeConfig.path === 'pin/:pin/rfa';
      }
      if (!Utilities.isStringEmptyOrNull(this.participantPin) && !this.hideWorkerTask) this.onInit();
    });
  }

  onInit() {
    const feature = 'WorkerTaskList';
    forkJoin(this.workerTaskService.modeForWorkerTask.pipe(take(1)), this.appService.featureToggles.pipe(take(1)), this.partService.getCachedParticipant(this.participantPin))
      .pipe(
        take(1),
        concatMap(results => {
          this.isInEditMode = results[0].isInEditMode;
          this.participant = results[2];
          if (results[1] && results[1].length > 0) {
            this.showWorkerTaskFeature = this.appService.getFeatureToggleDate(feature);
            return of(null);
          } else return this.appService.getFeatureToggleValues();
        })
      )
      .subscribe(res => {
        if (res) {
          const featureToggleDate = res.filter(i => i.parameterName === feature)[0].parameterValue;
          this.showWorkerTaskFeature = moment().isSameOrAfter(moment(featureToggleDate));
          this.appService.featureToggles.next(res);
        }
      });
  }

  onAdd() {
    this.isInEditMode = true;
    this.workerTaskService.modeForWorkerTask.next({ id: 0, readOnly: false, isInEditMode: this.isInEditMode, participant: this.participant });
  }

  closeEdit(e: boolean) {
    this.isInEditMode = e;
  }
}
