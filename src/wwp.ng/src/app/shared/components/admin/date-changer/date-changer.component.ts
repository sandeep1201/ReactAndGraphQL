import { NotificationsService, Notification } from 'angular2-notifications';
import { SystemClockService } from '../../../services/system-clock.service';
import { Component, OnInit } from '@angular/core';
import { Modal } from 'src/app/shared/components/modal-placeholder/modal-placeholder.component';
import { DestroyableComponent } from 'src/app/core/modal/index';

import { Subscription, forkJoin } from 'rxjs';
import { ValidationManager } from '../../../../shared/models/validation';
import { ModelErrors } from '../../../../shared/interfaces/model-errors';
import { SimulatedDate } from '../../../models/simulated-date.model';
import { CdoLogService } from '../../../services/cdo-log.service';
import { take } from 'rxjs/operators';
import { AppService } from 'src/app/core/services/app.service';

@Component({
  selector: 'app-date-changer',
  templateUrl: 'date-changer.component.html',
  styleUrls: ['date-changer.component.css']
})
@Modal()
export class DateChangerComponent implements DestroyableComponent, OnInit {
  public isTimeSimulated: boolean;
  private notification: Notification;
  private simulationNotificationSub: Subscription;
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public isSectionModified = false;
  public hasTriedSave = false;
  public isSectionValid = true;
  public isSaving = false;
  public disableSave = false;
  public modelErrors: ModelErrors = {};
  public originalDate: SimulatedDate = new SimulatedDate();
  public model: SimulatedDate;
  public destroy: Function = () => {};
  public closeDialog: Function = () => {};

  constructor(private notificationService: NotificationsService, public appService: AppService, private cdoLogService: CdoLogService) {
    this.model = SimulatedDate.create();
    this.model.cdoDate = SystemClockService.appDateTime.format('MM/DD/YYYY');

    SimulatedDate.clone(this.model, this.originalDate);

    this.isTimeSimulated = SystemClockService.isTimeSimulated;
  }

  ngOnInit() {}

  save() {
    this.hasTriedSave = true;
    this.isSaving = true;
    this.validate();
    if (this.isSectionValid === true) {
      this.saveAndExit();
    }
  }

  saveAndExit() {
    SystemClockService.simulateClientDateTime(this.model);

    forkJoin(
      this.appService.simulateDate(this.appService.user.username, null, this.appService.user.agencyCode, this.model.cdoDate).pipe(take(1)),
      this.cdoLogService.postCurrentDateOverrideLog(this.model).pipe(take(1))
    )
      .pipe(take(1))
      .subscribe(results => {
        this.cdoLogService.cachedSimulatedDate = new SimulatedDate();
        this.cdoLogService.cachedSimulatedDate = results[1];
      });

    this.notification = this.notificationService.warn('Date Simulation', `Simulating date: ${SystemClockService.appDateTime.format('MM/DD/YYYY')}`, {
      timeOut: 0,
      clickToClose: false
    });
    this.simulationNotificationSub = this.notification.click.pipe(take(1)).subscribe(x => {
      // this.modalService.create<DateChangerComponent>(DateChangerComponent);
      if (this.cdoLogService.cachedSimulatedDate) {
        this.cdoLogService.postCurrentDateOverrideLog(this.cdoLogService.cachedSimulatedDate).subscribe(data => {
          this.cdoLogService.cachedSimulatedDate = null;
        });
      }
      this.appService.stopDateSimulation();
      SystemClockService.cancelSimulateClientDateTime();
      this.notificationService.remove(this.notification.id);
      this.notificationService.info('Date Simulation', 'Date simulation cancelled!');
    });
    this.exit();
  }

  public validate() {
    this.isSectionModified = true;
    if (this.hasTriedSave === true) {
      this.validationManager.resetErrors();
      const result = this.model.validate(this.validationManager);

      this.isSectionValid = result.isValid;
      this.modelErrors = result.errors;
      if (this.modelErrors) {
        this.isSaving = false;
      }
    }
  }

  cancelSimulation() {
    SystemClockService.cancelSimulateClientDateTime();
    this.notificationService.remove();
    this.notificationService.info('Canceled', 'Date simulation cancelled.');
    this.notificationService.success('Date reset', `Date reset to: <br> ${SystemClockService.appDateTime.format('MM/DD/YYYY')}`);
    this.exit();
  }

  exit() {
    this.closeDialog();
    this.destroy();
  }

  onDestroy() {
    if (this.simulationNotificationSub) {
      this.simulationNotificationSub.unsubscribe();
    }
  }
}
