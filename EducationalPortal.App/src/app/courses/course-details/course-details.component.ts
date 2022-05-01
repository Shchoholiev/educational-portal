import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
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

  constructor(private route: ActivatedRoute, private service: CoursesService) { }

  setCourse(id: number){
    this.service.getCourse(id).subscribe(
      data => this.course = data
    );
  }

  ngOnInit(): void {
    this.id = Number(this.route.snapshot.paramMap.get('id')) || 0;
    this.setCourse(this.id);
  }

}
