import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private _http: HttpClient, private _jwtHelper: JwtHelperService) { }

  public get name(){
    var token = localStorage.getItem("jwt");
    if (token != null) {
      var decodedToken = this._jwtHelper.decodeToken(token);
      return decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
    }
  }

  public get isAuthenticated(){
    var token = localStorage.getItem("jwt");
    return token && !this._jwtHelper.isTokenExpired(token);
  }

  public isInRole(role: string){
    var token = localStorage.getItem("jwt");
    if (token != null) {
      var decodedToken = this._jwtHelper.decodeToken(token);
      var roles: Array<string> = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      return roles.includes(role);
    }
    return false;
  }

  public login(token: string){
    localStorage.setItem('jwt', token);
  }

  public logout(){
    localStorage.removeItem('jwt');
  }
}
