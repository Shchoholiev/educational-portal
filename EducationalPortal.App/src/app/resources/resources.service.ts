import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Resource } from '../shared/resource.model';

@Injectable({
  providedIn: 'root'
})
export class ResourcesService {

  private readonly baseURL = 'https://localhost:7106/api/resources';

  constructor(private _http: HttpClient) { }

  public getPage(pageSize: number, pageNumber: number){
    return this._http.get<Resource[]>(this.baseURL, { params: { 
                                          pageSize: pageSize, 
                                          pageNumber: pageNumber
                                        }, observe: 'response' });
  }

  public create(resource: Resource){
    return this._http.post(this.baseURL, resource);
  }

  public delete(id: number){
    return this._http.delete(`${this.baseURL}/${id}`);
  }
}
