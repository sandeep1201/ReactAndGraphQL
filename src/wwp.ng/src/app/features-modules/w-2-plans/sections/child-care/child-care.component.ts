import { W2PlanSectionResource } from '../../models/w-2-plan.model';
import { Component, OnInit } from '@angular/core';
import { AppService } from 'src/app/core/services/app.service';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { ValidationManager } from 'src/app/shared/models/validation';
import { BaseW2PlansComponent } from '../../base-w2-plans/base-w2-plans.component';
import { PlanSectionTypes, W2PlanSections } from '../../enums/w-2-plans-sections.enum';
import { W2PlanSection } from '../../models/w-2-plan.model';
import { Utilities } from 'src/app/shared/utilities';
import { Router } from '@angular/router';
import { take } from 'rxjs/operators';
import { W2PlansService } from '../../services/w-2-plans.service';

@Component({
  selector: 'app-child-care',
  templateUrl: './child-care.component.html',
  styleUrls: ['./child-care.component.scss']
})
export class ChildCareComponent implements OnInit {
  sectionName = W2PlanSections.ChildCare;
  sectionNameForId = 'ChildCare';
  constructor() {}

  ngOnInit() {}
}
