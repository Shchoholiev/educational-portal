import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Role } from '../shared/role.model';
import { AppUser } from './app-user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private user: AppUser = new AppUser();

  constructor(private _http: HttpClient, private _jwtHelper: JwtHelperService) { }

  get name(){
    var token = localStorage.getItem("jwt");
    if (token != null) {
      var decodedToken = this._jwtHelper.decodeToken(token);
      return decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
    }
  }

  get isAuthenticated(){
    var token = localStorage.getItem("jwt");
    return token && !this._jwtHelper.isTokenExpired(token);
  }

  isInRole(role: string){
    return this.user.roles.includes(new Role(role));
  }

  login(token: string){
    localStorage.setItem('jwt', token);
  }

  logout(){
    localStorage.removeItem('jwt');
  }
}
