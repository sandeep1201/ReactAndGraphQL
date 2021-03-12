import { CareerAssessment } from './../models/career-assessment.model';
import { Utilities } from './../../../shared/utilities';
import { FieldDataService } from './../../../shared/services/field-data.service';
import { DropDownField } from './../../../shared/models/dropdown-field';
import { Component, OnInit, Input } from '@angular/core';
import { ParticipantService } from '../../../shared/services/participant.service';
import { Participant } from '../../../shared/models/participant';
import * as _ from 'lodash';
import { EnrolledProgram } from '../../../shared/models/enrolled-program.model';
import { CareerAssessmentService } from '../services/career-assessment.service';
import { AppService } from 'src/app/core/services/app.service';

@Component({
  selector: 'app-career-assessment-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class CareerAssessmentListComponent implements OnInit {
  public currentlyEnrolledPrograms: EnrolledProgram[];
  @Input() pin: string;
  @Input() isReadOnly = false;

  @Input() participant: Participant;

  public isInEditMode = false;
  public careerAssessments: CareerAssessment[];
  public localCareerAssessments: any[] = [];
  public isLoaded = false;
  public canAdd = false;
  public canView = true;
  public canEdit = false;
  public careerAssessmentId: number;
  public elementsDrop: DropDownField[];
  constructor(
    private participantService: ParticipantService,
    private appService: AppService,
    private careerAssessmentService: CareerAssessmentService,
    private fdService: FieldDataService
  ) {}

  ngOnInit() {
    this.careerAssessmentService.modeForCareerAssessment.subscribe(res => {
      this.careerAssessmentId = res.id;
      this.isInEditMode = res.isInEditMode;
      if (!this.isInEditMode) {
        this.getAllCareerAssessmentsForPin();
      }
    });
  }
  getAllCareerAssessmentsForPin() {
    this.careerAssessmentService.getAllCareerAssessmentsForPin(this.pin).subscribe(res => {
      this.careerAssessments = res;
      this.initElementDrop();
      this.getAccessToCareerAssessments();
    });
  }
  initElementDrop() {
    this.fdService.getElement().subscribe(res => {
      this.elementsDrop = res;
      this.careerAssessments.forEach(ca => {
        ca.elementIds.forEach(e => {
          ca.elementNames.push(Utilities.fieldDataNameById(e, this.elementsDrop));
        });
      });
      this.isLoaded = true;
    });
  }

  onAdd() {
    this.isInEditMode = true;
    this.careerAssessmentId = 0;
    this.careerAssessmentService.modeForCareerAssessment.next({ id: 0, readonly: false, isInEditMode: this.isInEditMode });
  }
  edit(a, readonly) {
    this.isInEditMode = true;
    this.careerAssessmentId = a.id;
    this.careerAssessmentService.modeForCareerAssessment.next({ id: a.id, readonly: readonly, isInEditMode: this.isInEditMode });
  }
  public getAccessToCareerAssessments() {
    const programs = this.participant.programs;
    const programsUserHasAccessTo = this.participant.getMostRecentProgramsByAgency(this.appService.user.agencyCode);
    this.currentlyEnrolledPrograms = programsUserHasAccessTo.filter(p => p.status === 'Enrolled');
    if (this.appService.isMostRecentProgramInSisterOrg(programs)) {
      this.canView = true;
    }
    if (this.currentlyEnrolledPrograms.length > 0 && this.appService.coreAccessContext.canEdit) {
      // this.currentlyEnrolledPrograms.some(p => {
      //   if (p.enrolledProgramId && this.appService.isUserAuthorized(Authorization[`canAccessProgram_${p.enrolledProgramCode.trim()}`])) {
      //     this.canEdit = true;
      //   }
      // });
      this.canEdit = true;
      this.canAdd = true;
    }
    this.isLoaded = true;
  }
}
