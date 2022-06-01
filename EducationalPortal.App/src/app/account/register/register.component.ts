import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { catchError, map, throwError } from 'rxjs';
import { AuthService } from 'src/app/auth/auth.service';
import { AccountService } from '../account.service';
import { Register } from './register.model';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit, OnDestroy {

  public user: Register = new Register();

  private returnUrl: string = '';

  public error: string = "";

  constructor(private _route: ActivatedRoute, private _router: Router, 
              private _accountService: AccountService, private _authService: AuthService) { }

  onSubmit(){
    this.user.shoppingCart = localStorage.getItem("cart") || "";
    localStorage.removeItem("cart");
    this._accountService.register(this.user).pipe(
      map((response) => {
        this._authService.login(response);
        this._router.navigate([this.returnUrl]);
      }),
      catchError(err => {
        console.log(err);
        var error = err.error.errors;
        if (error) {
          this.error = Object.values(error).join("; ");
        }
        return throwError(() => {
          return this.error;})
    })).subscribe();
  }

  ngOnInit(): void {
    this.returnUrl = this._route.snapshot.queryParams['returnUrl'] || '/';
  }

  ngOnDestroy(): void {

  }
}
