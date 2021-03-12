import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'arraySeparatorByChar'
})
export class ArraySeparatorByCharPipe implements PipeTransform {

  transform(value: string[], char: string): any {
    if (value == null) {
      return;
    }

    for (let i = 0; i <= value.length; i++) {
      if (value.length - 1 !== i && value[i] != null) {
        value[i] = value[i] + char;
      }
    }

    return value;
  }

}
