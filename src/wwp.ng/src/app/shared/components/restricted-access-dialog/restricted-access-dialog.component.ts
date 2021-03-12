import { Component, OnInit } from '@angular/core';
import { DestroyableComponent } from 'src/app/core/modal/index';
import { Modal } from 'src/app/shared/components/modal-placeholder/modal-placeholder.component';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-restricted-access-dialog',
  templateUrl: './restricted-access-dialog.component.html',
  styleUrls: ['./restricted-access-dialog.component.css']
})
@Modal()
export class RestrictedAccessDialogComponent implements DestroyableComponent, OnInit {
  public actionSubmitted = new Subject<{ canAccess: boolean }>();
  public destroy: Function = () => {};
  public closeDialog: Function = () => {};

  constructor() {}

  ngOnInit() {}

  onCancel(): void {
    this.actionSubmitted.next({ canAccess: false });
    this.closeDialog();
    this.destroy();
  }
}
