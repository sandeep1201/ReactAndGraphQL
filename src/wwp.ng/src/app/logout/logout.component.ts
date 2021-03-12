import { CdoLogService } from './../shared/services/cdo-log.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AppService } from '../core/services/app.service';
import { NotificationsService } from 'angular2-notifications';
import { SystemClockService } from '../shared/services/system-clock.service';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent implements OnInit {
  constructor(private router: Router, private appService: AppService, private cdoService: CdoLogService, private notificationService: NotificationsService) {}

  ngOnInit() {
    if (this.cdoService.cachedSimulatedDate) {
      this.cdoService.postCurrentDateOverrideLog(this.cdoService.cachedSimulatedDate).subscribe(data => {
        this.cdoService.cachedSimulatedDate = null;
        SystemClockService.cancelSimulateClientDateTime();
        this.cancelSimulation();
        this.logout();
      });
    } else if (this.appService.isUserSimulated) {
      this.appService.stopUserSimulation();
      this.cancelSimulation();
      this.logout();
    } else this.logout();
  }

  logout() {
    this.appService.logoutUser();
    this.router.navigateByUrl('login');
  }

  cancelSimulation() {
    this.notificationService.remove();
    this.notificationService.info('Simulation', 'Simulation cancelled!');
  }
}
