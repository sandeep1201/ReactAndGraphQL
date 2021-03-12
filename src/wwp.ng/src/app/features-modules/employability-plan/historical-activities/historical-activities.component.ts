import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { PaginationInstance } from 'ng2-pagination';
// tslint:disable-next-line: import-blacklist
import { forkJoin } from 'rxjs';
import { take } from 'rxjs/operators';
import { Activity } from '../models/activity.model';
import { Participant } from 'src/app/shared/models/participant';
import { EmployabilityPlanService } from '../services/employability-plan.service';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { AppService } from 'src/app/core/services/app.service';
import { Utilities } from 'src/app/shared/utilities';

@Component({
  selector: 'app-historical-activities',
  templateUrl: './historical-activities.component.html',
  styleUrls: ['./historical-activities.component.scss']
})
export class HistoricalActivitiesComponent implements OnInit {
  public activities: Activity[];
  public participant: Participant;
  public isLoaded = false;
  public pin: string;
  public goBackUrl: string;
  public config: PaginationInstance = {
    id: 'custom',
    itemsPerPage: 10,
    currentPage: 1
  };

  constructor(
    private router: Router,
    private route: ActivatedRoute,
    private employabilityPlanService: EmployabilityPlanService,
    private partService: ParticipantService,
    private appService: AppService
  ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.pin = params.pin;
    });
    return forkJoin([this.partService.getCurrentParticipant(), this.employabilityPlanService.getActivitiesForPin(this.pin).pipe(take(1))])
      .pipe(take(1))
      .subscribe(result => {
        this.participant = result[0];
        this.activities = result[1];
        this.employabilityPlanService.findMinAndMaxDatesForActivity(this.activities);
        this.goBackUrl = '/pin/' + this.pin + '/employability-plan/list';
        this.isLoaded = true;
      });
  }

  numberInView(config, p, max: number) {
    return Utilities.numberOfItemsOnCurrentPage(config, p, max);
  }

  singleEntry(a) {
    this.employabilityPlanService.EditActivitySection.next({ readOnly: true, inEditView: true, showControls: false, activity: a });
    this.appService.inHistoryMode.next({ inHistory: true });
    this.router.navigateByUrl(`pin/${this.pin}/employability-plan/activities/${a.employabilityPlanId}`);
  }
}
