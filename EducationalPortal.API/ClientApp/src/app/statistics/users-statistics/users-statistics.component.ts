import { Component, OnInit } from '@angular/core';
import { UsersStatistics } from 'src/app/shared/statistics/users-statistics.model';
import { StatisticsService } from '../statistics.service';

@Component({
  selector: 'app-users-statistics',
  templateUrl: './users-statistics.component.html',
  styleUrls: ['./users-statistics.component.css']
})
export class UsersStatisticsComponent implements OnInit {

  public statistics: UsersStatistics = new UsersStatistics;

  public metadata: any;
  
  public pageSize = 10;

  constructor(private _statisticsService: StatisticsService) { }

  ngOnInit(): void {
    this.setPage(1);
  }

  public setPage(pageNumber: number){
    this._statisticsService.getUsersStatistics(this.pageSize, pageNumber).subscribe(
      response => { 
        this.statistics = response.body as UsersStatistics;
        var metadata = response.headers.get('x-pagination');
        if (metadata) {
          this.metadata = JSON.parse(metadata);
        }
      }
    )
  }
}
