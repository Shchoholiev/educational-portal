import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Course } from 'src/app/shared/course.model';
import { HelpersService } from 'src/app/shared/helpers.service';
import { CoursesService } from '../courses.service';

@Component({
  selector: 'app-course-create',
  templateUrl: './course-create.component.html',
  styleUrls: ['./course-create.component.css']
})
export class CourseCreateComponent implements OnInit {

  public course: Course = new Course();

  public panel: string = "";

  constructor(private _helpersService: HelpersService, private _coursesService: CoursesService,
              private _router: Router ) { }

  ngOnInit(): void {
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

  public create(){
    this._coursesService.create(this.course).subscribe(
      response => {
        var id = (response as any).id;
        this._router.navigate(["courses", id])
      }
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
