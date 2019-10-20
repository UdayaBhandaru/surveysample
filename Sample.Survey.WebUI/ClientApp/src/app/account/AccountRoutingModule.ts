import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './LoginComponent';

const AccountRoutes: Routes = [
    {
        path: 'Account',
        children: [
            {
                path: '', redirectTo: 'Login', pathMatch: 'full'
            },
            {
                path: 'Login',
                component: LoginComponent
            }
        ]
    }
];

@NgModule({
    imports: [
        RouterModule.forChild(AccountRoutes)
    ]
})
export class AccountRoutingModule { }
