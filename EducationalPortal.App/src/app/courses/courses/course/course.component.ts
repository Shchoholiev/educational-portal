import { Component, Input, OnInit } from '@angular/core';
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

  constructor(public shoppingCartService: ShoppingCartService) { }
  
  ngOnInit(): void {
  }

}
