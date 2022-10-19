import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth/auth.service';
import { Course } from 'src/app/shared/course.model';
import { ShoppingCartService } from 'src/app/shopping-cart/shopping-cart.service';

@Component({
  selector: 'app-course-search',
  templateUrl: './course-search.component.html',
  styleUrls: ['./course-search.component.css']
})
export class CourseSearchComponent implements OnInit {

  @Input() course: Course;

  constructor(private _shoppingCartService: ShoppingCartService, private _authService: AuthService,
              private _router: Router) { }
  
  ngOnInit(): void {
  }

  public addToCart(id: number){
    if (this._authService.isAuthenticated) {
      this._shoppingCartService.addToCartAuthorized(id).subscribe(
        () => this._router.navigate(["/shopping-cart"])
      );
    }
    else {
      this._shoppingCartService.addToCart(id);
      this._router.navigate(["/shopping-cart"]);
    }
  }

  public getUpdateDate(){
    var date = new Date(this.course.updateDateUTC);
    return date.toLocaleDateString("en-US", { month: 'long', day: '2-digit', year: 'numeric' });
  }
}
