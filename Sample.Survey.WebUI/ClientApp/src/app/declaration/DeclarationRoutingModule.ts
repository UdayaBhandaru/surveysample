import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { DeclarationComponent } from "./DeclarationComponent";
import { DeclarationListComponent } from "./DeclarationListComponent";
import { DeclarationNewResolver } from "./resolvers/DeclarationNewResolver";
import { DeclarationListResolver } from "./resolvers/DeclarationListResolver";
import { DeclarationOpenResolver } from "./resolvers/DeclarationOpenResolver";

const DeclarationRoutes: Routes = [
  {
    children: [
      {
        path: "", pathMatch: "full", redirectTo: "List",
      },
      {
        component: DeclarationListComponent,
        path: "List",
        resolve:
        {
          serviceDocument: DeclarationListResolver,
        },
      },
      {
        component: DeclarationComponent,
        path: "Open/:id",
        resolve:
        {
          serviceDocument: DeclarationOpenResolver,
        },
      },
      {
        component: DeclarationComponent,
        path: "New",
        resolve:
        {
          serviceDocument: DeclarationNewResolver,
        },
      }
    ],
    path: "",
  },
];

@NgModule({
  imports: [
    RouterModule.forChild(DeclarationRoutes),
  ],
})
export class DeclarationRoutingModule { }
