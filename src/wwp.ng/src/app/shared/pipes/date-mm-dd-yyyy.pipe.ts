import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';

/**
 *  Takes datetime and transforms it into 'MM/DD/YYYY with time'
 * 
 * @export
 * @class DateMmDdYyyyPipeTime
 * @implements {PipeTransform}
 */
@Pipe({
  name: 'dateMmDdYyyy'
})
export class DateMmDdYyyyPipe implements PipeTransform {

  transform(value: any, args?: any): any {
    if (value == null || value === '') {
      return;
    }

    const date = moment(value);

    if (date.isValid() === true) {
      return date.format('MM/DD/YYYY');
    }

  }
}
