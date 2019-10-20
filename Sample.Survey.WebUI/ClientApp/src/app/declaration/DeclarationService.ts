import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

import { ServiceDocument } from "@agility/frameworkcore";
import { DeclarationModel } from "./models/DeclarationModel";
import { Environment } from "../../environments/environment";

@Injectable()
export class DeclarationService {
  public serviceDocument: ServiceDocument<DeclarationModel> = new ServiceDocument<DeclarationModel>();
  constructor(private http: HttpClient) {
  }

  public list(): Observable<ServiceDocument<DeclarationModel>> {
    return this.serviceDocument.list(Environment.appApiBaseUrl + "/api/Declarations/list");
  }

  public new(): Observable<ServiceDocument<DeclarationModel>> {
    return this.serviceDocument.new(Environment.appApiBaseUrl + "/api/Declarations/new");
  }

  public open(id: number): Observable<ServiceDocument<DeclarationModel>> {
    return this.serviceDocument.open(Environment.appApiBaseUrl + "/api/Declarations/open", new HttpParams().set("id", id.toString()));
  }

  public delete(): Observable<ServiceDocument<DeclarationModel>> {
    return this.serviceDocument.delete(Environment.appApiBaseUrl + "/api/Declarations/Delete");
  }

  public save(): Observable<ServiceDocument<DeclarationModel>> {
    return this.serviceDocument.save(Environment.appApiBaseUrl + "/api/Declarations/Save");
  }

  public submit(): Observable<ServiceDocument<DeclarationModel>> {
    return this.serviceDocument.submit(Environment.appApiBaseUrl + "/api/Declarations/Submit");
  }
}
