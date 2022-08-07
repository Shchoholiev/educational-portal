import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-video-learn',
  templateUrl: './video-learn.component.html',
  styleUrls: ['./video-learn.component.css']
})
export class VideoLearnComponent implements OnInit {

  @Input() link: string = "";

  constructor() { }

  ngOnInit(): void {
  }

}
