import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/shared/user.model';
import { AccountService } from '../account.service';

@Component({
  selector: 'app-author',
  templateUrl: './author.component.html',
  styleUrls: ['./author.component.css']
})
export class AuthorComponent implements OnInit {

  public user: User = new User;

  constructor(private _route: ActivatedRoute, private _accountService: AccountService) { }

  ngOnInit(): void {
    var email = this._route.snapshot.paramMap.get('email') ?? "";
    this._accountService.getAuthor(email).subscribe(
      response => this.user = response
    )
  }

}
