import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http'
import { Course } from '../shared/course.model';

@Injectable({
  providedIn: 'root'
})
export class CoursesService {

  readonly baseURL = 'https://localhost:7106/api/courses';

  constructor(private http: HttpClient) { }

  getCoursesPage(pageSize: number, pageNumber: number){
    return this.http.get(this.baseURL, { params: { 
                                          pageSize: pageSize, 
                                          pageNumber: pageNumber
                                        }, observe: 'response' });
  }

  getCourse(id: number){
    return this.http.get<Course>(`${this.baseURL}/${id}`);
  }
}
