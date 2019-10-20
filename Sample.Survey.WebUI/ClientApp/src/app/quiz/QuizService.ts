import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProProfQuiz } from './models/ProProfQuiz';
import { Environment } from '../../environments/environment';
import { map } from 'rxjs/operators';

@Injectable()
export class QuizService {

    constructor(private http: HttpClient) {
    }

    list(): Observable<ProProfQuiz[]> {
        return this.http.get(Environment.appApiBaseUrl + `/api/quiz`).pipe(map((response: HttpEvent<any>) => this._initialize(response)));
    }

    new(): Observable<ProProfQuiz> {
        // tslint:disable-next-line:max-line-length
        return this.http.get(Environment.appApiBaseUrl + `/api/quiz/New`).pipe(map((response: HttpEvent<any>) => this._initialize(response)));
    }

    Take(uniqueQuiz: string): Observable<ProProfQuiz> {
        // tslint:disable-next-line:max-line-length
        return this.http.get(Environment.appApiBaseUrl + `/api/quiz/take?uniqueQuiz=${uniqueQuiz}`).pipe(map((response: HttpEvent<any>) => this._initialize(response)));
    }

    open(id: string): Observable<ProProfQuiz> {
        // tslint:disable-next-line:max-line-length
        return this.http.get(Environment.appApiBaseUrl + `/api/quiz/${id}`).pipe(map((response: HttpEvent<any>) => this._initialize(response)));
    }

    save(model: ProProfQuiz): Observable<ProProfQuiz> {
        // tslint:disable-next-line:max-line-length
        return this.http.post(Environment.appApiBaseUrl + '/api/quiz', model).pipe(map((response: HttpEvent<any>) => this._initialize(response)));
    }

    _initialize(response: HttpEvent<any>): any {
        return response;
      }
}
