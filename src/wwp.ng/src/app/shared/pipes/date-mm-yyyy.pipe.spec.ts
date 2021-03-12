import { DateMmYyyyPipe } from './date-mm-yyyy.pipe';

describe('DateMmYyyyPipe', () => {
  it('create an instance', () => {
    const pipe = new DateMmYyyyPipe();
    expect(pipe).toBeTruthy();
  });
  it('should return true since a valid Date', () => {
    const pipe = new DateMmYyyyPipe();
    expect(pipe.transform('11/11/2019')).toEqual('11/2019');
  });
  it('should return false since not a  valid Date', () => {
    const pipe = new DateMmYyyyPipe();
    expect(pipe.transform('13/2019')).toBeFalsy();
  });
});
