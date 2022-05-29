import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { Course } from '../shared/course.model';
import { User } from '../shared/user.model';
import { LearnCourse } from '../shared/learn-course.model';

@Injectable({
  providedIn: 'root'
})
export class CoursesService {

  private readonly baseURL = 'https://localhost:7106/api/courses';

  constructor(private _http: HttpClient) { }

  public getCoursesPage(pageSize: number, pageNumber: number){
    return this._http.get(this.baseURL, { params: { 
                                          pageSize: pageSize, 
                                          pageNumber: pageNumber
                                        }, observe: 'response' });
  }

  public getCourse(id: number){
    return this._http.get<Course>(`${this.baseURL}/${id}`);
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
