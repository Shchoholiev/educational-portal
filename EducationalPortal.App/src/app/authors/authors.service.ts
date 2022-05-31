import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Author } from '../shared/author.model';

@Injectable({
  providedIn: 'root'
})
export class AuthorsService {

  private readonly baseURL = 'https://localhost:7106/api/authors';

  constructor(private _http: HttpClient) { }

  public getPage(pageSize: number, pageNumber: number){
    return this._http.get<Author[]>(this.baseURL, { params: { 
                                          pageSize: pageSize, 
                                          pageNumber: pageNumber
                                        }, observe: 'response' });
  }

  public create(author: Author){
    return this._http.post(this.baseURL, author);
  }

  public delete(id: number){
    return this._http.delete(`${this.baseURL}/${id}`);
  }
}
