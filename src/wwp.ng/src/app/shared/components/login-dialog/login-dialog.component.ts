import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { Subject } from 'rxjs';

import { DestroyableComponent } from 'src/app/core/modal/index';
import { Modal } from 'src/app/shared/components/modal-placeholder/modal-placeholder.component';

@Component({
  selector: 'app-login-dialog',
  templateUrl: './login-dialog.component.html',
  styleUrls: ['./login-dialog.component.css']
})
@Modal()
export class LoginDialogComponent implements DestroyableComponent, OnInit {
  // public loginSubmitted: (authResult: { authorized: boolean, username: string }) => { };
  public loginSubmitted = new Subject<{ authorized: boolean; username: string }>();
  // public loginSubmitted = new EventEmitter<{ authorized: boolean, username: string }>;
  public destroy: Function = () => {};
  public closeDialog: Function = () => {};
  //  @Output() onLoggged = new EventEmitter();
  constructor() {}

  ngOnInit() {}
  onCancel(): void {
    this.closeDialog();
    this.destroy();
  }

  onLoginSubmitted(authResult: { authorized: boolean; username: string }) {
    this.loginSubmitted.next(authResult);
    if (authResult.authorized) {
      this.closeDialog();
      this.destroy();
    }
  }
}
