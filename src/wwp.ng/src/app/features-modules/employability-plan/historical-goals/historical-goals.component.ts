import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { PaginationInstance } from 'ng2-pagination';
// tslint:disable-next-line: import-blacklist
import { forkJoin } from 'rxjs';
import { take } from 'rxjs/operators';
import { EmployabilityPlanService } from '../services/employability-plan.service';
import { Goal } from '../models/goal.model';
import { Participant } from 'src/app/shared/models/participant';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { Utilities } from 'src/app/shared/utilities';

@Component({
  selector: 'app-historical-goals',
  templateUrl: './historical-goals.component.html',
  styleUrls: ['./historical-goals.component.scss']
})
export class HistoricalGoalsComponent implements OnInit {
  public goals: Goal[];
  public participant: Participant;
  public isLoaded = false;
  public pin: string;
  public goBackUrl: string;

  public config: PaginationInstance = {
    id: 'custom',
    itemsPerPage: 10,
    currentPage: 1
  };

  constructor(private router: Router, private route: ActivatedRoute, private employabilityPlanService: EmployabilityPlanService, private partService: ParticipantService) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.pin = params.pin;
    });
    return forkJoin([this.partService.getCurrentParticipant(), this.employabilityPlanService.getGoalsForPin(this.pin).pipe(take(1))])
      .pipe(take(1))
      .subscribe(result => {
        this.participant = result[0];
        this.goals = result[1];
        this.goBackUrl = '/pin/' + this.pin + '/employability-plan/list';
        this.isLoaded = true;
      });
  }

  numberInView(config, p, max: number) {
    return Utilities.numberOfItemsOnCurrentPage(config, p, max);
  }

  singleEntry(a) {
    this.employabilityPlanService.EditGoalSection.next({ readOnly: true, inEditView: true, goal: a, inHistory: true });
    this.router.navigateByUrl(`pin/${this.pin}/employability-plan/goals/${a.employabilityPlanId}`);
  }
}
