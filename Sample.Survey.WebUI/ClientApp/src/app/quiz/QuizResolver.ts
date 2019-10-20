import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { QuizService } from './QuizService';
import { ProProfQuiz } from './models/ProProfQuiz';

@Injectable()
export class QuizListResolver implements Resolve<ProProfQuiz[]> {
    constructor(private service: QuizService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<ProProfQuiz[]> {
        return this.service.list();
    }
}

@Injectable()
export class QuizNewResolver implements Resolve<ProProfQuiz> {
    constructor(private service: QuizService) { }

    resolve(): Observable<ProProfQuiz> {
        return this.service.new();
    }
}

@Injectable()
export class QuizTakeResolver implements Resolve<ProProfQuiz> {
    constructor(private service: QuizService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<ProProfQuiz> {
        return this.service.Take(route.params['id']);
    }
}

@Injectable()
export class QuizOpenResolver implements Resolve<ProProfQuiz> {
    constructor(private service: QuizService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<ProProfQuiz> {
        return this.service.open(route.params['id']);
    }
}
