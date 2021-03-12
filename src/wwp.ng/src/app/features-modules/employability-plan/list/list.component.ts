import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit, OnDestroy, Input } from '@angular/core';

import { PaginationInstance } from 'ng2-pagination';

import { EpOverviewComponent } from '../ep-overview/ep-overview.component';
import * as moment from 'moment';
import * as _ from 'lodash';
import { SubSink } from 'subsink';
import { forkJoin } from 'rxjs';
import { take, concatMap } from 'rxjs/operators';
import { EmployabilityPlanService } from '../services/employability-plan.service';
import { EmployabilityPlan } from '../models/employability-plan.model';
import { AppService } from 'src/app/core/services/app.service';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { Utilities } from 'src/app/shared/utilities';

@Component({
  selector: 'app-employability-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss'],
  providers: [EmployabilityPlanService]
})
export class EmployabilityListComponent implements OnInit, OnDestroy {
  participantProgramCd: string;
  subtractFromCurrentDate: any;
  latestSortedEP: any;
  lastObj: EmployabilityPlan;
  public config: PaginationInstance = {
    id: 'custom',
    itemsPerPage: 10,
    currentPage: 1
  };

  @Input() isReadOnly = true;

  @Input() pin;
  public employabilityPlans: EmployabilityPlan[] = [];
  public epOverviewComponent: EpOverviewComponent;
  public isLoaded = false;
  public hasAnInProgressEp = false;
  public subs = new SubSink();
  resultsForOverView: any;

  constructor(
    private employabilityPlanService: EmployabilityPlanService,
    route: ActivatedRoute,
    private router: Router,
    private appService: AppService,
    private partService: ParticipantService
  ) {}

  ngOnInit() {
    this.appService.cachedEvents.next(null);
    this.appService.isEventsLoaded.next(false);
    this.subs.add(
      this.requestDataFromMultipleSources()
        .pipe(take(1))
        .subscribe(results => {
          this.initEmployabilityPlans(results);
          this.resultsForOverView = results;
          // Search only for submitted Ep's and sort them based on their date. The sort is in ascending order, so we'll have the latest and greatest date at the end.
          this.latestSortedEP = results[0].filter(item => item.employabilityPlanStatusTypeName === 'Submitted');
          if (this.latestSortedEP != null) {
            this.appService.submittedEps.next({ submittedEps: this.latestSortedEP });
          }
        })
    );
  }

  public requestDataFromMultipleSources() {
    return forkJoin([this.employabilityPlanService.getEmployabilityPlans(this.pin).pipe(take(1)), this.partService.getCurrentParticipant().pipe(take(1))]);
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  numberInView(config, p, max: number) {
    return Utilities.numberOfItemsOnCurrentPage(config, p, max);
  }

  public addEpClick() {
    if (this.latestSortedEP != null) {
      this.appService.employabilityPlan.next({ id: 0, submittedEps: this.latestSortedEP });
    } else {
      this.appService.employabilityPlan.next({ id: 0 });
    }
    this.router.navigate(['/pin/' + this.pin + '/employability-plan/0']);
  }
  private goToOverView(p: EmployabilityPlan) {
    this.appService.employabilityPlanInfo.next({ results: this.resultsForOverView });
    this.router.navigateByUrl(`/pin/${this.pin}/employability-plan/overview/${p.id}`);
  }

  autoDeleteInProgressEp(ep: EmployabilityPlan[]) {
    const autoDelete = true;

    this.employabilityPlanService
      .deleteEP(this.pin, ep[0].id.toString(), autoDelete)
      .pipe(
        concatMap(res => {
          return this.employabilityPlanService.getEmployabilityPlans(this.pin);
        })
      )
      .subscribe(res => {
        this.employabilityPlans = res;
        this.isLoaded = true;
        this.hasAnInProgressEp = false;
      });
  }
  private initEmployabilityPlans(data) {
    this.employabilityPlans = data[0];
    const inProgressEP = this.employabilityPlans.filter(i => i.employabilityPlanStatusTypeName === 'In Progress');
    this.hasInProgressEP(data, inProgressEP);
    if (inProgressEP.length > 0) {
      //Get participant program code for the particular pin
      this.participantProgramCd = inProgressEP[0].enrolledProgramCd;
      // Refactor this to take the program code matching the EP that needs to be deleted

      this.employabilityPlanService.getDateForBackDating(this.participantProgramCd).subscribe(maxDate => {
        //Get max number of days that we can backdate from API based on program
        this.subtractFromCurrentDate = maxDate[0].maxDaysInProgressStatus;
        if (maxDate) {
          const past10Day = Utilities.currentDate.subtract(this.subtractFromCurrentDate, 'day');
          //Checking if created date of inprogressEp is before 10 days
          if (moment(inProgressEP[0].createdDate).isBefore(past10Day, 'day')) {
            this.autoDeleteInProgressEp(inProgressEP);
          } else {
            //Is loaded flag set if dates are not matching
            this.isLoaded = true;
          }
        }
      });
    } else {
      //Is loaded flag set if no inprogessEp's are present
      this.isLoaded = true;
    }
  }

  private hasInProgressEP(data: any, inProgressEP) {
    const enrolledPrograms = data[1].getCurrentEnrolledProgramsUserHasAccessTo(this.appService.user, this.appService);
    const inProgressEPsUserHasAccessTo = inProgressEP.filter(ep => this.appService.isUserAuthorizedForProgramByCode(ep.enrolledProgramCd));
    const enrolledProgramsIds = [];
    const inprogessEpProgramIds = [];
    enrolledPrograms.forEach(element => {
      enrolledProgramsIds.push(element.enrolledProgramId);
    });
    inProgressEPsUserHasAccessTo.forEach(element => {
      inprogessEpProgramIds.push(element.enrolledProgramId);
    });
    if (_.isEmpty(_.xor(enrolledProgramsIds, inprogessEpProgramIds))) this.hasAnInProgressEp = true;
  }
}
