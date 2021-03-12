import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-auxiliary',
  template: `
    <app-aux-approver-list *ngIf="!isParticipantAuxiliary && !isW2AuxApprover"></app-aux-approver-list>
    <app-aux-worker-list *ngIf="isParticipantAuxiliary"></app-aux-worker-list>
    <app-w2-auxiliary-approvers *ngIf="!isParticipantAuxiliary && isW2AuxApprover"></app-w2-auxiliary-approvers>
  `
})
export class AuxiliaryComponent {
  public isParticipantAuxiliary = false;
  public isW2AuxApprover = false;

  constructor(private router: Router) {
    this.isParticipantAuxiliary = this.router.url.includes('pin');
    this.isW2AuxApprover = this.router.url.indexOf('w2-auxiliary-approvers') > 0;
  }
}
