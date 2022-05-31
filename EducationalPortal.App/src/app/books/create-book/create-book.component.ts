import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { catchError, map, throwError } from 'rxjs';
import { Book } from 'src/app/shared/book.model';
import { HelpersService } from 'src/app/shared/helpers.service';
import { BookDto } from '../book-dto.model';
import { BooksService } from '../books.service';

@Component({
  selector: 'app-create-book',
  templateUrl: './create-book.component.html',
  styleUrls: ['./create-book.component.css']
})
export class CreateBookComponent implements OnInit {

  public book: BookDto = new BookDto();

  public createErrors: string[] = [];

  @Output() refreshPaging = new EventEmitter<any>(true);

  constructor(private _booksService: BooksService, private _helpersService: HelpersService) { }

  ngOnInit(): void {
  }

  public create(){
    this.createErrors = [];
    this._booksService.create(this.book).pipe(
      map(() => {
        this.refreshPaging.emit();
        this.book = new BookDto();
      }),
      catchError(err => {
        this.createErrors = Object.values(err.error);
        return throwError(() => {
          return this.createErrors;})
    })
    ).subscribe()
  }

  public upload(event: Event){
    var input = event.target as HTMLInputElement;
    if (input.files) {
      this.book.file = input?.files[0];
    }
  }
}
