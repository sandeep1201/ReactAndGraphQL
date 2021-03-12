import { BaseW2PlansComponent } from '../../base-w2-plans/base-w2-plans.component';
import { Component, OnInit } from '@angular/core';
import { take } from 'rxjs/operators';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { W2PlanSection, W2PlanSectionResource } from '../../models/w-2-plan.model';
import { Utilities } from 'src/app/shared/utilities';
import { PlanSectionTypes, W2PlanSections } from '../../enums/w-2-plans-sections.enum';
import { AppService } from 'src/app/core/services/app.service';
import { Router } from '@angular/router';
import { W2PlansService } from '../../services/w-2-plans.service';
import { ValidationManager } from 'src/app/shared/models/validation';

@Component({
  selector: 'app-other-needs',
  templateUrl: './other-needs.component.html',
  styleUrls: ['./other-needs.component.scss']
})
export class OtherNeedsComponent implements OnInit {
  sectionName = W2PlanSections.OtherNeeds;
  sectionNameForId = 'OtherNeeds';
  constructor() {}

  ngOnInit() {}
}
