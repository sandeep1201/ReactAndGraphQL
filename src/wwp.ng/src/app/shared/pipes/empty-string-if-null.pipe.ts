import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'emptyStringIfNull'
})
export class EmptyStringIfNullPipe implements PipeTransform {

  transform(value: string): string {
    if (value == null) {
      return '';
    }

    return value;
  }

}
