import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'padPipe'
})
//  this accepts two argumanets if the value's length is less the padNumber it will append that many 0 to the front of the value
//  example value = 1234 and padNumber is 6 the return value is 001234
export class PadPipe implements PipeTransform {
  transform(value: string | number, padNumber: number): any {
    if (value == null || padNumber == null) {
      return null;
    }

    if (value.toString().length < +padNumber) {
      const diff = +padNumber - value.toString().length;
      for (let i = 0; i < diff; i++) {
        value = '0' + value;
      }
    }

    return value;
  }
}
