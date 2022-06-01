import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { catchError, map, throwError } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';
import { Tokens } from 'src/app/auth/tokens.model';
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

  public error: string = "";

  constructor(private _route: ActivatedRoute, private _router: Router, 
              private _accountService: AccountService, private _authService: AuthService) { }

  onSubmit(){
    this.user.shoppingCart = localStorage.getItem("cart") || "";
    localStorage.removeItem("cart");
    this._accountService.login(this.user).pipe(
      map((response) => {
        this._authService.login(response);
        this._router.navigate([this.returnUrl]);
      }),
      catchError(err => {
        console.log(err);
        this.error = Object.values(err.error.errors).join("; ");
        return throwError(() => {
          return this.error;})
    })).subscribe();
  }

  ngOnInit(): void {
    this.returnUrl = this._route.snapshot.queryParams['returnUrl'] || '/';
  }

}
