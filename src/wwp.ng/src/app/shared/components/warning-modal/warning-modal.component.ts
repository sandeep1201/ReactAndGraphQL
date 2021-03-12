import { Component, OnInit } from '@angular/core';
import { DestroyableComponent, ModalService } from 'src/app/core/modal/index';
import { AppService } from 'src/app/core/services/app.service';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-warning-modal',
  templateUrl: './warning-modal.component.html',
  styleUrls: ['./warning-modal.component.css']
})
export class WarningModalComponent implements OnInit, DestroyableComponent {
  public modalClosedSubject = new Subject<boolean>();
  public title = '';

  public warningMessage = '';
  public destroy: Function = () => {};
  public action: Function = () => {};
  constructor(private appService: AppService, private modalService: ModalService) {}

  ngOnInit() {
    // we can figure different types of cards here.
    this.showUnsavedChangesWarning();
  }

  disregardAndExit() {
    this.appService.isUrlChangeBlocked = false;
    this.modalService.allowAction = true;
    this.exit();
  }

  private showUnsavedChangesWarning() {
    this.modalService.allowAction = false;
    this.title = 'Unsaved Changes';
    this.warningMessage = 'There are unsaved changes. Leaving without saving will result in your changes being lost.';
  }

  exit() {
    this.modalClosedSubject.next(!this.appService.isUrlChangeBlocked);
    this.action();
    this.destroy(true);
  }
}
