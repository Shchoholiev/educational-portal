import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { Login } from './login/login.model';
import { Register } from './register/register.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private readonly baseURL = 'https://localhost:7106/api/account';

  constructor(private http: HttpClient, public authService: AuthService) { }

  register(form: Register){
    this.http.post(this.baseURL + '/register', form).subscribe(
      res => {
        this.authService.login(res);
      }
    );
  }

  login(form: Login){
    this.http.post(this.baseURL + '/login', form).subscribe(
      res => {
        this.authService.login(res);
      }
    );
  }

  logout(){
    // this.http.get(this.baseURL + '/logout').subscribe(); // ????
    this.authService.logout();
  }
}
  