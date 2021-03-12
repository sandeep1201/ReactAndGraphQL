import * as moment from 'moment';

import { GenericIterator } from './utilities';
import { unitOfTime, MomentInputObject, Duration } from 'moment';


//-----------------------------------------------------------------------------
// Constants
//-----------------------------------------------------------------------------

const INTERVALS = {
    year: true,
    quarter: true,
    month: true,
    week: true,
    day: true,
    hour: true,
    minute: true,
    second: true
};

class RangeIterator extends GenericIterator<moment.Moment> {

    public next = (): IteratorResult<moment.Moment> => {
        return { done: true, value: null };
    }

    public return?= (): IteratorResult<moment.Moment> => null;
    public throw?= (): IteratorResult<moment.Moment> => null;

    [Symbol.iterator](): IterableIterator<moment.Moment> {
        return this;
    }
}

//-----------------------------------------------------------------------------
// Date Ranges
//-----------------------------------------------------------------------------
export class DateRange implements Range {
    public start: moment.Moment;
    public end: moment.Moment;

    constructor(start: moment.MomentInput, end?: moment.MomentInput) {

        this.start = (start == null) ? moment(-8640000000000000) : moment(start);
        this.end = (end == null) ? moment(8640000000000000) : moment(end);
    }

    adjacent(other: Range) {
        const sameStartEnd = this.start.isSame(other.end);
        const sameEndStart = this.end.isSame(other.start);

        return (sameStartEnd && (other.start.valueOf() <= this.start.valueOf())) || (sameEndStart && (other.end.valueOf() >= this.end.valueOf()));
    }

    add(other: Range) {
        if (this.overlaps(other)) {
            return new DateRange(moment.min(this.start, other.start), moment.max(this.end, other.end));
        }

        return null;
    }

    by(interval:moment.unitOfTime.Diff, options = { exclusive: false, step: 1 }): IterableIterator<moment.Moment> {
        const range = this;
        const exclusive = options.exclusive || false;
        const step = options.step || 1;
        const diff = Math.abs(range.start.diff(range.end, interval)) / step;
        let iteration = 0;
        const next = (value?: any): IteratorResult<moment.Moment> => {

            const current = range.start.clone().add((iteration * step), interval);
            const done = exclusive
                ? !(iteration < diff)
                : !(iteration <= diff);

            iteration++;

            return {
                done,
                value: (done ? undefined : current)
            };
        };

        let iterable = new RangeIterator();
        iterable.next = next;
        return iterable;

    }

    // byRange(intervalRange:any, options = { exclusive: false, step: 1 }) {
    //     const range = this;
    //     const step = options.step || 1;
    //     let diff = 0;
    //         diff = this.valueOf() / intervalRange.valueOf() / step;
    //     const exclusive = options.exclusive || false;
    //     const unit = Math.floor(diff);
    //     let iteration = 0;

    //     let iterable = new RangeIterator();

    //     if (unit !== Infinity) {

    //         const next = (value?: any): IteratorResult<moment.Moment> => {
    //             const current = moment(range.start.valueOf() + (intervalRange.valueOf() * iteration * step));
    //             const done = ((unit === diff) && exclusive)
    //                 ? !(iteration < unit)
    //                 : !(iteration <= unit);

    //             iteration++;

    //             return {
    //                 done,
    //                 value: (done ? undefined : current)
    //             };
    //         };
    //         iterable.next = next;
    //     }
    //     return iterable;
    // }

    center(): moment.Moment {
        const center = this.start.valueOf() + this.diff() / 2;

        return moment(center);
    }

    clone() {
        return new DateRange(this.start, this.end);
    }

    contains(other: moment.MomentInput | DateRange, options = { exclusive: false }) {
        const start = this.start.valueOf();
        const end = this.end.valueOf();
        let oStart: number;
        let oEnd: number;

        if (other instanceof DateRange) {
            oStart = other.start.valueOf();
            oEnd = other.end.valueOf();
        } else {
            // Some kinda of Moment or moment input
            oStart = moment(other).valueOf();
            oEnd = moment(other).valueOf();
        }

        const startInRange = (start < oStart) || ((start <= oStart) && !options.exclusive);
        const endInRange = (end > oEnd) || ((end >= oEnd) && !options.exclusive);

        return (startInRange && endInRange);
    }

    diff(unitOfTime?: moment.unitOfTime.Diff, precise?: boolean) {
        return this.end.diff(this.start, unitOfTime, precise);
    }

    duration(unitOfTime?: moment.unitOfTime.Diff, precise?: boolean) {
        return this.diff(unitOfTime, precise);
    }

    intersect(other: Range): Range {
        const start = this.start.valueOf();
        const end = this.end.valueOf();
        const oStart = other.start.valueOf();
        const oEnd = other.end.valueOf();

        if ((start <= oStart) && (oStart < end) && (end < oEnd)) {
            return new DateRange(oStart, end);
        }
        else if ((oStart < start) && (start < oEnd) && (oEnd <= end)) {
            return new DateRange(start, oEnd);
        }
        else if ((oStart < start) && (start <= end) && (end < oEnd)) {
            return this;
        }
        else if ((start <= oStart) && (oStart <= oEnd) && (oEnd <= end)) {
            return other;
        }

        return null;
    }

    isEqual(other: Range, unitOfTime?: moment.unitOfTime.Diff) {
        return (other != null) && this.start.isSame(other.start, unitOfTime) && this.end.isSame(other.end, unitOfTime);
    }

    // TODO: Add test for unitOfTime
    isSame(other: Range, unitOfTime?: moment.unitOfTime.Diff) {
        return this.isEqual(other,unitOfTime);
    }

    overlaps(other: Range, options = { adjacent: false }) {
        const intersect = (this.intersect(other) !== null);

        if (options.adjacent && !intersect) {
            return this.adjacent(other);
        }

        return intersect;
    }

    reverseBy(interval: moment.unitOfTime.Diff, options = { exclusive: false, step: 1 }) {
        const range = this;
        const exclusive = options.exclusive || false;
        const step = options.step || 1;
        const diff = Math.abs(range.start.diff(range.end, interval)) / step;
        let iteration = 0;
        const next = (value?: any): IteratorResult<moment.Moment> => {

            const current = range.end.clone().subtract((iteration * step), interval);
            const done = exclusive
                ? !(iteration < diff)
                : !(iteration <= diff);

            iteration++;

            return {
                done,
                value: (done ? undefined : current)
            };
        };

        let iterable = new RangeIterator();
        iterable.next = next;
        return iterable;
    }

    // reverseByRange(intervalRange: DateRange, options = { exclusive: false, step: 1 }) {
    //     const range = this;
    //     const step = options.step || 1;
    //     const diff = this.valueOf() / intervalRange.valueOf() / step;
    //     const exclusive = options.exclusive || false;
    //     const unit = Math.floor(diff);
    //     let iteration = 0;

    //     let iterable = new RangeIterator();
    //     if (unit !== Infinity) {
    //         let next = (): IteratorResult<moment.Moment> => {
    //             const current = moment(range.end.valueOf() - (intervalRange.valueOf() * iteration * step));
    //             const done = ((unit === diff) && exclusive)
    //                 ? !(iteration < unit)
    //                 : !(iteration <= unit);

    //             iteration++;

    //             return {
    //                 done,
    //                 value: (done ? undefined : current)
    //             };
    //         }
    //         iterable.next = next;
    //     }
    //     return iterable;

    // }



    subtract(other: Range) {
        const start = this.start.valueOf();
        const end = this.end.valueOf();
        const oStart = other.start.valueOf();
        const oEnd = other.end.valueOf();

        if (this.intersect(other) === null) {
            return [this];
        }
        else if ((oStart <= start) && (start < end) && (end <= oEnd)) {
            return [];
        }
        else if ((oStart <= start) && (start < oEnd) && (oEnd < end)) {
            return [new DateRange(oEnd, end)];
        }
        else if ((start < oStart) && (oStart < end) && (end <= oEnd)) {
            return [new DateRange(start, oStart)];
        }
        else if ((start < oStart) && (oStart < oEnd) && (oEnd < end)) {
            return [new DateRange(start, oStart), new DateRange(oEnd, end)];
        }
        else if ((start < oStart) && (oStart < end) && (oEnd < end)) {
            return [new DateRange(start, oStart), new DateRange(oStart, end)];
        }

        return [];
    }

    toDate() {
        return [this.start.toDate(), this.end.toDate()];
    }

    toString() {
        return this.start.format() + '/' + this.end.format();
    }

    valueOf() {
        return this.end.valueOf() - this.start.valueOf();
    }

}

interface IteraterOptions {
    exclusive: boolean;
    step: number;
}

export interface Range {
    start: moment.Moment;
    end: moment.Moment;

    adjacent(other: Range): boolean;

    add(other: Range): Range;

    by(interval:moment.unitOfTime.Diff, options: IteraterOptions): RangeIterator;

    // byRange(interval:Range, options: IteraterOptions): RangeIterator;

    center(): moment.Moment;

    clone(): Range;

    contains(other: moment.MomentInput | Range, options?: { exclusive: boolean }): boolean;

    diff(unitOfTime?: moment.unitOfTime.Diff, precise?: boolean): number;

    duration(unitOfTime?: moment.unitOfTime.Diff, precise?: boolean): number;

    intersect(other: Range): Range;

    isEqual(other: Range): boolean;

    isSame(other: DateRange): boolean;

    overlaps(other: DateRange, options?: { adjacent: false }): boolean;

    reverseBy(interval: moment.unitOfTime.Diff, options: IteraterOptions): RangeIterator;

    // reverseByRange(interval:Range, options: IteraterOptions): RangeIterator;
    subtract(other: DateRange): DateRange[];

    toDate(): Date[];

    toString(): string;

    valueOf(): number;
}

export function within(range: Range, moment: moment.Moment): boolean {
    return range.contains(moment.toDate());
}

type rangeInput = moment.Moment | Date | string | number | (number | string)[] | moment.MomentInputObject | void;
export function range(start?: moment.MomentInput, end?: moment.MomentInput) {
    if (!end && start) {
        // support range('xxx/xxx') - ISO 8601 Time Interval string with "/" seperator
        if (typeof start === 'string' && start.indexOf('/') >= 0) {
            const inputArr = start.split('/');
            start = inputArr[0];
            end = inputArr[1];
        }

        // support Range([moment1Obj,moment2Obj]) - array based initializer
        if (Array.isArray(start) && start.length === 2) {
            const temp = start;
            start = temp[0];
            end = temp[1];
        }
    }

    return new DateRange(start, end);
}

export function rangeInterval(momentInp: moment.MomentInput, unitOfTime: moment.unitOfTime.Diff) {
    let start = moment(momentInp).startOf(unitOfTime);
    let end = moment(momentInp).endOf(unitOfTime);
    return new DateRange(start, end);
}
