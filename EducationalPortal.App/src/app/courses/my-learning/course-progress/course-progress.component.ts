import { Component, Input, OnInit } from '@angular/core';
import { UsersCourses } from 'src/app/shared/users-courses.model';

@Component({
  selector: 'app-course-progress',
  templateUrl: './course-progress.component.html',
  styleUrls: ['./course-progress.component.css']
})
export class CourseProgressComponent implements OnInit {

  @Input() userCourse: UsersCourses = new UsersCourses();

  public progress: number = 0;

  constructor() { }

  ngOnInit(): void {
    if (this.userCourse.learnedMaterialsCount) {
      this.progress = Math.ceil(this.userCourse.materialsCount * 100 / this.userCourse.learnedMaterialsCount);
    }
  }

}
