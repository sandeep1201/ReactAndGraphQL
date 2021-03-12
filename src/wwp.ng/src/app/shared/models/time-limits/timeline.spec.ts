import { AppService } from 'src/app/core/services/app.service';
import { Timeline } from './timeline';
import * as TypeMoq from 'typemoq';
import { Router } from '@angular/router';
import { LogService } from '../../services/log.service';
import { JwtAuthConfig } from 'src/app/core/jwt-auth-config';
import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
import { empty } from 'rxjs';
import { Location } from '@angular/common';
import { ClockTypes } from './clocktypes';
import * as moment from 'moment';

fdescribe('TimeLimits: Timeline', () => {
  const authHttpClientMock: TypeMoq.IMock<AuthHttpClient> = TypeMoq.Mock.ofType<AuthHttpClient>();
  const router = TypeMoq.Mock.ofType<Router>();
  const logServiceMoq = TypeMoq.Mock.ofType<LogService>();
  const jwtMoq = TypeMoq.Mock.ofType<JwtAuthConfig>();
  const location = TypeMoq.Mock.ofType<Location>();
  router.setup(x => x.events).returns(() => empty());
  let model: Timeline;
  let appService: AppService;

  beforeEach(() => {
    appService = new AppService(authHttpClientMock.object, logServiceMoq.object, router.object, jwtMoq.object, location.object);
    model = new Timeline(appService);
  });

  it('GetIsInOrAfterTransition_CurrentDate_ForInBetweenCheckReturnsExpectedResult', () => {
    expect(model.getIsInOrAfterTransition(moment('01/20/2021'), '11/01/2021')).toBeFalsy();
    expect(model.getIsInOrAfterTransition(moment('11/01/2021'), '11/01/2021')).toBeFalsy();
    expect(model.getIsInOrAfterTransition(moment('11/02/2021'), '11/01/2021')).toBeFalsy();
    expect(model.getIsInOrAfterTransition(moment('06/02/2021'), '11/01/2021')).toBeTruthy();
  });

  it('GetIsInOrAfterTransition_CurrentDate_ForIsAfterCheckReturnsExpectedResult', () => {
    expect(model.getIsInOrAfterTransition(moment('01/20/2021'), '11/01/2021', true)).toBeFalsy();
    expect(model.getIsInOrAfterTransition(moment('06/02/2021'), '11/01/2021', true)).toBeFalsy();
    expect(model.getIsInOrAfterTransition(moment('11/01/2021'), '11/01/2021', true)).toBeTruthy();
    expect(model.getIsInOrAfterTransition(moment('11/02/2021'), '11/01/2021', true)).toBeTruthy();
  });

  it('GetTransitionPeriodStartDate_CurrentDateBeforeTransitionDate_ReturnsStartDateNull', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('01/20/2021'), '11/01/2021', 0, 0, ClockTypes.State);
    expect(startDate).toBeNull();
  });

  it('GetTransitionPeriodStartDate_CurrentDateSameTransitionDate_ReturnsStartDateIsNull', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('11/01/2021'), '11/01/2021', 0, 0, ClockTypes.State);
    expect(startDate).toBeNull();
  });

  it('GetTransitionPeriodStartDate_CurrentDateAfterTransitionDate_ReturnsStartDateIsNull', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('11/02/2021'), '11/01/2021', 0, 0, ClockTypes.State);
    expect(startDate).toBeNull();
  });

  it('GetTransitionPeriodStartDate_WithValidValues_ReturnsStartDateIsNotNull', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('10/02/2021'), '11/01/2021', 0, 0, ClockTypes.State);
    expect(startDate).toBeDefined();
  });

  it('GetTransitionPeriodStartDate_ClockTypeIsNotState_ReturnsStartDateIsNull', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('10/02/2021'), '11/01/2021', 0, 0, ClockTypes.Federal);
    expect(startDate).toBeNull();
  });

  it('GetTransitionPeriodStartDate_WithCurrentDate05/15/2021_ReturnsStartDate11/01/2021', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('05/15/2021'), '11/01/2021', 46, 0, ClockTypes.State);
    expect(moment(startDate).format('L')).toBe('11/01/2021');
  });

  it('GetTransitionPeriodStartDate_WithCurrentDate08/15/2021_ReturnsStartDate11/01/2021', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('08/15/2021'), '11/01/2021', 46, 0, ClockTypes.State);
    expect(moment(startDate).format('L')).toBe('11/01/2021');
  });

  it('GetTransitionPeriodStartDate_WithCurrentDate09/15/2021_ReturnsStartDate11/01/2021', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('09/15/2021'), '11/01/2021', 46, 0, ClockTypes.State);
    expect(moment(startDate).format('L')).toBe('11/01/2021');
  });

  it('GetTransitionPeriodStartDate_WithCurrentDate10/15/2021_ReturnsStartDate12/01/2021', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('10/15/2021'), '11/01/2021', 46, 0, ClockTypes.State);
    expect(moment(startDate).format('L')).toBe('12/01/2021');
  });

  it('GetTransitionPeriodStartDate_WithCurrentDate08/15/2021_46_ReturnsStartDate12/01/2021', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('08/15/2021'), '11/01/2021', 46, 0, ClockTypes.State);
    expect(moment(startDate).format('L')).toBe('11/01/2021');
  });

  it('GetTransitionPeriodStartDate_WithCurrentDate05/15/2021_48_ReturnsStartDate12/01/2021', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('05/15/2021'), '11/01/2021', 48, 0, ClockTypes.State);
    expect(moment(startDate).format('L')).toBe('11/01/2021');
  });

  it('GetTransitionPeriodStartDate_WithCurrentDate10/15/2021_48_ReturnsStartDate12/01/2021', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('10/15/2021'), '11/01/2021', 48, 0, ClockTypes.State);
    expect(moment(startDate).format('L')).toBe('11/01/2021');
  });

  it('GetTransitionPeriodStartDate_WithCurrentDate08/15/2021_ReturnsStartDate02/01/2022', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('08/15/2021'), '11/01/2021', 42, 0, ClockTypes.State);
    expect(moment(startDate).format('L')).toBe('02/01/2022');
  });

  it('GetTransitionPeriodStartDate_WithCurrentDate05/15/2021_ReturnsStartDate11/01/2021', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('05/15/2021'), '11/01/2021', 42, 0, ClockTypes.State);
    expect(moment(startDate).format('L')).toBe('11/01/2021');
  });

  it('GetTransitionPeriodStartDate_WithCurrentDate06/15/2021_ReturnsStartDate11/01/2022', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('06/15/2021'), '11/01/2021', 43, 0, ClockTypes.State);
    expect(moment(startDate).format('L')).toBe('11/01/2021');
  });

  it('GetTransitionPeriodStartDate_WithCurrentDate05/15/2021_50_ReturnsStartDate11/01/2021', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('05/15/2021'), '11/01/2021', 50, 0, ClockTypes.State);
    expect(moment(startDate).format('L')).toBe('11/01/2021');
  });

  it('GetTransitionPeriodStartDate_WithCurrentDate06/15/2021_50_ReturnsStartDate11/01/2021', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('06/15/2021'), '11/01/2021', 50, 0, ClockTypes.State);
    expect(moment(startDate).format('L')).toBe('11/01/2021');
  });

  it('GetTransitionPeriodStartDate_WithCurrentDate08/15/2021_50_ReturnsStartDate11/01/2021', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('08/15/2021'), '11/01/2021', 50, 0, ClockTypes.State);
    expect(moment(startDate).format('L')).toBe('11/01/2021');
  });

  it('GetTransitionPeriodStartDate_WithCurrentDate06/15/2021_58_ReturnsStartDate11/01/2022', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('06/15/2021'), '11/01/2021', 53, 0, ClockTypes.State);
    expect(moment(startDate).format('L')).toBe('11/01/2021');
  });

  it('GetTransitionPeriodStartDate_WithCurrentDate05/15/2021_ReturnsStartDate07/01/2021', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('05/15/2021'), '11/01/2021', 58, 2, ClockTypes.State);
    expect(moment(startDate).format('L')).toBe('07/01/2021');
  });

  it('GetTransitionPeriodStartDate_WithCurrentDate05/15/2021_59_ReturnsStartDate06/01/2021', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('05/15/2021'), '11/01/2021', 59, 1, ClockTypes.State);
    expect(moment(startDate).format('L')).toBe('06/01/2021');
  });

  it('GetTransitionPeriodStartDate_WithCurrentDate08/15/2021_59_ReturnsStartDate06/01/2021', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('08/15/2021'), '11/01/2021', 59, 1, ClockTypes.State);
    expect(moment(startDate).format('L')).toBe('09/01/2021');
  });

  it('GetTransitionPeriodStartDate_WithCurrentDate08/15/2021_ReturnsStartDate11/01/2021', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('08/15/2021'), '11/01/2021', 54, 6, ClockTypes.State);
    expect(moment(startDate).format('L')).toBe('11/01/2021');
  });

  it('GetTransitionPeriodStartDate_WithCurrentDate05/15/2021_54_ReturnsStartDate09/01/2021', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('05/15/2021'), '11/01/2021', 54, 6, ClockTypes.State);
    expect(moment(startDate).format('L')).toBe('11/01/2021');
  });

  it('GetTransitionPeriodStartDate_WithCurrentDate10/15/2021_54_ReturnsStartDate11/01/2021', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('10/15/2021'), '11/01/2021', 54, 6, ClockTypes.State);
    expect(moment(startDate).format('L')).toBe('11/01/2021');
  });

  it('GetTransitionPeriodStartDate_WithCurrentDate10/15/2021_56_ReturnsStartDate11/01/2021', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('10/15/2021'), '11/01/2021', 56, 4, ClockTypes.State);
    expect(moment(startDate).format('L')).toBe('11/01/2021');
  });

  it('GetTransitionPeriodStartDate_WithCurrentDate10/15/2021_57_ReturnsStartDate11/01/2021', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('10/15/2021'), '11/01/2021', 57, 3, ClockTypes.State);
    expect(moment(startDate).format('L')).toBe('11/01/2021');
  });

  it('GetTransitionPeriodStartDate_WithCurrentDate10/15/2021_42_ReturnsStartDate11/01/2021', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('10/15/2021'), '11/01/2021', 42, 0, ClockTypes.State);
    expect(moment(startDate).format('L')).toBe('04/01/2022');
  });

  it('GetTransitionPeriodStartDate_WithCurrentDate10/15/2021_54_ReturnsStartDate11/01/2021', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('10/15/2021'), '11/01/2021', 54, 6, ClockTypes.State);
    expect(moment(startDate).format('L')).toBe('11/01/2021');
  });

  it('GetTransitionPeriodStartDate_WithCurrentMonthTicked_ReturnsStartDate08/01/2021', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('05/15/2021'), '11/01/2021', 58, 2, ClockTypes.State, true);
    expect(moment(startDate).format('L')).toBe('08/01/2021');
  });

  it('GetTransitionPeriodStartDate_WithCurrentMonthTicked_ReturnsStartDate11/01/2021', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('08/15/2021'), '11/01/2021', 54, 6, ClockTypes.State, true);
    expect(moment(startDate).format('L')).toBe('11/01/2021');
  });

  it('GetTransitionPeriodStartDate_WithIsBackdated_ReturnsStartDate06/01/2021', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('05/15/2021'), '11/01/2021', 58, 2, ClockTypes.State, false, true);
    expect(moment(startDate).format('L')).toBe('06/01/2021');
  });

  it('GetTransitionPeriodStartDate_WithIsBackdated_ReturnsStartDate11/01/2021', () => {
    const startDate = model.getTransitionPeriodStartDate(moment('08/15/2021'), '11/01/2021', 54, 6, ClockTypes.State, false, true);
    expect(moment(startDate).format('L')).toBe('11/01/2021');
  });
});
