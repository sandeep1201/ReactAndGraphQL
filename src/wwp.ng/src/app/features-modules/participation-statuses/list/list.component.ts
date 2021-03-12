import { Component, OnInit, Input } from '@angular/core';
import { ParticipantService } from '../../../shared/services/participant.service';
import { ParticipationStatus } from '../../../shared/models/participation-statuses.model';
import { Participant } from '../../../shared/models/participant';
import { AppService } from '../../../core/services/app.service';
import { Authorization } from '../../../shared/models/authorization';
import * as _ from 'lodash';
import { EnrolledProgram } from '../../../shared/models/enrolled-program.model';
import { EnrolledProgramStatus } from '../../../shared/enums/enrolled-program-status.enum';

@Component({
  selector: 'app-participation-statuses-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss']
})
export class ParticipationStatusListComponent implements OnInit {
  public currentlyEnrolledPrograms: EnrolledProgram[];
  @Input() pin: string;
  @Input() isReadOnly = true;

  @Input() participant: Participant;

  public isInEditMode = false;
  public participationStatuses: ParticipationStatus[];
  public localParticipationStatuses;
  public isLoaded = false;
  public canAdd = false;

  constructor(private participantService: ParticipantService, private appService: AppService) {}

  ngOnInit() {
    this.participantService.modeForParticipationStatuses.subscribe(res => {
      this.isInEditMode = res.inEditMode;
      if (!this.isInEditMode) {
        this.getAllStatusesForPin();
      }
    });
  }
  getAllStatusesForPin() {
    this.participantService.getAllStatusesForPin(this.pin).subscribe(res => {
      this.participationStatuses = res;
      this.localParticipationStatuses = _.orderBy(this.participationStatuses.slice(), ['isCurrent', 'BeginDate'], ['desc', 'asc']);
      this.getAccessToPs();
      this.isLoaded = true;
    });
  }

  onAdd() {
    this.participantService.modeForParticipationStatuses.next({ id: 0, readonly: false, inEditMode: true });
  }
  edit(a) {
    this.participantService.modeForParticipationStatuses.next({ id: a.id, readonly: false, inEditMode: true });
  }
  public getAccessToPs() {
    if (this.participant) {
      const hasAuth = this.appService.isUserAuthorized(Authorization.canAccessInformalAssessment_Edit, this.participant);
      const programsUserHasAccessTo = this.participant.getMostRecentProgramsUserHasAccessTo(this.appService.user, this.appService);
      this.currentlyEnrolledPrograms = EnrolledProgram.filterByStatuses(programsUserHasAccessTo, [EnrolledProgramStatus.enrolled]);

      if (this.currentlyEnrolledPrograms.length > 0 && hasAuth && this.localParticipationStatuses) {
        this.localParticipationStatuses.forEach(ps => {
          if (ps.enrolledProgramCode && this.appService.isUserAuthorized(Authorization[`canAccessProgram_${ps.enrolledProgramCode.trim()}`])) {
            ps.canEdit = true;
          }
        });
        this.canAdd = true;
      }
    }

    this.isLoaded = true;
  }
}
