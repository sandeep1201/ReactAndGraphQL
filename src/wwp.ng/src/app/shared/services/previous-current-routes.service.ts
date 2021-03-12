import { Injectable } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
import { Observable } from 'rxjs/';
import { of } from 'rxjs';

@Injectable()
export class PreviousCurrentRoutesService {
  public currentUrl: string;
  public previousUrl: string;

  constructor(private router: Router) {}

  getCurrentAndPreviousRoutes(): Observable<any> {
    this.currentUrl = this.router.url;

    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.previousUrl = this.currentUrl;
        this.currentUrl = event.url;
      }
    });
    return of({ currentUrl: this.currentUrl, previousUrl: this.previousUrl });
  }
}
