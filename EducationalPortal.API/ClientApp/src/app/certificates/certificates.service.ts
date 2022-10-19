import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class CertificatesService {

  private readonly baseURL = '/api/certificates';

  constructor(private _http: HttpClient) { }

  public getCertificate(courseId: number){
    return this._http.get(`${this.baseURL}/${courseId}`, { responseType: 'blob' });
  }
}
