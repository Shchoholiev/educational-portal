import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { LearnCourse } from 'src/app/shared/learn-course.model';
import { MaterialBase } from 'src/app/shared/material-base.model';
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

  public showFinalTask = false;

  public chosenMaterialId = 0;

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

  public setBook(material: MaterialBase){
    this.videoLink = "";
    this.showFinalTask = false;
    this.bookLink = material.link;
    this.chosenMaterialId = material.id;
  }

  public setVideo(material: MaterialBase){
    this.bookLink = "";
    this.showFinalTask = false;
    this.videoLink = material.link;
    this.chosenMaterialId = material.id;
  }

  public setFinalTask(){
    if (this.course.progress == 100) {
      this.videoLink = "";
      this.bookLink = "";
      this.showFinalTask = true;
      this.chosenMaterialId = 0;
    }
  }
}
