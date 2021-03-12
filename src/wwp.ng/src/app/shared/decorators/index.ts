import {coerceBoolean} from './boolean-property';
import {coerceNumber} from './number-property';

export class coerce{
  static number = coerceNumber;
  static boolean = coerceBoolean;
}
