import { NgModule } from '@angular/core';
import { QuizRoutingModule } from './QuizRoutingModule';
import { QuizListComponent } from './QuizListComponent';
import { QuizComponent } from './QuizComponent';
import { QuizService } from './QuizService';
import { QuizListResolver, QuizNewResolver, QuizOpenResolver, QuizTakeResolver } from './QuizResolver';
import { FrameworkCoreModule } from '@agility/frameworkcore';
import { CoreModule } from '../common/CoreModule';
import { QuizTakeComponent } from './QuizTakeComponent';
@NgModule({
    imports: [
        QuizRoutingModule,
        FrameworkCoreModule,
        CoreModule
    ],
    declarations: [
        QuizListComponent,
        QuizComponent,
        QuizTakeComponent
    ],
    providers: [
        QuizService,
        QuizListResolver,
        QuizNewResolver,
        QuizOpenResolver,
        QuizTakeResolver
    ]
})
export class QuizModule {
}
