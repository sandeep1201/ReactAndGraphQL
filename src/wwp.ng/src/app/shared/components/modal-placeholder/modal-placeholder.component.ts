import { ModalService } from './../../../core/modal/modal.service';
import { Component, OnInit, ViewContainerRef, ViewChild, Injector, ɵbypassSanitizationTrustStyle } from '@angular/core';

@Component({
  selector: 'app-modal-placeholder',
  template: '<div #modalplaceholder></div>',
  styleUrls: ['./modal-placeholder.component.scss']
})
export class ModalPlaceholderComponent implements OnInit {
  @ViewChild('modalplaceholder', { read: ViewContainerRef, static: true }) viewContainerRef;

  constructor(private modalService: ModalService, private injector: Injector) {}
  ngOnInit(): void {
    if (this.viewContainerRef) this.modalService.registerViewContainerRef(this.viewContainerRef);
    if (this.injector) this.modalService.registerInjector(this.injector);
  }
}

export class ModalContainer {
  destroy: Function;
  closeModal() {
    this.destroy();
  }
}
export function Modal() {
  return function(target) {
    Object.assign(target.prototype, ModalContainer.prototype);
  };
}
