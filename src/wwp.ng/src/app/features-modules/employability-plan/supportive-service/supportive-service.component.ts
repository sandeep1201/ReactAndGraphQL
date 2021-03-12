import { FieldDataTypes } from './../../../shared/enums/field-data-types.enum';
// tslint:disable: import-blacklist
import { Router, ActivatedRoute } from '@angular/router';
import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { PaginationInstance } from 'ng2-pagination';
import { forkJoin, Subscription } from 'rxjs';
import { take } from 'rxjs/operators';
import { FieldDataService } from 'src/app/shared/services/field-data.service';
import { SupportiveServiceService } from 'src/app/features-modules/employability-plan/services/supportive-service.service';
import { SupportiveService } from 'src/app/features-modules/employability-plan/models/supportive-service.model';
import { DropDownField } from 'src/app/shared/models/dropdown-field';
import { ValidationManager, ValidationResult } from 'src/app/shared/models/validation';
import { ModelErrors } from 'src/app/shared/interfaces/model-errors';
import { AppService } from 'src/app/core/services/app.service';
import { EmployabilityPlanService } from '../services/employability-plan.service';
import { Utilities } from 'src/app/shared/utilities';
import * as _ from 'lodash';

@Component({
  selector: 'app-supportive-service',
  templateUrl: './supportive-service.component.html',
  styleUrls: ['./supportive-service.component.scss'],
  providers: [FieldDataService, SupportiveServiceService]
})
export class SupportiveServiceComponent implements OnInit {
  public config: PaginationInstance = {
    id: 'custom',
    itemsPerPage: 5,
    currentPage: 1
  };

  @Input() isReadOnly = false;
  public pin: string;
  public employabilityPlanId: string;
  public isLoaded = false;
  public supportiveServiceTypeNameAsId: DropDownField[];
  public model: SupportiveService[] = [];
  public originalModel: SupportiveService[] = [];
  public supportiveServiceTypes: DropDownField[];
  public validationManager: ValidationManager = new ValidationManager(this.appService);
  public modelErrors: ModelErrors = {};
  public isSectionValid = true;
  public otherSupportiveServiceId: number;
  public isSaving = false;
  public hadSaveError = false;

  public ssSub: Subscription;

  constructor(
    private fdService: FieldDataService,
    private router: Router,
    public appService: AppService,
    private supportiveServiceService: SupportiveServiceService,
    private route: ActivatedRoute,
    private employabilityPlanService: EmployabilityPlanService
  ) {}

  ngOnInit() {
    this.requestDataFromMultipleDataSources()
      .pipe(take(1))
      .subscribe(results => {
        this.pin = results[0].pin;
        this.initEmployabilityPlan(results[1]);
        this.initSupportiveServiceDropDowns(results[2]);
        this.supportiveServiceService.getSupportiveServices(this.pin, this.employabilityPlanId).subscribe(data => {
          this.initSupportiveServices(data);
          this.isLoaded = true;
          if (this.model != null) {
            for (const item of this.model) {
              const x = new SupportiveService();
              SupportiveService.clone(item, x);
              this.originalModel.push(x);
            }
          }
        });
      });
  }

  public requestDataFromMultipleDataSources() {
    return forkJoin([
      this.route.parent.params.pipe(take(1)),
      this.route.params.pipe(take(1)),
      this.fdService.getFieldDataByField(FieldDataTypes.SupportiveServiceTypes).pipe(take(1))
    ]);
  }

  private initEmployabilityPlan(data) {
    this.employabilityPlanId = data.id;
  }

  private initSupportiveServiceDropDowns(data) {
    this.supportiveServiceTypeNameAsId = Utilities.createDropDownWithIdAsName(data);
    this.otherSupportiveServiceId = Utilities.idByFieldDataName('Other', data);
    this.supportiveServiceTypes = data;
  }
  private initSupportiveServices(data) {
    this.model = data;
  }

  checkState() {
    if (!this.isSectionValid) this.validate();
    if (!_.isEqual(this.model, this.originalModel)) {
      this.appService.componentDataModified.next({ dataModified: true });
      this.appService.isEPUrlChangeBlocked = true;
    }
  }

  validate() {
    // Clear all previous errors.
    this.validationManager.resetErrors();
    this.isSectionValid = true;
    const result = new ValidationResult();
    const errArr = result.createErrorsArray('supportiveServices');

    if (this.model) {
      this.model.forEach(item => {
        const vr = item.validate(this.validationManager, this.otherSupportiveServiceId);

        errArr.push(vr.errors);

        result.isValid = result.isValid && vr.isValid;
      });
    }

    this.isSectionValid = result.isValid;
    this.modelErrors = result.errors;
  }

  public clickSave(exit: boolean) {
    this.validate();
    if (this.isSectionValid) {
      //Passing a boolean to identify if it is just save or Save +Exit
      this.save(exit);
    }
  }

  private save(exit: boolean) {
    this.isSaving = true;
    this.hadSaveError = false;
    this.originalModel = [];
    this.ssSub = this.supportiveServiceService.saveSupportiveService(this.pin, +this.employabilityPlanId, this.model).subscribe(
      data => {
        this.model = data;
        if (this.model != null) {
          for (const item of this.model) {
            const x = new SupportiveService();
            SupportiveService.clone(item, x);
            this.originalModel.push(x);
          }
        }

        if (exit) {
          this.router.navigateByUrl(`/pin/${this.pin}/employability-plan/overview/${this.employabilityPlanId}`);
        }
        this.appService.isEPUrlChangeBlocked = false;
        this.isSaving = false;
        this.appService.componentDataModified.next({ dataModified: false });
      },
      error => {
        this.hadSaveError = true;
        this.isSaving = false;
      }
    );
  }
}
