import { W2PlansService } from './../services/w-2-plans.service';
import { Utilities } from './../../../shared/utilities';
import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AppService } from 'src/app/core/services/app.service';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { Participant } from 'src/app/shared/models/participant';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { W2PlanSections } from '../enums/w-2-plans-sections.enum';
import { forkJoin } from 'rxjs';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss']
})
export class EditComponent implements OnInit {
  isSectionNeedingValidation = false;
  isModelLoaded = false;
  isSidebarCollapsed = false;
  goBackUrl: string;
  pin: string;
  id: number;
  isInEditMode = false;
  isSectionLoaded = false;
  isSaving = false;
  participant: Participant;
  goToUrl: W2PlanSections = null;
  //Setting this property for the use in template..
  importedW2PlanSections = W2PlanSections;
  activeSection = W2PlanSections.EmergencyNeeds;
  public planTypeModel: DropDownField[] = [];
  public header = '';
  public planTypeId: number;
  public planSections: DropDownField[];

  constructor(
    private route: ActivatedRoute,
    public appService: AppService,
    private partService: ParticipantService,
    private router: Router,
    private w2PlansService: W2PlansService
  ) {}

  ngOnInit() {
    this.pin = this.route.snapshot.params.pin;
    this.id = +this.route.snapshot.params.id;

    this.goBackUrl = `/pin/${this.pin}/w-2-plans/overview/${this.id}`;
    forkJoin(this.partService.getCachedParticipant(this.pin).pipe(take(1)), this.w2PlansService.routeState.pipe(take(1))).subscribe(results => {
      this.participant = results[0];
      this.planTypeModel = results[1].planTypeModel;
      this.planTypeId = results[1].planTypeId;
      this.planSections = results[1].planSections;
      this.setHeader();
      this.isModelLoaded = true;
    });
  }

  setHeader() {
    if (this.id > 0) {
      if (this.planTypeId === Utilities.idByFieldDataName('SSI/SSDI Transition Plan', this.planTypeModel)) {
        this.header = 'Edit SSI/SSDI Transition Plan';
      } else if (this.planTypeId === Utilities.idByFieldDataName('Supportive Service Plan', this.planTypeModel)) {
        this.header = 'Edit Supportive Service Plan';
      }
    } else if (this.id === 0) {
      if (this.planTypeId === Utilities.idByFieldDataName('SSI/SSDI Transition Plan', this.planTypeModel)) {
        this.header = 'New SSI/SSDI Transition Plan';
      } else if (this.planTypeId === Utilities.idByFieldDataName('Supportive Service Plan', this.planTypeModel)) {
        this.header = 'New Supportive Service Plan';
      }
    }
  }
  exitEAEditIgnoreChanges() {}
  navigateTo(section: W2PlanSections) {
    this.w2PlansService.routeState.next({ planSections: this.planSections, pin: this.pin, planTypeId: this.planTypeId, participantId: this.participant.id });
    this.router.navigateByUrl(`/pin/${this.pin}/w-2-plans/edit/${this.id}/${section}`);
  }
  goToSection(section: W2PlanSections) {
    this.goToUrl = section;
    this.navigateTo(section);
  }
}
