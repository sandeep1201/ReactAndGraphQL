import { Component, OnInit, Input } from '@angular/core';
import { CwwChild, CwwEligibility } from '../../../shared/models/child-youth-supports-section';

@Component({
  selector: 'app-cww-children',
  templateUrl: './cww-children.component.html',
  styleUrls: ['./cww-children.component.css']
})
export class CwwChildrenComponent implements OnInit {
  @Input() cwwChildren: CwwChild[];

  @Input() cwwEligibility: CwwEligibility;

  constructor() {}

  ngOnInit() {}
}
