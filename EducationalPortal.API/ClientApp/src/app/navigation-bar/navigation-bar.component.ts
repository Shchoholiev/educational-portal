import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../account/account.service';
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-navigation-bar',
  templateUrl: './navigation-bar.component.html',
  styleUrls: ['./navigation-bar.component.css']
})
export class NavigationBarComponent implements OnInit {

  public filter = "";

  constructor(
    public authService: AuthService, 
    private router: Router
    ) { }

  ngOnInit(): void {
  }

  public search(){
    this.router.navigate([`/courses/filtered/${this.filter}`]);
  }
}
