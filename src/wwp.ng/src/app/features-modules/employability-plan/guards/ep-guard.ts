import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router, NavigationEnd } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable()
export class EPGuard implements CanActivate {
  private previousUrl: string = undefined;
  private currentUrl: string = undefined;
  public loadRoute = true;
  constructor(private router: Router) {}
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {
    let pin = route.parent.params['pin'];
    if (pin === undefined) {
      pin = route.params['pin'];
    }
    const id = route.params['id'];
    this.currentUrl = this.router.url;

    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.previousUrl = this.currentUrl;
        this.currentUrl = event.url;
        if (this.previousUrl === '/') {
          this.router.navigateByUrl(`/pin/${pin}/employability-plan/list`);
          this.loadRoute = false;
        } else {
          this.loadRoute = true;
        }
      }
    });
    return this.loadRoute;
  }
}
