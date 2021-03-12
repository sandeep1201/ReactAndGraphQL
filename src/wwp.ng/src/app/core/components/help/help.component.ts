import { DestroyableComponent } from 'src/app/core/modal/index';
import { Component, OnInit } from '@angular/core';
@Component({
  selector: 'app-help',
  templateUrl: './help.component.html',
  styleUrls: ['./help.component.css']
})
export class HelpComponent implements OnInit, DestroyableComponent {
  public destroy: Function = () => {};
  public closeDialog: Function = () => {};
  constructor() {}

  ngOnInit() {}

  exit() {
    this.closeDialog();
    this.destroy();
  }
}
