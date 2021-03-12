import { Injectable } from '@angular/core';
import { CanActivate, Router, NavigationEnd } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable()
export class OrgInfoGuard implements CanActivate {
  private previousUrl: string = undefined;
  private currentUrl: string = undefined;
  public loadRoute = true;
  constructor(private router: Router) {}
  canActivate(): Observable<boolean> | Promise<boolean> | boolean {
    this.currentUrl = this.router.url;
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.previousUrl = this.currentUrl;
        this.currentUrl = event.url;
        if (this.previousUrl === '/') {
          this.router.navigateByUrl(`/home`);
          this.loadRoute = false;
        } else {
          this.loadRoute = true;
        }
      }
    });
    return this.loadRoute;
  }
}
