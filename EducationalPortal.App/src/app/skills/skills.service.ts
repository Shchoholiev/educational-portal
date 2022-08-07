import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Skill } from '../shared/skill.model';

@Injectable({
  providedIn: 'root'
})
export class SkillsService {

  private readonly baseURL = '/api/skills';

  constructor(private _http: HttpClient) { }

  public getPage(pageSize: number, pageNumber: number){
    return this._http.get<Skill[]>(this.baseURL, { params: { 
                                          pageSize: pageSize, 
                                          pageNumber: pageNumber
                                        }, observe: 'response' });
  }

  public create(skill: Skill){
    return this._http.post(this.baseURL, skill);
  }

  public delete(id: number){
    return this._http.delete(`${this.baseURL}/${id}`);
  }
}
