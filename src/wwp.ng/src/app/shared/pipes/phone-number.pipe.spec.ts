/* tslint:disable:no-unused-variable */

import { PhoneNumberPipe } from './phone-number.pipe';

describe('Pipe: PhoneNumber', () => {
  it('create an instance', () => {
    let pipe = new PhoneNumberPipe();
    expect(pipe).toBeTruthy();
  });
  it('formating a phone number from 1234567890 to (123) 456-7890', () => {
    let pipe = new PhoneNumberPipe();
    expect(pipe.transform(1234567890)).toEqual('(123) 456-7890');
  });
});
