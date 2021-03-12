import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';

@Pipe({
  name: 'possibleHighDate'
})
export class HighDatePipe implements PipeTransform {

  transform(value: any, args?: any): any {

    const date = moment(value);

    if (date.isValid() === true &&  date.format('MM/DD/YYYY') === '12/31/9999') {
      return 'Unknown';
    }
    return value;
  }

}
