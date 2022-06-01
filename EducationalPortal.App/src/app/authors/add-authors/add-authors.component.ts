import { Component, Input, OnInit } from '@angular/core';
import { catchError, map, throwError } from 'rxjs';
import { BookDto } from 'src/app/books/book-dto.model';
import { Author } from 'src/app/shared/author.model';
import { Book } from 'src/app/shared/book.model';
import { AuthorsService } from '../authors.service';

@Component({
  selector: 'app-add-authors',
  templateUrl: './add-authors.component.html',
  styleUrls: ['./add-authors.component.css']
})
export class AddAuthorsComponent implements OnInit {

  @Input() book: BookDto = new BookDto();

  public authors: Author[] = [];

  public metadata: any;
  
  public pageSize = 6;

  public deleteError: string;

  constructor(private _authorsService: AuthorsService) { }

  ngOnInit(): void {
    this.setPage(1);
  }

  public addAuthorToBook(id: number){
    if (this.authorExistsInBook(id)) {
      var index = this.book.authors.findIndex(a => a.id == id);
      this.book.authors.splice(index, 1);
    }
    else{
      var author = this.authors.find(a => a.id == id);
      if (author) {
        this.book.authors.push(author);
      }
    }
  }

  public setPage(pageNumber: number){
    this.deleteError = "";
    this._authorsService.getPage(this.pageSize, pageNumber).subscribe(
      response => { 
        this.authors = response.body as Author[];
        var metadata = response.headers.get('x-pagination');
        if (metadata) {
          this.metadata = JSON.parse(metadata);
        }
      }
    )
  }

  public delete(id: number){
    this.deleteError = "";
    this._authorsService.delete(id).pipe(
      map(() => {
        this.refreshPaging();
      }),
      catchError(err => {
        this.deleteError = err.error;
        return throwError(() => {
          return this.deleteError;})
    })
    ).subscribe()
  }

  public authorExistsInBook(id: number){
    return this.book.authors.find(a => a.id == id);
  }

  public refreshPaging(){
    if (this.metadata) {
      this.setPage(this.metadata.PageNumber);
    }
  }
}
