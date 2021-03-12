import { FieldDataService } from 'src/app/shared/services/field-data.service';
import { AppService } from './../../core/services/app.service';
import { ParticipationCalendarComponent } from './participation-calendar.component';
import * as TypeMoq from 'typemoq';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { ParticipationTrackingService } from './services/participation-tracking.service';
import * as moment from 'moment';
describe('ParticipationCalendarComponent', () => {
  let momentDate = moment(new Date()).toDate();
  const route = TypeMoq.Mock.ofType<ActivatedRoute>();
  const appServiceMoq = TypeMoq.Mock.ofType<AppService>();
  const partServiceMoq = TypeMoq.Mock.ofType<ParticipantService>();
  const ptServiceMoq = TypeMoq.Mock.ofType<ParticipationTrackingService>();
  const fdServiceMoq = TypeMoq.Mock.ofType<FieldDataService>();
  const pullDownDates = [
    { id: 1, benefitMonth: 1, benefitYear: 2020, pullDownDate: '2020-01-27T00:00:00-06:00' },
    { id: 2, benefitMonth: 2, benefitYear: 2020, pullDownDate: '2020-02-25T00:00:00-06:00' },
    { id: 3, benefitMonth: 3, benefitYear: 2020, pullDownDate: '2020-03-25T00:00:00-05:00' },
    { id: 4, benefitMonth: 4, benefitYear: 2020, pullDownDate: '2020-04-24T00:00:00-05:00' },
    { id: 5, benefitMonth: 5, benefitYear: 2020, pullDownDate: '2020-05-26T00:00:00-05:00' },
    { id: 6, benefitMonth: 6, benefitYear: 2020, pullDownDate: '2020-06-24T00:00:00-05:00' },
    { id: 7, benefitMonth: 7, benefitYear: 2020, pullDownDate: '2020-07-27T00:00:00-05:00' },
    { id: 8, benefitMonth: 8, benefitYear: 2020, pullDownDate: '2020-08-25T00:00:00-05:00' },
    { id: 9, benefitMonth: 9, benefitYear: 2020, pullDownDate: '2020-09-24T00:00:00-05:00' },
    { id: 10, benefitMonth: 10, benefitYear: 2020, pullDownDate: '2020-10-27T00:00:00-05:00' },
    { id: 11, benefitMonth: 11, benefitYear: 2020, pullDownDate: '2020-11-23T00:00:00-06:00' },
    { id: 12, benefitMonth: 12, benefitYear: 2020, pullDownDate: '2020-12-23T00:00:00-06:00' },
    { id: 13, benefitMonth: 1, benefitYear: 2021, pullDownDate: '2021-01-26T00:00:00-06:00' },
    { id: 14, benefitMonth: 2, benefitYear: 2022, pullDownDate: '2021-02-23T00:00:00-06:00' }
  ];

  ptServiceMoq.setup(x => x.viewDate).returns(() => new BehaviorSubject({ viewDate: moment(new Date(momentDate.getFullYear(), momentDate.getMonth(), 16)).toDate() }));
  let component: ParticipationCalendarComponent;
  beforeEach(() => {
    component = new ParticipationCalendarComponent(route.object, appServiceMoq.object, partServiceMoq.object, ptServiceMoq.object, fdServiceMoq.object);
  });

  it('Date should be adjusted and difference should be 1 with date less than 16', () => {
    component.iseventsLoaded = false;
    component.currentDate = moment('05/09/2020', 'MM/DD/YYYY');
    component.momentDate = component.currentDate.toDate();
    component.latestPullDownDate(pullDownDates);
    component.adjustDate(component.latestPullDownDate(pullDownDates));
    expect(component.currentDate.diff(component.viewDate, 'months')).toBe(1);
  });
  it('Date should be adjusted and difference should be 1 with date greater than 16 and before pulldown date', () => {
    component.iseventsLoaded = false;
    component.currentDate = moment('05/17/2020', 'MM/DD/YYYY');
    component.momentDate = component.currentDate.toDate();
    component.latestPullDownDate(pullDownDates);
    component.adjustDate(component.latestPullDownDate(pullDownDates));
    expect(component.currentDate.diff(component.viewDate, 'months')).toBe(1);
  });
  it('Date should be adjusted and difference should be 1 with date greater than 16 and after pulldown date', () => {
    component.iseventsLoaded = false;
    component.currentDate = moment('05/27/2020', 'MM/DD/YYYY');
    component.momentDate = component.currentDate.toDate();
    component.latestPullDownDate(pullDownDates);
    component.adjustDate(component.latestPullDownDate(pullDownDates));
    expect(component.currentDate.isSame(component.viewDate)).toBeTruthy();
  });
});
