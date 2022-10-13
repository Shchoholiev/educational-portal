import { Component, Input, OnInit, Output, EventEmitter } from '@angular/core';
import { CoursesService } from 'src/app/courses/courses.service';
import { Book } from 'src/app/shared/book.model';

@Component({
  selector: 'app-book-side',
  templateUrl: './book-side.component.html',
  styleUrls: ['./book-side.component.css']
})
export class BookSideComponent implements OnInit {

  @Input() book: Book = new Book();

  @Input() courseId: number = 0;

  @Input() isChosen = false;

  @Output() changeProgress = new EventEmitter<any>(true);

  constructor(private _coursesService: CoursesService) { }

  ngOnInit(): void {
  }

  public learned(){
    this._coursesService.learned(this.book.id, this.courseId).subscribe(
      response => {
        this.changeProgress.emit(response);
        this.book.isLearned = true;
      }
    )
  }

  public unlearned(){
    this._coursesService.unlearned(this.book.id, this.courseId).subscribe(
      response => {
        this.changeProgress.emit(response);
        this.book.isLearned = false;
      }
    )
  }

  public click(){
    if (this.book.isLearned) {
      this.unlearned();
    }
    else {
      this.learned();
    }
  }

}
