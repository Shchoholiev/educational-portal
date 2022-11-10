import { Component, OnInit } from '@angular/core';
import { MaterialStatistics } from 'src/app/shared/statistics/material-statistics.model';
import { StatisticsService } from '../statistics.service';

@Component({
  selector: 'app-materials-statistics',
  templateUrl: './materials-statistics.component.html',
  styleUrls: ['./materials-statistics.component.css']
})
export class MaterialsStatisticsComponent implements OnInit {

  public materials: MaterialStatistics[] = [];

  public metadata: any;
  
  public pageSize = 10;

  constructor(private _statisticsService: StatisticsService) { }

  ngOnInit(): void {
    this.setPage(1);
  }

  public setPage(pageNumber: number){
    this._statisticsService.getMaterialsStatistics(this.pageSize, pageNumber).subscribe(
      response => { 
        this.materials = response.body as MaterialStatistics[];
        var metadata = response.headers.get('x-pagination');
        if (metadata) {
          this.metadata = JSON.parse(metadata);
        }
      }
    )
  }
}
