import { Component, OnInit } from '@angular/core';
import { SalesStatistics } from 'src/app/shared/statistics/sales-statistics.model';
import { StatisticsService } from '../statistics.service';

@Component({
  selector: 'app-sales-statistics',
  templateUrl: './sales-statistics.component.html',
  styleUrls: ['./sales-statistics.component.css']
})
export class SalesStatisticsComponent implements OnInit {

  public sales: SalesStatistics = new SalesStatistics;

  constructor(private _statisticsService: StatisticsService) { }

  ngOnInit(): void {
    this._statisticsService.getSalesStatistics().subscribe(
      response => { 
        this.sales = response;
      }
    );
  }
}
