import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/auth/auth.service';
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
              public authService: AuthService) { }

  setCourse(id: number){
    this._service.getCourse(id).subscribe(
      data => this.course = data
    );
  }

  ngOnInit(): void {
    this.id = Number(this._route.snapshot.paramMap.get('id')) || 0;
    this.setCourse(this.id);
  }

}
