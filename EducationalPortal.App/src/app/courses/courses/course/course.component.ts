import { Component, Input, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/auth/auth.service';
import { ShoppingCartService } from 'src/app/shopping-cart/shopping-cart.service';
import { Course } from '../../../shared/course.model';

@Component({
  selector: 'app-course',
  providers: [Course],
  templateUrl: './course.component.html',
  styleUrls: ['./course.component.css']
})
export class CourseComponent implements OnInit {

  @Input() course: Course;

  @Input() width: number = 18;

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
  }
}
