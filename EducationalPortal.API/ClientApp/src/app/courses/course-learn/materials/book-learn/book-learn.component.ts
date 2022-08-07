import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-book-learn',
  templateUrl: './book-learn.component.html',
  styleUrls: ['./book-learn.component.css']
})
export class BookLearnComponent implements OnInit {

  @Input() link: string = "";

  constructor() { }

  ngOnInit(): void {
  }

}
