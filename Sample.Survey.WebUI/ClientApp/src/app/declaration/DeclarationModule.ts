import { FrameworkCoreModule } from "@agility/frameworkcore";
import { NgModule } from "@angular/core";
import { DeclarationComponent } from "./DeclarationComponent";
import { DeclarationListComponent } from "./DeclarationListComponent";
import { DeclarationRoutingModule } from "./DeclarationRoutingModule";
import { DeclarationService } from "./DeclarationService";
import { DeclarationListResolver } from "./resolvers/DeclarationListResolver";
import { DeclarationNewResolver } from "./resolvers/DeclarationNewResolver";
import { DeclarationOpenResolver } from "./resolvers/DeclarationOpenResolver";
import { CoreModule } from "../common/CoreModule";
@NgModule({
  declarations: [
    DeclarationListComponent,
    DeclarationComponent
  ],
  imports: [
    DeclarationRoutingModule,
    FrameworkCoreModule,
    CoreModule,
  ],
  providers: [
    DeclarationService,
    DeclarationListResolver,
    DeclarationNewResolver,
    DeclarationOpenResolver,
  ],
})
export class DeclarationModule {
}
