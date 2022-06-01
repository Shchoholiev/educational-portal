import { Component, Input, OnInit } from '@angular/core';
import { catchError, map, throwError } from 'rxjs';
import { Book } from 'src/app/shared/book.model';
import { Course } from 'src/app/shared/course.model';
import { BooksService } from '../books.service';

@Component({
  selector: 'app-add-books',
  templateUrl: './add-books.component.html',
  styleUrls: ['./add-books.component.css']
})
export class AddBooksComponent implements OnInit {

  @Input() course: Course = new Course();

  public books: Book[] = [];

  public metadata: any;
  
  public pageSize = 3;

  public deleteError: string;

  constructor(private _booksService: BooksService) { }

  ngOnInit(): void {
    this.setPage(1);
  }

  public addBookToCourse(id: number){
    if (this.materialExistsInCourse(id)) {
      var index = this.course.materials.findIndex(s => s.id == id);
      this.course.materials.splice(index, 1);
    }
    else{
      var book = this.books.find(a => a.id == id);
      if (book) {
        this.course.materials.push(book);
      }
    }
  }

  public setPage(pageNumber: number){
    this.deleteError = "";
    this._booksService.getPage(this.pageSize, pageNumber).subscribe(
      response => { 
        this.books = response.body as Book[];
        var metadata = response.headers.get('x-pagination');
        if (metadata) {
          this.metadata = JSON.parse(metadata);
        }
      }
    )
  }

  public delete(id: number){
    this.deleteError = "";
    this._booksService.delete(id).pipe(
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

  public materialExistsInCourse(id: number){
    return this.course.materials.find(m => m.id == id);
  }

  public refreshPaging(){
    if (this.metadata) {
      this.setPage(this.metadata.PageNumber);
    }
  }
}
