import { ServiceDocument, CommonService } from '@agility/frameworkcore';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, Subscription } from 'rxjs';
import { Subject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { ProProfQuiz } from '../quiz/models/ProProfQuiz';
import { Form, FormGroup } from '@angular/forms';
import { ProProfsUser } from './models/ProProfsUser';
import { ProProfsService } from './ProProfsService';
@Component({
  selector: 'app-proprofs-quiz',
  templateUrl: './ProProfsComponent.html',
})
export class ProProfsComponent implements OnInit, OnDestroy {
  public quizForm: FormGroup;
  public quizzes: ProProfQuiz[];
  public countryName: string;
  public stateName: string;
  public ngUnsubscribe = new Subject();
  public dataModel: ProProfsUser;
  constructor(private route: ActivatedRoute,
    private router: Router, private commonService: CommonService, private service: ProProfsService ) {
  }

  public ngOnInit(): void {
    this.dataModel = new ProProfsUser();
    this.dataModel.email = '';
    this.dataModel.quizId = '';
    this.dataModel.quizzes = [];
    this.quizForm = this.commonService.getFormGroup(this.dataModel, 0);
    this.quizzes = this.route.snapshot.data['quizzes'];
  }

  public changeselectedquiz(quiz: any): void {

  }

  public save() {
    const quiz = this.quizzes.find(x => x.quizUniqueName === this.quizForm.value.quizId);
    this.dataModel.email = this.quizForm.value.email;
    this.dataModel.quizzes.push(quiz);
    this.service.save(this.dataModel).subscribe((response
      ) => {
        this.router.navigate(['/ProProfs/Dashboard']);
      });
  }

  public cancel() {
    this.router.navigate(['/ProProfs/Dashboard']);
    return false;
  }

  ngOnDestroy() {
    this.ngUnsubscribe.next();
    this.ngUnsubscribe.complete();
  }
}
