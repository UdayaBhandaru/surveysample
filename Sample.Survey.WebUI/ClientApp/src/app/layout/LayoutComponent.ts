import { Component } from "@angular/core";
import { FxContext } from "@agility/frameworkcore";
import { Router } from "@angular/router";

@Component({
    selector: "layout",
    templateUrl: "./LayoutComponent.html"
})

export class LayoutComponent {
    menus: any;
    parentMenu: any;
    constructor(public fxContext: FxContext, private router: Router) {
        var url = this.router.routerState.snapshot.url;
        if (this.fxContext.userProfile) {
            this.menus = Object.assign([], this.fxContext.userProfile.menus);
            for (var i = 0; i < this.menus.length; i++) {
                if (this.menus[i].url === url) {
                    this.parentMenu = this.menus[i].items;
                }
            }
        }
    }
}
