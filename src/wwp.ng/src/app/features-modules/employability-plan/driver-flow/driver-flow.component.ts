import { SubSink } from 'subsink';
import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit, OnDestroy, Output, EventEmitter } from '@angular/core';

import { Observable, forkJoin, of } from 'rxjs';
import * as _ from 'lodash';
import { take, concatMap } from 'rxjs/operators';
import { BaseParticipantComponent } from 'src/app/shared/components/base-participant-component';
import { EmployabilityPlan } from '../models/employability-plan.model';
import { ParticipationStatus } from 'src/app/shared/models/participation-statuses.model';
import { EnrolledProgram } from 'src/app/shared/models/enrolled-program.model';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { AppService } from 'src/app/core/services/app.service';
import { EmployabilityPlanService } from '../services/employability-plan.service';
import { FeatureToggleTypes } from 'src/app/shared/enums/feature-toggle-types.enum';

@Component({
  selector: 'app-driver-flow',
  templateUrl: './driver-flow.component.html',
  styleUrls: ['./driver-flow.component.scss']
})
export class EmployabilityPlanDriverFlowComponent extends BaseParticipantComponent implements OnInit, OnDestroy {
  public isLoaded = false;
  public goBackUrl: string;
  public pin: string;
  public isReadOnly = true;
  public isCollapsed = false;
  protected showCCAuthorizations = false;
  protected showCCAuthorizationsButton = false;
  public ep: EmployabilityPlan;
  employabilityPlanId: string;
  public lastComponentCalled: string;
  public id: any;
  // tslint:disable-next-line: no-output-on-prefix
  @Output() onExitEditMode = new EventEmitter<boolean>();
  public dataModified: boolean;
  participationStatuses: ParticipationStatus[] = [];
  localParticipationStatuses: ParticipationStatus[];
  currentlyEnrolledPrograms: EnrolledProgram[];
  canView: boolean;
  programName: string;
  public epComponentClicked: string;
  public currentUrl: string[];
  private requestSub = new SubSink();

  constructor(route: ActivatedRoute, router: Router, private appService: AppService, partService: ParticipantService, private employabilityPlanService: EmployabilityPlanService) {
    super(route, router, partService);
  }

  ngOnInit() {
    super.onInit();
    this.showCCAuthorizationsButton = this.appService.getFeatureToggleDate(FeatureToggleTypes.CCAuthorizations);
    this.appService.employabilityPlan.subscribe(data => {
      if (data && data.id === 0) {
        this.id = 0;
        return;
      } else {
        this.ep = data;
        this.id = data.id;
        if (this.ep) {
          this.programName = data.enrolledProgramName;
          this.isLoaded = true;
        }
        if (this.localParticipationStatuses && this.programName) {
          // tslint:disable: no-shadowed-variable
          this.localParticipationStatuses = this.localParticipationStatuses.filter(data => data.enrolledProgramName === this.programName);
        }
        this.goBackUrl = '/pin/' + this.pin + '/employability-plan/overview/' + this.id;
      }
    });
    if (!this.ep) {
      this.requestDataFromMultipleSources()
        .pipe(
          concatMap(results => {
            this.employabilityPlanId = results[0].id;
            this.pin = results[1].pin;
            this.id = this.employabilityPlanId;
            if (+this.id === 0) {
              this.goBackUrl = '/pin/' + this.pin + '/employability-plan/list';
            } else {
              this.goBackUrl = '/pin/' + this.pin + '/employability-plan/overview/' + this.id;
            }
            if (this.id > 0) {
              return this.employabilityPlanService.getEpById(this.pin, this.id);
            } else {
              this.isLoaded = true;
              return of(null);
            }
          })
        )
        .subscribe(epdata => {
          this.ep = epdata;
          if (epdata) this.programName = epdata.enrolledProgramName;
          this.isLoaded = true;
        });
    }
    this.partService.getAllStatusesForPin(this.pin).subscribe(data => {
      this.getAllStatusesForPin(data);
    });
  }

  public requestDataFromMultipleSources(): Observable<any[]> {
    const response1 = this.route.firstChild.params.pipe(take(1));
    const response2 = this.route.params.pipe(take(1));
    // Observable.forkJoin (RxJS 5) changes to just forkJoin() in RxJS 6
    return forkJoin([response1, response2]);
  }

  onPinInit() {}

  public getAllStatusesForPin(res) {
    if (!this.programName) this.participationStatuses = res;
    else
      res.filter(data => {
        if (data && data.enrolledProgramName === this.programName) this.participationStatuses.push(data);
      });
    this.localParticipationStatuses = _.orderBy(this.participationStatuses.slice(), ['isCurrent', 'BeginDate'], ['desc', 'asc']);
  }

  sidebarToggle() {
    this.isCollapsed = !this.isCollapsed;
    return this.isCollapsed;
  }
  public onParticipantInit() {
    // Per US2431, workers can edit EP records if:
    //   1. EP App shall updateable by workers that have the security role for an enrolled
    //      program.
    //   2. EP App shall be read only by workers that have the security role for a
    //      program that the latest instance of "enrolled" occurred.

    // First get the enrolled programs the user has access to.
    const enrolledProgs = this.participant.getCurrentEnrolledProgramsUserHasAccessTo(this.appService.user, this.appService);

    // Only has access to CF, LF and W2.
    let isReadOnly = true;
    if (enrolledProgs != null) {
      for (const p of enrolledProgs) {
        if (p.isCF || p.isLF || p.isW2 || p.isWW) {
          isReadOnly = false;
          break;
        }
      }
    }

    // If we have any programs after the filtering, then the worker can edit.
    this.isReadOnly = isReadOnly;
  }

  unsavedChanges() {
    //setting dataModified to false on this method call...
    this.dataModified = false;
    this.requestSub.add(
      this.appService.componentDataModified.pipe(take(1)).subscribe(res => {
        if (res.dataModified) {
          this.dataModified = res.dataModified;
          this.appService.setDialogueFromDriverFlow = true;
        }
      })
    );
    if (this.appService.componentDataModifiedFromElasped) {
      this.dataModified = true;
    }
  }

  checkForCurrentUrl() {
    this.currentUrl = this.router.url.split('/');
  }

  setEpScreen(epComponent?: string) {
    this.epComponentClicked = epComponent ? epComponent : '';
    this.checkForCurrentUrl();
    if (epComponent === '' && this.currentUrl[3] === 'employability-plan') {
      this.epComponentClicked = 'employability-plan';
    }
    if (this.epComponentClicked !== 'employability-plan' && this.currentUrl[4] !== this.epComponentClicked) {
      this.unsavedChanges();
      if (this.appService.isEPUrlChangeBlocked !== false && this.dataModified) {
        this.appService.isDialogPresent = true;
      } else {
        if (epComponent === '') {
          const url = `/pin/${this.pin}/employability-plan/${this.id}`;
          this.appService.componentDataModified.next({ dataModified: false });
          this.router.navigateByUrl(url);
        } else {
          const url = `/pin/${this.pin}/employability-plan/${epComponent}/${this.id}`;
          this.appService.componentDataModified.next({ dataModified: false });
          this.router.navigateByUrl(url);
        }
      }
    }
    const idString = this.id.toString();
    if (this.epComponentClicked === 'employability-plan' && this.currentUrl[4] !== idString) {
      this.unsavedChanges();
      if (this.appService.isEPUrlChangeBlocked !== false && this.dataModified) {
        this.appService.isDialogPresent = true;
      } else {
        if (this.currentUrl[3] === 'employability-plan') epComponent = 'employability-plan';
        if (epComponent === '' || epComponent === 'employability-plan') {
          const url = `/pin/${this.pin}/employability-plan/${this.id}`;
          this.appService.componentDataModified.next({ dataModified: false });
          this.router.navigateByUrl(url);
        } else {
          const url = `/pin/${this.pin}/employability-plan/${epComponent}/${this.id}`;
          this.appService.componentDataModified.next({ dataModified: false });
          this.router.navigateByUrl(url);
        }
      }
    }
  }
  public exitActivityEditIgnoreChanges($event) {
    this.onExitEditMode.emit();
    this.checkForCurrentUrl();
    if (this.currentUrl[4] !== this.epComponentClicked) {
      this.appService.isEPUrlChangeBlocked = false;
    }
    this.setEpScreen(this.epComponentClicked);
  }

  ngOnDestroy() {
    this.requestSub.unsubscribe();
    super.onDestroy();
  }
}
