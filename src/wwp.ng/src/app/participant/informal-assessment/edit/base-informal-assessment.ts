import { ComponentRef, HostListener } from '@angular/core';

import { Participant } from '../../../shared/models/participant';
import { HelpComponent } from 'src/app/core/components/help/help.component';
import { ModalService } from 'src/app/core/modal/modal.service';
import { InformalAssessmentEditService } from '../../../shared/services/informal-assessment-edit.service';

import * as moment from 'moment';

export abstract class BaseInformalAssessmentSecton {
  public isSectionActive = true;
  public isSectionValid = true;
  public isSectionLoaded = false;
  public cloneModelString: string;
  public participant: Participant;
  public participantDOB: moment.Moment;
  public participantDOBMMmYyyy: moment.Moment;
  public participantDOBPlus120: moment.Moment;

  private tempModalRef: ComponentRef<HelpComponent>;

  private modalServiceBase: ModalService;

  public eiaServiceBase: InformalAssessmentEditService;

  public help(val: string) {
    console.warn('Help base method invoked from parent component: ' + typeof this + 'with argument "' + val + '". This shoul be overrided in the component.');
  }

  constructor(modalService, eiaService) {
    this.modalServiceBase = modalService;
    this.eiaServiceBase = eiaService;
  }

  openHelp(): void {
    if (this.tempModalRef && this.tempModalRef.instance) {
      this.tempModalRef.instance.destroy();
    }
    this.modalServiceBase.create<HelpComponent>(HelpComponent).subscribe(x => {
      this.tempModalRef = x;
    });
  }

  scrollToTop() {
    window.scroll(0, 0);
  }

  sectionLoaded() {
    this.isSectionLoaded = true;
    this.scrollToTop();
  }

  initParticipantModel(data: Participant) {
    this.participant = data;

    if (this.participant != null) {
      if (this.participant.dateOfBirth != null) {
        this.participantDOB = moment(this.participant.dateOfBirth);
        this.participantDOBMMmYyyy = this.participantDOB.clone();
        this.participantDOBMMmYyyy.date(1);
        this.participantDOBPlus120 = this.participantDOB.clone();
        this.participantDOBPlus120.add(120, 'y');
        if (!this.participantDOB.isValid()) {
          console.warn("Participant's date of birth is invalid");
        }
      } else {
        console.warn('Participant is missing date of birth');
      }
    }
  }

  @HostListener('document:keydown', ['$event'])
  onKeyDown(evt) {
    if (evt.which === 8 && evt.target.nodeName !== 'TEXTAREA' && evt.target.nodeName !== 'INPUT' && evt.target.nodeName !== 'SELECT') {
      evt.preventDefault();
    }
  }

  disableIaHotKeys(value) {
    this.eiaServiceBase.disableHotKeys = value;
  }
}
