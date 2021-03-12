import { Component, OnInit } from '@angular/core';
import { DestroyableComponent } from 'src/app/core/modal/index';
// import { Response } from '@angular/http';
// import { Participant } from '../shared/models/participant';
// import { ParticipantService } from '../shared/services/participant.service';
// import { Utilities } from '../shared/utilities';

// declare const $: any;

@Component({
  selector: 'app-tl-overview-help',
  templateUrl: './tl-overview.component.html'
})
export class TimeLimitOverviewHelpComponent implements OnInit, DestroyableComponent {
    public destroy = () => {

    };
    ngOnInit() {

    }
}