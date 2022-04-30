import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { User } from '../shared/user.model';
import { Login } from './login/login.model';
import { Register } from './register/register.model';
import { UserDTO } from './user-dto.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private readonly baseURL = 'https://localhost:7106/api/account';

  constructor(private http: HttpClient, public authService: AuthService) { }

  public getUser(){
    return this.http.get<User>(this.baseURL);
  }

  public update(user: User){
    var userDTO = new UserDTO(user.name, user.position, user.email, user.avatar);
    return this.http.put(this.baseURL, userDTO);
  }

  public register(form: Register){
    this.http.post<any>(this.baseURL + '/register', form).subscribe(
      response => {
        this.authService.login(response.token);
      }
    );
  }

  public login(form: Login){
    this.http.post<any>(this.baseURL + '/login', form).subscribe(
      response => {
        this.authService.login(response.token);
      }
    );
  }
}
  