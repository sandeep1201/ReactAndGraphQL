import { ParticipationTracking } from './../../shared/models/participation-tracking.model';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { concatMap } from 'rxjs/operators';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import * as moment from 'moment';
import { ParticipationTrackingService } from '../participation-calendar/services/participation-tracking.service';
import { Utilities } from 'src/app/shared/utilities';
import { AppService } from 'src/app/core/services/app.service';
import { Participant } from 'src/app/shared/models/participant';

@Component({
  selector: 'app-non-participation-details',
  templateUrl: './non-participation-details.component.html',
  styleUrls: ['./non-participation-details.component.scss']
})
export class NonParticipationDetailsComponent implements OnInit {
  @Output() closeDetails = new EventEmitter<boolean>();
  public participant: Participant;
  public response: ParticipationTracking[];
  public goBackUrl = '';
  public pageTitle = 'Non-Participation/Good Cause Details';
  public pin;
  public viewDate: Date;
  public isLoaded = false;
  public momentDate = moment(new Date()).toDate();
  public headerMonth = moment().format('MMMM');
  public headerNextMonth: string;
  public PTFeatureDate: Date;
  disablePreviousClick = false;
  disableForwardClick = false;

  constructor(private route: ActivatedRoute, private partService: ParticipantService, private ptService: ParticipationTrackingService, private appService: AppService) {}

  ngOnInit() {
    this.route.params

      .pipe(
        concatMap(result => {
          this.pin = result.pin;
          this.goBackUrl = '/pin/' + this.pin;
          this.PTFeatureDate = moment(new Date(this.appService.getFeatureToggleValue('ParticipationTracking'))).toDate();
          return this.partService.getCachedParticipant(this.pin);
        })
      )
      .subscribe(res => {
        this.participant = res;
        if (moment().date() < 16) {
          this.momentDate = moment(this.momentDate)
            .subtract(1, 'M')
            .toDate();
        }
        this.viewDate = moment(new Date(this.momentDate.getFullYear(), this.momentDate.getMonth(), 16)).toDate();
        this.checkForDisablingNavigation();
        this.getData();
      });
  }

  getData() {
    this.isLoaded = false;
    let startDate;
    let endDate;

    startDate = moment(this.viewDate).format('MMDDYYYY');
    endDate = moment(this.viewDate)
      .add(1, 'M')
      .subtract(1, 'day')
      .format('MMDDYYYY');
    this.headerMonth = moment(this.viewDate).format('MMMM');
    this.headerNextMonth = moment(this.viewDate)
      .add(1, 'M')
      .format('MMMM');

    this.ptService.getParticipationTrackingDetails(this.pin, this.participant.id, startDate, endDate, true).subscribe(res => {
      // console.log('details', res);
      this.response = res;
      this.isLoaded = true;
    });
  }

  checkForDisablingNavigation() {
    this.disablePreviousClick = moment(this.viewDate).isSameOrBefore(moment(this.PTFeatureDate), 'month');
    this.disableForwardClick = moment(this.viewDate).isSameOrAfter(this.momentDate, 'month');
  }

  nextClicked(e) {
    this.viewDate = moment(this.viewDate)
      .add(1, 'M')
      .toDate();
    this.headerMonth = moment(this.viewDate).format('MMMM');
    this.checkForDisablingNavigation();
    this.getData();
  }
  previousClicked(e) {
    this.viewDate = moment(this.viewDate)
      .subtract(1, 'M')
      .toDate();
    this.headerMonth = moment(this.viewDate).format('MMMM');
    this.checkForDisablingNavigation();
    this.getData();
  }

  close() {
    this.closeDetails.emit(false);
  }
  totalNonParticpationHours() {
    return Utilities.sumArrayByProperty(this.response, 'nonParticipatedHours');
  }
  totalGoodCauseHours() {
    return Utilities.sumArrayByProperty(this.response, 'goodCausedHours');
  }
  totalSanctionableHours() {
    return Utilities.sumArrayByProperty(this.response, 'sanctionableHours');
  }
}
