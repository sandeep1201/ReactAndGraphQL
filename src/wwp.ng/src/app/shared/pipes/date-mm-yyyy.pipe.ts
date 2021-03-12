import { Pipe, PipeTransform } from '@angular/core';
import * as moment from 'moment';

@Pipe({
  name: 'dateMmYyyy'
})
export class DateMmYyyyPipe implements PipeTransform {


  transform(value: any, args?: any): any {
    if (value == null || value === '') {
      return;
    }

    const date = moment(value);

    if (date.isValid() === true) {
      return date.format('MM/YYYY');
    }

  }

}
