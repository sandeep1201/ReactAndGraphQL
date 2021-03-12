import { Directive, OnInit, Renderer, ElementRef, Input } from '@angular/core';

@Directive({
  selector: 'textarea[focusOnInit]'
})
export class InputFocusDirective implements OnInit {
  @Input() focusOnInitEnabled: boolean = false;
  constructor(public renderer: Renderer, public elementRef: ElementRef) {}

  ngOnInit() {
    if (this.focusOnInitEnabled === true) {
      this.renderer.invokeElementMethod(
        this.elementRef.nativeElement, 'focus', []);
    }
  }
}


