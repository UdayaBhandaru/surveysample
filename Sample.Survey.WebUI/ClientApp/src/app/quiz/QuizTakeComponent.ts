import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import {BrowserModule, DomSanitizer} from '@angular/platform-browser';
import { ServiceDocument } from '@agility/frameworkcore/servicedocument';
import { ProProfQuiz } from './models/ProProfQuiz';
import { QuizService } from './QuizService';
import { Validators } from '@angular/forms/src/validators';
import { FormGroup } from '@angular/forms';
import { map } from 'rxjs/operators';
import { HttpEvent } from '@angular/common/http';
import { CommonService } from '@agility/frameworkcore';

@Component({
    selector: 'app-quiz-take',
    templateUrl: './QuizTakeComponent.html'
})
export class QuizTakeComponent implements OnInit {
    public iframeUrl: any;
    public proProfQuiz: ProProfQuiz = new ProProfQuiz();
    _showButton = false;

    constructor(private route: ActivatedRoute,
        private router: Router,
        public service: QuizService,
        private commonService: CommonService,
        private sanitizer: DomSanitizer) { }

    ngOnInit(): void {
        this.route.data.pipe(map((response: HttpEvent<any>) => this._initialize(response))).subscribe((res) => {
            this.proProfQuiz = res['Quiz'];
            this.iframeUrl = this.sanitizer.bypassSecurityTrustResourceUrl(this.proProfQuiz.quizLink);
          });
    }
    _initialize(response: HttpEvent<any>): any {
        return response;
    }
}
