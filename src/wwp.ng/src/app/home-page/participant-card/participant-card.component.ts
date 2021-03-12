import { Component, OnInit, Input, ChangeDetectionStrategy } from '@angular/core';
import { Participant } from '../../shared/models/participant';
@Component({
  selector: 'app-participant-card',
  templateUrl: './participant-card.component.html',
  styleUrls: ['./participant-card.component.css'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ParticipantCardComponent implements OnInit {
  public isCoenrolled = false;
  @Input() public displayDetailedView = false;
  @Input() participant: Participant;
  @Input() isGenderVisible = false;

  constructor() {}

  ngOnInit() {
    // if (this.participant.programs.length > 1) {
    //   this.isCoenrolled = true;
    // }
  }
}
