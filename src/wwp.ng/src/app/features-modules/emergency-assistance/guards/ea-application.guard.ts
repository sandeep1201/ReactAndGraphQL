import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';
import { of } from 'rxjs';

@Injectable()
export class EmergencyAssistanceApplicationGuard implements CanActivate {
  constructor(private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot) {
    const pin = route.paramMap.get('pin');
    const id = route.paramMap.get('id');
    let canActivate = of(false);
    if (pin && id) {
      if (this.router.url === '/' && !this.router.getCurrentNavigation().extras.state) {
        if (+id > 0) {
          this.router.navigate([`/pin/${pin}/emergency-assistance/ea-application-history/${id}`], { state: { id: id } });
        } else {
          this.router.navigate([`/pin/${pin}/emergency-assistance/ea-application-history`]);
        }
      } else {
        canActivate = of(true);
      }
    }
    return canActivate;
  }
}
