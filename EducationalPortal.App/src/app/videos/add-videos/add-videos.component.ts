import { Component, Input, OnInit } from '@angular/core';
import { catchError, map, throwError } from 'rxjs';
import { Course } from 'src/app/shared/course.model';
import { Video } from 'src/app/shared/video.model';
import { VideosService } from '../videos.service';

@Component({
  selector: 'app-add-videos',
  templateUrl: './add-videos.component.html',
  styleUrls: ['./add-videos.component.css']
})
export class AddVideosComponent implements OnInit {

  @Input() course: Course = new Course();

  public videos: Video[] = [];

  public metadata: any;
  
  public pageSize = 3;

  public deleteError: string;

  constructor(private _videosService: VideosService) { }

  ngOnInit(): void {
    this.setPage(1);
  }

  public addVideoToCourse(id: number){
    if (this.materialExistsInCourse(id)) {
      var index = this.course.materials.findIndex(s => s.id == id);
      this.course.materials.splice(index, 1);
    }
    else{
      var video = this.videos.find(v => v.id == id);
      if (video) {
        this.course.materials.push(video);
      }
    }
  }

  public setPage(pageNumber: number){
    this.deleteError = "";
    this._videosService.getPage(this.pageSize, pageNumber).subscribe(
      response => { 
        this.videos = response.body as Video[];
        var metadata = response.headers.get('x-pagination');
        if (metadata) {
          this.metadata = JSON.parse(metadata);
        }
      }
    )
  }

  public delete(id: number){
    this.deleteError = "";
    this._videosService.delete(id).pipe(
      map(() => {
        this.refreshPaging();
      }),
      catchError(err => {
        this.deleteError = err.error;
        return throwError(() => {
          return this.deleteError;})
    })
    ).subscribe()
  }

  public materialExistsInCourse(id: number){
    return this.course.materials.find(m => m.id == id);
  }

  public refreshPaging(){
    if (this.metadata) {
      this.setPage(this.metadata.PageNumber);
    }
  }

  public getTime(video: Video){
    var date = new Date(video.duration);
    var seconds: number | string = date.getSeconds();
    if (seconds < 10) {
      seconds = "0" + seconds;
    }
    return date.getMinutes() + ":" + seconds;
  }
}
