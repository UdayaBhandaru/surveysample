import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProProfsListComponent } from './ProProfsListComponent';
import { ProProfsDashboardResolver, ProProfNewResolver } from './resolvers/ProProfsResolver';
import { ProProfsComponent } from './ProProfsComponent';

const ProProfsRoutes: Routes = [
  {
    children: [
      {
        path: '', pathMatch: 'full', redirectTo: 'Dashboard',
      },
      {
        component: ProProfsListComponent,
        path: 'Dashboard',
        resolve:
          {
            user: ProProfsDashboardResolver,
          },
      },
      {
        component: ProProfsComponent,
        path: 'New',
        resolve:
          {
            quizzes: ProProfNewResolver,
          },
      }
    ],
    path: '',
  },
];

@NgModule({
  imports: [
    RouterModule.forChild(ProProfsRoutes),
  ],
})
export class ProProfsRoutingModule { }
