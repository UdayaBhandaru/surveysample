import { Component, Inject, Input , OnInit} from "@angular/core";
import { MAT_DIALOG_DATA } from "@angular/material";
import { Observable, Subject } from "rxjs";

import { FxSamplesButton, FxSamplesDialog } from "./FxSamplesControlModels";

@Component(
    {
        selector: "fxSamples-message-dialog",
        template: `<span class="fx-dialog-icon {{ _iconCss }}"></span><div mat-dialog-title
                class="fx-dialog-title {{ _titleCss }}">{{ _title }}</div>
                <div mat-dialog-content class="fx-dialog-content {{ _contentCss }}"></div>
                <div mat-dialog-actions class="fx-dialog-actions">
                    <button *ngIf="_showOk" mat-raised-button mat-dialog-close class="fx-button">Ok</button>
                    <button *ngFor="let btn of _buttonList" mat-button (click)='_onClick(btn.displayName)'
                    class="fx-button {{ btn.cssClass }}">{{ btn.displayName }}</button>
                </div>`
    }
)
export class FxSamplesMessageDialogComponent implements OnInit {
    public clickAnnouncedSource = new Subject<string>();
    public click = this.clickAnnouncedSource.asObservable();
    public _title: string;
    public _content: string;
    public _showOk: boolean = true;
    public _iconCss: string;
    public _titleCss: string;
    public _contentCss: string;
    public _buttonList: FxSamplesButton[];
    private _self: FxSamplesMessageDialogComponent;

    constructor( @Inject(MAT_DIALOG_DATA) private data: FxSamplesDialog) {
        this._title = data.title;
        this._content = data.content;
        this._titleCss = data.titleCss;
        this._contentCss = data.contentCss;
        this._buttonList = data.buttonList;
        this._iconCss = data.iconCss;
    }

    public ngOnInit(): void {
        if (this._buttonList !== undefined && this._buttonList.length > 0) {
            this._showOk = false;
        }
    }

    private _onClick(btnName: string) {
        this.clickAnnouncedSource.next(btnName);
    }
}
