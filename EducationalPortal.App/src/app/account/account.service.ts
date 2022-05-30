import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '../auth/auth.service';
import { Tokens } from '../auth/tokens.model';
import { User } from '../shared/user.model';
import { UsersCourses } from '../shared/users-courses.model';
import { Login } from './login/login.model';
import { Register } from './register/register.model';
import { UserDTO } from './user-dto.model';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  private readonly baseURL = 'https://localhost:7106/api/account';

  constructor(private _http: HttpClient, public authService: AuthService) { }

  public getUser(){
    return this._http.get<User>(this.baseURL);
  }

  public getAuthor(email: string){
    return this._http.get<User>(`${this.baseURL}/author/${email}`);
  }

  public update(user: User){
    var userDTO = new UserDTO(user.name, user.position, user.email, user.avatar);
    return this._http.put(this.baseURL, userDTO);
  }

  public register(form: Register){
    this._http.post<Tokens>(this.baseURL + '/register', form).subscribe(
      response => {
        this.authService.login(response);
      }
    );
  }

  public login(form: Login){
    this._http.post<Tokens>(this.baseURL + '/login', form).subscribe(
      response => {
        this.authService.login(response);
      }
    );
  }

  public getMyLearningCourses(pageSize: number, pageNumber: number){
    return this._http.get<UsersCourses[]>(this.baseURL + '/my-learning', 
                                        { params: { 
                                            pageSize: pageSize, 
                                            pageNumber: pageNumber
                                          }, observe: 'response' });
  }

  public getCoursesInProgress(pageSize: number, pageNumber: number){
    return this._http.get<UsersCourses[]>(this.baseURL + '/courses-in-progress', 
                                        { params: { 
                                            pageSize: pageSize, 
                                            pageNumber: pageNumber
                                          }, observe: 'response' });
  }

  public getLearnedCourses(pageSize: number, pageNumber: number){
    return this._http.get<UsersCourses[]>(this.baseURL + '/learned-courses', 
                                        { params: { 
                                            pageSize: pageSize, 
                                            pageNumber: pageNumber
                                          }, observe: 'response' });
  }
}
  