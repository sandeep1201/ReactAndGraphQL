import { HighDatePipe } from './high-date.pipe';

describe('HighDatePipe', () => {
  it('create an instance', () => {
    const pipe = new HighDatePipe();
    expect(pipe).toBeTruthy();
  });
  it(`if passed a date of '12/31/9999 should return unknown' `, () => {
    const pipe = new HighDatePipe();
    expect(pipe.transform('12/31/9999')).toEqual('Unknown');
  });
  it(`if passed a valid date of '12/12/2018 should return the same date' `, () => {
    const pipe = new HighDatePipe();
    expect(pipe.transform('12/12/2018')).toEqual('12/12/2018');
  });
});
