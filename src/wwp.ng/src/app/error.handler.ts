import { ErrorHandler, Injectable, ComponentRef } from '@angular/core';
import { ErrorInfo } from './shared/models/ErrorInfoContract';
import { NotificationsService } from 'angular2-notifications';
import { LogService } from './shared/services/log.service';
import { ModalService } from 'src/app/core/modal/modal.service';
import { ErrorModalComponent } from './shared/components/error-modal/error-modal.component';
import { NGXLogger } from 'ngx-logger';
import { version } from './shared/version';
import { Utilities } from './shared/utilities';

// Future Hook for Global Error Handling.
@Injectable()
export class AppErrorHandler implements ErrorHandler {
  private tempModalRefErrorComponent: ComponentRef<ErrorModalComponent>;
  // Tracks the number of errors.
  private numberOfErrors = 0;
  readonly maxNumberOfErrors = 5;
  constructor(private logService: LogService, private notificationsService: NotificationsService, private modalService: ModalService, private logger: NGXLogger) {}

  handleError(error) {
    this.numberOfErrors += 1;

    // TODO: remove this errorMSG crap.
    let errorMsg = 'Error 100';
    if (typeof error === 'string') {
      errorMsg = error;
    } else if (typeof error === 'object') {
      if (error.message != null) {
        errorMsg = error.message + ' - ' + error.details;
      } else if (error.status != null) {
        errorMsg = error.status + ' - ' + error.statusText;
      }
    }

    // This contract is sent the log api.
    const errorContract = new ErrorContract();

    // ErrorInfo is our HTTP error.
    if (error instanceof ErrorInfo) {
      errorContract.frontEnd = false;
      if (error.code === 409) {
        // Handle 409.
        this.display409Toast();
        errorContract.code = error.code;
        errorContract.errorMsg = error.message;
      } else {
        errorContract.code = error.code;
        errorContract.errorMsg = error.message;
        this.displayFatalErrorToast(error.code, error.message);
      }
    } else if (error instanceof ReferenceError) {
      errorContract.frontEnd = true;
      errorContract.code = 0;
      errorContract.stackTrace = error.stack;
      errorContract.errorMsg = error.message;
    } else if (error instanceof TypeError) {
      errorContract.frontEnd = true;
      errorContract.code = 1;
      errorContract.stackTrace = error.stack;
      errorContract.errorMsg = error.message;
    } else {
      // Unhandled Error.
      const x = this.notificationsService.error('Fatal Error', errorMsg, {
        timeOut: 0,
        maxLength: 3
      });
      errorContract.errorMsg = errorMsg;
    }

    // Lets set Session info if we have it.
    const username = localStorage.getItem('username');
    if (!Utilities.isStringEmptyOrNull(username)) {
      errorContract.username = username;
    }

    if (version != null) {
      errorContract.version = version.toString();
    }

    errorContract.url = window.location.href;

    // TODO: add debounce or throttle.
    this.logger.error(JSON.stringify(errorContract), 'Error');
    // this.logService.fatal(errorMsg);
    // this.logService.event('error', { error: errorMsg });

    // Prevents endless error loop.
    if (this.numberOfErrors > this.maxNumberOfErrors) {
      this.openErrorModal();
    }
  }
  public openErrorModal() {
    //this.modalService.
    if (this.tempModalRefErrorComponent && this.tempModalRefErrorComponent.instance) {
      this.tempModalRefErrorComponent.instance.destroy();
    }
    this.modalService.create<ErrorModalComponent>(ErrorModalComponent).subscribe(x => {
      this.tempModalRefErrorComponent = x;
      this.tempModalRefErrorComponent.hostView.onDestroy(() => this.stopApp());
    });
  }

  stopApp() {
    throw new Error('Something went badly wrong!');
  }

  private display409Toast() {
    const x = this.notificationsService.error('Save Conflict', 'Another user has already <br /> modified this page.<br /> Please refresh your page.', {
      timeOut: 0,
      maxLength: 3
    });
  }

  private displayFatalErrorToast(title: string | number, msg: string) {
    const x = this.notificationsService.error(title, msg, {
      timeOut: 0,
      maxLength: 3
    });
  }
}

export class ErrorContract {
  public frontEnd: boolean;
  public code = 0;
  public username = '';
  public version = '';
  public url = '';
  public errorMsg = '';
  public stackTrace = '';
}
