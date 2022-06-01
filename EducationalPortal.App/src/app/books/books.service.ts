import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Book } from '../shared/book.model';
import { BookDto } from './book-dto.model';

@Injectable({
  providedIn: 'root'
})
export class BooksService {

  private readonly baseURL = 'https://localhost:7106/api/books';

  constructor(private _http: HttpClient) { }

  public getPage(pageSize: number, pageNumber: number){
    return this._http.get<Book[]>(this.baseURL, { params: { 
                                          pageSize: pageSize, 
                                          pageNumber: pageNumber
                                        }, observe: 'response' });
  }

  public create(book: BookDto){
    var formData = new FormData();
    formData.append("file", book.file, book.file.name);
    formData.append("id", book.id.toString());
    formData.append("name", book.name);
    formData.append("pagesCount", book.pagesCount.toString());
    formData.append("publicationYear", book.publicationYear.toString());
    for (let i = 0; i < book.authors.length; i++) {
      formData.append(`authors[${i}].id`, book.authors[i].id.toString());
      formData.append(`authors[${i}].fullName`, book.authors[i].fullName);
    }
    return this._http.post(this.baseURL, formData);
  }

  public delete(id: number){
    return this._http.delete(`${this.baseURL}/${id}`);
  }
}
