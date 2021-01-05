import { UrlTree, Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable()
export class AuthGuardService implements CanActivate {
    constructor(protected jwtService: JwtHelperService, protected router: Router) {

    }
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree {
        if (this.jwtService.isTokenExpired()) {
            return this.router.parseUrl('session/login');
        }

        const token = this.jwtService.decodeToken();

        return true;
    }
}
