
import * as moment from 'moment';
import {  DateRange, range, rangeInterval, within } from './moment-range';

describe('Moment', function() {
  let dr = range(new Date(Date.UTC(2011, 2, 5)), new Date(Date.UTC(2011, 5, 5)));
  const m1 = moment('2011-04-15', 'YYYY-MM-DD');
  const m2 = moment('2012-12-25', 'YYYY-MM-DD');
  const mStart = moment('2011-03-05', 'YYYY-MM-DD');
  const mEnd = moment('2011-06-05', 'YYYY-MM-DD');
  const or = range(null, '2011-05-05');
  const or2 = range('2011-03-05', null);

  describe('#range()', function() {
    it('should return a DateRange with start & end properties', function() {
      dr = range(m1, m2);
      expect(moment.isMoment(dr.start)).toBeTruthy();
      expect(moment.isMoment(dr.end)).toBeTruthy();
    });


    it('should support string units like `year`, `month`, `week`, `day`, `minute`, `second`, etc...', function() {
      dr = rangeInterval(m1 , 'year');
      expect(dr.start.valueOf()).toEqual(moment(m1).startOf('year').valueOf());
      expect(dr.end.valueOf()).toEqual(moment(m1).endOf('year').valueOf());
    });
  });

  describe('#within()', function() {
    it('should determine if the current moment is within a given range', function() {
      expect(within(dr,m1)).toBeTruthy();
      expect(within(dr,m2)).toBeFalsy();
      expect(within(or,m1)).toBeTruthy();
      expect(within(or2,m1)).toBeTruthy();
      expect(within(or,m2)).toBeFalsy();
      expect(within(or2,m2)).toBeTruthy();
    });

    it('should consider the edges to be within the range', function() {
      expect(within(dr,mStart)).toBeTruthy();
      expect(within(dr,mEnd)).toBeTruthy();
    });
  });
});

describe('DateRange', function() {
  const d1 = new Date(Date.UTC(2011, 2, 5));
  const d2 = new Date(Date.UTC(2011, 5, 5));
  const d3 = new Date(Date.UTC(2011, 4, 9));
  const d4 = new Date(Date.UTC(1988, 0, 1));
  const m1 = moment.utc('06-05-1996', 'MM-DD-YYYY');
  const m2 = moment.utc('11-05-1996', 'MM-DD-YYYY');
  const m3 = moment.utc('08-12-1996', 'MM-DD-YYYY');
  const m4 = moment.utc('01-01-2012', 'MM-DD-YYYY');
  const sStart = '1996-08-12T00:00:00.000Z';
  const sEnd = '2012-01-01T00:00:00.000Z';

  describe('constructor', function() {
    it('should allow initialization with date string', function() {
      const dr = range(sStart, sEnd);

      expect(moment.isMoment(dr.start)).toBeTruthy();
      expect(moment.isMoment(dr.end)).toBeTruthy();
    });

    it('should allow initialization with Date object', function() {
      const dr = range(d1, d2);

      expect(moment.isMoment(dr.start)).toBeTruthy();
      expect(moment.isMoment(dr.end)).toBeTruthy();
    });

    it('should allow initialization with Moment object', function() {
      const dr = range(m1, m2);

      expect(moment.isMoment(dr.start)).toBeTruthy();
      expect(moment.isMoment(dr.end)).toBeTruthy();
    });

    it('should allow initialization with an ISO 8601 Time Interval string', function() {
      const start = '2015-01-17T09:50:04+00:00';
      const end   = '2015-04-17T08:29:55+00:00';
      const dr = range(start + '/' + end);

      expect(moment.utc(start).isSame(dr.start)).toBeTruthy();
      expect(moment.utc(end).isSame(dr.end)).toBeTruthy();
    });

    it('should allow initialization with an array', function() {
      const dr = range(...[m1, m2]);

      expect(m1.isSame(dr.start)).toBeTruthy();
      expect(m2.isSame(dr.end)).toBeTruthy();
    });

    it('should allow initialization with open-ended ranges', function() {
      let dr = range(null, m1);

      expect(moment.isMoment(dr.start)).toBeTruthy();

      dr = range(m1, null);

      expect(moment.isMoment(dr.end)).toBeTruthy();
    });

    it('should allow initialization without any arguments', function() {
      const dr = range();

      expect(moment.isMoment(dr.start)).toBeTruthy();
      expect(moment.isMoment(dr.end)).toBeTruthy();
    });

    it('should allow initialization with undefined arguments', function() {
      const dr = range(undefined, undefined);

      expect(moment.isMoment(dr.start)).toBeTruthy();
      expect(moment.isMoment(dr.end)).toBeTruthy();
    });

    it('should allow initialization with moment interval strings', function() {
      const date = moment('2016-12-12T11:12:18.607');
      const quarterStart = moment('2016-10-01T00:00:00.000');
      const quarterEnd = moment('2016-12-31T23:59:59.999');
      const r = rangeInterval(date, 'quarter');

      expect(r.start.isSame(quarterStart)).toBeTruthy();
      expect(r.end.isSame(quarterEnd)).toBeTruthy();
    });
  });

  describe('#adjacent', function() {
    it('should correctly indicate when ranges aren\'t adjacent', function() {
      const a = range(d4, d1);
      const b = range(d3, d2);

      expect(a.adjacent(b)).toBeFalsy();
    });

    it('should correctly indicate when a.start == b.start', function() {
      const a = moment('15-Mar-2016');
      const b = moment('29-Mar-2016');
      const c = moment('15-Mar-2016');
      const d = moment('30-Mar-2016');

      const range1 = range(a, b);
      const range2 = range(c, d);

      expect(range1.adjacent(range2)).toBeFalsy();
    });

    it('should correctly indicate when a.start == b.end', function() {
      const a = moment('15-Mar-2016');
      const b = moment('29-Mar-2016');
      const c = moment('10-Mar-2016');
      const d = moment('15-Mar-2016');

      const range1 = range(a, b);
      const range2 = range(c, d);

      expect(range1.adjacent(range2)).toBeTruthy();
    });

    it('should correctly indicate when a.end == b.start', function() {
      const a = moment('15-Mar-2016');
      const b = moment('20-Mar-2016');
      const c = moment('20-Mar-2016');
      const d = moment('25-Mar-2016');

      const range1 = range(a, b);
      const range2 = range(c, d);

      expect(range1.adjacent(range2)).toBeTruthy();
    });

    it('should correctly indicate when a.end == b.end', function() {
      const a = moment('15-Mar-2016');
      const b = moment('20-Mar-2016');
      const c = moment('10-Mar-2016');
      const d = moment('20-Mar-2016');

      const range1 = range(a, b);
      const range2 = range(c, d);

      expect(range1.adjacent(range2)).toBeFalsy();
    });
  });

  describe('#clone()', function() {
    it('should deep clone range', function() {
      const dr1 = range(sStart, sEnd);
      const dr2 = dr1.clone();

      dr2.start.add('days', 2);
      expect(dr1.start.toDate()).not.toBe(dr2.start.toDate());
    });
  });

  describe('#by', function() {
    it('should return a valid iterator', function() {
      const d1 = new Date(2012, 2, 1);
      const d2 = new Date(2012, 2, 5);
      const dr1 = range(d1, d2);

      // Splat
      const i1 = dr1.by('day');
      expect(Array.from(i1).length).toBe(5);

      // For/of
      const i2 = dr1.by('day');
      let i = 0;
      for (let n of Array.from(i2)) {
        i++;
      }
      expect(i).toBe(5);

      // Array.from
      const i3 = dr1.by('day');
      const acc = Array.from(i3);
      expect(acc.length).toBe(5);
    });

    it('should iterate correctly by shorthand string', function() {
      const d1 = new Date(2012, 2, 1);
      const d2 = new Date(2012, 2, 5);
      const dr1 = range(d1, d2);

      const i1 = dr1.by('days');
      const acc = Array.from(i1);

      expect(acc.length).toEqual(5);
      expect(acc[0].date()).toEqual(1);
      expect(acc[1].date()).toEqual(2);
      expect(acc[2].date()).toEqual(3);
      expect(acc[3].date()).toEqual(4);
      expect(acc[4].date()).toEqual(5);
    });

    it('should iterate correctly by year over a Date-constructed range when leap years are involved', function() {
      const d1 = new Date(Date.UTC(2011, 1, 1));
      const d2 = new Date(Date.UTC(2013, 1, 1));
      const dr1 = range(d1, d2);

      const i1 = dr1.by('years');
      const acc = Array.from(i1).map(m => m.utc().year());

      expect(acc).toEqual([2011, 2012, 2013]);
    });

    it('should iterate correctly by year over a moment()-constructed range when leap years are involved', function() {
      const dr1 = range(moment('2011', 'YYYY'), moment('2013', 'YYYY'));

      const i1 = dr1.by('years');
      const acc = Array.from(i1).map(m => m.year());

      expect(acc).toEqual([2011, 2012, 2013]);
    });

    it('should iterate correctly by month over a moment()-constructed range when leap years are involved', function() {
      const dr1 = range(moment.utc('2012-01', 'YYYY-MM'), moment.utc('2012-03', 'YYYY-MM'));

      const i1 = dr1.by('months');
      const acc = Array.from(i1).map(m => m.utc().format('YYYY-MM'));

      expect(acc).toEqual(['2012-01', '2012-02', '2012-03']);
    });

    it('should iterate correctly by month over a Date-contstructed range when leap years are involved', function() {
      const d1 = new Date(Date.UTC(2012, 0));
      const d2 = new Date(Date.UTC(2012, 2));
      const dr1 = range(d1, d2);

      const i1 = dr1.by('months');
      const acc = Array.from(i1).map(m => m.utc().format('YYYY-MM'));

      expect(acc).toEqual(['2012-01', '2012-02', '2012-03']);
    });

    it('should not include .end in the iteration if exclusive is set to true when iterating by string', function() {
      const my1 = moment('2014-04-02T00:00:00.000Z');
      const my2 = moment('2014-04-04T00:00:00.000Z');
      const dr1 = range(my1, my2);
      const options = { exclusive: true, step: 1 };
      let acc;

      acc = Array.from(dr1.by('d', options)).map(m => m.utc().format('YYYY-MM-DD'));
      expect(acc).toEqual(['2014-04-02', '2014-04-03']);

      acc = Array.from(dr1.by('d')).map(m => m.utc().format('YYYY-MM-DD'));
      expect(acc).toEqual(['2014-04-02', '2014-04-03', '2014-04-04']);
    });

    it('should be exlusive when using by with minutes as well', function() {
      const d1 = moment('2014-01-01T00:00:00.000Z');
      const d2 = moment('2014-01-01T00:06:00.000Z');
      const dr = range(d1, d2);
      const options = { exclusive: true, step: 1 };
      let acc;

      acc = Array.from(dr.by('m')).map(m => m.utc().format('mm'));
      expect(acc).toEqual(['00', '01', '02', '03', '04', '05', '06']);

      acc = Array.from(dr.by('m', options)).map(m => m.utc().format('mm'));
      expect(acc).toEqual(['00', '01', '02', '03', '04', '05']);
    });

    it('should correctly iterate by a given step', function() {
      const my1 = moment('2014-04-02T00:00:00.000Z');
      const my2 = moment('2014-04-08T00:00:00.000Z');
      const dr1 = range(my1, my2);

      const acc = Array.from(dr1.by('days', { exclusive: false, step: 2 })).map(m => m.utc().format('DD'));
      expect(acc).toEqual(['02', '04', '06', '08']);
    });

    it('should correctly iterate by a given step when exclusive', function() {
      const my1 = moment('2014-04-02T00:00:00.000Z');
      const my2 = moment('2014-04-08T00:00:00.000Z');
      const dr1 = range(my1, my2);

      const acc = Array.from(dr1.by('days', { exclusive: true, step: 2 })).map(m => m.utc().format('DD'));
      expect(acc).toEqual(['02', '04', '06']);
    });
  });

  describe('#reverseBy', function() {
    it('should return a valid iterator', function() {
      const d1 = new Date(Date.UTC(2013, 2, 1));
      const d2 = new Date(Date.UTC(2013, 2, 5));
      const dr1 = range(d1, d2);

      // Splat
      const i1 = dr1.reverseBy('day');
      expect(Array.from(i1).length).toBe(5);

      // For/of
      const i2 = dr1.reverseBy('day');
      let i = 0;
      for (let n of Array.from(i2)) {
        i++;
      }
      expect(i).toBe(5);

      // Array.from
      const i3 = dr1.reverseBy('day');
      const acc = Array.from(i3);
      expect(acc.length).toBe(5);
    });

    it('should iterate correctly by shorthand string', function() {
      const d1 = new Date(Date.UTC(2013, 2, 1));
      const d2 = new Date(Date.UTC(2013, 2, 5));
      const dr1 = range(d1, d2);

      const i1 = dr1.reverseBy('days');
      const acc = Array.from(i1);

      expect(acc.length).toEqual(5);
      expect(acc[0].utc().date()).toEqual(5);
      expect(acc[1].utc().date()).toEqual(4);
      expect(acc[2].utc().date()).toEqual(3);
      expect(acc[3].utc().date()).toEqual(2);
      expect(acc[4].utc().date()).toEqual(1);
    });

    it('should iterate correctly by year over a Date-constructed range when leap years are involved', function() {
      const d1 = new Date(Date.UTC(2011, 1, 1));
      const d2 = new Date(Date.UTC(2013, 1, 1));
      const dr1 = range(d1, d2);

      const i1 = dr1.reverseBy('years');
      const acc = Array.from(i1).map(m => m.utc().year());

      expect(acc).toEqual([2013, 2012, 2011]);
    });

    it('should iterate correctly by year over a moment()-constructed range when leap years are involved', function() {
      const dr1 = range(moment('2011', 'YYYY'), moment('2013', 'YYYY'));

      const i1 = dr1.reverseBy('years');
      const acc = Array.from(i1).map(m => m.year());

      expect(acc).toEqual([2013, 2012, 2011]);
    });

    it('should iterate correctly by month over a moment()-constructed range when leap years are involved', function() {
      const dr1 = range(moment.utc('2012-01', 'YYYY-MM'), moment.utc('2012-03', 'YYYY-MM'));

      const i1 = dr1.reverseBy('months');
      const acc = Array.from(i1).map(m => m.utc().format('YYYY-MM'));

      expect(acc).toEqual(['2012-03', '2012-02', '2012-01']);
    });

    it('should iterate correctly by month over a Date-contstructed range when leap years are involved', function() {
      const d1 = new Date(Date.UTC(2012, 0, 1));
      const d2 = new Date(Date.UTC(2012, 2, 28));
      const dr1 = range(d1, d2);

      const i1 = dr1.reverseBy('months');
      const acc = Array.from(i1).map(m => m.utc().format('YYYY-MM'));

      expect(acc).toEqual(['2012-03', '2012-02', '2012-01']);
    });

    it('should not include .start in the iteration if exclusive is set to true when iterating by string', function() {
      const my1 = moment.utc('2014-04-02T00:00:00');
      const my2 = moment.utc('2014-04-04T23:59:59');
      const dr1 = range(my1, my2);
      const options = { exclusive: true, step:1 };
      let acc;

      acc = Array.from(dr1.reverseBy('d', options)).map(m => m.utc().format('YYYY-MM-DD'));
      expect(acc).toEqual(['2014-04-04', '2014-04-03']);

      acc = Array.from(dr1.reverseBy('d')).map(m => m.utc().format('YYYY-MM-DD'));
      expect(acc).toEqual(['2014-04-04', '2014-04-03', '2014-04-02']);
    });

    it('should be exlusive when using by with minutes as well', function() {
      const d1 = moment('2014-01-01T00:00:00.000Z');
      const d2 = moment('2014-01-01T00:06:00.000Z');
      const dr = range(d1, d2);
      const options = { exclusive: true, step:1 };
      let acc;

      acc = Array.from(dr.reverseBy('m')).map(m => m.utc().format('mm'));
      expect(acc).toEqual(['06', '05', '04', '03', '02', '01', '00']);

      acc = Array.from(dr.reverseBy('m', options)).map(m => m.utc().format('mm'));
      expect(acc).toEqual(['06', '05', '04', '03', '02', '01']);
    });

    it('should correctly iterate by a given step', function() {
      const my1 = moment('2014-04-02T00:00:00.000Z');
      const my2 = moment('2014-04-08T00:00:00.000Z');
      const dr1 = range(my1, my2);

      const acc = Array.from(dr1.reverseBy('days', { step: 2, exclusive:false })).map(m => m.utc().format('DD'));
      expect(acc).toEqual(['08', '06', '04', '02']);
    });

    it('should correctly iterate by a given step when exclusive', function() {
      const my1 = moment('2014-04-02T00:00:00.000Z');
      const my2 = moment('2014-04-08T00:00:00.000Z');
      const dr1 = range(my1, my2);

      const acc = Array.from(dr1.reverseBy('days', { exclusive: true, step: 2 })).map(m => m.utc().format('DD'));
      expect(acc).toEqual(['08', '06', '04']);
    });
  });

  // describe('#byRange', function() {
  //   it('should return a valid iterator', function() {
  //     const d1 = new Date(Date.UTC(2012, 2, 1));
  //     const d2 = new Date(Date.UTC(2012, 2, 5));
  //     const d3 = new Date(Date.UTC(2012, 2, 15));
  //     const d4 = new Date(Date.UTC(2012, 2, 16));
  //     const dr1 = range(d1, d2);
  //     const dr2 = range(d3, d4);

  //     // Splat
  //     const i1 = dr1.byRange(dr2);
  //     expect([...i1].length).toBe(5);

  //     // For/of
  //     const i2 = dr1.byRange(dr2);
  //     let i = 0;
  //     for (let n of i2) {
  //       i++;
  //     }
  //     expect(i).toBe(5);

  //     // Array.from
  //     const i3 = dr1.byRange(dr2);
  //     const acc = Array.from(i3);
  //     expect(acc.length).toBe(5);
  //   });

  //   it('should iterate correctly by range', function() {
  //     const d1 = new Date(Date.UTC(2012, 2, 1));
  //     const d2 = new Date(Date.UTC(2012, 2, 5));
  //     const dr1 = range(d1, d2);
  //     const dr2 = range(1000 * 60 * 60 * 24);

  //     const acc = Array.from(dr1.byRange(dr2));

  //     expect(acc.length).toEqual(5);
  //     expect(acc[0].utc().date()).toEqual(1);
  //     expect(acc[1].utc().date()).toEqual(2);
  //     expect(acc[2].utc().date()).toEqual(3);
  //     expect(acc[3].utc().date()).toEqual(4);
  //     expect(acc[4].utc().date()).toEqual(5);
  //   });

  //   it('should iterate correctly by duration', function() {
  //     const d1 = new Date(Date.UTC(2014, 9, 6, 0, 0));
  //     const d2 = new Date(Date.UTC(2014, 9, 6, 23, 59));
  //     const dr1 = range(d1, d2);
  //     const dr2 = moment.duration(15, 'minutes');

  //     const acc = Array.from(dr1.byRange(dr2));

  //     expect(acc.length).toEqual(96);
  //     expect(acc[0].minute()).toEqual(0);
  //     expect(acc[95].minute()).toEqual(45);
  //   });

  //   it('should not include .end in the iteration if exclusive is set to true when iterating by range', function() {
  //     const my1 = moment('2014-04-02T00:00:00.000Z');
  //     const my2 = moment('2014-04-04T00:00:00.000Z');
  //     const dr1 = range(my1, my2);
  //     const dr2 = range(my1, moment('2014-04-03T00:00:00.000Z'));
  //     let acc;

  //     acc = Array.from(dr1.byRange(dr2)).map(m => m.utc().format('YYYY-MM-DD'));
  //     expect(acc).toEqual(['2014-04-02', '2014-04-03', '2014-04-04']);

  //     acc = Array.from(dr1.byRange(dr2, { exclusive: false, step:1 })).map(m => m.utc().format('YYYY-MM-DD'));
  //     expect(acc).toEqual(['2014-04-02', '2014-04-03', '2014-04-04']);

  //     acc = Array.from(dr1.byRange(dr2, { exclusive: true, step:1 })).map(m => m.utc().format('YYYY-MM-DD'));
  //     expect(acc).toEqual(['2014-04-02', '2014-04-03']);
  //   });

  //   it('should iterate correctly by a given step', function() {
  //     const d1 = new Date(Date.UTC(2012, 2, 2));
  //     const d2 = new Date(Date.UTC(2012, 2, 6));
  //     const dr1 = range(d1, d2);
  //     const dr2 = moment.duration(1000 * 60 * 60 * 24);

  //     const acc = Array.from(dr1.byRange(dr2, { step: 2, exclusive:false })).map(m => m.utc().format('DD'));

  //     expect(acc).toEqual(['02', '04', '06']);
  //   });

  //   it('should iterate correctly by a given step when exclusive', function() {
  //     const d1 = new Date(Date.UTC(2012, 2, 2));
  //     const d2 = new Date(Date.UTC(2012, 2, 6));
  //     const dr1 = range(d1, d2);
  //     const dr2 = 1000 * 60 * 60 * 24;

  //     const acc = Array.from(dr1.byRange(dr2, { exclusive: true, step: 2 })).map(m => m.utc().format('DD'));

  //     expect(acc).toEqual(['02', '04']);
  //   });
  // });

  // describe('#reverseByRange', function() {
  //   it('should return a valid iterator', function() {
  //     const d1 = new Date(Date.UTC(2012, 2, 1));
  //     const d2 = new Date(Date.UTC(2012, 2, 5));
  //     const d3 = new Date(Date.UTC(2012, 2, 15));
  //     const d4 = new Date(Date.UTC(2012, 2, 16));
  //     const dr1 = range(d1, d2);
  //     const dr2 = range(d3, d4);

  //     // Splat
  //     const i1 = dr1.reverseByRange(dr2);
  //     expect([...i1].length).toBe(5);

  //     // For/of
  //     const i2 = dr1.reverseByRange(dr2);
  //     let i = 0;
  //     for (let n of i2) {
  //       i++;
  //     }
  //     expect(i).toBe(5);

  //     // Array.from
  //     const i3 = dr1.reverseByRange(dr2);
  //     const acc = Array.from(i3);
  //     expect(acc.length).toBe(5);
  //   });

  //   it('should iterate correctly by range', function() {
  //     const d1 = new Date(Date.UTC(2012, 2, 1));
  //     const d2 = new Date(Date.UTC(2012, 2, 5));
  //     const dr1 = range(d1, d2);
  //     const dr2 = 1000 * 60 * 60 * 24;

  //     const acc = Array.from(dr1.reverseByRange(dr2));

  //     expect(acc.length).toEqual(5);
  //     expect(acc[0].utc().date()).toEqual(5);
  //     expect(acc[1].utc().date()).toEqual(4);
  //     expect(acc[2].utc().date()).toEqual(3);
  //     expect(acc[3].utc().date()).toEqual(2);
  //     expect(acc[4].utc().date()).toEqual(1);
  //   });

  //   it('should iterate correctly by duration', function() {
  //     const d1 = new Date(Date.UTC(2014, 9, 6, 0, 1));
  //     const d2 = new Date(Date.UTC(2014, 9, 7, 0, 0));
  //     const dr1 = range(d1, d2);
  //     const dr2 = moment.duration(15, 'minutes');

  //     const acc = Array.from(dr1.reverseByRange(dr2));

  //     expect(acc.length).toEqual(96);
  //     expect(acc[0].minute()).toEqual(0);
  //     expect(acc[95].minute()).toEqual(15);
  //   });

  //   it('should not include .start in the iteration if exclusive is set to true when iterating by range', function() {
  //     const my1 = moment('2014-04-02T00:00:00.000Z');
  //     const my2 = moment('2014-04-04T00:00:00.000Z');
  //     const dr1 = range(my1, my2);
  //     const dr2 = range(my1, moment('2014-04-03T00:00:00.000Z'));
  //     let acc;

  //     acc = Array.from(dr1.reverseByRange(dr2)).map(m => m.utc().format('YYYY-MM-DD'));
  //     expect(acc).toEqual(['2014-04-04', '2014-04-03', '2014-04-02']);

  //     acc = Array.from(dr1.reverseByRange(dr2, { exclusive: false, step:1 })).map(m => m.utc().format('YYYY-MM-DD'));
  //     expect(acc).toEqual(['2014-04-04', '2014-04-03', '2014-04-02']);

  //     acc = Array.from(dr1.reverseByRange(dr2, { exclusive: true, step:1 })).map(m => m.utc().format('YYYY-MM-DD'));
  //     expect(acc).toEqual(['2014-04-04', '2014-04-03']);
  //   });

  //   it('should iterate correctly by a given step', function() {
  //     const d1 = new Date(Date.UTC(2012, 2, 2));
  //     const d2 = new Date(Date.UTC(2012, 2, 6));
  //     const dr1 = range(d1, d2);
  //     const dr2 = 1000 * 60 * 60 * 24;

  //     const acc = Array.from(dr1.reverseByRange(dr2, { step: 2, exclusive:false })).map(m => m.utc().format('DD'));

  //     expect(acc).toEqual(['06', '04', '02']);
  //   });

  //   it('should iterate correctly by a given step when exclusive', function() {
  //     const d1 = new Date(Date.UTC(2012, 2, 2));
  //     const d2 = new Date(Date.UTC(2012, 2, 6));
  //     const dr1 = range(d1, d2);
  //     const dr2 = 1000 * 60 * 60 * 24;

  //     const acc = Array.from(dr1.reverseByRange(dr2, { exclusive: true, step: 2 })).map(m => m.utc().format('DD'));

  //     expect(acc).toEqual(['06', '04']);
  //   });
  // });

  describe('#contains()', function() {
    it('should work with Date objects', function() {
      const dr = range(d1, d2);

      expect(dr.contains(d3)).toBeTruthy();
      expect(dr.contains(d4)).toBeFalsy();
    });

    it('should work with Moment objects', function() {
      const dr = range(m1, m2);

      expect(dr.contains(m3)).toBeTruthy();
      expect(dr.contains(m4)).toBeFalsy();
    });

    it('should work with DateRange objects', function() {
      const dr1 = range(m1, m4);
      const dr2 = range(m3, m2);

      expect(dr1.contains(dr2)).toBeTruthy();
      expect(dr2.contains(dr1)).toBeFalsy();
    });

    it('should be an inclusive comparison', function() {
      const dr1 = range(m1, m4);

      expect(dr1.contains(m1)).toBeTruthy();
      expect(dr1.contains(m4)).toBeTruthy();
      expect(dr1.contains(dr1)).toBeTruthy();
    });

    it('should be exlusive when the exclusive param is set', function() {
      const dr1 = range(m1, m2);

      expect(dr1.contains(dr1, { exclusive: true })).toBeFalsy();
      expect(dr1.contains(dr1, { exclusive: false })).toBeTruthy();
      expect(dr1.contains(dr1)).toBeTruthy();
      expect(dr1.contains(m2, { exclusive: true })).toBeFalsy();
      expect(dr1.contains(m2, { exclusive: false })).toBeTruthy();
      expect(dr1.contains(m2)).toBeTruthy();
    });
  });

  describe('#overlaps()', function() {
    it('should work with DateRange objects', function() {
      const dr1 = range(m1, m2);
      const dr2 = range(m3, m4);
      const dr3 = range(m2, m4);
      const dr4 = range(m1, m3);

      expect(dr1.overlaps(dr2)).toBeTruthy();
      expect(dr1.overlaps(dr3)).toBeFalsy();
      expect(dr4.overlaps(dr3)).toBeFalsy();
    });

    it('should indicate if ranges overlap if the options is passed in', function() {
      const a = moment('15-Mar-2016');
      const b = moment('20-Mar-2016');
      const c = moment('20-Mar-2016');
      const d = moment('25-Mar-2016');

      const range1 = range(a, b);
      const range2 = range(c, d);

      expect(range1.overlaps(range2)).toBeFalsy();
      expect(range1.overlaps(range2, { adjacent: false })).toBeFalsy();
      expect(range1.overlaps(range2, { adjacent: true })).toBeTruthy();
    });
    it('should consider ranges overlaping if partially overlapped', function(){
      const a = moment('15-Mar-2016');
      const b = moment('18-Mar-2016');
      const c = moment('20-Mar-2016');
      const d = moment('25-Mar-2016');

      const range1 = range(a, c);
      const range2 = range(b, d);

      expect(range1.overlaps(range2)).toBeTruthy();
    });
  });

  describe('#intersect()', function() {
    const d5 = new Date(Date.UTC(2011, 2, 2));
    const d6 = new Date(Date.UTC(2011, 4, 4));
    const d7 = new Date(Date.UTC(2011, 6, 6));
    const d8 = new Date(Date.UTC(2011, 8, 8));

    it('should work with [---{==]---} overlaps where (a=[], b={})', function() {
      const dr1 = range(d5, d7);
      const dr2 = range(d6, d8);

      expect(dr1.intersect(dr2).isSame(range(d6, d7))).toBeTruthy();
    });

    it('should work with {---[==}---] overlaps where (a=[], b={})', function() {
      const dr1 = range(d6, d8);
      const dr2 = range(d5, d7);

      expect(dr1.intersect(dr2).isSame(range(d6, d7))).toBeTruthy();
    });

    it('should work with [{===]---} overlaps where (a=[], b={})', function() {
      const dr1 = range(d5, d6);
      const dr2 = range(d5, d7);

      expect(dr1.intersect(dr2).isSame(range(d5, d6))).toBeTruthy();
    });

    it('should work with {[===}---] overlaps where (a=[], b={})', function() {
      const dr1 = range(d5, d7);
      const dr2 = range(d5, d6);

      expect(dr1.intersect(dr2).isSame(range(d5, d6))).toBeTruthy();
    });

    it('should work with [---{===]} overlaps where (a=[], b={})', function() {
      const dr1 = range(d5, d7);
      const dr2 = range(d6, d7);

      expect(dr1.intersect(dr2).isSame(range(d6, d7))).toBeTruthy();
    });

    it('should work with {---[===}] overlaps where (a=[], b={})', function() {
      const dr1 = range(d6, d7);
      const dr2 = range(d5, d7);

      expect(dr1.intersect(dr2).isSame(range(d6, d7))).toBeTruthy();
    });

    it('should work with [---] {---} overlaps where (a=[], b={})', function() {
      const dr1 = range(d5, d6);
      const dr2 = range(d7, d8);

      expect(dr1.intersect(dr2)).toBe(null);
    });

    it('should work with {---} [---] overlaps where (a=[], b={})', function() {
      const dr1 = range(d7, d8);
      const dr2 = range(d5, d6);

      expect(dr1.intersect(dr2)).toBe(null);
    });

    it('should work with [---]{---} overlaps where (a=[], b={})', function() {
      const dr1 = range(d5, d6);
      const dr2 = range(d6, d7);

      expect(dr1.intersect(dr2)).toBe(null);
    });

    it('should work with {---}[---] overlaps where (a=[], b={})', function() {
      const dr1 = range(d6, d7);
      const dr2 = range(d5, d6);
      expect(dr1.intersect(dr2)).toBe(null);
    });

    it('should work with {--[===]--} overlaps where (a=[], b={})', function() {
      const dr1 = range(d6, d7);
      const dr2 = range(d5, d8);

      expect(dr1.intersect(dr2).isSame(dr1)).toBeTruthy();
    });

    it('should work with [--{===}--] overlaps where (a=[], b={})', function() {
      const dr1 = range(d5, d8);
      const dr2 = range(d6, d7);

      expect(dr1.intersect(dr2).isSame(dr2)).toBeTruthy();
    });

    it('should work with [{===}] overlaps where (a=[], b={})', function() {
      const dr1 = range(d5, d6);
      const dr2 = range(d5, d6);

      expect(dr1.intersect(dr2).isSame(dr2)).toBeTruthy();
    });

    it('should work with [--{}--] overlaps where (a=[], b={})', function() {
      const dr1 = range(d6, d6);
      const dr2 = range(d5, d7);

      expect(dr1.intersect(dr2).isSame(dr1)).toBeTruthy();
    });
  });

  describe('#add()', function() {
    const d5 = new Date(Date.UTC(2011, 2, 2));
    const d6 = new Date(Date.UTC(2011, 4, 4));
    const d7 = new Date(Date.UTC(2011, 6, 6));
    const d8 = new Date(Date.UTC(2011, 8, 8));

    it('should add ranges with [---{==]---} overlaps where (a=[], b={})', function() {
      const dr1 = range(d5, d7);
      const dr2 = range(d6, d8);

      expect(dr1.add(dr2).isSame(range(d5, d8))).toBeTruthy();
    });

    it('should add ranges with {---[==}---] overlaps where (a=[], b={})', function() {
      const dr1 = range(d6, d8);
      const dr2 = range(d5, d7);

      expect(dr1.add(dr2).isSame(range(d5, d8))).toBeTruthy();
    });

    it('should add ranges with [{===]---} overlaps where (a=[], b={})', function() {
      const dr1 = range(d5, d6);
      const dr2 = range(d5, d7);

      expect(dr1.add(dr2).isSame(range(d5, d7))).toBeTruthy();
    });

    it('should add ranges with {[===}---] overlaps where (a=[], b={})', function() {
      const dr1 = range(d5, d7);
      const dr2 = range(d5, d6);

      expect(dr1.add(dr2).isSame(range(d5, d7))).toBeTruthy();
    });

    it('should add ranges with [---{===]} overlaps where (a=[], b={})', function() {
      const dr1 = range(d5, d7);
      const dr2 = range(d6, d7);

      expect(dr1.add(dr2).isSame(range(d5, d7))).toBeTruthy();
    });

    it('should add ranges with {---[===}] overlaps where (a=[], b={})', function() {
      const dr1 = range(d6, d7);
      const dr2 = range(d5, d7);

      expect(dr1.add(dr2).isSame(range(d5, d7))).toBeTruthy();
    });

    it('should not add ranges with [---] {---} overlaps where (a=[], b={})', function() {
      const dr1 = range(d5, d6);
      const dr2 = range(d7, d8);

      expect(dr1.add(dr2)).toBe(null);
    });

    it('should not add ranges with {---} [---] overlaps where (a=[], b={})', function() {
      const dr1 = range(d7, d8);
      const dr2 = range(d5, d6);

      expect(dr1.add(dr2)).toBe(null);
    });

    it('should not add ranges with [---]{---} overlaps where (a=[], b={})', function() {
      const dr1 = range(d5, d6);
      const dr2 = range(d6, d7);

      expect(dr1.add(dr2)).toBe(null);
    });

    it('should not add ranges with {---}[---] overlaps where (a=[], b={})', function() {
      const dr1 = range(d6, d7);
      const dr2 = range(d5, d6);

      expect(dr1.add(dr2)).toBe(null);
    });

    it('should add ranges {--[===]--} overlaps where (a=[], b={})', function() {
      const dr1 = range(d6, d7);
      const dr2 = range(d5, d8);

      expect(dr1.add(dr2).isSame(range(d5, d8))).toBeTruthy();
    });

    it('should add ranges [--{===}--] overlaps where (a=[], b={})', function() {
      const dr1 = range(d5, d8);
      const dr2 = range(d6, d7);

      expect(dr1.add(dr2).isSame(range(d5, d8))).toBeTruthy();
    });

    it('should add ranges [{===}] overlaps where (a=[], b={})', function() {
      const dr1 = range(d5, d6);
      const dr2 = range(d5, d6);

      expect(dr1.add(dr2).isSame(range(d5, d6))).toBeTruthy();
    });
  });

  describe('#subtract()', function() {
    const d5 = new Date(2011, 2, 2);
    const d6 = new Date(2011, 4, 4);
    const d7 = new Date(2011, 6, 6);
    const d8 = new Date(2011, 8, 8);

    it('should turn [--{==}--] into (--) (--) where (a=[], b={})', function() {
      const dr1 = range(d5, d8);
      const dr2 = range(d6, d7);
      let resultArr = dr1.subtract(dr2);

      expect( resultArr[0].isEqual(range(d5, d6)) ).toBeTruthy();
      expect(resultArr[1].isEqual(range(d7, d8)) ).toBeTruthy();
    });

    it('should turn {--[==]--} into () where (a=[], b={})', function() {
      const dr1 = range(d6, d7);
      const dr2 = range(d5, d8);

      expect(dr1.subtract(dr2)).toEqual([]);
    });

    it('should turn {[==]} into () where (a=[], b={})', function() {
      const dr1 = range(d5, d6);
      const dr2 = range(d5, d6);

      expect(dr1.subtract(dr2)).toEqual([]);
    });

    it('should turn [--{==]--} into (--) where (a=[], b={})', function() {
      const dr1 = range(d5, d7);
      const dr2 = range(d6, d8);
      let result = dr1.subtract(dr2);
      expect(result[0].isEqual(range(d5,d6))).toBeTruthy();
    });

    it('should turn [--{==]} into (--) where (a=[], b={})', function() {
      const dr1 = range(d5, d7);
      const dr2 = range(d6, d7);

      let result = dr1.subtract(dr2);
      expect(result[0].isEqual(range(d5,d6))).toBeTruthy();
    });

    it('should turn {--[==}--] into (--) where (a=[], b={})', function() {
      const dr1 = range(d6, d8);
      const dr2 = range(d5, d7);

      let result = dr1.subtract(dr2);
      expect(result[0].isEqual(range(d7,d8))).toBeTruthy();

    });

    it('should turn {[==}--] into (--) where (a=[], b={})', function() {
      const dr1 = range(d6, d8);
      const dr2 = range(d6, d7);
      let result = dr1.subtract(dr2);
      expect(result[0].isEqual(range(d7,d8))).toBeTruthy();

    });

    it('should turn [--] {--} into (--) where (a=[], b={})', function() {
      const dr1 = range(d5, d6);
      const dr2 = range(d7, d8);

      expect(dr1.subtract(dr2)).toEqual([dr1]);
    });

    it('should turn {--} [--] into (--) where (a=[], b={})', function() {
      const dr1 = range(d7, d8);
      const dr2 = range(d5, d6);

      expect(dr1.subtract(dr2)).toEqual([dr1]);
    });

    it('should turn [--{==}--] into (--) where (a=[], b={})', function() {
      const o = range('2015-04-07T00:00:00+00:00/2015-04-08T00:00:00+00:00');
      const s = range('2015-04-07T17:12:18+00:00/2015-04-07T17:12:18+00:00');
      const subtraction = o.subtract(s);
      const a = range('2015-04-07T00:00:00+00:00/2015-04-07T17:12:18+00:00');
      const b = range('2015-04-07T17:12:18+00:00/2015-04-08T00:00:00+00:00');

      expect(subtraction[0].start.isSame(a.start)).toBeTruthy();
      expect(subtraction[0].end.isSame(a.end)).toBeTruthy();
      expect(subtraction[1].start.isSame(b.start)).toBeTruthy();
      expect(subtraction[1].end.isSame(b.end)).toBeTruthy();
    });
  });

  describe('#isSame()', function() {
    it('should true if the start and end of both DateRange objects equal', function() {
      const dr1 = range(d1, d2);
      const dr2 = range(d1, d2);

      expect(dr1.isSame(dr2)).toBeTruthy();
    });

    it('should false if the starts differ between objects', function() {
      const dr1 = range(d1, d3);
      const dr2 = range(d2, d3);

      expect(dr1.isSame(dr2)).toBeFalsy();
    });

    it('should false if the ends differ between objects', function() {
      const dr1 = range(d1, d2);
      const dr2 = range(d1, d3);

      expect(dr1.isSame(dr2)).toBeFalsy();
    });
  });

  describe('#toString()', function() {
    it('should be a correctly formatted ISO8601 Time Interval', function() {
      const start = moment.utc('2015-01-17T09:50:04+00:00');
      const end   = moment.utc('2015-04-17T08:29:55+00:00');
      const dr = range(start, end);

      expect(dr.toString()).toEqual(start.format() + '/' + end.format());
    });
  });

  describe('#valueOf()', function() {
    it('should be the value of the range in milliseconds', function() {
      const dr = range(d1, d2);

      expect(dr.valueOf()).toEqual(d2.getTime() - d1.getTime());
    });

    it('should correctly coerce to a number', function() {
      const dr1 = range(d4, d2);
      const dr2 = range(d3, d2);

      expect((dr1 > dr2)).toBeTruthy();
    });
  });

  describe('#toDate()', function() {
    it('should be a array like [dateObject, dateObject]', function() {
      const dr = range(d1, d2);
      const drTodate = dr.toDate();

      expect(drTodate.length).toEqual(2);
      expect(drTodate[0].valueOf()).toEqual(d1.valueOf());
      expect(drTodate[1].valueOf()).toEqual(d2.valueOf());
    });
  });

  describe('#diff()', function() {
    it('should use momentjs’ diff method', function() {
      const dr = range(d1, d2);

      expect(dr.diff('months')).toEqual(3);
      expect(dr.diff('days')).toEqual(92);
      expect(dr.diff()).toEqual(7948800000);
    });

    it('should optionally pass the rounded argument', function() {
      const d1 = new Date(Date.UTC(2011, 4, 1));
      const d2 = new Date(Date.UTC(2011, 4, 5, 12));
      const dr = range(d1, d2);

      expect(dr.diff('days', true)).toEqual(4.5);
    });
  });

  describe('#center()', function() {
    it('should use momentjs’ center method', function() {
      const d1 = new Date(Date.UTC(2011, 2, 5));
      const d2 = new Date(Date.UTC(2011, 3, 5));
      const dr = range(d1, d2);

      expect(dr.center().valueOf()).toEqual(1300622400000);
    });
  });
});
