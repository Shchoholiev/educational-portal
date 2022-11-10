import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Course } from 'src/app/shared/course.model';
import { HelpersService } from 'src/app/shared/helpers.service';
import { CoursesService } from '../courses.service';

@Component({
  selector: 'app-course-edit',
  templateUrl: './course-edit.component.html',
  styleUrls: ['./course-edit.component.css']
})
export class CourseEditComponent implements OnInit {

  public course: Course = new Course();

  public panel: string = "";

  constructor(private _helpersService: HelpersService, private _coursesService: CoursesService,
              private _route: ActivatedRoute, private _router: Router, ) { }

  ngOnInit(): void {
    var id = Number(this._route.snapshot.paramMap.get('id')) || 0;
    this.setCourse(id);
  }

  setCourse(id: number){
    this._coursesService.getCourseForEdit(id).subscribe(
      response => {
        this.course = response;
      }
    );
  }

  public setThumbnail(event: Event){
    this.fileToLink(event)?.subscribe(
      response => this.course.thumbnail = (response as any).link
    )
  }

  public removeSkill(id: number){
    var index = this.course.skills.findIndex(s => s.id == id);
    this.course.skills.splice(index, 1);
  }

  public removeMaterial(id: number){
    var index = this.course.materials.findIndex(s => s.id == id);
    this.course.materials.splice(index, 1);
  }

  public update(){
    console.log(this.course);
    
    this._coursesService.update(this.course).subscribe(
      () => this._router.navigate(["courses", this.course.id])
    );
  }

  public addSkills(){
    this.panel = "skills";
  }

  public addBooks(){
    this.panel = "books";
  }

  public addArticles(){
    this.panel = "articles";
  }

  public addVideos(){
    this.panel = "videos";
  }

  public addFinalTask(){
    this.panel = "finalTasks";
  }

  private fileToLink(event: Event){
    var input = event.target as HTMLInputElement;
    if (input.files) {
      return this._helpersService.fileToLink('thumbnails', input?.files[0]);
    }
    return;
  }
}
