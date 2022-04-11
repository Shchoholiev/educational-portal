import { MaterialBase } from "./material-base.model";
import { Resource } from "./resource.model";

export class Article extends MaterialBase {
    resource: Resource;
    publicationDate: Date;
}
