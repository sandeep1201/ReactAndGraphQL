import { of } from 'rxjs';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Participant } from 'src/app/shared/models/participant';
import { ParticipantService } from 'src/app/shared/services/participant.service';
import { take, concatMap } from 'rxjs/operators';
import { W2Plan } from '../models/w-2-plan.model';

@Component({
  selector: 'app-w-2-plan-overview',
  templateUrl: './w-2-plan-overview.component.html',
  styleUrls: ['./w-2-plan-overview.component.scss']
})
export class W2PlanOverviewComponent implements OnInit {
  public isLoaded = false;
  public goBackUrl: string;
  private pin: string;
  public participant: Participant;
  public model: W2Plan;

  constructor(private route: ActivatedRoute, private partService: ParticipantService) {}

  ngOnInit() {
    this.pin = this.route.snapshot.params.pin;
    this.goBackUrl = '/pin/' + this.pin;

    this.partService
      .getCachedParticipant(this.pin)
      .pipe(
        concatMap(participant => {
          this.participant = participant;
          return of(null);
        })
      )
      .pipe(take(1))
      .subscribe(results => {
        this.isLoaded = true;
      });
  }
}
