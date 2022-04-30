import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { Course } from '../shared/course.model';
import { User } from '../shared/user.model';

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
}
