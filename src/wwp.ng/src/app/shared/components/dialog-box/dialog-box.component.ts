import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { AppService } from 'src/app/core/services/app.service';
import { trigger, state, style, transition, animate } from '@angular/animations';

@Component({
  selector: 'app-dialog-box',
  templateUrl: './dialog-box.component.html',
  styleUrls: ['./dialog-box.component.css'],
  animations: [
    trigger('flyInOut', [
      state('in', style({ transform: '-translateY(0)' })),
      transition('void => *', [style({ transform: 'translateY(-100%)' }), animate('0.5s ease-in-out')]),
      transition('* => void', [animate('0.5s ease-in-out', style({ transform: 'translateY(100%)' }))])
    ])
  ]
})
export class DialogBoxComponent implements OnInit {
  @Input() doesNotNavigateBack: boolean;
  @Output() dialogClose = new EventEmitter();
  @Output() cancel = new EventEmitter();

  public animateModalLeave: boolean = false;

  constructor(private appService: AppService) {}

  ngOnInit() {}

  navigateBack() {
    this.dialogClose.emit();
    if (this.doesNotNavigateBack === true) {
      this.appService.hideModifiedWarningDialog();
    } else {
      this.appService.exitModifiedWarningDialog();
    }
  }

  closeDialogBox() {
    this.cancel.emit();
    this.animateModalLeave = true;
    this.appService.hideModifiedWarningDialog();
  }
}
