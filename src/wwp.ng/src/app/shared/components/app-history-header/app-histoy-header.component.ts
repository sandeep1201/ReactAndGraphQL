// tslint:disable: no-output-on-prefix
import { Subscription } from 'rxjs';
import { Component, OnInit, Input, Output, EventEmitter, OnDestroy } from '@angular/core';

import { AppService } from 'src/app/core/services/app.service';
import { DropDownField } from '../../models/dropdown-field';

@Component({
  selector: 'app-app-history-header',
  templateUrl: './app-history-header.component.html',
  styleUrls: ['./app-history-header.component.css']
})
export class AppHistoryHeaderComponent implements OnInit, OnDestroy {
  @Output()
  onHistory = new EventEmitter<boolean>();
  @Output()
  onHistorySelected = new EventEmitter<Number>();
  @Output()
  onCollapse = new EventEmitter<boolean>();
  @Output()
  onEdit = new EventEmitter<boolean>();
  @Input()
  rowVersion;
  @Input()
  title = '';

  @Input()
  isEditHidden = false;
  @Input() showHistoryIcon = true;
  @Input()
  historyDrop: DropDownField[];
  @Input()
  historyArrayLength;

  public hasPBAccessBol: boolean;
  public canRequestPBAccess: boolean;
  public requestedElevatedAccess: boolean;

  historyIndex = 0;
  isHistoryActive = false;
  isCollapsed = false;
  private pbSub: Subscription;

  public get isHistoryLoading(): boolean {
    return this.isHistoryActive && this.historyDrop == null;
  }

  constructor(private appService: AppService) {}

  ngOnInit() {
    this.pbSub = this.appService.PBSection.subscribe(res => {
      this.hasPBAccessBol = res.hasPBAccessBol;
      this.canRequestPBAccess = res.canRequestPBAccess;
      this.requestedElevatedAccess = res.requestedElevatedAccess;
    });
  }

  toggleHistory() {
    this.isHistoryActive = !this.isHistoryActive;

    if (this.isHistoryActive === false) {
      // If we are disabling history, reset the dropdowns
      this.historyDrop = null;
      this.historyIndex = 0;
    }

    this.onHistory.emit(this.isHistoryActive);
  }

  forward() {
    if (this.historyIndex > 0) {
      this.historyIndex--;
    } else {
      this.historyIndex = this.historyArrayLength - 1;
    }

    this.onHistorySelected.emit(this.historyIndex);
  }

  back() {
    if (this.historyIndex < this.historyArrayLength - 1) {
      this.historyIndex++;
    } else {
      this.historyIndex = 0;
    }
    this.onHistorySelected.emit(this.historyIndex);
  }

  loadHistorySection() {
    if (this.historyIndex != null) {
      this.onHistorySelected.emit(this.historyIndex);
    } else {
      this.historyIndex = 0;
      this.onHistorySelected.emit(this.historyIndex);
    }
  }

  ngOnDestroy() {
    if (this.pbSub != null) {
      this.pbSub.unsubscribe();
    }
  }
}
