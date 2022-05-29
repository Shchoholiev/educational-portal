import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';
import { UsersCourses } from 'src/app/shared/users-courses.model';

@Component({
  selector: 'app-my-learning',
  templateUrl: './my-learning.component.html',
  styleUrls: ['./my-learning.component.css']
})
export class MyLearningComponent implements OnInit {

  public usersCourses: UsersCourses[] = [];

  public metadata: string | null;
  
  public pageSize = 3;

  public delegate: (pageNumber: number) => void;

  constructor(private _accountService: AccountService) { }

  ngOnInit(): void {
    this.navToMyLearning();
  }

  public getMyLearningCoursesPage(pageNumber: number){
    this._accountService.getMyLearningCourses(this.pageSize, pageNumber).subscribe(
      response => { 
        if (response.body) {
          this.usersCourses = response.body;
          this.metadata = response.headers.get('x-pagination');
        }
      });
  }

  public getLearnedCoursesPage(pageNumber: number){
    this._accountService.getLearnedCourses(this.pageSize, pageNumber).subscribe(
      response => { 
        if (response.body) {
          this.usersCourses = response.body;
          this.metadata = response.headers.get('x-pagination');
        }
      });
  }

  public getCoursesInProgressPage(pageNumber: number){
    this._accountService.getCoursesInProgress(this.pageSize, pageNumber).subscribe(
      response => { 
        if (response.body) {
          this.usersCourses = response.body;
          this.metadata = response.headers.get('x-pagination');
        }
      });
  }

  public navToMyLearning(){
    this.getMyLearningCoursesPage(1);
    this.delegate = this.getMyLearningCoursesPage;
  }

  public navToLearnedCourses(){
    this.getLearnedCoursesPage(1);
    this.delegate = this.getLearnedCoursesPage;
  }

  public navToCoursesInProgress(){
    this.getCoursesInProgressPage(1);
    this.delegate = this.getCoursesInProgressPage;
  }
}
