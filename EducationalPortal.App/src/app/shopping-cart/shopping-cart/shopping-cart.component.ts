import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/auth/auth.service';
import { CartItem } from 'src/app/shared/cart-item.model';
import { Course } from 'src/app/shared/course.model';
import { ShoppingCartService } from '../shopping-cart.service';

@Component({
  selector: 'app-shopping-cart',
  templateUrl: './shopping-cart.component.html',
  styleUrls: ['./shopping-cart.component.css']
})
export class ShoppingCartComponent implements OnInit {

  public cartItems: CartItem[] = [];

  public totalPrice: number = 0;

  public pageSize: number = 10;

  public metadata: string | null;

  constructor(public _shoppingCartService: ShoppingCartService, private _authService: AuthService) { }

  ngOnInit(): void {
    this.refresh();
  }

  public refresh(){
    this.setPage(1);
    this.refreshTotalPrice();
  }

  public refreshTotalPrice(){
    this._shoppingCartService.getTotalPriceAuthorized().subscribe(
      response => this.totalPrice = response
    )
  }

  public setPage(pageNumber: number){
    this._shoppingCartService.getPageAuthorized(this.pageSize, pageNumber).subscribe(
      response => {
        if (response.body) {
          this.cartItems = response.body;
          this.metadata = response.headers.get('x-pagination');
        }
      }
    );
  }

  public delete(id: number){
    if (this._authService.isAuthenticated) {
      this._shoppingCartService.deleteAuthorized(id).subscribe(
        () => {
          var index = this.cartItems.findIndex(ci => ci.id == id);
          this.cartItems.splice(index, 1);
          this.refreshTotalPrice();
        }
      );
    }
  }

  public buy(){
    if (this._authService.isAuthenticated) {
      this._shoppingCartService.buyAuthorized().subscribe(
        () => this.refresh()
      );
    }
  }
}
