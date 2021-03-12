import { EmptyStringIfNullPipe } from './empty-string-if-null.pipe';

describe('Pipe: EmptyStringIfNull', () => {
  it('create an instance', () => {
    let pipe = new EmptyStringIfNullPipe();
    expect(pipe).toBeTruthy();
  });
  it('if null is passed should return an empty string', () => {
    let pipe = new EmptyStringIfNullPipe();
    expect(pipe.transform(null)).toEqual('');
  });
  it('if string is passed should return same string', () => {
    let pipe = new EmptyStringIfNullPipe();
    expect(pipe.transform('test')).toEqual('test');
  });
});
