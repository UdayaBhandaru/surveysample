import { Component, OnInit, HostListener } from '@angular/core';
import { Router, NavigationStart, NavigationEnd, NavigationCancel, NavigationError } from '@angular/router';
import { FxContext, CommonService, MessageType } from '@agility/frameworkcore';
import { KeepAliveSessionService } from './common/KeepAliveSessionService';
import { AccountService } from './account/AccountService';
import { SpinnerService } from './common/SpinnerService';


@Component({
  selector: 'app-root',
  templateUrl: './AppComponent.html',
  styles: ['.active {background-color:#dcdcdc}', '.fx-nav-container {height: 500px;}', '.fx-nav {width:70px}']
})
export class AppComponent implements OnInit {
  showLoader: boolean;
  pathName: string;
  constructor(public fxContext: FxContext, private router: Router, private keepAliveSessionService: KeepAliveSessionService,
    private accountService: AccountService, private spinnerService: SpinnerService, public commonService: CommonService) {
    this.keepAliveSessionService.setKeepAliveSessionConfiguration();
    router.events.subscribe((event: any) => {
      this.navigationInterceptor(event);
    });
  }


  navigationInterceptor(event): void {
    if (event instanceof NavigationStart) {
      this.showLoader = true;
    }
    if (event instanceof NavigationEnd) {
      this.showLoader = false;
    }
    if (event instanceof NavigationCancel) {
      this.showLoader = false;
    }
    if (event instanceof NavigationError) {
      this.showLoader = false;
    }
  }
  ngOnInit(): void {
    this.spinnerService.status.subscribe((val: boolean) => {
      this.showLoader = val;
    });
    this.router.navigate(['/Account']);
  }

  logout(): any {
    this.router.navigate(['/Account']);
  }

}
