import { TemplateRef } from "@angular/core";

export class FxSamplesDialog {
    public disableClose: boolean;
    public width?: string;
    public height?: string;
    public position: {
        top?: string;
        bottom?: string;
        left?: string;
        right?: string;
    };
    public title: string;
    public content: string;
    public iconCss: string;
    public titleCss: string;
    public contentCss: string;
    public buttonList: FxSamplesButton[];
    public templateRef: TemplateRef<any>;
    public componentRef: any;
    public data: any;
}

export class FxSamplesButton {
    public name: string;
    public displayName: string;
    public cssClass: string;

    public constructor(name: string, displayname: string, cssClass: string) {
        const self: FxSamplesButton = this;
        self.name = name;
        self.displayName = displayname;
        self.cssClass = cssClass;
    }
}
