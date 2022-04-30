import { Component, OnInit } from '@angular/core';
import { CoursesService } from 'src/app/courses/courses.service';
import { HelpersService } from 'src/app/shared/helpers.service';
import { User } from 'src/app/shared/user.model';
import { AccountService } from '../account.service';
import { catchError, of } from 'rxjs';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  public user: User = new User;

  public file: File;

  public error: string = "";

  constructor(private _accountService: AccountService, private _helpersService: HelpersService,
              private _coursesService: CoursesService) { }

  ngOnInit(): void {
    this.refresh();
  }

  public refresh(){
    this._accountService.getUser().subscribe(
      data => this.user = data
    );
  }

  public deleteCourse(id: number){
    this._coursesService.delete(id).subscribe(
      () => this.refresh()
    );
  }

  public update(){
    this._accountService.update(this.user).pipe(
      catchError(error => {
        this.error = error.error;
        this.refresh();
        return of(error);
      })
    ).subscribe(
      () => this.error = ""
    );
  }

  public fileToLink(event: Event){
    var input = event.target as HTMLInputElement;
    if (input.files) {
      this._helpersService.fileToLink('avatars', input?.files[0]).subscribe(
        response => this.user.avatar = (<any>response).link
      );
    }
  }
}
