import { Component, OnInit, ComponentRef, Input, Output, EventEmitter } from '@angular/core';
import { AppService } from './../../core/services/app.service';
import { ModalService } from 'src/app/core/modal/modal.service';
import { Participant } from '../../shared/models/participant';
import { EnrollmentComponent } from '../../enrollment/enrollment/enrollment.component';
import { DisenrollmentComponent } from '../../enrollment/disenrollment/disenrollment.component';
import { ReassignComponent } from '../../enrollment/reassign/reassign.component';
import { TransferComponent } from '../transfer/transfer.component';

@Component({
  selector: 'app-enrollment-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class EnrollmentHeaderComponent implements OnInit {
  @Input()
  pin: string;

  @Input()
  participant: Participant;

  @Output()
  reload = new EventEmitter<boolean>();
  public inEditView = false;
  public regId: string = this.pin;

  private tempModalRefEnrollmentComponent: ComponentRef<EnrollmentComponent>;
  private tempModalRefDisenrollmentComponent: ComponentRef<DisenrollmentComponent>;
  private tempModalReassignComponent: ComponentRef<ReassignComponent>;
  private tempModalTransferComponent: ComponentRef<TransferComponent>;

  constructor(private appService: AppService, private modalService: ModalService) {}

  ngOnInit() {}

  forceUpdate() {
    this.reload.emit(true);
  }

  public enroll() {
    if (this.tempModalRefEnrollmentComponent && this.tempModalRefEnrollmentComponent.instance) {
      this.tempModalRefEnrollmentComponent.instance.destroy();
    }
    this.modalService.create<EnrollmentComponent>(EnrollmentComponent).subscribe(x => {
      this.tempModalRefEnrollmentComponent = x;
      this.tempModalRefEnrollmentComponent.instance.pin = this.pin;
      this.tempModalRefEnrollmentComponent.hostView.onDestroy(() => this.forceUpdate());
    });
  }

  public disenroll() {
    if (this.tempModalRefDisenrollmentComponent && this.tempModalRefDisenrollmentComponent.instance) {
      this.tempModalRefDisenrollmentComponent.instance.destroy();
    }
    this.modalService.create<DisenrollmentComponent>(DisenrollmentComponent).subscribe(x => {
      this.tempModalRefDisenrollmentComponent = x;
      this.tempModalRefDisenrollmentComponent.instance.pin = this.pin;
      this.tempModalRefDisenrollmentComponent.hostView.onDestroy(() => this.forceUpdate());
    });
  }

  public reassign() {
    if (this.tempModalReassignComponent && this.tempModalReassignComponent.instance) {
      this.tempModalReassignComponent.instance.destroy();
    }
    this.modalService.create<ReassignComponent>(ReassignComponent).subscribe(x => {
      this.tempModalReassignComponent = x;
      this.tempModalReassignComponent.instance.pin = this.pin;
      this.tempModalReassignComponent.hostView.onDestroy(() => this.forceUpdate());
    });
  }

  public transfer() {
    if (this.tempModalTransferComponent && this.tempModalTransferComponent.instance) {
      this.tempModalTransferComponent.instance.destroy();
    }
    this.modalService.create<TransferComponent>(TransferComponent).subscribe(x => {
      this.tempModalTransferComponent = x;
      this.tempModalTransferComponent.instance.pin = this.pin;
      this.tempModalTransferComponent.hostView.onDestroy(() => this.forceUpdate());
    });
  }

  addReg(): void {
    // this.regId = 0;
    this.inEditView = true;
  }
  exitReg(): void {
    this.inEditView = false;
  }

  isEnrollEnabled(): boolean {
    if (this.participant == null || this.participant.programs == null) {
      return false;
    } else {
      const currentReferredPrograms = this.participant.getCurrentReferredProgramsByAgency(this.appService.user.agencyCode);

      if (currentReferredPrograms.length === 0) {
        return false;
      } else {
        return this.appService.isUserAuthorizedForAnyProgram(currentReferredPrograms) && this.appService.user.roles !== 'FCDP Query Only Access';
      }
    }
  }

  isDisenrollEnabled(): boolean {
    return this.isUserAuthorizedForAnyCurrentEnrolledProgram(this.participant) && this.appService.user.roles !== 'FCDP Query Only Access';
  }

  // US 3036 enabled when we have enrolled programs and if not FCDP case manager.
  isTransferEnabled(): boolean {
    return (
      this.isUserAuthorizedForAnyCurrentEnrolledProgram(this.participant) &&
      this.appService.user.roles !== 'FCDP Case Manager' &&
      this.appService.user.roles !== 'FCDP Query Only Access'
    );
  }

  isReassignEnabled(): boolean {
    return this.isUserAuthorizedForAnyCurrentEnrolledProgram(this.participant) && this.appService.user.roles !== 'FCDP Query Only Access';
  }

  private isUserAuthorizedForAnyCurrentEnrolledProgram(participant: Participant): boolean {
    if (participant == null || participant.programs == null) {
      return false;
    } else {
      const currentEnrolledPrograms = participant.getCurrentEnrolledProgramsByAgency(this.appService.user.agencyCode);

      if (currentEnrolledPrograms.length === 0) {
        return false;
      } else {
        return this.appService.isUserAuthorizedForAnyProgram(currentEnrolledPrograms);
      }
    }
  }
}
