import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { QuizListComponent } from './QuizListComponent';
import { QuizComponent } from './QuizComponent';
import { QuizListResolver, QuizOpenResolver, QuizNewResolver, QuizTakeResolver } from './QuizResolver';
import { QuizTakeComponent } from './QuizTakeComponent';

const QuizRoutes: Routes = [
    {
        children: [
            {
                path: '', redirectTo: 'List', pathMatch: 'full'
            },
            {
                path: 'List',
                component: QuizListComponent,
                resolve:
                {
                    Quizzes: QuizListResolver
                }
            },
            {
                path: 'Open/:id',
                component: QuizComponent,
                resolve:
                {
                    Quiz: QuizOpenResolver
                }
            },
            {
                path: 'Take/:id',
                component: QuizTakeComponent,
                resolve:
                {
                    Quiz: QuizTakeResolver
                }
            },
            {
                path: 'New',
                component: QuizComponent,
                resolve:
                {
                    Quiz: QuizNewResolver
                }
            },
        ],
    path: '',
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(QuizRoutes)
    ]
})
export class QuizRoutingModule { }
