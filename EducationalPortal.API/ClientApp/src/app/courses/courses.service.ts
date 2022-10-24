import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { Course } from '../shared/course.model';
import { User } from '../shared/user.model';
import { LearnCourse } from '../shared/learn-course.model';
import { CoursesOrderBy } from '../shared/courses-order-by.model';
import { SkillLookup } from '../shared/lookup-models/skill-lookup.model';

@Injectable({
  providedIn: 'root'
})
export class CoursesService {

  private readonly baseURL = '/api/courses';

  constructor(private _http: HttpClient) { }

  public getCoursesPage(pageSize: number, pageNumber: number){
    return this._http.get(this.baseURL, { params: { 
                                          pageSize: pageSize, 
                                          pageNumber: pageNumber
                                        }, observe: 'response' });
  }

  public getFilteredCoursesPage(pageNumber: number, pageSize: number, orderBy: CoursesOrderBy, 
                                isAscending: boolean, filter: string){
    return this._http.get<Course[]>(`${this.baseURL}/filtered/${pageNumber}/${pageSize}/${orderBy}/${isAscending}/${filter}`, 
            { observe: 'response' });
  }

  public getCoursesByAutomatedSearch(skillLookups: SkillLookup[]){
    return this._http.post<Course[]>(`${this.baseURL}/automated-search`, skillLookups);
  }

  public getCoursesByAutomatedSearchBasedOnTime(skillLookups: SkillLookup[]){
    return this._http.post<Course[]>(`${this.baseURL}/automated-search-based-on-time`, skillLookups);
  }

  public getCourse(id: number){
    return this._http.get<Course>(`${this.baseURL}/${id}`);
  }
  
  public create(course: Course){
    return this._http.post(`${this.baseURL}`, course);
  }

  public update(course: Course){
    return this._http.put(`${this.baseURL}/${course.id}`, course);
  }

  public delete(id: number){
    return this._http.delete(`${this.baseURL}/${id}`);
  }

  public getCourseToLearn(id: number){
    return this._http.get<LearnCourse>(`${this.baseURL}/learn/${id}`);
  }

  public learned(materialId: number, courseId: number){
    return this._http.put<number>(`${this.baseURL}/learned?materialId=${materialId}&courseId=${courseId}`,
                                  { params: { }, observe: 'response' });
  }

  public unlearned(materialId: number, courseId: number){
    return this._http.put<number>(`${this.baseURL}/unlearned?materialId=${materialId}&courseId=${courseId}`,
                                  { params: { }, observe: 'response' });
  }
}
