import { Component, OnInit } from '@angular/core';
import { ModalBase } from 'src/app/core/modal/modal-base';

@Component({
  selector: 'app-error-modal',
  templateUrl: './error-modal.component.html',
  styleUrls: ['./error-modal.component.css']
})
export class ErrorModalComponent extends ModalBase implements OnInit {
  constructor() {
    super();
  }

  ngOnInit() {}
}
