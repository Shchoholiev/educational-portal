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

  constructor(private route: ActivatedRoute, private router: Router, 
              private accountService: AccountService) { }

  onSubmit(){
    this.accountService.register(this.user);
    this.router.navigate([this.returnUrl]);
  }

  ngOnInit(): void {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  }

  ngOnDestroy(): void {

  }

}
