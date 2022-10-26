import { Component, OnInit } from '@angular/core';
import { CourseStatistics } from 'src/app/shared/statistics/course-statistics.model';
import { StatisticsService } from '../statistics.service';

@Component({
  selector: 'app-courses-statistics',
  templateUrl: './courses-statistics.component.html',
  styleUrls: ['./courses-statistics.component.css']
})
export class CoursesStatisticsComponent implements OnInit {

  public courses: CourseStatistics[] = [];

  public metadata: any;
  
  public pageSize = 10;

  constructor(private _statisticsService: StatisticsService) { }

  ngOnInit(): void {
    this.setPage(1);
  }

  public setPage(pageNumber: number){
    this._statisticsService.getCoursesStatistics(this.pageSize, pageNumber).subscribe(
      response => { 
        this.courses = response.body as CourseStatistics[];
        var metadata = response.headers.get('x-pagination');
        if (metadata) {
          this.metadata = JSON.parse(metadata);
        }
      }
    )
  }

  public getDate(date: Date){
    var converted = new Date(date);
    return converted.toLocaleDateString("en-US", { hour: 'numeric', minute: 'numeric', month: 'long', day: '2-digit' });
  }
}
