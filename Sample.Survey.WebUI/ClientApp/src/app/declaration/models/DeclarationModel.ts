import { ProfileModel } from "@agility/frameworkcore";
import { LocationModel } from "./LocationModel";

export class DeclarationModel extends ProfileModel {
  declarationId: number;
  DeclarationsNo: string;
  portOfOriginId: number;
  portOfDestinationId: number;
  portOfOrigin: LocationModel;
  portOfDestination: LocationModel;
  multiSelectMember: string[];
}
