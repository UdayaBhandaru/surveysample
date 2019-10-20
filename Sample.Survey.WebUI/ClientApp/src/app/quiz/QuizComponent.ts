import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ServiceDocument } from '@agility/frameworkcore/servicedocument';
import { ProProfQuiz } from './models/ProProfQuiz';
import { QuizService } from './QuizService';
import { Validators } from '@angular/forms/src/validators';
import { FormGroup } from '@angular/forms';
import { map } from 'rxjs/operators';
import { HttpEvent } from '@angular/common/http';
import { CommonService } from '@agility/frameworkcore';

@Component({
    selector: 'app-quiz',
    templateUrl: './QuizComponent.html'
})
export class QuizComponent implements OnInit {
    quizForm: FormGroup;
    proProfQuiz: ProProfQuiz = new ProProfQuiz();
    _showButton = false;

    constructor(private route: ActivatedRoute,
        private router: Router,
        public service: QuizService,
        private commonService: CommonService) { }

    ngOnInit(): void {
        this.route.data.pipe(map((response: HttpEvent<any>) => this._initialize(response))).subscribe((res) => {
            this.proProfQuiz = res['Quiz'];
            this.quizForm = this.commonService.getFormGroup(this.proProfQuiz, 0);
          });
    }
    _initialize(response: HttpEvent<any>): any {
        return response;
    }

    save() {
        this.service.save(this.quizForm.value).subscribe(() => {
            this.router.navigate(['/Quiz/List']);
        });
    }

    cancel() {
        this.router.navigate(['/Quiz/List']);
        return false;
    }
}
