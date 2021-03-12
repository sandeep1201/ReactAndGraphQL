import * as TypeMoq from 'typemoq';
import { ActivatedRoute, ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import * as moment from 'moment';
import { AppService } from 'src/app/core/services/app.service';
import { ParticipationTrackingService } from '../services/participation-tracking.service';
import { ParticipationCalendarGuard } from './participation-calendar.guard';
describe('ParticipationCalendarComponent', () => {
  let momentDate = moment(new Date()).toDate();
  const route = TypeMoq.Mock.ofType<ActivatedRouteSnapshot>();
  const router = TypeMoq.Mock.ofType<Router>();
  const state = TypeMoq.Mock.ofType<RouterStateSnapshot>();
  const appServiceMoq = TypeMoq.Mock.ofType<AppService>();
  const partServiceMoq = TypeMoq.Mock.ofType<ParticipantService>();
  const ptServiceMoq = TypeMoq.Mock.ofType<ParticipationTrackingService>();

  ptServiceMoq.setup(x => x.viewDate).returns(() => new BehaviorSubject({ viewDate: moment(new Date(momentDate.getFullYear(), momentDate.getMonth(), 16)).toDate() }));
  let testingClass: ParticipationCalendarGuard;
  beforeEach(() => {
    testingClass = new ParticipationCalendarGuard(router.object, appServiceMoq.object, partServiceMoq.object, ptServiceMoq.object);
  });

  it('tetsing canActivate', () => {
    testingClass.canActivate(route.object, state.object);
  });
  // it('Date should be adjusted and difference should be 1 with date greater than 16 and before pulldown date', () => {

  // });
  // it('Date should be adjusted and difference should be 1 with date greater than 16 and after pulldown date', () => {

  // });
});
