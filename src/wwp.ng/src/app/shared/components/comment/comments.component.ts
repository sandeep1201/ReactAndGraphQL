// tslint:disable: import-blacklist
import { Component } from '@angular/core';
import { Router, ActivationEnd } from '@angular/router';
import { AppService } from 'src/app/core/services/app.service';
import { forkJoin, of } from 'rxjs';
import * as moment from 'moment';
import { take, concatMap } from 'rxjs/operators';
import { PinCommentsService } from '../../services/pin-comments.service';
import { CommentsService } from './comments.service';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.scss']
})
export class CommentsComponent {
  public participantPin: string;
  public isPinBased = false;
  public isInEditMode = false;
  public hideComments = false;
  public commentId: number;
  public showCommentsFeature = false;
  public commentType = 'PIN';
  showComments = false;

  constructor(private router: Router, public appService: AppService, public commentsService: CommentsService, private pinCommentService: PinCommentsService) {
    this.router.events.subscribe(val => {
      if (val instanceof ActivationEnd) {
        // Everytime the router changes, lets clear pin.
        this.participantPin = '';
        this.commentType = 'PIN';
        this.commentsService.participantPin = this.participantPin;
        this.pinCommentService.participantPin = this.participantPin;
        this.isPinBased = false;
        this.participantPin = val.snapshot.params['pin'];
        if (this.participantPin != null) {
          this.commentsService.participantPin = this.participantPin;
          this.pinCommentService.participantPin = this.participantPin;
          this.isPinBased = true;
        }

        if (val.snapshot.routeConfig.path === 'pin/:pin/pin-comments' || val.snapshot.routeConfig.path === 'pin/:pin/rfa' || val.snapshot.routeConfig.path === 'pin/:pin/rfa/:id') {
          this.hideComments = true;
        } else if (val.snapshot.routeConfig.path === 'pin/:pin/emergency-assistance') {
          this.commentType = 'EA';
          if (val.snapshot['_routerState'].url.indexOf('edit') < 0 || !(val.snapshot['_routerState'].url.indexOf('/0/edit') < 0)) this.hideComments = true;
        } else {
          this.hideComments = false;
        }
      }

      this.appService.isCommentsShown = this.isPinBased && !this.hideComments && this.commentsService.canEdit();

      if (this.participantPin && this.participantPin.trim() !== '' && !this.hideComments) this.onInit();
    });
  }

  onInit() {
    const feature = 'PinComments';
    forkJoin(this.commentsService.modeForComment.pipe(take(1)), this.appService.featureToggles.pipe(take(1)))
      .pipe(
        take(1),
        concatMap(results => {
          this.commentId = results[0].id;
          this.isInEditMode = results[0].isInEditMode;

          if (results[1] && results[1].length > 0) {
            this.showCommentsFeature = this.appService.getFeatureToggleDate(feature);
            return of(null);
          } else return this.appService.getFeatureToggleValues();
        })
      )
      .subscribe(res => {
        if (res) {
          const featureToggleDate = res.filter(i => i.parameterName === feature)[0].parameterValue;
          this.showCommentsFeature = moment().isSameOrAfter(moment(featureToggleDate));
          this.appService.featureToggles.next(res);
        }
      });
  }

  onAdd() {
    this.isInEditMode = true;
    this.commentId = 0;
    this.commentsService.modeForComment.next({ id: 0, readOnly: false, isInEditMode: this.isInEditMode, commentType: this.commentType });
  }

  closeEdit(e: boolean) {
    this.isInEditMode = e;
  }
}
