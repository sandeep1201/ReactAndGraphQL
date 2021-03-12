import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { Activity } from '../../models/activity.model';
import { EmployabilityPlanService } from '../../services/employability-plan.service';

@Component({
  selector: 'app-single',
  templateUrl: './single.component.html',
  styleUrls: ['./single.component.scss']
})
export class ActivitySingleComponent implements OnInit {
  public pin: string;
  public activityId: string;
  public activity: Activity;
  public isLoaded = false;
  constructor(private route: ActivatedRoute, private router: Router, private employabilityPlanService: EmployabilityPlanService) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      this.pin = params.pin;
      this.activityId = params.id;
      this.getActivityById();
    });
  }

  getActivityById() {
    this.employabilityPlanService.getActivity(this.pin, this.activityId, this.activity.employabilityPlanId.toString()).subscribe(res => {
      this.activity = res;
      this.isLoaded = true;
    });
  }

  cancel() {
    this.router.navigateByUrl(`/pin/${this.pin}/employability-plan/overview/${this.activity.employabilityPlanId}`);
  }
}
