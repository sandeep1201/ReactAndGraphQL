import { ModalService } from './modal.service';
import { Component } from '@angular/core';

export { ModalService } from './modal.service';
//export { Modal } from './modal-placeholder/modal-placeholder.component';
//export { ModalContainer } from './modal-placeholder/modal-placeholder.component';
//export { ModalPlaceholderComponent } from './modal-placeholder/modal-placeholder.component';

export interface ICanBeDestroyed {
  destroy: Function;
}

export abstract class DestroyableComponent extends Component implements ICanBeDestroyed {
  abstract destroy: Function;
}
