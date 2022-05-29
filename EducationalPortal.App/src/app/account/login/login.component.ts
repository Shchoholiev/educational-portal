import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from '../account.service';
import { Login } from './login.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  public user: Login = new Login();

  private returnUrl: string = '';

  constructor(private _route: ActivatedRoute, private _router: Router, 
              private _accountService: AccountService) { }

  onSubmit(){
    this.user.shoppingCart = localStorage.getItem("cart") || "";
    localStorage.removeItem("cart");
    this._accountService.login(this.user);
    this._router.navigate([this.returnUrl]);
  }

  ngOnInit(): void {
    this.returnUrl = this._route.snapshot.queryParams['returnUrl'] || '/';
  }

}
