import { Component, OnInit } from '@angular/core';

import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
import { DestroyableComponent } from 'src/app/core/modal/index';
import { Modal } from 'src/app/shared/components/modal-placeholder/modal-placeholder.component';

@Component({
  selector: 'app-timeout-dialog',
  templateUrl: './timeout-dialog.component.html',
  styleUrls: ['./timeout-dialog.component.css']
})
@Modal()
export class TimeoutDialogComponent implements DestroyableComponent, OnInit {
  public timeoutMessage = 'shortly';

  public destroy: Function = () => {};

  constructor(private authHttpClient: AuthHttpClient) {}

  ngOnInit() {
    this.initCountdown();
  }

  resetTimer() {
    this.authHttpClient.markUserAsActive();
    this.destroy();
  }

  initCountdown(): void {
    setInterval(() => {
      const sec = this.authHttpClient.getSecondsUntilInactive();

      // We don't want to be too granular here with the message because the timer that actually
      // does the logging out (in the header component) has a latency of 10 seconds.
      if (sec < 1) {
        this.timeoutMessage = 'in a few seconds';
      } else if (sec < 30) {
        this.timeoutMessage = 'in less than 30 seconds';
      } else if (sec < 60) {
        this.timeoutMessage = 'in less than 1 minute';
      } else if (sec < 120) {
        this.timeoutMessage = 'in less than 2 minutes';
      } else if (sec < 180) {
        this.timeoutMessage = 'in less than 3 minutes';
      } else if (sec < 300) {
        this.timeoutMessage = 'in less than 5 minutes';
      }
    }, 5000); // Fire the timer ever 5 seconds.
  }
}
