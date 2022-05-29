import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
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

  constructor(private _route: ActivatedRoute, private _router: Router, 
              private _accountService: AccountService) { }

  onSubmit(){
    this.user.shoppingCart = localStorage.getItem("cart") || "";
    localStorage.removeItem("cart");
    this._accountService.register(this.user);
    this._router.navigate([this.returnUrl]);
  }

  ngOnInit(): void {
    this.returnUrl = this._route.snapshot.queryParams['returnUrl'] || '/';
  }

  ngOnDestroy(): void {

  }

}
