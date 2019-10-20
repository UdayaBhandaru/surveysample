import { HttpClient, HttpParams, HttpEvent } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ProProfsUser } from './models/ProProfsUser';
import { Environment } from '../../environments/environment';

@Injectable()
export class ProProfsService {
  constructor(private http: HttpClient) {
  }

  public list(): Observable<ProProfsUser> {
    // tslint:disable-next-line:max-line-length
    return this.http.get(Environment.appApiBaseUrl + '/api/ProProfs/User').pipe(map((response: HttpEvent<any>) => this._initialize(response)));
  }

  public save(model: ProProfsUser): Observable<ProProfsUser> {
    // tslint:disable-next-line:max-line-length
    return this.http.post(Environment.appApiBaseUrl + '/api/ProProfs/UserQuiz', model).pipe(map((response: HttpEvent<any>) => this._initialize(response)));
  }

  _initialize(response: HttpEvent<any>): any {
    return response;
  }
}
