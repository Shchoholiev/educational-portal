import { Author } from "./author.model";
import { MaterialBase } from "./material-base.model";

export class Book extends MaterialBase {
    pagesCount: number = 0;
    extension: string = "";
    publicationYear: number = 0;
    authors: Author[] = [];
}