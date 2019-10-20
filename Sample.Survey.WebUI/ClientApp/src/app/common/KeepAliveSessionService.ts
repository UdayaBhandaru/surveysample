import { Idle, DEFAULT_INTERRUPTSOURCES } from '@ng-idle/core';
import { Keepalive } from '@ng-idle/keepalive';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { ServiceDocument, UserProfile, FxContext, MessageResult } from '@agility/frameworkcore';
import { HttpClient, HttpEvent, HttpHeaders } from '@angular/common/http';
import { FxSamplesDialogService } from '../common/FxSamplesDialogService';
import { FxSamplesButton } from '../common/FxSamplesControlModels';
import { Environment } from '../../environments/environment';
@Injectable()
export class KeepAliveSessionService {
  private messageResult: MessageResult = new MessageResult();
  // tslint:disable-next-line:max-line-length
  constructor(private http: HttpClient, private router: Router, private keepalive: Keepalive, private idle: Idle, private _dialogService: FxSamplesDialogService, private fxContext: FxContext) {
  }

  private getData(): Observable<any> {
    const headers = new HttpHeaders();
    headers.set('Content-Type', 'text/plain; charset=utf-8');
    const resp = this.http.get(Environment.appApiBaseUrl + '/api/account/UserProfile', { responseType: 'text' });
    return resp;
  }

  public setKeepAliveSessionConfiguration(): void {
    const timeInterval = 1000;
    this.keepalive.onPing.subscribe(() => {
      this.getData().subscribe(data => {
        this.fxContext.userProfile.userAccessToken = JSON.parse(data).userAccessToken;
      });
    });
    this.keepalive.interval(timeInterval);

  }

  start(): void {
    this.keepalive.start();
  }

  stop(): void {
    this.keepalive.stop();
  }



  clearTimeout(timeOutHandle: any): void {
    window.clearTimeout(timeOutHandle);
  }

  yesClickEvent(timeOutHandle: any): void {
    this.clearTimeout(timeOutHandle);
    this.start();
  }

  noClickEvent(timeOutHandle: any): void {
    this.clearTimeout(timeOutHandle);
    this.stop();
    this.router.navigate(['/Account']);
  }
}
