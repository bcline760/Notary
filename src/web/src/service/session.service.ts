import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { AuthenticatedUser } from 'src/contract/authenticated-user.contract';
import { Credentials } from 'src/contract/credentials.contract';
import { Role } from 'src/contract/role.enum';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class SessionService {

  constructor(private httpClient: HttpClient) {
    // this.tokenSubject = new BehaviorSubject<AuthenticatedUser | null>(null);
  }

  /**
   * Get the current JWT or null if not signed in
   */
  public get currentAuthenticatedUser(): AuthenticatedUser | null {
    // return this.tokenSubject.value;
    if (sessionStorage.length > 0) {
      const json: string | null = sessionStorage.getItem("auth_user");
      if (json) {
        return JSON.parse(json) as AuthenticatedUser;
      }
    }

    return null;
  }

  /**
   * Perform user sign in
   * 
   * @param credentials - The credentials to sign in
   * @returns an observable JWT from the API
   */
  public signIn(credentials: Credentials): Observable<AuthenticatedUser> {
    const url: string = `${environment.apiUrl}/session/signin`;
    const result: Observable<AuthenticatedUser> = this.httpClient.post<AuthenticatedUser>(url, credentials).pipe(
      map(token => {
        sessionStorage.setItem("auth_user", JSON.stringify(token));
        return token;
      })
    );

    return result;
  }

  /**
   * Perform user signout and destroy current JWT
   */
  public signOut() {
    sessionStorage.clear();
    const url: string = `${environment.apiUrl}/session/signout`;

    if (this.currentAuthenticatedUser) {
      this.httpClient.post(url, this.currentAuthenticatedUser)
    }
  }

  /**
   * Check to see if the current session is still valid
   * 
   * @returns Boolean value to indicate session validity
   */
  public isValidSession(): boolean {
    const token: AuthenticatedUser | null = this.currentAuthenticatedUser;

    if (token) {
      const now: Date = new Date();
      return new Date(token.expiry) > now;
    }

    return false;
  }

  public isInRole(role: Role): boolean {
    const token: AuthenticatedUser | null = this.currentAuthenticatedUser;

    if (token) {
      return token.role === role;
    }

    return false;
  }
}
