import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { trigger, state, style, transition, animate } from '@angular/animations';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.css'],
  animations: [
    trigger('flyInOut', [
      state('in', style({ transform: '-translateY(0)' })),
      transition('void => *', [
        style({ transform: 'translateY(-100%)' }),
        animate('0.5s ease-in-out')
      ]),
      transition('* => void', [
        animate('0.5s ease-in-out', style({ transform: 'translateY(100%)' }))
      ])
    ])
  ]
})
export class ConfirmDialogComponent implements OnInit {

  @Input() title: string = 'Please Confirm';
  @Input() message: string = 'Are you sure want to continue?';
  @Input() confirmButtonText: string = 'Yes';
  @Input() cancelButtonText: string = 'No';
  @Input() warningMsg: string = null;
  @Input() showWarning: boolean = false;

  @Output() confirm = new EventEmitter();
  @Output() cancel = new EventEmitter();

  public animateModalLeave: boolean = false;

  constructor() { }

  ngOnInit() {
  }

  onCancelClick() {
    this.animateModalLeave = true;
    this.cancel.emit();
  }

  onConfirmClick() {
    this.animateModalLeave = true;
    this.confirm.emit();
  }

}
