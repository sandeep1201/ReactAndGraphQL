import { ElementRef, HostListener, Directive, OnInit, Input, AfterViewInit, OnChanges, SimpleChanges, NgZone, Renderer } from '@angular/core';
import { Subject } from 'rxjs';
import { Router } from '@angular/router';
// https://github.com/stevepapa/angular2-autosize

@Directive({
  selector: 'textarea[AutoResize]'
})
export class AutoResizeDirective implements OnInit, AfterViewInit, OnChanges {
  @Input() shrinkOnBlur: boolean = true;

  @HostListener('input', ['$event.target']) onInput(textArea: HTMLTextAreaElement): void {
    this.adjust();
  }

  constructor(public element: ElementRef, private ngZone: NgZone, private render: Renderer, private router: Router) {}

  ngOnInit(): void {
    this.adjust();
  }

  adjust(): void {
    this.ngZone.runOutsideAngular(() => {
      this.element.nativeElement.style.overflow = 'hidden';
      if (this.router.url.indexOf('edit/non-custodial-parents') > -1) this.element.nativeElement.style.height = '35.74px';
      else this.element.nativeElement.style.height = '2.4em';
      this.element.nativeElement.style.lineHeight = '1.25em';
      this.element.nativeElement.style.height = this.element.nativeElement.scrollHeight + 'px';
    });
  }

  @HostListener('blur', ['$event.target'])
  onBlur() {
    if (this.shrinkOnBlur) {
      if (this.router.url.indexOf('edit/non-custodial-parents') > -1) this.element.nativeElement.style.height = '35.74px';
      else this.element.nativeElement.style.height = '2.4em';
    }
  }

  @HostListener('focus', ['$event.target'])
  onFocus() {
    this.adjust();
  }

  ngOnChanges(changes: SimpleChanges) {
    this.adjust();
  }

  ngAfterViewInit() {
    this.adjust();
  }
}
