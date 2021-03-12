// augmentations.ts
// TODO: Remove this when RxJS releases a stable version with a correct declaration of `Subject`. (v6?)
import {Operator, Observable} from 'rxjs'

declare module 'rxjs/Subject' {
  interface Subject<T> {
    lift<R>(operator: Operator<T, R>): Observable<R>
  }
}