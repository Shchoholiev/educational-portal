import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Role } from '../shared/role.model';
import { AppUser } from './app-user.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private user: AppUser = new AppUser();

  constructor(private http: HttpClient) { 
    var json = localStorage.getItem('user');
    if (json != null) {
      this.user = JSON.parse(json);
    }
  }

  get name(){
    return this.user.name;
  }

  get isAuthenticated(){
    return this.user.isAuthenticated;
  }

  isInRole(role: string){
    return this.user.roles.includes(new Role(role));
  }

  login(claims: any){
    claims.forEach((claim: { type: string; value: string }) => {
      switch (claim.type) {
        case 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name':
          this.user.name = claim.value;
          // alert(claim.value);
          break;
          
        case 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress':
          this.user.email = claim.value;
          // alert(claim.value);
          break;
          
        case 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role':
          this.user.roles.push(new Role(claim.value));
          // alert(claim.value);
          break;
      
        default:
          break;
      }
    });
    this.user.isAuthenticated = true;
    localStorage.setItem('user', JSON.stringify(this.user));
  }

  logout(){
    this.user = new AppUser();
    localStorage.removeItem('user');
  }
}
