import { Component, OnInit } from '@angular/core';
import { Course } from 'src/app/shared/course.model';
import { ShoppingCartService } from '../shopping-cart.service';

@Component({
  selector: 'app-shopping-cart',
  templateUrl: './shopping-cart.component.html',
  styleUrls: ['./shopping-cart.component.css']
})
export class ShoppingCartComponent implements OnInit {

  public courses: Course[] = [];

  public totalPrice: number = 0;

  public pageSize: number = 10;

  public metadata: string | null;

  constructor(private _shoppingCartService: ShoppingCartService) { }

  ngOnInit(): void {
    this.refresh();
  }

  public refresh(){
    this.setPage(1);
    this._shoppingCartService.getTotalPrice().subscribe(
      response => this.totalPrice = response
    )
  }

  public setPage(pageNumber: number){
    this._shoppingCartService.getPage(this.pageSize, pageNumber).subscribe(
      response => {
        if (response.body) {
          this.courses = response.body;
          this.metadata = response.headers.get('x-pagination');
        }
      }
    );
  }
}
