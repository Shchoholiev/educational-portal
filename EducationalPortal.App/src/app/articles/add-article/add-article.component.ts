import { Component, Input, OnInit } from '@angular/core';
import { catchError, map, throwError } from 'rxjs';
import { Article } from 'src/app/shared/article.model';
import { Course } from 'src/app/shared/course.model';
import { ArticlesService } from '../articles.service';

@Component({
  selector: 'app-add-article',
  templateUrl: './add-article.component.html',
  styleUrls: ['./add-article.component.css']
})
export class AddArticleComponent implements OnInit {

  @Input() course: Course = new Course();

  public articles: Article[] = [];

  public metadata: any;
  
  public pageSize = 4;

  public article: Article = new Article();

  public createErrors: string[] = [];

  constructor(private _articlesService: ArticlesService) { }

  ngOnInit(): void {
    this.setPage(1);
  }

  public addArticleToCourse(id: number){
    if (this.materialExistsInCourse(id)) {
      var index = this.course.materials.findIndex(s => s.id == id);
      this.course.materials.splice(index, 1);
    }
    else{
      var article = this.articles.find(a => a.id == id);
      if (article) {
        this.course.materials.push(article);
      }
    }
  }

  public setPage(pageNumber: number){
    this._articlesService.getPage(this.pageSize, pageNumber).subscribe(
      response => { 
        this.articles = response.body as Article[];
        console.log(this.articles);
        
        var metadata = response.headers.get('x-pagination');
        if (metadata) {
          this.metadata = JSON.parse(metadata);
        }
      }
    )
  }

  public delete(id: number){
    this._articlesService.delete(id).subscribe(
      () => {
        if (this.metadata) {
          this.setPage(this.metadata.PageNumber);
        }
      }
    );
  }

  public create(){
    this.createErrors = [];
    this._articlesService.create(this.article).pipe(
      map(() => {
        if (this.metadata) {
          this.setPage(this.metadata.PageNumber);
        }
        this.article = new Article();
      }),
      catchError(err => {
        this.createErrors = Object.values(err.error);
        return throwError(() => {
          return this.createErrors;})
    })
    ).subscribe()
  }

  public materialExistsInCourse(id: number){
    return this.course.materials.find(m => m.id == id);
  }

  public getDate(article: Article){
    var date = new Date(article.publicationDate);
    return date.toLocaleDateString();
  }
}
