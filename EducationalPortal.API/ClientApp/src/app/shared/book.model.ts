import { Author } from "./author.model";
import { Extension } from "./extension.model";
import { MaterialBase } from "./material-base.model";

export class Book extends MaterialBase {
    pagesCount: number = 0;
    extension: Extension = new Extension();
    publicationYear: number = 0;
    authors: Author[] = [];
}