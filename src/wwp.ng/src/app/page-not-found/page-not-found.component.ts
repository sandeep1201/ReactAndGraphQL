import { Component, OnInit } from '@angular/core';

import { Participant } from '../shared/models/participant';
import { ParticipantService } from '../shared/services/participant.service';
import { Utilities } from '../shared/utilities';

@Component({
  selector: 'app-page-not-found',
  templateUrl: './page-not-found.component.html',
  styleUrls: ['./page-not-found.component.css']
})
export class PageNotFoundComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
