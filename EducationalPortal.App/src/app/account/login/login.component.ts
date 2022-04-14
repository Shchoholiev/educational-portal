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

  constructor(private route: ActivatedRoute, private router: Router, 
              private accountService: AccountService) { }

  onSubmit(){
    alert(this.user.email);
    this.accountService.login(this.user);
    this.router.navigate([this.returnUrl]);
  }

  ngOnInit(): void {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

}
