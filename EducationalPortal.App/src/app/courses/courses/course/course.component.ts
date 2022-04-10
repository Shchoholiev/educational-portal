import { Component, Input, OnInit } from '@angular/core';
import { Course } from '../../course.model';

@Component({
  selector: 'app-course',
  providers: [Course],
  templateUrl: './course.component.html',
  styleUrls: ['./course.component.css']
})
export class CourseComponent implements OnInit {

  @Input() course: Course;

  constructor() { }
  
  ngOnInit(): void {
  }

}
