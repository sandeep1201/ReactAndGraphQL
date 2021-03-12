// tslint:disable: deprecation
import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { Router, ActivatedRoute, NavigationEnd } from '@angular/router';
import { environment } from '../environments/environment';
import { LogService } from './shared/services/log.service';
import { Startup } from './shared/models/events/startup';
import { version, SemanticVersion } from './shared/version';
import { take, timeInterval } from 'rxjs/operators';
import { interval, Subscription } from 'rxjs';
import { AppService } from './core/services/app.service';

declare var ga: any;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit, OnDestroy {
  private static previousUrl = '';
  private routerSub: Subscription;
  private timeUpdateSub: Subscription;
  public isLoaded = true;
  public loadingLabel = '';

  // http://stackoverflow.com/questions/37655898/tracking-google-analytics-page-views-in-angular2
  // private appService: AppService,
  constructor(private router: Router, private route: ActivatedRoute, public appService: AppService, private logService: LogService, private cdRef: ChangeDetectorRef) {
    // Set up a subscriptionfor route changes.
    this.routerSub = this.router.events.subscribe(e => {
      if (e instanceof NavigationEnd) {
        if (AppComponent.previousUrl !== '') {
          this.logService.timerEndUpdateEvent('route');
        }

        this.logService.timerStartEvent('route', { url: e.url });
        AppComponent.previousUrl = e.url;

        if (environment.production) {
          ga('send', 'pageview', e.url);
        }
      }
    });

    // Update the application time every 60 seconds
    this.timeUpdateSub = interval(1000 * 60)
      .pipe(timeInterval())
      .subscribe(x => {
        appService
          .getServerStatus()
          .pipe(take(1))
          .subscribe(
            status => {
              if (status.environment != null) {
                this.appService.currentEnvironment = status.environment;
                // localStorage.setItem('environment', status.environment.toString());
              }

              // First determine if we think we are out of date
              if (status.version && !status.version.equals(version)) {
                let reloadedToVersion: SemanticVersion = null;
                const versionString = localStorage.getItem('reloadedToVersion');
                reloadedToVersion = SemanticVersion.parse(versionString);
                if (reloadedToVersion && reloadedToVersion.equals(status.version)) {
                  logService.warn(`Already attempted application reload to version ${reloadedToVersion}. Version out of sync. Server:${version}. Client:${status.version}`);
                } else {
                  localStorage.setItem('reloadedToVersion', status.version.toString());
                  location.reload(true);
                }
              } else {
                localStorage.removeItem('reloadedToVersion');
              }
            },
            error => console.error(error)
          );
      });
    const s = new Startup();
    s.os = navigator.platform;

    s.sh = screen.height;
    s.sw = screen.width;

    const w = window.innerWidth || document.documentElement.clientWidth || document.body.clientWidth;

    const h = window.innerHeight || document.documentElement.clientHeight || document.body.clientHeight;

    s.wh = h;
    s.ww = w;

    s.ua = navigator.userAgent;
    s.host = window.location.host;
    s.ver = version.toString();

    this.logService.event('app-startup', s);
  }

  public notificationOptions = {
    position: ['bottom', 'left'],
    timeOut: 5000,
    lastOnBottom: true,
    preventDuplicates: true,
    maxStack: 3,
    pauseOnHover: true
  };
  ngOnInit() {
    this.appService.loadingComponentInput.subscribe(res => {
      this.isLoaded = res.isLoaded;
      this.loadingLabel = res.loadingLabel;
      if (res.isLoaded) {
        this.cdRef.detectChanges();
      }
    });
  }

  ngOnDestroy() {
    if (this.routerSub != null) {
      this.routerSub.unsubscribe();
    }
    if (this.timeUpdateSub != null) {
      this.timeUpdateSub.unsubscribe();
    }
  }
}
