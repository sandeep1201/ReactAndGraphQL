import { Directive, Output, HostListener, EventEmitter, ElementRef, Renderer } from '@angular/core';

@Directive({
  selector: '[appScroll]'
})
export class ScrollDirective {
  @Output() scroll = new EventEmitter();
  @Output() resize = new EventEmitter();

  constructor(private el: ElementRef, private renderer: Renderer) {}
}
