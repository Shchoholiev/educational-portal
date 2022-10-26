import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CourseStatistics } from '../shared/statistics/course-statistics.model';
import { MaterialStatistics } from '../shared/statistics/material-statistics.model';
import { SalesStatistics } from '../shared/statistics/sales-statistics.model';
import { UsersStatistics } from '../shared/statistics/users-statistics.model';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {

  private readonly baseURL = '/api/statistics';

  constructor(private _http: HttpClient) { }

  public getMaterialsStatistics(pageSize: number, pageNumber: number){
    return this._http.get<MaterialStatistics[]>(`${this.baseURL}/materials/${pageSize}/${pageNumber}`, { observe: 'response' });
  }
  
  public getSalesStatistics(){
    return this._http.get<SalesStatistics>(`${this.baseURL}/sales`);
  }

  public getCoursesStatistics(pageSize: number, pageNumber: number){
    return this._http.get<CourseStatistics[]>(`${this.baseURL}/courses/${pageSize}/${pageNumber}`, { observe: 'response' });
  }

  public getUsersStatistics(pageSize: number, pageNumber: number){
    return this._http.get<UsersStatistics>(`${this.baseURL}/users/${pageSize}/${pageNumber}`, { observe: 'response' });
  }
}
