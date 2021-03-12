import { BaseW2PlansComponent } from '../../base-w2-plans/base-w2-plans.component';
import { Component, OnInit } from '@angular/core';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { ValidationManager } from 'src/app/shared/models/validation';
import { PlanSectionTypes, W2PlanSections } from '../../enums/w-2-plans-sections.enum';
import { AppService } from 'src/app/core/services/app.service';
import { Router } from '@angular/router';
import { W2PlansService } from '../../services/w-2-plans.service';
import { take } from 'rxjs/operators';
import { W2PlanSection, W2PlanSectionResource } from '../../models/w-2-plan.model';
import { Utilities } from 'src/app/shared/utilities';

@Component({
  selector: 'app-education-and-training',
  templateUrl: './education-and-training.component.html',
  styleUrls: ['./education-and-training.component.scss']
})
export class EducationAndTrainingComponent implements OnInit {
  sectionName = W2PlanSections.EducationAndTraining;
  sectionNameForId = 'EducationAndTraining';

  constructor() {}

  ngOnInit() {}
}