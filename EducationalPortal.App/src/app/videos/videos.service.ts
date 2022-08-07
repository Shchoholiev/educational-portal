import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Quality } from '../shared/quality.model';
import { Video } from '../shared/video.model';
import { VideoDto } from './video-dto.model';

@Injectable({
  providedIn: 'root'
})
export class VideosService {

  private readonly baseURL = '/api/videos';

  constructor(private _http: HttpClient) { }

  public getPage(pageSize: number, pageNumber: number){
    return this._http.get<Video[]>(this.baseURL, { params: { 
                                          pageSize: pageSize, 
                                          pageNumber: pageNumber
                                        }, observe: 'response' });
  }

  public create(video: VideoDto){
    var formData = new FormData();
    formData.append("file", video.file, video.file.name);
    formData.append("id", video.id.toString());
    formData.append("name", video.name);
    formData.append("duration", video.duration.toString());
    formData.append("quality.id", video.quality.id.toString());
    formData.append("quality.name", video.quality.name);
    return this._http.post(this.baseURL, formData);
  }

  public delete(id: number){
    return this._http.delete(`${this.baseURL}/${id}`);
  }

  public getQualities(){
    return this._http.get<Quality[]>(`${this.baseURL}/get-qualities`);
  }
}
