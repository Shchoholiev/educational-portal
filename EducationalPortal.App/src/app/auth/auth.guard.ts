import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router){}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot){
    // if (this.authService.isAuthenticated) {
      return true;
    // }

    // alert('dg');
    // return this.router.navigate(['/account/register'], { queryParams: { returnUrl: state.url }});
  }
  
}
