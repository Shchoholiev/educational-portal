import { Component, OnInit } from '@angular/core';
import { ShoppingCartService } from 'src/app/shopping-cart/shopping-cart.service';
import { Course } from '../../shared/course.model';
import { CoursesService } from '../courses.service';

@Component({
  selector: 'app-courses',
  templateUrl: './courses.component.html',
  styleUrls: ['./courses.component.css']
})
export class CoursesComponent implements OnInit {

  public list: Course[];

  public metadata: any;
  
  public pageSize = 4;

  constructor(public service: CoursesService, public shoppingCartService: ShoppingCartService) { }
  
  ngOnInit(): void {
    this.setPage(1);
  }

  setPage(pageNumber: number) {
    this.service.getCoursesPage(this.pageSize, pageNumber).subscribe(
      response => { 
        this.list = response.body as Course[];
        var metadata = response.headers.get('x-pagination');
        if (metadata) {
          this.metadata = JSON.parse(metadata);
        }
      }
    )
  }
}
