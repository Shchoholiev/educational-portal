import { Component, Input, OnInit } from '@angular/core';
import { catchError, map, throwError } from 'rxjs';
import { Article } from 'src/app/shared/article.model';
import { Resource } from 'src/app/shared/resource.model';
import { ResourcesService } from '../resources.service';

@Component({
  selector: 'app-add-resource',
  templateUrl: './add-resource.component.html',
  styleUrls: ['./add-resource.component.css']
})
export class AddResourceComponent implements OnInit {

  @Input() article: Article = new Article();

  public resources: Resource[] = [];

  public metadata: any;
  
  public pageSize = 3;

  public resource: Resource = new Resource();

  public createErrors: string[] = [];

  public deleteError: string;

  constructor(private _resourcesService: ResourcesService) { }

  ngOnInit(): void {
    this.setPage(1);
  }

  public chooseResource(resource: Resource){
    this.article.resource = resource;
  }

  public setPage(pageNumber: number){
    this.deleteError = "";
    this._resourcesService.getPage(this.pageSize, pageNumber).subscribe(
      response => { 
        this.resources = response.body as Article[];
        var metadata = response.headers.get('x-pagination');
        if (metadata) {
          this.metadata = JSON.parse(metadata);
        }
      }
    )
  }

  public delete(id: number){
    this.deleteError = "";
    this._resourcesService.delete(id).pipe(
      map(() => {
        if (this.metadata) {
          this.setPage(this.metadata.PageNumber);
        }
      }),
      catchError(err => {
        this.deleteError = err.error;
        return throwError(() => {
          return this.deleteError;})
    })
    ).subscribe()
  }

  public create(){
    this.createErrors = [];
    this._resourcesService.create(this.resource).pipe(
      map(() => {
        if (this.metadata) {
          this.setPage(this.metadata.PageNumber);
        }
        this.resource = new Resource();
      }),
      catchError(err => {
        this.createErrors = Object.values(err.error);
        return throwError(() => {
          return this.createErrors;})
    })
    ).subscribe()
  }

  public resourceExistsInArticle(id: number){
    return this.article?.resource?.id == id;
  }
}
