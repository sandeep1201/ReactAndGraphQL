import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'ssn'
})
export class SsnPipe implements PipeTransform {

  transform(value: any, args?: any): string {
    if (value == null) {
      return null;
    }


    const ssnString = value.toString();

    if (ssnString.length !== 9) {
      return ssnString;
    }

    const firstPart = ssnString.substring(0, 3);
    const secondPart = ssnString.substring(3, 5);
    const thirdPart = ssnString.substring(5);

    return firstPart + '-' + secondPart + '-' + thirdPart;
  }

}
