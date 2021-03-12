import { Component, OnInit } from '@angular/core';
import { InputHistory } from '../../models/input-history';

@Component({
  selector: 'app-input-history',
  templateUrl: './input-history.component.html',
  styleUrls: ['./input-history.component.css']
})
export class InputHistoryComponent implements OnInit {
  get history(): InputHistory[] {
    return this._history;
  }

  set history(histories: InputHistory[]) {
    this._history = histories;
    this.setDeleteReasonsCol();
  }
  public prettyPropName: string;
  public sectionName: string;
  public dataConversion = 'default';
  public hasDeleteReasons = false;
  private _history: InputHistory[] = [];
  public destroy: Function = () => {};
  public closeDialog: Function = () => {};

  constructor() {}

  ngOnInit() {}

  private setDeleteReasonsCol() {
    if (this.history != null) {
      for (const hist of this.history) {
        if (hist.deleteReason != null) {
          this.hasDeleteReasons = true;
          break;
        }
      }
    }
  }

  exit() {
    this.closeDialog();
    this.destroy();
  }
}
