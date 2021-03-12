import { Component, OnInit } from '@angular/core';
import { DestroyableComponent } from 'src/app/core/modal/index';

// import { Response } from '@angular/http';
// import { Participant } from '../shared/models/participant';
// import { ParticipantService } from '../shared/services/participant.service';
// import { Utilities } from '../shared/utilities';

// declare const $: any;

@Component({
  selector: 'app-tl-ext-decisions-help',
  templateUrl: './tl-ext-decisions.component.html'
})
export class TimeLimitExtensionsHelpComponent implements OnInit, DestroyableComponent {
    ngOnInit() {

    }
    public destroy = () => {

    };
}