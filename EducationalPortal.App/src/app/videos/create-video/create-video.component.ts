import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { catchError, map, throwError } from 'rxjs';
import { HelpersService } from 'src/app/shared/helpers.service';
import { Quality } from 'src/app/shared/quality.model';
import { VideoDto } from '../video-dto.model';
import { VideosService } from '../videos.service';

@Component({
  selector: 'app-create-video',
  templateUrl: './create-video.component.html',
  styleUrls: ['./create-video.component.css']
})
export class CreateVideoComponent implements OnInit {

  public video: VideoDto = new VideoDto();

  public qualities: Quality[] = [];

  public createErrors: string[] = [];

  @Output() refreshPaging = new EventEmitter<any>(true);

  constructor(private _videosService: VideosService) { }

  ngOnInit(): void {
    this._videosService.getQualities().subscribe(
      response => this.qualities = response
    )
  }

  public create(){
    this.createErrors = [];
    this._videosService.create(this.video).pipe(
      map(() => {
        this.refreshPaging.emit();
        this.video = new VideoDto();
      }),
      catchError(err => {
        this.createErrors = Object.values(err.error);
        return throwError(() => {
          return this.createErrors;})
    })
    ).subscribe()
  }

  public upload(event: Event){
    var input = event.target as HTMLInputElement;
    if (input.files) {
      this.video.file = input?.files[0];
    }
  }

  public chooseQuality(event: Event){
    var option = event.target as HTMLOptionElement;
    var quality = this.qualities.find(q => q.id == parseInt(option.value));
    if (quality) {
      this.video.quality = quality;
    }
  }
}
