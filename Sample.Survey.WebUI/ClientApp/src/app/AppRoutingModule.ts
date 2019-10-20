import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
const appRoutes: Routes = [
  { path: 'ProProfs', loadChildren: './proprofs/ProProfsModule#ProProfsModule' },
  { path: 'Quiz', loadChildren: './quiz/QuizModule#QuizModule' }
];
@NgModule({

  imports: [
    RouterModule.forRoot(appRoutes)
  ]
})
export class AppRoutingModule { }
