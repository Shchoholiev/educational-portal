import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LearnCourse } from 'src/app/shared/learn-course.model';
import { CoursesService } from '../courses.service';

@Component({
  selector: 'app-course-learn',
  templateUrl: './course-learn.component.html',
  styleUrls: ['./course-learn.component.css']
})
export class CourseLearnComponent implements OnInit {

  public course: LearnCourse = new LearnCourse();

  public bookLink = "";

  public videoLink = "";

  constructor(private _route: ActivatedRoute, private _coursesService: CoursesService) { }

  setCourse(id: number){
    this._coursesService.getCourseToLearn(id).subscribe(
      response => this.course = response
    );
  }

  ngOnInit(): void {
    var id = Number(this._route.snapshot.paramMap.get('id')) || 0;
    this.setCourse(id);
  }

  public changeProgress(number: number){
    this.course.progress = number;
  }

  public setBook(link: string){
    this.videoLink = "";
    this.bookLink = link;
  }

  public setVideo(link: string){
    this.bookLink = "";
    this.videoLink = link;
  }
}
