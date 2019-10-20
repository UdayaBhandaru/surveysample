import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, ValidatorFn, AbstractControl } from '@angular/forms';
import { Router } from '@angular/router';
import { LoginModel } from './LoginModel';
import { FxContext, CommonService, ServiceDocument, MessageType } from '@agility/frameworkcore';
import { AccountService } from './AccountService';
import { UserProfile } from '@agility/frameworkcore';
import { KeepAliveSessionService } from '../common/KeepAliveSessionService';


@Component({
  selector: 'app-login',
  templateUrl: './LoginComponent.html'
})

export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  dataModel: LoginModel;
  serviceDocument: ServiceDocument<UserProfile>;

  constructor(private fxContext: FxContext, private router: Router,
    private accountService: AccountService,
    private commonService: CommonService,
    private service: AccountService, private keepAliveSessionService: KeepAliveSessionService) {
    fxContext.IsAuthenticated = false;
  }

  ngOnInit(): void {
    this.dataModel = new LoginModel();
    this.dataModel.userName = '';
    this.dataModel.userPassword = '';
    this.loginForm = this.commonService.getFormGroup(this.dataModel, 0);
    // adding multple validators including custom validators."
    this.loginForm.controls['userName'].setValidators([Validators.required, emailValidator('Invalid Email Address')]);
  }

  validate(): any {
    this.fxContext.userProfile = null;
    this.accountService.login(this.loginForm.value).subscribe((response
    ) => {
      if (response == null) {
        this.commonService.showAlert('Login Failed. Please try again.');
        return;
      }
      this.service.serviceDocument.userProfile = response;
      this.fxContext.IsAuthenticated = true;
      // this.fxContext.menus = this.service.serviceDocument.userProfile.menus;
      this.fxContext.userProfile = response;

      this.keepAliveSessionService.start();
      this.router.navigate(['/ProProfs/Dashboard']);

    }, () => {
      this.commonService.showAlert('Login Failed. Please try again.');
    });
  }
}

export function emailValidator(errorMessage: any): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } => {
    // tslint:disable-next-line:max-line-length
    if (control.value.match(/[a-z0-9!#$%&"*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&"*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?/)) {
      return null;
    } else {
      return { errorMessage };
    }
  };
}
