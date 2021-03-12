import { DateMmDdYyyyPipe } from './date-mm-dd-yyyy.pipe';
import * as moment from 'moment';

describe('DateMmDdYyyyPipe', () => {
  it('create an instance', () => {
    let pipe = new DateMmDdYyyyPipe();
    expect(pipe).toBeTruthy();
  });
  it('should be a valid Date', () => {
    let pipe = new DateMmDdYyyyPipe();
    expect(pipe.transform('12/12/2019')).toBeTruthy();
  });
  it('should be a valid Date', () => {
    let pipe = new DateMmDdYyyyPipe();
    expect(pipe.transform('13/12/2019')).toBeFalsy();
  });
});
