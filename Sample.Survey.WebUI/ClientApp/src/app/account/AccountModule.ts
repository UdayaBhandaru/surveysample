import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { FrameworkCoreModule } from '@agility/frameworkcore';
import { AccountRoutingModule } from './AccountRoutingModule';
import { LoginComponent } from './LoginComponent';
import { AccountService } from './AccountService';
import { CoreModule } from '../common/CoreModule';

@NgModule({
    imports: [
      FrameworkCoreModule, AccountRoutingModule, FormsModule, ReactiveFormsModule,
      CoreModule
    ],
    declarations: [
        LoginComponent,
    ],
    providers: [
        AccountService,
    ],
exports: [FormsModule, ReactiveFormsModule ]
})
export class AccountModule {
}
