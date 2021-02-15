import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TokenService {

  private readonly TOKEN_KEY: string = 'NOTARY_JWT';
  constructor() { }

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  setToken(token: string) {
    localStorage.setItem(this.TOKEN_KEY, token);
  }

  tokenIsValid(): boolean {
    const token: string | null = this.getToken();

    if (token) {

    }

    return false;
  }
}
