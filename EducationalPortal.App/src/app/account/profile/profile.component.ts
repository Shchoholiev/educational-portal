import { Component, OnInit } from '@angular/core';
import { CoursesService } from 'src/app/courses/courses.service';
import { User } from 'src/app/shared/user.model';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {

  public user: User;

  constructor(private accountService: AccountService) { }

  ngOnInit(): void {
    this.accountService.getUser().subscribe(
      data => this.user = data
    )
  }

}
