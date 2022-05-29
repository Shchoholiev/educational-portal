import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { CartItem } from '../shared/cart-item.model';

@Injectable({
  providedIn: 'root'
})
export class ShoppingCartService {

  private readonly baseURL = 'https://localhost:7106/api/shopping-cart';

  constructor(private _http: HttpClient) { }

  public getPageAuthorized(pageSize: number, pageNumber: number){
    return this._http.get<CartItem[]>(this.baseURL, { params: { 
                                          pageSize: pageSize, 
                                          pageNumber: pageNumber
                                        }, observe: 'response' });
  }

  public getTotalPriceAuthorized(){
    return this._http.get<number>(`${this.baseURL}/total-price`);
  }

  public addToCartAuthorized(id: number){
    return this._http.put(`${this.baseURL}/add-to-cart/${id}`, {});
  }

  public deleteAuthorized(id: number){
    return this._http.delete(`${this.baseURL}/${id}`);
  }

  public buyAuthorized(){
    return this._http.put(`${this.baseURL}/buy`, {});
  }
}
