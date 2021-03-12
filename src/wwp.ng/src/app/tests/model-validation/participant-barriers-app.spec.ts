import { ParticipantBarrier } from '../../shared/models/participant-barriers-app';
import * as moment from 'moment';

describe('ParticipantBarrier: isOpen', () => {
  let model: ParticipantBarrier;

  beforeEach(() => {
    // Start with a fresh model for each test.
    model = new ParticipantBarrier();
  });

  it('End Month is same as current date - isOpen is true', () => {
    model.endDate = moment().format('MM/YYYY');
    // Then: The section is not valid.
    expect(model.isOpen).toBe(true);
  });

  it('End Month is 1 month before current date - isOpen is false', () => {
    model.endDate = moment()
      .subtract(1, 'month')
      .format('MM/YYYY');
    // Then: The section is not valid.
    expect(model.isOpen).toBe(false);
  });

  it('End Month is 1 month after current date - isOpen is true', () => {
    model.endDate = moment()
      .add(1, 'month')
      .format('MM/YYYY');
    // Then: The section is not valid.
    expect(model.isOpen).toBe(true);
  });
});
