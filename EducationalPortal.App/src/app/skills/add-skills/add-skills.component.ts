import { Component, Input, OnInit } from '@angular/core';
import { catchError, map, of, throwError } from 'rxjs';
import { Course } from 'src/app/shared/course.model';
import { Skill } from 'src/app/shared/skill.model';
import { SkillsService } from '../skills.service';

@Component({
  selector: 'app-add-skills',
  templateUrl: './add-skills.component.html',
  styleUrls: ['./add-skills.component.css']
})
export class AddSkillsComponent implements OnInit {

  @Input() course: Course = new Course();

  public skills: Skill[] = [];

  public metadata: any;
  
  public pageSize = 6;

  public skill: Skill = new Skill();

  public createErrors: string[] = [];

  constructor(private _skillsService: SkillsService) { }

  ngOnInit(): void {
    this.setPage(1);
  }

  public addSkillToCourse(id: number){
    if (this.skillExistsInCourse(id)) {
      var index = this.course.skills.findIndex(s => s.id == id);
      this.course.skills.splice(index, 1);
    }
    else{
      var skill = this.skills.find(s => s.id == id);
      if (skill) {
        this.course.skills.push(skill);
      }
    }
  }

  public setPage(pageNumber: number){
    this._skillsService.getPage(this.pageSize, pageNumber).subscribe(
      response => { 
        this.skills = response.body as Skill[];
        var metadata = response.headers.get('x-pagination');
        if (metadata) {
          this.metadata = JSON.parse(metadata);
        }
      }
    )
  }

  public delete(id: number){
    this._skillsService.delete(id).subscribe(
      () => {
        if (this.metadata) {
          this.setPage(this.metadata.PageNumber);
        }
      }
    );
  }

  public create(){
    this.createErrors = [];
    this._skillsService.create(this.skill).pipe(
      map(() => {
        if (this.metadata) {
          this.setPage(this.metadata.PageNumber);
        }
        this.skill = new Skill();
      }),
      catchError(err => {
        this.createErrors = Object.values(err.error);
        return throwError(() => {
          return this.createErrors;})
    })
    ).subscribe()
  }

  public skillExistsInCourse(skillId: number){
    return this.course.skills.find(s => s.id == skillId);
  }
}
