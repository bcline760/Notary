import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class TokenService {
    private _storageKey: string = 'access_token';

    constructor(private jwtService:JwtHelperService) { }

    checkExpired(): boolean {
        const token: string | null = this.getToken();
        if (token) {
            return this.jwtService.isTokenExpired(token);
        }

        return true;
    }

    getToken(): string | null {
        const token: string | null = localStorage.getItem(this._storageKey);

        return token;
    }

    setToken(token: string): void {
        if (!token) {
            throw new Error('Missing token');
        }

        localStorage.setItem(this._storageKey, token);
    }

    removeToken(): void {
        localStorage.removeItem(this._storageKey);
    }
}
