import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Course } from 'src/app/shared/course.model';
import { CoursesOrderBy } from 'src/app/shared/courses-order-by.model';
import { ShoppingCartService } from 'src/app/shopping-cart/shopping-cart.service';
import { CoursesService } from '../courses.service';

@Component({
  selector: 'app-courses-search',
  templateUrl: './courses-search.component.html',
  styleUrls: ['./courses-search.component.css']
})
export class CoursesSearchComponent implements OnInit {

  public list: Course[];

  public metadata: any;
  
  public pageSize = 3;

  public orderBy = CoursesOrderBy.Id;

  public isAscending = true;

  public filter = "";

  public lastSearch = "";

  constructor(
    public service: CoursesService, 
    public shoppingCartService: ShoppingCartService,
    private _route: ActivatedRoute
    ) { }
  
  ngOnInit(): void {
    this.filter = this._route.snapshot.paramMap.get('filter') || "";
    this.setPage(1);
  }

  setPage(pageNumber: number) {
    this.service.getFilteredCoursesPage(pageNumber, this.pageSize, this.orderBy, this.isAscending, this.filter)
    .subscribe(
      response => { 
        this.list = response.body as Course[];
        var metadata = response.headers.get('x-pagination');
        if (metadata) {
          this.metadata = JSON.parse(metadata);
        }
        this.lastSearch = this.filter;
      }
    )
  }

  setOrderBy(order: number) {
    if (this.orderBy != order) {
      this.orderBy = order;
      this.setPage(1);
    }
  }

  setAscending(isAscending: boolean) {
    if (this.isAscending != isAscending) {
      this.isAscending = isAscending;
      this.setPage(1);
    }
  }
}
