import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent implements OnInit {

  @Input() metadata: string | null;
  @Output() changePageFunction = new EventEmitter<any>(true);

  public pageNumber: number;
  public totalItems: number;
  public totalPages: number;
  public hasNextPage: boolean;
  public hasPreviousPage: boolean;

  constructor() { }

  setPage(number: number){
    this.changePageFunction.emit(number);
    this.pageNumber = number;
    this.hasNextPage = number < this.totalPages;
    this.hasPreviousPage = number > 1;
  }

  forLoop(){
    return new Array(this.totalPages);
  }

  ngOnInit(): void {
    if (this.metadata != null) {
      var object = JSON.parse(this.metadata);
      this.totalItems = Number(object.TotalItems);
      this.totalPages = Number(object.TotalPages);
      this.pageNumber = Number(object.PageNumber);
      this.hasNextPage = Boolean(object.HasNextPage);
      this.hasPreviousPage = Boolean(object.HasPreviousPage);
    }
  }

}
