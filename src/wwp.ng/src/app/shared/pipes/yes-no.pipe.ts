import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'yesNo'
})
export class YesNoPipe implements PipeTransform {

  transform(value: any): string {
    switch (value) {
      case 'true':
        return 'Yes';

      case 'false':
        return 'No';

      case true:
        return 'Yes';

      case false:
        return 'No';

      default:
        return '';
    }
  }
}
