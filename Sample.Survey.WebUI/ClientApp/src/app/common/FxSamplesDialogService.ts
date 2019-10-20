import { Injectable, TemplateRef, ViewChild } from "@angular/core";
import { MatDialog, MatDialogConfig, MatDialogRef } from "@angular/material";

import { MessageResult, ServiceDocumentResult } from "@agility/frameworkcore";
import { FxSamplesButton, FxSamplesDialog } from "./FxSamplesControlModels";
import { FxSamplesMessageDialogComponent } from "./FxSamplesDialogComponent";

@Injectable()
export class FxSamplesDialogService {
    public _config: MatDialogConfig = {
        disableClose: false,
        width: "",
        height: "",
        position: {
            top: "",
            bottom: "",
            left: "",
            right: "",
        },
        data: new FxSamplesDialog(),
    };
    public FxSamplesDialog: FxSamplesDialog = new FxSamplesDialog();
    public dialogRef: MatDialogRef<any>;

    constructor(private dialog: MatDialog) {
    }

    public openDialog(FxSamplesDialog: FxSamplesDialog): MatDialogRef<any> {

        this._fillData(FxSamplesDialog);
        if (FxSamplesDialog.templateRef !== undefined) {
            return this.dialog.open(FxSamplesDialog.templateRef, this._config);
        } else if (FxSamplesDialog.componentRef !== undefined) {
            this._config.data = FxSamplesDialog.data;
            return this.dialog.open(FxSamplesDialog.componentRef, this._config);
        } else {
            return this.dialog.open(FxSamplesMessageDialogComponent, this._config);
        }
    }
    public openFxMessageDialog(FxSamplesDialog: FxSamplesDialog): MatDialogRef<FxSamplesMessageDialogComponent> {

        this._fillData(FxSamplesDialog);
        return this.dialog.open(FxSamplesMessageDialogComponent, this._config);
    }

    public openMessageDialog(
        title: string,
        result: MessageResult,
        buttonList: FxSamplesButton[] = [],
        width: string = "",
        height: string = "",
        iconCss: string = "",
        titleCss: string = "",
        contentCss: string = ""): MatDialogRef<FxSamplesMessageDialogComponent> {
        return this._openDialog(title, result.message, buttonList, width, height, iconCss, titleCss, contentCss);
    }

    private _openDialog(
        title: string,
        content: string,
        buttonList: FxSamplesButton[] = [],
        width: string = "",
        height: string = "",
        iconCss: string = "",
        titleCss: string = "",
        contentCss: string = ""): MatDialogRef<FxSamplesMessageDialogComponent> {
        this.FxSamplesDialog.title = title;
        this.FxSamplesDialog.content = content;
        this.FxSamplesDialog.width = width !== "" ? width : "";
        this.FxSamplesDialog.height = height !== "" ? height : "";
        this.FxSamplesDialog.iconCss = iconCss !== "" ? iconCss : "";
        this.FxSamplesDialog.titleCss = titleCss !== "" ? titleCss : "";
        this.FxSamplesDialog.contentCss = contentCss !== "" ? contentCss : "";
        this.FxSamplesDialog.buttonList = buttonList;

        this._fillData(this.FxSamplesDialog);
        this.dialogRef = this.dialog.open(FxSamplesMessageDialogComponent, this._config);
        this.dialogRef.componentInstance.click.subscribe(
            () => {
                this.dialogRef.close();
            });

        return this.dialogRef;
    }

    private _fillData(FxSamplesDialog: FxSamplesDialog) {
        this._config.disableClose = FxSamplesDialog.disableClose;
        this._config.width = (FxSamplesDialog.width == null || FxSamplesDialog.width === undefined) ? "" : FxSamplesDialog.width;
        this._config.height = (FxSamplesDialog.height == null || FxSamplesDialog.height === undefined) ? "" : FxSamplesDialog.height;
        this._config.position = (FxSamplesDialog.position == null || FxSamplesDialog.position === undefined) ? {
            top: "",
            bottom: "",
            left: "",
            right: "",
        } : FxSamplesDialog.position;

        this._config.data = FxSamplesDialog;
    }

}
