import { Injectable } from '@angular/core';
import { HttpClient, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ServiceDocument, UserProfile } from '@agility/frameworkcore';
import { LoginModel } from './LoginModel';
import { Environment } from '../../environments/environment';

@Injectable()
export class AccountService {
  serviceDocument: ServiceDocument<UserProfile> = new ServiceDocument<UserProfile>();
    constructor(private http: HttpClient) {
  }

  public GetUserProfile(): Observable<ServiceDocument<UserProfile>> {
    return this.serviceDocument.login(Environment.appApiBaseUrl + '/api/account/userprofile', null);
  }

  login(loginModel: LoginModel): Observable<UserProfile> {
    // tslint:disable-next-line:max-line-length
    return this.http.post(Environment.appApiBaseUrl + '/api/account/login', loginModel).pipe(map((response: HttpEvent<any>) => this._initialize(response)));
  }
  _initialize(response: HttpEvent<any>): any {
    return response;
  }

  getOrganizations(): any {
    return this.http.get(Environment.appApiBaseUrl + '/api/account/getOrganizations');
  }

  logout(): any {
    return this.serviceDocument.logout(Environment.appApiBaseUrl + '/api/account/logout');
  }
}
