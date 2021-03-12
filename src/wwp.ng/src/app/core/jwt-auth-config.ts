import { JwtModule } from '@auth0/angular-jwt';
import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';

export class JwtAuthConfig {
  public noJwtError = false;
  public noTokenScheme = false;
  public expirationOffset = 0;
  public globalHeaders = [{ 'Content-Type': 'application/json' }];
  public tokenGetter(tokenName) {
    return localStorage.getItem(tokenName);
  }
  public tokenSetter(tokenName, tokenValue) {
    localStorage.setItem(tokenName, tokenValue);
  }

  // constructor(
  //   tokenName: 'jt',
  //   public tokenGetter: () => string = () => localStorage.getItem(tokenName),
  //   noJwtError: boolean = false, // try the request anyway, let the server figure out what to do
  //   globalHeaders: Array<Object> = [{ 'Content-Type': 'application/json' }],
  //   noTokenScheme: boolean = false,
  //   public expirationOffset: number = 0,
  //   public tokenSetter: (data: any) => void = data => localStorage.setItem(tokenName, data)
  // ) {}
}

// export function authHttpFactory(http: Http, options: RequestOptions, config: IAuthConfig) {
//   return new AuthHttp(new AuthConfig(config), http, options);
// }

// export function jwtAuthConfigFactory() {
//   return new JwtAuthConfig();
// }

// export function ProvideJwtAuth() {
//   return [
//     {
//       provide: AuthHttp,
//       deps: [Http, RequestOptions, JwtAuthConfig],
//       useFactory: authHttpFactory
//     },
//     { provide: JwtAuthConfig, useFactory: jwtAuthConfigFactory }
//   ];
// }
