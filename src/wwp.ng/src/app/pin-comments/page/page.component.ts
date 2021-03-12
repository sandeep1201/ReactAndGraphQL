import { Component, OnInit } from '@angular/core';
import { Participant } from '../../shared/models/participant';
import { ActivatedRoute } from '@angular/router';
import { forkJoin } from 'rxjs';
import { ParticipantService } from '../../shared/services/participant.service';
import { take } from 'rxjs/operators';
import { CommentsService } from 'src/app/shared/components/comment/comments.service';

@Component({
  selector: 'app-pin-comments-page',
  templateUrl: './page.component.html',
  styleUrls: ['./page.component.scss']
})
export class PinCommentsPageComponent implements OnInit {
  public goBackUrl: string;
  public isReadOnly: boolean;
  public isInEditMode: boolean;
  public canEdit: boolean;
  public canView: boolean;
  public pin: string;
  public pinComments: any[];
  public isLoaded = false;
  public participant: Participant;

  constructor(private route: ActivatedRoute, private partService: ParticipantService, private commentService: CommentsService) {}

  ngOnInit() {
    forkJoin(this.route.params.pipe(take(1)), this.commentService.modeForComment.pipe(take(1)), this.partService.getCurrentParticipant().pipe(take(1))).subscribe(result => {
      this.pin = result[0].pin;
      this.goBackUrl = '/pin/' + this.pin;
      this.isReadOnly = result[1].readOnly;
      this.isInEditMode = result[1].isInEditMode;
      this.participant = result[2];
      this.isLoaded = true;
    });
  }
}
