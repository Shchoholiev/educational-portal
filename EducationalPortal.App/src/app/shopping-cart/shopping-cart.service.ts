import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Course } from '../shared/course.model';

@Injectable({
  providedIn: 'root'
})
export class ShoppingCartService {

  private readonly baseURL = 'https://localhost:7106/api/shopping-cart';

  constructor(private _http: HttpClient) { }

  public getPage(pageSize: number, pageNumber: number){
    return this._http.get<Course[]>(this.baseURL, { params: { 
                                          pageSize: pageSize, 
                                          pageNumber: pageNumber
                                        }, observe: 'response' });
  }

  public getTotalPrice(){
    return this._http.get<number>(`${this.baseURL}/total-price`);
  }

  public addToCart(id: number){
    this._http.put(`${this.baseURL}/add-to-cart/${id}`, {}).subscribe();
  }

  public delete(id: number){
    this._http.delete(`${this.baseURL}/${id}`).subscribe();
  }

  public buy(){
    this._http.put(`${this.baseURL}/buy`, {}).subscribe();
  }

}
