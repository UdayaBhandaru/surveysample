import { DatePipe } from "@angular/common";
import { ChangeDetectorRef, Component, Inject, LOCALE_ID, OnInit, TemplateRef, ViewChild } from "@angular/core";
import { MatDialogRef } from "@angular/material";
import { ActivatedRoute, Router } from "@angular/router";

import { ServiceDocument } from "@agility/frameworkcore";
import { FxSamplesButton, FxSamplesDialog } from "../common/FxSamplesControlModels";
import { FxSamplesDialogService } from "../common/FxSamplesDialogService";
import { CommonService } from "@agility/frameworkcore";
import { DeclarationService } from "./DeclarationService";
import { DeclarationModel } from "./models/DeclarationModel";

@Component({
  selector: "Declaration-list",
  templateUrl: "./DeclarationListComponent.html",
})
export class DeclarationListComponent implements OnInit {
  public Payload: any;
  public dialogRef: MatDialogRef<any>;
  public dataList: any[] = [];
  public serviceDocument: ServiceDocument<DeclarationModel>;
  public columns: any[];
  @ViewChild("editButton") editTmpl: TemplateRef<DeclarationListComponent>;
  public DeclarationModel: DeclarationModel;
  public FxSamplesDialog: FxSamplesDialog = new FxSamplesDialog();

  constructor(private service: DeclarationService, private route: ActivatedRoute,
    private dialogService: FxSamplesDialogService,
    private commonService: CommonService, private router: Router,
    private changedecref: ChangeDetectorRef) { }
  public Refresh() {
    if (this.dataList.length <= 0) {
      this.service.list().subscribe(() => {
        this.serviceDocument = this.service.serviceDocument;
        this.dataList = this.serviceDocument.dataProfile.dataList! = null
          ? this.serviceDocument.dataProfile.dataList : [];
        this.changedecref.detectChanges();
      });
    }
  }
  public ngOnInit(): void {
    this.route.data
      .subscribe(() => {
        this.serviceDocument = this.service.serviceDocument;
        if (this.serviceDocument.dataProfile != null) {
          this.dataList = this.serviceDocument.dataProfile.dataList;
        }
      });
    this.columns = [
      { name: "ID", prop: "declarationsId" },
      
      { name: "Created Date", prop: "createdDate", pipe: new DateOnlyPipe("en-US") },
      { name: "Declaration No", prop: "declarationNo" },
      { name: "Port Of Origin", prop: "portOfOrigin.locationName" },
      { name: "Port Of Destination", prop: "portOfDestination.locationName" },
      { name: "State Name", prop: "workflowInstance.stateName" },
      {
        cellTemplate: this.editTmpl, name: "Actions", prop: "Actions",
      },
    ];
  }

  public isNew(actions) {
    return actions.claimValue === "New";
  }

  public new(): void {
    this.service.new().subscribe();
  }

  public add(): void {
    this.router.navigate(["/Declaration/New"]);
  }

  public delete($event: MouseEvent, model: DeclarationModel) {
    $event.preventDefault();
    this.DeclarationModel = model;
    this.FxSamplesDialog.title = "Confirmation";
    this.FxSamplesDialog.content = "<div style=\"background-color:yellow\">" +
      "Are you sure you want to delete this Declaration? </div>";
    this.FxSamplesDialog.buttonList = [new FxSamplesButton("", "Ok", ""), new FxSamplesButton("", "Cancel", "")];
    this.dialogRef = this.dialogService.openFxMessageDialog(this.FxSamplesDialog);
    this.dialogRef.componentInstance.click.subscribe(
      (btnName) => {
        if (btnName === "Ok") {
          this.serviceDocument.dataProfile.dataModel = this.DeclarationModel;
          this.service.delete().subscribe((response: any) => {
            if (response.result) {
              this.serviceDocument.dataProfile.dataList.splice(this.serviceDocument.dataProfile.
                dataList.indexOf(this.DeclarationModel), 1);
              this.serviceDocument.dataProfile.dataList = [...this.serviceDocument.dataProfile.dataList];
              this.commonService.showAlert("Declaration deleted successfully.");
            } else {
              this.commonService.showAlert("There is a problem in deleting the Declaration.");
            }
            this.dialogRef.close();
          });
        } else {
          this.dialogRef.close();
        }
      });
  }
}

class DateOnlyPipe extends DatePipe {
  public transform(value): any {
    return super.transform(value, "dd-MM-y HH:mm");
  }
}
