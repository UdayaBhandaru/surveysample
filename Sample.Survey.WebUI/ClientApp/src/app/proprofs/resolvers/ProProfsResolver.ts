import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve } from '@angular/router';

import { Observable } from 'rxjs';
import { ProProfsService } from '../ProProfsService';
import { ProProfsUser } from '../models/ProProfsUser';
import { ProProfQuiz } from '../../quiz/models/ProProfQuiz';
import { QuizService } from '../../quiz/QuizService';

@Injectable()
export class ProProfsDashboardResolver implements Resolve<ProProfsUser> {
  constructor(private service: ProProfsService) { }

  public resolve(route: ActivatedRouteSnapshot): Observable<ProProfsUser> {
    return this.service.list();
  }
}

@Injectable()
export class ProProfNewResolver implements Resolve<ProProfQuiz[]> {
    constructor(private service: QuizService) { }

    resolve(route: ActivatedRouteSnapshot): Observable<ProProfQuiz[]> {
        return this.service.list();
    }
}
