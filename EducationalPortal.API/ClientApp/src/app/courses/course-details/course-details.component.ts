import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/auth/auth.service';
import { ShoppingCartService } from 'src/app/shopping-cart/shopping-cart.service';
import { Course } from '../../shared/course.model';
import { CoursesService } from '../courses.service';

@Component({
  selector: 'app-course-details',
  templateUrl: './course-details.component.html',
  styleUrls: ['./course-details.component.css']
})
export class CourseDetailsComponent implements OnInit {

  private id: number = 0;

  public course: Course;

  constructor(private _route: ActivatedRoute, private _service: CoursesService, 
              public authService: AuthService, private _shoppingCartService: ShoppingCartService, 
              private _router: Router) { }

  setCourse(id: number){
    this._service.getCourse(id).subscribe(
      data => this.course = data
    );
  }

  ngOnInit(): void {
    this.id = Number(this._route.snapshot.paramMap.get('id')) || 0;
    this.setCourse(this.id);
  }

  public addToCart(id: number){
    if (this.authService.isAuthenticated) {
      this._shoppingCartService.addToCartAuthorized(id).subscribe(
        () => this._router.navigate(["/shopping-cart"])
      );
    }
    else {
      this._shoppingCartService.addToCart(id);
      this._router.navigate(["/shopping-cart"]);
    }
  }
}
