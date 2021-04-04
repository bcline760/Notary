import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { from, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { log } from 'console';

import { Account } from 'src/contract/account.contract';
import { AuthenticatedUser } from 'src/contract/authenticated-user.contract';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private httpClient: HttpClient) { }

  public get(slug: string): Observable<Account> | null {
    const url: string = `${environment.apiUrl}/account/${slug}`;

    try {
      const account: Observable<Account> = this.httpClient.get<Account>(url);

      return account;
    } catch (e) {
      log(e);
      return null;
    }
  }

  public getAll(): Observable<Account[]> {
    const url: string = `${environment.apiUrl}/account`;
    try {
      const accounts: Observable<Account[]> = this.httpClient.get<Account[]>(url)

      return accounts;
    } catch (e) {
      log(e);
      return from([]);
    }
  }

  public getByEmail(email: string): Observable<Account> | null {
    const url: string = `${environment.apiUrl}/account?email=${encodeURI(email)}`;
    try {
      const account: Observable<Account> = this.httpClient.get<Account>(url);

      return account;
    } catch (e) {
      log(e);
      return null;
    }
  }

  public getSessions(slug: string, onlyActive: boolean): Observable<AuthenticatedUser[]> | null {
    const url = `${environment.apiUrl}/account/${slug}/sessions/${onlyActive}`;

    try {
      const sessions: Observable<AuthenticatedUser[]> = this.httpClient.get<AuthenticatedUser[]>(url).pipe(map(c => {
        return c;
      }));

      return sessions;
    } catch (e) {
      return null;
    }
  }

  public save(account: Account): void {
    let url: string = '';
    let method: 'PUT' | 'POST';

    if (account.slug) {
      url = `${environment.apiUrl}/account/${account.slug}`;
      method = "PUT";
    } else {
      url = `${environment.apiUrl}/account`;
      method = "POST";
    }

    try {
      switch (method) {
        case "POST":
          this.httpClient.post<Account>(url, account);
          break;
        case "PUT":
          this.httpClient.put(url, account);
          break;
      }
    } catch (e) {
      log(e);
    }
  }
}
