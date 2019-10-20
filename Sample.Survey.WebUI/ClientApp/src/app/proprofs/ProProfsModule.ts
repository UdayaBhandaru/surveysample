import { FrameworkCoreModule } from '@agility/frameworkcore';
import { NgModule } from '@angular/core';
import { ProProfsListComponent } from './ProProfsListComponent';
import { ProProfsRoutingModule } from './ProProfsRoutingModule';
import { ProProfsService } from './ProProfsService';
import { ProProfsDashboardResolver, ProProfNewResolver } from './resolvers/ProProfsResolver';
import { CoreModule } from '../common/CoreModule';
import { QuizService } from '../quiz/QuizService';
import { ProProfsComponent } from './ProProfsComponent';
@NgModule({
  declarations: [
    ProProfsListComponent,
    ProProfsComponent
  ],
  imports: [
    ProProfsRoutingModule,
    FrameworkCoreModule,
    CoreModule,
  ],
  providers: [
    QuizService,
    ProProfsService,
    ProProfsDashboardResolver,
    ProProfNewResolver
  ],
})
export class ProProfsModule {
}
