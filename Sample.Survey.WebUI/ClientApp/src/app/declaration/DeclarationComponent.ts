import { ServiceDocument } from "@agility/frameworkcore/servicedocument";
import { Component, OnInit } from "@angular/core";
import { Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";

import { DeclarationService } from "./DeclarationService";
import { DeclarationModel } from "./models/DeclarationModel";
import { LocationModel } from "./models/LocationModel";
import { FxContext } from "@agility/frameworkcore";
@Component({
  selector: "Declaration",
  templateUrl: "./DeclarationComponent.html",
})
export class DeclarationComponent implements OnInit {

public toppingList: string[] = ['Extra cheese', 'Mushroom', 'Onion', 'Pepperoni', 'Sausage'];
  public selected: number = -1;


  public temp: any;
  public serviceDocument: ServiceDocument<DeclarationModel>;
  public DeclarationModel: DeclarationModel = new DeclarationModel();
  public locationData: LocationModel[] = [];
  constructor(private route: ActivatedRoute, private router: Router, public service: DeclarationService, public fxContext: FxContext) { }
  public ngOnInit(): void {
    this.route.data
      .subscribe(() => {
        this.serviceDocument = this.service.serviceDocument;
        this.locationData = this.serviceDocument.domainData.location;
        this.temp = this.service.serviceDocument.dataProfile.dataModel;
      });
    // this.setValidators();
  }
  public submit(transitionClaim: string): void {
    if (this.serviceDocument.dataProfile.profileForm.valid) {
      this.serviceDocument.dataProfile.profileForm.controls["currentActionId"].setValue(transitionClaim);
      this.service.serviceDocument = this.serviceDocument;
      this.service.submit()
        .subscribe(() => {
          this.router.navigate(["/Declaration/List"]);
        });
    }
  }
  /*checkbox change event*/
  public onChange(event) {
    console.log(event)
  }
  public save() {
    this.service.save().subscribe(() => {
      alert("saved");
    });
  }

  public cancel() {
    this.router.navigate(["/Declaration/List"]);
    return false;
  }
  public setValidators(): void {
    if (this.temp.workflowInstance.stateName === "Declaration Rejected") {
      this.serviceDocument.dataProfile.profileForm.controls["designation"].setValidators([Validators.required]);
    }
  }
}
