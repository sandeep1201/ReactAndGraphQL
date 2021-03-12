import { HelpComponent } from './components/help/help.component';
import { ModalService } from 'src/app/core/modal/modal.service';
import { JwtAuthConfig } from 'src/app/core/jwt-auth-config';
import { NgModule, Optional, SkipSelf } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';
import { AuthHttpInterceptor } from './interceptors/AuthHttpInterceptor';
import { AppService } from './services/app.service';
import { AuthHttpClient } from './interceptors/AuthHttpClient';

@NgModule({
  declarations: [HelpComponent],
  imports: [CommonModule, HttpClientModule],
  entryComponents: [HelpComponent],
  providers: [
    AppService,
    ModalService,
    JwtAuthConfig,
    { provide: HTTP_INTERCEPTORS, useClass: AuthHttpInterceptor, multi: true },
    {
      provide: AuthHttpClient,
      useFactory: authHttpClientCreator,
      deps: [HttpClient, JwtAuthConfig, ModalService]
    }
  ]
})
export class CoreModule {
  constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    if (parentModule) {
      throw new Error('Core is already loaded. Import it in the AppModule only');
    }
  }
}

export function authHttpClientCreator(http: HttpClient, jwtAuthConfig: JwtAuthConfig, modalService: ModalService) {
  return new AuthHttpClient(http, jwtAuthConfig, modalService);
}
