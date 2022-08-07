import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { from } from 'rxjs';
import { AuthService } from '../auth/auth.service';
import { CartItem } from '../shared/cart-item.model';

@Injectable({
  providedIn: 'root'
})
export class ShoppingCartService {

  private readonly baseURL = '/api/shopping-cart';

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

  public getPage(pageSize: number, pageNumber: number){
    var data = localStorage.getItem("cart");
    if (data) {
      return this._http.get<CartItem[]>(`${this.baseURL}/page-from-cookie`, 
                                          { params: { 
                                              pageSize: pageSize, 
                                              pageNumber: pageNumber,
                                              cookie: data
                                          }, observe: 'response' });
    }

    return from([]);
  }

  public getTotalPrice(){
    var data = localStorage.getItem("cart");
    if (data) {
      return this._http.get<number>(`${this.baseURL}/total-price-from-cookie`, 
                                                    { params: {
                                                        cookie: data
                                                    }, observe: 'response' });
    }

    return from([]);
  }

  public addToCart(id: number){
    var data = localStorage.getItem("cart") || "";
    var item = `-${id}`;
    if (!data.includes(item)) {
      data += item;
      localStorage.setItem("cart", data);
    }
  }

  public delete(id: number){
    var data = localStorage.getItem("cart");
    if (data) {
      var regex = new RegExp(`-${id}`, "gm");
      var result = data.replace(regex, "");
      localStorage.setItem("cart", result);
    }
  }
}
