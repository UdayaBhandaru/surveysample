import { Directive, Input, TemplateRef, ViewContainerRef } from "@angular/core";
import { FormGroup } from "@angular/forms";

@Directive({
  selector: "[fxPermission]",
})
export class FxPermissionDirective {

  public showButton: boolean = false;
  private _claim: string;
  private _type: string;
  private _row: any;

  constructor(private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef) { }

  @Input() set fxPermissionRowValue(value: any) {
    this._row = value;
  }
  @Input() set fxPermissionClaimType(value: string) {
    this._type = value;
  }
  @Input() set fxPermissionClaim(value: string) {
    this._claim = value;
  }
  @Input() set fxPermission(inputData: any) {
    switch (this._type) {
      case "WorkflowClaim":
        if (this._row) {
          for (const item of inputData.dataProfile.actionService.allowedActions) {
            if (item.fromStateId === this._row.workflowStateId
              && item.transitionType === "ST" && item.actionName === this._claim) {
              if (inputData.dataProfile.profileForm !== undefined) {
                inputData.dataProfile.profileForm.controls.currentActionId.setValue(this._claim);
              }
              this.showButton = true;
            }
          }
        }
        else {
          const arr1 = inputData.dataProfile.actionService.allowedActions.find(d => d.transitionClaim = this._claim);
          if (arr1) {
            if (inputData.dataProfile.profileForm !== undefined) {
              inputData.dataProfile.profileForm.controls.currentActionId.setValue(this._claim);
            }
            this.showButton = true;
          }
        }
        break;
      case "PageClaim":
        if (inputData.dataProfile.actionService.pageClaims) {
          for (const item of inputData.dataProfile.actionService.pageClaims) {
            if (item.actionMethod === this._claim) {
              this.showButton = true;
            }
          }
        }
        break;
      case "RoleClaim":
        if (inputData) {
          if (inputData.userProfile && inputData.userProfile.userRoles) {
            for (const item of inputData.userProfile.userRoles) {
              if (item.name === this._claim) {
                this.showButton = true;
              }
            }
          }
        }
        break;
      default:

        break;
    }

    if (this.showButton) {
      this.viewContainer.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainer.clear();
    }
  }
}
