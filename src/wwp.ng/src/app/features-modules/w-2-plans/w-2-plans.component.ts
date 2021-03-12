import { EnrolledProgramStatus } from './../../shared/enums/enrolled-program-status.enum';
import { DropDownField } from './../../shared/models/dropdown-field';
import { FieldDataTypes } from 'src/app/shared/enums/field-data-types.enum';
import { W2PlansService } from './services/w-2-plans.service';
import { FieldDataService } from './../../shared/services/field-data.service';
import { forkJoin } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from 'src/app/core/services/app.service';
import { Participant } from 'src/app/shared/models/participant';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { take, concatMap } from 'rxjs/operators';
import { W2Plan } from './models/w-2-plan.model';
import { W2PlansStatusCodes } from './models/w-2-plans-status.enum';
import { Authorization } from 'src/app/shared/models/authorization';

@Component({
  selector: 'app-w-2-plans',
  templateUrl: './w-2-plans.component.html',
  styleUrls: ['./w-2-plans.component.scss']
})
export class W2PlansComponent implements OnInit {
  public isLoaded = false;
  public goBackUrl: string;
  private pin: string;
  public participant: Participant;
  public model: W2Plan[];
  public planTypeModel: DropDownField[] = [];
  public planSections: DropDownField[] = [];

  constructor(
    private route: ActivatedRoute,
    private partService: ParticipantService,
    private router: Router,
    private appService: AppService,
    private fdService: FieldDataService,
    private planService: W2PlansService
  ) {}

  ngOnInit() {
    this.pin = this.route.snapshot.params.pin;
    this.goBackUrl = '/pin/' + this.pin;

    this.partService
      .getCachedParticipant(this.pin)
      .pipe(
        concatMap(participant => {
          this.participant = participant;
          return forkJoin(
            this.fdService.getFieldDataByField(FieldDataTypes.PlanTypes),
            this.planService.getW2PlansByParticipant(this.participant.id),
            this.fdService.getFieldDataByField(FieldDataTypes.PlanSections)
          );
        })
      )
      .pipe(take(1))
      .subscribe(results => {
        this.planTypeModel = results[0];
        this.model = results[1];
        this.planSections = results[2];
        const mostRecentPrograms = this.participant.getMostRecentProgramsUserHasAccessTo(this.appService.user, this.appService);
        const isUserInSameAgency =
          mostRecentPrograms &&
          !!mostRecentPrograms.find(x => [EnrolledProgramStatus.enrolled, EnrolledProgramStatus.referred].includes(x.status.toLowerCase() as EnrolledProgramStatus));
        this.planTypeModel.forEach(
          x =>
            (x.canEdit =
              this.model &&
              !this.model.find(y => y.planTypeId === x.id && y.planStatusTypeCode === W2PlansStatusCodes.InProgress) &&
              this.appService.isUserAuthorized(Authorization.canAccessW2Plans_Edit) &&
              isUserInSameAgency)
        );
        this.isLoaded = true;
      });
  }

  getPlansByPlanType(planTypeId: number): W2Plan[] {
    return this.model && this.model.length > 0 ? this.model.filter(x => x.planTypeId === planTypeId) : [];
  }

  single(item) {
    this.router.navigateByUrl(`${this.goBackUrl}/w-2-plans/overview/${item.id}`);
  }

  onAdd(planTypeId: number) {
    const id = 0;
    this.planService.routeState.next({
      planSections: this.planSections,
      planTypeModel: this.planTypeModel,
      planTypeId: planTypeId,
      pin: this.pin,
      participantId: this.participant.id
    });
    this.router.navigateByUrl(`${this.goBackUrl}/w-2-plans/edit/${id}/emergency-needs`);
  }
}
