import { SsnPipe } from './ssn.pipe';

describe('SsnPipe', () => {
  it('create an instance', () => {
    const pipe = new SsnPipe();
    expect(pipe).toBeTruthy();
  });
  it('if passed a number should return a formated SSN 123456789 to 123-45-6789', () => {
    const pipe = new SsnPipe();
    expect(pipe.transform(123456789)).toEqual('123-45-6789');
  });
});
