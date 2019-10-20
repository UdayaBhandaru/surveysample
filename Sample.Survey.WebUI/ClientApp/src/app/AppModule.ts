import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { MiniProfilerInterceptor } from './common/MiniProfilerInterceptor';
import { RouterModule } from '@angular/router';
import { AppComponent } from './AppComponent';
import { FrameworkCoreModule } from "@agility/frameworkcore";
import { AppRoutingModule } from './AppRoutingModule';
import { CoreModule } from './common/CoreModule';
import { MatProgressSpinnerModule } from '@angular/material';
import { KeepAliveSessionService } from './common/KeepAliveSessionService';
import { NgIdleModule } from '@ng-idle/core';
import { NgIdleKeepaliveModule } from '@ng-idle/keepalive';
import { AccountService } from './account/AccountService';
import { SpinnerService } from './common/SpinnerService';
import { AccountModule } from './account/AccountModule';
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    BrowserAnimationsModule,
    HttpClientModule,
    FormsModule,
    FrameworkCoreModule.forRoot(),
    AppRoutingModule,
    CoreModule,
    MatProgressSpinnerModule,
    NgIdleKeepaliveModule.forRoot(),
    NgIdleModule,
    AccountModule
  ],
  providers: [KeepAliveSessionService, AccountService, SpinnerService,
    { provide: HTTP_INTERCEPTORS, useClass: MiniProfilerInterceptor, multi: true}],
  bootstrap: [AppComponent]
})
export class AppModule { }
