import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { catchError, map, throwError } from 'rxjs';
import { Course } from 'src/app/shared/course.model';
import { CoursesOrderBy } from 'src/app/shared/courses-order-by.model';
import { SkillLookup } from 'src/app/shared/lookup-models/skill-lookup.model';
import { SkillsService } from 'src/app/skills/skills.service';
import { CoursesService } from '../courses.service';

@Component({
  selector: 'app-courses-search',
  templateUrl: './courses-search.component.html',
  styleUrls: ['./courses-search.component.css']
})
export class CoursesSearchComponent implements OnInit {

  public list: Course[];

  public metadata: any;
  
  public pageSize = 3;

  public orderBy = CoursesOrderBy.Id;

  public isAscending = true;

  public filter = "";

  public lastSearch = "";

  public showSkills = false;

  public skillLookups: SkillLookup[] = [];

  public isBasedOnTime = false;

  public error = "";

  constructor(
    private _service: CoursesService, 
    private _route: ActivatedRoute
    ) { }
  
  ngOnInit(): void {
    this.filter = this._route.snapshot.paramMap.get('filter') || "";
    this.setPage(1);
  }

  setPage(pageNumber: number) {
    this._service.getFilteredCoursesPage(pageNumber, this.pageSize, this.orderBy, this.isAscending, this.filter)
    .subscribe(
      response => { 
        this.list = response.body as Course[];
        var metadata = response.headers.get('x-pagination');
        if (metadata) {
          this.metadata = JSON.parse(metadata);
        }
        this.lastSearch = this.filter;
        this.showSkills = false;
      }
    )
  }

  setOrderBy(order: number) {
    if (this.orderBy != order) {
      this.orderBy = order;
      this.setPage(1);
    }
  }

  setAscending(isAscending: boolean) {
    if (this.isAscending != isAscending) {
      this.isAscending = isAscending;
      this.setPage(1);
    }
  }

  setShowSkills(){
    this.showSkills = true;
    this.error = "";
  }

  toggleBasedOnTime(){
    this.isBasedOnTime = !this.isBasedOnTime;
  }

  cleanSkills(){
    this.skillLookups = [];
  }

  removeSkill(skillId: number){
    var index = this.skillLookups.findIndex(s => s.skillId == skillId);
    if (index > -1) {
      this.skillLookups.splice(index, 1);
    }
  }

  clearFilters(){
    this.filter = "";
    this.isAscending = true;
    this.orderBy = CoursesOrderBy.Id;
    this.error = "";
  }

  performAutomatedSearch(){
    this.clearFilters();
    if (this.isBasedOnTime) {
      this._service.getCoursesByAutomatedSearchBasedOnTime(this.skillLookups)
      .pipe(
        map(response => { 
          this.list = response;
          this.showSkills = false;
        }),
        catchError(err => {
          this.error = err.error.Message;
          return throwError(() => { return this.error; })
      })
      ).subscribe();
    }
    else {
      this._service.getCoursesByAutomatedSearch(this.skillLookups)
      .pipe(
        map(response => { 
          this.list = response;
          this.showSkills = false;
        }),
        catchError(err => {
          this.error = err.error.Message;
          return throwError(() => { return this.error; })
      })
      ).subscribe();
    }
  }

  public getPdf(){
    this.clearFilters();
    if (this.isBasedOnTime) {
      this._service.getPdfForAutomatedSearchBasedOnTime(this.skillLookups)
      .pipe(
        map(response => {
          var url = window.URL.createObjectURL(response);
          window.open(url);
        }),
        catchError(err => {
          this.error = err.error.Message;
          return throwError(() => { return this.error; })
        })
      ).subscribe();
    } else {
      this._service.getPdfForAutomatedSearch(this.skillLookups)
      .pipe(
        map(response => {
          var url = window.URL.createObjectURL(response);
          window.open(url);
        }),
        catchError(err => {
          this.error = err.error.Message;
          return throwError(() => { return this.error; })
        })
      ).subscribe();
    }
  }
}
