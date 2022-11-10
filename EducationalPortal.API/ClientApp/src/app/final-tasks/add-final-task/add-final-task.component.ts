import { Component, Input, OnInit } from '@angular/core';
import { catchError, map, throwError } from 'rxjs';
import { Course } from 'src/app/shared/course.model';
import { FinalTask } from 'src/app/shared/final-task.model';
import { ReviewQuestion } from 'src/app/shared/review-question.model';
import { FinalTasksService } from '../final-tasks.service';

@Component({
  selector: 'app-add-final-task',
  templateUrl: './add-final-task.component.html',
  styleUrls: ['./add-final-task.component.css']
})
export class AddFinalTaskComponent implements OnInit {

  @Input() course: Course = new Course();

  public finalTasks: FinalTask[] = [];

  public metadata: any;
  
  public pageSize = 3;

  public finalTask: FinalTask = new FinalTask();

  public createErrors: string[] = [];

  public deleteError: string;

  constructor(private _service: FinalTasksService) { }

  ngOnInit(): void {
    this.setPage(1);
  }

  public chooseFinalTask(id: number){
    var finalTask = this.finalTasks.find(a => a.id == id);
    if (finalTask) {
      this.course.finalTask = finalTask;
    }
  }

  public setPage(pageNumber: number){
    this.deleteError = "";
    this._service.getPage(this.pageSize, pageNumber).subscribe(
      response => { 
        this.finalTasks = response.body as FinalTask[];
        var metadata = response.headers.get('x-pagination');
        if (metadata) {
          this.metadata = JSON.parse(metadata);
        }
      }
    )
  }

  public delete(id: number){
    this.deleteError = "";
    this._service.delete(id).pipe(
      map(() => {
        if (this.metadata) {
          this.setPage(this.metadata.PageNumber);
        }
      }),
      catchError(err => {
        this.deleteError = err.error;
        return throwError(() => {
          return this.deleteError;})
    })
    ).subscribe()
  }

  public create(){
    this.createErrors = [];
    this._service.create(this.finalTask).pipe(
      map(() => {
        if (this.metadata) {
          this.setPage(this.metadata.PageNumber);
        }
        this.finalTask = new FinalTask();
      }),
      catchError(err => {
        this.createErrors = Object.values(err.error);
        return throwError(() => {
          return this.createErrors;})
    })
    ).subscribe()
  }

  public getDate(finalTask: FinalTask){
    var date = new Date(finalTask.reviewDeadlineTime);
    return date.toLocaleDateString("en-US", { month: '2-digit', day: '2-digit', hour: "2-digit"});
  }

  public addReviewQuestion() {
    this.finalTask.reviewQuestions.push(new ReviewQuestion());
  }
}
