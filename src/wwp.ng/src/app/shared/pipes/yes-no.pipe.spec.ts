/* tslint:disable:no-unused-variable */

import { YesNoPipe } from './yes-no.pipe';

describe('Pipe: YesNo', () => {
  let pipe = new YesNoPipe();
  it('create an instance', () => {
    expect(pipe).toBeTruthy();
  });
  it('if passed a string true should return string Yes', () => {
    expect(pipe.transform('true')).toEqual('Yes');
  });
  it('if passed a boolean true should return string Yes', () => {
    expect(pipe.transform(true)).toEqual('Yes');
  });
  it('if passed a string false should return string No', () => {
    expect(pipe.transform('false')).toEqual('No');
  });
  it('if passed a boolean false should return string No', () => {
    expect(pipe.transform(false)).toEqual('No');
  });
  it('if passed some random text should return emplty string', () => {
    expect(pipe.transform('asdasd')).toEqual('');
  });
});
