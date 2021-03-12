import { ChildCareAuthorizations } from './../models/child-care-authorizations.model';
import { EmployabilityPlanService } from './../services/employability-plan.service';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-childcare-authorizations',
  templateUrl: './childcare-authorizations.component.html',
  styleUrls: ['./childcare-authorizations.component.scss']
})
export class ChildCareAuthorizationsComponent implements OnInit {
  @Input() pin: string;
  @Output() closeModal = new EventEmitter<boolean>();
  public model: ChildCareAuthorizations;
  public isLoaded = false;

  constructor(private employabilityPlanService: EmployabilityPlanService) {}

  ngOnInit() {
    this.employabilityPlanService
      .getChildCareAuthorizationsByPin(this.pin)
      .pipe(take(1))
      .subscribe(res => {
        this.model = res;
        this.isLoaded = true;
      });
  }
}
