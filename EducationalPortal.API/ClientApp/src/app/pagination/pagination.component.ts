import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent implements OnInit {

  @Input() metadata: any;

  @Output() changePageFunction = new EventEmitter<any>(true);

  constructor() { }

  setPage(number: number){
    this.changePageFunction.emit(number);
  }

  forLoop(){
    return new Array(this.metadata.TotalPages);
  }

  ngOnInit(): void {
  }

}
