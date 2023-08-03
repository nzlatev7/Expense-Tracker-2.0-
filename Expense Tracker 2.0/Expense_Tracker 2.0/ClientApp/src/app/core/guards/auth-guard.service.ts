import { Injectable } from '@angular/core';
import { UserService } from '../services/user.service';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate {

  constructor(private user: UserService,
              private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, 
              state: RouterStateSnapshot): boolean | Observable<boolean> | Promise<boolean> {
    let token = localStorage.getItem('token');
    if (token) {
      return true;
    } else {
      this.router.navigate(['/log-in']);
      return false;
    }
  }
}
