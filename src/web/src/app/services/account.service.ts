import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { from, Observable } from 'rxjs';
import { first, map } from 'rxjs/operators';

import { Account } from '../contracts/account';
import { AuthenticatedUser } from '../contracts/authenticated-user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private httpClient: HttpClient) { }

  /**
   * Get an account
   * @param slug The slug of the account
   * @returns An observable of the account or null if not found
   */
  public get(slug: string): Observable<Account> | null {
    const url: string = `${environment.apiUrl}/account/${slug}`;

    try {
      const account: Observable<Account> = this.httpClient.get<Account>(url);

      return account;
    } catch (e) {
      console.log(e);
      return null;
    }
  }

  /**
   * Get all accounts. Warning! This can be an expensive operation. Use with caution!
   * @returns An observable list of account objects
   */
  public getAll(): Observable<Account[]> {
    const url: string = `${environment.apiUrl}/account`;
    try {
      const accounts: Observable<Account[]> = this.httpClient.get<Account[]>(url)

      return accounts;
    } catch (e) {
      console.log(e);
      return from([]);
    }
  }

  /**
   * Get an account by the holder's e-mail.
   * @param email The e-mail of the account holder
   * @returns The account observable or null if not found
   */
  public getByEmail(email: string): Observable<Account> | null {
    const url: string = `${environment.apiUrl}/account?email=${encodeURI(email)}`;
    try {
      const account: Observable<Account> = this.httpClient.get<Account>(url);

      return account;
    } catch (e) {
      console.log(e);
      return null;
    }
  }

  /**
   * Get currently active sessions
   * @param slug The slug of the account
   * @param onlyActive Boolean value to get only active sessions
   * @returns An observable list of authenticated users or null if none found
   */
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

  /**
   * Delete an account
   * @param slug The slug of the account to delete
   */
  public deleteBySlug(slug: string): void {
    this.get(slug)?.pipe(first()).subscribe(a => {
      if (a) {
        a.active = false;
        a.updated = new Date();

        this.save(a);
      }
    });
  }

  /**
   * Delete an account
   * @param account The account object to delete
   */
  public delete(account: Account): void {
    this.deleteBySlug(account.slug);
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
      console.log(e);
    }
  }
}