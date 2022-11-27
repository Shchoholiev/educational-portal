import { Component, OnInit } from '@angular/core';
import { AccountService } from 'src/app/account/account.service';
import { DetailedLearningUserStatistics } from 'src/app/shared/user-statistics/detailed-learning-user-statistics.model';
import { LearningUserStatistics } from 'src/app/shared/user-statistics/learning-user-statistics.model';

@Component({
  selector: 'app-user-statistics',
  templateUrl: './user-statistics.component.html',
  styleUrls: ['./user-statistics.component.css']
})
export class UserStatisticsComponent implements OnInit {

  public statistics:LearningUserStatistics[] = [];

  public details: DetailedLearningUserStatistics | null;

  public startDate: Date = new Date;

  public endDate: Date = new Date;

  public weeks: (object|null)[][] = [];

  public chosenDate: Date;

  constructor(private _accountService: AccountService) { }

  ngOnInit(): void {
    let now = new Date;

    this.startDate.setDate(this.startDate.getDate() - now.getDay() - 7);
    this.endDate.setDate(this.endDate.getDate() - now.getDay() + 14);

    this.getStatistics()
  }

  public getWeeks(){
    let timeInMilisec: number = this.endDate.getTime() - this.startDate.getTime();
    
    let daysBetweenDates: number = Math.floor(timeInMilisec / (1000 * 60 * 60 * 24));
    let array: (object|null)[][] = [];
    let weeks = Math.ceil(daysBetweenDates / 7);
    let offset = 0;
    for (let w = 0; w < weeks; w++) {
      array.push([]);
      for (let d = 0; d < 7; daysBetweenDates--, d++) {
        let date = new Date(this.startDate);
        date.setDate(this.startDate.getDate() + offset + d);
        date.setHours(0,0,0,0);
        if (daysBetweenDates > 0) {
          array[w].push({
            date: date,
            dayOfMonth: date.getDate(),
            data: this.statistics.find(s => {
              var value = (s.date as any).year == date.getUTCFullYear() 
                && (s.date as any).month == date.getUTCMonth() + 1
                && (s.date as any).day == date.getUTCDate();

              return value;
            })
          });
        } else {
          array[w].push(null);
        }
      }
      offset += 7;
    }
    return array;
  }

  private toUtc(date: Date){
    let dateObj = new Date(date)
    let utc = Date.UTC(
      dateObj.getUTCFullYear(), 
      dateObj.getUTCMonth(),
      dateObj.getUTCDate(), 
      dateObj.getUTCHours(),
      dateObj.getUTCMinutes(), 
      dateObj.getUTCSeconds());

    return new Date(utc);
  }

  public getStatistics(){
    this._accountService.getLearningStatisticsForDateRange(
      this.toUtc(this.startDate), this.toUtc(this.endDate)).subscribe(
      response => {
        this.statistics = response;
        this.weeks = this.getWeeks();
      }
    )
  }

  public getDetailsForDay(date: Date){
    this.chosenDate = date;
    this._accountService.getLearningStatiscsForDay(date).subscribe(
      response => this.details = response
    );
  }

  public getDeadlineDate(date: Date) {
    var newDate = new Date(date);
    return newDate.toLocaleDateString("en-US", { hour: 'numeric', minute: 'numeric', month: 'long', day: '2-digit' });
  }

  public getNextTwoWeeks(){
    this.startDate.setDate(this.startDate.getDate() + 14);
    this.endDate.setDate(this.endDate.getDate() + 14);
    this.getStatistics();
  }

  public getPreviousTwoWeeks(){
    this.startDate.setDate(this.startDate.getDate() - 14);
    this.endDate.setDate(this.endDate.getDate() - 14);
    this.getStatistics();
  }

  private readonly monthNames = ["January", "February", "March", "April", "May", "June",
      "July", "August", "September", "October", "November", "December"];

  public getMonthsText(){
    var months: string[] = [];
    months.push(this.monthNames[this.startDate.getMonth()]);
    var second = this.monthNames[this.endDate.getMonth()];
    if (months[0] != second) {
      months.push(second);
    }

    return months;
  }
}
