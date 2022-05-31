import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Article } from '../shared/article.model';

@Injectable({
  providedIn: 'root'
})
export class ArticlesService {

  private readonly baseURL = 'https://localhost:7106/api/articles';

  constructor(private _http: HttpClient) { }

  public getPage(pageSize: number, pageNumber: number){
    return this._http.get<Article[]>(this.baseURL, { params: { 
                                          pageSize: pageSize, 
                                          pageNumber: pageNumber
                                        }, observe: 'response' });
  }

  public create(article: Article){
    return this._http.post(this.baseURL, article);
  }

  public delete(id: number){
    return this._http.delete(`${this.baseURL}/${id}`);
  }
}
