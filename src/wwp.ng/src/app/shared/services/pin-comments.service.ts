// tslint:disable: import-blacklist
// tslint:disable: deprecation
import { Injectable } from '@angular/core';
import { AuthHttpClient } from './../../core/interceptors/AuthHttpClient';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { AppService } from './../../core/services/app.service';
import { AccessType } from '../enums/access-type.enum';
import { map, catchError } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { CommentModel } from '../components/comment/comment.model';

@Injectable()
export class PinCommentsService {
  private pinCommentsUrl: string;
  public participantPin: string;

  constructor(private http: AuthHttpClient, private appService: AppService) {
    this.pinCommentsUrl = this.appService.apiServer + 'api/pin-comment/pin';
  }

  public getAllPinCommentsForPin(pin: string) {
    const requestUrl = `${this.pinCommentsUrl}/${pin}`;
    return this.http.get(requestUrl).pipe(
      map(res => this.extractAllPinCommentData(res)),
      catchError(err => {
        return this.handleError(err);
      })
    );
  }

  public getPinComment(pin: string, id: number) {
    const requestUrl = `${this.pinCommentsUrl}/${pin}/${id}`;
    return this.http.get(requestUrl).pipe(
      map(res => this.extractComment(res)),
      catchError(err => {
        return this.handleError(err);
      })
    );
  }

  private extractAllPinCommentData(res: CommentModel[]) {
    const jsonObjs = res as CommentModel[];
    const objs: CommentModel[] = [];

    for (const obj of jsonObjs) {
      objs.push(new CommentModel().deserialize(obj));
    }

    return objs || [];
  }

  public extractComment(res: CommentModel[]) {
    const body = res as CommentModel[];
    const pinComment = new CommentModel().deserialize(body);
    return pinComment || null;
  }

  private handleError(error: any): Observable<any> {
    if (error instanceof HttpErrorResponse) {
      return throwError(error);
    } else {
      const errMsg = error.message ? error.message : error.status ? `${error.status} - ${error.statusText}` : 'Server error';
      return throwError(errMsg);
    }
  }
}
