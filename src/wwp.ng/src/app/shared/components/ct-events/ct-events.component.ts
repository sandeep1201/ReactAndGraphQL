import { Component, OnInit, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-ct-events',
  templateUrl: './ct-events.component.html',
  styleUrls: ['./ct-events.component.scss']
})
export class CtEventsComponent implements OnInit {
  @Input() day: any;
  @Input() canEdit = true;
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
