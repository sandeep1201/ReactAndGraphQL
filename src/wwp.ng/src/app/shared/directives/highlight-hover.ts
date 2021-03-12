import { Directive, ElementRef, HostListener, Input } from '@angular/core';

@Directive({
    selector: '[appHighlightHover]',

})
export class HighlightHoverDirective {
    private _defaultColor = '';
    private el: HTMLElement;

    constructor(el: ElementRef) { this.el = el.nativeElement; }

    @Input('highlightColor') highlightColor: string;

    @Input() isHoverHighlightDisabled: boolean;

    @HostListener('mouseenter') onMouseEnter() {
        if (!this.isHoverHighlightDisabled) {
            this.highlight(this.highlightColor || this._defaultColor);
        }
    }

    @HostListener('mouseleave') onMouseLeave() {
        this.highlight('');
    }

    private highlight(color: string) {
        this.el.style.backgroundColor = color;
    }

}