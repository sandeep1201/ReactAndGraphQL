// tslint:disable: import-blacklist
// tslint:disable: deprecation
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthHttpClient } from 'src/app/core/interceptors/AuthHttpClient';
import { AppService } from 'src/app/core/services/app.service';
import { CommentModel } from './comment.model';
import { AccessType } from '../../enums/access-type.enum';

@Injectable()
export class CommentsService {
  public participantPin: string;
  public modeForComment = new BehaviorSubject<any>({ id: null, readOnly: false, isInEditMode: false, commentType: null });
  public modeOnSaveComment = new BehaviorSubject<any>({ id: null, commentSaved: false });

  constructor(private http: AuthHttpClient, private appService: AppService) {}

  public extractComment(res: CommentModel[]) {
    const body = res as CommentModel[];
    const pinComment = new CommentModel().deserialize(body);
    return pinComment || null;
  }

  public saveComment(model: CommentModel, pin: string, commentModeType = 'PIN', requestId?: number) {
    let requestUrl = `${this.appService.apiServer}api/pin-comment/pin/${pin}/${model.id}`;
    if (commentModeType === 'EA') {
      requestUrl = `${this.appService.apiServer}api/ea/request/comment/${pin}/${requestId}`;
    }
    return this.http.post(requestUrl, model).pipe(
      map(res => this.extractComment(res)),
      catchError(err => {
        return this.handleError(err);
      })
    );
  }

  private handleError(error: any): Observable<any> {
    if (error instanceof HttpErrorResponse) {
      return throwError(error);
    } else {
      const errMsg = error.message ? error.message : error.status ? `${error.status} - ${error.statusText}` : 'Server error';
      return throwError(errMsg);
    }
  }

  public canEdit(): boolean {
    if (this.appService.coreAccessContext) {
      if (!this.appService.coreAccessContext.isStateStaff && this.appService.coreAccessContext.evaluate() === AccessType.edit) {
        return true;
      } else {
        return false;
      }
    } else {
      return false;
    }
  }
}
