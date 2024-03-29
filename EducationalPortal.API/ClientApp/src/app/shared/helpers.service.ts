import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class HelpersService {

  private readonly helpersURL = '/api/helpers';

  constructor(private _http: HttpClient) { }

  public fileToLink(blobContainer: string, file: File){
    var formData = new FormData;
    formData.append("file", file, file.name)
    return this._http.post(`${this.helpersURL}/file-to-link/${blobContainer}`, formData);
  }
}
