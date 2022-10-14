import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { CoursesService } from 'src/app/courses/courses.service';
import { Video } from 'src/app/shared/video.model';

@Component({
  selector: 'app-video-side',
  templateUrl: './video-side.component.html',
  styleUrls: ['./video-side.component.css']
})
export class VideoSideComponent implements OnInit {

  @Input() video: Video = new Video();

  @Input() courseId: number = 0;

  @Input() isChosen = false;

  @Output() changeProgress = new EventEmitter<any>(true);

  constructor(private _coursesService: CoursesService) { }

  ngOnInit(): void {
  }

  public getTime(){
    var date = new Date(this.video.duration);
    var seconds: number | string = date.getSeconds();
    if (seconds < 10) {
      seconds = "0" + seconds;
    }
    return date.getMinutes() + ":" + seconds;
  }

  public learned(){
    this._coursesService.learned(this.video.id, this.courseId).subscribe(
      response => {
        this.changeProgress.emit(response);
        this.video.isLearned = true;
      }
    )
  }

  public unlearned(){
    this._coursesService.unlearned(this.video.id, this.courseId).subscribe(
      response => {
        this.changeProgress.emit(response);
        this.video.isLearned = false;
      }
    )
  }

  public click(){
    if (this.video.isLearned) {
      this.unlearned();
    }
    else {
      this.learned();
    }
  }

}
