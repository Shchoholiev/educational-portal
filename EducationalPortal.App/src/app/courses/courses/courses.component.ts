import { Component, OnInit } from '@angular/core';
import { Course } from '../../shared/course.model';
import { CoursesService } from '../courses.service';

@Component({
  selector: 'app-courses',
  templateUrl: './courses.component.html',
  styleUrls: ['./courses.component.css']
})
export class CoursesComponent implements OnInit {

  public list: Course[];

  public metadata: string | null;
  
  public pageSize = 3;

  constructor(public service: CoursesService) { 
    this.setPage(1);
  }

  setPage(pageNumber: number) {
    this.service.getCoursesPage(this.pageSize, pageNumber).subscribe(
      response => { 
        this.list = response.body as Course[];
        this.metadata = response.headers.get('x-pagination');
      }
    )
  }

  ngOnInit(): void {
  }

}
