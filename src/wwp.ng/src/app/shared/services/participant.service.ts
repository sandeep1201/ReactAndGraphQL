import { Person } from './../models/person.model';
// tslint:disable: no-shadowed-variable
// tslint:disable: no-redundant-jsdoc
import { Injectable } from '@angular/core';
import { Observable, Subject, of, throwError, BehaviorSubject } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { AppService } from './../../core/services/app.service';
import { EnrolledProgram } from '../models/enrolled-program.model';
import { WhyReason } from '../models/why-reasons.model';
import { Participant, ParticipantDetails } from '../models/participant';
import { AuthHttpClient } from './../../core/interceptors/AuthHttpClient';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Status } from '../models/status.model';
import { ParticipationStatus } from '../models/participation-statuses.model';
import { DropDownMultiField } from '../models/dropdown-multi-field';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable()
export class ParticipantService {
  maxBackDate: string;
  private participant: Participant;
  private participantsUrl: string;
  private participantUrl: string;
  private historyAppUrl: string;
  private participantPin: string;
  public actionSubmitted = new Subject<any>();
  public modeForParticipationStatuses = new BehaviorSubject<any>({
    readOnly: false,
    inEditView: false
  });

  public isFromPartSumGuard = new BehaviorSubject<boolean>(false);

  constructor(private router: Router, private route: ActivatedRoute, private http: AuthHttpClient, private appService: AppService) {
    this.participantsUrl = this.appService.apiServer + 'api/participants/';
    this.participantUrl = this.appService.apiServer + 'api/participant/';
    this.historyAppUrl = this.appService.apiServer + 'api/history/app/';
    this.maxBackDate = this.appService.apiServer + 'api/FieldData/max-days/';
  }

  /**
   * Gets the current participant based on the URL. Use case would be
   * with guards.
   *
   * @returns {Observable<Participant>}
   * @memberof ParticipantService
   */
  setAction(value: any) {
    this.actionSubmitted.next(value);
  }

  getCurrentParticipant(): Observable<Participant> {
    // First get the lastest pin from the URL.
    this.getParticipantPinFromURL();
    return this.getParticipant(this.participantPin, false);
  }

  public getDaysForBackDating(programName): Observable<any> {
    return this.http.get(this.maxBackDate + programName).pipe(map(this.extractMultiFieldData), catchError(this.handleError));
  }

  getRecentParticipants(): Observable<Participant[]> {
    if (!this.appService.user) {
      console.warn('this.appService.user is NULL in ParticipantService::getRecentParticipants()');
    }

    return this.http.get(this.participantsUrl + this.appService.user.username + '/recent').pipe(map(this.extractParticipantsData), catchError(this.handleError));
  }

  getParticipantsByWorkerByProgram(wamsId: string, program: string, agency: string): Observable<Participant[]> {
    if (!this.appService.user) {
      console.warn('this.appService.user is NULL in ParticipantService::getRecentParticipants()');
    }

    return this.http.get(this.participantsUrl + wamsId + '/' + agency + '/' + program).pipe(map(this.extractParticipantsData), catchError(this.handleError));
  }

  getParticipantsByWorker(wamsId: string, agency: string): Observable<Participant[]> {
    if (!this.appService.user) {
      console.warn('this.appService.user is NULL in ParticipantService::getRecentParticipants()');
    }

    return this.http.get(this.participantsUrl + wamsId + '/' + agency).pipe(map(this.extractParticipantsData), catchError(this.handleError));
  }

  getReferredParticipants(): Observable<Participant[]> {
    if (!this.appService.user) {
      throw new Error('this.appService.user is NULL in ParticipantService::getRecentParticipants()');
    }

    const url = this.participantsUrl + this.appService.user.username + '/referrals/';

    return this.http.get(url).pipe(
      map(x => {
        return this.extractParticipantsData(x);
      }),
      catchError(this.handleError)
    );
  }

  public getCachedParticipant(pin: string, isFromPartSumGuard = false): Observable<Participant> {
    // First check if we have a cached participant.  If we do and the PINs
    // match then just return it.
    if (this.participant != null && this.participant.pin === pin) {
      return of(this.participant);
    }

    // OK, we don't have valid one so let's request a refreshed one.
    return this.getParticipant(pin, true, true, isFromPartSumGuard);
  }

  public isUserAuthorized(pin: string): Observable<boolean> | Promise<boolean> {
    let isAuthorzed = new Observable<boolean>();
    this.getPepsDataBasedonPinOrProgram(pin, 'WW').subscribe(parts => {
      let isValid = false;
      if (parts) {
        parts.forEach(part => {
          if (part.agencyCode === this.appService.user.agencyCode) {
            if (this.appService.isUserAuthorizedToViewNonParticipationDetails(part)) {
              isValid = true;
            } else {
              isValid = false;
            }
          }
        });
      }
      if (isValid) {
        isAuthorzed = of(true);
      } else {
        isAuthorzed = of(false);
      }
    });
    return isAuthorzed;
  }

  clearCachedParticipant() {
    this.participant = null;
  }

  getParticipant(pin: string, refresh = false, usePepAgency = true, isFromPartSumGuard = false): Observable<Participant> {
    this.isFromPartSumGuard.next(isFromPartSumGuard);

    let requestUrl = this.participantUrl + pin;

    if (refresh) {
      requestUrl += '/refresh';
    }

    if (usePepAgency) {
      requestUrl += '/?usePEPAgency=true';
    } else {
      requestUrl += '/?usePEPAgency=false';
    }

    return this.http.get(requestUrl).pipe(
      map((x: Participant) => this.extractParticipantData(x)),
      catchError(err => {
        return this.handleError(err);
      })
    );
  }

  getParticipantsBySearch(person: Person): Observable<Participant[]> {
    let requestUrl = `${this.participantsUrl}search/?firstName=${person.firstName}&lastName=${person.lastName}`;
    if (person.middleInitial) requestUrl += `&middleName=${person.middleInitial}`;
    if (person.gender) requestUrl += `&gender=${person.gender}`;
    if (person.dateOfBirthMmDdYyyy) requestUrl += `&dob=${person.dateOfBirthMmDdYyyy}`;
    return this.http.get(requestUrl).pipe(
      map((x: Participant[]) => this.extractParticipantsData(x)),
      catchError(err => {
        return this.handleError(err);
      })
    );
  }

  getCurrentStatusesForPin(pin: string) {
    const requestUrl = `${this.participantUrl}${pin}/status/current`;
    return this.http.get(requestUrl).pipe(
      map(res => this.extractStatusesData(res)),
      catchError(err => {
        return this.handleError(err);
      })
    );
  }
  getAllStatusesForPin(pin: string) {
    const requestUrl = `${this.participantUrl}${pin}/status/all`;
    return this.http.get(requestUrl).pipe(
      map(res => this.extractAllParticipationStatusesData(res)),
      catchError(err => {
        return this.handleError(err);
      })
    );
  }
  getPSStatusById(pin: string, id: number) {
    const requestUrl = `${this.participantUrl}${pin}/status/${id}`;
    return this.http.get(requestUrl).pipe(
      map(res => this.extractStatusesData(res)),
      catchError(err => {
        return this.handleError(err);
      })
    );
  }
  addStatus(pin: string, ParticipationStatus: ParticipationStatus) {
    return this.http.post(this.participantUrl + pin + '/status/add', ParticipationStatus).pipe(
      map(this.extractStatusesData),
      catchError(err => {
        this.handleError(err);
        return throwError(err);
      })
    );
  }
  updateStatus(pin: string, ParticipationStatus: ParticipationStatus) {
    return this.http.post(this.participantUrl + pin + '/status/update', ParticipationStatus).pipe(
      map(this.extractStatusesData),
      catchError(err => {
        this.handleError(err);
        return throwError(err);
      })
    );
  }

  getParticipantSummaryDetails(pin: string, fromSummary = false): Observable<ParticipantDetails> {
    const requestUrl = `${this.participantUrl}${pin}/details/?fromSummary=${fromSummary}`;

    return this.http.get(requestUrl).pipe(
      map((x, i) => this.extractParticipantDetailsData(x)),
      catchError(err => {
        return this.handleError(err);
      })
    );
  }
  getPepsDataBasedonPinOrProgram(pin: string, programName?: string): Observable<any> {
    let requestUrl: string;
    if (programName) {
      requestUrl = this.appService.apiServer + 'api/pep/' + pin + '/program/' + programName;
    } else {
      requestUrl = this.appService.apiServer + 'api/pep/' + pin + '/program';
    }
    return this.http.get(requestUrl).pipe(
      map(res => {
        const body = res as any;
        return body;
      }),
      catchError(err => {
        this.handleError(err);
        return throwError(err);
      })
    );
  }

  enrollParticipant(enrolledProgram: EnrolledProgram): Observable<Status> {
    return this.http.put(`${this.participantUrl}enroll`, enrolledProgram).pipe(
      map(
        res => this.extractStatusData(res),
        catchError(err => {
          this.handleError(err);
          return throwError(err);
        })
      )
    );
  }

  canTransferParticipant(pin: string, enrolledProgram: EnrolledProgram): Observable<WhyReason> {
    return this.http.post(this.participantUrl + pin + '/pretransfer', enrolledProgram).pipe(
      map(this.extractRuleReasonData),
      catchError(err => {
        this.handleError(err);
        return throwError(err);
      })
    );
  }

  transferParticipant(pin: string, enrolledProgram: EnrolledProgram): Observable<Participant> {
    const putUrl = this.participantUrl + pin + '/transfer';

    return this.http.put(putUrl, enrolledProgram).pipe(
      catchError(err => {
        this.handleError(err);
        return throwError(err);
      })
    );
  }

  reassignParticipant(enrolledProgram: EnrolledProgram): Observable<Participant> {
    return this.http.put(this.participantUrl + 'reassign', enrolledProgram).pipe(
      map(this.extractEnrolledProgramData),
      catchError(err => this.handleError(err))
    );
  }

  canEnrollParticipant(pin: string, enrolledProgramId: number): Observable<WhyReason> {
    return this.http.get(this.participantUrl + pin + '/preenroll/' + enrolledProgramId).pipe(
      map(this.extractRuleReasonData),
      catchError(err => {
        this.handleError(err);
        return throwError(err);
      })
    );
  }

  canDisenrollParticipant(pin: string, enrolledProgramId: number): Observable<WhyReason> {
    return this.http.get(this.participantUrl + pin + '/predisenroll/' + enrolledProgramId).pipe(
      map(this.extractRuleReasonData),
      catchError(err => {
        this.handleError(err);
        return throwError(err);
      })
    );
  }
  canAddPS(pin: string, psc: ParticipationStatus): Observable<WhyReason> {
    const requestUrl = `${this.participantUrl}${pin}/status/validate`;
    return this.http.post(requestUrl, psc).pipe(
      map(this.extractRuleReasonData),
      catchError(err => {
        this.handleError(err);
        return throwError(err);
      })
    );
  }

  disenrollParticipant(enrolledProgram: EnrolledProgram): Observable<Participant> {
    return this.http.put(this.participantUrl + 'disenroll', enrolledProgram).pipe(
      map(this.extractEnrolledProgramData),
      catchError(err => this.handleError(err))
    );
  }

  private cacheParticipant(refresh = false): Observable<Participant> {
    return this.getParticipant(this.participantPin, false);
  }

  private extractMultiFieldData(res: DropDownMultiField[]) {
    const body = res as DropDownMultiField[];
    return body || [];
  }

  private extractEnrolledProgramData(res: EnrolledProgram) {
    const body = res as EnrolledProgram;
    const enrolledProgram = new EnrolledProgram().deserialize(body);
    return enrolledProgram || null;
  }

  private extractRuleReasonData(res: WhyReason) {
    const body = res as WhyReason;
    const whyReason = new WhyReason().deserialize(body);
    return whyReason || null;
  }

  private extractStatusesData(res: ParticipationStatus[]) {
    const body = res as ParticipationStatus[];
    const participationStatus = new ParticipationStatus().deserialize(body);
    return participationStatus || null;
  }

  private extractAllParticipationStatusesData(res: ParticipationStatus[]) {
    const jsonObjs = res as ParticipationStatus[];
    const objs: ParticipationStatus[] = [];

    for (const obj of jsonObjs) {
      objs.push(new ParticipationStatus().deserialize(obj));
    }

    return objs || [];
  }

  private extractParticipantData(res: Participant) {
    const body = res as Participant;
    // Whenever we extract particpant data, store the object in a cached value.
    this.participant = new Participant().deserialize(body);
    return this.participant || null;
  }

  private extractParticipantDetailsData(res: ParticipantDetails) {
    const participantDetails = res as ParticipantDetails;
    return participantDetails || null;
  }

  private extractParticipantsData(res: Participant[]) {
    const body = res as Participant[];
    const participants: Participant[] = [];
    for (const p of body) {
      participants.push(new Participant().deserialize(p));
    }
    return participants || null;
  }

  private extractStatusData(res: Status): Status {
    const obj = new Status().deserialize(res as Status);
    return obj || null;
  }

  private handleError(error: any): Observable<any> {
    if (error instanceof HttpErrorResponse) {
      return throwError(error);
    } else {
      // In a real world app, we might use a remote logging infrastructure
      // We'd also dig deeper into the error to get a better message
      const errMsg = error.message ? error.message : error.status ? `${error.status} - ${error.statusText}` : 'Server error';
      return throwError(errMsg);
    }
  }

  private getParticipantPinFromURL() {
    this.participantPin = null;
    this.participantPin = this.route.snapshot.params.pin;

    this.route.params.subscribe((params: Params) => {
      const pin = params.pin;
    });

    // Hack begins for child routes if the above fails.
    if (this.participantPin == null) {
      const urlString = window.location.href;
      const urlArr = urlString.split('/');
      const iPin = urlArr.indexOf('pin') + 1;

      if (iPin) {
        this.participantPin = urlArr[iPin];
      }
    }
  }

  public getHistoryBySection(id: number, section: string, pin: string): Observable<any> {
    return this.http.get(this.historyAppUrl + pin + '/' + section + '/' + id).pipe(
      map(this.extractSectionData),
      catchError(err => this.handleError(err))
    );
  }

  private extractSectionData(res: Response) {
    const body = res;
    return body || null;
  }
}
