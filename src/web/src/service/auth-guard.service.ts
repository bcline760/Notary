import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, throwError } from 'rxjs';
import { AuthenticatedUser } from 'src/contract/authenticated-user.contract';
import { Role } from 'src/contract/role.enum';
import { SessionService } from './session.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuardService implements CanActivate {

  constructor(private sessionService: SessionService, private router: Router) { }
  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
    const validSession: boolean = this.sessionService.isValidSession();

    if (validSession) {

      //Check to see if the route has roles defined
      if (route.data.roles) {
        const roles: Role[] = route.data.roles;

        if (this.sessionService.currentAuthenticatedUser &&
          roles.indexOf(this.sessionService.currentAuthenticatedUser.role) === -1) {
          return this.router.navigate(['/']);
        }
      }
      // route has no roles, but user is authenticated, allow them to pass
      return true;
    }
    return this.router.parseUrl('/session/signin');
  }
}
