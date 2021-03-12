import { Component, OnInit, OnDestroy, ViewChild, AfterViewInit, Renderer, ElementRef } from '@angular/core';
import { Subject, Subscription } from 'rxjs';
import { distinctUntilChanged, debounceTime } from 'rxjs/operators';
import { Router } from '@angular/router';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.css', '../../shared/components/text-input/text-input.component.css']
})
export class SearchComponent implements OnInit, OnDestroy, AfterViewInit {
  isDisabled = false;
  isInvalid = false;
  isRequired = false;
  maxLength: number = null;
  inputModel: string;
  model: string;
  isSearching = false;
  searchMode: 'PIN' | 'CASE' | 'NAME' = 'PIN';
  inputEmitter = new Subject<string>();
  inputSubscription: Subscription;

  @ViewChild('searchInput', { static: true })
  searchInput: ElementRef;

  constructor(private renderer: Renderer, private router: Router) {}

  ngOnInit() {
    this.inputSubscription = this.inputEmitter
      .pipe(
        debounceTime(500),
        distinctUntilChanged()
      )
      .subscribe(x => this.model = x);
  }

  onKeyup(event: KeyboardEvent) {
    this.inputEmitter.next(this.inputModel);
  }

  ngAfterViewInit(): void {
    // try to focus the textbox
    if (this.searchInput) {
      this.renderer.invokeElementMethod(this.searchInput.nativeElement, 'focus', []);
    }
  }

  ngOnDestroy() {
    if (this.inputSubscription != null) {
      this.inputSubscription.unsubscribe();
    }
  }

  search() {
    // TODO:
    this.router.navigateByUrl('/home');
  }
}
