import { NgModule } from "@angular/core";
import { FrameworkCoreModule } from "@agility/frameworkcore";
import { LayoutComponent } from "./LayoutComponent";
import { LayoutRoutingModule } from "./LayoutRoutingModule";
import { CoreModule } from "../common/CoreModule";
@NgModule({
    imports: [
        FrameworkCoreModule,
      LayoutRoutingModule,
      CoreModule
    ],
    declarations: [
        LayoutComponent
    ],
    exports: [FrameworkCoreModule, LayoutComponent]
})
export class LayoutModule {
}
