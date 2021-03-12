import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { environment } from '../../environments/environment';
import { AppService } from 'src/app/core/services/app.service';

@Component({
  selector: 'app-unauthorized',
  templateUrl: './unauthorized.component.html',
  styleUrls: ['./unauthorized.component.css']
})
export class UnauthorizedComponent implements OnInit {
  public url: string;
  public guard: string;
  public programs: string;
  public authorizations: string;
  public context: string;
  public showUnauthorizedSecurityDetails = environment.showUnauthorizedSecurityDetails;

  constructor(private route: ActivatedRoute, private appService: AppService) {
    this.route.queryParams.subscribe(params => {
      this.url = params['url'];
      this.guard = params['guard'];
    });
  }

  ngOnInit() {
    if (this.appService && this.appService.coreAccessContext) {
      this.programs = JSON.stringify(this.appService.coreAccessContext.programs);
      this.context = this.appService.coreAccessContext.toString();
    } else {
      this.programs = null;
      this.context = null;
    }

    if (this.appService && this.appService.user) this.authorizations = JSON.stringify(this.appService.user.authorizations);
    else this.authorizations = null;
  }
}
