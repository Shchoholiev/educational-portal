import { Component, Input, OnInit } from '@angular/core';
import { FinalTask } from 'src/app/shared/final-task.model';
import { HelpersService } from 'src/app/shared/helpers.service';
import { SubmittedFinalTask } from 'src/app/shared/submitted-final-task.model';
import { FinalTasksService } from '../final-tasks.service';
import { catchError, map } from 'rxjs/operators';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-final-task',
  templateUrl: './final-task.component.html',
  styleUrls: ['./final-task.component.css']
})
export class FinalTaskComponent implements OnInit {

  @Input() courseId: number = 0;

  public finalTask: FinalTask = new FinalTask();

  public dropArea: HTMLElement | null;

  public link = "";

  public submittedFinalTask: SubmittedFinalTask | null;

  public showReviewTask = false;

  constructor(private _finalTasksService: FinalTasksService, private _helpersService: HelpersService) { }

  ngOnInit(): void {
    this._finalTasksService.getFinalTask(this.courseId).subscribe(
      response => {
        this.finalTask = response;
        this.updateSubmittedTask();
      }
    );
  }

  public updateSubmittedTask(){
    this.showReviewTask = false;
    this._finalTasksService.getSubmittedTask(this.finalTask.id).pipe(
      map((response) => {
        this.submittedFinalTask = response;
      }),
      catchError(err => {
        this.addDragNDropListeners();
        return new Observable();
      })
    ).subscribe();
  }

  public submitTask(){
    this.submittedFinalTask = new SubmittedFinalTask();
    this.submittedFinalTask.finalTaskId = this.finalTask.id;
    this.submittedFinalTask.fileLink = this.link;
    this._finalTasksService.submit(this.submittedFinalTask).subscribe();
  }

  public getReviewDeadline(){
    if (this.submittedFinalTask != null) {
      var submit = new Date(this.submittedFinalTask?.submitDateUTC);
      var deadlineTime = new Date(this.finalTask?.reviewDeadlineTime);
      var miliseconds = submit.getTime() + deadlineTime.getTime();
      var date = new Date(miliseconds);
      return date.toLocaleDateString("en-US", { hour: 'numeric', minute: 'numeric', month: 'long', day: '2-digit' });
    }
    return;
  }

  public reviewTask(){
    this.showReviewTask = true;
  }

  private addDragNDropListeners(){
    this.dropArea = document.getElementById('dropArea');
    if (this.dropArea != null) {
      ['dragenter', 'dragover', 'dragleave', 'drop'].forEach(eventName => {
        this.dropArea?.addEventListener(eventName, this.preventDefaults);
      });
      ['dragenter', 'dragover'].forEach(eventName => {
        this.dropArea?.addEventListener(eventName, this.highlight.bind(this))
      });
      ['dragleave', 'drop'].forEach(eventName => {
        this.dropArea?.addEventListener(eventName, this.unhighlight.bind(this));
      });

      this.dropArea.addEventListener('drop', this.handleDrop.bind(this));
    }
  }

  private preventDefaults(e: Event) {
    e.preventDefault();
    e.stopPropagation();
  }
  
  private highlight(e: Event) {
    if (this.dropArea != null) {
      this.dropArea.classList.add('highlight');
    }
  }
  
  private unhighlight(e: Event) {
    if (this.dropArea != null) {
      this.dropArea.classList.remove('highlight');
    }
  }

  private handleDrop(e: DragEvent) {
    let dt = e.dataTransfer;
    if (dt != null) {
      let files = dt.files;
      this._helpersService.fileToLink('thumbnails', files[0]).subscribe(
        response => { 
          this.link = (response as any).link;
        }
      );
    }
  }
}
