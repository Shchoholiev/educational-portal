import { Author } from "../shared/author.model";
import { MaterialBase } from "../shared/material-base.model";

export class BookDto extends MaterialBase {
    pagesCount: number = 0;
    publicationYear: number = 0;
    authors: Author[] = [];
    file: File;
}
