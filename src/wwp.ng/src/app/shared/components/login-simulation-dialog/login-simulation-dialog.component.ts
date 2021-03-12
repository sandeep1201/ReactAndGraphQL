import { Component, OnInit } from '@angular/core';
import { NotificationsService, Notification } from 'angular2-notifications';
import { forkJoin, Subscription } from 'rxjs';
import { take, finalize } from 'rxjs/operators';

import { AppService } from 'src/app/core/services/app.service';
import { AgencyService } from '../../services/agency.service';
import { IMultiSelectOption } from '../multi-select-dropdown/multi-select-dropdown.component';
import { DestroyableComponent } from 'src/app/core/modal/index';
import { ModalService } from 'src/app/core/modal/modal.service';
import { Modal } from 'src/app/shared/components/modal-placeholder/modal-placeholder.component';

@Component({
  selector: 'app-login-simulation-dialog',
  templateUrl: './login-simulation-dialog.component.html',
  styleUrls: ['./login-simulation-dialog.component.css'],
  providers: [AgencyService]
})
@Modal()
export class LoginSimulationDialogComponent implements OnInit, DestroyableComponent {
  private sumulationNotificationSub: Subscription;
  errorMessage: string;
  roles: { name: string; code: string }[] = [];
  public rolesDropOptions: IMultiSelectOption[] = [];
  public usernamesDropOptions: IMultiSelectOption[] = [];
  public officesDropOptions: IMultiSelectOption[] = [];
  public selectedRoles: string[] = [];
  public isSubmitting = false;
  public selectedUserIndex: number;
  public selectedOfficeIndex: number;
  public usernameMap = new Map<number, string>();
  public rolesMap = new Map<number, string>();
  public agencyMap = new Map<number, string>();
  private notification: Notification;

  public destroy: Function = () => {};
  public closeDialog: Function = () => {};
  public get username(): string {
    return this.usernameMap.get(this.selectedUserIndex);
  }
  public get canSubmit() {
    return !!this.username || (this.selectedRoles && this.selectedRoles.length > 0) || this.selectedOfficeIndex > 0;
  }

  constructor(private appService: AppService, private agencyService: AgencyService, private notificationService: NotificationsService, private modalService: ModalService) {}

  ngOnInit() {
    const appRoles = this.appService.getAppRoles().pipe(take(1));
    const usernames = this.appService.getUsernames().pipe(take(1));
    const agencies = this.agencyService.getAgencies().pipe(take(1));

    forkJoin(appRoles, usernames, agencies).subscribe(x => {
      this.roles = x[0];

      let i = 1;
      this.roles.map(y => {
        this.rolesMap.set(i, y.code);
        this.rolesDropOptions.push({ description: y.name, name: y.name, id: i++, disablesOthers: false, isDisabled: false });
      });

      i = 1;
      x[1].map(x => {
        this.usernameMap.set(i, x);
        this.usernamesDropOptions.push({ description: x, name: x, id: i++, disablesOthers: false, isDisabled: false });
      });

      i = 1;
      (<any>x[2]).map(x => {
        if (x && x.agencyName && x.agencyCode) {
          this.agencyMap.set(i, x.agencyCode);
          const name = `${x.agencyName} (${x.agencyCode})`;
          this.officesDropOptions.push({ description: name, name: name, id: i++, disablesOthers: false, isDisabled: false });
        }
      });
    });
  }

  exit() {
    this.closeDialog();
    this.destroy();
  }

  submit() {
    this.isSubmitting = true;
    this.errorMessage = null;
    let rolesCodes: string[] = null;
    if (this.selectedRoles.length) {
      rolesCodes = [];
      this.selectedRoles.map(x => {
        const roleCode = this.rolesMap.get(+x);
        rolesCodes.push(roleCode);
      });
    }
    const username = this.username || this.appService.user.username;
    const orgCode = this.agencyMap.has(this.selectedOfficeIndex) ? this.agencyMap.get(this.selectedOfficeIndex) : null;
    this.appService
      .simulateUser(username, rolesCodes, orgCode)
      .pipe(
        take(1),
        finalize(() => (this.isSubmitting = false))
      )
      .subscribe(isAuth => {
        if (isAuth) {
          if (this.sumulationNotificationSub) {
            this.sumulationNotificationSub.unsubscribe();
          }
          const appService = this.appService;
          this.notification = this.notificationService.warn('Simulation', `Simulating user: <b>${username}</b>`, { timeOut: 0, clickToClose: false });
          this.sumulationNotificationSub = this.notification.click.subscribe(x => {
            appService.stopUserSimulation();
            this.notificationService.remove(this.notification.id);
            this.notificationService.info('Simulation', 'Simulation cancelled!');
          });

          this.exit();
        } else {
          this.errorMessage = this.appService.authStatus;
        }
      });
  }

  onDestroy() {
    if (this.sumulationNotificationSub) {
      this.sumulationNotificationSub.unsubscribe();
    }
  }
}
