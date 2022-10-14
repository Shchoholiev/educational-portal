import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { FinalTaskForReview } from 'src/app/shared/final-task-for-review.model';
import { ReviewedFinalTask } from 'src/app/shared/reviewed-final-task.model';
import { SubmittedReviewQuestion } from 'src/app/shared/submitted-review-question.model';
import { FinalTasksService } from '../final-tasks.service';

@Component({
  selector: 'app-review-final-task',
  templateUrl: './review-final-task.component.html',
  styleUrls: ['./review-final-task.component.css']
})
export class ReviewFinalTaskComponent implements OnInit {

  @Input() courseId: number = 0;

  @Output() updateSubmittedTask = new EventEmitter(true);

  public finalTaskForReview: FinalTaskForReview | null;

  public reviewedFinalTask = new ReviewedFinalTask();

  constructor(private _finalTasksService: FinalTasksService) { }

  ngOnInit(): void {
    this._finalTasksService.getSubmittedTaskForReview(this.courseId).pipe(
      map((response) => {
        this.finalTaskForReview = response;
        this.mapFinalTask();
      }),
      catchError(err => {
        return new Observable();
      })
    ).subscribe();
  }

  public getReviewedQuestion(questionId: number){
    return this.reviewedFinalTask.submittedReviewQuestions.find(s => s.questionId == questionId)!.mark;
  }

  public submitReview(){
    console.log(this.reviewedFinalTask.submittedReviewQuestions);
    this._finalTasksService.review(this.reviewedFinalTask).subscribe(
      response =>  {
        console.log("reviewed");
        
        this.updateSubmittedTask.emit()
      }
    );
  }

  private mapFinalTask(){
    this.reviewedFinalTask.submittedFinalTaskId = this.finalTaskForReview!.submittedFinalTaskId;
    this.finalTaskForReview!.reviewQuestions.forEach(q => {
      var submittedQuestion = new SubmittedReviewQuestion();
      submittedQuestion.questionId = q.id;
      this.reviewedFinalTask.submittedReviewQuestions.push(submittedQuestion);
    });
  }
}
