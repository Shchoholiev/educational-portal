import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { catchError, map, throwError } from 'rxjs';
import { Author } from 'src/app/shared/author.model';
import { AuthorsService } from '../authors.service';

@Component({
  selector: 'app-create-author',
  templateUrl: './create-author.component.html',
  styleUrls: ['./create-author.component.css']
})
export class CreateAuthorComponent implements OnInit {

  public author: Author = new Author();

  public createErrors: string[] = [];

  @Output() refreshPaging = new EventEmitter<any>(true);

  constructor(private _authorsService: AuthorsService) { }

  ngOnInit(): void {
  }

  public create(){
    this.createErrors = [];
    this._authorsService.create(this.author).pipe(
      map(() => {
        this.refreshPaging.emit();
        this.author = new Author();
      }),
      catchError(err => {
        this.createErrors = Object.values(err.error);
        return throwError(() => {
          return this.createErrors;})
    })
    ).subscribe()
  }
}
