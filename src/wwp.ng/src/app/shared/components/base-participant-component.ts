import { Employment } from './../models/work-history-app';
import { Observable, Subscription } from 'rxjs';
import { ActivatedRoute, Router } from '@angular/router';
import { BaseComponent } from './base-component';
import { Participant } from '../models/participant';
import { ParticipantService } from '../../shared/services/participant.service';
import * as moment from 'moment';
import { UpdateSectionCallback } from '../../shared/types/update-section-callback';
import { AppHistoryManager } from '../../shared/models/app-history-manager';
import { ParticipantBarrier } from '../../shared/models/participant-barriers-app';
import { Utilities } from '../utilities';

// TODO: Might be better to remove this component and merge it's props and methods into part's service.

// A base class that loads a participant object from the pin on the URL.
export class BaseParticipantComponent extends BaseComponent {
  public participant: Participant;
  public participantDOB: moment.Moment;
  public participantDOBMMmYyyy: moment.Moment;
  public participantDOBPlus120: moment.Moment;
  public enrolledDate: moment.Moment;
  public disenrolledDate: moment.Moment;
  public pin: string;
  protected routeSub: Subscription;
  protected partSub: Subscription;
  public isParticipantInfoLoaded = false;
  public isHistoryActive = false;
  public cachedSection: any;
  public appHistoryManager: AppHistoryManager;

  // By default, we will look up the agency/office for the participant by the Enrolled Program. Otherwise use
  // the refresh/local value in the participant table
  public usePEPAgency = true;
  public caseNumber: number;

  constructor(protected route: ActivatedRoute, protected router: Router, protected partService: ParticipantService) {
    super();
  }

  onInit() {
    this.routeSub = this.route.params.subscribe(params => {
      this.pin = params['pin'];
      // If not found in child url look in parent.
      // TODO: Look for better way to handle this.
      if (this.pin == null && this.route.parent != null) {
        this.routeSub = this.route.parent.params.subscribe(prms => {
          this.pin = prms['pin'];
        });
      }

      if (this.pin == null) {
        console.warn('could not get PIN');
        return;
      }

      this.onPinInit();
      let $participant: Observable<Participant>;
      if (this.usePEPAgency) {
        $participant = this.partService.getParticipant(this.pin);
      } else {
        $participant = this.partService.getParticipant(this.pin, true, false);
      }

      this.partSub = $participant.subscribe(part => {
        this.initParticipant(part);
        this.onParticipantInit();
      });
    });
  }

  get participantAge(): number {
    const dob = moment(this.participantDOB, moment.ISO_8601);
    if (dob.isValid()) {
      return Utilities.currentDate.diff(dob, 'years');
    } else {
      return 0;
    }
  }

  // Override this to know when the PIN property has been set.
  onPinInit() {}

  // Override this to know when the Participant property has been set.
  onParticipantInit() {}

  protected initParticipant(data: Participant) {
    this.participant = data;
    if (this.participant != null) {
      if (this.participant.programs != null && this.participant.programs[0] != null) {
        this.caseNumber = this.participant.currentEnrolledProgram.caseNumber;
        this.enrolledDate = moment(this.participant.currentEnrolledProgram.enrollmentDate);
        if (!this.enrolledDate.isValid()) {
          console.warn("Participant's date of enrollment is invalid");
          // alert('Participant\'s date of enrollment is invalid');
        }
      } else {
        console.warn('Participant is missing date of enrollment');
        //  alert('Participant is missing date of enrollment');
      }

      this.disenrolledDate = moment(this.participant.disenrolledDate);

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
      this.isParticipantInfoLoaded = true;
    }
  }

  onDestroy() {
    if (this.partSub != null) {
      this.partSub.unsubscribe();
    }
    if (this.routeSub != null) {
      this.routeSub.unsubscribe();
    }
  }

  getHistory(routeName: string, id: number) {
    this.partService.getHistoryBySection(id, routeName, this.pin).subscribe(data => this.initHistory(data, routeName));
  }

  protected toggleHistory($event: any, sectionName: string, section: any, id: number, usc: UpdateSectionCallback) {
    this.isHistoryActive = $event;
    if (this.isHistoryActive === true) {
      this.cachedSection = section;
      this.getHistory(sectionName, id);
    } else {
      if (this.cachedSection) {
        usc(this.cachedSection);
      }
    }
  }

  private initHistory(jsonString: string, routeName: string) {
    this.appHistoryManager = new AppHistoryManager(this.partService, this.pin);
    if (jsonString != null) {
      const sections = jsonString;
      for (const section of sections) {
        let sectionHistory;

        switch (routeName) {
          case 'participant-barriers-app':
            sectionHistory = new ParticipantBarrier();
            break;

          case 'work-history-app':
            sectionHistory = new Employment();
            break;
        }

        this.appHistoryManager.history.push(sectionHistory.deserialize(section));
      }
      this.appHistoryManager.initHistoryDrop();
    }
  }
}
