// tslint:disable: no-shadowed-variable
import { Injectable, ComponentRef } from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { Observable, Subscription, Subject, of } from 'rxjs';
import { EditComponent as EditInformalAssessmentComponent } from '../../participant/informal-assessment/edit/edit.component';
import { ModalService } from 'src/app/core/modal/modal.service';
import { WarningModalComponent } from '../components/warning-modal/warning-modal.component';
import { AppService } from 'src/app/core/services/app.service';
import { take } from 'rxjs/operators';
/**
 *  This is a unsaved changes guard on the Informal Assessment.
 *
 * @static
 * @param
 * @returns
 *
 * @memberOf InformalAssessmentGuard
 */

@Injectable()
export class InformalAssessmentGuard implements CanDeactivate<EditInformalAssessmentComponent> {
  private modalServiceBase: ModalService;
  private tempModalRef: ComponentRef<WarningModalComponent>;
  private navigationAllowedSubject = new Subject<boolean>();
  private modalClosedSub: Subscription;

  constructor(private appService: AppService, private modalService: ModalService) {
    this.modalServiceBase = modalService;
  }

  canDeactivate() {
    const result = this.appService.isUrlChangeBlocked;
    if (!result) {
      return true;
    }

    if (this.tempModalRef && this.tempModalRef.instance) {
      this.tempModalRef.instance.destroy();
    }
    this.modalServiceBase
      .create<WarningModalComponent>(WarningModalComponent)
      .pipe(take(1))
      .subscribe(x => {
        this.modalClosedSub = x.instance.modalClosedSubject.subscribe(x => {
          this.navigationAllowedSubject.next(x);
        });

        this.tempModalRef = x;
        this.tempModalRef.hostView.onDestroy(() => this.allowNavToUrl());
      });

    return this.navigationAllowedSubject;
  }

  private allowNavToUrl() {}

  public presentUnsavedWarning(): Observable<boolean> {
    const result = of(this.appService.isUrlChangeBlocked);
    if (this.tempModalRef && this.tempModalRef.instance) {
      this.tempModalRef.instance.destroy();
    }
    this.modalServiceBase.create<WarningModalComponent>(WarningModalComponent).subscribe(x => {
      this.tempModalRef = x;
      this.tempModalRef.hostView.onDestroy(() => this.allowNavToUrl());
    });

    return result;
  }
}
