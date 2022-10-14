import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { FinalTaskForReview } from '../shared/final-task-for-review.model';
import { FinalTask } from '../shared/final-task.model';
import { ReviewedFinalTask } from '../shared/reviewed-final-task.model';
import { SubmittedFinalTask } from '../shared/submitted-final-task.model';

@Injectable({
  providedIn: 'root'
})
export class FinalTasksService {

  private readonly baseURL = '/api/final-tasks';

  constructor(private _http: HttpClient) { }

  public getFinalTask(courseId: number){
    return this._http.get<FinalTask>(`${this.baseURL}/${courseId}`);
  }

  public submit(submittedTask: SubmittedFinalTask){
    return this._http.post(`${this.baseURL}/submit`, submittedTask);
  }

  public review(reviewedTask: ReviewedFinalTask){
    return this._http.post(`${this.baseURL}/review`, reviewedTask);
  }

  public getSubmittedTask(finalTaskId: number){
    return this._http.get<SubmittedFinalTask>(`${this.baseURL}/submitted/${finalTaskId}`);
  }

  public getSubmittedTaskForReview(courseId: number){
    return this._http.get<FinalTaskForReview>(`${this.baseURL}/for-review/${courseId}`);
  }
}
