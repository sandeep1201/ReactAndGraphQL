import { PadPipe } from './pad.pipe';

describe('ParticipantPinPipe', () => {
  it('create an instance', () => {
    const pipe = new PadPipe();
    expect(pipe).toBeTruthy();
  });
  it(`given a value and padNumber and if value's lenght is less the padNumber it should add zeros to the front of the value`, () => {
    const pipe = new PadPipe();
    expect(pipe.transform(12345, 6)).toEqual('012345');
  });
});
