import { Injectable } from '@angular/core';
import { Utilities } from '../utilities';

@Injectable()
export class UserPreferencesService {

  private isLocalStorageSupported = false;
  constructor() {
    this.isLocalStorageSupported = this.checkIfLocalStorageSupported();
  }


  set isRfaSidebarCollapsed(value: boolean) {
    if (!this.isLocalStorageSupported) {
      return;
    }

    let valueString = '';
    if (value === true) {
      valueString = 'true';
    } else {
      valueString = 'false';
    }
    localStorage.setItem('isRfaSidebarCollapsed', valueString);
  }

  get isRfaSidebarCollapsed(): boolean {
    let valueString = '';
    if (this.isLocalStorageSupported) {
      valueString = localStorage.getItem('isRfaSidebarCollapsed');
    }
    if (valueString === 'true') {
      return true;
    } else {
      return false;
    }
  }

  set currentHomeTab(value: string) {
    if (!this.isLocalStorageSupported) {
      return;
    }
    localStorage.setItem('currentHomeTab', value);
  }

  get currentHomeTab(): string {
    let valueString = '';
    if (this.isLocalStorageSupported) {
      valueString = localStorage.getItem('currentHomeTab');
    }
    if (!Utilities.isStringEmptyOrNull(valueString)) {
      return valueString;
    } else {
      return 'recentTab';
    }
  }


  private checkIfLocalStorageSupported() {
    try {
      const itemBackup = localStorage.getItem('');
      localStorage.removeItem('');
      localStorage.setItem('', itemBackup);
      if (itemBackup === null) {
        localStorage.removeItem('');
      } else {
        localStorage.setItem('', itemBackup);
      }
      return true;
    } catch (e) {
      return false;
    }
  }

  // TODO: Dry out methods to store values via types.

}
