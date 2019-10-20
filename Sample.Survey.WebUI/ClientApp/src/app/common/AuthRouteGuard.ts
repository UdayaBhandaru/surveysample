import { CanActivate } from "@angular/router";
import { Injectable } from "@angular/core";
import { FxContext } from "@agility/frameworkcore";

@Injectable()
export class AuthRouteGuard implements CanActivate {

    constructor(private fxContext: FxContext) {
    }

    canActivate(): any {

        if (this.fxContext.IsAuthenticated) {
            return true;
        }
        return false;
    }
}