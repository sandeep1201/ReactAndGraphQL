import { Utilities } from './../../shared/utilities';
import { EnrolledProgramCode } from './../../shared/enums/enrolled-program-code.enum';
import { TJTMJEmploymentVerificationModel } from './tj-tmj-employment-verification.model';
import { TJTMJEmploymentVerificationService } from './services/tj-tmj-employment-verification.service';
import { Authorization } from 'src/app/shared/models/authorization';
import { AppService } from './../../core/services/app.service';
import { DropDownField } from './../../shared/models/dropdown-field';
import { FieldDataService } from './../../shared/services/field-data.service';
import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { Participant } from 'src/app/shared/models/participant';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { forkJoin } from 'rxjs/internal/observable/forkJoin';
import { FieldDataTypes } from 'src/app/shared/enums/field-data-types.enum';

@Component({
  selector: 'app-tj-tmj-employment-verification',
  templateUrl: './tj-tmj-employment-verification.component.html',
  styleUrls: ['./tj-tmj-employment-verification.component.scss']
})
export class TjTmjEmploymentVerificationComponent implements OnInit {
  private pin: string;
  public isLoaded = false;
  public isEmploymentsLoaded = false;
  public isSaving = false;
  public isSectionModified = false;
  public participant: Participant;
  public goBackUrl = '';
  public employmentTypeDrop: DropDownField[] = [];
  public employmentTypeId: number;
  public cachedEmploymentTypeID: number;
  public model: TJTMJEmploymentVerificationModel[] = [];
  public cachedModel: TJTMJEmploymentVerificationModel[];
  public verificationDrop: DropDownField[] = [
    { ...new DropDownField(), id: true, name: 'Verified' },
    { ...new DropDownField(), id: false, name: 'Unverified' }
  ];
  public isAcknowledgementShown = false;
  public isAgreed = false;

  constructor(
    private route: ActivatedRoute,
    private partService: ParticipantService,
    private fdService: FieldDataService,
    private appService: AppService,
    private router: Router,
    private cdRef: ChangeDetectorRef,
    private employmentVerificationService: TJTMJEmploymentVerificationService
  ) {}

  ngOnInit() {
    this.pin = this.route.parent.snapshot.paramMap.get('pin');
    this.goBackUrl = '/pin/' + this.pin;

    forkJoin(this.partService.getCachedParticipant(this.pin), this.fdService.getFieldDataByField(FieldDataTypes.JobTypes))
      .pipe(take(1))
      .subscribe(result => {
        this.participant = result[0];
        this.employmentTypeDrop = result[1]
          .filter(i =>
            this.appService.isStateStaff ||
            (this.appService.isUserAuthorized(Authorization.canAccessProgram_TJ) && this.appService.isUserAuthorized(Authorization.canAccessProgram_TMJ))
              ? i.name.indexOf(EnrolledProgramCode.tj.toUpperCase()) > -1 || i.name.indexOf(EnrolledProgramCode.tmj.toUpperCase()) > -1
              : this.appService.isUserAuthorized(Authorization.canAccessProgram_TJ)
              ? i.name.indexOf(EnrolledProgramCode.tj.toUpperCase()) > -1
              : i.name.indexOf(EnrolledProgramCode.tmj.toUpperCase()) > -1
          )
          .reverse();

        this.isLoaded = true;
        this.isEmploymentsLoaded = true;
      });
  }

  getEmployments() {
    this.isSectionModified = false;
    const programCode = Utilities.fieldDataNameById(this.employmentTypeId, this.employmentTypeDrop).split(' (')[0];
    const enrollmentDate =
      programCode === EnrolledProgramCode.tj.toUpperCase() && this.participant.enrolledTJProgram
        ? this.participant.enrolledTJProgram.enrollmentDate
        : programCode === EnrolledProgramCode.tmj.toUpperCase() && this.participant.enrolledTmjProgram
        ? this.participant.enrolledTmjProgram.enrollmentDate
        : null;

    if (enrollmentDate !== null) {
      this.isEmploymentsLoaded = false;
      this.employmentVerificationService
        .getEmploymentsByJobType(this.participant.id, this.employmentTypeId, enrollmentDate)
        .pipe(take(1))
        .subscribe(i => {
          this.model = i;
          this.cachedModel = [];
          this.model.forEach(j => {
            const tjtmjModel = new TJTMJEmploymentVerificationModel();
            TJTMJEmploymentVerificationModel.clone(j, tjtmjModel);
            this.cachedModel.push(tjtmjModel);
          });

          this.isEmploymentsLoaded = true;
          this.isAcknowledgementShown = false;
          this.isAgreed = false;
        });
    } else {
      this.model = [];
      this.cachedModel = [];
    }
  }

  setSectionModified() {
    this.isSectionModified = this.model.some((x, i) => x.isVerified !== this.cachedModel[i].isVerified);
    this.isAcknowledgementShown = this.model.some((x, i) => x.isVerified !== this.cachedModel[i].isVerified && x.isVerified);
    if (!this.isAcknowledgementShown) this.isAgreed = false;
  }

  updateEmployments($event: number) {
    if (this.cachedEmploymentTypeID !== $event) {
      if (this.isSectionModified) {
        this.cdRef.detectChanges();
        this.employmentTypeId = this.cachedEmploymentTypeID;
        this.appService.isDialogPresent = true;
      } else {
        this.getEmployments();
      }
      this.cachedEmploymentTypeID = $event;
    } else this.employmentTypeId = this.cachedEmploymentTypeID;
  }

  exitEditIgnoreChanges() {
    if (this.employmentTypeId !== this.cachedEmploymentTypeID) {
      this.employmentTypeId = this.cachedEmploymentTypeID;
      this.getEmployments();
    } else this.router.navigateByUrl(this.goBackUrl);
  }

  cancelDialog() {
    this.cachedEmploymentTypeID = this.employmentTypeId;
  }

  cancel() {
    if (this.isSectionModified) {
      this.appService.isDialogPresent = true;
    } else this.router.navigateByUrl(this.goBackUrl);
  }

  save() {
    const changedEmployments = this.model.filter((x, i) => x.isVerified !== this.cachedModel[i].isVerified);
    const updatedEmployments = changedEmployments.map(x => ({ ...x, numberOfDaysAtVerification: x.calculatedEmploymentDays })) as TJTMJEmploymentVerificationModel[];
    if (updatedEmployments.length && updatedEmployments.length > 0) {
      this.isSaving = true;
      this.employmentVerificationService.saveEmploymentVerification(this.pin, updatedEmployments).subscribe(
        res => {
          this.isSaving = false;
          this.router.navigateByUrl(this.goBackUrl);
        },
        error => (this.isSaving = false)
      );
    } else this.router.navigateByUrl(this.goBackUrl);
  }
}
