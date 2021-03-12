import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { forkJoin } from 'rxjs';
import { ParticipantService } from '../../shared/services/participant.service';
import { Participant } from '../../shared/models/participant';
import { CareerAssessmentService } from './services/career-assessment.service';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-career-assessment',
  templateUrl: './career-assessment.component.html',
  styleUrls: ['./career-assessment.component.scss']
})
export class CareerAssessmentComponent implements OnInit {
  public goBackUrl: string;
  public isReadOnly: boolean;
  public isInEditMode: boolean;
  public canEdit: boolean;
  public canView: boolean;
  public pin: string;
  public careerAssessments: any[];
  public isLoaded = false;
  public participant: Participant;
  constructor(private route: ActivatedRoute, private careerAssessmentService: CareerAssessmentService, private partService: ParticipantService) {}

  ngOnInit() {
    forkJoin(this.route.params.pipe(take(1)), this.careerAssessmentService.modeForCareerAssessment.pipe(take(1)), this.partService.getCurrentParticipant().pipe(take(1))).subscribe(
      result => {
        this.pin = result[0].pin;
        this.goBackUrl = '/pin/' + this.pin;
        this.isReadOnly = result[1].readOnly;
        this.isInEditMode = result[1].isInEditMode;
        this.participant = result[2];
        this.isLoaded = true;
      }
    );
  }
}
