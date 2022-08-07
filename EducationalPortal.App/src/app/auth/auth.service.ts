import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router, RouterStateSnapshot } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { Tokens } from './tokens.model';
import { catchError, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private _http: HttpClient, private _jwtHelper: JwtHelperService, private _router: Router) 
  { 
    if (localStorage.getItem("jwt")){
      this.refreshTokens(null).subscribe();
    }
  }

  public get name(){
    var token = localStorage.getItem("jwt");
    if (token != null) {
      var decodedToken = this._jwtHelper.decodeToken(token);
      return decodedToken['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'];
    }
  }

  public get isAuthenticated(){
    var token = localStorage.getItem("jwt");
    return token != null && !this._jwtHelper.isTokenExpired(token);
  }

  public isInRole(role: string){
    var token = localStorage.getItem("jwt");
    if (token != null) {
      var decodedToken = this._jwtHelper.decodeToken(token);
      var roles: Array<string> = decodedToken['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'];
      if (roles) {
        return roles.includes(role);
      }
    }
    return false;
  }

  public login(token: Tokens){
    localStorage.setItem('jwt', token.accessToken);
    localStorage.setItem('refreshToken', token.refreshToken);
    this.startTokenTimer();
  }

  public logout(){
    localStorage.removeItem('jwt');
    localStorage.removeItem('refreshToken');
    clearTimeout(this.refreshTokensTimeout);
  }

  public refreshTokens(state: RouterStateSnapshot | null): Observable<boolean> {
    var accessToken = localStorage.getItem("jwt");
    var refreshToken = localStorage.getItem("refreshToken");
    if (!accessToken || !refreshToken) { 
      this._router.navigate(['/account/login'], { queryParams: { returnUrl: state?.url ?? "" }});
      return of(false);
    }

    var tokens = new Tokens(accessToken, refreshToken);
    return this._http.post("https://localhost:7106/api/tokens/refresh", tokens, { observe: 'response' }).pipe(
      map((response) => {
        this.login((<any>response).body);
        return true;
      }),
      catchError(err => {
        return this._router.navigate(['/account/login'], { queryParams: { returnUrl: state?.url ?? "" }});
      })
    );
  }

  private refreshTokensTimeout: any;

  private startTokenTimer(){
    var token = localStorage.getItem("jwt");
    if (token) {
      var decoded = this._jwtHelper.decodeToken(token);
      var expires = new Date(decoded.exp * 1000);
      var timeout = expires.getTime() - Date.now();
      this.refreshTokensTimeout = setTimeout(() => this.refreshTokens(null).subscribe(), timeout);
    }
  }

}
