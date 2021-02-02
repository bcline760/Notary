import { UrlTree, Router } from '@angular/router';
import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { TokenService } from './token.service';

@Injectable({
    providedIn: 'root'
})
export class AuthGuardService implements CanActivate {
    constructor(private tokenService:TokenService, protected router: Router) {

    }
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree {
        const expired: boolean = this.tokenService.checkExpired();

        if (expired) {
            return this.router.parseUrl('session/login');
        }

        return true;
    }
}
