import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'phoneNumber'
})
export class PhoneNumberPipe implements PipeTransform {

  //transform(value: any, args?: any): any {
  transform(value: number): string {
    if (value == null) {
      return null;
    }

    let phoneNumber = value.toString();

    if (phoneNumber.length !== 10) {
      return phoneNumber;
    }

    let areaCode = phoneNumber.slice(0, 3);
    let middle = phoneNumber.slice(3, 6);
    let last4Digits = phoneNumber.slice(6, 10);

    return '(' + areaCode + ') ' + middle + '-' + last4Digits;
  }

}
