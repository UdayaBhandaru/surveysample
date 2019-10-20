import { Component, OnInit, ViewChild, TemplateRef, Inject, LOCALE_ID } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DatePipe } from '@angular/common';
import { MatDialogRef } from '@angular/material';
import { ServiceDocument } from '@agility/frameworkcore';
import { QuizService } from './QuizService';
import { ProProfQuiz } from './models/ProProfQuiz';
import { FxSamplesButton, FxSamplesDialog } from '../common/FxSamplesControlModels';
import { FxSamplesDialogService } from '../common/FxSamplesDialogService';
import { CommonService } from '@agility/frameworkcore';
import { map } from 'rxjs/operators';
import { HttpEvent } from '@angular/common/http';


@Component({
    selector: 'app-quiz-list',
    templateUrl: './QuizListComponent.html'
})
export class QuizListComponent implements OnInit {
    dialogRef: MatDialogRef<any>;
    columns: any[];
    @ViewChild('editButton') public editTmpl: TemplateRef<QuizListComponent>;

  public quizmodel: ProProfQuiz;
  fxDialog: FxSamplesDialog = new FxSamplesDialog();
  public quizzes: ProProfQuiz[];
    constructor(private service: QuizService, private route: ActivatedRoute, private dialogService: FxSamplesDialogService,
        @Inject(LOCALE_ID) private _locale: string,
        private commonService: CommonService) {
    }

    ngOnInit(): void {
        this.route.data.pipe(map((response: HttpEvent<any>) => this._initialize(response))).subscribe((res) => {
            this.quizzes = res['Quizzes'];
          });
        this.columns = [
            { name: 'Quiz Id', prop: 'id' },
            { name: 'Quiz Name', prop: 'quizName' },
            { name: 'Quiz Unique Name', prop: 'quizUniqueName' },
            {
                name: 'Actions', prop: 'Actions', cellTemplate: this.editTmpl
            }
        ];
    }

    open(id: string): void {
        this.service.open(id).subscribe();
    }

    _initialize(response: HttpEvent<any>): any {
        return response;
      }

}
