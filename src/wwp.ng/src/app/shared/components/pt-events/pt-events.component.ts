import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-pt-events',
  templateUrl: './pt-events.component.html',
  styleUrls: ['./pt-events.component.scss']
})
export class PtEventsComponent implements OnInit {
  @Input() day: any;

  @Input() canEdit: boolean;
  @Output() clickedActivity = new EventEmitter<any>();
  @Output() buttonClick = new EventEmitter<any>();
  constructor() {}

  ngOnInit() {}
  clicked(e) {
    this.clickedActivity.emit(e);
  }
  buttonClicked(e, d) {
    this.buttonClick.emit({ innerText: e.target.innerText, details: d });
  }
}
