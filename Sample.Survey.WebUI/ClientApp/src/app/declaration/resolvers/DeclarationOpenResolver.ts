import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve } from "@angular/router";

import { Observable } from "rxjs";

import { ServiceDocument } from "@agility/frameworkcore/servicedocument";
import { DeclarationService } from "../DeclarationService";
import { DeclarationModel } from "../models/DeclarationModel";

@Injectable()
export class DeclarationOpenResolver implements Resolve<ServiceDocument<DeclarationModel>> {
  constructor(private service: DeclarationService) { }

  public resolve(route: ActivatedRouteSnapshot): Observable<ServiceDocument<DeclarationModel>> {
    return this.service.open(route.params.id);
  }
}
