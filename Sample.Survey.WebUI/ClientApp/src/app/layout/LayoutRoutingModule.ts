import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { LayoutComponent } from "./LayoutComponent";

const LayoutRoutes: Routes = [
    {
        path: "Layout",
        children: [
            {
                path: "", redirectTo: "Index", pathMatch: "full"
            },
            {
                path: "Index",
                component: LayoutComponent,
            },
        ]
    }
];



@NgModule({
    imports: [
        RouterModule.forChild(LayoutRoutes)
    ]
})
export class LayoutRoutingModule { }
