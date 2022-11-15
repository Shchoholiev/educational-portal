import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';
import { CertificatesService } from 'src/app/certificates/certificates.service';
import { UsersCourses } from 'src/app/shared/users-courses.model';
import { MyLearningSections } from './my-learning-sections.model';

@Component({
  selector: 'app-my-learning',
  templateUrl: './my-learning.component.html',
  styleUrls: ['./my-learning.component.css']
})
export class MyLearningComponent implements OnInit {

  public usersCourses: UsersCourses[] = [];

  public metadata: any;
  
  public pageSize = 3;

  public delegate: (pageNumber: number) => void;

  public displayedSection = MyLearningSections.All;

  constructor(private _accountService: AccountService) { }

  ngOnInit(): void {
    this.setDisplaySection(MyLearningSections.All);
  }

  public getMyLearningCoursesPage(pageNumber: number){
    this._accountService.getMyLearningCourses(this.pageSize, pageNumber).subscribe(
      response => { 
        if (response.body) {
          this.usersCourses = response.body;
          var metadata = response.headers.get('x-pagination');
          if (metadata) {
            this.metadata = JSON.parse(metadata);
          }
        }
      });
  }

  public getLearnedCoursesPage(pageNumber: number){
    this._accountService.getLearnedCourses(this.pageSize, pageNumber).subscribe(
      response => { 
        if (response.body) {
          this.usersCourses = response.body;
          var metadata = response.headers.get('x-pagination');
          if (metadata) {
            this.metadata = JSON.parse(metadata);
          }
        }
      });
  }

  public getCoursesInProgressPage(pageNumber: number){
    this._accountService.getCoursesInProgress(this.pageSize, pageNumber).subscribe(
      response => { 
        if (response.body) {
          this.usersCourses = response.body;
          var metadata = response.headers.get('x-pagination');
          if (metadata) {
            this.metadata = JSON.parse(metadata);
          }
        }
      });
  }

  public setDisplaySection(section: MyLearningSections) {
    switch (section) {
      case MyLearningSections.All:
        this.displayedSection = MyLearningSections.All;
        this.getMyLearningCoursesPage(1);
        this.delegate = this.getMyLearningCoursesPage;
        break;

      case MyLearningSections.InProgress:
        this.displayedSection = MyLearningSections.InProgress;
        this.getCoursesInProgressPage(1);
        this.delegate = this.getCoursesInProgressPage;
        break;

      case MyLearningSections.Learned:
        this.displayedSection = MyLearningSections.Learned;
        this.getLearnedCoursesPage(1);
        this.delegate = this.getLearnedCoursesPage;
        break;

      case MyLearningSections.Statistics:
        this.displayedSection = MyLearningSections.Statistics;
        
        break;
    }
  }

  public getDisplayedSectionString(){
    switch (this.displayedSection) {
      case MyLearningSections.All:
        return "All"

      case MyLearningSections.InProgress:
        return "In Progress"

      case MyLearningSections.Learned:
        return "Learned"

      case MyLearningSections.Statistics:
        return "Statistics"
    }
  }
}
