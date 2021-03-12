import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';

import { AppService } from 'src/app/core/services/app.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-start',
  templateUrl: './start.component.html',
  styleUrls: ['./start.component.css']
})
export class StartComponent implements OnInit, OnDestroy {
  authSub: Subscription;
  constructor(private router: Router, private appService: AppService) {}
  ngOnInit() {
    this.authSub = this.appService.isUserAuthenticated().subscribe(valid => {
      if (valid) {
        this.router.navigateByUrl('home');
      } else {
        this.router.navigateByUrl('login');
      }
    });
  }

  ngOnDestroy() {
    if (this.authSub) {
      this.authSub.unsubscribe();
    }
  }
}
