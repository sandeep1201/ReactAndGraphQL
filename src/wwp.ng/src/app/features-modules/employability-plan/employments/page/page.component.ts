import { Component, OnInit, OnDestroy, DoCheck } from '@angular/core';
import { SubSink } from 'subsink';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, forkJoin } from 'rxjs';
import { concatMap, take } from 'rxjs/operators';
import { Employment } from '../../../../shared/models/work-history-app';
import { EmployabilityPlan } from '../../models/employability-plan.model';
import { EpEmployment } from '../../models/ep-employment.model';
import { EmployabilityPlanService } from '../../services/employability-plan.service';
import { AppService } from 'src/app/core/services/app.service';
@Component({
  selector: 'app-epemployments-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.scss']
})
export class EpEmploymentsPageComponent implements OnInit, OnDestroy, DoCheck {
  public isLoaded = false;
  public noEmployments = false;
  public allEmployments: Employment[];
  public ep: EmployabilityPlan;
  public subs = new SubSink();
  public pin: string;
  public epId: string;
  public filteredEmployments: EpEmployment[];
  public selectedEmployments = new Set();
  public isSaving = false;
  public hadSaveError = false;

  constructor(private route: ActivatedRoute, private router: Router, private employabilityPlanService: EmployabilityPlanService, private appService: AppService) {}

  ngOnInit() {
    this.subs.add(
      this.requestDataFromMultipleSources().subscribe(results => {
        this.pin = results[0].pin;
        this.epId = results[1].id;
        this.initEmploymentsAndEp();
      })
    );
  }

  public requestDataFromMultipleSources(): Observable<any[]> {
    const response1 = this.route.parent.params.pipe(take(1));
    const response2 = this.route.params.pipe(take(1));
    // Observable.forkJoin (RxJS 5) changes to just forkJoin() in RxJS 6
    return forkJoin([response1, response2]);
  }
  ngDoCheck() {}
  initEmploymentsAndEp() {
    this.employabilityPlanService
      .getEpById(this.pin, this.epId)
      .pipe(concatMap(res => this.employabilityPlanService.getEmploymentForEP(this.pin, +this.epId, res.beginDate.split('/').join('-'), res.enrolledProgramCd)))
      .subscribe(res => {
        this.filteredEmployments = res;
        if (this.filteredEmployments.length === 0) this.noEmployments = true;
        if (this.filteredEmployments) {
          this.filteredEmployments.forEach(i => {
            if (i.isSelected) {
              this.selectedEmployments.add(i);
            }
          });
        }
        this.isLoaded = true;
      });
  }
  public saveAndContinue(isCont: boolean) {
    this.isSaving = true;
    this.hadSaveError = false;
    if (this.filteredEmployments.length > 0) {
      this.employabilityPlanService.saveEmploymentForEP(this.filteredEmployments, this.pin, +this.epId).subscribe(
        res => {
          if (res) {
            this.filteredEmployments = res;
            if (isCont) this.router.navigateByUrl(`/pin/${this.pin}/employability-plan/elapsed-activities/${this.epId}`);
            this.appService.componentDataModified.next({ dataModified: false });
            this.isSaving = false;
          } else {
            return;
          }
        },
        error => {
          this.hadSaveError = true;
          this.isSaving = false;
        }
      );
    } else {
      this.router.navigateByUrl(`/pin/${this.pin}/employability-plan/elapsed-activities/${this.epId}`);
    }
  }
  public selectedEmployment(emp) {
    this.filteredEmployments = emp;
  }
  ngOnDestroy() {
    this.subs.unsubscribe();
  }
}
