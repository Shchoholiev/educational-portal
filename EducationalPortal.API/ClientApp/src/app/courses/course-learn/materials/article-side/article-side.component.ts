import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { CoursesService } from 'src/app/courses/courses.service';
import { Article } from 'src/app/shared/article.model';

@Component({
  selector: 'app-article-side',
  templateUrl: './article-side.component.html',
  styleUrls: ['./article-side.component.css']
})
export class ArticleSideComponent implements OnInit {

  @Input() article: Article = new Article();

  @Input() courseId: number = 0;

  @Output() changeProgress = new EventEmitter<any>(true);

  constructor(private _coursesService: CoursesService) { }

  ngOnInit(): void {
  }

  public getDate(){
    var date = new Date(this.article.publicationDate);
    return date.toLocaleDateString();
  }

  public learned(){
    this._coursesService.learned(this.article.id, this.courseId).subscribe(
      response => {
        this.changeProgress.emit(response);
        this.article.isLearned = true;
      }
    )
  }

  public unlearned(){
    this._coursesService.unlearned(this.article.id, this.courseId).subscribe(
      response => {
        this.changeProgress.emit(response);
        this.article.isLearned = false;
      }
    )
  }

  public click(){
    if (this.article.isLearned) {
      this.unlearned();
    }
    else {
      this.learned();
    }
  }

}
