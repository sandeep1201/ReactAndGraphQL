import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-ep-events',
  templateUrl: './ep-events.component.html',
  styleUrls: ['./ep-events.component.scss']
})
export class EpEventsComponent implements OnInit {
  @Input() day: any;

  constructor() {}

  ngOnInit() {}
}
